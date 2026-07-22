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
    protected List<BaseCollection> bfs = new List<BaseCollection>();
    protected List<BaseCollection> all_bfs = new List<BaseCollection>();
    protected List<BaseCollection> accepted_bfs = new List<BaseCollection>();
    protected List<BaseCollection> bankruptcies = new List<BaseCollection>();
    protected List<BaseCollection> demands = new List<BaseCollection>();
    protected List<BaseCollection> foreclosures = new List<BaseCollection>();
    protected List<BaseCollection> removed_bfs = new List<BaseCollection>();
    protected History history = null;
    protected void Page_Load(object sender, EventArgs e)
    {
        this.workflow = getArg("workflow", "All");
        this.version = getArg("version", "current");
        int iverstion = stringToInt(version, 0);
        this.section = getArg("section", "Collections");

        BaseCollection query_collection = new CurrentCollection();
        /*
        if (iverstion > 0)
        {
            history = (new History()).get(iverstion);
            if (    history != null) query_collection = new HistoryCollection(history);
        }
        */
        query_collection.getBankruptcyReportLoans(accepted_bfs, removed_bfs);
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

    protected void lbExport_Click(object sender, EventArgs e)
    {
        string filename = "Bankruptcy Report ";
        CSVExport.writeHeadersAndCSV(filename, "Loan#,Loan Name,Due Date of next payment,Mortgage Status Code,Loan Stop Code", this.accepted_bfs);
    }


}