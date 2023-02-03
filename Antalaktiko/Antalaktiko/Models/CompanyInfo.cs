using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace Antalaktiko.Models
{
    public class CompanyInfo
    {
        [JsonProperty("cname")]
        public string Cname { get; set; }
        [JsonProperty("website")]
        public string Website { get; set; }
        [JsonProperty("cmail")]
        public string Company_Email { get; set; }
        [JsonProperty("vat")]
        public string Vat { get; set; }
        [JsonProperty("country")]
        public string Country { get; set; }
        [JsonProperty("address")]
        public string Address { get; set; }
        [JsonProperty("pc")]
        public string Post_Code { get; set; }
        [JsonProperty("phone")]
        public string Phone { get; set; }
        [JsonProperty("mobile")]
        public string Mobile { get; set; }
        [JsonProperty("sstart")]
        public string Subscription_Started { get; set; }
        [JsonProperty("send")]
        public string Subscription_Ends { get; set; }
        [JsonProperty("seller")]
        public string Seller { get; set; }
        [JsonProperty("cjob")]
        public string Job_id { get; set; }
        [JsonProperty("cjobn")]
        public string Job_name { get; set; }
        [JsonProperty("region")]
        public string Region { get; set; }
        [JsonProperty("region_name")]
        public string Region_name { get; set; }
        [JsonProperty("accepted")]
        public string Accepted { get; set; }
        [JsonProperty("active")]
        public string Active { get; set; }
        [JsonProperty("users")]
        public List<string> Users { get; set; }
      
    }
}
