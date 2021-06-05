using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using ASP_API.AccessData;
using ASP_API.Common;

namespace SmartSoft.AppLibs
{
    public class LibHttp
    {
        public static string HttpContextUserName()
        {
            string username = "";

            if (HttpContext.Current != null && HttpContext.Current.User != null
                    && HttpContext.Current.User.Identity.Name != null)
            {
                UserPrincipal up = (UserPrincipal)HttpContext.Current.User;

                username = up.UserName;
            }

            return username;
        }

        public static string HttpContextUserTiket()
        {
            string tiket = "";

            if (HttpContext.Current != null && HttpContext.Current.User != null
                    && HttpContext.Current.User.Identity.Name != null)
            {
                UserPrincipal up = (UserPrincipal)HttpContext.Current.User;

                tiket = up.Tiket;
            }

            return tiket;
        }

        public static string HttpContextUserUnitId()
        {
            string uk_id = "";

            if (HttpContext.Current != null && HttpContext.Current.User != null
                    && HttpContext.Current.User.Identity.Name != null)
            {
                UserPrincipal up = (UserPrincipal)HttpContext.Current.User;

                uk_id = up.UkId;
            }

            return uk_id;
        }

        public static string HttpContextUserSatkerId()
        {
            string satker_id = "";

            if (HttpContext.Current != null && HttpContext.Current.User != null
                    && HttpContext.Current.User.Identity.Name != null)
            {
                UserPrincipal up = (UserPrincipal)HttpContext.Current.User;

                satker_id = up.SatkerId;
            }

            return satker_id;
        }

        public static void HttpContextByToken(string token, ref string username, ref string satker_id, ref string tiket)
        {
            string[] decodedCredentials = Encoding.ASCII.GetString(Convert.FromBase64String(token)).Split(new[] { ':' });

            string decodedUserName = decodedCredentials[0], decodedPassword = decodedCredentials[1];
            decodedPassword = EnCryptDecrypt.CryptorEngine.Decrypt(decodedPassword, true);
            ConnectDB db = new ConnectDB();
            if (db.IsTiketAktif(decodedUserName, decodedPassword, ref satker_id, ref tiket))
            {
                username = decodedUserName;
                tiket = decodedPassword;
            }
        }
    }
}
