using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data;
using System.Linq;
using System.Web;
using System.Configuration;

namespace NaatsWebApp.Models
{
	public class DBHelper
	{
        static string constr = ConfigurationManager.ConnectionStrings["conString"].ConnectionString;
        public SqlConnection conn = new SqlConnection(constr);
        public SqlCommand cmd = null;
        public SqlDataReader sdr = null;


        public void OpenConnection()
        {
            if (conn.State == ConnectionState.Closed)
            {
                conn.Open();
            }

        }
        public void CloseConnection()
        {
            if (conn.State == ConnectionState.Open)
            {
                conn.Close();
            }
        }

        public void InsertUpdateDelete(string query)
        {
            cmd = new SqlCommand(query, conn);
            cmd.ExecuteNonQuery();
        }

        public SqlDataReader getData(string query)
        {
            cmd = new SqlCommand(query, conn);
            sdr = cmd.ExecuteReader();
            return sdr;
        }
    }
}