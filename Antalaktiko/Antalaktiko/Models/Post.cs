using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;
using Xamarin.Forms;

namespace Antalaktiko.Models
{
    public class Post
    {
        [JsonProperty("id")]
        public string Id { get; set; }
        [JsonProperty("type")]
        public string Type { get; set; }
        [JsonProperty("status")]
        public string Status { get; set; }
        [JsonProperty("date")]
        public string Date { get; set; }
        [JsonProperty("comments")]
        public string Comments { get; set; }
        [JsonProperty("data")]
        public PostInfo Data { get; set; }
        [JsonIgnore]
        public string Type_Letter { 
            get
            {
                if (Type == "Θέλω να αγοράσω")
                    return "Α";
                else
                    return "Π";
            }
        }
        [JsonIgnore]
        public Color BackGround {
            get 
            {
                if (Type == "Θέλω να αγοράσω")
                    return Color.Gray;
                else
                    return Color.FromHex("#90c9f9");
            }
        }
        [JsonIgnore]
        public Color TextColor
        {
            get
            {
                if (Type == "Θέλω να αγοράσω")
                    return Color.FromHex("#90c9f9");
                else
                    return Color.Black;
            }
        }
    }
}
