using System;
using System.Collections.Generic;
using System.Web;


using System.Data;
using System.Data.OleDb;

using System.Data.Sql;
using System.Data.SqlClient;
using System.Data.Common;

/// <summary>
/// Summary description for CommonModel
/// </summary>
namespace com.sp.rmmc.common.models
{
    public class MsSqlModel : DbModel
    {
        //public static string connectionString = @"Data Source=fics_server_any5;Provider=SAOLEDB.12";
        public string connectionString = "";
        public SqlConnection connection = null;
        public string error = "";

        public MsSqlModel()
        {

        }

        public void setConnectionString(string connectionString)
        {
            this.connectionString = connectionString;
            this.get_connection();
        }

        public SqlCommand getCommand()
        {
            SqlConnection db_connection = this.get_connection();
            return db_connection.CreateCommand();
        }

        protected SqlConnection get_connection() 
        {
            if (connection == null)
            {
                try
                {
                    connection = new SqlConnection(connectionString);
                    connection.Open();
                }
                catch (Exception) { }
            }
            return connection;
        }

        public void close_connection()
        {
            if (connection != null)
            {
                connection.Close();
                connection = null;
            }
        }

        public SqlDataReader getReader(string query)
        {
            error = "";
            SqlConnection dbConnection = get_connection();
            try
            {
                if( dbConnection.State != ConnectionState.Open ) dbConnection.Open();
                SqlCommand cmd = new SqlCommand(query, dbConnection);
                return cmd.ExecuteReader();
            }
            catch (Exception e)
            {
                error = e.Message;
                return null;
            }
        }

        public string executeStatement(string query)
        {
            error = "";
            SqlConnection dbConnection = new SqlConnection(connectionString);
            try
            {
                if (dbConnection.State != ConnectionState.Open) dbConnection.Open();
                SqlCommand cmd = new SqlCommand(query, dbConnection);
                int affected_rows = cmd.ExecuteNonQuery();
                return "";
            }
            catch (Exception e)
            {
                error = e.Message;
                return error;
            }
        }

        public int executeCommandStatement(SqlCommand cmd)
        {
            error = "";
            try
            {   
                int affected_rows = cmd.ExecuteNonQuery();
                if (affected_rows > 0)
                {
                    int new_id = getLastIdentity();
                    return new_id;
                }
                return 0;
            }
            catch (Exception e)
            {
                error = e.Message;
                return 0;
            }
        }

        public int getLastIdentity()
        {
            int ret = 0;
            string query = "SELECT @@IDENTITY";
            SqlCommand cmd = new SqlCommand(query, this.connection);
            SqlDataReader reader = cmd.ExecuteReader();
            if (reader.Read())
            {
                ret = Convert.ToInt32(readDBDecimal(reader, 0));
            }
            reader.Close();
            return ret;
        }
    }
}