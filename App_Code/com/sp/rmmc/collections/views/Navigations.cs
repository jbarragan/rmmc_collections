using System;
using System.Collections.Generic;
using System.Web;

/// <summary>
/// Summary description for Navigations
/// </summary>
namespace com.sp.rmmc.collections.views
{
    public class Navigations
    {
        public Navigations()
        {
        }

        public static string main_nav =
    @"<div class=""navbar"">
       <div class=""navbar-inner"">
        <ul class=""nav"">
          <li>
            <a href=""Default.aspx"">Main Home</a>
          </li>
          <li>
            <a href=""TaskList/Default.aspx"">Task List</a>
          </li>
          <li>
            <a href=""events/Default.aspx"">Events</a>
          </li>
          <li>
            <a href=""epd/Default.aspx"">EPD</a>
          </li>
        </ul>
       </div>
      </div>";

        public static string sub_main_nav =
    @"<div class=""navbar"">
       <div class=""navbar-inner"">
        <ul class=""nav"">
          <li>
            <a href=""../Default.aspx"">Main Home</a>
          </li>
          <li>
            <a href=""../TaskList/Delinquent.aspx"">Task List</a>
          </li>
        </ul>
       </div>
      </div>";

        private static string nav_start = @"<div class=""navbar""><div class=""navbar-inner""><ul class=""nav"">";
        private static string nav_end = @"</ul></div></div>";
        class Nav
        {
            public string link = "";
            public string text = "";
        }

        public static string sub_main_nav_select(string active)
        {
            string s = nav_start;
            List<Nav> navs = new List<Nav>();
            Nav nav = new Nav(); nav.link = "../Default.aspx"; nav.text = "Main Home"; navs.Add(nav);
            nav = new Nav(); nav.link = "../TaskList/Delinquent.aspx"; nav.text = "Task List"; navs.Add(nav);
            nav = new Nav(); nav.link = "../Events/Default.aspx"; nav.text = "Events"; navs.Add(nav);
            nav = new Nav(); nav.link = "../epd/Default.aspx"; nav.text = "EPD"; navs.Add(nav);
            string active_class = "";
            foreach (Nav n in navs)
            {
                if (active == n.text) active_class = @" class=""active"""; else active_class = "";
                s += @"<li" + active_class + @"><a href=""" + n.link + @""">" + n.text + "</a></li>";
            }
            return s + nav_end;
        }


        public static string events_nav_select(string active, string version)
        {
            string s = nav_start;
            List<Nav> navs = new List<Nav>();
            Nav nav = new Nav(); nav.link = "Default.aspx?version=" + version; nav.text = "Reason Codes"; navs.Add(nav);
            nav = new Nav(); nav.link = "Default.aspx?workflow=Collections&sublist=ALL&loan_officer=ALL&loan_type=ALL&event_type=Promised%20To%20Pay%20Expired&version=" + version; nav.text = "Promised To Pay"; navs.Add(nav);
            nav = new Nav(); nav.link = "Default.aspx?workflow=Collections&sublist=ALL&loan_officer=ALL&loan_type=ALL&event_type=Using%20Current%20Promised%20To%20Pay%20Expired&version=" + version; nav.text = "Beta Promised To Pay using last current"; navs.Add(nav);
            nav = new Nav(); nav.link = "Default.aspx?workflow=4-Month%20Delinquent%20In%20Collections&sublist=ALL&loan_officer=ALL&loan_type=ALL&event_type=ALL&version=" + version; nav.text = "4-Month Delinquency Report In Collections"; navs.Add(nav);
            nav = new Nav(); nav.link = "Default.aspx?workflow=4-Month%20Delinquent%20NOT%20In%20Collections&sublist=ALL&loan_officer=ALL&loan_type=ALL&event_type=ALL&version=" + version; nav.text = "4-Month Delinquency Report NOT In Collections"; navs.Add(nav);
            nav = new Nav(); nav.link = "Default.aspx?workflow=FHA%20VA%2017-day%20Call%20Listing&sublist=ALL&collector=ALL&loan_type=ALL&event_type=ALL&version=" + version; nav.text = "FHA VA 17-day Call Listing"; navs.Add(nav);
            nav = new Nav(); nav.link = "Default.aspx?workflow=Fannie%20Mae%2017%20day%20Call%20Listing&sublist=ALL&collector=ALL&loan_type=ALL&event_type=ALL&version=" + version; nav.text = "Fannie Mae 17 day Call Listing"; navs.Add(nav);
            nav = new Nav(); nav.link = "Default.aspx?workflow=Fannie%20Mae%2017-day%20MCM%20Call%20Listing&sublist=ALL&collector=ALL&loan_type=ALL&event_type=ALL&version=" + version; nav.text = "Fannie Mae 17-day MCM Call Listing"; navs.Add(nav);
            nav = new Nav(); nav.link = "Default.aspx?workflow=FHA%2060%20Day%20Call%20Listing&sublist=ALL&collector=ALL&loan_type=ALL&event_type=ALL&version=" + version; nav.text = "FHA 60 Day Call Listing"; navs.Add(nav);
            nav = new Nav(); nav.link = "Default.aspx?workflow=HUD%203%20Month%20Call%20Listing&sublist=ALL&collector=ALL&loan_type=ALL&event_type=ALL&version=" + version; nav.text = "HUD 3 Month Call Listing"; navs.Add(nav);
            nav = new Nav(); nav.link = "History.aspx?version=" + version; nav.text = "History"; navs.Add(nav);
            string active_class = "";
            foreach (Nav n in navs)
            {
                if (active == n.text) active_class = @" class=""active"""; else active_class = "";
                s += @"<li" + active_class + @"><a href=""" + n.link + @""">" + n.text + "</a></li>";
            }
            return s + nav_end;
        }

        public static string task_list_nav =
    @"<div class=""navbar"">
       <div class=""navbar-inner"">
        <ul class=""nav"">
          <li>
            <a href=""Demand.aspx"">Demand Queue</a>
          </li>
          <li>
            <a href=""PreForeclosure.aspx"">Pre-Foreclosure Queue</a>
          </li>
          <li>
            <a href=""Foreclosure.aspx"">Foreclosure Queue</a>
          </li>
        </ul>
       </div>
      </div>";


        public static string task_list_nav_select(string active)
        {
            string s = nav_start;
            List<Nav> navs = new List<Nav>();
            Nav nav = new Nav(); nav.link = "Default.aspx"; nav.text = "Delinquent Flow"; navs.Add(nav);
            nav = new Nav(); nav.link = "Delinquent.aspx"; nav.text = "Delinquent Queue"; navs.Add(nav);
            nav = new Nav(); nav.link = "DelinquentFlow.aspx"; nav.text = "Delinquent Grid"; navs.Add(nav);
            string active_class = "";
            foreach (Nav n in navs)
            {
                if (active == n.text) active_class = @" class=""active"""; else active_class = "";
                s += @"<li" + active_class + @"><a href=""" + n.link + @""">" + n.text + "</a></li>";
            }
            return s + nav_end;
        }
    }
}