using System;
using System.Collections.Generic;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Text.RegularExpressions;
using System.Web.UI.HtmlControls;

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
        if (iverstion > 0)
        {
            history = (new History()).get(iverstion);
            if (    history != null) query_collection = new HistoryCollection(history);
        }

        if (this.workflow == "Bankruptcies" || workflow == "All")
        {
            query_collection.getBankruptcies(bankruptcies, removed_bfs);
            foreach (CollectionGroup group in groupCollections(bankruptcies))
            {
                group.populateTable(tblBackruptcies);
            }
        }

        if (this.workflow == "Demands" || workflow == "All")
        {
            query_collection.getDemands(demands, removed_bfs);
            foreach (CollectionGroup group in groupCollections(demands))
            {
                group.populateTable(tblDemands);
            }
        }

        if (this.workflow == "Foreclosures" || workflow == "All")
        {
            query_collection.getForeclosures(foreclosures, removed_bfs);
            foreach (CollectionGroup group in groupCollections(foreclosures))
            {
                group.populateTable(tblForeclosures);
            }

        }

        if (this.workflow == "FNMA")
        {
            query_collection.getFNMA3MonthDelinquent(accepted_bfs, removed_bfs);
        }

        if (this.workflow == "GNMA")
        {
            query_collection.getGNMA3MonthDelinquent(accepted_bfs, removed_bfs);
        }


    }

    protected List<CollectionGroup> groupGNMAByLoanTypes(List<BaseCollection> collections)
    {
        List<CollectionGroup> groups = new List<CollectionGroup>();
        CollectionGroup groupFHA = new CollectionGroup("FHA");
        CollectionGroup groupVA = new CollectionGroup("VA");
        groups.Add(groupFHA); groups.Add(groupVA);

        foreach (BaseCollection c in collections)
        {
            if (c.loan.loan_type == "FHA") groupFHA.addCollection(c);
            if (c.loan.loan_type == "VA") groupVA.addCollection(c);
        }
        return groups;
    }


    public class CollectionGroup : List<BaseCollection>
    {
        public string name = "";
        public int total_count = 0;
        public decimal total_amount = 0.0M;
        LinkButton lb = new LinkButton();

        public CollectionGroup(string name) : base()
        {
            this.name = name;
            lb.Text = "Export";
            lb.Click += new System.EventHandler(this.lb_click);
        }

        public void addCollection(BaseCollection collection)
        {
            this.total_amount += collection.loan.prin_bal;
            this.total_count++;
            this.Add(collection);
        }

        public void lb_click(object sender, EventArgs e)
        {
            if (this.Count == 0) return;
            BaseCollection c = this[0];
            if (c.type == BaseCollection.BANKRUPTCY_TYPE)
            {
                string filename = "Bankruptcies " + DateTime.Today.ToString("MMMM yyyy") + " on ";
                CSVExport.writeHeadersAndCSV(filename, "Loan#,Loan Name,Loan Type,Due Date,Days Delinquent,UPB", this);
            }
            if (c.type == BaseCollection.DEMAND_TYPE)
            {
                string filename = "Demands " + DateTime.Today.ToString("MMMM yyyy") + " on ";
                CSVExport.writeHeadersAndCSV(filename, "Loan#,Loan Name,Loan Type,Due Date,Days Delinquent,UPB", this);
            }
            if (c.type == BaseCollection.FORECLOSURE_TYPE)
            {
                string filename = "Foreclosures " + DateTime.Today.ToString("MMMM yyyy") + " on ";
                CSVExport.writeHeadersAndCSV(filename, "Loan#,Loan Name,Loan Type,Due Date,Days Delinquent,UPB", this);
            }
        }

        public void populateTable(HtmlTable table)
        {
            if (this.Count == 0) return;
            table.Rows.Add(getTitleRow());
            table.Rows.Add(getCollectionHeaderRow());
            foreach (HtmlTableRow r in getCollectionRows()) table.Rows.Add(r);
            table.Rows.Add(getTotalsRow());
        }

        public HtmlTableRow getTitleRow()
        {
            HtmlTableRow row = new HtmlTableRow();
            HtmlTableCell cell = new HtmlTableCell();
            cell.ColSpan = 6;
            cell.InnerHtml = "<b>" + this.name + "</b> - ";
            row.Cells.Add(cell);
            cell.Controls.Add(this.lb);

            row.Attributes.Add("class", "success");
            return row;
        }

        public HtmlTableRow getCollectionHeaderRow()
        {
            HtmlTableRow row = new HtmlTableRow();
            HtmlTableCell cell0 = new HtmlTableCell();
            cell0.InnerHtml = "<b>Loan #</b>";
            row.Cells.Add(cell0);
            HtmlTableCell cell1 = new HtmlTableCell();
            cell1.InnerHtml = "<b>Loan Name</b>";
            row.Cells.Add(cell1);
            HtmlTableCell cell2 = new HtmlTableCell();
            cell2.InnerHtml = "<b>Loan Type</b>";
            row.Cells.Add(cell2);
            HtmlTableCell cell3 = new HtmlTableCell();
            cell3.InnerHtml = "<b>Due Date</b>";
            row.Cells.Add(cell3);
            HtmlTableCell cell4 = new HtmlTableCell();
            cell4.InnerHtml = "<b>Days Delinquent</b>";
            row.Cells.Add(cell4);
            HtmlTableCell cell5 = new HtmlTableCell();
            cell5.InnerHtml = "<b>UPB</b>";
            row.Cells.Add(cell5);
            return row;
        }

        
        //<tr class="info"><td colspan="3"></td><td>Total</td><td><%=group.total_count%></td><td><%=group.total_amount.ToString("C") %></td></tr>
        public HtmlTableRow getTotalsRow()
        {
            HtmlTableRow row = new HtmlTableRow();
            HtmlTableCell cell0 = new HtmlTableCell();
            cell0.InnerHtml = "";
            cell0.ColSpan = 3;
            row.Cells.Add(cell0);
            HtmlTableCell cell3 = new HtmlTableCell();
            cell3.InnerHtml = "Total";
            row.Cells.Add(cell3);
            HtmlTableCell cell4 = new HtmlTableCell();
            cell4.InnerHtml = this.Count.ToString();
            row.Cells.Add(cell4);
            HtmlTableCell cell5 = new HtmlTableCell();
            cell5.InnerHtml = this.total_amount.ToString("C");
            row.Cells.Add(cell5);
            row.Attributes.Add("class", "info");
            return row;
        }

        public List<HtmlTableRow> getCollectionRows()
        {
            List<HtmlTableRow> rows = new List<HtmlTableRow>();
            foreach(BaseCollection c in this ) rows.Add(getCollectionRow(c));
            return rows;
        }

        public HtmlTableRow getCollectionRow(BaseCollection c)
        {
            HtmlTableRow row = new HtmlTableRow();
            HtmlTableCell cell0 = new HtmlTableCell();
            cell0.InnerText = c.loan.loan_id.ToString();
            row.Cells.Add(cell0);
            HtmlTableCell cell1 = new HtmlTableCell();
            cell1.InnerHtml = c.loan.loan_name;
            row.Cells.Add(cell1);
            HtmlTableCell cell2 = new HtmlTableCell();
            cell2.InnerHtml = c.loan.loan_type;
            row.Cells.Add(cell2);
            HtmlTableCell cell3 = new HtmlTableCell();
            cell3.InnerHtml = c.loan.due_date_next_payment.ToString();
            row.Cells.Add(cell3);
            HtmlTableCell cell4 = new HtmlTableCell();
            cell4.InnerHtml = (c.months_late() * 30).ToString();
            row.Cells.Add(cell4);
            HtmlTableCell cell5 = new HtmlTableCell();
            cell5.InnerHtml = c.loan.prin_bal.ToString("C");
            row.Cells.Add(cell5);
            return row;
        }
    }

    protected List<CollectionGroup> groupCollections(List<BaseCollection> collections)
    {
        List<CollectionGroup> groups = new List<CollectionGroup>();
        CollectionGroup group30 = new CollectionGroup("30-day Delinquent");
        CollectionGroup group60 = new CollectionGroup("60-day Delinquent");
        CollectionGroup group90 = new CollectionGroup("90-day Delinquent");
        CollectionGroup group120 = new CollectionGroup("120+day Delinquent");
        groups.Add(group30); groups.Add(group60); groups.Add(group90); groups.Add(group120);

        foreach(BaseCollection c in collections){
            switch (c.months_late())
            {
                case 0:
                    break;
                case 1:
                    group30.addCollection(c);
                    break;
                case 2:
                    group60.addCollection(c);
                    break;
                case 3:
                    group90.addCollection(c);
                    break;
                default:
                    group120.addCollection(c);
                    break;
            }
        }
        return groups;

    }

    protected void filter()
    {
        bfs.Clear();
        foreach (BaseCollection bf in accepted_bfs)
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

    protected void lbExport_Click(object sender, EventArgs e)
    {
        string filename = this.workflow + " " + DateTime.Today.ToString("MMMM yyyy") + " on ";
        CSVExport.writeHeadersAndCSV(filename, "Loan#, Loan Name, Due Date of next payment, Investor, Collector", this.accepted_bfs);
    }

    protected void lbExportBankruptcies_Click(object sender, EventArgs e)
    {
        string filename = "Bankruptcies " + DateTime.Today.ToString("MMMM yyyy") + " on ";
        CSVExport.writeHeadersAndCSV(filename, "Loan#,Loan Name,Loan Type,Due Date,Days Delinquent,UPB", this.bankruptcies);
    }

    protected void lbExportDemands_Click(object sender, EventArgs e)
    {
        string filename = "Demands " + DateTime.Today.ToString("MMMM yyyy") + " on ";
        CSVExport.writeHeadersAndCSV(filename, "Loan#,Loan Name,Loan Type,Due Date,Days Delinquent,UPB", this.demands);
    }

    protected void lbExportForeclosures_Click(object sender, EventArgs e)
    {
        string filename = "Foreclosures " + DateTime.Today.ToString("MMMM yyyy") + " on ";
        CSVExport.writeHeadersAndCSV(filename, "Loan#,Loan Name,Loan Type,Due Date,Days Delinquent,UPB", this.foreclosures);
    }

    protected void lbExportFNMA_Click(object sender, EventArgs e)
    {
        string filename = "FNMA " + DateTime.Today.ToString("MMMM yyyy") + " on ";
        CSVExport.writeHeadersAndCSV(filename, "Loan#,Loan Name,UPB", this.accepted_bfs);
    }

    protected void lbExportGNMA_Click(object sender, EventArgs e)
    {
        string filename = "GNMA " + DateTime.Today.ToString("MMMM yyyy") + " on ";
        CSVExport.writeHeadersAndCSV(filename, "Loan#,Loan Name,Loan Type,UPB", this.accepted_bfs);
    }

}