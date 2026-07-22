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

    protected void Page_Load(object sender, EventArgs e)
    {
        this.workflow = getArg("workflow", "All");
        this.version = getArg("version", "current");
        int iverstion = stringToInt(version, 0);
        this.section = getArg("section", "Collections");

        BaseModification query = new BaseModification();
        query.getModificationAnalysisLoans(accepted_bfs, removed_bfs);
        
        int today_month = DateTime.Now.Year * 12 + DateTime.Now.Month;
        foreach (BaseModification m in accepted_bfs)
        {
            this.all.Add(m);
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

    protected void lbExport_ClickAll(object sender, EventArgs e)
    {
        string filename = "Modification Analysis Report - ";
        exportCsv(filename, this.all);
    }

    protected void exportCsv(String filename, ModificationGroup list)
    {
        CSVExport.writeHeadersAndCSV(filename, "Loan#,Loan Name,Mod Dt,Alert Desc,Modification Stop Desc,Loan Payment Stop Desc,Next Analysis Date,# Months Shortage,# Months Deficiency", list);
    }




}