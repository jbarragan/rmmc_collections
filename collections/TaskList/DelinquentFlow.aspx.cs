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
    protected String letter = "";
    protected String inspection = "";
    protected String calldate = "";
    protected String callresult = "";
    
    protected void Page_Load(object sender, EventArgs e)
    {
        if (Page.IsPostBack) return;
        setArgumentsEnvironment();
        populateFilterOptions();
        all_in_three_month_delinquent = Delinquent.in_three_month_delinquent_mode();
        foreach (Delinquent d in all_in_three_month_delinquent)
        {
            if (this.type != "ANY" && d.loan.loan_type != this.type) continue;
            if (this.letter != "ANY" && d.last_mail_letter_valid() != isVALID(this.letter)) continue;
            if (this.inspection != "ANY" && d.last_inspection_valid() != isVALID(this.inspection)) continue;
            if (this.calldate != "ANY" && d.last_call_date_valid() != isVALID(this.calldate)) continue;
            if (this.callresult != "ANY" && ((isCONTACT(this.callresult) && d.last_call_result() != "Contact") || (isCONTACT(this.callresult) == false && d.last_call_result() != "No Contact"))) continue;
            in_three_month_delinquent.Add(d);
            in_three_month_delinquent_total += d.loan.prin_bal;
        }    
    }

    protected bool isYES(String s)
    {
        return s.ToUpper() == "YES";
    }

    protected bool isVALID(String s)
    {
        return s.ToUpper() == "VALID";
    }

    protected bool isCONTACT(String s)
    {
        return s.ToUpper() == "CONTACT";
    }

    protected void setArgumentsEnvironment()
    {
        this.type = getArgument("type");
        this.letter = getArgument("letter");
        this.inspection = getArgument("inspection");
        this.calldate = getArgument("calldate");
        this.callresult = getArgument("callresult");
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
        String[] valid_invalid_any = { "ANY", "VALID", "INVALID" };
        String[] contact_no_contact_any = { "ANY", "CONTACT", "NO CONTACT" };

        populate(type, valid_types, ddlType);
        populate(letter, valid_invalid_any, ddlLastMailLetter);

        populate(inspection, valid_invalid_any, ddlInspection);
        populate(calldate, valid_invalid_any, ddlLastCall);
        populate(callresult, contact_no_contact_any, ddlLastCallResult);
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
        return "DelinquentFlow.aspx" +
            "?type=" + ddlType.SelectedValue.ToUpper() +
            "&letter=" + ddlLastMailLetter.SelectedValue.ToUpper() +
            "&inspection=" + ddlInspection.SelectedValue.ToUpper() +
            "&calldate=" + ddlLastCall.SelectedValue.ToUpper() +
            "&callresult=" + ddlLastCallResult.SelectedValue.ToUpper();
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