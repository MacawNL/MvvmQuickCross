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

namespace CloudAuction
{
    [Activity(Label = "My Activity")]
    public class OrderView : Activity
    {
        protected override void OnCreate(Bundle bundle)
        {
            base.OnCreate(bundle);
            SetContentView(Resource.Layout.Main);
            //Button button = FindViewById<Button>(Resource.Id.MyButton);
            // Resources: add layout
            // event handler op buttons
            // Create your application here
        }

        protected override void OnResume()
        {
            base.OnResume();
            AddHandlers();
        }

        protected override void OnPause()
        {
            RemoveHandlers();
            base.OnPause();
        }

        private void AddHandlers()
        {
        }

        private void RemoveHandlers()
        {
        }
    }
}