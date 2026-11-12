using System;
using System.Collections.Generic;
using System.Text;

namespace VisumCRM
{
    public class CVRData
    {
        public string? vat { get; set; } 
        public string? name { get; set; }
        public string? address { get; set; }
        public string? zipcode { get; set; }
        public string? city { get; set; }
        public string? country { get; set; }
        public bool @protected { get; set; }
        public string? phone { get; set; }
        public string? email { get; set; }
        //public string? fax { get; set; }
        public string? startdate { get; set; }
        public string? enddate { get; set; }
        public int? employees { get; set; }
        public int? industrycode { get; set; }
        public string? industrydesc { get; set; }
        public int? companycode { get; set; }
        public string? companydesc { get; set; }
        public bool bankrupt { get; set; }
        public string? status { get; set; }
        public string? faxcompanytypeshort { get; set; }
        public string? website { get; set; }

        public List<CVRPerson>? fuldt_ansvarlige { get; set; }
        public List<CVRPerson>? direktion { get; set; }
    }
}
