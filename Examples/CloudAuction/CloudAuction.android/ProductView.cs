using Android.App;
using Android.OS;
using MvvmQuickCross;
using CloudAuction.Shared;
using CloudAuction.Shared.ViewModels;

namespace CloudAuction
{
    [Activity(Label = "Product Details")]
    public class ProductView : ActivityViewBase<ProductViewModel>
    {
        protected override void OnCreate(Bundle bundle)
        {
            base.OnCreate(bundle);
            SetContentView(Resource.Layout.ProductView);
            Initialize(FindViewById(Resource.Id.ProductView), CloudAuctionApplication.Instance.ProductViewModel);
        }
    }
}