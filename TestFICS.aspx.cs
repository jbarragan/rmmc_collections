using System;
using System.Collections.Generic;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Data.OleDb;

public partial class TestFICs : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        string connectionString = @"Data Source=fics;Provider=SAOLEDB.12";
        OleDbConnection connection = null;
        connection = new OleDbConnection(connectionString);
        try
        {
            connection.Open();
            this.lblResult.Text = "Success";
        }
        catch (Exception exp)
        {
            this.lblResult.Text = exp.Message;
        }
    }
}