//#if TEMPLATE // To add a new viewmodel class: in the Visual Studio Package Manager Console (menu View | Other Windows), enter "New-ViewModel <viewmodel name>". Alternatively: copy this file, then in the copy remove the enclosing #if TEMPLATE ... #endif lines and replace _VIEWMODEL_ with the viewmodel name.
using System;
using MvvmQuickCross;

namespace MvvmQuickCross.Templates.ViewModels
{
    public class _VIEWNAME_ViewModel : ViewModelBase
    {
        public _VIEWNAME_ViewModel()
        {
            // TODO: pass any services that this model needs as contructor parameters. 
        }

#region Data-bindable properties and commands
        // TODO: Generate data-bindable properties and commands here with prop* and cmd* code snippets
        #endregion
    }
}

// Design-time data support
#if DEBUG
namespace MvvmQuickCross.Templates.ViewModels.Design
{
    public class _VIEWNAME_ViewModelDesign : _VIEWNAME_ViewModel
    {
        public _VIEWNAME_ViewModelDesign()
        {
            // TODO: Initialize the view model with hardcoded design-time data
        }
    }
}
#endif

//#endif // TEMPLATE