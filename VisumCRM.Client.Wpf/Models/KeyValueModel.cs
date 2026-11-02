using System;
using System.Collections.Generic;
using System.Text;

namespace VisumCRM.Client.Wpf.Models
{
    public class KeyValueModel
    {
        public long Id { get; set; }
        public string Key { get; set; } = "";
        public string Value { get; set; } = "";
    }
}
