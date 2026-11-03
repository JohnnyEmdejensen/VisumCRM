using System;
using System.Collections.Generic;
using System.Text;

namespace VisumCRM.Client.Wpf.Internals
{
    public class AppShare
    {
        public string? CurrentDatabase { get; set; }
        public Entity.User? CurrentUser { get; set; }
        public List<WorkerItem> WorkerItems { get; set; } = new List<WorkerItem>();
    }
}
