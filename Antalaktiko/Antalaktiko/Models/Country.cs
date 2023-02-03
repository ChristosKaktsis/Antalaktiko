using System;
using System.Collections.Generic;
using System.Text;

namespace Antalaktiko.Models
{
    public class Country
    {
        public string Name { get; set; }
        public List<Region> Regions { get; set; }
    }
}
