using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Security.Principal;

namespace SmartSoft.Common
{
    public class UserPrincipal : IPrincipal
    {
        public string UserName { get; set; }
        public string SatkerId { get; set; }
        public string UkId { get; set; }
        public string Tiket { get; set; }
        public string UserJob { get; set; }
        public IIdentity Identity { get; set; }
        public bool IsInRole(string role)
        {
            if(role.Equals("user"))
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public UserPrincipal(string userName, string accesskey, string satker_id, string uk_id, string tiket, string userjob){
            UserName = userName;
            Identity = new GenericIdentity(accesskey);
            SatkerId = satker_id;
            UkId = uk_id;
            Tiket = tiket;
            UserJob = userjob;
        }

        public static string CurrentUserName()
        {
            string userName = "";

            if (HttpContext.Current != null && HttpContext.Current.User != null
                    && HttpContext.Current.User.Identity.Name != null)
            {
                UserPrincipal up = (UserPrincipal)HttpContext.Current.User;

                userName = up.UserName;
            }

            return userName;
        }
    }
}