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
    //protected History history = null;

    protected void Page_Load(object sender, EventArgs e)
    {
        //
        this.loan_officer = getArg("loan_officer", "ALL");
        this.workflow = getArg("workflow", "ALL");
        this.loan_type = getArg("loan_type", "ALL");
        this.event_type = getArg("event_type", "ALL");
        this.loan_id = stringDecimal(getArg("loan_id", "ALL"), "ALL");
        this.decimal_loan_id = stringToDecimal(this.loan_id, 0M);
        this.version = getArg("version", "current");

        int iverstion = stringToInt(version, 0);
        if (Page.IsPostBack) return;
        BaseCollection query_collection = new CurrentCollection();
        if (iverstion > 0)
        {
            /*
            history = (new History()).get(iverstion);
            if (history != null) query_collection = new HistoryForeclosure(iverstion);
             * */
        }

        if (this.decimal_loan_id != 0M)
        {
            if (workflow == "Collections" || workflow == "ALL")
            {
                query_collection.getForeclosureLoan(this.decimal_loan_id, accepted_bfs, removed_bfs, BaseCollection.COLLECTION_TYPE);
            }
        }
        else
        {
            if (workflow == "Collections" || workflow == "ALL")
            {
                List<BaseCollection> abfs = new List<BaseCollection>();
                List<BaseCollection> rbfs = new List<BaseCollection>();
                query_collection.getCollections(abfs, rbfs);
                accepted_bfs.AddRange(abfs);// removed_bfs.AddRange(rbfs);
            }
        }
        all_bfs.AddRange(accepted_bfs);
        //all_bfs.AddRange(removed_bfs);

        populateddlLoanTypes();
        populateddlLoanOfficer();
        populateddlEventTypes();
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

    protected void populateddlLoanTypes()
    {
        string[] loan_types = { "ALL", "FHA", "CNV", "VA" };
        foreach (string lt in loan_types)
        {
            ListItem new_li = new ListItem(lt.Trim().ToUpper());
            if (this.loan_type == lt.Trim().ToUpper()) new_li.Selected = true;
            ddlLoanType.Items.Add(new_li);
        }
    }

    protected void populateddlEventTypes()
    {
        string[] event_types = { "ALL", "Warnings", "Alerts" };
        string[] event_type_values = { "ALL", "0", "1" };
        int i = 0;
        foreach (string et in event_types)
        {
            ListItem new_li = new ListItem(et,  event_type_values[i]);
            if (this.event_type == event_type_values[i]) new_li.Selected = true;
            ddlEventType.Items.Add(new_li);
            i++;
        }
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

    protected void populateddlLoanOfficer()
    {
        ListItem li = new ListItem("ALL");
        ddlLoanOfficer.Items.Add(li);
        List<String> loan_officers = new List<string>();
        foreach (BaseCollection bf in all_bfs)
        {
            bool is_new_loan_officer = true;
            foreach (string lo in loan_officers)
            {
                if (bf.foreclosure_officers.Trim().ToUpper() == lo)
                {
                    is_new_loan_officer = false;
                    break;
                }
            }
            if (is_new_loan_officer)
            {
                loan_officers.Add(bf.foreclosure_officers.ToUpper());
                ListItem new_li = new ListItem(bf.foreclosure_officers.Trim().ToUpper());
                if (this.loan_officer == bf.foreclosure_officers.Trim().ToUpper()) new_li.Selected = true;
                ddlLoanOfficer.Items.Add(new_li);
            }
        }
    }

    protected void ddlLoanOfficer_SelectedIndexChanged(object sender, EventArgs e)
    {
        this.loan_officer = ddlLoanOfficer.SelectedValue.ToUpper();
        Response.Redirect(generate_url());
    }

    protected void ddlLoanType_SelectedIndexChanged(object sender, EventArgs e)
    {
        this.loan_type = ddlLoanType.SelectedValue.ToUpper();
        Response.Redirect(generate_url());
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
        return "Events.aspx" +
            "?workflow=" + workflow +
            "&loan_officer=" + loan_officer +
            "&loan_id=" + loan_id +
            "&loan_type=" + loan_type +
            "&event_type=" + event_type +
            "&version=" + version;
    }
    protected void ddlEventType_SelectedIndexChanged(object sender, EventArgs e)
    {
        this.event_type = ddlEventType.SelectedValue.ToUpper();
        Response.Redirect(generate_url());
    }
}