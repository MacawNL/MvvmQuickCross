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

        /* Fields:
        public string Name, Description;
        public int ListPrice;
        */

        #region Data-bindable properties and commands
        // TODO: Generate data-bindable properties and commands here with prop* and cmd* code snippets
        public string Name /* One-way data-bindable property generated with propdb1 snippet. Keep on one line - see http://goo.gl/Yg6QMd for why. */ { get { return _Name; } protected set { if (_Name != value) { _Name = value; RaisePropertyChanged(PROPERTYNAME_Name); } } } private string _Name; public const string PROPERTYNAME_Name = "Name";
        public string Description /* One-way data-bindable property generated with propdb1 snippet. Keep on one line - see http://goo.gl/Yg6QMd for why. */ { get { return _Description; } protected set { if (_Description != value) { _Description = value; RaisePropertyChanged(PROPERTYNAME_Description); } } } private string _Description; public const string PROPERTYNAME_Description = "Description";
        public int ListPriceNumeric /* Two-way data-bindable property that calls custom code in OnListPriceNumericChanged() from setter, generated with propdb2c snippet. Keep on one line - see http://goo.gl/Yg6QMd for why. */ { get { return _ListPriceNumeric; } set { if (_ListPriceNumeric != value) { _ListPriceNumeric = value; RaisePropertyChanged(PROPERTYNAME_ListPriceNumeric); OnListPriceNumericChanged(); } } } private int _ListPriceNumeric; public const string PROPERTYNAME_ListPriceNumeric = "ListPriceNumeric";
        public string ListPrice /* One-way data-bindable property generated with propdb1 snippet. Keep on one line - see http://goo.gl/Yg6QMd for why. */ { get { return _ListPrice; } protected set { if (_ListPrice != value) { _ListPrice = value; RaisePropertyChanged(PROPERTYNAME_ListPrice); } } } private string _ListPrice; public const string PROPERTYNAME_ListPrice = "ListPrice";
        #endregion

        private void OnListPriceNumericChanged()
        {
            ListPrice = string.Format("$ {0},00", ListPriceNumeric);
        }

        public override string ToString()
        {
            return Name + "\r\n$ " + ListPrice.ToString() + ",00\r\n" + Description;
        }
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
            ListPriceNumeric = 240 + nr++;
        }
    }
}
#endif

