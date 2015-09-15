using System;
using System.Collections.Generic;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

using com.sp.rmmc.collections.models;

public partial class lossmitigation_Default : System.Web.UI.Page
{
    public string fileName = "";
    protected void Page_Load(object sender, EventArgs e)
    {
        string fullPath = /* System.Web.HttpContext.Current. (optional in most cases) */ Request.Url.AbsolutePath;
        fileName = System.IO.Path.GetFileName(fullPath).Replace("%20", " ").Replace(".aspx","") + " Event";
    }
}