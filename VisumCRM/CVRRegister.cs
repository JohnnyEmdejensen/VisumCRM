using System;
using System.Collections.Generic;
using System.Text;

namespace VisumCRM
{
    public class CVRRegister
    {
        public string baseurl = "https://apicvr.dk/";

        public CVRData? GetCompany(string Taxnumber)
        {
            CVRData? result = null;
            var content = "";
            using (HttpClient client = new())
            {
                client.BaseAddress = new Uri(baseurl);
                var response = client.GetAsync("api/v1/" + Taxnumber).Result;
                content = response.Content.ReadAsStringAsync().Result;
                if (!string.IsNullOrEmpty(content))
                {
                    var data = Newtonsoft.Json.JsonConvert.DeserializeObject<CVRData>(content);
                    result = data;
                }
            }
            return result;
        }
    }
}
