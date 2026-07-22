using System;
using System.Collections.Generic;
using System.Web;


using System.Data;
using System.Data.Sql;
using System.Data.SqlClient;
using System.Data.Common;

/// <summary>
/// Summary description for CommonModel
/// </summary>
namespace com.sp.rmmc.common.models
{
    public class DbModel
    {
        public static string error = "";

        public DbModel()
        {
        }

        public static string readDBString(DbDataReader reader, int pos)
        {
            if (reader.GetValue(pos) is DBNull)
                return "";
            else
                return reader.GetString(pos);
        }

        public static decimal readDBDecimal(DbDataReader reader, int pos)
        {
            if (reader.GetValue(pos) is DBNull)
                return 0M;
            else
                return reader.GetDecimal(pos);
        }

        public static decimal readDBDoubleDecimal(DbDataReader reader, int pos)
        {
            if (reader.GetValue(pos) is DBNull)
                return 0M;
            else
                return Convert.ToDecimal(reader.GetDouble(pos));
        }

        public static int readDBInt(DbDataReader reader, int pos)
        {
            if (reader.GetValue(pos) is DBNull)
                return 0;
            else
            {
                try
                {
                    return reader.GetInt16(pos);
                }
                catch (Exception e)
                {
                    return reader.GetInt32(pos);
                }
            }
        }

        public static bool readDBYesNo(DbDataReader reader, int pos)
        {
            if (reader.GetValue(pos) is DBNull)
                return false;
            else
                return reader.GetBoolean(pos);
        }

        public static int readDBInt32(DbDataReader reader, int pos)
        {
            if (reader.GetValue(pos) is DBNull)
                return 0;
            else
                return reader.GetInt32(pos);
        }

        public static DateTime readDBDate(DbDataReader reader, int pos)
        {
            if (reader.GetValue(pos) is DBNull)
                return DateTime.Now;
            else
                return reader.GetDateTime(pos);
        }

        public static DateTimeObject readDBDateObject(DbDataReader reader, int pos)
        {
            DateTimeObject ret = new DateTimeObject();
            if (reader.GetValue(pos) is DBNull)
            {
                ret.date = DateTime.Now;
            }
            else
            {
                ret.isNull = false;
                ret.date = reader.GetDateTime(pos);
            }
            return ret;
        }

        public static IntObject readDBIntObject(DbDataReader reader, int pos, int defaultValue)
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

        public static string toDBString(string s)
        {
            string r = "'" + s.Replace("'", "''") + "'";
            return r;
        }
    }
}