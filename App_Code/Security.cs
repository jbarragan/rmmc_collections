using System;
using System.Data;
using System.Configuration;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;

/// <summary>
/// Summary description for Security
/// </summary>
public class Security
{
	public Security()
	{
		//
		// TODO: Add constructor logic here
		//
	}

    public static string checkAccessSecurity(Page page)
    {
        ADUser user = new ADUser();
        ADTools.getDomainUser(HttpContext.Current.User.Identity.Name,user);

        string url = page.Request.Url.ToString();
        char[] split1 = { '?' };
        string[] urlArray = url.Split(split1);
        url = urlArray[0];
        char[] split2 = { '/' };
        urlArray = url.Split(split2); 
        int pos = urlArray.Length;
        string msg = "";
        while (pos >= 3)
        {
            //Check security
            msg += "<br/>Search: " + url + "%";
            
            url = url.Substring(0,url.Length - urlArray[--pos].Length -1);
            pos--;
        }
        return msg;
    }
}
