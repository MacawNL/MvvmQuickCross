using Android.App;
using Android.Views;
using Android.OS;
using MvvmQuickCross;
using CloudAuction.Shared;
using Android.Content.PM;

namespace CloudAuction
{
    [Activity(Label = "Cloud Auction", MainLauncher = true, LaunchMode = LaunchMode.SingleTask, Icon = "@drawable/icon")]
    public class MainActivity : Activity
    {
        private bool areHandlersAdded; // TODO: see if we can use the view base class without a view model, to eliminate lifetime management code?
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

            EnsureApplication(); // TODO: check if we should use class derived from android application object as entry point?
            AndroidHelpers.Initialize(typeof(Resource));
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
        }

        public enum TabIndex { Auction, Products, Help }

        public void SelectTab(TabIndex tabIndex)
        {
            int index = (int)tabIndex;
            if (ActionBar.SelectedNavigationIndex != index) ActionBar.SetSelectedNavigationItem(index);
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
    }
}

