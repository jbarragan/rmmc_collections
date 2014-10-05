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

    protected String type = "";
    protected String payment = "";
    protected String demand = "";
    protected String bankruptcy = "";
    protected String promised = "";
    protected String lm = "";
    protected String repayment = "";
    protected String nothing = "";
    
    protected void Page_Load(object sender, EventArgs e)
    {
        if (Page.IsPostBack) return;
        setArgumentsEnvironment();
        populateFilterOptions();
        all_in_three_month_delinquent = Delinquent.in_three_month_delinquent_mode();
        foreach (Delinquent d in all_in_three_month_delinquent)
        {
            if (this.type != "ANY" && d.loan.loan_type != this.type) continue;
            if (this.payment != "ANY" && d.last_paid_date_in_3_months.isNull == isYES(this.payment)) continue;
            if (this.demand != "ANY" && d.demand_letter_due_date.isNull == isYES(this.demand)) continue;
            if (this.bankruptcy != "ANY" && d.bankruptcy_filed_date.isNull == isYES(this.bankruptcy)) continue;
            if (this.promised != "ANY" && d.promised_to_pay_date.isNull == isYES(this.promised)) continue;
            if (this.lm != "ANY" && d.lm_reception.isNull == isYES(this.lm)) continue;
            if (this.repayment != "ANY" && d.repay_start_date.isNull == isYES(this.repayment)) continue;
            if (this.nothing != "ANY" && d.is_3_month_no_activity() != isYES(this.nothing)) continue;
            in_three_month_delinquent.Add(d);
            in_three_month_delinquent_total += d.loan.prin_bal;
        }    
    }

    protected bool isYES(String s)
    {
        return s.ToUpper() == "YES";
    }

    protected void setArgumentsEnvironment()
    {
        this.type = getArgument("type");
        this.payment = getArgument("payment");
        this.demand = getArgument("demand");
        this.bankruptcy = getArgument("bankruptcy");
        this.promised = getArgument("promised");
        this.lm = getArgument("lm");
        this.repayment = getArgument("repayment");
        this.nothing = getArgument("nothing");
    }

    protected String getArgument(String s)
    {
        String ret = Request.QueryString[s];
        return (ret != null && ret.Length > 0) ? ret : "ANY";
    }

    protected void populateFilterOptions()
    {
        String[] valid_types = { "ANY", "FHA", "VA", "CNV" };
        String[] yes_no_any = { "ANY", "YES", "NO" };

        populate(type, valid_types, ddlType);
        populate(payment, yes_no_any, ddlPayment);

        populate(demand, yes_no_any, ddlDemand);
        populate(bankruptcy, yes_no_any, ddlBankruptcy);
        populate(promised, yes_no_any, ddlPromised);
        populate(lm, yes_no_any, ddlLM);
        populate(repayment, yes_no_any, ddlRepayment);
        populate(nothing, yes_no_any, ddlNothing);
    }

    protected void populate(String value, String[] values, DropDownList ddl)
    {
        foreach (String s in values)
        {
            ListItem new_li = new ListItem(s);
            if (value == new_li.Text) new_li.Selected = true;
            ddl.Items.Add(new_li);
        }
    }

    protected String generateFilterURL()
    {
        return "DelinquentGrid.aspx" +
            "?type=" + ddlType.SelectedValue.ToUpper() +
            "&payment=" + ddlPayment.SelectedValue.ToUpper() +
            "&demand=" + ddlDemand.SelectedValue.ToUpper() +
            "&bankruptcy=" + ddlBankruptcy.SelectedValue.ToUpper() +
            "&promised=" + ddlPromised.SelectedValue.ToUpper() +
            "&lm=" + ddlLM.SelectedValue.ToUpper() +
            "&repayment=" + ddlRepayment.SelectedValue.ToUpper() +
            "&nothing=" + ddlNothing.SelectedValue.ToUpper();
    }


    protected void btn3MonthDelinquentToCSV_Click(object sender, EventArgs e)
    {
        CSVExport.writeCSV("3-month-delinquent" , all_in_three_month_delinquent);
    }

    protected void ddlSelectedIndexChanged(object sender, EventArgs e)
    {
        Response.Redirect(this.generateFilterURL());
    }
}