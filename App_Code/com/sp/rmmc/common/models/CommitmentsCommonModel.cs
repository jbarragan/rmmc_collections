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
    public class CommitmentsCommonModel
    {
        public static string connectionString = @"Data Source=fics;Provider=SAOLEDB.12";
        public static OleDbConnection connection = null;
        public static OleDbConnection accessConnection = null;

        //public static string accessConnectionString = "NA";
            //System.Configuration.ConfigurationManager.ConnectionStrings["InvestorReport"].ConnectionString;

        public CommitmentsCommonModel()
        {
            //
            // TODO: Add constructor logic here
            //
        }

        protected static string readDBString(DbDataReader reader, int pos)
        {
            if (reader.GetValue(pos) is DBNull)
                return "";
            else
                return reader.GetString(pos);
        }

        protected static decimal readDBDecimal(DbDataReader reader, int pos)
        {
            if (reader.GetValue(pos) is DBNull)
                return 0M;
            else
                return reader.GetDecimal(pos);
        }

        protected static int readDBInt(DbDataReader reader, int pos)
        {
            if (reader.GetValue(pos) is DBNull)
                return 0;
            else
                return reader.GetInt16(pos);
        }

        protected static bool readDBYesNo(DbDataReader reader, int pos)
        {
            if (reader.GetValue(pos) is DBNull)
                return false;
            else
                return reader.GetBoolean(pos);
        }

        protected static int readDBInt32(DbDataReader reader, int pos)
        {
            if (reader.GetValue(pos) is DBNull)
                return 0;
            else
                return reader.GetInt32(pos);
        }

        protected static DateTime readDBDate(DbDataReader reader, int pos)
        {
            if (reader.GetValue(pos) is DBNull)
                return DateTime.Now;
            else
                return reader.GetDateTime(pos);
        }

        protected static DateTimeObject readDBDateObject(DbDataReader reader, int pos, DateTime defaultValue)
        {
            DateTimeObject ret = new DateTimeObject();
            if (reader.GetValue(pos) is DBNull)
            {
                ret.date = defaultValue;
            }
            else
            {
                ret.isNull = false;
                ret.date = reader.GetDateTime(pos);
            }
            return ret;
        }

        protected static IntObject readDBIntObject(DbDataReader reader, int pos, int defaultValue)
        {
            IntObject ret = new IntObject();
            if (reader.GetValue(pos) is DBNull)
            {
                ret.isNull = true;
                ret.i = defaultValue;
            }
            else
            {
                ret.isNull = false;
                ret.i = readDBInt(reader, pos);
            }
            return ret;
        }

        public static string error = "";

        protected static OleDbConnection get_connecetion() 
        {
            if (connection == null)
                connection = new OleDbConnection(connectionString);
            
            return connection;
        }

        public static void close_connection()
        {
            if (connection != null)
            {
                connection.Close();
                connection = null;
            }
        }

        protected static bool is_access_connection_open()
        {
            if (accessConnection == null) return false;
            return true;
        }

        protected static OleDbConnection get_access_connection(string access_connection_string) 
        {
            if (accessConnection == null)
                accessConnection = new OleDbConnection(access_connection_string);

            return accessConnection;
        }

        public static void close_access_connection()
        {
            if (accessConnection != null)
            {
                accessConnection.Close();
                accessConnection = null;
            }
        }

        protected static OleDbDataReader getReader(string query)
        {
            error = "";
            OleDbConnection dbConnection = get_connecetion();
            try
            {
                dbConnection.Open();
                OleDbCommand cmd = new OleDbCommand(query, dbConnection);
                return cmd.ExecuteReader();
            }
            catch (Exception e)
            {
                error = e.Message;
                return null;
            }
        }

        protected static string executeStatement(string query)
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

        protected static OleDbDataReader getAccessReader(string accessConnectionString, string query)
        {
            error = "";
            bool is_connection_open = is_access_connection_open();

            OleDbConnection dbConnection = get_access_connection(accessConnectionString);
            try
            {
                if( is_connection_open == false ) dbConnection.Open();
                OleDbCommand cmd = new OleDbCommand(query, dbConnection);
                return cmd.ExecuteReader();
            }
            catch (Exception e)
            {
                error = e.Message;
                return null;
            }
        }

        private static int getAccessLastIdentity(OleDbConnection dbConnection)
        {
            int ret = 0;
            string query = "SELECT @@IDENTITY";
            OleDbCommand cmd = new OleDbCommand(query, dbConnection);
            OleDbDataReader reader = cmd.ExecuteReader();
            if (reader.Read())
            {
                ret = readDBInt32(reader, 0);
            }
            return ret;
        }

        protected static int executeAccessStatement(string accessConnectionString, string query)
        {
            error = "";
            OleDbConnection dbConnection = new OleDbConnection(accessConnectionString);
            try
            {
                dbConnection.Open();
                OleDbCommand cmd = new OleDbCommand(query, dbConnection);
                int affected_rows = cmd.ExecuteNonQuery();
                if (affected_rows > 0)
                {
                    int new_id = getAccessLastIdentity(dbConnection);
                    dbConnection.Close();
                    return new_id;
                }
                else
                {
                    dbConnection.Close();
                    return 0;
                }
            }
            catch (Exception e)
            {
                error = e.Message;
                return -1;
            }
        }

        protected static string getTableOpenRow(string cssClass, string format)
        {
            if (format == "csv")
                return Environment.NewLine;
            else
                return @"<tr class=""" + cssClass + @""">";
        }

        protected static string getTableCloseRow(string format)
        {
            if (format == "csv")
                return "";
            else
                return @"</tr>";
        }

        protected static string addAnEnter(string format)
        {
            if (format == "csv")
                return "";
            else
                return @"
";
        }   
        
        // MsSQL adapter.
        public static SqlConnection ms_sql_connection = null;
        protected static bool isMsSQLConnectionOpen()
        {
            if (ms_sql_connection == null) return false;
            return true;
        }

        protected static SqlConnection getMsSQLConnection(string ms_sql_connection_string)
        {
            if (ms_sql_connection == null)
                ms_sql_connection = new SqlConnection(ms_sql_connection_string);

            return ms_sql_connection;
        }

        public static void closeMsSQLConnection()
        {
            if (ms_sql_connection != null)
            {
                ms_sql_connection.Close();
                ms_sql_connection = null;
            }
        }

        protected static SqlDataReader getMsSQLReader(string ms_sql_connection_string, string query)
        {
            error = "";
            bool is_connection_open = isMsSQLConnectionOpen();

            SqlConnection dbConnection = getMsSQLConnection(ms_sql_connection_string);
            try
            {
                if (is_connection_open == false) dbConnection.Open();
                SqlCommand cmd = new SqlCommand(query, dbConnection);
                return cmd.ExecuteReader();
            }
            catch (Exception e)
            {
                error = e.Message;
                return null;
            }
        }

        private static int getMsSQLLastIdentity(SqlConnection dbConnection)
        {
            int ret = 0;
            string query = "SELECT @@IDENTITY";
            SqlCommand cmd = new SqlCommand(query, dbConnection);
            SqlDataReader reader = cmd.ExecuteReader();
            if (reader.Read())
            {
                ret = Convert.ToInt32(readDBDecimal(reader, 0));
            }
            return ret;
        }

        protected static int executeMsSQLStatement(string ms_sql_connection_string, string query)
        {
            error = "";
            SqlConnection dbConnection = new SqlConnection(ms_sql_connection_string);
            try
            {
                dbConnection.Open();
                SqlCommand cmd = new SqlCommand(query, dbConnection);
                int affected_rows = cmd.ExecuteNonQuery();
                if (affected_rows > 0)
                {
                    int new_id = getMsSQLLastIdentity(dbConnection);
                    dbConnection.Close();
                    return new_id;
                }
                else
                {
                    dbConnection.Close();
                    return 0;
                }
            }
            catch (Exception e)
            {
                error = e.Message;
                return -1;
            }
        }

    }
}