using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace VisumCRM.Entity
{
    public class Branche : BaseEntity
    {
        [MaxLength(16)]
        public string Key { get; set; } = "";
        [MaxLength(128)]
        public string Value { get; set; } = "";

    }
}
