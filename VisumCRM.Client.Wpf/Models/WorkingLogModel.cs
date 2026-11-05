using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace VisumCRM.Client.Wpf.Models
{
    public class WorkingLogModel
    {
        public int LogType { get; set; } // 0 = UserId, 1 = CompanyId, 2 = ContactpersonId.
        public long LogTime { get; set; }
        public long RecordId { get; set; }
        public string Message { get; set; } = "";
        public long UserId { get; set; }
        public string UserName { get; set; } = "";
        public DateTime FormattetTime { get; set; }
    }
}
