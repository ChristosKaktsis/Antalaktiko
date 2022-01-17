using System;
using System.Collections.Generic;
using System.Text;

namespace Antalaktiko.Models
{
    public class Company
    {
        public string Id { get; set; }
        public string Title { get; set; }
        public CompanyInfo Info { get; set; }
    }
}
