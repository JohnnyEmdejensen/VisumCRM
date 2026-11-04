using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace VisumCRM.Entity
{
    public class Workinglog : BaseEntity
    {
        public int LogType { get; set; } // 0 = UserId, 1 = CompanyId, 2 = ContactpersonId.
        public long LogTime { get; set; }
        public long RecordId { get; set; }
        [MaxLength(1024)]
        public string Message { get; set; } = "";
        public long UserId { get; set; }
    }
}
