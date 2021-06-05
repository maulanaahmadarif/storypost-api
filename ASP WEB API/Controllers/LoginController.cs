using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web;
using System.Web.Http;
using System.Web.Http.Controllers;
using System.Web.Http.Hosting;
using Newtonsoft.Json;
using ASP_API.AccessData;
using EnCryptDecrypt;
using ASP_API.Models;
using ArsipAPI.Services;
using ASP_API.Handlers;
using System.Data;
using ArsipAPI.Controllers.Auth;
using System.Security.Claims;

namespace ASP_API.Controllers
{
    [RoutePrefix("api/login")]
    //[Authorize]
    public class LoginController : ApiController
    {
        [AllowAnonymous]
        [HttpPost]
        [Route("userlogin")]
        public IHttpActionResult UserLogin([FromBody] Users user)
        {
            string returnResult = "";

            AuthService auth = new AuthService();
            DataSet ds = new DataSet();
            ds = auth.auth(user.Username, user.Password);
            if (ds.Tables.Count > 0)
            {
                string id = ds.Tables[0].Rows[0]["ID"].ToString();
                string username = ds.Tables[0].Rows[0]["USERNAME"].ToString();
                JwtAuth jwt = new JwtAuth();
                string token = jwt.getToken(id, username);
                if (User.Identity.IsAuthenticated)
                {
                    var identity = User.Identity as ClaimsIdentity;
                    if (identity != null)
                    {
                        IEnumerable<Claim> claims = identity.Claims;
                    }
                }
                //string[] info = result.Split(';');
                //string token = "";
                //string orgname = info[5];
                //string personalname = info[7];
                //string userjob = info[12];
                //token = CryptorEngine.Encrypt(info[1], true);
                //AuthMessagehandler.SetAuthorize(user.Username, token, info[2], info[3], info[11], userjob);

                //returnResult = token + ";" + orgname + ";" + personalname + ";" + info[2] + ";" + userjob;
                return Ok<string>(token);
            }
            else
            {
                return NotFound();
            }
        }

        [Authorize]
        [HttpPost]
        [Route("testRoute")]
        public Object testRoute()
        {
            //var identity = User.Identity as ClaimsIdentity;
            //if (identity != null)
            //{
            //    IEnumerable<Claim> claims = identity.Claims;
            //    var name = claims.Where(p => p.Type == "name").FirstOrDefault()?.Value;
            //    return new
            //    {
            //        data = name
            //    };

            //}

            return "abc";
        }

        //[AllowAnonymous]
        //[HttpPost]
        //[Route("adminlogin")]
        //public IHttpActionResult AdminLogin([FromBody] UserInfo userinfo)
        //{
        //    string token = "";

        //    ConnectDB ctc = new ConnectDB();
        //    string result = ctc.CheckValidAdmin(userinfo.username, userinfo.password, "");
        //    string[] info = result.Split(';');
        //    AuthMessagehandler.SetAuthorize(userinfo.username, token, info[2], info[3], info[11], info[12]);
        //    if (result.StartsWith("SUCCESS"))
        //    {
        //        token = EnCryptDecrypt.CryptorEngine.Encrypt(info[11], true);
        //        AuthMessagehandler.SetAuthorize(userinfo.username, token, info[2], info[3], info[11], info[12]);
        //        //ReturnData data = new ReturnData(true, token);


        //        return Ok<string>(token);
        //    }
        //    else
        //    {
        //        return NotFound();
        //    }
        //}

        //[AllowAnonymous]
        //[HttpPost]
        //[Route("userlogout")]
        //public HttpResponseMessage UserLogout([FromBody] UserInfo userinfo)
        //{
        //    string tiket = EnCryptDecrypt.CryptorEngine.Decrypt(userinfo.tiket, true);
        //    ConnectDB db = new ConnectDB();
        //    db.UserLogout(tiket);

        //    //ReturnData data = new ReturnData(true, "logout berhasil");

        //    return Request.CreateResponse(HttpStatusCode.OK);

        //    //return Ok<string>("success logout");
        //}

        //[HttpPost]
        //[Route("changepswd")]
        //public HttpResponseMessage ChangePassword([FromBody] UserInfo userinfo, string modul_id)
        //{
        //    string tiket = EnCryptDecrypt.CryptorEngine.Decrypt(userinfo.tiket, true);

        //    string userName = "";
        //    string result = "";
        //    if (HttpContext.Current != null && HttpContext.Current.User != null
        //            && HttpContext.Current.User.Identity.Name != null)
        //    {
        //        UserPrincipal up = (UserPrincipal)HttpContext.Current.User;

        //        userName = up.UserName;
        //    }

        //    ConnectDB db = new ConnectDB();
        //    result = db.ChangePassword(userinfo.username, userinfo.newpassword, userinfo.password,
        //        userName, tiket, modul_id);

        //    string[] info = result.Split(';');

        //    //ReturnData data = new ReturnData(true, "logout berhasil");

        //    if (info[0] == "SUCCESS")
        //        return Request.CreateResponse(HttpStatusCode.OK);
        //    else
        //        return Request.CreateResponse(HttpStatusCode.NotFound);

        //    //return Ok<string>("success logout");
        //}

        //[HttpPost]
        //[Route("changepswdfree")]
        //public HttpResponseMessage ChangePasswordfree([FromBody] UserInfo userinfo, string modul_id)
        //{
        //    string tiket = EnCryptDecrypt.CryptorEngine.Decrypt(userinfo.tiket, true);

        //    string userName = "";
        //    string result = "";
        //    if (HttpContext.Current != null && HttpContext.Current.User != null
        //            && HttpContext.Current.User.Identity.Name != null)
        //    {
        //        UserPrincipal up = (UserPrincipal)HttpContext.Current.User;

        //        userName = up.UserName;
        //    }

        //    ConnectDB db = new ConnectDB();
        //    result = db.ChangePasswordFree(userinfo.username, userinfo.newpassword,
        //        userName, tiket, modul_id);

        //    string[] info = result.Split(';');

        //    //ReturnData data = new ReturnData(true, "logout berhasil");

        //    if (info[0] == "SUCCESS")
        //        return Request.CreateResponse(HttpStatusCode.OK);
        //    else
        //        return Request.CreateResponse(HttpStatusCode.NotFound);

        //    //return Ok<string>("success logout");
        //}


        // GET api/<controller>
        public IEnumerable<string> Get()
        {
            return new string[] { "value1", "value2" };
        }

        // GET api/<controller>/5
        public string Get(int id)
        {
            return "value";
        }

        //// POST api/<controller>
        //public void Post([FromBody]string value)
        //{
        //}

        // PUT api/<controller>/5
        public void Put(int id, [FromBody]string value)
        {

        }

        // DELETE api/<controller>/5
        public void Delete(int id)
        {
        }

    }
}