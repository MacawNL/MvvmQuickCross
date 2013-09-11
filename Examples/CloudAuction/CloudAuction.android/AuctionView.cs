using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Util;
using Android.Views;
using Android.Widget;
using CloudAuction.Shared;
using CloudAuction.Shared.ViewModels;
using System.Reflection;

namespace CloudAuction
{
    public class AuctionView : Fragment
    {
        private bool areHandlersAdded;
        private View thisView;
        private AuctionViewModel viewModel;

        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {
            base.OnCreateView(inflater, container, savedInstanceState);

            var view = inflater.Inflate(Resource.Layout.AuctionView, container, false);

            thisView = view;
            viewModel = CloudAuctionApplication.Instance.AuctionViewModel;
            AddHandlers();
            viewModel.NotifyAllPropertiesChanged();
                        
            return view;
        }

        public override void OnPause()
        {
            RemoveHandlers();
            base.OnPause();
        }

        public override void OnResume()
        {
            base.OnResume();
            AddHandlers();
        }

        private void AddHandlers()
        {
            if (areHandlersAdded) return;
            viewModel.PropertyChanged += AuctionViewModel_PropertyChanged;
            areHandlersAdded = true;
        }

        private void RemoveHandlers()
        {
            if (!areHandlersAdded) return;
            viewModel.PropertyChanged -= AuctionViewModel_PropertyChanged;
            RemoveDataBindingHandlers();
            areHandlersAdded = false;
        }

        void AuctionViewModel_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            UpdateViewModelPropertyInView<AuctionViewModel>(thisView, "AuctionView", e.PropertyName, viewModel);
        }

        public class DataBinding
        {
            public enum BindingMode { OneWay, OneTime, TwoWay };
            public BindingMode Mode;
            public View View;
            public PropertyInfo ViewModelPropertyInfo;
            public int? ResourceId;
        }

        private Dictionary<string, DataBinding> dataBindings = new Dictionary<string,DataBinding>();

        private void UpdateViewModelPropertyInView<T>(View view, string prefix, string propertyName, T viewModel)
        {
            string name = prefix + propertyName;

            DataBinding binding;
            if (!dataBindings.TryGetValue(name, out binding))
            {
                binding = new DataBinding();
                var fieldInfo = typeof(Resource.Id).GetField(name);
                if (fieldInfo != null)
                {
                    binding.ViewModelPropertyInfo = typeof(T).GetProperty(propertyName);
                    binding.ResourceId = (int)fieldInfo.GetValue(null);
                    binding.View = view.FindViewById(binding.ResourceId.Value);
                    if (binding.View != null && binding.View.Tag != null)
                    {
                        string tag = binding.View.Tag.ToString();
                        if (tag.Contains("TwoWay"))
                        {
                            AddTwoWayHandler(binding);
                            binding.Mode = DataBinding.BindingMode.TwoWay;
                        }
                    }
                }
                dataBindings.Add(name, binding);
            }

            UpdateView<T>(binding, viewModel);
        }

        private void AddTwoWayHandler(DataBinding binding)
        {
            string viewTypeName = binding.View.GetType().FullName;
            switch (viewTypeName)
            {
                case "Android.Widget.EditText":
                    ((Android.Widget.EditText)binding.View).TextChanged += AuctionView_TextChanged;
                    break;
                default:
                    throw new NotImplementedException("View type not implemented: " + viewTypeName);
            }
        }

        private void RemoveTwoWayHandler(DataBinding binding)
        {
            string viewTypeName = binding.View.GetType().FullName;
            switch (viewTypeName)
            {
                case "Android.Widget.EditText":
                    ((Android.Widget.EditText)binding.View).TextChanged -= AuctionView_TextChanged;
                    break;
                default:
                    throw new NotImplementedException("View type not implemented: " + viewTypeName);
            }
        }

        private void RemoveDataBindingHandlers()  // TODO: How do we re-add after we have removed handlers?
        {
            foreach (var item in dataBindings)
            {
                var binding = item.Value;
                if (binding.Mode == DataBinding.BindingMode.TwoWay) RemoveTwoWayHandler(binding);
            }
        }

        void AuctionView_TextChanged(object sender, Android.Text.TextChangedEventArgs e)
        {
            var view = (View)sender;
            var binding = dataBindings.First((b) => { return Object.ReferenceEquals(b.Value.View, sender); });
            binding.Value.ViewModelPropertyInfo.SetValue(viewModel, e.Text.ToString());
        }

        private void UpdateView<T>(DataBinding binding, T viewModel)
        {
            if (binding.View != null)
            {
                string viewTypeName = binding.View.GetType().FullName;
                object value = binding.ViewModelPropertyInfo.GetValue(viewModel);

                switch (viewTypeName)
                {
                    case "Android.Widget.TextView": 
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

        private void UpdateView<T>(View view, T viewModel)
        {
            foreach (var item in dataBindings)
            {
                var binding = item.Value;
                if (binding.ResourceId.HasValue)
                {
                    binding.View = view.FindViewById(binding.ResourceId.Value);
                }

                UpdateView<T>(binding, viewModel);
            }
        }
    }
}