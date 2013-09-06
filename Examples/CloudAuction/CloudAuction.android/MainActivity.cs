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
    [Activity(Label = "CloudAuction", MainLauncher = true, Icon = "@drawable/icon")]
    public class MainActivity : Activity
    {
        private bool AreHandlersAdded;
        private ActionBar.Tab AuctionTab, ProductsTab, HelpTab;
        private Fragment AuctionFragment, ProductsFragment, HelpFragment;

        protected override void OnCreate(Bundle bundle)
        {
            base.OnCreate(bundle);

            SetContentView(Resource.Layout.Main);
            ActionBar.NavigationMode = ActionBarNavigationMode.Tabs;

            EnsureApplication();
            CloudAuctionApplication.Instance.CurrentNavigationContext = this;
            CloudAuctionApplication.Instance.ContinueToAuction(skipNavigation: true);

            AuctionTab = ActionBar.NewTab().SetText("Auction");
            AuctionFragment = new AuctionView();

            ProductsTab = ActionBar.NewTab().SetText("Products");
            ProductsFragment = new Fragment();

            HelpTab = ActionBar.NewTab().SetText("Help");
            HelpFragment = new Fragment();

            AddHandlers();

            ActionBar.AddTab(AuctionTab);
            ActionBar.AddTab(ProductsTab);
            ActionBar.AddTab(HelpTab);

            //// Get our button from the layout resource,
            //// and attach an event to it
            //Button button = FindViewById<Button>(Resource.Id.MyButton);
            //button.Click += delegate { button.Text = string.Format("{0} clicks!", count++); };
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
            if (AreHandlersAdded) return;
            AuctionTab.TabSelected += AuctionTab_TabSelected;
            ProductsTab.TabSelected += ProductsTab_TabSelected;
            HelpTab.TabSelected += HelpTab_TabSelected;
            AreHandlersAdded = true;
        }

        private void RemoveHandlers()
        {
            if (!AreHandlersAdded) return;
            AuctionTab.TabSelected -= AuctionTab_TabSelected;
            ProductsTab.TabSelected -= ProductsTab_TabSelected;
            HelpTab.TabSelected -= HelpTab_TabSelected;
            AreHandlersAdded = false;
        }

        private void AuctionTab_TabSelected(object sender, ActionBar.TabEventArgs e)
        {
            e.FragmentTransaction.Add(Resource.Id.mainFragmentContainer, AuctionFragment);
        }

        void ProductsTab_TabSelected(object sender, ActionBar.TabEventArgs e)
        {
            e.FragmentTransaction.Add(Resource.Id.mainFragmentContainer, ProductsFragment);
        }

        void HelpTab_TabSelected(object sender, ActionBar.TabEventArgs e)
        {
            e.FragmentTransaction.Add(Resource.Id.mainFragmentContainer, HelpFragment);
        }

        private CloudAuctionApplication EnsureApplication()
        {
            return CloudAuctionApplication.Instance ?? new CloudAuctionApplication(new CloudAuctionNavigator());
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

