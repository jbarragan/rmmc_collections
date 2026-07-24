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

        // Collectors are no longer hardcoded. This list is loaded at runtime with the
        // collectors (ms_loan_memo user_ids) that made an outbound call/email in the last
        // 30 days, and is used as the round-robin pool for loans without a last-call collector.
        private List<string> current_collectors = new List<string>();
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
            int today_year_month = DateTime.Today.Year * 12 + DateTime.Today.Month;

            // Priority 1: the collector is the user_id of the most recent outbound call/email
            // memo on the loan (from ms_loan_memo on the servicing DB).
            Dictionary<decimal, string> last_call_collectors = get_last_call_collectors(collections);

            // Pool used for the round-robin fallback: collectors who called in the last 30 days.
            this.current_collectors = get_last_30_days_collectors();

            List<BaseCollection> collections_without_collector = new List<BaseCollection>();

            // Priority 2 (only for loans without a last-call collector): reuse the sticky
            // assignment already persisted this month in loan_collectors.
            string query =
                "select * from loan_collectors " +
                "where id in ( " +
                "select max(id) from loan_collectors " +
                "where loan_id in ";
            List<string> collection_loans = new List<string>();
            foreach (BaseCollection collection in collections)
                if (!last_call_collectors.ContainsKey(collection.loan_id))
                    collection_loans.Add(collection.loan_id.ToString());
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
                if (last_call_collectors.ContainsKey(collection.loan_id))
                {
                    LoanCollector lc = new LoanCollector();
                    lc.loan_id = collection.loan_id;
                    lc.collector = last_call_collectors[collection.loan_id];
                    collection.collector = lc;
                }
                else if (loan_lcs.ContainsKey(collection.loan_id))
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

        // Returns loan_id -> user_id of the most recent (ever) outbound call/email memo for
        // each loan in the list. Same "last attempted call" definition used by UnfilteredCollection.
        public Dictionary<decimal, string> get_last_call_collectors(List<BaseCollection> collections)
        {
            Dictionary<decimal, string> last_call_collectors = new Dictionary<decimal, string>();

            List<string> loan_ids = new List<string>();
            foreach (BaseCollection collection in collections) loan_ids.Add(collection.loan_id.ToString());
            if (loan_ids.Count == 0) return last_call_collectors;

            string query =
                "select m.loan_id, m.user_id " +
                "from ms_loan_memo m " +
                "where (m.memo_subject like '%OUTBOUND%CALL%' or m.memo_subject like '%O%EMAIL%') " +
                "and m.loan_id in (" + String.Join(", ", loan_ids.ToArray()) + ") " +
                "and m.actual_create_dt = ( " +
                "    select max(m2.actual_create_dt) from ms_loan_memo m2 " +
                "    where m2.loan_id = m.loan_id " +
                "    and (m2.memo_subject like '%OUTBOUND%CALL%' or m2.memo_subject like '%O%EMAIL%') ) ";

            SybaseModel model = new SybaseModel();
            DbDataReader reader = model.getReader(query);
            if (reader != null)
            {
                while (reader.Read())
                {
                    decimal loan_id = readDBDecimal(reader, 0);
                    string user_id = readDBString(reader, 1).Trim();
                    if (loan_id > 0 && user_id != "" && !last_call_collectors.ContainsKey(loan_id))
                        last_call_collectors.Add(loan_id, user_id);
                }
                reader.Close();
            }
            model.close_connection();
            return last_call_collectors;
        }

        // Returns the distinct collectors (ms_loan_memo user_ids) that made an outbound
        // call/email in the last 30 days. Used for the round-robin fallback and the dropdown.
        public List<string> get_last_30_days_collectors()
        {
            List<string> collectors = new List<string>();

            string query =
                "select distinct m.user_id " +
                "from ms_loan_memo m " +
                "where (m.memo_subject like '%OUTBOUND%CALL%' or m.memo_subject like '%O%EMAIL%') " +
                "and m.actual_create_dt >= dateadd(dd, -30, getdate()) " +
                "and m.user_id is not null and m.user_id <> '' ";

            SybaseModel model = new SybaseModel();
            DbDataReader reader = model.getReader(query);
            if (reader != null)
            {
                while (reader.Read())
                {
                    string user_id = readDBString(reader, 0).Trim();
                    if (user_id != "" && !collectors.Contains(user_id)) collectors.Add(user_id);
                }
                reader.Close();
            }
            model.close_connection();
            return collectors;
        }

        public void populate_collections_without_collector(List<BaseCollection> collections)
        {
            if (this.current_collectors.Count == 0) return; // no callers in the last 30 days to assign from
            latest_selected_collector = DateTime.Today.DayOfYear % current_collectors.Count;
            List<LoanCollector> lcs = new List<LoanCollector>();
            foreach (BaseCollection collection in collections)
            {
                LoanCollector lc = new LoanCollector();
                lc.loan_id = collection.loan_id;
                lc.assigned_on.set_date(DateTime.Today);
                lc.collector = this.current_collectors[(latest_selected_collector++) % this.current_collectors.Count];
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