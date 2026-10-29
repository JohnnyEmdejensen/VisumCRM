using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace VisumCRM.Entity
{
    public class BaseCompany : BaseEntity
    {
        [MaxLength(24)]
        public string Number { get; set; } = "";
        [MaxLength(24)]
        public string Taxnumber { get; set; } = "";
        [MaxLength(128)]
        public string Name { get; set; } = "";
        [MaxLength(64)]
        public string? Nickname { get; set; }

        public long LocationId { get; set; }
        [MaxLength(16)]
        public string? Phone { get; set; }
        [MaxLength(16)]
        public string? Phone2 { get; set; }
        [MaxLength(512)]
        public string? Email { get; set; }
        [MaxLength(1024)]
        public string? Website { get; set; }
    }
}
