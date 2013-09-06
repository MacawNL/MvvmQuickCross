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
            UpdateView<AuctionViewModel>(view, viewModel); 
            AddHandlers();

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
            areHandlersAdded = false;
        }

        void AuctionViewModel_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            switch (e.PropertyName)
            {
                default:
                    UpdateViewModelPropertyInView<AuctionViewModel>(thisView, "AuctionView", e.PropertyName, viewModel);
                    // UpdateViewModelPropertyInView<AuctionViewModel>(thisView, "", e.PropertyName, viewModel, useTagInsteadOfId: true);
                    break;
            }
        }

        public class DataBinding
        {
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
                }
                dataBindings.Add(name, binding);
            }

            UpdateView<T>(binding, viewModel);
        }

        private void UpdateView<T>(DataBinding binding, T viewModel)
        {
            if (binding.View != null)
            {
                string viewTypeName = binding.View.GetType().Name;
                switch (viewTypeName)
                {
                    case "TextView": ((TextView)binding.View).Text = (string)binding.ViewModelPropertyInfo.GetValue(viewModel); break;
                    default: throw new NotImplementedException("View type not implemented: " + viewTypeName);
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