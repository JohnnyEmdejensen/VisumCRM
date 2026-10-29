using System;
using System.Buffers.Text;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace VisumCRM.Entity
{
    public class Location : BaseEntity
    {
        [MaxLength(128)]
        public string Street { get; set; } = "";
        [MaxLength(16)]
        public string House { get; set; } = "";
        [MaxLength(8)]
        public string? Floor { get; set; }
        [MaxLength(8)]
        public string? Door { get; set; } 
        public long PostalzoneId { get; set; }
        public double? Latitude { get; set; }
        public double? Longitude { get; set; }

        [NotMapped]
        public string? Address { get; set; }
    }
}
