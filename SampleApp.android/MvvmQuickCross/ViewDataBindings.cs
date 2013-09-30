using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Reflection;
using System.Text.RegularExpressions;

using Android.Views;
using Android.Widget;
using System.ComponentModel;

namespace MvvmQuickCross
{
    public enum BindingMode { OneWay, TwoWay, Command };

    public class BindingParameters
    {
        public string propertyName;
        public string listPropertyName;
        public BindingMode mode = BindingMode.OneWay;
        public View view;
    }

    public partial class ViewDataBindings
    {
        private class DataBinding
        {
            public BindingMode Mode;
            public View View;
            public PropertyInfo ViewModelPropertyInfo;
            public int? ResourceId;

            public PropertyInfo ViewModelListPropertyInfo;
            public IDataBindableListAdapter ListAdapter;
        }

        private readonly View rootView;
        private readonly ViewExtensionPoints rootViewExtensionPoints;
        private ViewModelBase viewModel;
        private readonly LayoutInflater layoutInflater;
        private readonly string idPrefix;

        private Dictionary<string, DataBinding> dataBindings = new Dictionary<string, DataBinding>();

        public interface ViewExtensionPoints  // Implement these methods as virtual in a view base class
        {
            void UpdateView(View view, object value);
            void OnCollectionChanged(object sender, NotifyCollectionChangedEventArgs e);
        }

        public ViewDataBindings(View rootView, ViewModelBase viewModel, LayoutInflater layoutInflater, string idPrefix)
        {
            if (rootView == null) throw new ArgumentNullException("rootView");
            if (viewModel == null) throw new ArgumentNullException("viewModel");
            if (layoutInflater == null) throw new ArgumentNullException("layoutInflater");
            this.rootView = rootView;
            this.rootViewExtensionPoints = rootView as ViewExtensionPoints;
            this.viewModel = viewModel;
            this.layoutInflater = layoutInflater;
            this.idPrefix = idPrefix;
        }

        public void SetViewModel(ViewModelBase newViewModel)
        {
            if (Object.ReferenceEquals(viewModel, newViewModel)) return;
            RemoveHandlers();
            viewModel = newViewModel;
            AddHandlers();
            UpdateView(findViews: false);
        }

        public void EnsureCommandBindings()
        {
            foreach (string commandName in viewModel.CommandNames)
            {
                DataBinding binding;
                if (!dataBindings.TryGetValue(IdName(commandName), out binding))
                {
                    AddBinding(commandName, BindingMode.Command);
                }
            }
        }

        public void UpdateView(bool findViews = true)
        {
            foreach (var item in dataBindings)
            {
                var binding = item.Value;
                if (binding.ResourceId.HasValue && findViews)
                {
                    binding.View = rootView.FindViewById(binding.ResourceId.Value);
                }

                UpdateList(binding);
                UpdateView(binding);
            }
        }

        public void UpdateView(string propertyName)
        {
            DataBinding binding;
            if (dataBindings.TryGetValue(IdName(propertyName), out binding))
            {
                UpdateList(binding);
                UpdateView(binding);
                return;
            }

            binding = FindBindingForListProperty(propertyName);
            if (binding != null)
            {
                UpdateList(binding);
                return;
            }

            binding = AddBinding(propertyName);
            if (binding != null)
            {
                UpdateList(binding);
                UpdateView(binding);
            }
        }

        public void RemoveHandlers()
        {
            foreach (var item in dataBindings)
            {
                var binding = item.Value;
                RemoveListHandlers(binding);
                switch (binding.Mode)
                {
                    case BindingMode.TwoWay: RemoveTwoWayHandler(binding); break;
                    case BindingMode.Command: RemoveCommandHandler(binding); break;
                }
            }
        }

        public void AddHandlers()
        {
            foreach (var item in dataBindings)
            {
                var binding = item.Value;
                AddListHandlers(binding);
                switch (binding.Mode)
                {
                    case BindingMode.TwoWay: AddTwoWayHandler(binding); break;
                    case BindingMode.Command: AddCommandHandler(binding); break;
                }
            }
        }

        private void RemoveListHandlers(DataBinding binding)
        {
            if (binding != null && binding.ListAdapter != null) binding.ListAdapter.RemoveHandlers();
        }

        private void AddListHandlers(DataBinding binding)
        {
            if (binding != null && binding.ListAdapter != null) binding.ListAdapter.AddHandlers();
        }

        public void AddBindings(BindingParameters[] bindingsParameters)
        {
            if (bindingsParameters != null)
            {
                foreach (var bp in bindingsParameters)
                {
                    if (bp.view != null && FindBindingForView(bp.view) != null) throw new ArgumentException("Cannot add binding because a binding already exists for the view with Id " + bp.view.Id.ToString());
                    if (dataBindings.ContainsKey(IdName(bp.propertyName))) throw new ArgumentException("Cannot add binding because a binding already exists for the view with Id " + IdName(bp.propertyName));
                    AddBinding(bp.propertyName, bp.mode, bp.listPropertyName, bp.view);
                }
            }
        }

        private string IdName(string name) { return idPrefix + name; }

        private DataBinding AddBinding(string propertyName, BindingMode mode = BindingMode.OneWay, string listPropertyName = null, View view = null)
        {
            string idName = (view != null) ? view.Id.ToString() : IdName(propertyName);
            int? resourceId = AndroidHelpers.FindResourceId(idName);
            if (view == null && resourceId.HasValue) view = rootView.FindViewById(resourceId.Value);
            if (view == null) return null;

            bool itemIsValue = false;
            string itemTemplateName = null, itemValueId = null;
            if (view.Tag != null)
            {
                // Get optional parameters from tag:
                // {Binding propertyName, Mode=OneWay|TwoWay|Command} {List ItemsSource=listPropertyName, ItemIsValue=false|true, ItemTemplate=listItemTemplateName, ItemValueId=listItemValueId}
                // Defaults:
                //   propertyName is known by convention from view Id = <rootview prefix><propertyName>; the default for the rootview prefix is the rootview class name + "_".
                //   Mode = OneWay
                // Additional defaults for views derived from AdapterView (i.e. lists):
                //   ItemsSource = propertyName + "List"
                //   ItemIsValue = false
                //   ItemTemplate = ItemsSource + "Item"
                //   ItemValueId : if ItemIsValue = true then the default for ItemValueId = ItemTemplate
                string tag = view.Tag.ToString();
                if (tag != null && tag.Contains("{Binding"))
                {
                    var match = Regex.Match(tag, @"{Binding\s+((?<assignment>[^,{}]+),?)+\s*}(\s*{List\s+((?<assignment>[^,{}]+),?)+\s*})?");
                    if (match.Success)
                    {
                        var gc = match.Groups["assignment"];
                        if (gc != null)
                        {
                            var cc = gc.Captures;
                            if (cc != null)
                            {
                                for (int i = 0; i < cc.Count; i++)
                                {
                                    string[] assignmentElements = cc[i].Value.Split('=');
                                    if (assignmentElements.Length == 1)
                                    {
                                        propertyName = assignmentElements[0].Trim();
                                    }
                                    else if (assignmentElements.Length == 2)
                                    {
                                        string name = assignmentElements[0].Trim();
                                        string value = assignmentElements[1].Trim();
                                        switch (name)
                                        {
                                            case "Mode": Enum.TryParse<BindingMode>(value, true, out mode); break;
                                            case "ItemsSource": listPropertyName = value; break;
                                            case "ItemIsValue": Boolean.TryParse(value, out itemIsValue); break;
                                            case "ItemTemplate": itemTemplateName = value; break;
                                            case "ItemValueId": itemValueId = value; break;
                                        }
                                    }
                                }
                            }
                        }
                    }
                }
            }

            var binding = new DataBinding { View = view, ResourceId = resourceId, Mode = mode, ViewModelPropertyInfo = viewModel.GetType().GetProperty(propertyName) };

            if (binding.View is AdapterView)
            {
                if (listPropertyName == null) listPropertyName = propertyName + "List";
                var pi = viewModel.GetType().GetProperty(listPropertyName);
                if (pi == null && binding.ViewModelPropertyInfo.PropertyType.GetInterface("IList") != null)
                {
                    listPropertyName = propertyName;
                    pi = binding.ViewModelPropertyInfo;
                    binding.ViewModelPropertyInfo = null;
                }
                binding.ViewModelListPropertyInfo = pi;

                pi = binding.View.GetType().GetProperty("Adapter", BindingFlags.Public | BindingFlags.Instance);
                if (pi != null)
                {
                    var adapter = pi.GetValue(binding.View);
                    if (adapter == null) {
                        if (itemTemplateName == null) itemTemplateName = listPropertyName + "Item";
                        if (itemIsValue && itemValueId == null) itemValueId = itemTemplateName;
                        int? itemTemplateResourceId = AndroidHelpers.FindResourceId(itemTemplateName, AndroidHelpers.ResourceCategory.Layout);
                        int? itemValueResourceId = AndroidHelpers.FindResourceId(itemValueId);
                        if (itemTemplateResourceId.HasValue)
                        {
                            adapter = new DataBindableListAdapter<object>(layoutInflater, itemTemplateResourceId.Value, itemTemplateName + "_", itemValueResourceId, rootViewExtensionPoints);
                            pi.SetValue(binding.View, adapter);
                        }
                    }
                    binding.ListAdapter = adapter as IDataBindableListAdapter;
                }
            }

            switch (binding.Mode)
            {
                case BindingMode.TwoWay: AddTwoWayHandler(binding); break;
                case BindingMode.Command: AddCommandHandler(binding); break;
            }

            dataBindings.Add(idName, binding);
            return binding;
        }

        private DataBinding FindBindingForView(View view)
        {
            return dataBindings.FirstOrDefault(i => object.ReferenceEquals(i.Value.View, view)).Value;
        }

        private DataBinding FindBindingForListProperty(string propertyName)
        {
            return dataBindings.FirstOrDefault(i => i.Value.ViewModelListPropertyInfo != null && i.Value.ViewModelListPropertyInfo.Name == propertyName).Value;
        }

        private void UpdateView(DataBinding binding)
        {
            if (binding.View != null && binding.ViewModelPropertyInfo != null)
            {
                var view = binding.View;
                var value = binding.ViewModelPropertyInfo.GetValue(viewModel);
                if (rootViewExtensionPoints != null) rootViewExtensionPoints.UpdateView(view, value); else UpdateView(view, value);
            }
        }

        private void UpdateList(DataBinding binding)
        {
            if (binding.ViewModelListPropertyInfo != null && binding.ListAdapter != null)
            {
                var list = (IList)binding.ViewModelListPropertyInfo.GetValue(viewModel);
                binding.ListAdapter.SetList(list);
            }
        }
    }
}