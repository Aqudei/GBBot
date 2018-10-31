using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GMB_And_Selenium.Models
{
    class ProjectData
    {
        public int Id { get; set; }
        public int Tid { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public string RecoveryEmail { get; set; }
        public string Notation { get; set; }
        public string LeftCount { get; set; }
        public string Mode { get; set; }
        public string Status { get; set; }
        //public bool Check { get; set; }

        public string Country { get; set; }
        public string Address { get; set; }
        public string City { get; set; }
        public string Zip { get; set; }
        public string State { get; set; }
        public string Phone { get; set; }
        public string BusinessName { get; set; }
        public string Category { get; set; }
        public string URL { get; set; }
        public string HideAddress { get; set; }
        public bool HideAddressBoolean => !string.IsNullOrWhiteSpace(HideAddress) && HideAddress.ToUpper() == "YES";
        public string ServiceRadius { get; set; }
        public string Monday { get; set; }
        public string Tuesday { get; set; }
        public string Wednesday { get; set; }
        public string Thursday { get; set; }
        public string Friday { get; set; }
        public string Saturday { get; set; }
        public string Sunday { get; set; }
        public string Description { get; set; }
        public string Proxy { get; set; }
        public string LinkToSite { get; set; }
        public string PathToDebug { get; set; }
        public string Province { get; set; }
        public string StreetName { get; set; }
        public string StreetAddress { get; set; }
        public string District { get; set; }
        public string Suburn { get; set; }
        public string DeliverGoods { get; set; }
        public bool DeliverGoodsBoolean => !string.IsNullOrWhiteSpace(DeliverGoods) && DeliverGoods.ToUpper() == "YES";
    }
}
