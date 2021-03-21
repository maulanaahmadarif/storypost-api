using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;

namespace ArsipAPI.Services
{
    public class AuthService
    {
        static public string eAssistConn()
        {
            return ConfigurationManager.ConnectionStrings["DbConn"].ConnectionString;
        }

        static public string dbConn()
        {
            return ConfigurationManager.ConnectionStrings["DbConn"].ConnectionString;
        }

        public DataSet auth(string username, string password)
        {
            string sValid = "";
            DataSet ds = new DataSet();
            string sConnection = eAssistConn();
            SqlConnection conn = new SqlConnection(sConnection);
            try
            {
                conn.Open();
                string sCommand = "SP_Login";
                SqlCommand cmd = new SqlCommand(sCommand, conn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@username", username);
                cmd.Parameters.AddWithValue("@password", password);
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                da.Fill(ds);
                //sValid = (string)cmd.ExecuteScalar();
            }
            catch (Exception e)
            {
                if (conn.State != ConnectionState.Closed)
                    conn.Close();
            }
            finally
            {
                conn.Close();
            }

            return ds;
        }
    }
}