using System;
using System.Collections.Generic;
using System.Web;

/// <summary>
/// Summary description for DateTimeObject
/// </summary>

namespace com.sp.rmmc.common.models
{
    public class DateTimeObject
    {
        public DateTime date;
        public bool isNull = true;
        
        public DateTimeObject()
        {   //
            // TODO: Add constructor logic here
            //
        }

        public string ToString()
        {
            string ret = "NA";
            if (!isNull) ret = date.ToShortDateString();
            return ret;
        }

        public DateTime GetValue()
        {
            return date;
        }

        public void set_date(DateTime the_date)
        {
            this.date = the_date;
            this.isNull = false;
        }

        public string ToShortDateString()
        {
            return (!isNull) ? date.ToShortDateString() : "";
        }

        public string toDBValue()
        {
            string s = "NULL";
            if (!this.isNull)
            {
                s = "'" + date.ToString("MM/dd/yyyy HH:mm:ss") + "'";
            }
            return s;
        }

        public string toSQLAnywhereDBValue()
        {
            string s = "NULL";
            if (!this.isNull)
            {
                s = "'" + date.ToString("dd MMMM yyyy") + "'";
            }
            return s;
        }


    }
}