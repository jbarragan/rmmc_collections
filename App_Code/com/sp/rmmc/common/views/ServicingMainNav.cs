using System;
using System.Collections.Generic;
using System.Web;

namespace com.sp.rmmc.common.views
{
    public class ServicingMainNav
    {
        public ServicingMainNav()
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
            nav = secure_nav(); nav.link = "http://localrmmc/devintranet/servicing_management/"; nav.text = "Management"; navs.Add(nav);
        
            foreach (CommonNav n in navs)
            {
                s += n.link_html(active, ad_user);
            }
            return s + CommonNav.nav_end;
        }
    }
}