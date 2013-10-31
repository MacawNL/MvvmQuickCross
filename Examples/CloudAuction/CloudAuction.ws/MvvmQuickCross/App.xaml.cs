using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using CloudAuction.Shared;

namespace CloudAuction
{
    sealed partial class App : Application
    {
        public static CloudAuctionApplication EnsureCloudAuctionApplication(Frame navigationContext)
        {
            return CloudAuctionApplication.Instance ?? new CloudAuctionApplication(new CloudAuctionNavigator(), navigationContext);
        }
    }
}
