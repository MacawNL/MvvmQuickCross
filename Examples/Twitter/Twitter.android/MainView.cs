using Android.App;
using Android.OS;
using Android.Content.PM;
using MvvmQuickCross;
using Twitter.Shared;
using Twitter.Shared.ViewModels;

namespace Twitter
{
    [Activity(Label = "Twitter Main", MainLauncher = true, LaunchMode = LaunchMode.SingleTask, Icon = "@drawable/icon")]
    public class MainView : ActivityViewBase<MainViewModel>
    {
        private TwitterApplication EnsureApplication()
        {
            return TwitterApplication.Instance ?? new TwitterApplication(new TwitterNavigator());
        }

        protected override void OnCreate(Bundle bundle)
        {
            base.OnCreate(bundle);
            SetContentView(Resource.Layout.MainView);
            AndroidHelpers.Initialize(typeof(Resource));
            EnsureApplication();
            TwitterApplication.Instance.CurrentNavigationContext = this;
            TwitterApplication.Instance.ContinueToMain(skipNavigation: true);
            Initialize(FindViewById(Resource.Id.MainView), TwitterApplication.Instance.MainViewModel);
        }
    }
}
