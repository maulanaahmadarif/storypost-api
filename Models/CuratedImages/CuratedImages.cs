using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Configuration;

namespace ASP_API.Models.CuratedImages
{
    public class CuratedImages
	{
        public string OrderNumber { set; get; }        
        public string ImageURL { set; get; }
        public string ThumbURL { set; get; }
        public string PostURL { set; get; }
        public string Username { set; get; }
        public string Ratio { set; get; }
        public string Width { set; get; }
        public string Height { set; get; }
	}

    public class CuratedImagesReturn
	{
        public CuratedImages[] data { get; set; }
	}

    public class CuratedImagesParams
	{
        public string tahun { get; set; }
        public string perihal { get; set; }
       
		public int currpage { get; set; }
		public int pagesize { get; set; }
	}

    public class ImageHandle
    {
        public string ImagesFolder()
        {
            return ConfigurationManager.AppSettings["dirImages"].ToString();
        }

        public string linkStoryPost()
        {
            return ConfigurationManager.AppSettings["linkStoryPost"].ToString();
        }

        public int GCD(int a, int b)
        {
            int Remainder;

            while (b != 0)
            {
                Remainder = a % b;
                a = b;
                b = Remainder;
            }

            return a;
        }
    }
}