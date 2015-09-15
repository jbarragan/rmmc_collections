using System;
using System.Collections.Generic;
using System.Web;


/// <summary>
/// Summary description for Export
/// </summary>

namespace com.sp.rmmc.common.tools
{
    public class CSVExport
    {
        public string headers = null;
        public CSVExport()
        {
            
        }

        public static void writeCSV<T>(string name, List<T> list)
        {
            CSVExport export = new CSVExport();
            export.write(name, list);
        }

        public static void writeHeadersAndCSV<T>(string name, string headers, List<T> list)
        {
            CSVExport export = new CSVExport();
            export.headers = headers;
            export.write(name, list);
        }

        private List<ICSVExport> to_ICSVExport_list<T>(List<T> data)
        {
            List<ICSVExport> list = new List<ICSVExport>();
            foreach (T d in data) list.Add((ICSVExport)d);
            return list;
        }

        public void write<T>(string name, List<T> list)
        {
            string attachment = "attachment; filename=" +  name +
                                "-" + DateTime.Today.ToString("yyyyMMdd") + ".csv";
            report_header(attachment);
            report_body(to_ICSVExport_list(list));
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
            if (headers != null)
            {
                HttpContext.Current.Response.Write(headers);
                HttpContext.Current.Response.Write(Environment.NewLine);
            }
            foreach (ICSVExport item in list)
            {
                HttpContext.Current.Response.Write(item.to_csv());
                HttpContext.Current.Response.Write(Environment.NewLine);
            }
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