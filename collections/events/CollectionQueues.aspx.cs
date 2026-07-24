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
    protected string loan_officer = "ALL";
    //protected string workflow = "Promised To Pay";
    protected string workflow = "Fannie Mae 17 day Call Listing";
    protected string loan_type = "ALL";
    protected string collector = "ALL";
    // protected string event_type = "Promised To Pay";
    protected string event_type = "Fannie Mae 17 day Call Listing"; 
    protected string version = "current";
    protected string exportCollector = null;
    protected string sublist = "ALL";
    protected List<BaseCollection> bfs = new List<BaseCollection>();
    protected List<BaseCollection> all_bfs = new List<BaseCollection>();
    protected List<BaseCollection> accepted_bfs = new List<BaseCollection>();
    protected List<BaseCollection> removed_bfs = new List<BaseCollection>();
    protected Dictionary<string, List<BaseCollection>> collector_loans = new Dictionary<string, List<BaseCollection>>();
    protected History history = null;
    protected decimal[] array_completed_checklists_loan_ids;

    protected void Page_Load(object sender, EventArgs e)
    {
        this.loan_officer = getArg("loan_officer", "ALL");
        this.workflow = getArg("workflow", "Fannie Mae 17 day Call Listing");
        this.loan_type = getArg("loan_type", "ALL");
        this.collector = getArg("collector", "ALL");
        this.event_type = getArg("event_type", "ALL");
        this.sublist = getArg("sublist", "ALL");
        this.version = getArg("version", "current");
        this.exportCollector = getArg("export", null);
        int iverstion = stringToInt(version, 0);

        BaseCollection query_collection = new CurrentCollection();
        if (iverstion > 0)
        {
            history = (new History()).get(iverstion);
            if (    history != null) query_collection = new HistoryCollection(history);
        }

        if (workflow == "Collections")
        {
            if (event_type == "Different Reason Codes")
                query_collection.getCollectionsAndDemandsWithReportType(accepted_bfs, removed_bfs, BaseCollection.REASON_CODE_REPORT);
            else
                query_collection.getCollectionsAndDemandsWithReportType(accepted_bfs, removed_bfs, BaseCollection.PROMISED_TO_REPORT);
        }else if (workflow == "Promised To Pay")
        {
            this.event_type = "Promised To Pay Expired";
            query_collection.getCollectionsAndDemandsWithReportType(accepted_bfs, removed_bfs, BaseCollection.PROMISED_TO_REPORT);
        }
        else if (workflow == "4-Month Delinquent In Collections")
        {
            query_collection.getCollections4MonthDelinquent(accepted_bfs, removed_bfs);
        }
        else if (workflow == "4-Month Delinquent NOT In Collections")
        {
            query_collection.getCollections4MonthDelinquent(removed_bfs, accepted_bfs);
        }
        else if (workflow == "FHA VA 17-day Call Listing")
        {
            query_collection.getCollections17DayDelinquentFHAVA(accepted_bfs, removed_bfs);
        }
        else if (workflow == "Fannie Mae 17 day Call Listing")
        {
            query_collection.getCollections17DayDelinquentCNVNonCMCM(accepted_bfs, removed_bfs);
        }
        else if (workflow == "Fannie Mae 17-day MCM Call Listing")
        {
            query_collection.getCollections17DayDelinquentCNVCMCM(accepted_bfs, removed_bfs);
        }
        else if (workflow == "FHA 60 Day Call Listing")
        {
            query_collection.getCollections2MonthFHADelinquent(accepted_bfs, removed_bfs);
        }
        else if (workflow == "Fannie Mae 2 Month Call Listing")
        {
            query_collection.getCollections2MonthCONVDelinquent(accepted_bfs, removed_bfs);
        }
        else if (workflow == "VA 2 Month Call Listing")
        {
            query_collection.getCollections2MonthVADelinquent(accepted_bfs, removed_bfs);
        }
        else if (workflow == "HUD 3 Month Call Listing")
        {
            query_collection.getCollections3MonthFHAHudDelinquent(accepted_bfs, removed_bfs);
        }
        else if (workflow == "Fannie Mae 3 Month Call Listing")
        {
            query_collection.getCollections3MonthCONVDelinquent(accepted_bfs, removed_bfs);
        }
        else if (workflow == "VA 3 Month Call Listing")
        {
            query_collection.getCollections3MonthVADelinquent(accepted_bfs, removed_bfs);
        }
        

        all_bfs.AddRange(accepted_bfs);
        all_bfs.AddRange(removed_bfs);

        populateddlLoanTypes();
        populateddlCollector();
        populateddlEventType();

        filter();

        populate_collector_loans();
        if (exportCollector != null) exportCollectorList(exportCollector);

    }

    protected void populate_collector_loans()
    {
        foreach (BaseCollection bf in bfs)
        {
            if (collector_loans.ContainsKey(bf.collector.collector))
                collector_loans[bf.collector.collector].Add(bf);
            else
            {
                List<BaseCollection> collections = new List<BaseCollection>();
                collections.Add(bf);
                collector_loans.Add(bf.collector.collector, collections);
            }
        }
    }

    protected void filter()
    {
        bfs.Clear();
        foreach (BaseCollection bf in accepted_bfs)
        {
            if ((collector == "ALL" || collector == bf.collector.collector.ToUpper()) &&
                (loan_type == "ALL" || bf.loan.loan_type.Contains(loan_type)))
            {
                if (this.event_type == "ALL")
                {
                    bfs.Add(bf);
                }
                else
                {
                    foreach (com.sp.rmmc.common.models.events.Event e in bf.getEvents())
                    {
                        if (e.name == this.event_type || this.event_type == "ALL")
                        {
                            bfs.Add(bf);
                            break;
                        }
                    }
                }
                
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

    protected void populateddlCollector()
    {
        ListItem li = new ListItem("ALL");
        ddlCollectors.Items.Add(li);
        List<String> collectors = new List<string>();
        foreach (string c in (new LoanCollector()).get_last_30_days_collectors())
        {
            string name = c.Trim().ToUpper();
            if (name == "" || collectors.Contains(name)) continue;
            collectors.Add(name);
            ListItem new_li = new ListItem(name);
            if (this.collector == name) new_li.Selected = true;
            ddlCollectors.Items.Add(new_li);
        }
    }

    protected void populateddlEventType()
    {
        ListItem li = new ListItem("ALL");
        ddlEventType.Items.Add(li);
        List<String> event_types = new List<string>();
        foreach (BaseCollection bf in all_bfs)
        {
            foreach( com.sp.rmmc.common.models.events.Event ev in bf.getEvents())
            {
                bool is_new_event_type = true;

                foreach (string evt in event_types)
                {
                    if (ev.name == evt)
                    {
                        is_new_event_type = false;
                        break;
                    }
                }
                if (is_new_event_type)
                {
                    event_types.Add(ev.name);
                    ListItem new_li = new ListItem(ev.name);
                    if (this.event_type == ev.name) new_li.Selected = true;
                    ddlEventType.Items.Add(new_li);
                }
            }
        }
    }

    protected void ddlCollectors_SelectedIndexChanged(object sender, EventArgs e)
    {
        this.collector = ddlCollectors.SelectedValue.ToUpper();
        Response.Redirect(generate_url());
    }

    protected void ddlLoanType_SelectedIndexChanged(object sender, EventArgs e)
    {
        this.loan_type = ddlLoanType.SelectedValue.ToUpper();
        Response.Redirect(generate_url());
    }    

    protected string generate_url()
    {
        return "CollectionQueues.aspx" +
            "?workflow=" + workflow +
            "&sublist=" + sublist +
            "&collector=" + collector +
            "&loan_type=" + loan_type +
            "&event_type=" + event_type +
            "&version=" + version;
    }

    protected void ddlEventType_SelectedIndexChanged(object sender, EventArgs e)
    {
        this.event_type = ddlEventType.SelectedValue;
        Response.Redirect(generate_url());
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

    protected void exportCollectorList(string collector)
    {
        string filename = this.workflow + " collector " + collector + " " + DateTime.Today.ToString("MMMM yyyy") + " on ";
        if (this.workflow == "FHA VA 17-day Call Listing" ||
            this.workflow == "Fannie Mae 17 day Call Listing" ||
            this.workflow == "Fannie Mae 17-day MCM Call Listing" ||
            this.workflow == "HUD 3 Month Call Listing" ||
            this.workflow == "Fannie Mae 3 Month Call Listing" ||
            this.workflow == "VA 3 Month Call Listing" ||
            this.workflow == "FHA 60 Day Call Listing" ||
            this.workflow == "Fannie Mae 2 Month Call Listing" ||
            this.workflow == "VA 2 Month Call Listing")
        {
            CSVExport.writeHeadersAndCSV(filename, "Loan#,Loan Name,Due Date of next payment,Promised to Pay,Investor,Last Attempted Call Date,Collector", collector_loans[collector]);
        }
        else
        {
            CSVExport.writeHeadersAndCSV(filename, "Loan#, Loan Name, Due Date of next payment, Investor, Collector", collector_loans[collector]);
        }
    }

    protected void lbExport_Click(object sender, EventArgs e)
    {
        string filename = this.workflow + " " + DateTime.Today.ToString("MMMM yyyy") + " on ";
        if ( this.workflow == "Old Collector List")
        {
            CSVExport.writeHeadersAndCSV(filename, "Loan#,Loan Name,Due Date of next payment,Promised to Pay,Investor,Collector", this.accepted_bfs);
        }
        else if (this.workflow == "Fannie Mae 17 day Call Listing" ||
                 this.workflow == "Fannie Mae 17-day MCM Call Listing" ||
                 this.workflow == "FHA VA 17-day Call Listing" ||
                 this.workflow == "HUD 3 Month Call Listing" ||
                 this.workflow == "Fannie Mae 3 Month Call Listing" ||
                 this.workflow == "VA 3 Month Call Listing" ||
                 this.workflow == "FHA 60 Day Call Listing" ||
                 this.workflow == "Fannie Mae 2 Month Call Listing" ||
                 this.workflow == "VA 2 Month Call Listing")
        {  
            CSVExport.writeHeadersAndCSV(filename, "Loan#,Loan Name,Due Date of next payment,Promised to Pay,Investor,Last Attempted Call Date,Collector", this.accepted_bfs);
        }
        else
        {
            CSVExport.writeHeadersAndCSV(filename, "Loan#, Loan Name, Due Date of next payment, Investor, Collector", this.accepted_bfs);
        }
    }

    protected void lbExportReasonCode_Click(object sender, EventArgs e)
    {
        string filename = "Different Reason Codes " + DateTime.Today.ToString("MMMM yyyy") + " on ";
        CSVExport.writeHeadersAndCSV(filename, "Loan#, Loan Type, Loan Name, Due Date of next payment, UPB, Interest Rate, Occupancy, Reason Code, Memo Reason Code, 031 Date", this.bfs);
    }

    protected void lbExportPromisedToPay_Click(object sender, EventArgs e)
    {
        string filename = this.event_type + " " + DateTime.Today.ToString("MMMM yyyy") + " on ";
        CSVExport.writeHeadersAndCSV(filename, "Loan#, Loan Type, Loan Name, Due Date of next payment, Notify me date on Memo, Last 031 Date, UPB", this.bfs);
    }

}