using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using Newtonsoft.Json;

namespace ASP_API.AccessData
{
    public class ConnectDB
    {
        static public string eAssistConn()
        {
            return ConfigurationManager.ConnectionStrings["DbConn"].ConnectionString;
        }

        static public string dbConn()
        {
            return ConfigurationManager.ConnectionStrings["DbConn"].ConnectionString;
        }

        public string CheckValid(string sUserId, string sPassword, string ip)
        {
            string sValid = "";
            string sConnection = eAssistConn();
            SqlConnection conn = new SqlConnection(sConnection);
            try
            {
                conn.Open();
                string sCommand = "usp_sysuser_validasiuser_ticket";
                SqlCommand cmd = new SqlCommand(sCommand, conn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@loginauth", "");
                cmd.Parameters.AddWithValue("@userid", sUserId);
                cmd.Parameters.AddWithValue("@password", sPassword);
                cmd.Parameters.AddWithValue("@mac", "");
                cmd.Parameters.AddWithValue("@ip", ip);
                sValid = (string)cmd.ExecuteScalar();
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

            return sValid;
        }

        public string CheckValidAdmin(string sUserId, string sPassword, string ip)
        {
            string sValid = "";
            string sConnection = eAssistConn();
            SqlConnection conn = new SqlConnection(sConnection);
            try
            {
                conn.Open();
                string sCommand = "usp_sysuser_validasiuser_ticket_admin";
                SqlCommand cmd = new SqlCommand(sCommand, conn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@loginauth", "");
                cmd.Parameters.AddWithValue("@userid", sUserId);
                cmd.Parameters.AddWithValue("@password", sPassword);
                cmd.Parameters.AddWithValue("@mac", "");
                cmd.Parameters.AddWithValue("@ip", ip);
                sValid = (string)cmd.ExecuteScalar();
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

            return sValid;
        }

        public void UserLogout(string tiket)
        {
            string sConnection = eAssistConn();
            SqlConnection conn = new SqlConnection(sConnection);
            try
            {
                conn.Open();
                string sCommand = "usp_loglog_logout";
                SqlCommand cmd = new SqlCommand(sCommand, conn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@log_no", tiket);
                cmd.ExecuteNonQuery();
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

        }

        public string ChangePassword(string username, string newpassword, string oldpassword,
            string diubah_oleh, string tiket, string modul_id)
        {
            string sConnection = eAssistConn();
            string result = "";
            SqlConnection conn = new SqlConnection(sConnection);
            try
            {
                conn.Open();
                string sCommand = "usp_sysuser_changepwdsendiri";
                SqlCommand cmd = new SqlCommand(sCommand, conn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@userid", username);
                cmd.Parameters.AddWithValue("@userpwd", oldpassword);
                cmd.Parameters.AddWithValue("@newpwd", newpassword);
                cmd.Parameters.AddWithValue("@diubah_oleh", diubah_oleh);
                cmd.Parameters.AddWithValue("@tiket", tiket);
                cmd.Parameters.AddWithValue("@modul_id", modul_id);
                result = (string)cmd.ExecuteScalar();
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

            return result;

        }

        public string ChangePasswordFree(string username, string newpassword,
            string diubah_oleh, string tiket, string modul_id)
        {
            string sConnection = dbConn();
            string result = "";
            SqlConnection conn = new SqlConnection(sConnection);
            try
            {
                conn.Open();
                string sCommand = "usp_sysuser_changepwdsendirifree";
                SqlCommand cmd = new SqlCommand(sCommand, conn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@userid", username);
                cmd.Parameters.AddWithValue("@newpwd", newpassword);
                cmd.Parameters.AddWithValue("@diubah_oleh", diubah_oleh);
                cmd.Parameters.AddWithValue("@tiket", tiket);
                cmd.Parameters.AddWithValue("@modul_id", modul_id);
                result = (string)cmd.ExecuteScalar();
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

            return result;

        }

        public bool IsTiketAktif(string username, string tiket, ref string satker_id, ref string uk_id)
        {
            bool is_aktif = false;
            string sConnection = eAssistConn();
            SqlConnection conn = new SqlConnection(sConnection);
            try
            {
                conn.Open();
                string sCommand = "usp_tiket_isactive";
                SqlCommand cmd = new SqlCommand(sCommand, conn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@username", username);
                cmd.Parameters.AddWithValue("@tiket_no", tiket);

                SqlParameter param1 = new SqlParameter();
                param1.DbType = DbType.String;
                param1.Size = 20;
                param1.ParameterName = "satker_id";
                param1.Direction = ParameterDirection.Output;

                SqlParameter param2 = new SqlParameter();
                param2.DbType = DbType.String;
                param2.Size = 20;
                param2.ParameterName = "uk_id";
                param2.Direction = ParameterDirection.Output;

                cmd.Parameters.Add(param1);
                cmd.Parameters.Add(param2);

                is_aktif = (bool)cmd.ExecuteScalar();

                satker_id = (string)param1.Value;
                uk_id = (string)param2.Value;
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
            return is_aktif;
        }

        public static string DataTableToJSON(DataTable table)
        {
            List<Dictionary<string, object>> list = new List<Dictionary<string, object>>();

            foreach (DataRow row in table.Rows)
            {
                Dictionary<string, object> dict = new Dictionary<string, object>();

                foreach (DataColumn col in table.Columns)
                {
                    dict[col.ColumnName] = row[col];
                }
                list.Add(dict);
            }

            return JsonConvert.SerializeObject(list);
        }
    }
}