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
        public string Author_Name { get; set; }
        public int Parent { get; set; }
        public string Date { get; set; }
    }
}
