#if TEMPLATE // To add a navigator class: in the Visual Studio Package Manager Console (menu View | Other Windows), enter "Install-Mvvm". Alternatively: copy this file, then in the copy remove the enclosing #if TEMPLATE ... #endif lines and replace _APPNAME_ with the application name.
using System;
using System.Windows.Controls;
using MvvmQuickCrossLibrary.Templates;

namespace MvvmQuickCross.Templates
{
    class _APPNAME_Navigator : I_APPNAME_Navigator
    {
        private void Navigate(object navigationContext, string uri)
        {
            ((Frame)navigationContext).Navigate(new Uri(uri, UriKind.Relative));
        }

        public void NavigateToMainView(object navigationContext)
        {
            Navigate(navigationContext, "/MainView.xaml"); // TODO: Specify the correct main view uri 
        }
    }
}
#endif // TEMPLATE