using System;

using Android.Content;
using CloudAuction.Shared;

namespace CloudAuction
{
    public class CloudAuctionNavigator : ICloudAuctionNavigator
    {
        private void Navigate(object navigationContext, Type type)
        {
            var context = (Context)navigationContext;
            context.StartActivity(type);
        }

        public void NavigateToAuctionView(object navigationContext)
        {
            if (navigationContext == null || navigationContext.GetType().Name != "MainActivity") Navigate(navigationContext, typeof(MainActivity));
            ((MainActivity)CloudAuctionApplication.Instance.CurrentNavigationContext).SelectTab(MainActivity.TabIndex.Auction);
        }

        public void NavigateToOrderView(object navigationContext)
        {
            Navigate(navigationContext, typeof(OrderView));
        }

        public void NavigateToOrderResultView(object navigationContext)
        {
            throw new NotImplementedException();
        }
    }
}