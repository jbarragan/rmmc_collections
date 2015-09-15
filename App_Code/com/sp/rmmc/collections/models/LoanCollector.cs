using System;
using System.Collections.Generic;
using System.Web;

using System.Data.SqlClient;
using System.Data.Common;

using com.sp.rmmc.common.models;
using com.sp.rmmc.common.models.events;

namespace com.sp.rmmc.collections.models
{
    public class LoanCollector : MsSqlModel
    {
        public int id = 0;
        public decimal loan_id = 0M;
        public DateTimeObject assigned_on = new DateTimeObject();
        public string collector = "";

        private string[] current_collectors = {"smena", "lamaya", "ogonzalez"};
        private int latest_selected_collector = 0;

        private string connection_string = System.Configuration.ConfigurationManager.ConnectionStrings["collection"].ConnectionString;
        private string select_base_query = "select id, loan_id, assigned_on, collector from loan_collectors ";

        public LoanCollector()
        {
        }

        public string insert()
        {
            this.setConnectionString(this.connection_string);
            SqlCommand cmd = getCommand();
            cmd.CommandText = "INSERT INTO loan_collectors (loan_id, assigned_on, collector) VALUES(@loan_id, getdate(), @collector)";
            cmd.Parameters.AddWithValue("@loan_id", this.loan_id);
            cmd.Parameters.AddWithValue("@collector", this.collector);
            this.id = executeCommandStatement(cmd);
            return error;
        }

        public LoanCollector get(int loan_id)
        {
            LoanCollector lc = null;
            string query = select_base_query + " where loan_id = " + loan_id + " order by assigned_on desc ";
            this.setConnectionString(this.connection_string);
            DbDataReader reader = getReader(query);
            if (reader.Read()) lc = read(reader, 0);
            close_connection();
            return lc;
        }

        private List<LoanCollector> get_list(String query)
        {
            List<LoanCollector> lcs = new List<LoanCollector>();
            this.setConnectionString(this.connection_string);
            DbDataReader reader = getReader(query);
            while (reader.Read()) lcs.Add((new LoanCollector()).read(reader, 0));
            reader.Close();
            close_connection();
            return lcs;
        }

        private LoanCollector read(DbDataReader reader, int pos)
        {
            LoanCollector lc = new LoanCollector();
            lc.id = readDBInt32(reader, pos++);
            lc.loan_id = readDBDecimal(reader, pos++);
            lc.assigned_on = readDBDateObject(reader, pos++);
            lc.collector = readDBString(reader, pos++);
            return lc;
        }

        public void get_collections_collector(List<BaseCollection> collections)
        {
            DateTime collection_date = DateTime.Today;
            int today_year_month = DateTime.Today.Year * 12 + DateTime.Today.Month;

            List<BaseCollection> collections_without_collector = new List<BaseCollection>();

            string query =
                "select * from loan_collectors " +
                "where id in ( " +
                "select max(id) from loan_collectors " +
                "where loan_id in ";
            List<string> collection_loans = new List<string>();
            foreach (BaseCollection collection in collections) collection_loans.Add(collection.loan_id.ToString());
            if (collection_loans.Count > 0) query += " (" + String.Join(", ", collection_loans.ToArray()) + " ) ";
            else query += " (-10) ";
            query += 
                "and YEAR(assigned_on)*12 + MONTH(assigned_on) = " + today_year_month.ToString() + " " +
                "group by loan_id " +
                ")";

            List<LoanCollector> lcs = get_list(query);
            Dictionary<decimal, LoanCollector> loan_lcs = new Dictionary<decimal, LoanCollector>();
            foreach (LoanCollector lc in lcs)
            {
                if (loan_lcs.ContainsKey(lc.loan_id)) continue;
                loan_lcs.Add(lc.loan_id, lc);
            }

            foreach (BaseCollection collection in collections)
            {
                if (loan_lcs.ContainsKey(collection.loan_id))
                {
                    LoanCollector lc = loan_lcs[collection.loan_id];
                    int assigned_on_year_month = lc.assigned_on.date.Year * 12 + lc.assigned_on.date.Month;
                    if (assigned_on_year_month >= today_year_month) collection.collector = loan_lcs[collection.loan_id];
                    else collections_without_collector.Add(collection);
                }
                else
                {
                    collections_without_collector.Add(collection);
                }
            }
            populate_collections_without_collector(collections_without_collector);

        }

        public void populate_collections_without_collector(List<BaseCollection> collections)
        {
            latest_selected_collector = DateTime.Today.DayOfYear % current_collectors.Length;
            List<LoanCollector> lcs = new List<LoanCollector>();
            foreach (BaseCollection collection in collections)
            {
                LoanCollector lc = new LoanCollector();
                lc.loan_id = collection.loan_id;
                lc.assigned_on.set_date(DateTime.Today);
                lc.collector = this.current_collectors[(latest_selected_collector++) % this.current_collectors.Length];
                collection.collector = lc;
                lcs.Add(lc);
            }
            insert_multiple(lcs);
        }

        public void insert_multiple(List<LoanCollector> lcs)
        {
            MsSqlModel model = new MsSqlModel();
            model.setConnectionString(connection_string);
            string insert_query = 
                "INSERT INTO loan_collectors " + 
                "(loan_id, assigned_on, collector) " +
                "VALUES(@loan_id, @assigned_on, @collector)";
            foreach (LoanCollector lc in lcs)
            {
                SqlCommand cmd = model.getCommand();
                cmd.CommandText = insert_query;
                cmd.Parameters.AddWithValue("@loan_id", lc.loan_id);
                cmd.Parameters.AddWithValue("@assigned_on", lc.assigned_on.toDBInsert());
                cmd.Parameters.AddWithValue("@collector", lc.collector);
                model.executeCommandStatement(cmd);
            }
            model.close_connection();
            //if (model.error != "")
        }
        
    }
}