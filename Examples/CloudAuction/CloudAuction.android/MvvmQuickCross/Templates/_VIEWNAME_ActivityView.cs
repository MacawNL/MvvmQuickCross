#if TEMPLATE // To add a new activity view class: in the Visual Studio Package Manager Console (menu View | Other Windows), use the New-View command (enter "Get-Help New-View" for details). Alternatively: copy this file, then in the copy remove the enclosing #if TEMPLATE ... #endif lines and replace _VIEWNAME_ and CloudAuction with the viewmodel resp application name.
// To add a new activity view class: copy this file, then in the copy remove the enclosing #if TEMPLATE ... #endif lines, replace _VIEWNAME_ with the view name and implement any missing CloudAuctionApplication members that are called from below code.
using Android.App;
using Android.OS;
using Android.Content.PM;
using MvvmQuickCross;
using CloudAuction.Shared;
using CloudAuction.Shared.ViewModels;

namespace CloudAuction
{
    [Activity(Label = "CloudAuction _VIEWNAME_", LaunchMode = LaunchMode.SingleTask, Icon = "@drawable/icon")]
    public class _VIEWNAME_View : ActivityViewBase<_VIEWNAME_ViewModel>
    {
        protected override void OnCreate(Bundle bundle)
        {
            base.OnCreate(bundle);
            SetContentView(Resource.Layout._VIEWNAME_View);
            Initialize(FindViewById(Resource.Id._VIEWNAME_View), CloudAuctionApplication.Instance._VIEWNAME_ViewModel);
        }
    }
}
#endif // TEMPLATE