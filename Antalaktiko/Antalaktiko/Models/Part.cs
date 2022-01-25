using SQLite;
using System;
using System.Collections.Generic;
using System.Text;

namespace Antalaktiko.Models
{
    public class Part : BaseItem
    {
        [PrimaryKey]
        public int Id { get; set; }
        public string Title { get; set; }
    }
}
