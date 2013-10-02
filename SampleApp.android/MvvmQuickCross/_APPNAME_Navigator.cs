using System;

using Android.Content;
using MvvmQuickCross.Templates.Shared;

namespace MvvmQuickCross.Templates
{
    class _APPNAME_Navigator : I_APPNAME_Navigator
    {
        private void Navigate(object navigationContext, Type type)
        {
            var context = (Context)navigationContext;
            context.StartActivity(type);
        }

        public void NavigateTo_VIEWNAME_View(object navigationContext)
        {
            Navigate(navigationContext, typeof(_VIEWNAME_View));
        }
    }
}
