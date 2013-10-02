using Android.App;
using Android.OS;
using Android.Content.PM;
using MvvmQuickCross;
using MvvmQuickCross.Templates.Shared;
using MvvmQuickCross.Templates.Shared.ViewModels;

namespace MvvmQuickCross.Templates
{
    [Activity(Label = "_VIEWNAME_", MainLauncher = true, LaunchMode = LaunchMode.SingleTask, Icon = "@drawable/icon")]
    public class _VIEWNAME_View : ActivityViewBase<_VIEWNAME_ViewModel>
    {
        private _APPNAME_Application EnsureApplication()
        {
            return _APPNAME_Application.Instance ?? new _APPNAME_Application(new _APPNAME_Navigator());
        }

        protected override void OnCreate(Bundle bundle)
        {
            base.OnCreate(bundle);
            SetContentView(Resource.Layout._VIEWNAME_View);
            AndroidHelpers.Initialize(typeof(Resource));
            EnsureApplication();
            _APPNAME_Application.Instance.CurrentNavigationContext = this;
            _APPNAME_Application.Instance.ContinueTo_VIEWNAME_(skipNavigation: true);
            Initialize(FindViewById(Resource.Id._VIEWNAME_View), _APPNAME_Application.Instance._VIEWNAME_ViewModel);
        }
    }
}
