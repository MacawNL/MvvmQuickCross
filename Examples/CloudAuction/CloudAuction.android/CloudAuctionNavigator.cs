using System;

using Android.Content;
using MvvmQuickCross;
using CloudAuction.Shared;
using CloudAuction.Shared.ViewModels;

namespace CloudAuction
{
    public class CloudAuctionNavigator : ICloudAuctionNavigator
    {
        private void Navigate(object navigationContext, Type type)
        {
            var context = (Context)navigationContext;
            context.StartActivity(type);
        }

        public void NavigateToMainView(object navigationContext, MainViewModel.SubView? subView)
        {
            if (subView.HasValue) MainView.CurrentSubView = subView.Value;
            Navigate(navigationContext, typeof(MainView));
        }

        public void NavigateToOrderView(object navigationContext)
        {
            Navigate(navigationContext, typeof(OrderView));
        }

        public void NavigateToOrderResultView(object navigationContext)
        {
            Navigate(navigationContext, typeof(OrderResultView));
        }
    }
}