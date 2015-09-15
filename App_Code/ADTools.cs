using System;
using System.Data;
using System.Configuration;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;


using System.DirectoryServices;
using System.DirectoryServices.ActiveDirectory;


using System.Collections.Generic;
using System.Collections;


/// <summary>
/// Summary description for ADTools
/// </summary>
public class ADTools
{
    private static string errorDirEntry = "";

    private static string ADqueryGlobal = System.Configuration.ConfigurationManager.AppSettings["ADOCS"];

	public ADTools()
	{

	}

    private static DirectoryEntry getGlobalDirectoryEntry()
    {
        try
        {
            DirectoryEntry deSystem = new DirectoryEntry(ADqueryGlobal);
            deSystem.AuthenticationType = AuthenticationTypes.Secure;
            deSystem.Username = System.Configuration.ConfigurationManager.AppSettings["ADUser"];
            deSystem.Password = System.Configuration.ConfigurationManager.AppSettings["ADPass"];
            return deSystem;
        }
        catch (Exception e)
        {
            errorDirEntry = "Error on Global Directory Entry:" + e.Message;
            return null;
        }
    }

    public static string testGDE()
    {
        getGlobalDirectoryEntry();
        return errorDirEntry;
    }

    public static string getUser(string username, ADUser user)
    {
        try
        {
            DirectoryEntry gde = getGlobalDirectoryEntry();
            string strFilter = "(&(objectClass=user)(sAMAccountName=" + username + "))";
            DirectorySearcher ds = new DirectorySearcher(gde, strFilter);
            ds.SearchScope = SearchScope.Subtree;
            SearchResult sr = ds.FindOne();
            string objUsername = "";

            foreach (string str in sr.Properties["sAMAccountName"]) objUsername = str;

            if (objUsername.ToLower() == username.ToLower())
            {
                getADUserInfo(sr, user);
                return "Successful: Found";
            }
            else return "Successful: Not Found";
        }
        catch (Exception e)
        {
            return "Connection Error: " + errorDirEntry + " Error in getUser(): " + e.Message;
        }
    }


    public static string getUser(int usncreated, ADUser user)
    {
        try
        {
            DirectoryEntry gde = getGlobalDirectoryEntry();
            string strFilter = "(&(objectClass=user)(usncreated=" + usncreated.ToString() + "))";
            DirectorySearcher ds = new DirectorySearcher(gde, strFilter);
            ds.SearchScope = SearchScope.Subtree;
            SearchResult sr = ds.FindOne();

            getADUserInfo(sr, user);

            return "Successful: Found";
            
        }
        catch (Exception e)
        {
            return "Connection Error: " + errorDirEntry + " Error in getUser(): " + e.Message;
        }
    }

    private static string getADUserInfo(SearchResult sr, ADUser user)
    {
        try
        {
            foreach (string str in sr.Properties["sAMAccountName"]) user.username = str;
            foreach (string str in sr.Properties["givenname"]) user.first = str;
            foreach (string str in sr.Properties["sn"]) user.last = str;
            foreach (string str in sr.Properties["mail"]) user.email = str;
            foreach (string str in sr.Properties["department"]) user.department = str;
            foreach (Int64 i64 in sr.Properties["usncreated"]) user.usncreated = Convert.ToInt32(i64);
            foreach (string str in sr.Properties["memberOf"])
            {
                String[] group_components = str.Split(',');
                foreach (String s in group_components)
                    if (s.StartsWith("CN="))
                    {
                        user.groups.Add(s.Substring(3).ToLower());
                        break;
                    }
            }
            return "Successful";
        }
        catch (Exception e)
        {
            return "Error on getADUserInfo(): " + e.Message; 
        }
    }

    public static string getUsersStartWith(string startWith, ArrayList users)
    {
        return getUsersStartWith("sAMAccountName", startWith, users);
    }

    public static string getUsersStartWith(string searchParam, string startWith, ArrayList users)
    {
        try
        {
            DirectoryEntry gde = getGlobalDirectoryEntry();
            string strFilter = "(&(objectClass=user)(" + searchParam + "=" + startWith + "*))";
            DirectorySearcher ds = new DirectorySearcher(gde, strFilter);
            ds.SearchScope = SearchScope.Subtree;
            SearchResultCollection src = ds.FindAll();
            foreach (SearchResult sr in src)
            {
                ADUser user = new ADUser();
                getADUserInfo(sr, user);
                users.Add(user);
            }
            return "Successful: Found";

        }
        catch (Exception e)
        {
            return "Connection Error: " + errorDirEntry + " Error in getUsersStartWith(): " + e.Message;
        }
    }
    public static string getDomainUser(string domainUsername, ADUser user)
    {
        string domainName = System.Configuration.ConfigurationManager.AppSettings["userDomainName"];
        if (domainUsername.ToUpper().StartsWith(domainName.ToUpper()))
        {
            string username = domainUsername.Substring(domainName.Length + 1);
            return getUser(username, user);
        }
        else
        {
            user = new ADUser();
            return "Error on getDomainUser(): User is not part of the domain";
        }

    }
    

}
