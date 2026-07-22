using System;
using System.Collections.Generic;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Text.RegularExpressions;

using com.sp.rmmc.collections.models;
using com.sp.rmmc.common.tools;

public partial class lossmitigation_Default : System.Web.UI.Page
{
    protected string workflow = "All";
    protected string version = "current";
    protected string sublist = "ALL";
    protected string section = "Collections";
    protected List<BaseModification> accepted_bfs = new List<BaseModification>();
    protected List<BaseModification> removed_bfs = new List<BaseModification>();

    protected void Page_Load(object sender, EventArgs e)
    {
        this.workflow = getArg("workflow", "All");
        this.version = getArg("version", "current");
        int iverstion = stringToInt(version, 0);
        this.section = getArg("section", "Collections");

        BaseModification query = new BaseModification();
        query.getSpreadDeficiencyShortage(accepted_bfs, removed_bfs);
    
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

    protected void lbExport_ClickAll(object sender, EventArgs e)
    {
        string filename = "SpreadDeficiencyShortageReport-";
        CSVExport.writeHeadersAndCSV(filename, "Loan#,Loan Name,Alert Desc,# Months Shortage,# Months Deficiency,Next Analysis Date", this.accepted_bfs);
    }

}
