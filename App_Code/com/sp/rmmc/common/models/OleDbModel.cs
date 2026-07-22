using System;
using System.Collections.Generic;
using System.Web;


using System.Data;
using System.Data.OleDb;
using System.Data.Common;


namespace com.sp.rmmc.common.models
{
    public class OleDbModel : DbModel
    {
        public string connectionString = "";
        public OleDbConnection connection = null;
        public string error = "";

        public OleDbModel()
        {

        }

        public void setConnectionString(string connectionString)
        {
            this.connectionString = connectionString;
            this.get_connection();
        }

        public OleDbCommand getCommand()
        {
            OleDbConnection db_connection = this.get_connection();
            return db_connection.CreateCommand();
        }

        protected OleDbConnection get_connection() 
        {
            if (connection == null)
            {
                try
                {
                    connection = new OleDbConnection(connectionString);
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

        public OleDbDataReader getReader(string query)
        {
            error = "";
            OleDbConnection dbConnection = get_connection();
            try
            {
                if( dbConnection.State != ConnectionState.Open ) dbConnection.Open();
                OleDbCommand cmd = new OleDbCommand(query, dbConnection);
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
            OleDbConnection dbConnection = new OleDbConnection(connectionString);
            try
            {
                if (dbConnection.State != ConnectionState.Open) dbConnection.Open();
                OleDbCommand cmd = new OleDbCommand(query, dbConnection);
                int affected_rows = cmd.ExecuteNonQuery();
                return "";
            }
            catch (Exception e)
            {
                error = e.Message;
                return error;
            }
        }

        public int executeCommandStatement(OleDbCommand cmd)
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
            OleDbCommand cmd = new OleDbCommand(query, this.connection);
            OleDbDataReader reader = cmd.ExecuteReader();
            if (reader.Read())
            {
                ret = Convert.ToInt32(readDBDecimal(reader, 0));
            }
            reader.Close();
            return ret;
        }
    }
}