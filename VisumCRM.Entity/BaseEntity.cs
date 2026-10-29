using System;
using System.Collections.Generic;
using System.Text;

namespace VisumCRM.Entity
{
    public class BaseEntity
    {
        public long Id { get; set; }
        public long Created { get; set; }
        public long? Updated { get; set; }
        public bool IsDeleted { get; set; }
    }
}
