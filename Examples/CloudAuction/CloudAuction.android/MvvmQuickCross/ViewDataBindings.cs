using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

using Android.Views;
using System.Text.RegularExpressions;
using Android.Widget;

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

        private Type resourceIdType;
        private readonly View rootView;
        private readonly ViewModelBase viewModel;
        private readonly string idPrefix;

        private Dictionary<string, DataBinding> dataBindings = new Dictionary<string, DataBinding>();

        public ViewDataBindings(Type resourceIdType, View rootView, ViewModelBase viewModel, string idPrefix)
        {
            this.resourceIdType = resourceIdType;
            this.rootView = rootView;
            this.viewModel = viewModel;
            this.idPrefix = idPrefix;
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

        public void UpdateView()
        {
            foreach (var item in dataBindings)
            {
                var binding = item.Value;
                if (binding.ResourceId.HasValue)
                {
                    binding.View = rootView.FindViewById(binding.ResourceId.Value);
                }

                UpdateView(binding);
            }
        }

        public void UpdateView(string propertyName)
        {
            DataBinding binding;
            if (dataBindings.TryGetValue(IdName(propertyName), out binding))
            {
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

            var binding = new DataBinding();
            binding.View = view;
            binding.Mode = mode;
            binding.ViewModelPropertyInfo = viewModel.GetType().GetProperty(propertyName);
            if (listPropertyName == null) listPropertyName = propertyName + "List";
            binding.ViewModelListPropertyInfo = viewModel.GetType().GetProperty(listPropertyName);

            var fieldInfo = resourceIdType.GetField(idName);
            if (fieldInfo != null)
            {
                binding.ResourceId = (int)fieldInfo.GetValue(null);
                if (binding.View == null)
                {
                    binding.View = rootView.FindViewById(binding.ResourceId.Value);
                }
            }

            if (binding.View == null) return null;

            if (binding.View.Tag != null)
            {
                string tag = binding.View.Tag.ToString();
                if (tag.Contains("Binding{Mode=TwoWay}"))
                {
                    binding.Mode = BindingMode.TwoWay;
                }
                // TODO: get list property name override from tag
            }

            if (binding.View is AdapterView)
            {
                var pi = binding.View.GetType().GetProperty("Adapter", BindingFlags.Public | BindingFlags.Instance);
                if (pi != null) binding.ListAdapter = pi.GetValue(binding.View) as IDataBindableListAdapter;
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
            UpdateView(binding.View, binding.ViewModelPropertyInfo.GetValue(viewModel));
        }

        private void UpdateList(DataBinding binding)
        {
            if (binding.ViewModelListPropertyInfo != null && binding.ListAdapter != null)
            {
                binding.ListAdapter.SetList((IList)binding.ViewModelListPropertyInfo.GetValue(viewModel));
            }
        }
    }
}