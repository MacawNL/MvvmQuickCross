using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Util;
using Android.Views;
using Android.Widget;
using CloudAuction.Shared;
using CloudAuction.Shared.ViewModels;

namespace CloudAuction
{
    public class AuctionView : Fragment
    {
        private View ThisView;
        private AuctionViewModel ViewModel;
        private TextView NameTextView, IntroTextView, DescriptionTextView;

        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {
            base.OnCreateView(inflater, container, savedInstanceState);

            var view = inflater.Inflate(Resource.Layout.AuctionView, container, false);

            NameTextView = view.FindViewById<TextView>(Resource.Id.NameTextView);
            IntroTextView = view.FindViewById<TextView>(Resource.Id.IntroTextView);
            DescriptionTextView = view.FindViewById<TextView>(Resource.Id.DescriptionTextView);

            ThisView = view;
            ViewModel = CloudAuctionApplication.Instance.AuctionViewModel;
            AddHandlers();


            return view;
        }

        public override void OnPause()
        {
            RemoveHandlers();
            base.OnPause();
        }

        public override void OnResume()
        {
            base.OnResume();
            AddHandlers();
        }

        private void AddHandlers()
        {
            ViewModel.PropertyChanged += AuctionViewModel_PropertyChanged;
        }

        private void RemoveHandlers()
        {
            ViewModel.PropertyChanged -= AuctionViewModel_PropertyChanged;
        }

        void AuctionViewModel_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            switch (e.PropertyName)
            {
                case AuctionViewModel.PROPERTYNAME_Name : NameTextView.Text  = ViewModel.Name ; break;
                case AuctionViewModel.PROPERTYNAME_Intro: IntroTextView.Text = ViewModel.Intro; break;
                default:
            //        case (typeof())
            //TextView : (TextView) .TextAlignmentCenter = 
            //        ctrl.propname = value

            //        int id = Resources.GetIdentifier(e.PropertyName, "", "");
                    
            //        DescriptionTextView.Text += " " + e.PropertyName + (v == null ? " found." : " not found.");
            //         TODO: ***HERE View == null? No exception? solve that
            //        if (v is TextView)
            //        {
            //            ((TextView)v).Text = e.PropertyName;
            //        }
                    break;
            }
        }
    }
}