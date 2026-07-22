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
    protected string workflow = BaseCollection.FNMA_HAMP_REPORTING;
    protected string version = "current";
    protected string sublist = "ALL";
    protected string section = "Collections";
    protected List<BaseCollection> bfs = new List<BaseCollection>();
    protected List<BaseCollection> all_bfs = new List<BaseCollection>();
    protected List<BaseCollection> accepted_bfs = new List<BaseCollection>();
    protected List<BaseCollection> delinquencies = new List<BaseCollection>();
    protected List<BaseCollection> demands = new List<BaseCollection>();
    protected List<BaseCollection> foreclosures = new List<BaseCollection>();
    protected List<BaseCollection> removed_bfs = new List<BaseCollection>();
    protected History history = null;
    protected void Page_Load(object sender, EventArgs e)
    {
        this.workflow = getArg("workflow", this.workflow);
        this.version = getArg("version", "current");
        int iverstion = stringToInt(version, 0);
        this.section = getArg("section", "Collections");

        BaseCollection query_collection = new CurrentCollection();
        query_collection.getFNMAHAMPStopLoans(delinquencies, removed_bfs);
        foreach (CollectionGroup group in groupCollections(delinquencies))
        {
             group.populateTable(tblBackruptcies);
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
            if (c.type == BaseCollection.FNMA_HAMP_REPORTING)
            {
                string filename = BaseCollection.FANNIE_MAE_DELINQUENCY_REPORT  + DateTime.Today.ToString("MMMM yyyy") + " on ";
                CSVExport.writeHeadersAndCSV(filename, "Loan#,HAMP Service Number,Loan Name,Last Paid Due Date,UPB,Last Principal Payment Amount,Last Interest Payment Amount", this);
            }
        }

        public void populateTable(HtmlTable table)
        {
            if (this.Count == 0) return;
            // table.Rows.Add(getTitleRow());
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
            //cell.Controls.Add(this.lb);

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
            cell1.InnerHtml = "<b>HAMP Servicer Number</b>";
            row.Cells.Add(cell1);
            HtmlTableCell cell2 = new HtmlTableCell();
            cell2.InnerHtml = "<b>Loan Name</b>";
            row.Cells.Add(cell2);
            HtmlTableCell cell3 = new HtmlTableCell();
            cell3.InnerHtml = "<b>Last Paid Due Date</b>";
            row.Cells.Add(cell3);
            HtmlTableCell cell4 = new HtmlTableCell();
            cell4.InnerHtml = "<b>UPB</b>";
            row.Cells.Add(cell4);
            HtmlTableCell cell5 = new HtmlTableCell();
            cell5.InnerHtml = "<b>Last Principal Payment Amount</b>";
            row.Cells.Add(cell5);
            HtmlTableCell cell6 = new HtmlTableCell();
            cell6.InnerHtml = "<b>Last Payment Interest Amount</b>";
            row.Cells.Add(cell6);
            return row;
        }

        
        //<tr class="info"><td colspan="3"></td><td>Total</td><td><%=group.total_count%></td><td><%=group.total_amount.ToString("C") %></td></tr>
        public HtmlTableRow getTotalsRow()
        {
            HtmlTableRow row = new HtmlTableRow();
            HtmlTableCell cell0 = new HtmlTableCell();
            cell0.InnerHtml = "";
            cell0.ColSpan = 2;
            row.Cells.Add(cell0);
            HtmlTableCell cell3 = new HtmlTableCell();
            cell3.InnerHtml = "Total";
            row.Cells.Add(cell3);
            HtmlTableCell cell4 = new HtmlTableCell();
            cell4.InnerHtml = this.Count.ToString();
            row.Cells.Add(cell4);
            HtmlTableCell cell5 = new HtmlTableCell();
            cell5.InnerHtml = this.total_amount.ToString("C");
            cell5.ColSpan = 4;
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
            if (c.loan_id == 142184M)
                c.loan_id = 142184M;
            HtmlTableRow row = new HtmlTableRow();
            HtmlTableCell cell0 = new HtmlTableCell();
            cell0.InnerText = c.loan.loan_id.ToString();
            row.Cells.Add(cell0);
            HtmlTableCell cell1 = new HtmlTableCell();
            cell1.InnerHtml = "909389186";
            row.Cells.Add(cell1);
            HtmlTableCell cell3 = new HtmlTableCell();
            cell3.InnerHtml = c.loan.loan_name;
            row.Cells.Add(cell3);
            HtmlTableCell cell4 = new HtmlTableCell();
            cell4.InnerHtml = c.last_reg_due_dt.ToString();
            row.Cells.Add(cell4);
            HtmlTableCell cell5 = new HtmlTableCell();
            cell5.InnerHtml = c.loan.prin_bal.ToString("C");
            row.Cells.Add(cell5);
            HtmlTableCell cell6 = new HtmlTableCell();
            cell6.InnerHtml = c.getTotalAmt().ToString("C");
            row.Cells.Add(cell6);
            HtmlTableCell cell7 = new HtmlTableCell();
            cell7.InnerHtml = c.getTotalIntAmt().ToString("C"); ;
            row.Cells.Add(cell7);
            return row;
        }
    }

    protected List<CollectionGroup> groupCollections(List<BaseCollection> collections)
    {
        List<CollectionGroup> groups = new List<CollectionGroup>();
        CollectionGroup all = new CollectionGroup("ALL");
        groups.Add(all);

        foreach (BaseCollection c in collections)
        {
            all.addCollection(c);
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
        CSVExport.writeHeadersAndCSV(filename, "Loan#,HAMP Service Number,Loan Name,Last Paid Due Date,UPB,Last Principal Payment Amount,Last Interest Payment Amount", this.delinquencies);
    }
}