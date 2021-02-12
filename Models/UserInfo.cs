using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SmartsoftAPI.Models
{
    public class UserInfo
    {
        public string username { get; set; }
        public string password { get; set; }
        public string newpassword { get; set; }
        public string confirmpassword { get; set; }
        public string tiket { get; set; }
    }
}