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
    public class SybaseModel : DbModel
    {
        //public static string connectionString = @"Data Source=fics_server_any5;Provider=SAOLEDB.12";
        //public string connectionString = @"Data Source=fics;Provider=SAOLEDB.12";
        public string connectionString = @"Data Source=ficssql;Provider=SQLOLEDB;Initial Catalog=fics;User ID=Dbguy;Password=myfwXyx%7tU#;";
        public OleDbConnection connection = null;
        public OleDbConnection accessConnection = null;
        public string error = "";

        public SybaseModel()
        {
        }

        protected OleDbConnection get_connecetion()
        {
            if (connection == null)
                connection = new OleDbConnection(connectionString);

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

        public OleDbDataReader getReader(string query)
        {
            error = "";
            OleDbConnection dbConnection = get_connecetion();
            try
            {
                if (dbConnection.State != ConnectionState.Open) dbConnection.Open();
                OleDbCommand cmd = new OleDbCommand(query, dbConnection);
                return cmd.ExecuteReader();
            }
            catch (Exception e)
            {
                error = e.Message;
                return null;
            }
        }

        protected string executeStatement(string query)
        {
            error = "";
            OleDbConnection dbConnection = new OleDbConnection(connectionString);
            try
            {
                dbConnection.Open();
                OleDbCommand cmd = new OleDbCommand(query, dbConnection);
                cmd.ExecuteNonQuery();
                return "";
            }
            catch (Exception e)
            {
                error = e.Message;
                return error;
            }
        }

    }
}