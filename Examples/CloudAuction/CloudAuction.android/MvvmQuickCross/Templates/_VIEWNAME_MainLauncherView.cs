#if TEMPLATE // To add a new main launcher view class: in the Visual Studio Package Manager Console (menu View | Other Windows), use the New-View command with option -ViewType MainLauncher (enter "Get-Help New-View" for details). Alternatively: copy this file, then in the copy remove the enclosing #if TEMPLATE ... #endif lines and replace _VIEWNAME_ and CloudAuction with the viewmodel resp application name.
using Android.App;
using Android.OS;
using Android.Content.PM;
using MvvmQuickCross;
using CloudAuction.Shared;
using CloudAuction.Shared.ViewModels;

namespace CloudAuction
{
    [Activity(Label = "CloudAuction _VIEWNAME_", MainLauncher = true, LaunchMode = LaunchMode.SingleTask, Icon = "@drawable/icon")]
    public class _VIEWNAME_View : ActivityViewBase<_VIEWNAME_ViewModel>
    {
        private CloudAuctionApplication EnsureApplication()
        {
            return CloudAuctionApplication.Instance ?? new CloudAuctionApplication(new CloudAuctionNavigator());
        }

        protected override void OnCreate(Bundle bundle)
        {
            base.OnCreate(bundle);
            SetContentView(Resource.Layout._VIEWNAME_View);
            AndroidHelpers.Initialize(typeof(Resource));
            EnsureApplication();
            CloudAuctionApplication.Instance.CurrentNavigationContext = this;
            CloudAuctionApplication.Instance.ContinueTo_VIEWNAME_(skipNavigation: true);
            Initialize(FindViewById(Resource.Id._VIEWNAME_View), CloudAuctionApplication.Instance._VIEWNAME_ViewModel);
        }
    }
}
#endif // TEMPLATE