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

    public class ModificationGroup : List<BaseModification>
    {
        public String title = "";
        public ModificationGroup(String title)
        {
            this.title = title;
        }
        public decimal Sum
        {
            get
            {
                decimal sum = 0.0M;
                for (int i = 0; i < this.Count; i++)
                {
                    BaseModification m = (BaseModification)this[i];
                    sum = sum + m.loan.prin_bal;
                }
                return sum;
            }
        }
    }

    protected ModificationGroup all = new ModificationGroup("All");
    protected ModificationGroup with_in_one_year = new ModificationGroup("Within One Year");
    protected ModificationGroup with_in_one_year_90plus = new ModificationGroup("90 Days Plus");
    protected ModificationGroup with_in_one_year_60 = new ModificationGroup("60 Days");
    protected ModificationGroup with_in_one_year_30 = new ModificationGroup("30 Days");
    protected ModificationGroup with_in_one_year_current = new ModificationGroup("Current");

    protected ModificationGroup one_year_or_more = new ModificationGroup("One Year or More");
    protected ModificationGroup one_year_or_more_90plus = new ModificationGroup("90 Days Plus");
    protected ModificationGroup one_year_or_more_60 = new ModificationGroup("60 Days");
    protected ModificationGroup one_year_or_more_30 = new ModificationGroup("30 Days");
    protected ModificationGroup one_year_or_more_current = new ModificationGroup("Current");

    protected void Page_Load(object sender, EventArgs e)
    {
        this.workflow = getArg("workflow", "All");
        this.version = getArg("version", "current");
        int iverstion = stringToInt(version, 0);
        this.section = getArg("section", "Collections");

        BaseModification query = new BaseModification();
        query.getAll(accepted_bfs, removed_bfs);
        
        int today_month = DateTime.Now.Year * 12 + DateTime.Now.Month;
        foreach (BaseModification m in accepted_bfs)
        {
            this.all.Add(m);
            int eff_month = m.mod_eff_date.date.Month + m.mod_eff_date.date.Year*12;
            int due_month = m.loan.due_date_next_payment.date.Month + m.loan.due_date_next_payment.date.Year*12;
            if (today_month - eff_month >= 12)
            {
                one_year_or_more.Add(m);
                if (due_month >= today_month) one_year_or_more_current.Add(m);
                else if (due_month == today_month - 1) one_year_or_more_30.Add(m);
                else if (due_month == today_month - 2) one_year_or_more_60.Add(m);
                else one_year_or_more_90plus.Add(m);
            }
            else
            {
                with_in_one_year.Add(m);
                if (due_month >= today_month) with_in_one_year_current.Add(m);
                else if (due_month == today_month - 1) with_in_one_year_30.Add(m);
                else if (due_month == today_month - 2) with_in_one_year_60.Add(m);
                else with_in_one_year_90plus.Add(m);
            }
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

    protected void lbExport_ClickOneYearOrMore90(object sender, EventArgs e)
    {
        string filename = "Mod Completed Report - One Year or More - " + this.one_year_or_more_90plus.title + " ";
        exportCsv(filename, this.one_year_or_more_90plus);
    }

    protected void lbExport_ClickOneYearOrMore60(object sender, EventArgs e)
    {
        string filename = "Mod Completed Report - One Year or More - " + this.one_year_or_more_60.title + " ";
        exportCsv(filename, this.one_year_or_more_60);
    }

    protected void lbExport_ClickOneYearOrMore30(object sender, EventArgs e)
    {
        string filename = "Mod Completed Report - One Year or More - " + this.one_year_or_more_30.title + " ";
        exportCsv(filename, this.one_year_or_more_30);
    }

    protected void lbExport_ClickOneYearOrMoreCurrent(object sender, EventArgs e)
    {
        string filename = "Mod Completed Report - Within One Year - " + this.one_year_or_more_current.title + " ";
        exportCsv(filename, this.one_year_or_more_current);
    }

    protected void lbExport_ClickWithYear90(object sender, EventArgs e)
    {
        string filename = "Mod Completed Report - Within One Year - " + this.with_in_one_year_90plus.title + " ";
        exportCsv(filename, this.with_in_one_year_90plus);
    }

    protected void lbExport_ClickWithYear60(object sender, EventArgs e)
    {
        string filename = "Mod Completed Report - Within One Year - " + this.with_in_one_year_60.title + " ";
        exportCsv(filename, this.with_in_one_year_60);
    }

    protected void lbExport_ClickWithYear30(object sender, EventArgs e)
    {
        string filename = "Mod Completed Report - Within One Year - " + this.with_in_one_year_30.title + " ";
        exportCsv(filename, this.with_in_one_year_30);
    }

    protected void lbExport_ClickWithYearCurrent(object sender, EventArgs e)
    {
        string filename = "Mod Completed Report - Within Year - " + this.with_in_one_year_current.title + " ";
        exportCsv(filename, this.with_in_one_year_current);
    }

    protected void exportCsv(String filename, ModificationGroup list)
    {
        CSVExport.writeHeadersAndCSV(filename, "Loan#,invagency,Loan Name,Loan Type,Mod Plan,Hamp YN?,Mod Dt,Mod Eff Dt,Due Date,Mod Term,Mod UPB,Mod Rate,Mod P&I,Mod T&I,Mod Mat Dt,Alert Desc", list);
    }




}