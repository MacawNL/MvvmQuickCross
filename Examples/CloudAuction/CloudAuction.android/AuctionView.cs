using System;

using Android.App;
using Android.OS;
using Android.Views;
using Android.Widget;
using MvvmQuickCross;
using CloudAuction.Shared;
using CloudAuction.Shared.ViewModels;

namespace CloudAuction
{
    public class AuctionView : FragmentViewBase<AuctionViewModel>
    {
        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {
            base.OnCreateView(inflater, container, savedInstanceState);
            var view = inflater.Inflate(Resource.Layout.AuctionView, container, false);
            Initialize(typeof(Resource.Id), view, CloudAuctionApplication.Instance.AuctionViewModel);
            return view;
        }
    }
}