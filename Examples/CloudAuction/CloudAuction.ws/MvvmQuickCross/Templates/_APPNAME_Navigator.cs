#if TEMPLATE // To add a navigator class: in the Visual Studio Package Manager Console (menu View | Other Windows), enter "Install-Mvvm". Alternatively: copy this file, then in the copy remove the enclosing #if TEMPLATE ... #endif lines and replace CloudAuction with the application name.
using System;
using Windows.UI.Xaml.Controls;
using CloudAuction.Shared;

namespace CloudAuction
{
    class CloudAuctionNavigator : ICloudAuctionNavigator
    {
        private void Navigate(object navigationContext, Type sourcePageType)
        {
            ((Frame)navigationContext).Navigate(sourcePageType);
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