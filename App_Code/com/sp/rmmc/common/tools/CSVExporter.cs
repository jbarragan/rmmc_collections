using System;
using System.Collections.Generic;
using System.Web;

using com.sp.rmmc.commitments.models;

/// <summary>
/// Summary description for Export
/// </summary>

namespace com.sp.rmmc.common.tools
{
    public class CSVExport
    {
        public CSVExport()
        {
            
        }

        public static void writeCSV(string name, List<ICSVExport> list){
            CSVExport export = new CSVExport();
            export.write(name, list);
        }

        public void write(string name, List<ICSVExport> list)
        {
            string attachment = "attachment; filename=" +  name +
                                "-" + DateTime.Today.ToString("yyyyMMdd") + ".csv";
            report_header(attachment);
            report_body(list);
            report_footer();
        }
        
        private void report_footer()
        { 
            HttpContext.Current.Response.End();
        }

        private void report_header(string attachment)
        {
            HttpContext.Current.Response.Clear();
            HttpContext.Current.Response.ClearHeaders();
            HttpContext.Current.Response.ClearContent();
            HttpContext.Current.Response.AddHeader("content-disposition", attachment);
            HttpContext.Current.Response.ContentType = "text/csv";
            HttpContext.Current.Response.AddHeader("Pragma", "public");
        }

        private void report_body(List<ICSVExport> list)
        {
            foreach (ICSVExport item in list)
                HttpContext.Current.Response.Write(item.to_csv);
        }

        private string removeSpecialCharacters(string s){
            string ret = s.Replace(" ", "").Replace(",", "").Replace(".", "").Replace("&", "").Replace("%", "");
            return ret;
        }


        private string removeComma(string s)
        {
            string ret = s.Replace(",", "");
            return ret;
        }

    }
}