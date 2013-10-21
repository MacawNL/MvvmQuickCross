using System;
using Windows.UI.Xaml.Controls;
using CloudAuction.Shared;

namespace CloudAuction
{
    public class CloudAuctionNavigator : ICloudAuctionNavigator
    {
        private void Navigate(object navigationContext, Type sourcePageType)
        {
            ((Frame)navigationContext).Navigate(sourcePageType);
        }

        public void NavigateToMainView(object navigationContext, Shared.ViewModels.MainViewModel.SubView? subView)
        {
            if (!subView.HasValue || subView.Value == Shared.ViewModels.MainViewModel.SubView.Auction)
            {
                Navigate(navigationContext, typeof(AuctionView));
                return;
            }
            throw new NotImplementedException();
        }

        public void NavigateToProductView(object navigationContext)
        {
            throw new NotImplementedException();
        }

        public void NavigateToOrderView(object navigationContext)
        {
            Navigate(navigationContext, typeof(OrderView));
        }

        public void NavigateToOrderResultView(object navigationContext)
        {
            OrderResultView.Show();
        }
    }
}
