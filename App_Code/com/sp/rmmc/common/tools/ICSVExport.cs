using System;
using System.Collections.Generic;
using System.Web;

/// <summary>
/// Summary description for Export
/// </summary>

namespace com.sp.rmmc.common.tools
{
    public interface ICSVExport
    {
        public String to_csv();
        public List<ICSVExport> csv_export_list<T>(List<T> data);
    }
}