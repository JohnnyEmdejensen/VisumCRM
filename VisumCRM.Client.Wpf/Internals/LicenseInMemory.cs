using System;
using System.Collections.Generic;
using System.Text;

namespace VisumCRM.Client.Wpf.Internals
{
    public class LicenseInMemory
    {
        public string Type { get; set; } = "";
        public string Name { get; set; } = "";
        public string Company { get; set; } = "";
        public string Street { get; set; } = "";
        public string House { get; set; } = "";
        public string Floor { get; set; } = "";
        public string Door { get; set; } = "";
        public string Zipcode { get; set; } = "";
        public string City { get; set; } = "";
        public string Country { get; set; } = "";
        public string Phone { get; set; } = "";
        public string Email { get; set; } = "";
        public Guid Key { get; set; }

    }
}
