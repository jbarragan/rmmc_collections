using System;
using System.Collections.Generic;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Text.RegularExpressions;

using com.sp.rmmc.collections.models;

public partial class lossmitigation_Default : System.Web.UI.Page
{
    protected string loan_officer = "ALL";
    protected string workflow = "EPD";
    protected string loan_type = "ALL";
    protected string event_type = "Different Reason Codes";
    protected string version = "current";
    protected string sublist = "ALL";
    protected List<BaseEPD> bfs = new List<BaseEPD>();
    protected List<BaseEPD> all_bfs = new List<BaseEPD>();
    protected List<BaseEPD> accepted_bfs = new List<BaseEPD>();
    protected List<BaseEPD> removed_bfs = new List<BaseEPD>();
    //protected History history = null;
    protected decimal[] array_completed_checklists_loan_ids;

    protected void Page_Load(object sender, EventArgs e)
    {
        this.loan_officer = getArg("loan_officer", "ALL");
        this.workflow = getArg("workflow", "EPD");
        this.loan_type = getArg("loan_type", "ALL");
        this.event_type = getArg("event_type", "ALL");
        this.sublist = getArg("sublist", "ALL");
        this.version = getArg("version", "current");
        int iverstion = stringToInt(version, 0);

        BaseEPD query = new CurrentEPD();
        if (iverstion > 0)
        {
            //history = (new History()).get(iverstion);
            //if( history != null ) query_foreclosure = new HistoryForeclosure(iverstion);
        }

        //if (workflow == "Collections")
        query.getAll(accepted_bfs, removed_bfs);

        all_bfs.AddRange(accepted_bfs);
        all_bfs.AddRange(removed_bfs);

        filter();
    }

    protected void filter()
    {
        bfs.Clear();
        foreach (BaseEPD bf in accepted_bfs)
        {
            bfs.Add(bf);
        }
    }

    protected string getArg(string arg, string default_value)
    {
        string request_value = Request.QueryString[arg];
        if (request_value != "" && request_value != null) return request_value;
        return default_value;
    }

    protected string generate_url()
    {
        return "Default.aspx" +
            "?workflow=" + workflow +
            "&sublist=" + sublist +
            "&loan_officer=" + loan_officer +
            "&loan_type=" + loan_type +
            "&event_type=" + event_type +
            "&version=" + version;
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

}