using System;

using Android.App;
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
            Navigate(navigationContext, typeof(MainActivity));
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