using System;
using MvvmQuickCross;

namespace CloudAuction.Shared.ViewModels
{
    public class ProductViewModel : ViewModelBase
    {
        public ProductViewModel()
        {
            // TODO: pass any services that this model needs as contructor parameters. 
        }

        public string Name, Description;
        public int ListPrice;

        #region Data-bindable properties and commands
        // TODO: Generate data-bindable properties and commands here with prop* and cmd* code snippets

        #endregion
    }
}

// Design-time data support
#if DEBUG
namespace CloudAuction.Shared.ViewModels.Design
{
    public class ProductViewModelDesign : ProductViewModel
    {
        private static int nr = 0;

        public ProductViewModelDesign()
        {
            Name = "Name " + nr.ToString();
            Description = "Description " + nr.ToString();
            ListPrice = nr++;
        }
    }
}
#endif

