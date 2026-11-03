using System;
using System.Collections.Generic;
using System.Text;

namespace VisumCRM.Entity
{
public class Company : BaseCompany
    {
        public byte[]? Logo { get; set; }
        public long SalespersonId { get; set; }
        public long BrancheId { get; set; }
        public long TargetgroupId { get; set; }
    }
}
