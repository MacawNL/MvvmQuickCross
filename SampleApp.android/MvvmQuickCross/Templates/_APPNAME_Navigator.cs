#if TEMPLATE // To add a navigator class: in the Visual Studio Package Manager Console (menu View | Other Windows), enter "Install-Mvvm". Alternatively: copy this file, then in the copy remove the enclosing #if TEMPLATE ... #endif lines and replace _APPNAME_ with the application name.
using System;

using Android.Content;
using MvvmQuickCrossLibrary.Templates;

namespace MvvmQuickCross.Templates
{
    class _APPNAME_Navigator : I_APPNAME_Navigator
    {
        private void Navigate(object navigationContext, Type type)
        {
            var context = (Context)navigationContext;
            context.StartActivity(type);
        }

        /* TODO: For each view, add a method to navigate to that view like this:

        public void NavigateTo_VIEWNAME_View(object navigationContext)
        {
            Navigate(navigationContext, typeof(_VIEWNAME_View));
        }
         * Note that the New-View command adds the above code automatically (see http://github.com/MacawNL/MvvmQuickCross#new-view). */
    }
}
#endif // TEMPLATE