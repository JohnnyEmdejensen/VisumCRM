using System;
using System.Collections.Generic;
using System.Text;

namespace VisumCRM.Entity
{
    public class Contactperson : BasePerson
    {
        public long SalespersonId { get; set; }
        public long EmployeerId { get; set; }

    }
}
