using System;

using Android.Content;
using Twitter.Shared;

namespace Twitter
{
    class TwitterNavigator : ITwitterNavigator
    {
        private void Navigate(object navigationContext, Type type)
        {
            var context = (Context)navigationContext;
            context.StartActivity(type);
        }

        public void NavigateToMainView(object navigationContext)
        {
            Navigate(navigationContext, typeof(MainView));
        }

        /* TODO: For each view, add a method to navigate to that view like this:

        public void NavigateTo_VIEWNAME_View(object navigationContext)
        {
            Navigate(navigationContext, typeof(_VIEWNAME_View));
        }
         * Note that the New-View command adds the above code automatically (see http://github.com/MacawNL/MvvmQuickCross#new-view). */
    }
}
