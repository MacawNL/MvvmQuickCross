#if TEMPLATE // To add a new activity view class: copy this file, then in the copy remove the enclosing #if TEMPLATE ... #endif lines, replace _VIEWNAME_ with the view name and implement any missing _APPNAME_Application members that are called from below code.
using Android.OS;
using Android.Views;
using MvvmQuickCross;
using MvvmQuickCross.Templates.Shared;
using MvvmQuickCross.Templates.Shared.ViewModels;

namespace MvvmQuickCross.Templates
{
    public class _VIEWNAME_View : FragmentViewBase<_VIEWNAME_ViewModel>
    {
        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {
            _APPNAME_Application.Instance.ContinueTo_VIEWNAME_(skipNavigation: true); // This line is only needed if this view can be navigated to by some other means than through ContinueTo_VIEWNAME_() 
            var view = inflater.Inflate(Resource.Layout._VIEWNAME_View, container, false);
            Initialize(view, _APPNAME_Application.Instance._VIEWNAME_ViewModel, inflater);
            return view;
        }
    }
}
#endif // TEMPLATE