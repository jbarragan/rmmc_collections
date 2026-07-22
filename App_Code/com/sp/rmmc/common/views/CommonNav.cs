using System;
using System.Collections.Generic;
using System.Web;

namespace com.sp.rmmc.common.views
{
    public class CommonNav
    {
        public string link = "";
        public string text = "";
        public List<string> ad_users = new List<string>();
        public List<string> ad_departments = new List<string>();
        public List<string> ad_groups = new List<string>();

        public static string nav_start = @"<div class=""navbar""><div class=""navbar-inner""><ul class=""nav"">";
        public static string nav_end = @"</ul></div></div>";

        public string link_html(string selected_text, ADUser ad_user)
        {
            string s = "";
            if (with_access(ad_user) == false) return s;
            string active_class = "";
            if (selected_text.ToUpper() == text.ToUpper()) active_class = @" class=""active""";
            s += @"<li" + active_class + @"><a href=""" + link + @""">" + text + "</a></li>";
            return s;
        }

        public bool with_access(ADUser ad_user)
        {
            if (ad_user == null) return true;
            if (this.ad_users.Count == 0 && this.ad_departments.Count == 0 && this.ad_groups.Count == 0) return true;
            if (ad_user.inAnyUser(this.ad_users)) return true;
            if (ad_user.inAnyDepartments(this.ad_departments)) return true;
            if (ad_user.inAnyGroups(this.ad_groups)) return true;
            return false;
        }

        public CommonNav()
        {
        }

        private static CommonNav secure_nav()
        {
            CommonNav nav = new CommonNav();
            //nav.ad_groups.Add("gl1806");
            //nav.ad_departments.Add("mis");
            return nav;
        }

        public static string main_nav_select(string active, ADUser ad_user)
        {
            string s = CommonNav.nav_start;
            List<CommonNav> navs = new List<CommonNav>();
            CommonNav nav = secure_nav(); nav.link = "http://localrmmc/devintranet/bankruptcies/"; nav.text = "Bankruptcies"; navs.Add(nav);
            nav = secure_nav(); nav.link = "http://localrmmc/devintranet/collections/"; nav.text = "Collections"; navs.Add(nav);
            nav = secure_nav(); nav.link = "http://localrmmc/devintranet/foreclosures/"; nav.text = "Foreclosures"; navs.Add(nav);
            nav = secure_nav(); nav.link = "http://localrmmc/devintranet/lossmitigation/"; nav.text = "Loss Mitigations"; navs.Add(nav);
            nav = secure_nav(); nav.link = "http://localrmmc/devintranet/inspections/"; nav.text = "Inspections"; navs.Add(nav);
            nav = secure_nav(); nav.link = "http://localrmmc/devintranet/cashier/"; nav.text = "Cashier"; navs.Add(nav);
            nav = secure_nav(); nav.link = "http://localrmmc/devintranet/servicing_management/"; nav.text = "Management"; navs.Add(nav);
            nav = secure_nav(); nav.link = "http://localrmmc/devintranet/cashier/portfolio"; nav.text = "Board Reports"; navs.Add(nav);

            foreach (CommonNav n in navs)
            {
                s += n.link_html(active, ad_user);
            }
            return s + CommonNav.nav_end;
        }
    }
}