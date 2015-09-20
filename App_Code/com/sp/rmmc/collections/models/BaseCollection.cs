using System;
using System.Collections.Generic;
using System.Web;
using System.Data.SqlClient;

using System.Data.Common;

using com.sp.rmmc.common.models;
using com.sp.rmmc.common.models.events;
using com.sp.rmmc.common.tools;

namespace com.sp.rmmc.collections.models
{
    public class BaseCollection : DbModel, ICSVExport
    {
        public decimal loan_id = 0M;
        public DateTimeObject demand_letter_due_date = new DateTimeObject();
        public String foreclosure_officers = "";
        public String fcl_type = "";
        public DateTimeObject last_reg_paid_dt = new DateTimeObject();
        public decimal last_payments_balance = 0M;
        public DateTimeObject papers_to_attorney_date = new DateTimeObject();
        public DateTimeObject posting_date = new DateTimeObject();
        public string chapter_filed = "";
        public DateTimeObject bankruptcy_filed_date = new DateTimeObject();
        public DateTimeObject bankruptcy_discharge_date = new DateTimeObject();
        public DateTimeObject bankruptcy_case_dismissed_date = new DateTimeObject();
        public DateTimeObject bankruptcy_closed_date = new DateTimeObject();
        public DateTimeObject fc_foreclosure_referral_date = new DateTimeObject();
        public DateTimeObject fc_payoff_date = new DateTimeObject();
        public DateTimeObject appraisal_ordered_date = new DateTimeObject();
        public DateTimeObject foreclosure_date = new DateTimeObject();
        public int lm_stop_code_id = 0;
        public DateTimeObject memo_fc_third_party_sale_date = new DateTimeObject();
        public DateTimeObject extension_ordered_date = new DateTimeObject();
        public string fcl_deed_in_lieu = "";
        public DateTimeObject lm_date = new DateTimeObject();
        public DateTimeObject possession_date = new DateTimeObject();
        public DateTimeObject deed_from_borrower_received_date = new DateTimeObject();
        public string mortgage_status = "";

        public DateTimeObject sale_date = new DateTimeObject();
        public decimal ytd_prin_paid_amt = 0M;
        public decimal principal_amt_from_history = 0M;
        public decimal audit_prin_bal = 0M;

        public com.sp.rmmc.common.models.MsLoan loan = new com.sp.rmmc.common.models.MsLoan();
        public LoanCollector collector = new LoanCollector();

        public CollectionReasonCodeEvents reason_code = null;
        public CollectionPromiseToPayEvents promise_to_pay_events = null;
        
        public string filter_reason = "";
        public string type = "";
        public const string COLLECTION_TYPE = "Collection";
        public const string ALL_TYPE = "All";

        protected virtual string default_columns { get { return ""; } }
        protected virtual string collections_query { get { return ""; } }
        protected virtual string collections_4_month_delinquent_query { get { return ""; } }
        protected virtual string collections_17_days_fha_va_delinquent_query { get { return ""; } }
        protected virtual string collections_17_days_cnv_non_c_mcm_delinquent_query { get { return ""; } }
        protected virtual string collections_17_days_cnv_c_mcm_delinquent_query { get { return ""; } }
        protected virtual string collections_2_month_fha_delinquent_query { get { return ""; } }
        protected virtual string collections_3_month_fha_hud_delinquent_query { get { return ""; } }
        protected virtual string all_types_query { get { return ""; } }

        public void getCollections(List<BaseCollection> collections, List<BaseCollection> removed_collections)
        {
            this.type = COLLECTION_TYPE;
            string columns = this.default_columns + getEventsColumns();
            string query = "select " + columns + " from " + "( " + collections_query + ")base where 1=1 " + collections_conditions();
            query = query + " order by ms_loan_due_date_next_payment, ms_loan_type";
            List<BaseCollection> all = this.get_loan_list(query);
            foreach (BaseCollection f in all) { f.type = COLLECTION_TYPE; collections.Add(f); }
            commonFilters(collections, removed_collections);
        }

        public void getCollectionsAndDemands(List<BaseCollection> collections, List<BaseCollection> removed_collections)
        {
            this.type = COLLECTION_TYPE;
            string columns = this.default_columns + getEventsColumns();
            string query = "select " + columns + " from " + "( " + collections_query + ")base where 1=1 " + collections_conditions();
            query = query + " order by ms_loan_due_date_next_payment, ms_loan_type";
            List<BaseCollection> all = this.get_loan_list(query);
            foreach (BaseCollection f in all) { f.type = COLLECTION_TYPE; collections.Add(f); }
            commonDemandCollectionFilters(collections, removed_collections);
        }

        public void getCollections4MonthDelinquent(List<BaseCollection> collections, List<BaseCollection> removed_collections)
        {
            this.type = COLLECTION_TYPE;
            string columns = this.default_columns + getEventsColumns();
            string query = "select " + columns + " from " + "( " + collections_4_month_delinquent_query + ")base where 1=1 " + collections_conditions();
            query = query + " order by ms_loan_due_date_next_payment, ms_loan_type";
            List<BaseCollection> all = this.get_loan_list(query);
            foreach (BaseCollection f in all) { f.type = COLLECTION_TYPE; collections.Add(f); }
            commonFilters(collections, removed_collections);
        }

        public void getCollections2MonthFHADelinquent(List<BaseCollection> collections, List<BaseCollection> removed_collections)
        {
            this.type = COLLECTION_TYPE;
            string columns = this.default_columns + getEventsColumns();
            string query = "select " + columns + " from " + "( " + collections_2_month_fha_delinquent_query + ")base where 1=1 " + collections_conditions();
            query = query + " order by ms_loan_id ";
            List<BaseCollection> all = this.get_loan_list(query);
            foreach (BaseCollection f in all) { f.type = COLLECTION_TYPE; collections.Add(f); }
            commonFilters(collections, removed_collections);
            remove_repayment_plan(collections, removed_collections);
            (new LoanCollector()).get_collections_collector(collections);
        }

        public void getCollections3MonthFHAHudDelinquent(List<BaseCollection> collections, List<BaseCollection> removed_collections)
        {
            this.type = COLLECTION_TYPE;
            string columns = this.default_columns + getEventsColumns();
            string query = "select " + columns + " from " + "( " + collections_3_month_fha_hud_delinquent_query + ")base where 1=1 " + collections_conditions();
            query = query + " order by ms_loan_id ";
            List<BaseCollection> all = this.get_loan_list(query);
            foreach (BaseCollection f in all) { f.type = COLLECTION_TYPE; collections.Add(f); }
            commonFilters(collections, removed_collections);
            remove_repayment_plan(collections, removed_collections);
            (new LoanCollector()).get_collections_collector(collections);
        }

        public void getCollections17DayDelinquentFHAVA(List<BaseCollection> collections, List<BaseCollection> removed_collections)
        {
            this.type = COLLECTION_TYPE;
            string columns = this.default_columns + getEventsColumns();
            string query = "select " + columns + " from " + "( " + collections_17_days_fha_va_delinquent_query + ")base where 1=1 " + collections_conditions();
            query = query + " order by ms_loan_id ";
            List<BaseCollection> all = this.get_loan_list(query);
            foreach (BaseCollection f in all) { f.type = COLLECTION_TYPE; collections.Add(f); }
            commonFilters(collections, removed_collections);
            remove_loss_mitigation(collections, removed_collections);
            (new LoanCollector()).get_collections_collector(collections);
        }

        public void getCollections17DayDelinquentCNVNonCMCM(List<BaseCollection> collections, List<BaseCollection> removed_collections)
        {
            this.type = COLLECTION_TYPE;
            string columns = this.default_columns + getEventsColumns();
            string query = "select " + columns + " from " + "( " + collections_17_days_cnv_non_c_mcm_delinquent_query + ")base where 1=1 " + collections_conditions();
            query = query + " order by ms_loan_id ";
            List<BaseCollection> all = this.get_loan_list(query);
            foreach (BaseCollection f in all) { f.type = COLLECTION_TYPE; collections.Add(f); }
            commonFilters(collections, removed_collections);
            remove_loss_mitigation(collections, removed_collections);
            (new LoanCollector()).get_collections_collector(collections);
        }

        public void getCollections17DayDelinquentCNVCMCM(List<BaseCollection> collections, List<BaseCollection> removed_collections)
        {
            this.type = COLLECTION_TYPE;
            string columns = this.default_columns + getEventsColumns();
            string query = "select " + columns + " from " + "( " + collections_17_days_cnv_c_mcm_delinquent_query + ")base where 1=1 " + collections_conditions();
            query = query + " order by ms_loan_id ";
            List<BaseCollection> all = this.get_loan_list(query);
            foreach (BaseCollection f in all) {
                f.type = COLLECTION_TYPE; collections.Add(f);
            }
            commonFilters(collections, removed_collections);
            remove_loss_mitigation(collections, removed_collections);
            (new LoanCollector()).get_collections_collector(collections);
        }

        public void getForeclosureLoan(decimal loan_id, List<BaseCollection> accepted, List<BaseCollection> removed, String type)
        {
            this.type = type;
            string columns = this.default_columns + getEventsColumns();
            string query = "select " + columns + " from " + "( " + all_types_query + ")base where 1=1 " + single_loan_conditions(loan_id);
            query = query + " order by ms_loan_type, demand_letter_due_date asc";
            List<BaseCollection> all = get_loan_list(query);
            foreach (BaseCollection f in all) { f.type = type; accepted.Add(f); }
            commonDemandCollectionFilters(accepted, removed);
        }

        public List<string> columns_list()
        {
            List<string> columns = new List<string>();
            columns.AddRange(default_columns_list());
            this.type = COLLECTION_TYPE;
            foreach (BaseEvents be in getEventsTypes()) columns.AddRange(be.column_names_list());
            
            return columns;
        }

        public List<string> default_columns_list()
        {
            List<string> columns = new List<string>();
            string[] common_columns = this.default_columns.Split(new string[] { " as " }, StringSplitOptions.None);
            for (int i = 1; i < common_columns.Length; i++)
            {
                columns.Add(common_columns[i].Split(',')[0]);
            }
            return columns;
        }

        public void addInsertParameters(SqlCommand cmd)
        {
            cmd.Parameters.AddWithValue("@loan_id", this.loan_id);
            cmd.Parameters.AddWithValue("@demand_letter_due_date", this.demand_letter_due_date.toDBInsert());
            cmd.Parameters.AddWithValue("@foreclosure_officers", this.foreclosure_officers);
            cmd.Parameters.AddWithValue("@fcl_type", this.fcl_type);
            cmd.Parameters.AddWithValue("@last_reg_paid_dt", this.last_reg_paid_dt.toDBInsert());
            cmd.Parameters.AddWithValue("@last_payments_balance", this.last_payments_balance);
            cmd.Parameters.AddWithValue("@papers_to_attorney_date", this.papers_to_attorney_date.toDBInsert());
            cmd.Parameters.AddWithValue("@posting_date", this.posting_date.toDBInsert());
            cmd.Parameters.AddWithValue("@chapter_filed", this.chapter_filed);
            cmd.Parameters.AddWithValue("@bankruptcy_filed_date", this.bankruptcy_filed_date.toDBInsert());
            cmd.Parameters.AddWithValue("@bankruptcy_discharge_date", this.bankruptcy_discharge_date.toDBInsert());
            cmd.Parameters.AddWithValue("@bankruptcy_case_dismissed_date", this.bankruptcy_case_dismissed_date.toDBInsert());
            cmd.Parameters.AddWithValue("@bankruptcy_closed_date", this.bankruptcy_closed_date.toDBInsert());
            cmd.Parameters.AddWithValue("@sale_date", this.sale_date.toDBInsert());
            cmd.Parameters.AddWithValue("@ytd_prin_paid_amt", this.ytd_prin_paid_amt);
            cmd.Parameters.AddWithValue("@principal_amt_from_history", this.principal_amt_from_history);
            cmd.Parameters.AddWithValue("@audit_prin_bal", this.audit_prin_bal);
            cmd.Parameters.AddWithValue("@fc_foreclosure_referral_date", this.fc_foreclosure_referral_date.toDBInsert());
            cmd.Parameters.AddWithValue("@fc_payoff_date", this.fc_payoff_date.toDBInsert());
            cmd.Parameters.AddWithValue("@appraisal_ordered_date", this.appraisal_ordered_date.toDBInsert());
            cmd.Parameters.AddWithValue("@foreclosure_date", this.foreclosure_date.toDBInsert());
            cmd.Parameters.AddWithValue("@lm_stop_code_id", this.lm_stop_code_id);
            cmd.Parameters.AddWithValue("@memo_fc_third_party_sale_date", this.memo_fc_third_party_sale_date.toDBInsert());
            cmd.Parameters.AddWithValue("@extension_ordered_date", this.extension_ordered_date.toDBInsert());
            cmd.Parameters.AddWithValue("@fcl_deed_in_lieu", this.fcl_deed_in_lieu);
            cmd.Parameters.AddWithValue("@lm_date", this.lm_date.toDBInsert());
            cmd.Parameters.AddWithValue("@possession_date", this.possession_date.toDBInsert());
            cmd.Parameters.AddWithValue("@deed_from_borrower_received_date", this.deed_from_borrower_received_date.toDBInsert());
            cmd.Parameters.AddWithValue("@mortgage_status", this.mortgage_status);
            
            this.loan.addInsertParameters(cmd);

            this.type = COLLECTION_TYPE;
            foreach (BaseEvents be in getEventsTypes()) be.addInsertParameters(cmd);
        }

        private void commonFilters(List<BaseCollection> accepted, List<BaseCollection> removed)
        {
            List<BaseCollection> all_accepted = new List<BaseCollection>();
            foreach (BaseCollection f in accepted) all_accepted.Add(f);
            //remove_received_payment(all_accepted, removed);
            remove_bankruptcy(all_accepted, removed);
            get_lm_date(all_accepted);
            remove_lm(all_accepted, removed);
            remove_demands(all_accepted, removed);
            foreach (BaseCollection f in all_accepted)
            {   
                if (f.filter_reason != "") accepted.Remove(f);
            }
        }

        private void commonDemandCollectionFilters(List<BaseCollection> accepted, List<BaseCollection> removed)
        {
            List<BaseCollection> all_accepted = new List<BaseCollection>();
            foreach (BaseCollection f in accepted) all_accepted.Add(f);
            //remove_received_payment(all_accepted, removed);
            remove_bankruptcy(all_accepted, removed);
            get_lm_date(all_accepted);
            remove_lm(all_accepted, removed);
            foreach (BaseCollection f in all_accepted)
            {
                if (f.filter_reason != "") accepted.Remove(f);
            }
        }

        private bool isThirdPartySale()
        {
            BaseCollection f = this;
            return (f.memo_fc_third_party_sale_date.isNull == false);
        }

        private static void remove_bankruptcy(List<BaseCollection> accepted, List<BaseCollection> removed)
        {
            foreach (BaseCollection f in accepted)
            {
                if (removed.Contains(f)) continue;
                if (f.bankruptcy_filed_date.isNull == false &&
                    (f.bankruptcy_discharge_date.isNull == true &&
                        f.bankruptcy_case_dismissed_date.isNull == true &&
                        f.bankruptcy_closed_date.isNull == true
                    )
                   )
                {
                    f.filter_reason = "Bankruptcy";
                    removed.Add(f);
                }
            }
        }

        private static void remove_repayment_plan(List<BaseCollection> accepted, List<BaseCollection> removed)
        {
            List<BaseCollection> all_accepted = new List<BaseCollection>();
            all_accepted.AddRange(accepted);
            foreach (BaseCollection f in all_accepted)
            {
                if (removed.Contains(f)) continue;
                if ( f.mortgage_status == "12")
                {
                    f.filter_reason = "Repayment Plan";
                    removed.Add(f);
                    accepted.Remove(f);
                }
            }
        }

        private static void remove_loss_mitigation(List<BaseCollection> accepted, List<BaseCollection> removed)
        {
            List<BaseCollection> all_accepted = new List<BaseCollection>();
            all_accepted.AddRange(accepted);
            foreach (BaseCollection f in all_accepted)
            {
                if (removed.Contains(f)) continue;
                if (f.mortgage_status == "09")
                {
                    f.filter_reason = "Loss Mitigation";
                    removed.Add(f);
                    accepted.Remove(f);
                }
            }
        }

        private bool isBankruptcy()
        {
            BaseCollection f = this;
            return (f.bankruptcy_filed_date.isNull == false &&
                    (f.bankruptcy_discharge_date.isNull == true &&
                        f.bankruptcy_case_dismissed_date.isNull == true &&
                        f.bankruptcy_closed_date.isNull == true
                    )
                   );
        }

        private static void remove_received_payment(List<BaseCollection> accepted, List<BaseCollection> removed)
        {   
            foreach (BaseCollection f in accepted)
            {
                //if (f.loan_id == 152526M)
                //    f.filter_reason = "";
                if (removed.Contains(f)) continue;
                if ((f.last_reg_paid_dt.isNull == false &&
                    f.last_reg_paid_dt.date >= f.demand_letter_due_date.date.AddDays(-33) &&
                    f.last_payments_balance >= 0)  ||
                    f.loan.due_date_next_payment.date >= f.demand_letter_due_date.date )
                {   
                    f.filter_reason = "Received Payment";
                    removed.Add(f);
                }
            }
        }

        private bool hasReceivedPayment()
        {
            BaseCollection f = this;
            return ((f.last_reg_paid_dt.isNull == false &&
                    f.last_reg_paid_dt.date >= f.demand_letter_due_date.date.AddDays(-33) &&
                    f.last_payments_balance >= 0) ||
                    f.loan.due_date_next_payment.date >= f.demand_letter_due_date.date);
        }

        public static string collections_conditions()
        {
            string query = //" and ( sale_date is null ) \n" +
                           //" and ( ms_loan_id = 158939 ) \n" +
                           " and ( ms_loan_prin_bal > 0 )";
            return query;
        }

        public static string single_loan_conditions(decimal loan_id)
        {
            string query = " and ( ms_loan_id = " + loan_id + " )";
            return query;
        }

        public void remove_lm(List<BaseCollection> accepted, List<BaseCollection> removed)
        {
            // Stop removing LM from Collections
            return;
            /*
            foreach (BaseCollection f in accepted)
            {
                if (removed.Contains(f)) continue;
                if ( f.lm_date.isNull == false )
                {
                    f.filter_reason = "Loss Mitigation";
                    removed.Add(f);
                }
            }
            */ 
        }

        public void remove_demands(List<BaseCollection> accepted, List<BaseCollection> removed)
        {
            foreach (BaseCollection f in accepted)
            {
                if (f.loan_id == 142760M)
                    f.loan_id = 142760M;

                if (removed.Contains(f)) continue;
                if (f.demand_letter_due_date.isNull == false && 
                    f.last_reg_paid_dt.date < f.demand_letter_due_date.date.AddDays(-33))
                {
                    f.filter_reason = "Demand";
                    removed.Add(f);
                }
            }
        }

        public void remove_foreclosures(List<BaseCollection> accepted, List<BaseCollection> removed)
        {
            foreach (BaseCollection f in accepted)
            {
                if (removed.Contains(f)) continue;
                if (f.fcl_type == "A")
                {
                    f.filter_reason = "Foreclosure";
                    removed.Add(f);
                }
            }
        }

        protected virtual void get_lm_date(List<BaseCollection> accepted)
        {   
        }

        protected virtual List<BaseCollection> get_loan_list(String query)
        {
            List<BaseCollection> list = new List<BaseCollection>();
            return list;
        }

        public BaseCollection read_loan(DbDataReader reader)
        {
            BaseCollection f = this;
            int pos = 0;
            f.loan_id = readDBDecimal(reader, pos++);
            f.demand_letter_due_date = readDBDateObject(reader, pos++);
            f.foreclosure_officers = readDBString(reader, pos++);
            f.fcl_type = readDBString(reader, pos++);
            f.last_reg_paid_dt = readDBDateObject(reader, pos++);
            f.last_payments_balance = readDBDecimal(reader, pos++);
            f.papers_to_attorney_date = readDBDateObject(reader, pos++);
            f.posting_date = readDBDateObject(reader, pos++);
            f.chapter_filed = readDBString(reader, pos++);
            f.bankruptcy_filed_date = readDBDateObject(reader, pos++);
            f.bankruptcy_discharge_date = readDBDateObject(reader, pos++);
            f.bankruptcy_case_dismissed_date = readDBDateObject(reader, pos++);
            f.bankruptcy_closed_date = readDBDateObject(reader, pos++);
            f.sale_date = readDBDateObject(reader, pos++);
            f.ytd_prin_paid_amt = readDBDecimal(reader, pos++);
            f.principal_amt_from_history = readDBDecimal(reader, pos++);
            f.audit_prin_bal = readDBDecimal(reader, pos++);
            f.fc_foreclosure_referral_date = readDBDateObject(reader, pos++);
            f.fc_payoff_date = readDBDateObject(reader, pos++);
            f.appraisal_ordered_date = readDBDateObject(reader, pos++);
            f.foreclosure_date = readDBDateObject(reader, pos++);
            f.lm_stop_code_id = readDBInt(reader, pos++);
            f.memo_fc_third_party_sale_date = readDBDateObject(reader, pos++);
            f.extension_ordered_date = readDBDateObject(reader, pos++);
            f.fcl_deed_in_lieu = readDBString(reader, pos++);
            f.lm_date = readDBDateObject(reader, pos++);
            f.possession_date = readDBDateObject(reader, pos++);
            f.deed_from_borrower_received_date = readDBDateObject(reader, pos++);
            f.mortgage_status = readDBString(reader, pos++);

            if (f.loan_id > 0)
            {
                DateTime event_date = f.loan.event_todays_date;
                f.loan = com.sp.rmmc.common.models.MsLoan.read(reader, pos);
                f.loan.event_todays_date = event_date;
                pos += com.sp.rmmc.common.models.MsLoan.COLUMNS;
                foreach (BaseEvents be in f.getEventsTypes())
                {
                    be.read(reader, pos, f.loan);
                    pos += be.fields_count();
                }
            }
            return f;
        }

        public List<Event> getEvents()
        {
            List<Event> es = new List<Event>();
            es.AddRange(internalEvents());

            if (this.type == COLLECTION_TYPE)
            {
                es.AddRange(reason_code.events);
                es.AddRange(promise_to_pay_events.events);
            }
            return es;
        }

        protected virtual string getEventsColumns()
        {
            string s = "";
            foreach (BaseEvents be in this.getEventsTypes())
            {
                s += ", \n" + be.query("base.loan_id");
            }
            return s;
        }

        protected List<BaseEvents> getEventsTypes()
        {
            List<BaseEvents> event_types = new List<BaseEvents>();
            if (this.type == COLLECTION_TYPE)
            {
                if (reason_code == null) reason_code = new CollectionReasonCodeEvents();
                event_types.Add(reason_code);
                if (promise_to_pay_events == null) promise_to_pay_events = new CollectionPromiseToPayEvents();
                event_types.Add(promise_to_pay_events);
                
            }
            return event_types;
        }

        private List<Event> internalEvents()
        {
            List<Event> es = new List<Event>();
            return es;
        }

        public decimal non_zero_balance()
        {
            if (this.loan.prin_bal > 0M) return this.loan.prin_bal;
            if (this.principal_amt_from_history > 0M) return this.principal_amt_from_history;
            if (this.ytd_prin_paid_amt > 0M) return this.ytd_prin_paid_amt;
            if (this.audit_prin_bal > 0M) return this.audit_prin_bal;
            else return this.principal_amt_from_history;
        }

        public string getEventStatusColor()
        {
            string s = "";
            List<Event> events = getEvents();
            foreach(Event e in events){
                if( e.type == Event.WARNING ) s = e.getEventStatusColor();
                if (e.type == Event.ALERT) { s = e.getEventStatusColor(); break; }
            }
            return s;
        }

        public bool isMortgageStatusValid(){
            return (this.mortgage_status == "42" ||
                this.mortgage_status == "11" ||
                this.mortgage_status == "12" ||
                this.mortgage_status == "" ||
                this.mortgage_status == "67" ||
                this.mortgage_status == "98");
        }

        public String to_csv()
        {
            return this.loan_id.ToString() + ",\"" + this.loan.loan_name + "\"," +
                this.loan.due_date_next_payment.ToString() + ","  + this.loan.loan_type + "," + this.collector.collector;
        }
    }
}