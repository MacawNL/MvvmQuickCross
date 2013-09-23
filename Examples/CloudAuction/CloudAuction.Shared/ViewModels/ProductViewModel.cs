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
        public string ListPrice;

        public override string ToString()
        {
            return Name + "\r\n" + ListPrice + "\r\n" + Description;
        }

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
        private static int nr = 1;

        public ProductViewModelDesign()
        {
            Name = "Product Name " + nr.ToString();
            Description = "Lorem ipsum dolor sit amet, consectetur adipiscing elit. Donec id placerat nisi. Phasellus scelerisque vestibulum lorem eget aliquam. Nunc quis.";
            ListPrice = string.Format("$ {0},00", 240 + nr++);
        }
    }
}
#endif

