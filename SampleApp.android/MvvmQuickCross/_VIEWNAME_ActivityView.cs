#if TEMPLATE // To add a new activity view class: copy this file, then in the copy remove the enclosing #if TEMPLATE ... #endif lines, replace _VIEWNAME_ with the view name and implement any missing _APPNAME_Application members that are called from below code.
using Android.App;
using Android.OS;
using MvvmQuickCross;

namespace MvvmQuickCross.Templates
{
    [Activity(Label = "_VIEWNAME_")]
    public class _VIEWNAME_View : ActivityViewBase<_VIEWNAME_ViewModel>
    {
        protected override void OnCreate(Bundle bundle)
        {
            base.OnCreate(bundle);
            SetContentView(Resource.Layout._VIEWNAME_View);
            Initialize(FindViewById(Resource.Id._VIEWNAME_View), _APPNAME_Application.Instance._VIEWNAME_ViewModel);
        }
    }
}
#endif // TEMPLATE