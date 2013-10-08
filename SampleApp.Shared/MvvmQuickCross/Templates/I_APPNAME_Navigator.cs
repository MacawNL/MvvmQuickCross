#if TEMPLATE // To add a navigator interface class: in the Visual Studio Package Manager Console (menu View | Other Windows), enter "Install-Mvvm". Alternatively: copy this file, then in the copy remove the enclosing #if TEMPLATE ... #endif lines and replace _APPNAME_ with the application name.
namespace MvvmQuickCross.Templates
{
    public interface I_APPNAME_Navigator
    {
        void NavigateToMainView(object navigationContext);

        /* TODO: For each view, add a method to navigate to that view with a signature like this:
        void NavigateTo_VIEWNAME_View(object navigationContext);
        */
    }
}
#endif // TEMPLATE