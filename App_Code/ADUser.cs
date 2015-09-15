using System;
using System.Data;
using System.Configuration;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
using System.Collections.Generic;

/// <summary>
/// Summary description for ADUser
/// </summary>
public class ADUser
{
    public int id = -1;
    public string username = "";
    public string first = "";
    public string last = "";
    public string department = "";

    public string email = "";

    public int usncreated = -1;

    public List<String> groups = new List<string>();

	public ADUser()
	{
	}


    public ADUser(string context_identity)
    {
        ADTools.getDomainUser(context_identity, this);
    }

    public string getXMLData()
    {
        return "<ADUser>" +
                "<username>" + this.username + "</username>" +
                "<first>" + this.first + "</first>" +
                "<last>" + this.last + "</last>" +
                "<department>" + this.department + "</department>" +
                "<email>" + this.email + "</email>" +
                "<usncreated>" + this.usncreated.ToString() + "</usncreated></ADUser>";
    }

    public string getHTMLData()
    {
        String s = "User:<br/>" +
                "username: " + this.username + "<br/>" +
                "first: " + this.first + "<br/>" +
                "last: " + this.last + "</last><br/>" +
                "department: " + this.department + "</department><br/>" +
                "email: " + this.email + "<br/>" +
                "usncreated: " + this.usncreated.ToString() + "<br/>groups:<br/>";
        foreach (String g in groups) s += "group: " + g + "<br/>";
        return s + "<br/>";
    }

    public void fillUser(ADUser sourceUser)
    {
        this.id = sourceUser.id;
        this.username = sourceUser.username;
        this.first = sourceUser.first;
        this.last = sourceUser.last;
        this.department = sourceUser.department;
        this.email = sourceUser.email;
        this.usncreated = sourceUser.usncreated;
    }

    public bool inGroup(String group)
    {
        foreach (String user_group in this.groups)
            if (user_group.ToLower() == group.ToLower()) return true;

        return false;
    }

    public bool inAnyUser(List<string> look_into_users)
    {
        foreach (String u in look_into_users)
            if (this.username.ToLower() == u.ToLower()) return true;
        return false;
    }

    public bool inAnyDepartments(List<string> look_into_departments)
    {
        foreach (String d in look_into_departments)
            if (this.department.ToLower() == d.ToLower()) return true;
        return false;
    }

    public bool inAnyGroups(List<string> look_into_groups)
    {
        foreach (String g in look_into_groups)
            if (this.inGroup(g.ToLower())) return true;
        return false;
    }


}
