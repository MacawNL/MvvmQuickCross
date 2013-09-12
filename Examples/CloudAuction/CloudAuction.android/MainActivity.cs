using System;
using Android.App;
using Android.Content;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Android.OS;
using CloudAuction.Shared;

namespace CloudAuction
{
    [Activity(Label = "Cloud Auction", MainLauncher = true, Icon = "@drawable/icon")]
    public class MainActivity : Activity
    {
        private bool areHandlersAdded;
        private ActionBar.Tab auctionTab, productsTab, helpTab;
        private Fragment auctionFragment, productsFragment, helpFragment;

        private CloudAuctionApplication EnsureApplication()
        {
            return CloudAuctionApplication.Instance ?? new CloudAuctionApplication(new CloudAuctionNavigator());
        }

        private void EnsureHandlersAdded()
        {
            if (areHandlersAdded) return;
            auctionTab.TabSelected += AuctionTab_TabSelected;
            productsTab.TabSelected += ProductsTab_TabSelected;
            helpTab.TabSelected += HelpTab_TabSelected;
            areHandlersAdded = true;
        }

        private void EnsureHandlersRemoved()
        {
            if (!areHandlersAdded) return;
            auctionTab.TabSelected -= AuctionTab_TabSelected;
            productsTab.TabSelected -= ProductsTab_TabSelected;
            helpTab.TabSelected -= HelpTab_TabSelected;
            areHandlersAdded = false;
        }

        protected override void OnCreate(Bundle bundle)
        {
            base.OnCreate(bundle);

            SetContentView(Resource.Layout.Main);
            ActionBar.NavigationMode = ActionBarNavigationMode.Tabs;

            EnsureApplication();
            CloudAuctionApplication.Instance.CurrentNavigationContext = this;
            CloudAuctionApplication.Instance.ContinueToAuction(skipNavigation: true);

            auctionTab = ActionBar.NewTab().SetText("Auction");
            auctionFragment = new AuctionView();

            productsTab = ActionBar.NewTab().SetText("Products");
            productsFragment = new Fragment();

            helpTab = ActionBar.NewTab().SetText("Help");
            helpFragment = new Fragment();

            EnsureHandlersAdded();

            ActionBar.AddTab(auctionTab);
            ActionBar.AddTab(productsTab);
            ActionBar.AddTab(helpTab);

            //// Get our button from the layout resource,
            //// and attach an event to it
            //Button button = FindViewById<Button>(Resource.Id.MyButton);
            //button.Click += delegate { button.Text = string.Format("{0} clicks!", count++); };
        }

        protected override void OnResume()
        {
            base.OnResume();
            EnsureHandlersAdded();
        }

        protected override void OnPause()
        {
            EnsureHandlersRemoved();
            base.OnPause();
        }

        private void AuctionTab_TabSelected(object sender, ActionBar.TabEventArgs e)
        {
            e.FragmentTransaction.Replace(Resource.Id.mainFragmentContainer, auctionFragment);
            // TODO: Check if we should also use .Remove in TabUnselected event? E.g. see http://arvid-g.de/12/android-4-actionbar-with-tabs-example
        }

        void ProductsTab_TabSelected(object sender, ActionBar.TabEventArgs e)
        {
            e.FragmentTransaction.Replace(Resource.Id.mainFragmentContainer, productsFragment);
        }

        void HelpTab_TabSelected(object sender, ActionBar.TabEventArgs e)
        {
            e.FragmentTransaction.Replace(Resource.Id.mainFragmentContainer, helpFragment);
        }

        public override bool OnCreateOptionsMenu(IMenu menu)
        {
            MenuInflater.Inflate(Resource.Menu.MainMenu, menu);
            return base.OnPrepareOptionsMenu(menu);
        }

        // TODO: this should go into view fragment
        /*
        public override bool OnMenuItemSelected(int featureId, IMenuItem item)
        {
            switch (item.ItemId)
            {
                case Resource.Id.LogoutMenuItem:
                    //do something
                    return true;
            }
            return base.OnOptionsItemSelected(item);
        }
        */
    }
}

