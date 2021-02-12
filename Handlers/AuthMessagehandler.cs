using System;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading;
using System.Web;
using SmartSoft.Common;
using System.Collections.Specialized;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using SmartsoftAPI.Models;
using Newtonsoft.Json;
using ASP_API.AccessData;
using EnCryptDecrypt;

namespace SmartsoftAPI.Handlers
{
    public class AuthMessagehandler : DelegatingHandler
    {

        private string _username;
        private string _accesskey;
        private string _satkerid;
        private string _ukid;
        private string _tiket;
        private string _userjob;

        //Capturing the incoming request by overriding the SendAsync method
        protected override System.Threading.Tasks.Task<HttpResponseMessage> SendAsync(
           HttpRequestMessage request,
           CancellationToken cancellationToken)
        {

            //if the credentials are validated, set CurrentPrincipal and Current.User
            if (ValidateCredentials(request.Headers.Authorization))
            {
                Thread.CurrentPrincipal = new UserPrincipal(_username, _accesskey, _satkerid, _ukid, _tiket, _userjob);
                HttpContext.Current.User = new UserPrincipal(_username, _accesskey, _satkerid, _ukid, _tiket, _userjob);
            }
                        
            //Execute base.SendAsync to execute default actions and once it is completed, 
            //capture the response object and add WWW-Authenticate header if the request was marked as unauthorized.
            return base.SendAsync(request, cancellationToken)
               .ContinueWith(task =>
               {
                   HttpResponseMessage response = task.Result;
                   if (response.StatusCode == HttpStatusCode.Unauthorized
                       && !response.Headers.Contains("WWW-Authenticate"))
                   {
                       response.Headers.Add("WWW-Authenticate", "Basic");
                   }
                   return response;
               });
        }


        static public void SetAuthorize(string username, string acceskey, string satkerid, 
            string ukid, string tiket, string userjob)
        {
            Thread.CurrentPrincipal = new UserPrincipal(username, acceskey, satkerid, ukid, tiket, userjob);
            HttpContext.Current.User = new UserPrincipal(username, acceskey, satkerid, ukid, tiket, userjob);
        }

        //Method to validate credentials from Authorization header value
        private bool ValidateCredentials(AuthenticationHeaderValue authenticationHeaderVal)
        {
            if (authenticationHeaderVal != null && !String.IsNullOrEmpty(authenticationHeaderVal.Parameter))
            {

                string[] decodedCredentials = Encoding.ASCII.GetString(
                                                Convert.FromBase64String(
                                                    authenticationHeaderVal.Parameter))
                                                    .Split(new[] { ':' });

                //now decodedCredentials[0] will contain username and decodedCredentials[2] will contain password.
                //You need to implement your business logic to verify credentials here. For simplicity, we are hardcoding user name and password here.
                string decodedUserName = decodedCredentials[0], decodedPassword = decodedCredentials[1];
                decodedPassword = EnCryptDecrypt.CryptorEngine.Decrypt(decodedPassword, true);
                ConnectDB db = new ConnectDB();
                if (db.IsTiketAktif(decodedUserName, decodedPassword, ref _satkerid, ref _ukid))
                {
                    _username = decodedUserName;
                    _accesskey = decodedPassword;
                    _tiket = decodedPassword;
                    _userjob = "";

                    return true;
                }
            }
            return false;//request not authenticated.
        }

        static public bool ValidateCredentials(string authString, ref string username)
        {
            if (authString != null && !String.IsNullOrEmpty(authString))
            {

                string[] decodedCredentials = Encoding.ASCII.GetString(
                                                Convert.FromBase64String(
                                                    authString))
                                                    .Split(new[] { ':' });

                //now decodedCredentials[0] will contain username and decodedCredentials[2] will contain password.
                //You need to implement your business logic to verify credentials here. For simplicity, we are hardcoding user name and password here.
                string decodedUserName = decodedCredentials[0], decodedPassword = decodedCredentials[1];
                decodedPassword = EnCryptDecrypt.CryptorEngine.Decrypt(decodedPassword, true);
                ConnectDB db = new ConnectDB();
                string _satkerid = "";
                string _ukid = "";
                if (db.IsTiketAktif(decodedUserName, decodedPassword, ref _satkerid, ref _ukid))
                {
                    username = decodedUserName;
                    return true;
                }
            }
            return false;//request not authenticated.
        }

    }
}