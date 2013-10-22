using Android.App;
using Android.OS;
using Android.Content.PM;
using MvvmQuickCross;
using SampleApp.Shared;
using SampleApp.Shared.ViewModels;

namespace SampleApp
{
    [Activity(Label = "SampleApp", MainLauncher = true, LaunchMode = LaunchMode.SingleTask, Icon = "@drawable/icon")]
    public class SampleItemListView : ActivityViewBase<SampleItemListViewModel>
    {
        private SampleAppApplication EnsureApplication()
        {
            return SampleAppApplication.Instance ?? new SampleAppApplication(new SampleAppNavigator());
        }

        protected override void OnCreate(Bundle bundle)
        {
            base.OnCreate(bundle);
            SetContentView(Resource.Layout.SampleItemListView);
            AndroidHelpers.Initialize(typeof(Resource));
            EnsureApplication();
            SampleAppApplication.Instance.ContinueToSampleItemList(skipNavigation: true);
            Initialize(FindViewById(Resource.Id.SampleItemListView), SampleAppApplication.Instance.SampleItemListViewModel);
        }
    }
}

