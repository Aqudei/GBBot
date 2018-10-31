using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.AccessControl;
using System.Text;
using System.Threading.Tasks;
using GMB_And_Selenium.Models;

namespace GMB_And_Selenium.ViewModels
{
    class ProjectItemViewModel : Caliburn.Micro.PropertyChangedBase
    {

        // Todo : NotifyChange

        private int _id;
        private int _tid;
        private string _email;
        private string _password;
        private string _recoveryEmail;
        private string _notation;
        private string _leftCount;
        private string _mode;
        private string _status;
        private bool _check;

        private string _country;

        public string Country
        {
            get { return _country; }
            set { _country = value; }
        }

        private string _zip;

        public string Zip
        {
            get { return _zip; }
            set { _zip = value; }
        }

        private string _state;

        public string State
        {
            get { return _state; }
            set { _state = value; }
        }

        private string _city;

        public string City
        {
            get { return _city; }
            set { _city = value; }
        }

        private string _businessName;
        private string _streetAddress;
        private bool _hideAddressBoolean;
        private bool _deliverGoodsBoolean;
        private string _phone;

        public string BusinessName
        {
            get { return _businessName; }
            set { Set(ref _businessName, value); }
        }


        //public int Id { get => _id; set => _id = value; }
        //public int Tid { get => _tid; set => _tid = value; }
        public string Email { get => _email; set => _email = value; }
        public string Password { get => _password; set => _password = value; }
        public string RecoveryEmail { get => _recoveryEmail; set => _recoveryEmail = value; }
        public string Notation { get => _notation; set => _notation = value; }
        public string LeftCount { get => _leftCount; set => _leftCount = value; }
        public string Mode { get => _mode; set => _mode = value; }
        public string Status { get => _status; set => Set(ref _status, value); }
        //public bool Check { get => _check; set => Set(ref _check , value); }
        public string StreetAddress { get => _streetAddress; set => Set(ref _streetAddress, value); }

        public bool DeliverGoodsBoolean
        {
            get => _deliverGoodsBoolean;
            set => Set(ref _deliverGoodsBoolean, value);
        }

        public string Phone
        {
            get => _phone;
            set => Set(ref _phone, value);
        }

        public bool HideAddressBoolean
        {
            get => _hideAddressBoolean;
            set => Set(ref _hideAddressBoolean, value);
        }

        public ProjectData ProjectData { get; set; }

        public bool IsFitForBot()
        {
            return !string.IsNullOrWhiteSpace(Email) &&
                   !string.IsNullOrWhiteSpace(Country) &&
                   !string.IsNullOrWhiteSpace(Password) &&
                   !string.IsNullOrWhiteSpace(City) &&
                   !string.IsNullOrWhiteSpace(Zip) &&
                   !string.IsNullOrWhiteSpace(State) &&
                   !string.IsNullOrWhiteSpace(BusinessName) &&
                   !string.IsNullOrWhiteSpace(StreetAddress);
        }
    }
}
