using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

using Android.Views;
using CloudAuction;

namespace MvvmQuickCross
{
    public class ViewDataBindings
    {
        public enum BindingMode { OneWay, OneTime, TwoWay, Command };

        private class DataBinding
        {
            public BindingMode Mode;
            public View View;
            public PropertyInfo ViewModelPropertyInfo;
            public int? ResourceId;
        }

        private readonly View rootView;
        private readonly ViewModelBase viewModel;
        private readonly string idPrefix;

        private Dictionary<string, DataBinding> dataBindings = new Dictionary<string, DataBinding>();

        public ViewDataBindings(View rootView, ViewModelBase viewModel, string idPrefix)
        {
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
                    AddBinding(commandName, isCommand: true);
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
            if (!dataBindings.TryGetValue(IdName(propertyName), out binding))
            {
                binding = AddBinding(propertyName, isCommand: false);
            }

            if (binding != null) UpdateView(binding);
        }

        public void RemoveHandlers()
        {
            foreach (var item in dataBindings)
            {
                var binding = item.Value;
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
                switch (binding.Mode)
                {
                    case BindingMode.TwoWay: AddTwoWayHandler(binding); break;
                    case BindingMode.Command: AddCommandHandler(binding); break;
                }
            }
        }

        public void AddPropertyBinding(string propertyName, View view)
        {
            AddBinding(propertyName, false, view);
        }

        public void AddCommandBinding(string commandName, View view)
        {
            AddBinding(commandName, true, view);
        }

        private string IdName(string name) { return idPrefix + name; }

        private DataBinding AddBinding(string propertyName, bool isCommand, View view = null)
        {
            string idName = (view != null) ? view.Id.ToString() : IdName(propertyName);

            var binding = new DataBinding();
            binding.View = view;
            binding.Mode = isCommand ? BindingMode.Command : BindingMode.OneWay;
            binding.ViewModelPropertyInfo = viewModel.GetType().GetProperty(propertyName);

            var fieldInfo = typeof(Resource.Id).GetField(idName);
            if (fieldInfo != null)
            {
                binding.ResourceId = (int)fieldInfo.GetValue(null);
                if (binding.View == null)
                {
                    binding.View = rootView.FindViewById(binding.ResourceId.Value);
                }
            }

            if (binding.View == null) return null;

            if (!isCommand && binding.View.Tag != null)
            {
                string tag = binding.View.Tag.ToString();
                if (tag.Contains("TwoWay"))
                {
                    binding.Mode = BindingMode.TwoWay;
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

        #region View types that support command binding

        private void AddCommandHandler(DataBinding binding)
        {
            if (binding.View == null) return;
            string viewTypeName = binding.View.GetType().FullName;
            switch (viewTypeName)
            {
                case "Android.Widget.Button":
                    ((Android.Widget.Button)binding.View).Click += Button_Click;
                    break;
                default:
                    throw new NotImplementedException("View type not implemented: " + viewTypeName);
            }
        }

        private void RemoveCommandHandler(DataBinding binding)
        {
            if (binding.View == null) return;
            string viewTypeName = binding.View.GetType().FullName;
            switch (viewTypeName)
            {
                case "Android.Widget.Button":
                    ((Android.Widget.Button)binding.View).Click -= Button_Click;
                    break;
                default:
                    throw new NotImplementedException("View type not implemented: " + viewTypeName);
            }
        }

        private void Button_Click(object sender, EventArgs e)
        {
            var binding = dataBindings.FirstOrDefault(i => object.ReferenceEquals(i.Value.View, sender)).Value;
            if (binding != null)
            {
                var command = (RelayCommand)binding.ViewModelPropertyInfo.GetValue(viewModel);
                command.Execute(null);
            }
        }

        #endregion View types that support command binding

        #region View types that support one-way data binding

        private void UpdateView(DataBinding binding)
        {
            if (binding.View != null)
            {
                string viewTypeName = binding.View.GetType().FullName;
                object value = binding.ViewModelPropertyInfo.GetValue(viewModel);

                switch (viewTypeName)
                {
                    case "Android.Widget.TextView":
                    case "Android.Widget.EditText":
                        ((Android.Widget.TextView)binding.View).Text = value.ToString();
                        break;

                    case "Android.Widget.ProgressBar":
                        ((Android.Widget.ProgressBar)binding.View).Progress = (int)value;
                        break;

                    case "Macaw.UIComponents.MultiImageView":
                        if (value is Uri) value = ((Uri)value).AbsoluteUri;
                        var multiImageView = (Macaw.UIComponents.MultiImageView)binding.View;
                        multiImageView.LoadImageList(new[] { (string)value });
                        multiImageView.LoadImage(); // TODO: Fix hang on no connection; Fix this LoadImage call shoudl not be needed.
                        break;

                    default:
                        throw new NotImplementedException("View type not implemented: " + viewTypeName);
                }
            }
        }

        #endregion View types that support one-way data binding

        #region View types that support two-way data binding

        private void AddTwoWayHandler(DataBinding binding)
        {
            if (binding.View == null) return;
            string viewTypeName = binding.View.GetType().FullName;
            switch (viewTypeName)
            {
                case "Android.Widget.EditText":
                    ((Android.Widget.EditText)binding.View).FocusChange += TextView_FocusChange;
                    break;
                default:
                    throw new NotImplementedException("View type not implemented: " + viewTypeName);
            }
        }

        private void RemoveTwoWayHandler(DataBinding binding)
        {
            if (binding.View == null) return;
            string viewTypeName = binding.View.GetType().FullName;
            switch (viewTypeName)
            {
                case "Android.Widget.EditText":
                    ((Android.Widget.EditText)binding.View).FocusChange -= TextView_FocusChange;
                    break;
                default:
                    throw new NotImplementedException("View type not implemented: " + viewTypeName);
            }
        }

        private void TextView_FocusChange(object sender, View.FocusChangeEventArgs e)
        {
            if (!e.HasFocus)
            {
                var binding = dataBindings.FirstOrDefault(i => object.ReferenceEquals(i.Value.View, sender)).Value;
                if (binding != null)
                {
                    binding.ViewModelPropertyInfo.SetValue(viewModel, ((Android.Widget.TextView)binding.View).Text);
                }
            }
        }

        #endregion View types that support two-way data binding
    }
}