using System;
using System.Collections.Generic;
using System.Web;

using com.sp.rmmc.common.models;
namespace com.sp.rmmc.common.models.events
{
    public class Event
    {
        public MsLoan loan = new MsLoan();
        public static int WARNING = 0;
        public static int ALERT = 1;
        public String name = "";
        public String description = "";
        public int type = WARNING;
        public DateTimeObject date = new DateTimeObject();
        public string parent_type = "";


        public string getEventStatusColor()
        {
            string s = "";            
            if (type == Event.WARNING) s = "warning";
            if (type == Event.ALERT) s = "error"; 
            return s;
        }

        public string typeToString()
        {
            if (this.type == Event.WARNING) return "Warning";
            if (this.type == Event.ALERT) return "Alert";
            return "";
        }
    }
}