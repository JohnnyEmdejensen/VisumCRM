using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace VisumCRM.Entity
{
    public class Activity : BaseEntity
    {
        public int ActivityType { get; set; }
        public long SalespersonId { get; set; }
        public long CompanyId { get; set;}
        public long? ContactpersonId { get; set; }
        public long FollowuptypeId { get; set; }
        public long ActivityStart { get; set; }
        public long ActivityEnd { get; set; }
        [MaxLength(512)]
        public string Subject { get; set; } = "";
        public long? LocationId { get; set; }
        [MaxLength(4096)]
        public string Reason { get; set; } = "";
        [MaxLength(4096)]
        public string?Conclution { get; set; }
        public bool IsComplete { get; set; }
        public long? TimeUsed { get; set; }
        public long? TimeBilled { get; set; }

    }
}
