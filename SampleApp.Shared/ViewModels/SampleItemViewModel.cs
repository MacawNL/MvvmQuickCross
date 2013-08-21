using MvvmQuickCross;
using SampleApp.Shared.Models;

namespace SampleApp.Shared.ViewModels
{
    public class SampleItemViewModel : ViewModelBase
    {
        protected SampleItemViewModel () { } // VS Design time support

        public SampleItemViewModel(SampleItem item)
        {
            Item = item;
        }

        public SampleItem Item /* One-way data-bindable property generated with propdb1 snippet. Keep on one line - see http://goo.gl/Yg6QMd for why. */ { get { return _Item; } protected set { if (_Item != value) { _Item = value; RaisePropertyChanged(PROPERTYNAME_Item); } } } private SampleItem _Item; public const string PROPERTYNAME_Item = "Item";
    }
}

namespace SampleApp.Shared.ViewModels.Design
{
    public class SampleItemViewModelDesign : SampleItemViewModel
    {
        public SampleItemViewModelDesign() 
        {
            Item = new SampleItem { Title = "Item Design", Description = "A design-time item" };
        }
    }
}