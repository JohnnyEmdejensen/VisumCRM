using System;
using System.Collections.Generic;
using System.Text;

namespace VisumCRM.Client.Wpf.Models
{
    public class ActivityModel : Entity.Activity
    {
        public string? CompanyName { get; set; }
        public string? ContactpersonName { get; set; }
        public string? SalespersonName { get; set; }
        public string? FolowuptypeName { get; set; }
        public string? Address { get; set; }
        public string? Zipcode { get; set; }
        public string? City { get; set; }
        public string? Country { get; set; }

        public string ActivityStartText { get; set; } = "";
        public string ActivityEndText { get; set; } = "";
    }
}
