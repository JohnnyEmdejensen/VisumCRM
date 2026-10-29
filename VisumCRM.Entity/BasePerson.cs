using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace VisumCRM.Entity
{
    public class BasePerson : BaseEntity
    {
        [MaxLength(128)]
        public string Firstname { get; set; } = "";
        [MaxLength(128)]
        public string? Middlename { get; set; }
        [MaxLength(128)]
        public string Surname { get; set; } = "";
        [MaxLength(16)]
        public string? Phone { get; set; }
        [MaxLength(16)]
        public string? Mobile { get; set; }
        [MaxLength(512)]
        public string? Email { get; set; }
        public long? LocationId { get; set; }
        public byte[]? Picture { get; set; }
        public long? Birthday { get; set; }

        [NotMapped]
        public string? Fullname { get; set; }
    }
}
