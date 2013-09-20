using Android.App;
using Android.Views;
using Android.OS;
using Android.Content.PM;
using MvvmQuickCross;
using CloudAuction.Shared;

namespace CloudAuction
{
    [Activity(Label = "Cloud Auction", MainLauncher = true, LaunchMode = LaunchMode.SingleTask, Icon = "@drawable/icon")]
    public class MainActivity : Activity
    {
        private bool areHandlersAdded; // TODO: see if we can use the view base class with either the auction view model including extra , to eliminate lifetime management code?
        private ActionBar.Tab[] tabs;
        private Fragment[] tabFragments;

        private CloudAuctionApplication EnsureApplication()
        {
            return CloudAuctionApplication.Instance ?? new CloudAuctionApplication(new CloudAuctionNavigator());
        }

        private void EnsureHandlersAdded()
        {
            if (areHandlersAdded) return;
            foreach (var tab in tabs) tab.TabSelected += Tab_TabSelected;
            areHandlersAdded = true;
        }

        private void EnsureHandlersRemoved()
        {
            if (!areHandlersAdded) return;
            foreach (var tab in tabs) tab.TabSelected -= Tab_TabSelected;
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

            tabFragments = new Fragment[] { new AuctionView(), new ProductsView(), new Fragment() };
            tabs = new ActionBar.Tab[] { ActionBar.NewTab().SetText("Auction"), ActionBar.NewTab().SetText("Products"), ActionBar.NewTab().SetText("Help") };
            EnsureHandlersAdded();
            foreach (var tab in tabs) ActionBar.AddTab(tab);
        }

        public enum TabIndex { Auction, Products, Help }
        public static TabIndex CurrentTabIndex;

        private void EnsureCurrentTabIsSelected()
        {
            int index = (int)CurrentTabIndex;
            if (ActionBar.SelectedNavigationIndex != index) ActionBar.SetSelectedNavigationItem(index);
        }

        protected override void OnDestroy()
        {
            EnsureHandlersRemoved();
            AndroidHelpers.ClearActivityReference(this);
            base.OnDestroy();
        }

        protected override void OnPause()
        {
            EnsureHandlersRemoved();
            AndroidHelpers.ClearActivityReference(this);
            base.OnPause();
        }

        protected override void OnResume()
        {
            base.OnResume();
            AndroidHelpers.SetCurrentActivity(this);
            EnsureHandlersAdded();
            EnsureCurrentTabIsSelected();
        }

        private void Tab_TabSelected(object sender, ActionBar.TabEventArgs e)
        {
            var tab = (ActionBar.Tab)sender;
            TabIndex tabIndex = (TabIndex)tab.Position;
            switch (tabIndex)
            {
                case TabIndex.Auction: CloudAuctionApplication.Instance.ContinueToAuction(skipNavigation: true); break;
                case TabIndex.Products: CloudAuctionApplication.Instance.ContinueToProducts(skipNavigation: true); break;
            }
            e.FragmentTransaction.Replace(Resource.Id.mainFragmentContainer, tabFragments[tab.Position]);
            // TODO: Check if we should also use .Remove in TabUnselected event? E.g. see http://arvid-g.de/12/android-4-actionbar-with-tabs-example
        }

        public override bool OnCreateOptionsMenu(IMenu menu)
        {
            MenuInflater.Inflate(Resource.Menu.MainMenu, menu);
            return base.OnPrepareOptionsMenu(menu);
        }
    }
}

