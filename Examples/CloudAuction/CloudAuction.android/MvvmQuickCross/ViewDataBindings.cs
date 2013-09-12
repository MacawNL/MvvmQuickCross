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

        private readonly View view;
        private readonly ViewModelBase viewModel;
        private readonly string idPrefix;

        private Dictionary<string, DataBinding> dataBindings = new Dictionary<string, DataBinding>();

        public ViewDataBindings(View view, ViewModelBase viewModel, string idPrefix)
        {
            this.view = view;
            this.viewModel = viewModel;
            this.idPrefix = idPrefix;
            UpdateCommandBindings();
        }

        private void UpdateCommandBindings()
        {
            foreach (string commandName in viewModel.CommandNames)
            {
                string name = idPrefix + commandName;

                DataBinding binding;
                if (!dataBindings.TryGetValue(name, out binding))
                {
                    binding = AddBinding(commandName, name, binding, isCommand: true);
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
                    binding.View = view.FindViewById(binding.ResourceId.Value);
                }

                UpdateView(binding);
            }
        }

        public void UpdateView(string propertyName)
        {
            string name = idPrefix + propertyName;

            DataBinding binding;
            if (!dataBindings.TryGetValue(name, out binding))
            {
                binding = AddBinding(propertyName, name, binding);
            }

            UpdateView(binding);
        }

        private DataBinding AddBinding(string propertyName, string name, DataBinding binding, bool isCommand = false)
        {
            binding = new DataBinding();
            binding.Mode = isCommand ? BindingMode.Command : BindingMode.OneWay;
            binding.ViewModelPropertyInfo = viewModel.GetType().GetProperty(propertyName);
            var fieldInfo = typeof(Resource.Id).GetField(name);
            if (fieldInfo != null)
            {
                binding.ResourceId = (int)fieldInfo.GetValue(null);
                binding.View = view.FindViewById(binding.ResourceId.Value);

                if (binding.View != null)
                {
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
                }
            }
            dataBindings.Add(name, binding);
            return binding;
        }

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

        private void Button_Click(object sender, EventArgs e)
        {
            var binding = dataBindings.First(i => object.ReferenceEquals(i.Value.View, sender)).Value;
            if (binding != null)
            {
                var command = (RelayCommand)binding.ViewModelPropertyInfo.GetValue(viewModel);
                command.Execute(null);
            }
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
                var binding = dataBindings.First(i => object.ReferenceEquals(i.Value.View, sender)).Value;
                if (binding != null)
                {
                    binding.ViewModelPropertyInfo.SetValue(viewModel, ((Android.Widget.TextView)binding.View).Text);
                }
            }
        }
    }
}