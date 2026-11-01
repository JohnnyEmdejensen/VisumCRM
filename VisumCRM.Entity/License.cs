using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace VisumCRM.Entity
{
    public class License : BaseCompany
    {
        public bool IsInactive { get; set; }

        [NotMapped]
        public string Street { get; set; } = "";
        [NotMapped]
        public string House { get; set; } = "";
        [NotMapped]
        public string? Floor { get; set; }
        [NotMapped]
        public string? Door { get; set; } 
    }
}
