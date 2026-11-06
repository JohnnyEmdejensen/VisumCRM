using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace VisumCRM.Client.Wpf.Models
{
    public class ContactpersonModel
    {
        public long Id { get; set; }
        public string Firstname { get; set; } = "";
        public string? Middlename { get; set; }
        public string Surname { get; set; } = "";
        public string? Phone { get; set; }
        public string? Mobile { get; set; }
        public string? Email { get; set; }
        public long? LocationId { get; set; }
        public byte[]? Picture { get; set; }
        public long? Birthday { get; set; }
        public long SalespersonId { get; set; }
        public long EmployeerId { get; set; }

        public string? SalespersonName { get; set; }
        public string? EmployeeName { get; set; }
        public string? Address { get; set; }
        public string? Zipcode { get; set; }
        public string? City { get; set; }
        public string? Country { get; set; }

        public string? Fullname { get; set; }
    }
}
