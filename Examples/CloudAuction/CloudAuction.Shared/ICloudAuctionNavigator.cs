namespace CloudAuction.Shared
{
    public interface ICloudAuctionNavigator
    {
        /* TODO: For each view, add a method to navigate to that view with a signature like this:
                void NavigateTo_VIEWNAME_View(object navigationContext);
         */
        void NavigateToAuctionView(object navigationContext);
        void NavigateToOrderView(object navigationContext);
    }
}
