using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.SqlClient;
using System.Data;
using ASP_API.Models.CuratedImages;
using Newtonsoft.Json;
using System.IO;
using System.Drawing;
using System.Net;

namespace ASP_API.AccessData.CuratedImage
{
    public class DataCuratedImages
	{
        public CuratedImagesReturn ListCuratedImages()

		{
            ImageHandle imagehandle = new ImageHandle();
            //string savedir = imagehandle.ImagesFolder() + "Gallery/ImageFull?image=" + "\\";

            string savedir = imagehandle.ImagesFolder() + "Gallery/ImageFull?image=";
            string thumbdir = imagehandle.ImagesFolder() + "Gallery/Image?image=";
            string webapp = imagehandle.linkStoryPost() + "Post.aspx?id=";

			string json = "";
            CuratedImages[] list = new CuratedImages[0];
            CuratedImagesReturn returndata = new CuratedImagesReturn();
			DataSet ds = new DataSet();

			SqlConnection conn = new SqlConnection(ConnectDB.dbConn());
			try
			{
				conn.Open();
                string sCommand = "sp_curatedimages_gridview_api_cms";
				SqlDataAdapter da = new SqlDataAdapter(sCommand, conn);
				da.SelectCommand.CommandType = CommandType.StoredProcedure;
                da.SelectCommand.Parameters.AddWithValue("@savedir", savedir);
                da.SelectCommand.Parameters.AddWithValue("@thumbdir", thumbdir);
                da.SelectCommand.Parameters.AddWithValue("@webapp", webapp);
				da.Fill(ds, "records");
			}
			catch (Exception)
			{
				if (conn.State != ConnectionState.Closed)
					conn.Close();
			}
			finally
			{
				conn.Close();
			}
			if (ds.Tables.Count > 0)
			{
				if (ds.Tables[0].Rows.Count > 0)
				{
					json = ConnectDB.DataTableToJSON(ds.Tables[0]);
                    list = JsonConvert.DeserializeObject<CuratedImages[]>(json);

                    foreach (var listTable in list)
                    {
                        string image = @"" + listTable.ImageURL;
                        byte[] imageData = new WebClient().DownloadData(image);
                        MemoryStream imgStream = new MemoryStream(imageData);
                        Image img = Image.FromStream(imgStream);

                        int x = (int)img.PhysicalDimension.Width;
                        int y = (int)img.PhysicalDimension.Height;

                        listTable.Ratio = string.Format("{0}:{1}", x / imagehandle.GCD(x, y), y / imagehandle.GCD(x, y));
                        listTable.Width = x.ToString();
                        listTable.Height = y.ToString();

                        //using (FileStream stream = new FileStream(image, FileMode.Open, FileAccess.Read))
                        //{
                        //    using (Image tif = Image.FromStream(stream, false, false))
                        //    {
                        //        int x = (int)tif.PhysicalDimension.Width;
                        //        int y = (int)tif.PhysicalDimension.Height;

                        //        listTable.Ratio = string.Format("{0}:{1}", x / imagehandle.GCD(x, y), y / imagehandle.GCD(x, y));
                        //        listTable.Width = x.ToString();
                        //        listTable.Height = y.ToString();
                        //    }
                        //}
                    }

					returndata.data = list;
				}
			}
			return returndata;
		}

        private object GetFile(string path)
        {
            throw new NotImplementedException();
        }
	}
}