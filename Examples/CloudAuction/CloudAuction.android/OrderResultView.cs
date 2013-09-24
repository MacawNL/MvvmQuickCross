using Android.App;
using Android.OS;
using MvvmQuickCross;
using CloudAuction.Shared.ViewModels;
using CloudAuction.Shared;

namespace CloudAuction
{
    [Activity(Label = "Order Result View")]
    public class OrderResultView : ActivityViewBase<OrderResultViewModel>
    {
        protected override void OnCreate(Bundle bundle)
        {
            base.OnCreate(bundle);
            SetContentView(Resource.Layout.OrderResultView);
            Initialize(FindViewById(Resource.Id.OrderResultView), CloudAuctionApplication.Instance.OrderResultViewModel);
        }
    }
}