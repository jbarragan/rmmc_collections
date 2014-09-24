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
            <a href=""TaskList/Delinquent.aspx"">Task List</a>
          </li>
          <li>
            <a href=""Default.aspx"">Home</a>
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
            string active_class = "";
            foreach (Nav n in navs)
            {
                if (active == n.text) active_class = @" class=""active"""; else active_class = "";
                s += @"<li" + active_class +  @"><a href=""" + n.link + @""">" + n.text +"</a></li>";
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
            Nav nav = new Nav(); nav.link = "Delinquent.aspx"; nav.text = "Delinquent Queue"; navs.Add(nav);
            string active_class = "";
            foreach (Nav n in navs)
            {
                if (active == n.text) active_class = @" class=""active"""; else active_class = "";
                s += @"<li" + active_class + @"><a href=""" + n.link + @""">" + n.text + "</a></li>";
            }
            return s + nav_end;
        }
}