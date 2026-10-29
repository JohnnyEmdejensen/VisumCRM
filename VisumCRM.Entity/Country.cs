using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace VisumCRM.Entity
{
    public class Country : BaseEntity
    {
        [MaxLength(128)]
        public string Name { get; set; } = "";
        [MaxLength(16)]
        public string IsoCode { get; set; } = "";
        public int Number { get; set; }
        public byte[]? Flag { get; set; } 

    }
}
