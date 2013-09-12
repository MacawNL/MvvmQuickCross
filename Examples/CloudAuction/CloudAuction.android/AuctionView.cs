using System;

using Android.App;
using Android.OS;
using Android.Views;
using Android.Widget;
using MvvmQuickCross;
using CloudAuction.Shared;
using CloudAuction.Shared.ViewModels;

namespace CloudAuction
{
    public class AuctionView : FragmentViewBase<AuctionViewModel>
    {
        private Button placeBidButton;

        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {
            base.OnCreateView(inflater, container, savedInstanceState);
            var view = inflater.Inflate(Resource.Layout.AuctionView, container, false);
            placeBidButton = view.FindViewById<Button>(Resource.Id.AuctionViewPlaceBid);
            Initialize(view, CloudAuctionApplication.Instance.AuctionViewModel);
            return view;
        }

        void placeBidButton_Click(object sender, EventArgs e)
        {
            CloudAuctionApplication.Instance.ContinueToOrder(null);
        }

        protected override void AddHandlers()
        {
            base.AddHandlers();
            placeBidButton.Click += placeBidButton_Click;
        }

        protected override void RemoveHandlers()
        {
            base.RemoveHandlers();
            placeBidButton.Click -= placeBidButton_Click;
        }

        /*
        private void EnsureHandlersAreAdded()
        {
            if (areHandlersAdded) return;
            placeBidButton.Click += placeBidButton_Click;
            bindings.AddHandlers();
            viewModel.PropertyChanged += AuctionViewModel_PropertyChanged;
            areHandlersAdded = true;
        }

        private void EnsureHandlersAreRemoved()
        {
            if (!areHandlersAdded) return;
            viewModel.PropertyChanged -= AuctionViewModel_PropertyChanged;
            bindings.RemoveHandlers();
            areHandlersAdded = false;
        }

        void AuctionViewModel_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            bindings.UpdateView("AuctionView", e.PropertyName);
        }
         */
    }
}