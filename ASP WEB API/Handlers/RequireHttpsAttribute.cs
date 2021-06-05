using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net.Http;
using System.Web.Http.Filters;
using System.Web.Http.Controllers;

namespace SmartsoftAPI.Handlers
{
    //public class RequireHttpsAttribute : AuthorizationFilterAttribute
    //{
    //    public override void OnAuthorization(HttpActionContext actionContext)
    //    {
    //        if (actionContext.Request.RequestUri.Scheme != Uri.UriSchemeHttps)
    //        {
    //            actionContext.Response = new HttpResponseMessage(System.Net.HttpStatusCode.Forbidden)
    //            {
    //                ReasonPhrase = "HTTPS Required"
    //            };
    //        }
    //        else
    //        {
    //            base.OnAuthorization(actionContext);
    //        }
    //    }
    //}
}
