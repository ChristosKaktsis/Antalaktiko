using System;
using System.Collections.Generic;
using System.Text;

namespace Antalaktiko.Models
{
    public class Comment
    {
        public int Id { get; set; }
        public int Pid { get; set; }
        public string Description { get; set; }
        public int Author { get; set; }
        public int Parent { get; set; }
        public string Date { get; set; }
        public string Author_string { get; set; }
        public string Company_id { get; set; }
        public string Company_name { get; set; }
        public Comment Child { get; set; }
    }
}
