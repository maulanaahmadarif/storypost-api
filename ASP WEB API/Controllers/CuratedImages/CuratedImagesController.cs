using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web;
using System.Web.Http;
using System.Data.SqlClient;
using Newtonsoft.Json;
using ASP_API.Models.CuratedImages;
using ASP_API.AccessData.CuratedImage;

namespace ASP_API.Controllers.CuratedImages
{
    //[RoutePrefix("api/pd1srtin")]
    public class CuratedImagesController : ApiController
	{
        [AllowAnonymous]
        [HttpGet]
        [Route("api/CuratedImages")]
        public CuratedImagesReturn GetListCuratedImages()
		{
            DataCuratedImages data = new DataCuratedImages();
            return data.ListCuratedImages();
		}
	}
}