using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using CloudAuction.Shared;

namespace CloudAuction
{
    class CloudAuctionNavigator : ICloudAuctionNavigator
    {
        private void Navigate(Activity navigationContext, Type type)
        {
            var intent = new Intent(navigationContext, type);
            navigationContext.StartActivity(intent);
        }

        public void NavigateToAuctionView(object navigationContext)
        {
            Navigate((Activity)navigationContext, typeof(AuctionView));
        }

        public void NavigateToOrderView(object navigationContext)
        {
            throw new NotImplementedException();
        }

        public void NavigateToOrderResultView(object navigationContext)
        {
            throw new NotImplementedException();
        }
    }
}