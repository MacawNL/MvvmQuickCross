using System;
using MvvmQuickCross;

namespace MvvmQuickCross.Templates.ViewModels
{
    public class _VIEWMODEL_ViewModel : ViewModelBase
    {
        public _VIEWMODEL_ViewModel()
        {
            // TODO: pass any services that this model needs as contructor parameters. 
            // To support design-time data in VS, either make all parameters for this constructor optional or add a protected contructor without parameters.
        }

        #region Data-bindable properties and commands
        // TODO: Generate data-bindable properties and commands here with prop* and cmd* code snippets
        #endregion
    }
}

// VS Design-time data support
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