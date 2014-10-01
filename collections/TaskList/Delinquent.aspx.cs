using System;
using System.Collections.Generic;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

using com.sp.rmmc.collections.models;
using com.sp.rmmc.common.tools;

public partial class lossmitigation_Collections_Default : System.Web.UI.Page
{
    protected List<Delinquent> all_in_three_month_delinquent = new List<Delinquent>();
    protected List<Delinquent> in_three_month_delinquent = new List<Delinquent>();
    protected Decimal in_three_month_delinquent_total = 0M;    
    protected List<Delinquent> in_three_month_delinquent_fha = new List<Delinquent>();
    protected Decimal in_three_month_delinquent_fha_total = 0M;
    protected List<Delinquent> in_three_month_delinquent_va = new List<Delinquent>();
    protected Decimal in_three_month_delinquent_va_total = 0M;
    protected List<Delinquent> in_three_month_delinquent_conv = new List<Delinquent>();
    protected Decimal in_three_month_delinquent_conv_total = 0M;
    
    protected void Page_Load(object sender, EventArgs e)
    {
        all_in_three_month_delinquent = Delinquent.in_three_month_delinquent_mode();
        foreach (Delinquent d in all_in_three_month_delinquent)
        {
            in_three_month_delinquent.Add(d);
            in_three_month_delinquent_total += d.loan.prin_bal;
            if (d.loan.loan_type == "FHA"){ in_three_month_delinquent_fha.Add(d); in_three_month_delinquent_fha_total += d.loan.prin_bal; }
            if (d.loan.loan_type == "VA"){ in_three_month_delinquent_va.Add(d); in_three_month_delinquent_va_total += d.loan.prin_bal; }
            if (d.loan.loan_type == "CNV" || d.loan.loan_type == "CONV" ){ in_three_month_delinquent_conv.Add(d); in_three_month_delinquent_conv_total += d.loan.prin_bal; }
        }    
    }

    protected void btn3MonthDelinquentToCSV_Click(object sender, EventArgs e)
    {
        CSVExport.writeCSV("3-month-delinquent" , all_in_three_month_delinquent);
    }
}