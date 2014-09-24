using System;
using System.Collections.Generic;
using System.Web;

/// <summary>
/// Summary description for DateTimeObject
/// </summary>

namespace com.sp.rmmc.common.models
{
    public class IntObject
    {
        public int i;
        public bool isNull = true;
        
        public IntObject()
        {   //
            // TODO: Add constructor logic here
            //
        }

        public string ToString()
        {
            string ret = "NA";
            if (!isNull) ret = i.ToString();
            return ret;
        }

        public int GetValue()
        {
            return i;
        }

    }
}