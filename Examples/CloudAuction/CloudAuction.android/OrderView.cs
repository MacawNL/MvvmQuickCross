using Android.App;
using Android.OS;
using MvvmQuickCross;
using CloudAuction.Shared.ViewModels;
using CloudAuction.Shared;

namespace CloudAuction
{
    [Activity(Label = "Order View")]
    public class OrderView : ActivityViewBase<OrderViewModel>
    {
        protected override void OnCreate(Bundle bundle)
        {
            base.OnCreate(bundle);
            SetContentView(Resource.Layout.OrderView);

            /* Example of how to specify a data bindable list adapter in code:
            var spinner = FindViewById<Android.Widget.Spinner>(Resource.Id.OrderView_DeliveryLocation);
            spinner.Adapter = new DataBindableListAdapter<string>(LayoutInflater, Resource.Layout.TextListItem, Resource.Id.TextListItem);
            */

            /* Example of how to specify data bindings in code instead of from Layout markup:
            var bindingsParameters = new BindingParameters[] {
               new BindingParameters { mode = BindingMode.TwoWay, view = spinner, propertyName = OrderViewModel.PROPERTYNAME_DeliveryLocation, listPropertyName = OrderViewModel.PROPERTYNAME_DeliveryLocationList }
            };
            */

            Initialize(FindViewById(Resource.Id.OrderView), CloudAuctionApplication.Instance.OrderViewModel /*, bindingsParameters */);
        }
    }
}