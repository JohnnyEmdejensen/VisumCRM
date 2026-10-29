using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace VisumCRM.Entity
{
    public class License : BaseCompany
    {
        public bool IsInactive { get; set; }

    }
}
