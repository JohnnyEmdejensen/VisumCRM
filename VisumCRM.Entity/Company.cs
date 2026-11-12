using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace VisumCRM.Entity
{
public class Company : BaseCompany
    {
        public byte[]? Logo { get; set; }
        public long SalespersonId { get; set; }
        public long BrancheId { get; set; }
        public long TargetgroupId { get; set; }
        public long? Founded { get; set; }
        public int? Employees { get; set; }

        [NotMapped]
        public string VisualFundedDate { get; set; } = "";

        public void SetVisualDates()
        {
            if (Founded.HasValue)
            {
                var str = Founded.Value.ToString();
                var date = "";
                if (str.Length >= 4)
                    date = str.Substring(0, 4);
                if (str.Length >= 6)
                    date += "-" + str.Substring(4, 2);
                if (str.Length >= 8)
                    date += "-" + str.Substring(6, 2);
                VisualFundedDate = date;
            }
        }
    }
}
