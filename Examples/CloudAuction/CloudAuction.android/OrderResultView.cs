using Android.App;
using Android.OS;
using MvvmQuickCross;
using CloudAuction.Shared;
using CloudAuction.Shared.ViewModels;

namespace CloudAuction
{
    [Activity(Label = "Order Confirmation")]
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