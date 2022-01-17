using System;
using System.Collections.Generic;
using System.Text;

namespace Antalaktiko.Models
{
    public class User
    {
        public string Id { get; set; }
        public string Email { get; set; }
        public string First_Name { get; set; }
        public string Last_Name { get; set; }
        public UserInfo Info { get; set; }
    }
}
