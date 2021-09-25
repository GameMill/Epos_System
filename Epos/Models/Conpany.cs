using System;
using System.Collections.Generic;
using System.Text;

namespace Epos.Models
{
    public class Conpany
    {
        public int Id { get; set; }
        public string ConpanyName { get; set; }
        public string ConpanyRegNumber { get; set; } = "";
        public string ConpanyVatNumber { get; set; } = "";
        public bool IsVatRegisted { get { return ConpanyVatNumber != ""; } }
        public string FirstLine { get; set; } 
        public string County { get; set; } 
        public string Postcode { get; set; } 
        public string Country { get; set; } /
        public string PhoneNumber { get; set; } 
        public string EmailAddress { get; set; } 
        public string WebSite { get; set; } 
        public DateTime Registed { get; set; }

    }
}
