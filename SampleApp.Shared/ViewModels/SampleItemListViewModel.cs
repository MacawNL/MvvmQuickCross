using System;
using System.Collections.ObjectModel;
using MvvmQuickCross;
using SampleApp.Shared.Models;
using SampleApp.Shared.Services;

namespace SampleApp.Shared.ViewModels
{
    public class SampleItemListViewModel : ViewModelBase
    {
        private SampleItemService _itemService;

        protected SampleItemListViewModel () { } // VS Design time support

        public SampleItemListViewModel(SampleItemService itemService)
        {
            _itemService = itemService;
            Items = new ObservableCollection<SampleItem>(itemService.GetItems());
        }

        public ObservableCollection<SampleItem> Items /* One-way data-bindable property generated with propdbcol snippet. Keep on one line - see http://goo.gl/Yg6QMd for why. */ { get { return _Items; } protected set { if (_Items != value) { _Items = value; RaisePropertyChanged(PROPERTYNAME_Items); UpdateItemsHasItems(); } } } private ObservableCollection<SampleItem> _Items; public const string PROPERTYNAME_Items = "Items";
        public bool ItemsHasItems /* One-way data-bindable property generated with propdbcol snippet. Keep on one line - see http://goo.gl/Yg6QMd for why. */ { get { return _ItemsHasItems; } protected set { if (_ItemsHasItems != value) { _ItemsHasItems = value; RaisePropertyChanged(PROPERTYNAME_ItemsHasItems); } } } private bool _ItemsHasItems; public const string PROPERTYNAME_ItemsHasItems = "ItemsHasItems";
        protected void UpdateItemsHasItems() /* Helper method generated with propdbcol snippet. Keep on one line - see http://goo.gl/Yg6QMd for why. */ { ItemsHasItems = _Items != null && _Items.Count > 0; }

        public RelayCommand ViewItemCommand /* Data-bindable command with parameter that calls ViewItem(), generated with cmdp snippet. Keep on one line - see http://goo.gl/Yg6QMd for why. */ { get { if (_ViewItemCommand == null) _ViewItemCommand = new RelayCommand(ViewItem); return _ViewItemCommand; } } private RelayCommand _ViewItemCommand;
        public RelayCommand AddItemCommand /* Data-bindable command that calls AddItem(), generated with cmd snippet. Keep on one line - see http://goo.gl/Yg6QMd for why. */ { get { if (_AddItemCommand == null) _AddItemCommand = new RelayCommand(AddItem); return _AddItemCommand; } } private RelayCommand _AddItemCommand;
        public RelayCommand RemoveItemCommand /* Data-bindable command with parameter that calls RemoveItem(), generated with cmdp snippet. Keep on one line - see http://goo.gl/Yg6QMd for why. */ { get { if (_RemoveItemCommand == null) _RemoveItemCommand = new RelayCommand(RemoveItem); return _RemoveItemCommand; } } private RelayCommand _RemoveItemCommand;

        private void ViewItem(object parameter)
        {
            var item = (SampleItem)parameter;
            SampleAppApplication.Current.ContinueToSampleItem(item);
        }

        private void AddItem()
        {
            var item = new SampleItem { Title = "new", Description = "" };
            Items.Add(item);
            SampleAppApplication.Current.ContinueToSampleItem(item);
        }

        private void RemoveItem(object parameter)
        {
            var item = (SampleItem)parameter;
            Items.Remove(item);
        }
    }
}

namespace SampleApp.Shared.ViewModels.Design
{
    public class SampleItemListViewModelDesign : SampleItemListViewModel
    {
        private static SampleItem[] _itemData = new SampleItem[] {
            new SampleItem { Title = "One Design", Description = "First design-time item" },
            new SampleItem { Title = "Two Design", Description = "Second design-time item" },
            new SampleItem { Title = "Three Design", Description = "Third design-time item" }
        };

        public SampleItemListViewModelDesign()
        {
            Items = new ObservableCollection<SampleItem>(_itemData);
        }
    }
}