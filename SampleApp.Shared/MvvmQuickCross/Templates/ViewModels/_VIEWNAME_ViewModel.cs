#if TEMPLATE // To add a new view model class: copy this file, then in the copy remove the enclosing #if TEMPLATE ... #endif lines and replace _VIEWMODEL_ with the view model name.
using System;
using MvvmQuickCross;

namespace MvvmQuickCross.Templates.ViewModels
{
    public class _VIEWMODEL_ViewModel : ViewModelBase
    {
        public _VIEWMODEL_ViewModel()
        {
            // TODO: pass any services that this model needs as contructor parameters. 
        }

        #region Data-bindable properties and commands
        // TODO: Generate data-bindable properties and commands here with prop* and cmd* code snippets
        #endregion
    }
}

// Design data support
#if DEBUG
namespace MvvmQuickCross.Templates.ViewModels.Design
{
    public class _VIEWMODEL_ViewModelDesign : _VIEWMODEL_ViewModel
    {
        public _VIEWMODEL_ViewModelDesign()
        {
            // TODO: Initialize the view model with hardcoded design-time data
        }
    }
}
#endif

#endif // TEMPLATE