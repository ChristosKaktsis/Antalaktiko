using System;
using System.Collections.Generic;
using System.Text;

namespace Antalaktiko.Models
{
    public class UserInfo
    {
        public string FName { get; set; }
        public string LName { get; set; }
        public string Email { get; set; }
        //public string Brands { get; set; }
        //public string Parts { get; set; }
        //public string Condition { get; set; }
        public List<int> Parts { get; set; }
        public List<int> Brands { get; set; }
        public List<int> Condition { get; set; }
        public string Mobile { get; set; }
        public string Cid { get; set; }
        public string Cname { get; set; }
        public string Accepted { get; set; }
        public string Active { get; set; }
        public string Sstart { get; set; }
        public string Send { get; set; }
        public string Region { get; set; }
        public string Region_name { get; set; }
        public string Terms_of_use { get; set; }
        public string Privacy_policy { get; set; }
        public string Password { get; set; }//this is only for post you wont use it whe you get items 

    }
}
