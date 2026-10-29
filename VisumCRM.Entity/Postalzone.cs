using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace VisumCRM.Entity
{
    public class Postalzone : BaseEntity
    {
        [MaxLength(128)]
        public string City { get; set; } = "";
        [MaxLength(24)]
        public string Zipcode { get; set; } = "";
        [MaxLength(64)]
        public string? State { get; set; }
        public long CountryId { get; set; }
    }
}
