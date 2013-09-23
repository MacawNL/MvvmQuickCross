using System;
using MvvmQuickCross;

namespace CloudAuction.Shared.ViewModels
{
    public class MainViewModel : ViewModelBase
    {
        public enum SubView { Auction, Products, Help }

        #region Data-bindable properties and commands
        public RelayCommand ShowSubViewCommand /* Data-bindable command with parameter that calls ShowSubView(), generated with cmdp snippet. Keep on one line - see http://goo.gl/Yg6QMd for why. */ { get { if (_ShowSubViewCommand == null) _ShowSubViewCommand = new RelayCommand(ShowSubView); return _ShowSubViewCommand; } } private RelayCommand _ShowSubViewCommand;
        #endregion

        private void ShowSubView(object parameter)
        {
            var subView = (SubView)parameter;
            CloudAuctionApplication.Instance.ContinueToMain(subView);
        }
    }
}
