using System;
using System.Collections.Generic;
using System.Web;

using System.Data.SqlClient;
using System.Data.Common;

using com.sp.rmmc.common.models;
using com.sp.rmmc.common.models.events;

namespace com.sp.rmmc.collections.models
{

    public class History : MsSqlModel
    {
        public int id = 0;
        public string label = "";
        public DateTimeObject history_date = new DateTimeObject();
        public string history_text{
            get{ return label + " on " + this.history_date.ToShortDateString(); }
        }

        private string connection_string = System.Configuration.ConfigurationManager.ConnectionStrings["collection"].ConnectionString;
        private string select_base_query = "select id, label, history_date from collection_history ";

        public History()
        {
            
        }

        public string insert()
        {
            this.setConnectionString(this.connection_string);
            SqlCommand cmd = getCommand();
            cmd.CommandText = "INSERT INTO collection_history (label, history_date) VALUES(@label, @history_date)";
            cmd.Parameters.AddWithValue("@label", this.label);
            cmd.Parameters.AddWithValue("@history_date", this.history_date.date);
            this.id = executeCommandStatement(cmd);
            return error;
        }

        public History get(int history_id)
        {
            History h = null;
            string query = select_base_query + " where id = " + history_id;
            this.setConnectionString(this.connection_string);
            DbDataReader reader = getReader(query);
            if (reader.Read()) h = read(reader, 0);
            close_connection();
            return h;
        }

        public List<History> all()
        {
            string query = select_base_query;
            return this.get_list(query);
        }

        public List<History> last_of_month_all()
        {
            string query = select_base_query + "  order by id desc";
            List < History > all = this.get_list(query);
            List<History> first_of_month_list = new List<History>();
            History fmh = null;
            if( all.Count > 0 ) first_of_month_list.Add(fmh = all[0]);
            foreach( History h in all){
                if( h.history_date.date.Month == fmh.history_date.date.Month &&
                    h.history_date.date.Year == fmh.history_date.date.Year ) continue;
                first_of_month_list.Add(fmh = h);
            }
            return first_of_month_list;
        }

        private List<History> get_list(String query)
        {
            List<History> histories = new List<History>();
            this.setConnectionString(this.connection_string);
            DbDataReader reader = getReader(query);
            while (reader.Read()) histories.Add((new History()).read(reader, 0));
            reader.Close();
            close_connection();
            return histories;
        }

        private History read(DbDataReader reader, int pos)
        {
            History h = new History();
            h.id = readDBInt32(reader, pos++);
            h.label = readDBString(reader, pos++);
            h.history_date = readDBDateObject(reader, pos++);
            return h;
        }
    }
}