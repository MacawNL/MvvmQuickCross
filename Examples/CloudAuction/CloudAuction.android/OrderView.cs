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
            Initialize(FindViewById(Resource.Id.OrderView), CloudAuctionApplication.Instance.OrderViewModel);
        }
    }
}