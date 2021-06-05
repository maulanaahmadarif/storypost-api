using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;

namespace ASP_API.Common
{
    public class Config
    {
        public string jwtKey()
        {
            return ConfigurationManager.AppSettings["jwtKey"];
        }
    }
}