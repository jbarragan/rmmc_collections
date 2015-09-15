using System;
using System.Collections.Generic;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data.SqlClient;

using System.Text.RegularExpressions;

using com.sp.rmmc.collections.models;
using com.sp.rmmc.common.models;
using com.sp.rmmc.common.models.events;

public partial class lossmitigation_Default : System.Web.UI.Page
{
    protected string loan_officer = "ALL";
    protected string workflow = "Demands";
    protected string loan_type = "ALL";
    protected string loan_id = "ALL";
    protected string version = "current";
    protected decimal decimal_loan_id = 0M;
    protected List<Event> events = new List<Event>();
    protected List<BaseCollection> bfs = new List<BaseCollection>();
    protected List<BaseCollection> all_bfs = new List<BaseCollection>();
    protected List<BaseCollection> accepted_bfs = new List<BaseCollection>();
    protected List<BaseCollection> removed_bfs = new List<BaseCollection>();
    protected History history = null;
    protected string output = "";

    protected void Page_Load(object sender, EventArgs e)
    {
        this.version = getArg("version", "current");
        int iverstion = stringToInt(version, 0);
        
        if (iverstion > 0)
        {
            history = (new History()).get(iverstion);
        }
        populateddlHistories();
        return;
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

    protected string generate_url()
    {
        return "History.aspx" +
            "?workflow=" + workflow +
            "&loan_officer=" + loan_officer +
            "&loan_id=" + loan_id +
            "&loan_type=" + loan_type +
            "&version=" + version;
    }

    protected void ddlHistories_SelectedIndexChanged(object sender, EventArgs e)
    {
        this.version = ddlHistories.SelectedValue;
        Response.Redirect(generate_url());
    }

    protected void populateddlHistories()
    {
        //ddlHistories.Items.Clear();
        if (ddlHistories.Items.Count > 0) return;
        ListItem li = new ListItem("Current", "current");
        ddlHistories.Items.Add(li);
        List<History> hs = (new History()).all();
        ListItem selected_li = li;
        foreach (History h in hs)
        {   
            ListItem new_li = new ListItem(h.history_text, h.id.ToString());
            if (this.history != null && this.history.id == h.id) selected_li = new_li;
            ddlHistories.Items.Add(new_li);
        }
        selected_li.Selected = true;
    }

    protected void btnSave_Click(object sender, EventArgs e)
    {
        lblSuccess.Text = ""; 
        lblError.Text = "";
        BaseCollection header = new CurrentCollection();
        history = new History();
        history.label = this.txtLabel.Text;
        history.history_date.set_date(DateTime.Today);
        history.insert();
        (new CurrentCollection()).getCollections(accepted_bfs, removed_bfs);
        if (history.id <= 0)
        {
            lblError.Text = "Error saving history: " + history.error;
        }
        string connection_string = System.Configuration.ConfigurationManager.ConnectionStrings["foreclosure"].ConnectionString;
        MsSqlModel model = new MsSqlModel();
        model.setConnectionString(connection_string);
        string insert_query = "INSERT INTO collection_history_loan_details (history_id, ";
        insert_query += String.Join(", ", header.columns_list().ToArray());
        insert_query += ") VALUES (@history_id, @";
        insert_query += String.Join(", @", header.columns_list().ToArray());
        insert_query += ")";
        accepted_bfs.AddRange(removed_bfs);
        try{
            foreach (BaseCollection bf in accepted_bfs)
            {
                SqlCommand cmd = model.getCommand();
                cmd.CommandText = insert_query;
                cmd.Parameters.AddWithValue("@history_id", history.id);
                bf.addInsertParameters(cmd);
                model.executeCommandStatement(cmd);
            }
        }catch(Exception exp){
            this.lblError.Text = "Error saving history: " + exp.Message + " - " + model.error; 
        }
        model.close_connection();
        if (model.error != "")
        {
            lblError.Text = "Error saving history: " + history.error;
        }
        else
        {
            lblSuccess.Text = "History stored successfully";
        }
    }
}