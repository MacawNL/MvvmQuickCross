using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
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

            var spinner = FindViewById<Spinner>(Resource.Id.OrderView_DeliveryLocation);
            spinner.Adapter = new DataBindableToStringListAdapter<string>(
                layoutInflater:         LayoutInflater, 
                viewResourceId:         Resource.Layout.TextListItem, 
                objectValueResourceId:  Resource.Id.TextListItem
            );

            /* Specify binding in code:
            var bindingParameters = new BindingParameters { 
                mode = BindingMode.TwoWay, 
                view = spinner, 
                propertyName = OrderViewModel.PROPERTYNAME_DeliveryLocation,
                listPropertyName = OrderViewModel.PROPERTYNAME_DeliveryLocations
            };
            */

            Initialize(
                resourceIdType:     typeof(Resource.Id), 
                rootView:           FindViewById(Resource.Id.OrderView), 
                viewModel:          CloudAuctionApplication.Instance.OrderViewModel
                /*, bindingsParameters: new BindingParameters[] { bindingParameters } */
            );
        }
    }
}