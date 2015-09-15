using System;
using System.Collections.Generic;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

using System.Text.RegularExpressions;

using com.sp.rmmc.collections.models;
using com.sp.rmmc.common.models.events;

public partial class lossmitigation_Default : System.Web.UI.Page
{
    protected string loan_officer = "ALL";
    protected string workflow = "ALL";
    protected string loan_type = "ALL";
    protected string event_type = "ALL";
    protected string loan_id = "ALL";
    protected string version = "current";
    protected decimal decimal_loan_id = 0M;
    protected List<Event> events = new List<Event>();
    protected List<BaseCollection> bfs = new List<BaseCollection>();
    protected List<BaseCollection> all_bfs = new List<BaseCollection>();
    protected List<BaseCollection> accepted_bfs = new List<BaseCollection>();
    protected List<BaseCollection> removed_bfs = new List<BaseCollection>();
    protected BaseCollection loan_collection = null;

    protected bool individual_condition = true;
    protected bool overall_condition = true;

    protected DateTime event_date = DateTime.Today;

    protected History history = null;

    protected void Page_Load(object sender, EventArgs e)
    {
        //
        this.loan_officer = getArg("loan_officer", "ALL");
        this.workflow = getArg("workflow", "ALL");
        this.loan_type = getArg("loan_type", "ALL");
        this.event_type = getArg("event_type", "ALL");
        this.loan_id = stringDecimal(getArg("loan_id", ""), "");
        this.decimal_loan_id = stringToDecimal(this.loan_id, 0M);
        this.version = getArg("version", "current");

        int iverstion = stringToInt(version, 0);
        if (Page.IsPostBack) return;
        BaseCollection query_collection = new CurrentCollection();
        if (iverstion > 0)
        {
            
            history = (new History()).get(iverstion);
            if (history != null)
            {
                query_collection = new HistoryCollection(history);
                event_date = history.history_date.date;
            }
        }

        if (this.decimal_loan_id != 0M)
        {
            query_collection.getForeclosureLoan(this.decimal_loan_id, accepted_bfs, removed_bfs, BaseCollection.COLLECTION_TYPE);
        }
        all_bfs.AddRange(accepted_bfs);
        all_bfs.AddRange(removed_bfs);
        if (all_bfs.Count > 0) loan_collection = all_bfs[0];

        this.txtLoanId.Text = this.loan_id;

        filter();
    }

    protected void filter()
    {
        bfs.Clear();
        foreach (BaseCollection bf in accepted_bfs)
        {
            if ((loan_officer == "ALL" || loan_officer == bf.foreclosure_officers) &&
                (loan_type == "ALL" || loan_type == bf.loan.loan_type)) bfs.Add(bf);
            else continue;
            foreach (Event e in bf.getEvents())
            {
                e.parent_type = bf.type;
                if (event_type == "ALL" || e.type.ToString() == event_type) events.Add(e);
            }
        }
    }

    protected void setEvents()
    {
        foreach (BaseCollection bf in bfs)
        {
            foreach (Event e in bf.getEvents())
            {
                e.parent_type = bf.type;
                if( event_type == "ALL" || e.type.ToString() == event_type ) events.Add(e);
            }
        }
    }

    protected string getArg(string arg, string default_value)
    {        
        string request_value = Request.QueryString[arg];
        if (request_value != "" && request_value != null) return request_value;
        return default_value;
    }

    protected decimal stringToDecimal(string value, decimal default_value)
    {
        if (value == "") return default_value;
        try
        {
            return Convert.ToDecimal(Regex.Replace(value, "[^0-9]", ""));
        }
        catch (Exception)
        {
            return default_value;
        }
    }

    protected string stringDecimal(string value, string default_value)
    {
        if (value == "") return default_value;
        try
        {
            return Convert.ToDecimal(Regex.Replace(value, "[^0-9]", "")).ToString();
        }
        catch (Exception)
        {
            return default_value;
        }
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

    protected void btnSearch_Click(object sender, EventArgs e)
    {
        if (this.txtLoanId.Text == "" || this.txtLoanId.Text.ToUpper() == "ALL")
        {
            this.loan_id = "ALL";
            Response.Redirect(generate_url());
        }
        try
        {
            decimal dloan_id = Convert.ToDecimal(Regex.Replace(this.txtLoanId.Text, "[^0-9]", ""));
            if (dloan_id > 0)
            {
                this.loan_id = dloan_id.ToString();
                Response.Redirect(generate_url());
            }
            else
                this.lblError.Text = "Loan Number '" + txtLoanId.Text + "' is incorrect.";
        }
        catch (Exception)
        {
            this.lblError.Text = "Loan Number '" + txtLoanId.Text + "' is incorrect.";
            return;
        }
    }

    protected string generate_url()
    {
        return "PromisedToPayLoan.aspx" +
            "?workflow=" + workflow +
            "&loan_officer=" + loan_officer +
            "&loan_id=" + loan_id +
            "&loan_type=" + loan_type +
            "&event_type=" + event_type +
            "&version=" + version;
    }

}