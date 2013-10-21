using CloudAuction.Shared.ViewModels;
namespace CloudAuction.Shared
{
    public interface ICloudAuctionNavigator
    {
        void NavigateToMainView(object navigationContext, MainViewModel.SubView? subView);
        void NavigateToProductView(object navigationContext);
        void NavigateToOrderView(object navigationContext);
        void NavigateToOrderResultView(object navigationContext);

        /* TODO: For each view, add a method to navigate to that view with a signature like this:
        void NavigateTo_VIEWNAME_View(object navigationContext);
         * Note that the New-View command adds the above code automatically (see http://github.com/MacawNL/MvvmQuickCross#new-view). */
    }
}
