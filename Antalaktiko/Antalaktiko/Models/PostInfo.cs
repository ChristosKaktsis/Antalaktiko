using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace Antalaktiko.Models
{
    public class PostInfo
    {
        [JsonProperty("pid")]
        public string Pid { get; set; }
        [JsonProperty("desc")]
        public string Desc { get; set; }
        [JsonProperty("date")]
        public string Date { get; set; }
        [JsonProperty("user_id")]
        public string User_id { get; set; }
        [JsonProperty("company_id")]
        public string Company_id { get; set; }
        [JsonProperty("company_name")]
        public string Company_name { get; set; }
        [JsonProperty("fname")]
        public string Fname { get; set; }
        [JsonProperty("lname")]
        public string Lname { get; set; }
        [JsonProperty("vtype")]
        public string Vtype { get; set; }
        [JsonProperty("fuel")]
        public string Fuel { get; set; }
        [JsonProperty("brand")]
        public string Brand { get; set; }
        [JsonProperty("brand_name")]
        public string Brand_name { get; set; }
        [JsonProperty("model")]
        public string Model { get; set; }
        [JsonProperty("model_name")]
        public string Model_name { get; set; }
        [JsonProperty("date_from")]
        public string Date_from { get; set; }
        [JsonProperty("date_to")]
        public string Date_to { get; set; }
        [JsonProperty("cat")]
        public string Cat { get; set; }
        [JsonProperty("cat_name")]
        public string Cat_name { get; set; }
        [JsonProperty("part_number")]
        public string Part_number { get; set; }
        [JsonProperty("engine_code")]
        public string Engine_code { get; set; }
        [JsonProperty("vin")]
        public string Vin { get; set; }
        [JsonProperty("doors")]
        public string Doors { get; set; }
        [JsonProperty("qnt")]
        public string Qnt { get; set; }
        [JsonProperty("price")]
        public string Price { get; set; }
        [JsonProperty("state")]
        public List<int> State { get; set; }
        [JsonProperty("state_string")]
        public List<string> State_string { get; set; }
        [JsonProperty("gtype")]
        public string Gtype { get; set; }
        [JsonProperty("gid")]
        public string Gid { get; set; }
        [JsonProperty("read")]
        public string Read { get; set; }
        [JsonProperty("calling_action")]
        public string Calling_action { get; set; }
        [JsonProperty("gal")]
        public string Gal { get; set; }//photo id
        [JsonProperty("item_State")]
        public string Item_State { get; set; }
        [JsonProperty("images")]
        public List<Image> Images { get; set; }
        //public string Item_State_Name 
        //{ 
        //    get 
        //    {
        //        string state = string.Empty;
        //        switch (Item_State)
        //        {
        //            case "1":
        //                state = "Ιμιτασιόν";
        //                break;
        //            case "2":
        //                state = "Γνήσιο";
        //                break;
        //            case "3":
        //                state = "Μεταχειρισμένο";
        //                break;
        //            case "4":
        //                state = "Ανακατασκευασμένο";
        //                break;
        //            default:
        //                state = "Όλα";
        //                break;
        //        }
        //        return state;
        //    } 
        //}
    }
}
