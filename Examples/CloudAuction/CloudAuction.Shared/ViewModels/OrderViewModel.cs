using System;
using System.Collections.ObjectModel;
using MvvmQuickCross;

namespace CloudAuction.Shared.ViewModels
{
    public class OrderViewModel : ViewModelBase
    {
        public OrderViewModel()
        {
            // TODO: pass any services that this model needs as contructor parameters. 
        }

        public void Initialize(int lotId)
        {
            // TODO: Implement Initialize()
        }

        #region Data-bindable properties and commands
        public string[] DeliveryLocations /* One-way data-bindable property generated with propdb1 snippet. Keep on one line - see http://goo.gl/Yg6QMd for why. */ { get { return _DeliveryLocations; } protected set { if (_DeliveryLocations != value) { _DeliveryLocations = value; RaisePropertyChanged(PROPERTYNAME_DeliveryLocations); } } } private string[] _DeliveryLocations; public const string PROPERTYNAME_DeliveryLocations = "DeliveryLocations";
        public string DeliveryLocation /* Two-way data-bindable property generated with propdb2 snippet. Keep on one line - see http://goo.gl/Yg6QMd for why. */ { get { return _DeliveryLocation; } set { if (_DeliveryLocation != value) { _DeliveryLocation = value; RaisePropertyChanged(PROPERTYNAME_DeliveryLocation); } } } private string _DeliveryLocation; public const string PROPERTYNAME_DeliveryLocation = "DeliveryLocation";
        public string[] Titles /* One-way data-bindable property generated with propdb1 snippet. Keep on one line - see http://goo.gl/Yg6QMd for why. */ { get { return _Titles; } protected set { if (_Titles != value) { _Titles = value; RaisePropertyChanged(PROPERTYNAME_Titles); } } } private string[] _Titles; public const string PROPERTYNAME_Titles = "Titles";
        public string Title /* Two-way data-bindable property generated with propdb2 snippet. Keep on one line - see http://goo.gl/Yg6QMd for why. */ { get { return _Title; } set { if (_Title != value) { _Title = value; RaisePropertyChanged(PROPERTYNAME_Title); } } } private string _Title; public const string PROPERTYNAME_Title = "Title";
        public string FirstName /* Two-way data-bindable property generated with propdb2 snippet. Keep on one line - see http://goo.gl/Yg6QMd for why. */ { get { return _FirstName; } set { if (_FirstName != value) { _FirstName = value; RaisePropertyChanged(PROPERTYNAME_FirstName); } } } private string _FirstName; public const string PROPERTYNAME_FirstName = "FirstName";
        public string MiddleName /* Two-way data-bindable property generated with propdb2 snippet. Keep on one line - see http://goo.gl/Yg6QMd for why. */ { get { return _MiddleName; } set { if (_MiddleName != value) { _MiddleName = value; RaisePropertyChanged(PROPERTYNAME_MiddleName); } } } private string _MiddleName; public const string PROPERTYNAME_MiddleName = "MiddleName";
        public string LastName /* Two-way data-bindable property generated with propdb2 snippet. Keep on one line - see http://goo.gl/Yg6QMd for why. */ { get { return _LastName; } set { if (_LastName != value) { _LastName = value; RaisePropertyChanged(PROPERTYNAME_LastName); } } } private string _LastName; public const string PROPERTYNAME_LastName = "LastName";
        public string Street /* Two-way data-bindable property generated with propdb2 snippet. Keep on one line - see http://goo.gl/Yg6QMd for why. */ { get { return _Street; } set { if (_Street != value) { _Street = value; RaisePropertyChanged(PROPERTYNAME_Street); } } } private string _Street; public const string PROPERTYNAME_Street = "Street";
        public string Zip /* Two-way data-bindable property generated with propdb2 snippet. Keep on one line - see http://goo.gl/Yg6QMd for why. */ { get { return _Zip; } set { if (_Zip != value) { _Zip = value; RaisePropertyChanged(PROPERTYNAME_Zip); } } } private string _Zip; public const string PROPERTYNAME_Zip = "Zip";
        public string City /* Two-way data-bindable property generated with propdb2 snippet. Keep on one line - see http://goo.gl/Yg6QMd for why. */ { get { return _City; } set { if (_City != value) { _City = value; RaisePropertyChanged(PROPERTYNAME_City); } } } private string _City; public const string PROPERTYNAME_City = "City";
        public string Country /* Two-way data-bindable property generated with propdb2 snippet. Keep on one line - see http://goo.gl/Yg6QMd for why. */ { get { return _Country; } set { if (_Country != value) { _Country = value; RaisePropertyChanged(PROPERTYNAME_Country); } } } private string _Country; public const string PROPERTYNAME_Country = "Country";
        public string Email /* Two-way data-bindable property generated with propdb2 snippet. Keep on one line - see http://goo.gl/Yg6QMd for why. */ { get { return _Email; } set { if (_Email != value) { _Email = value; RaisePropertyChanged(PROPERTYNAME_Email); } } } private string _Email; public const string PROPERTYNAME_Email = "Email";
        public string Mobile /* Two-way data-bindable property generated with propdb2 snippet. Keep on one line - see http://goo.gl/Yg6QMd for why. */ { get { return _Mobile; } set { if (_Mobile != value) { _Mobile = value; RaisePropertyChanged(PROPERTYNAME_Mobile); } } } private string _Mobile; public const string PROPERTYNAME_Mobile = "Mobile";
        public string Phone /* Two-way data-bindable property generated with propdb2 snippet. Keep on one line - see http://goo.gl/Yg6QMd for why. */ { get { return _Phone; } set { if (_Phone != value) { _Phone = value; RaisePropertyChanged(PROPERTYNAME_Phone); } } } private string _Phone; public const string PROPERTYNAME_Phone = "Phone";

        public RelayCommand CancelCommand /* Data-bindable command that calls Cancel(), generated with cmd snippet. Keep on one line - see http://goo.gl/Yg6QMd for why. */ { get { if (_CancelCommand == null) _CancelCommand = new RelayCommand(Cancel); return _CancelCommand; } } private RelayCommand _CancelCommand;
        public RelayCommand ConfirmCommand /* Data-bindable command that calls Confirm(), generated with cmd snippet. Keep on one line - see http://goo.gl/Yg6QMd for why. */ { get { if (_ConfirmCommand == null) _ConfirmCommand = new RelayCommand(Confirm); return _ConfirmCommand; } } private RelayCommand _ConfirmCommand;
        #endregion

        private void Cancel()
        {
            CloudAuctionApplication.Instance.ContinueToAuction();
        }

        private void Confirm()
        {
            throw new NotImplementedException(); // TODO: Implement Confirm()
        }
    }
}

// Design-time data support
#if DEBUG
namespace CloudAuction.Shared.ViewModels.Design
{
    public class OrderViewModelDesign : OrderViewModel
    {
        public OrderViewModelDesign()
        {
            DeliveryLocations = new string[] { "At home", "Pickup" };
            DeliveryLocation = DeliveryLocations[0];
            Titles = new string[] { "Mr.", "Ms." };
            Title = Titles[0];
            FirstName = "First name";
            MiddleName = "Mid";
            LastName = "Last name";
            Street = "Street";
            Zip = "Zip";
            City = "City";
            Country = "Country";
            Email = "email@example.com";
            Mobile = "01 234 567 89";
            Phone = "555 234 567";
        }
    }
}
#endif
