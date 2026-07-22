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
        query_collection.getBankruptciesTxVetNationStar(delinquencies, removed_bfs);
        foreach (CollectionGroup group in groupCollections(delinquencies))
        {
            group.populateTable(tblBackruptcies);
        }
    }

    protected List<CollectionGroup> groupGNMAByLoanTypes(List<BaseCollection> collections)
    {
        List<CollectionGroup> groups = new List<CollectionGroup>();
        CollectionGroup groupTXVET = new CollectionGroup("TX VET", this);
        groups.Add(groupTXVET);

        foreach (BaseCollection c in collections)
        {
            groupTXVET.addCollection(c);
        }
        return groups;
    }


    public class CollectionGroup : List<BaseCollection>
    {
        public string name = "";
        public int total_count = 0;
        public decimal total_amount = 0.0M;
        public lossmitigation_Default parent;
        LinkButton lb = new LinkButton();

        public CollectionGroup(string name, lossmitigation_Default parent)
            : base()
        {
            this.name = name;
            this.parent = parent;
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
            if (c.type == BaseCollection.NATION_STAR_PDF)
            {
                string filename = BaseCollection.NATION_STAR_PDF + DateTime.Today.ToString("MMMM yyyy") + " on ";
                CSVExport.writeHeadersAndCSV(filename, "Loan#,Investor,Loan Name,Due Date,Mortgage Status,Reason Code", this);
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
            cell.ColSpan = 7;
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
            cell0 = new HtmlTableCell();
            cell0.InnerHtml = "<b>Investor Loan#</b>";
            row.Cells.Add(cell0);
            HtmlTableCell cell1 = new HtmlTableCell();
            cell1.InnerHtml = "<b>Investor</b>";
            row.Cells.Add(cell1);
            HtmlTableCell cell2 = new HtmlTableCell();
            cell2.InnerHtml = "<b>Loan Name</b>";
            row.Cells.Add(cell2);
            HtmlTableCell cell3 = new HtmlTableCell();
            cell3.InnerHtml = "<b>Due Date</b>";
            row.Cells.Add(cell3);
            HtmlTableCell cell4 = new HtmlTableCell();
            cell4.InnerHtml = "<b>View PDF</b>";
            row.Cells.Add(cell4);
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
            cell0 = new HtmlTableCell();
            cell0.InnerText = c.inv_loan_nbr;
            row.Cells.Add(cell0);
            HtmlTableCell cell1 = new HtmlTableCell();
            cell1.InnerHtml = c.loan.inv_cd;
            row.Cells.Add(cell1);
            HtmlTableCell cell3 = new HtmlTableCell();
            cell3.InnerHtml = c.loan.loan_name;
            row.Cells.Add(cell3);
            HtmlTableCell cell4 = new HtmlTableCell();
            cell4.InnerHtml = c.loan.due_date_next_payment.ToString();
            row.Cells.Add(cell4);
            HtmlTableCell cell5 = new HtmlTableCell();
            cell5.InnerHtml = "<a href='NationStarGeneratePdf.aspx?loan_id=" + c.loan.loan_id.ToString() + "'>View PDF</a>";
            row.Cells.Add(cell5);
            return row;
        }
    }

    protected List<CollectionGroup> groupCollections(List<BaseCollection> collections)
    {
        List<CollectionGroup> groups = new List<CollectionGroup>();
        CollectionGroup groupTXVET = new CollectionGroup("TXVET", this);
        groups.Add(groupTXVET);
        foreach(BaseCollection c in collections){
            groupTXVET.addCollection(c);
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
        CSVExport.writeHeadersAndCSV(filename, "Loan#,Investor Loan#,Investor,Loan Name,Due Date,Mortgage Status,Reason Code", this.delinquencies);
    }

    protected void lbExportBankruptcies_Click(object sender, EventArgs e)
    {
        string filename = "Bankruptcies " + DateTime.Today.ToString("MMMM yyyy") + " on ";
        CSVExport.writeHeadersAndCSV(filename, "Loan#,Loan Name,Loan Type,Due Date,Days Delinquent,UPB", this.delinquencies);
    }
    protected void generatePDFBtn_Click(object sender, EventArgs e)
    {
        try
        {
            this.lblError.Text = "";
            string loan_number = this.loanNumberTxt.Text;
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