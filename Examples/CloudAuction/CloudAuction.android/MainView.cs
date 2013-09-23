using Android.App;
using Android.Views;
using Android.OS;
using Android.Content.PM;
using MvvmQuickCross;
using CloudAuction.Shared;
using CloudAuction.Shared.ViewModels;

namespace CloudAuction
{
    [Activity(Label = "Cloud Auction", MainLauncher = true, LaunchMode = LaunchMode.SingleTask, Icon = "@drawable/icon")]
    public class MainView : ActivityViewBase<MainViewModel>
    {
        public static MainViewModel.SubView CurrentSubView;

        private ActionBar.Tab[] tabs;

        private CloudAuctionApplication EnsureApplication()
        {
            return CloudAuctionApplication.Instance ?? new CloudAuctionApplication(new CloudAuctionNavigator());
        }

        protected override void OnCreate(Bundle bundle)
        {
            base.OnCreate(bundle);

            SetContentView(Resource.Layout.MainView);
            ActionBar.NavigationMode = ActionBarNavigationMode.Tabs;

            EnsureApplication(); // TODO: check if we should use class derived from android application object as entry point?
            AndroidHelpers.Initialize(typeof(Resource));
            CloudAuctionApplication.Instance.CurrentNavigationContext = this;
            CloudAuctionApplication.Instance.ContinueToMain(skipNavigation: true);

            tabs = new ActionBar.Tab[] { 
                ActionBar.NewTab().SetText("Auction").SetTag(new AuctionView()), 
                ActionBar.NewTab().SetText("Products").SetTag(new ProductsView()), 
                ActionBar.NewTab().SetText("Help").SetTag(new Fragment()) };

            Initialize(FindViewById(Resource.Layout.MainView), CloudAuctionApplication.Instance.MainViewModel);
            
            foreach (var tab in tabs) ActionBar.AddTab(tab);
        }

        protected override void AddHandlers()
        {
            base.AddHandlers();
            foreach (var tab in tabs) tab.TabSelected += Tab_TabSelected;
        }

        protected override void RemoveHandlers()
        {
            foreach (var tab in tabs) tab.TabSelected -= Tab_TabSelected;
            base.RemoveHandlers();
        }

        private void EnsureCurrentTabIsSelected()
        {
            int index = (int)CurrentSubView;
            if (ActionBar.SelectedNavigationIndex != index)
            {
                ActionBar.SetSelectedNavigationItem(index);
            }
        }

        protected override void OnResume()
        {
            base.OnResume();
            EnsureCurrentTabIsSelected();
        }

        private void Tab_TabSelected(object sender, ActionBar.TabEventArgs e)
        {
            var tab = (ActionBar.Tab)sender;
            e.FragmentTransaction.Replace(Resource.Id.MainView_TabFragmentContainer, (Fragment)tab.Tag);
            // TODO: Check if we should also use .Remove in TabUnselected event? E.g. see http://arvid-g.de/12/android-4-actionbar-with-tabs-example
            CurrentSubView = (MainViewModel.SubView)tab.Position;
        }

        public override bool OnCreateOptionsMenu(IMenu menu)
        {
            MenuInflater.Inflate(Resource.Menu.MainMenu, menu);
            return base.OnPrepareOptionsMenu(menu);
        }
    }
}

