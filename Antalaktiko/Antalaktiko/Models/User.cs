using System;
using System.Collections.Generic;
using System.Text;

namespace Antalaktiko.Models
{
    public class User
    {
        public string Id { get; set; }
      
        public int Error { get; set; }
        public UserInfo Data { get; set; }
    }
}
