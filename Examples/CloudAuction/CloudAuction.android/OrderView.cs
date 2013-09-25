using Android.App;
using Android.OS;
using MvvmQuickCross;
using CloudAuction.Shared.ViewModels;
using CloudAuction.Shared;

namespace CloudAuction
{
    [Activity(Label = "Order")]
    public class OrderView : ActivityViewBase<OrderViewModel>
    {
        protected override void OnCreate(Bundle bundle)
        {
            base.OnCreate(bundle);
            SetContentView(Resource.Layout.OrderView);

            /* Example of how to specify a data bindable list adapter in code:
            var spinner = FindViewById<Android.Widget.Spinner>(Resource.Id.OrderView_DeliveryLocation);
            spinner.Adapter = new DataBindableListAdapter<string>(LayoutInflater, Resource.Layout.TextListItem, "TextListItem_", Resource.Id.TextListItem);
            */

            /* Example of how to specify data bindings in code instead of from Layout markup:
            var bindingsParameters = new BindingParameters[] {
               new BindingParameters { mode = BindingMode.TwoWay, view = spinner, propertyName = OrderViewModel.PROPERTYNAME_DeliveryLocation, listPropertyName = OrderViewModel.PROPERTYNAME_DeliveryLocationList }
            };
            */

            Initialize(FindViewById(Resource.Id.OrderView), CloudAuctionApplication.Instance.OrderViewModel /*, bindingsParameters */);
        }

        /* Example of how to handle specific viewmodel property changes in code instead of with (or in addition to) data binding:
        protected override void OnPropertyChanged(string propertyName)
        {
            switch (propertyName)
            {
                case OrderViewModel.PROPERTYNAME_DeliveryLocationListHasItems:
                    var hasItems = ViewModel.GetPropertyValue<bool>(OrderViewModel.PROPERTYNAME_DeliveryLocationListHasItems);
                    var spinner = FindViewById<Android.Widget.Spinner>(Resource.Id.OrderView_DeliveryLocation);
                    spinner.Visibility = hasItems ? Android.Views.ViewStates.Visible : Android.Views.ViewStates.Invisible;
                    break;
                default:
                    base.OnPropertyChanged(propertyName); break;
            }
        }
        */
    }
}