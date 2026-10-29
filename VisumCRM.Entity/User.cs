using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace VisumCRM.Entity
{
    public class User : BasePerson
    {
        [MaxLength(512)]
        public string Login { get; set; } = "";
        [MaxLength(128)]
        public string Password { get; set; } = "";
        public long? LastVisit { get; set; }
        public long? TimeUsed { get; set; }
        public int SecurityLevel { get; set; }
        public bool IsInactive { get; set; }
    }
}
