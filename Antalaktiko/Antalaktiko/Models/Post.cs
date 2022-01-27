using System;
using System.Collections.Generic;
using System.Text;
using Xamarin.Forms;

namespace Antalaktiko.Models
{
    public class Post
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string Author { get; set; }
        public string Status { get; set; }
        public string Date { get; set; }
        public string Comments { get; set; }
        public string author_name { get; set; }
        public string company_id { get; set; }
        public string company_name { get; set; }
        public PostInfo Info { get; set; }
        public string AuthorDesc { get; set; }
        public string TitleInfo { get; set; }
        public string Type { 
            get
            {
                if (Name == "Θέλω να αγοράσω")
                    return "Α";
                else
                    return "Π";
            }
        }
        public Color BackGround {
            get 
            {
                if (Name == "Θέλω να αγοράσω")
                    return Color.Gray;
                else
                    return Color.FromHex("#90c9f9");
            }
        }
        public Color TextColor
        {
            get
            {
                if (Name == "Θέλω να αγοράσω")
                    return Color.FromHex("#90c9f9");
                else
                    return Color.Black;
            }
        }
    }
}
