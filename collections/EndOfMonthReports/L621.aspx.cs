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


    public class ForbearanceGroup : List<BaseForbearance>
    {
        public String title = "";
        private decimal _sum = 0.0M;
        public DateTime start_date = new DateTime();
        public DateTime end_date = new DateTime();
        public ForbearanceGroup(String title)
        {
            this.title = title;
        }
        public decimal Sum
        {
            get
            {
                if( this._sum > 0M ) return _sum;
                decimal sum = 0.0M;
                for (int i = 0; i < this.Count; i++)
                {
                    BaseForbearance m = (BaseForbearance)this[i];
                    sum = sum + m.loan.prin_bal;
                }
                this._sum = sum;
                return sum;
            }
        }
        public decimal Avg
        {
            get
            {
                if (this.Count <= 0) return 0M;
                return this.Sum / this.Count;
            }
        }

        public string getQuarterTitle()
        {
            int quarter = (this.start_date.Month + 2)/3;
            return title + " Q" + quarter + " (" + this.start_date.ToShortDateString() + " - " + this.end_date.ToShortDateString() + ")";
        }

        public string getMetrics()
        {
            return "(Count: " + this.Count + " UPB: " + this.Sum.ToString("C") + " Avg: " + this.Avg.ToString("C") + ")";
        }
    }

    protected ForbearanceGroup all_current = new ForbearanceGroup("Current Quarter Forbearance");
    protected ForbearanceGroup current_fha = new ForbearanceGroup("FHA");
    protected ForbearanceGroup current_cnv = new ForbearanceGroup("Fannie Mae");
    protected ForbearanceGroup current_va = new ForbearanceGroup("VA");

    protected ForbearanceGroup all_previous = new ForbearanceGroup("Previous Quarter Forbearance");
    protected ForbearanceGroup previous_fha = new ForbearanceGroup("FHA");
    protected ForbearanceGroup previous_cnv = new ForbearanceGroup("Fannie Mae");
    protected ForbearanceGroup previous_va = new ForbearanceGroup("VA");


    protected List<BaseForbearance> accepted = new List<BaseForbearance>();
    protected List<BaseForbearance> removed = new List<BaseForbearance>();
    protected List<BaseForbearance> previous_quarter_accepted = new List<BaseForbearance>();
    protected List<BaseForbearance> previous_quarter_removed = new List<BaseForbearance>();

    protected void Page_Load(object sender, EventArgs e)
    {
        this.workflow = getArg("workflow", "All");
        this.version = getArg("version", "current");
        int iverstion = stringToInt(version, 0);
        this.section = getArg("section", "Collections");

        DateTime now = DateTime.Now;
        int current_quarter = (int)((now.Month + 2) / 3);
        int previous_quarter = ((current_quarter + 2) % 4) + 1;

        DateTime quarter_date = now;
        DateTime quarter_start = new DateTime(quarter_date.Year, current_quarter * 3 - 2, 1);
        DateTime quarter_end = quarter_start.AddMonths(3).AddHours(-1);

        DateTime previous_quarter_date = now.AddMonths(-3);
        DateTime previous_quarter_start = new DateTime(previous_quarter_date.Year, previous_quarter * 3 - 2, 1);
        DateTime previous_quarter_end = previous_quarter_start.AddMonths(3).AddHours(-1);

        BaseForbearance forberance_query = new BaseForbearance();
        forberance_query.getFromToDates(quarter_start, quarter_end, accepted, removed);
        forberance_query.getFromToDates(previous_quarter_start, previous_quarter_end, previous_quarter_accepted, previous_quarter_removed);

        this.all_current.start_date = quarter_start;
        this.all_current.end_date = quarter_end;
        foreach (BaseForbearance f in removed)
        {
            if ((f.borrower_request_date.date.Year * 12 + f.borrower_request_date.date.Month) -
                   (f.min_due_date.date.Year * 12 + f.min_due_date.date.Month) != 2) continue;
            if (f.loan.loan_type.Contains("CNV")) continue;
            this.all_current.Add(f);
            if (f.loan.loan_type.Contains("FHA")) this.current_fha.Add(f);
            if (f.loan.loan_type.Contains("VA")) this.current_va.Add(f);
            if (f.loan.loan_type.Contains("CNV")) this.current_cnv.Add(f);
        }
        this.all_previous.start_date = previous_quarter_start;
        this.all_previous.end_date = previous_quarter_end;
        foreach (BaseForbearance f in previous_quarter_removed)
        {
            if ((f.borrower_request_date.date.Year * 12 + f.borrower_request_date.date.Month) -
                   (f.min_due_date.date.Year * 12 + f.min_due_date.date.Month) != 2) continue;
            if (f.loan.loan_type.Contains("CNV")) continue;
            this.all_previous.Add(f);
            if (f.loan.loan_type.Contains("FHA")) this.previous_fha.Add(f);
            if (f.loan.loan_type.Contains("VA")) this.previous_va.Add(f);
            if (f.loan.loan_type.Contains("CNV")) this.previous_cnv.Add(f);
        }

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

    protected void lbExport_ClickAllCurrent(object sender, EventArgs e)
    {
        string filename = this.all_current.getQuarterTitle() + "-";
        exportCsv(filename, this.all_current);
    }

    protected void lbExport_ClickCurrentFHA(object sender, EventArgs e)
    {
        string filename = this.current_fha.title + "-";
        exportCsv(filename, this.current_fha);
    }

    protected void lbExport_ClickCurrentVA(object sender, EventArgs e)
    {
        string filename = this.current_va.title + "-";
        exportCsv(filename, this.current_va);
    }

    protected void lbExport_ClickCurrentCNV(object sender, EventArgs e)
    {
        string filename = this.current_cnv.title + "-";
        exportCsv(filename, this.current_cnv);
    }

    protected void lbExport_ClickAllPrevious(object sender, EventArgs e)
    {
        string filename = this.all_previous.getQuarterTitle() + "-";
        exportCsv(filename, this.all_previous);
    }

    protected void lbExport_ClickPreviousFHA(object sender, EventArgs e)
    {
        string filename = this.previous_fha.title + "-";
        exportCsv(filename, this.previous_fha);
    }

    protected void lbExport_ClickPreviousVA(object sender, EventArgs e)
    {
        string filename = this.previous_va.title + "-";
        exportCsv(filename, this.previous_va);
    }

    protected void lbExport_ClickPreviousCNV(object sender, EventArgs e)
    {
        string filename = this.previous_cnv.title + "-";
        exportCsv(filename, this.previous_cnv);
    }

    protected void exportCsv(String filename, ForbearanceGroup list)
    {
        CSVExport.writeHeadersAndCSV(filename, "Loan#,Loan Name,Loan Type,UPB,Forbearance Plan,Forbearance Start Date,Forbearance Borrower Request,Alert Desc", list);
    }




}