using System;
using System.Collections.Generic;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Text.RegularExpressions;
using System.Web.UI.HtmlControls;
using System.Diagnostics;

using com.sp.rmmc.collections.models;
using com.sp.rmmc.common.tools;

public partial class lossmitigation_Default : System.Web.UI.Page
{
    protected string workflow = BaseCollection.NATION_STAR_PDF;
    protected string version = "current";
    protected string sublist = "ALL";
    protected string section = "Collections";
    protected string loan_id = ""; 
    protected void Page_Load(object sender, EventArgs e)
    {
        this.loan_id = getArg("loan_id", "0");
        this.version = getArg("version", "current");
        int iverstion = stringToInt(version, 0);
        this.section = getArg("section", "Collections");
        generatePDF(this.loan_id);
    }

    protected string getArg(string arg, string default_value)
    {        
        string request_value = Request.QueryString[arg];
        if (request_value != "" && request_value != null) return request_value;
        return default_value;
    }

    protected int stringToInt(string value, int default_value)
    {
        if (value == "") return default_value;
        try
        {
            return Convert.ToInt32(Regex.Replace(value, "[^0-9]", ""));
        }
        catch (Exception)
        {
            return default_value;
        }
    }

    protected void generatePDF(String loan_number)
    {
        try
        {
            this.lblError.Text = "";
            string path = Server.MapPath("."); // e.g. J:\tech\jaime\collections\collections\EOMInvestorReporting
            path += @"\python\";
            Process p = new Process();
            p.StartInfo.WorkingDirectory = path;
            p.StartInfo.FileName = "populateLoanPdf.bat";
            p.StartInfo.Arguments = string.Format("{0}", loan_number);
            p.StartInfo.CreateNoWindow = false;
            p.Start();
            p.WaitForExit();
            Response.Redirect(string.Format("python/{0}.pdf", loan_number));
        }
        catch (Exception execption)
        {
            this.lblError.Text = "Error Generating PDF:\n " + execption.Message;
        }
    }
}