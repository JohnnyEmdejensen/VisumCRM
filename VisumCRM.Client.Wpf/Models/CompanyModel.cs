using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace VisumCRM.Client.Wpf.Models
{
    public class CompanyModel
    {
        public long Id { get; set; }
        public string Number { get; set; } = "";
        public string Taxnumber { get; set; } = "";
        public string Name { get; set; } = "";
        public string? Nickname { get; set; }

        public long LocationId { get; set; }
        public string? Phone { get; set; }
        public string? Phone2 { get; set; }
        public string? Email { get; set; }
        public string? Website { get; set; }

        public byte[]? Logo { get; set; }
        public long SalespersonId { get; set; }
        public long BrancheId { get; set; }
        public long TargetgroupId { get; set; }
        public string? Address { get; set; }
        public string? Zipcode { get; set; }
        public string? City { get; set; }
        public string? Country { get; set; }
        public string? Branche { get; set; }
        public string? Targetgroup { get; set; }
    }
}
