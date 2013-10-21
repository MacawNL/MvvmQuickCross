using System;

using Android.Content;
using CloudAuction.Shared;
using CloudAuction.Shared.ViewModels;

namespace CloudAuction
{
    class CloudAuctionNavigator : ICloudAuctionNavigator
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

        public void NavigateToProductView(object navigationContext)
        {
            Navigate(navigationContext, typeof(ProductView));
        }

        public void NavigateToOrderView(object navigationContext)
        {
            Navigate(navigationContext, typeof(OrderView));
        }

        public void NavigateToOrderResultView(object navigationContext)
        {
            Navigate(navigationContext, typeof(OrderResultView));
        }

        /* TODO: For each view, add a method to navigate to that view like this:

        public void NavigateTo_VIEWNAME_View(object navigationContext)
        {
            Navigate(navigationContext, typeof(_VIEWNAME_View));
        }
         * Note that the New-View command adds the above code automatically (see http://github.com/MacawNL/MvvmQuickCross#new-view). */
    }
}
