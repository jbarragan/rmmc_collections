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
        public DateTimeObject last_reg_due_dt = new DateTimeObject();
        public decimal last_reg_prin_amt = 0M;
        public decimal previous_month_prin_amt = 0M;
        public decimal previous_month_int_amt = 0M;
        
        public decimal last_reg_cur_amt = 0M;
        public decimal last_after_reg_cur_amt = 0M;
        public decimal last_reg_int_amt = 0M;
        public decimal last_payments_balance = 0M;
        public DateTimeObject papers_to_attorney_date = new DateTimeObject();
        public DateTimeObject posting_date = new DateTimeObject();
        public string chapter_filed = "";
        public DateTimeObject bankruptcy_filed_date = new DateTimeObject();
        public DateTimeObject bankruptcy_discharge_date = new DateTimeObject();
        public DateTimeObject bankruptcy_case_dismissed_date = new DateTimeObject();
        public DateTimeObject bankruptcy_closed_date = new DateTimeObject();
        public DateTimeObject bankruptcy_release_of_stay_date = new DateTimeObject();
        public DateTimeObject fc_foreclosure_referral_date = new DateTimeObject();
        public DateTimeObject fc_payoff_date = new DateTimeObject();
        public DateTimeObject appraisal_ordered_date = new DateTimeObject();
        public DateTimeObject foreclosure_date = new DateTimeObject();
        public int lm_stop_code_id = 0;
        public int bankrupcty_ch_13_stop_code_id = 0;
        public DateTimeObject memo_fc_third_party_sale_date = new DateTimeObject();
        public DateTimeObject extension_ordered_date = new DateTimeObject();
        public string fcl_deed_in_lieu = "";
        public DateTimeObject lm_date = new DateTimeObject();
        public DateTimeObject possession_date = new DateTimeObject();
        public DateTimeObject deed_from_borrower_received_date = new DateTimeObject();
        public string mortgage_status = "";
        public string inv_loan_nbr = "";
        public string mortgage_status_desc = "";
        public DateTimeObject last_property_inspection_date = new DateTimeObject();
        public string loan_plan_name = "";
        public int stop_code_id = 0;
        public string stop_code_description = "";
        public string stop_code_description_90_day = "";
        public string stop_code_description_67_Prop_Insp_Fee = "";
        public string stop_code_description_78_Partial_Claim = "";
        public string stop_code_description_78_HUD_Claim_loss = "";
        public string stop_code_description_FNMA_HAMP_MODIFICATION = "";
        public string alert_type = "";
        public DateTimeObject mortgage_status_date = new DateTimeObject();
        public string ci_reason_code = "";
        public string default_reason_desc = "";
        public string agency_case = "";

        public string last_memo_type_desc = "";
        public string last_memo_category_desc = "";
        public DateTimeObject last_memo_created_dt = new DateTimeObject();
        public DateTimeObject last_memo_promise_to_pay_notify_dt = new DateTimeObject();
        public DateTimeObject last_current_paid_dt = new DateTimeObject();
        public DateTimeObject last_attempted_call_dt = new DateTimeObject();
        public int current_month_attempted_calls = 0;

        public decimal monthly_payment_amount = 0M;
        public DateTimeObject program_end_dt = new DateTimeObject();

        public string pmi_company_name = "";

        public DateTimeObject sale_date = new DateTimeObject();
        public decimal ytd_prin_paid_amt = 0M;
        public decimal principal_amt_from_history = 0M;
        public decimal audit_prin_bal = 0M;

        public com.sp.rmmc.common.models.MsLoan loan = new com.sp.rmmc.common.models.MsLoan();
        public LoanCollector collector = new LoanCollector();

        public CollectionReasonCodeEvents reason_code = null;
        public CollectionPromiseToPayEvents promise_to_pay_events = null;
        public string inv_bank_cd = "";
        
        public string filter_reason = "";
        public string type = "";
        public const string COLLECTION_TYPE = "Collection";
        public const string FHA_COLLECTION_LM_TYPE = "FHA Collection LM";
        public const string COVID_COLLECTION_LM_TYPE = "COVID Collection LM";
        public const string ALL_COLLECTION_LM_TYPE = "ALL Collection LM";
        public const string FHA_EXCOLLECTION_HUDDEL_TYPE = "FHA ExCollection HUDDEL42";
        public const string BANKRUPTCY_REPORT_TYPE = "Bankruptcy Report";

        public const string FHA_COLLECTION_MISSING_OCCUPANCY_CODE_TYPE = "FHA Collection Missing Occupancy Code";
        public const string FHA_COLLECTION_MISSING_MORTGAGE_STATUS_DATE_TYPE = "FHA Collection Missing Mortgage Status Date";
        public const string FHA_COLLECTION_MISSING_MORTGAGE_STATUS_CODE_TYPE = "FHA Collection Missing Mortgage Status Code";
        public const string FHA_COLLECTION_MISSING_REASON_CODE_TYPE = "FHA Collection Missing Reason Code";
        public const string MONTH_CURRENT_WITH_RESTRICT_AUTOPAY_DRAFT = "RestrictAutopayDraft";
        public const string ALL_TYPE = "All";

        public const string BANKRUPTCY_TYPE = "Bankruptcy";
        public const string DEMAND_TYPE = "Demand";
        public const string FORECLOSURE_TYPE = "Foreclosure";
        public const string NO_CONTACT_TYPE = "No Contact List";
        public const string FANNIE_MAE_DELINQUENCY_REPORT = "Fannie Mae Delinquency Report";
        public const string NATION_STAR_PDF = "Nation Star PDF";
        public const string FNMA_HAMP_REPORTING = "FNMA HAMP Reporting";

        public const string FNMA_TYPE = "FNMA";
        public const string GNMA_TYPE = "GNMA";
        public const string CNV_TYPE = "Conventional";

        public const string DEFAULT_REPORT = "Default";
        public const string REASON_CODE_REPORT = "Reason";
        public const string PROMISED_TO_REPORT = "Promised";
        public const string FHA_60_DAY_COLLECTION = "FHA 60 DAY COLLECTION";
        public const string WITH_PROMISED_TO_PAY_CALLING_LIST = "WITH_PROMISED_TO_PAY_CALLING_LIST";
        public const string FANNIE_MAE_17_DAY_CALL_LISTING = "Fannie Mae 17 day Call Listing";
        public string report_type = DEFAULT_REPORT;

        public const decimal DEBUG_LOAN_ID = 51002012M;

        protected virtual string default_columns { get { return ""; } }
        protected virtual string collections_query { get { return ""; } }
        protected virtual string promised_to_pay_query { get { return ""; } }
        protected virtual string month_current_with_restrict_autopay_draft_query { get { return ""; } }
        protected virtual string collections_4_month_delinquent_query { get { return ""; } }
        protected virtual string collections_17_days_fha_va_delinquent_query { get { return ""; } }
        protected virtual string collections_17_days_fha_delinquent_query { get { return ""; } }
        protected virtual string collections_17_days_fha_delinquent_query_part1 { get { return ""; } }
        protected virtual string collections_17_days_fha_delinquent_query_part2 { get { return ""; } }
        protected virtual string collections_17_days_va_delinquent_query { get { return ""; } }
        protected virtual string collections_17_days_cnv_non_c_mcm_delinquent_query { get { return ""; } }
        protected virtual string collections_17_days_cnv_c_mcm_delinquent_query { get { return ""; } }
        protected virtual string collections_2_month_fha_delinquent_query { get { return ""; } }
        protected virtual string collections_2_month_conv_delinquent_query { get { return ""; } }
        protected virtual string collections_2_month_va_delinquent_query { get { return ""; } }
        protected virtual string collections_3_month_fha_hud_delinquent_query { get { return ""; } }
        protected virtual string collections_3_month_conv_delinquent_query { get { return ""; } }
        protected virtual string collections_3_month_va_delinquent_query { get { return ""; } }
        protected virtual string collections_3_month_or_more_delinquent_query { get { return ""; } }
        protected virtual string collections_3_month_or_more_delinquent_query_gnma { get { return ""; } }
        protected virtual string collections_cnv_1_month_or_more_delinquent_query { get { return ""; } }
        protected virtual string collections_fnma_hamp_stop_query { get { return ""; } }
        
        protected virtual string all_types_query { get { return ""; } }

        protected virtual string all_bankruptcies_query { get { return ""; } }
        protected virtual string all_bankruptcies_txvet_nation_star_query { get { return ""; } }
        protected virtual string all_demands_query { get { return ""; } }
        protected virtual string all_no_contact_query { get { return ""; } }
        protected virtual string all_foreclosures_query { get { return ""; } }
        protected virtual string all_collections_query { get { return ""; } }
        protected virtual string fha_collections_in_loss_mitigation_query { get { return ""; } }
        protected virtual string covid_collections_in_loss_mitigation_query { get { return ""; } }
        protected virtual string all_collections_in_loss_mitigation_query { get { return ""; } }
        protected virtual string fha_collections_query { get { return ""; } }
        protected virtual string fha_excollections_huddel42_query { get { return ""; } }
        protected virtual string bankrupcty_report_query { get { return ""; } }

        public void getCollections(List<BaseCollection> collections, List<BaseCollection> removed_collections)
        {
            this.type = COLLECTION_TYPE;
            string columns = this.default_columns + getEventsColumns();
            string query = columns + " from " + "( " + collections_query + ")base " +
                CurrentCollection.default_joins + " where 1=1 " + collections_conditions();
            query = query + " order by ms_loan_due_date_next_payment, ms_loan_type";
            List<BaseCollection> all = this.get_loan_list(query);
            foreach (BaseCollection f in all) { f.type = COLLECTION_TYPE; collections.Add(f); }
            commonFilters(collections, removed_collections);
        }

        public void getFHACollectionsInLossMitigation(List<BaseCollection> collections, List<BaseCollection> removed_collections)
        {
            this.type = FHA_COLLECTION_LM_TYPE;
            string columns = this.default_columns + getEventsColumns();
            string query = columns + " from " + "( " + fha_collections_in_loss_mitigation_query + ")base " +
                CurrentCollection.default_joins + " where 1=1 " + collections_conditions();
            query = query + " order by mortgage_status, ms_loan_due_date_next_payment, ms_loan_type";
            List<BaseCollection> all = this.get_loan_list(query);
            foreach (BaseCollection f in all) { f.type = FHA_COLLECTION_LM_TYPE; collections.Add(f); }
        }

        public void getCovidInLossMitigation(List<BaseCollection> collections, List<BaseCollection> removed_collections)
        {
            this.type = COVID_COLLECTION_LM_TYPE;
            string columns = this.default_columns + getEventsColumns();
            string query = columns + " from " + "( " + covid_collections_in_loss_mitigation_query + ")base " +
                CurrentCollection.default_joins + " where 1=1 " + collections_conditions();
            query = query + " order by ms_loan_type";
            List<BaseCollection> all = this.get_loan_list(query);
            foreach (BaseCollection f in all) { f.type = COVID_COLLECTION_LM_TYPE; collections.Add(f); }
        }

        public void getAllCollectionsInLossMitigation(List<BaseCollection> collections, List<BaseCollection> removed_collections)
        {
            this.type = ALL_COLLECTION_LM_TYPE;
            string columns = this.default_columns + getEventsColumns();
            string query = columns + " from " + "( " + all_collections_in_loss_mitigation_query + ")base " +
                CurrentCollection.default_joins + " where 1=1 " + collections_conditions();
            query = query + " order by mortgage_status, ms_loan_due_date_next_payment, ms_loan_type";
            List<BaseCollection> all = this.get_loan_list(query);
            foreach (BaseCollection f in all) { f.type = ALL_COLLECTION_LM_TYPE; collections.Add(f); }
        }

        public void getFHAExCollectionsHUDDEL42(List<BaseCollection> collections, List<BaseCollection> removed_collections)
        {
            this.type = FHA_EXCOLLECTION_HUDDEL_TYPE;
            string columns = this.default_columns + getEventsColumns();
            string query = columns + " from " + "( " + fha_excollections_huddel42_query + ")base " +
                CurrentCollection.default_joins + " where 1=1 " + collections_conditions();
            query = query + " order by mortgage_status, ms_loan_due_date_next_payment, ms_loan_type";
            List<BaseCollection> all = this.get_loan_list(query);
            foreach (BaseCollection f in all) { f.type = FHA_EXCOLLECTION_HUDDEL_TYPE; collections.Add(f); }
        }

        public void getBankruptcyReportLoans(List<BaseCollection> collections, List<BaseCollection> removed_collections)
        {
            this.type = BANKRUPTCY_REPORT_TYPE;
            string columns = this.default_columns + getEventsColumns();
            string query = columns + " from " + "( " + bankrupcty_report_query + ")base " +
                CurrentCollection.default_joins + " where 1=1 " + collections_conditions();
            query = query + " order by ms_loan_due_date_next_payment, ms_loan_id";
            List<BaseCollection> all = this.get_loan_list(query);
            foreach (BaseCollection f in all) { f.type = BANKRUPTCY_REPORT_TYPE; collections.Add(f); }
        }

        public void getFHACollectionsMissingOccupancyCode(List<BaseCollection> collections, List<BaseCollection> removed_collections)
        {
            this.type = FHA_COLLECTION_MISSING_OCCUPANCY_CODE_TYPE;
            string columns = this.default_columns + getEventsColumns();
            string query = columns + " from " + "( " + fha_collections_query + ")base " +
                CurrentCollection.default_joins + " where 1=1 " + collections_missing_occupancy_conditions();
            query = query + " order by ms_loan_due_date_next_payment, ms_loan_type";
            List<BaseCollection> all = this.get_loan_list(query);
            foreach (BaseCollection f in all) { f.type = FHA_COLLECTION_MISSING_OCCUPANCY_CODE_TYPE; collections.Add(f); }
        }

        public void getFHACollectionsMissingMortgageStatusDate(List<BaseCollection> collections, List<BaseCollection> removed_collections)
        {
            this.type = FHA_COLLECTION_MISSING_MORTGAGE_STATUS_DATE_TYPE;
            string columns = this.default_columns + getEventsColumns();
            string query = columns + " from " + "( " + fha_collections_query + ")base " +
                CurrentCollection.default_joins + " where 1=1 " + collections_missing_mortgage_status_date_conditions();
            query = query + " order by ms_loan_due_date_next_payment, ms_loan_type";
            List<BaseCollection> all = this.get_loan_list(query);
            foreach (BaseCollection f in all) { f.type = FHA_COLLECTION_MISSING_MORTGAGE_STATUS_DATE_TYPE; collections.Add(f); }
        }

        public void getFHACollectionsMissingMortgageStatusCode(List<BaseCollection> collections, List<BaseCollection> removed_collections)
        {
            this.type = FHA_COLLECTION_MISSING_MORTGAGE_STATUS_CODE_TYPE;
            string columns = this.default_columns + getEventsColumns();
            string query = columns + " from " + "( " + fha_collections_query + ")base " +
                CurrentCollection.default_joins + " where 1=1 " + collections_missing_mortgage_status_code_conditions();
            query = query + " order by ms_loan_due_date_next_payment, ms_loan_type";
            List<BaseCollection> all = this.get_loan_list(query);
            foreach (BaseCollection f in all) { f.type = FHA_COLLECTION_MISSING_MORTGAGE_STATUS_CODE_TYPE; collections.Add(f); }
        }

        public void getFHACollectionsMissingReasonCode(List<BaseCollection> collections, List<BaseCollection> removed_collections)
        {
            this.type = FHA_COLLECTION_MISSING_REASON_CODE_TYPE;
            string columns = this.default_columns + getEventsColumns();
            string query = columns + " from " + "( " + fha_collections_query + ")base " +
                CurrentCollection.default_joins + " where 1=1 " + collections_missing_reason_code_conditions();
            query = query + " order by ms_loan_due_date_next_payment, ms_loan_type";
            List<BaseCollection> all = this.get_loan_list(query);
            foreach (BaseCollection f in all) { f.type = FHA_COLLECTION_MISSING_REASON_CODE_TYPE; collections.Add(f); }
        }

        public void getMonthCurrentWithRestrictAutopayDraft(List<BaseCollection> collections, List<BaseCollection> removed_collections)
        {
            this.type = MONTH_CURRENT_WITH_RESTRICT_AUTOPAY_DRAFT;
            string columns = this.default_columns + getEventsColumns();
            string query = columns + " from " + "( " + month_current_with_restrict_autopay_draft_query + ")base " +
                CurrentCollection.default_joins + " where 1=1 " + not_90_day_delq_stop_code_collections_conditions();
            query = query + " order by ms_loan_due_date_next_payment, ms_loan_type, ms_loan_id";
            List<BaseCollection> all = this.get_loan_list(query);
            foreach (BaseCollection f in all) { f.type = this.type; collections.Add(f); }
        }

        public void getCollectionsAndDemands(List<BaseCollection> collections, List<BaseCollection> removed_collections, string report_type)
        {
            this.type = COLLECTION_TYPE;
            string base_query = collections_query;
            string extra_where = " ";
            if (report_type == BaseCollection.PROMISED_TO_REPORT)
            {
                base_query = promised_to_pay_query;
            }
            string columns = this.default_columns + getEventsColumns();
            int offset = 0;
            int limit = 100;
            int all_count = 0;
            int previous_count = 0;
            List<BaseCollection> all = new List<BaseCollection>();
            do
            {
                int offset_plus_limit = offset + limit;
                string query = columns +
                @" 
                 from " + "( " + base_query + ")base " +
                CurrentCollection.default_joins + " where 1=1 and base.rownum >= " + offset + " and base.rownum  <= " + offset_plus_limit +
                extra_where + collections_conditions();
                query = query + " order by ms_loan_due_date_next_payment, ms_loan_type";
                List<BaseCollection> chunk = this.get_loan_list(query);
                previous_count = all.Count;
                all.AddRange(chunk);
                offset = offset_plus_limit;
            } while (previous_count < all.Count);

            foreach (BaseCollection f in all) { f.type = COLLECTION_TYPE; collections.Add(f); }
            commonDemandCollectionFilters(collections, removed_collections);
        }

        public void getCollectionsAndDemandsDifferentReasonCodes(List<BaseCollection> collections, List<BaseCollection> removed_collections, string report_type)
        {
            DifferentReasonCodes different_reason_codes_loans = new DifferentReasonCodes();
            List<DifferentReasonCodes> loans = different_reason_codes_loans.getLoanWithDifferentReasonCodes();
            foreach (DifferentReasonCodes f in loans)
            {
                BaseCollection b = new BaseCollection();
                b.type = COLLECTION_TYPE;
                b.loan_id = f.loan_id;
                b.loan.loan_name = f.loan_name;
                b.loan.loan_type = f.loan_type;
                b.loan.due_date_next_payment = f.due_date_next_payment;
                b.loan.prin_bal = f.prin_bal;
                b.loan.int_rate = f.int_rate;
                b.loan.fha_occupancy_code = f.occupancy_code;
                b.reason_code = new CollectionReasonCodeEvents();
                b.reason_code.reason_code = f.reason_code;
                b.reason_code.last_memo_reason_code = f.last_memo_reason_code;
                b.reason_code.memo_create_dt = f.memo_create_dt;

                collections.Add(b);
            }
        }

        public void getCollectionsAndDemandsWithReportType(List<BaseCollection> collections, List<BaseCollection> removed_collections, string report_type)
        {
            getCollectionsAndDemands(collections, removed_collections, report_type);
            foreach (BaseCollection f in collections) { f.report_type = report_type;}
        }

        public void getCollectionsAndDemandsDifferentReasonCodesWithReportType(List<BaseCollection> collections, List<BaseCollection> removed_collections, string report_type)
        {
            getCollectionsAndDemandsDifferentReasonCodes(collections, removed_collections, report_type);
            foreach (BaseCollection f in collections) { f.report_type = report_type; }
        }

        

        public void getCollections4MonthDelinquent(List<BaseCollection> collections, List<BaseCollection> removed_collections)
        {
            this.type = COLLECTION_TYPE;
            string columns = this.default_columns + getEventsColumns();
            string query = columns + " from " + "( " + collections_4_month_delinquent_query + ")base " +
                CurrentCollection.default_joins + " where 1=1 " + collections_conditions();
            query = query + " order by ms_loan_due_date_next_payment, ms_loan_type";
            List<BaseCollection> all = this.get_loan_list(query);
            foreach (BaseCollection f in all) { f.type = COLLECTION_TYPE; collections.Add(f); }
            remove_loss_mitigation(collections, removed_collections);
            commonFilters(collections, removed_collections);
        }

        public void getCollections2MonthFHADelinquent(List<BaseCollection> collections, List<BaseCollection> removed_collections)
        {
            this.type = COLLECTION_TYPE;
            string columns = this.default_columns + getEventsColumns();
            string query = columns + " from " + "( " + collections_2_month_fha_delinquent_query + ")base " +
                CurrentCollection.default_joins + " where 1=1 " + collections_conditions();
            query = query + " order by ms_loan_id ";
            List<BaseCollection> all = this.get_loan_list(query);
            foreach (BaseCollection f in all) { f.type = FHA_60_DAY_COLLECTION; collections.Add(f); }
            commonFilters(collections, removed_collections);
            remove_repayment_plan(collections, removed_collections);
            remove_loss_mitigation(collections, removed_collections);
            (new LoanCollector()).get_collections_collector(collections);
        }

        public void getCollections2MonthCONVDelinquent(List<BaseCollection> collections, List<BaseCollection> removed_collections)
        {
            this.type = COLLECTION_TYPE;
            string columns = this.default_columns + getEventsColumns();
            string query = columns + " from " + "( " + collections_2_month_conv_delinquent_query + ")base " +
                CurrentCollection.default_joins + " where 1=1 " + collections_conditions();
            query = query + " order by ms_loan_id ";
            List<BaseCollection> all = this.get_loan_list(query);
            foreach (BaseCollection f in all) { f.type = COLLECTION_TYPE; collections.Add(f); }
            commonFilters(collections, removed_collections);
            remove_repayment_plan(collections, removed_collections);
            remove_loss_mitigation(collections, removed_collections);
            (new LoanCollector()).get_collections_collector(collections);
        }

        public void getCollections2MonthVADelinquent(List<BaseCollection> collections, List<BaseCollection> removed_collections)
        {
            this.type = COLLECTION_TYPE;
            string columns = this.default_columns + getEventsColumns();
            string query = columns + " from " + "( " + collections_2_month_va_delinquent_query + ")base " +
                CurrentCollection.default_joins + " where 1=1 " + collections_conditions();
            query = query + " order by ms_loan_id ";
            List<BaseCollection> all = this.get_loan_list(query);
            foreach (BaseCollection f in all) { f.type = COLLECTION_TYPE; collections.Add(f); }
            commonFilters(collections, removed_collections);
            remove_repayment_plan(collections, removed_collections);
            remove_loss_mitigation(collections, removed_collections);
            (new LoanCollector()).get_collections_collector(collections);
        }

        public void getCollections3MonthFHAHudDelinquent(List<BaseCollection> collections, List<BaseCollection> removed_collections)
        {
            this.type = COLLECTION_TYPE;
            string columns = this.default_columns + getEventsColumns();
            string query = columns + " from " + "( " + collections_3_month_fha_hud_delinquent_query + ")base " +
                CurrentCollection.default_joins + " where 1=1 " + collections_conditions();
            query = query + " order by ms_loan_id ";
            List<BaseCollection> all = this.get_loan_list(query);
            foreach (BaseCollection f in all) { f.type = COLLECTION_TYPE; collections.Add(f); }
            commonFilters(collections, removed_collections);
            remove_repayment_plan(collections, removed_collections);
            remove_loss_mitigation(collections, removed_collections);
            // remove_foreclosures_standalone(collections, removed_collections);
            (new LoanCollector()).get_collections_collector(collections);
        }

        public void getFNMA3MonthDelinquent(List<BaseCollection> collections, List<BaseCollection> removed_collections)
        {
            this.type = FNMA_TYPE;
            string columns = this.default_columns + getEventsColumns();
            string query = columns + " from " + "( " + collections_3_month_or_more_delinquent_query + ")base " +
                CurrentCollection.default_joins + " where 1=1 " + fnma_collections_conditions();
            query = query + " order by ms_loan_id ";
            List<BaseCollection> all = this.get_loan_list(query);
            foreach (BaseCollection f in all) { f.type = FNMA_TYPE; collections.Add(f); }
        }

        public void getCNVMoreThanOneMonthDelinquent(List<BaseCollection> collections, List<BaseCollection> removed_collections)
        {
            this.type = FANNIE_MAE_DELINQUENCY_REPORT;
            string columns = this.default_columns + getEventsColumns();
            string query = columns + " from " + "( " + collections_cnv_1_month_or_more_delinquent_query + ")base " +
                CurrentCollection.default_joins + " where 1=1 " + end_of_month_reports_conditions();
            query = query + " order by ms_loan_due_date_next_payment, ms_loan_id ";
            List<BaseCollection> all = this.get_loan_list(query);
            foreach (BaseCollection f in all) { f.type = FANNIE_MAE_DELINQUENCY_REPORT; collections.Add(f); }
        }

        public void getFNMAHAMPStopLoans(List<BaseCollection> collections, List<BaseCollection> removed_collections)
        {
            this.type = FNMA_HAMP_REPORTING;
            string columns = this.default_columns + getEventsColumns();
            string query = columns + " from " + "( " + collections_fnma_hamp_stop_query + ")base " +
                CurrentCollection.default_joins + " where 1=1 " + end_of_month_reports_conditions() + 
                " and stop_code_description_FNMA_HAMP_MODIFICATION is not NULL and alert_type = 'MOD' ";
            query = query + " order by ms_loan_due_date_next_payment, ms_loan_id ";
            List<BaseCollection> all = this.get_loan_list(query);
            foreach (BaseCollection f in all) { f.type = FNMA_HAMP_REPORTING; collections.Add(f); }
        }

        public void getGNMA3MonthDelinquent(List<BaseCollection> collections, List<BaseCollection> removed_collections)
        {
            this.type = GNMA_TYPE;
            string columns = this.default_columns + getEventsColumns();
            string query = columns + " from " + "( " + collections_3_month_or_more_delinquent_query_gnma + ")base " +
                CurrentCollection.default_joins + " where 1=1 " + gnma_collections_conditions();
            query = query + " order by ms_loan_type, ms_loan_id ";
            List<BaseCollection> all = this.get_loan_list(query);
            foreach (BaseCollection f in all) { f.type = GNMA_TYPE; collections.Add(f); }
        }

        public void getCollections3MonthCONVDelinquent(List<BaseCollection> collections, List<BaseCollection> removed_collections)
        {
            this.type = COLLECTION_TYPE;
            string columns = this.default_columns + getEventsColumns();
            string query = columns + " from " + "( " + collections_3_month_conv_delinquent_query + ")base " +
                CurrentCollection.default_joins + " where 1=1 " + collections_conditions();
            query = query + " order by ms_loan_id ";
            List<BaseCollection> all = this.get_loan_list(query);
            foreach (BaseCollection f in all) { f.type = WITH_PROMISED_TO_PAY_CALLING_LIST; collections.Add(f); }
            commonFilters(collections, removed_collections);
            remove_repayment_plan(collections, removed_collections);
            remove_loss_mitigation(collections, removed_collections);
            (new LoanCollector()).get_collections_collector(collections);
        }

        public void getCollections3MonthVADelinquent(List<BaseCollection> collections, List<BaseCollection> removed_collections)
        {
            this.type = COLLECTION_TYPE;
            string columns = this.default_columns + getEventsColumns();
            string query = columns + " from " + "( " + collections_3_month_va_delinquent_query + ")base " +
                CurrentCollection.default_joins + " where 1=1 " + collections_conditions();
            query = query + " order by ms_loan_id ";
            List<BaseCollection> all = this.get_loan_list(query);
            foreach (BaseCollection f in all) { f.type = WITH_PROMISED_TO_PAY_CALLING_LIST; collections.Add(f); }
            commonFilters(collections, removed_collections);
            remove_repayment_plan(collections, removed_collections);
            remove_loss_mitigation(collections, removed_collections);
            (new LoanCollector()).get_collections_collector(collections);
        }

        public void getCollections17DayDelinquentFHAVA(List<BaseCollection> collections, List<BaseCollection> removed_collections)
        {
            List<UnfilteredCollection> unfiltered = (new UnfilteredCollection()).getFHAVA17DayDelinquent();
            foreach (UnfilteredCollection u in unfiltered) {
                BaseCollection b = new BaseCollection();
                b.type = "OTHER";
                b.loan_id = u.loan_id;
                b.loan.loan_name = u.loan_name;
                b.loan.loan_type = u.loan_type;
                b.loan.due_date_next_payment = u.due_date_next_payment;
                b.last_memo_promise_to_pay_notify_dt = u.last_memo_promise_to_pay_notify_dt;
                b.last_attempted_call_dt = u.last_attempted_call_dt;
                b.current_month_attempted_calls = u.current_month_attempted_calls;
                collections.Add(b); 
            }
            (new LoanCollector()).get_collections_collector(collections);

        }

        public void getCollections17DayDelinquentFHAVAOld(List<BaseCollection> collections, List<BaseCollection> removed_collections)
        {
            this.type = COLLECTION_TYPE;
            string columns = this.default_columns + getEventsColumns();
            int offset = 0;
            int limit = 50;
            int all_count = 0;
            int previous_count = 0;
            List<BaseCollection> all = new List<BaseCollection>();
            string base_query = collections_17_days_fha_va_delinquent_query;
            string extra_where = " ";
            do
            {
                int offset_plus_limit = offset + limit;
                string query = columns +
                @" 
                 from " + "( " + base_query + ")base " +
                CurrentCollection.default_joins + " where 1=1 and base.rownum >= " + offset + " and base.rownum  <= " + offset_plus_limit +
                extra_where + collections_conditions();
                query = query + " order by ms_loan_due_date_next_payment, ms_loan_type";
                List<BaseCollection> chunk = this.get_loan_list(query);
                previous_count = all.Count;
                all.AddRange(chunk);
                offset = offset_plus_limit;
            } while (previous_count < all.Count);

            foreach (BaseCollection f in all) { f.type = WITH_PROMISED_TO_PAY_CALLING_LIST; collections.Add(f); }

            commonFilters(collections, removed_collections);
            remove_loss_mitigation(collections, removed_collections);
            (new LoanCollector()).get_collections_collector(collections);
        }

        public void getCollections17DayDelinquentCNVNonCMCM(List<BaseCollection> collections, List<BaseCollection> removed_collections)
        {
            List<UnfilteredCollection> unfiltered = (new UnfilteredCollection()).getCONVNonCMCM17DayDelinquent();
            foreach (UnfilteredCollection u in unfiltered)
            {
                BaseCollection b = new BaseCollection();
                b.type = WITH_PROMISED_TO_PAY_CALLING_LIST;
                b.loan_id = u.loan_id;
                b.loan.loan_name = u.loan_name;
                b.loan.loan_type = u.loan_type;
                b.loan.due_date_next_payment = u.due_date_next_payment;
                b.last_memo_promise_to_pay_notify_dt = u.last_memo_promise_to_pay_notify_dt;
                b.last_attempted_call_dt = u.last_attempted_call_dt;
                b.current_month_attempted_calls = u.current_month_attempted_calls;
                collections.Add(b);
            }
            (new LoanCollector()).get_collections_collector(collections);
        }

        public void getCollections17DayDelinquentCNVNonCMCMOld(List<BaseCollection> collections, List<BaseCollection> removed_collections)
        {
            this.type = COLLECTION_TYPE;
            string columns = this.default_columns + getEventsColumns();
            int offset = 0;
            int limit = 100;
            int all_count = 0;
            int previous_count = 0;
            List<BaseCollection> all = new List<BaseCollection>();
            string base_query = collections_17_days_cnv_non_c_mcm_delinquent_query;
            string extra_where = " ";
            do
            {
                int offset_plus_limit = offset + limit;
                string query = columns +
                @" 
                 from " + "( " + base_query + ")base " +
                CurrentCollection.default_joins + " where 1=1 and base.rownum >= " + offset + " and base.rownum  <= " + offset_plus_limit +
                extra_where + collections_conditions();
                query = query + " order by ms_loan_id";
                List<BaseCollection> chunk = this.get_loan_list(query);
                previous_count = all.Count;
                all.AddRange(chunk);
                offset = offset_plus_limit;
            } while (previous_count < all.Count);

            foreach (BaseCollection f in all) { f.type = FANNIE_MAE_17_DAY_CALL_LISTING; collections.Add(f); }
            commonFilters(collections, removed_collections);
            remove_loss_mitigation(collections, removed_collections);
            (new LoanCollector()).get_collections_collector(collections);
        }

        public void getCollections17DayDelinquentCNVCMCM(List<BaseCollection> collections, List<BaseCollection> removed_collections)
        {
            this.type = COLLECTION_TYPE;
            string columns = this.default_columns + getEventsColumns();
            string query = columns + " from " + "( " + collections_17_days_cnv_c_mcm_delinquent_query + ")base " +
                CurrentCollection.default_joins + " where 1=1 " + collections_conditions();
            query = query + " order by ms_loan_id ";
            List<BaseCollection> all = this.get_loan_list(query);
            foreach (BaseCollection f in all) {
                f.type = FANNIE_MAE_17_DAY_CALL_LISTING; collections.Add(f);
            }
            commonFilters(collections, removed_collections);
            remove_loss_mitigation(collections, removed_collections);
            (new LoanCollector()).get_collections_collector(collections);
        }

        public void getBankruptcies(List<BaseCollection> collections, List<BaseCollection> removed_collections)
        {
            this.type = BANKRUPTCY_TYPE;
            string columns = this.default_columns + getEventsColumns();
            string query = columns + " from " + "( " + all_bankruptcies_query + ")base " +
                CurrentCollection.default_joins + " where 1=1 " + end_of_month_reports_conditions();
            query = query + " order by ms_loan_type, ms_loan_due_date_next_payment desc, ms_loan_id";
            List<BaseCollection> all = this.get_loan_list(query);
            foreach (BaseCollection f in all) { f.type = BANKRUPTCY_TYPE; collections.Add(f); }
        }

        public void getBankruptciesTxVetNationStar(List<BaseCollection> collections, List<BaseCollection> removed_collections)
        {
            this.type = NATION_STAR_PDF;
            string columns = this.default_columns + getEventsColumns();
            string query = columns + " from " + "( " + all_bankruptcies_txvet_nation_star_query + ")base " +
                CurrentCollection.default_joins + " where 1=1 " + end_of_month_reports_conditions();
            query = query + " order by ms_loan_type, ms_loan_due_date_next_payment desc, ms_loan_id";
            List<BaseCollection> all = this.get_loan_list(query);
            foreach (BaseCollection f in all) { f.type = NATION_STAR_PDF; collections.Add(f); }
        }

        public void getDemands(List<BaseCollection> collections, List<BaseCollection> removed_collections)
        {
            this.type = DEMAND_TYPE;
            string columns = this.default_columns + getEventsColumns();
            string query = columns + " from " + "( " + all_demands_query + ")base " +
                CurrentCollection.default_joins + " where 1=1 " + end_of_month_reports_conditions();
            query = query + " order by ms_loan_type, ms_loan_due_date_next_payment desc, ms_loan_id";
            List<BaseCollection> all = this.get_loan_list(query);
            foreach (BaseCollection f in all) { f.type = DEMAND_TYPE; collections.Add(f); }
        }

        public void getNoContacts(List<BaseCollection> collections, List<BaseCollection> removed_collections)
        {
            this.type = NO_CONTACT_TYPE;
            string columns = this.default_columns + getEventsColumns();
            string query = columns + " from " + "( " + all_no_contact_query + ")base " +
                CurrentCollection.default_joins + " where 1=1 " + no_contacts_conditions();
            query = query + " order by ms_loan_type, ms_loan_due_date_next_payment desc, ms_loan_id";
            List<BaseCollection> all = this.get_loan_list(query);
            foreach (BaseCollection f in all) { f.type = NO_CONTACT_TYPE; collections.Add(f); }
            commonFilters(collections, removed_collections);
            remove_foreclosures(collections, removed_collections);
            remove_loss_mitigation(collections, removed_collections);
        }

        public void getForeclosures(List<BaseCollection> collections, List<BaseCollection> removed_collections)
        {
            this.type = FORECLOSURE_TYPE;
            string columns = this.default_columns + getEventsColumns();
            string query = columns + " from " + "( " + all_foreclosures_query + ")base " +
                CurrentCollection.default_joins + " where 1=1 " + end_of_month_reports_conditions();
            query = query + " order by ms_loan_type, ms_loan_due_date_next_payment desc, ms_loan_id";
            List<BaseCollection> all = this.get_loan_list(query);
            foreach (BaseCollection f in all) { f.type = FORECLOSURE_TYPE; collections.Add(f); }
        }

        public void getForeclosureLoan(decimal loan_id, List<BaseCollection> accepted, List<BaseCollection> removed, String type)
        {
            this.type = type;
            string columns = this.default_columns + getEventsColumns();
            string query = columns + " from " + "( " + all_types_query + ")base " +
                CurrentCollection.default_joins + " where 1=1 " + single_loan_conditions(loan_id);
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
            cmd.Parameters.AddWithValue("@last_reg_due_dt", this.last_reg_due_dt.toDBInsert());
            cmd.Parameters.AddWithValue("@last_reg_prin_amt", this.last_reg_prin_amt);
            cmd.Parameters.AddWithValue("@previous_month_prin_amt", this.previous_month_prin_amt);
            cmd.Parameters.AddWithValue("@previous_month_int_amt", this.previous_month_int_amt);
            cmd.Parameters.AddWithValue("@last_reg_int_amt", this.last_reg_int_amt);
            cmd.Parameters.AddWithValue("@last_payments_balance", this.last_payments_balance);
            cmd.Parameters.AddWithValue("@papers_to_attorney_date", this.papers_to_attorney_date.toDBInsert());
            cmd.Parameters.AddWithValue("@posting_date", this.posting_date.toDBInsert());
            cmd.Parameters.AddWithValue("@chapter_filed", this.chapter_filed);
            cmd.Parameters.AddWithValue("@bankruptcy_filed_date", this.bankruptcy_filed_date.toDBInsert());
            cmd.Parameters.AddWithValue("@bankruptcy_discharge_date", this.bankruptcy_discharge_date.toDBInsert());
            cmd.Parameters.AddWithValue("@bankruptcy_case_dismissed_date", this.bankruptcy_case_dismissed_date.toDBInsert());
            cmd.Parameters.AddWithValue("@bankruptcy_closed_date", this.bankruptcy_closed_date.toDBInsert());
            cmd.Parameters.AddWithValue("@bankruptcy_release_of_stay_date", this.bankruptcy_release_of_stay_date.toDBInsert());
            cmd.Parameters.AddWithValue("@sale_date", this.sale_date.toDBInsert());
            cmd.Parameters.AddWithValue("@ytd_prin_paid_amt", this.ytd_prin_paid_amt);
            cmd.Parameters.AddWithValue("@principal_amt_from_history", this.principal_amt_from_history);
            cmd.Parameters.AddWithValue("@audit_prin_bal", this.audit_prin_bal);
            cmd.Parameters.AddWithValue("@fc_foreclosure_referral_date", this.fc_foreclosure_referral_date.toDBInsert());
            cmd.Parameters.AddWithValue("@fc_payoff_date", this.fc_payoff_date.toDBInsert());
            cmd.Parameters.AddWithValue("@appraisal_ordered_date", this.appraisal_ordered_date.toDBInsert());
            cmd.Parameters.AddWithValue("@foreclosure_date", this.foreclosure_date.toDBInsert());
            cmd.Parameters.AddWithValue("@lm_stop_code_id", this.lm_stop_code_id);
            cmd.Parameters.AddWithValue("@bankrupcty_ch_13_stop_code_id", this.bankrupcty_ch_13_stop_code_id);
            cmd.Parameters.AddWithValue("@memo_fc_third_party_sale_date", this.memo_fc_third_party_sale_date.toDBInsert());
            cmd.Parameters.AddWithValue("@extension_ordered_date", this.extension_ordered_date.toDBInsert());
            cmd.Parameters.AddWithValue("@fcl_deed_in_lieu", this.fcl_deed_in_lieu);
            cmd.Parameters.AddWithValue("@lm_date", this.lm_date.toDBInsert());
            cmd.Parameters.AddWithValue("@possession_date", this.possession_date.toDBInsert());
            cmd.Parameters.AddWithValue("@deed_from_borrower_received_date", this.deed_from_borrower_received_date.toDBInsert());
            cmd.Parameters.AddWithValue("@mortgage_status", this.mortgage_status);
            cmd.Parameters.AddWithValue("@inv_loan_nbr", this.inv_loan_nbr);
            cmd.Parameters.AddWithValue("@mortgage_status_desc", this.mortgage_status_desc);
            cmd.Parameters.AddWithValue("@last_property_inspection_date", this.last_property_inspection_date.toDBInsert());
            cmd.Parameters.AddWithValue("@loan_plan_name", this.loan_plan_name);
            cmd.Parameters.AddWithValue("@stop_code_id", this.stop_code_id);
            cmd.Parameters.AddWithValue("@stop_code_description", this.stop_code_description);
            cmd.Parameters.AddWithValue("@stop_code_description_90_day", this.stop_code_description_90_day);
            cmd.Parameters.AddWithValue("@stop_code_description_67_Prop_Insp_Fee", this.stop_code_description_67_Prop_Insp_Fee);
            cmd.Parameters.AddWithValue("@stop_code_description_78_Partial_Claim", this.stop_code_description_78_Partial_Claim);
            cmd.Parameters.AddWithValue("@stop_code_description_78_HUD_Claim_loss", this.stop_code_description_78_HUD_Claim_loss);
            cmd.Parameters.AddWithValue("@stop_code_description_FNMA_HAMP_MODIFICATION", this.stop_code_description_FNMA_HAMP_MODIFICATION);
            cmd.Parameters.AddWithValue("@alert_type", this.alert_type);
            cmd.Parameters.AddWithValue("@mortgage_status_date", this.mortgage_status_date.toDBInsert());
            cmd.Parameters.AddWithValue("@ci_reason_code", this.ci_reason_code);
            cmd.Parameters.AddWithValue("@default_reason_desc", this.default_reason_desc);
            cmd.Parameters.AddWithValue("@agency_case", this.agency_case);
            cmd.Parameters.AddWithValue("@last_memo_type_desc", this.last_memo_type_desc);
            cmd.Parameters.AddWithValue("@last_memo_category_desc", this.last_memo_category_desc);
            cmd.Parameters.AddWithValue("@last_memo_created_dt", this.last_memo_created_dt.toDBInsert());
            cmd.Parameters.AddWithValue("@last_memo_promise_to_pay_notify_dt", this.last_memo_promise_to_pay_notify_dt.toDBInsert());
            cmd.Parameters.AddWithValue("@last_current_paid_dt", this.last_current_paid_dt.toDBInsert());
            cmd.Parameters.AddWithValue("@inv_bank_cd", this.inv_bank_cd);
            cmd.Parameters.AddWithValue("@last_attempted_call_dt", this.last_attempted_call_dt.toDBInsert());
            cmd.Parameters.AddWithValue("@pmi_company_name", this.pmi_company_name);
            cmd.Parameters.AddWithValue("@current_month_attempted_calls", this.current_month_attempted_calls);
            cmd.Parameters.AddWithValue("@monthly_payment_amount", this.monthly_payment_amount);
            cmd.Parameters.AddWithValue("@program_end_dt", this.program_end_dt.toDBInsert());
            
            
            
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
                if (f.loan_id == DEBUG_LOAN_ID)
                    f.loan_id = DEBUG_LOAN_ID;
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
                if (f.loan_id == DEBUG_LOAN_ID)
                    f.loan_id = DEBUG_LOAN_ID;
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
                if (f.loan_id == DEBUG_LOAN_ID)
                    f.loan_id = DEBUG_LOAN_ID;
                if (removed.Contains(f)) continue;
                if (f.bankruptcy_filed_date.isNull == false &&
                    (
                        (f.bankruptcy_discharge_date.isNull == true || f.bankruptcy_discharge_date.date < f.bankruptcy_filed_date.date) &&
                        (f.bankruptcy_case_dismissed_date.isNull == true || f.bankruptcy_case_dismissed_date.date < f.bankruptcy_filed_date.date) &&
                        f.bankruptcy_closed_date.isNull == true &&
                        f.bankruptcy_release_of_stay_date.isNull == true
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
                if (f.loan_id == DEBUG_LOAN_ID)
                    f.loan_id = DEBUG_LOAN_ID;
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
                if (f.loan_id == DEBUG_LOAN_ID)
                    f.loan_id = DEBUG_LOAN_ID;
                if (removed.Contains(f)) continue;
                if (f.mortgage_status == "09" ||
                    f.mortgage_status == "06" ||
                    f.mortgage_status == "15" ||
                    f.mortgage_status == "17" ||
                    f.mortgage_status == "28" ||
                    f.mortgage_status == "36" ||
                    f.mortgage_status == "37" ||
                    f.mortgage_status == "39" ||
                    f.mortgage_status == "41" ||
                    f.mortgage_status == "44" ||
                    f.mortgage_status == "AA" ||
                    f.mortgage_status == "AH" ||
                    f.mortgage_status == "08")
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
                        f.bankruptcy_closed_date.isNull == true &&
                        f.bankruptcy_release_of_stay_date.isNull == true
                    )
                   );
        }

        private static void remove_received_payment(List<BaseCollection> accepted, List<BaseCollection> removed)
        {   
            foreach (BaseCollection f in accepted)
            {

                if (f.loan_id == DEBUG_LOAN_ID)
                    f.loan_id = DEBUG_LOAN_ID;
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
            string query = " and ( " + MsLoan.MS_LOAN_PRIN_BAL + " > 0 )";
            return query;
        }

        public static string collections_missing_occupancy_conditions()
        {
            string query = " and ( " + MsLoan.MS_LOAN_PRIN_BAL + " > 0" +
                " and (select top 1 ms_occupancy_types.occ_description from ms_loan_occupancy, ms_occupancy_types where ms_occupancy_types.fha_occ_code = ms_loan_occupancy.fha_occ_code and ms_loan_occupancy.loan_id = base.loan_id order by ms_loan_occupancy.fcl_hud_notice_of_occ_sent desc, ms_loan_occupancy.prop_insp_date desc) is null )";
            return query;
        }

        public static string collections_missing_mortgage_status_date_conditions()
        {
            string query = " and ( " + MsLoan.MS_LOAN_PRIN_BAL + " > 0" +
                " and (select top 1 ms_credit_information.mortgage_status_date from ms_credit_information where ms_credit_information.loan_id = base.loan_id order by ms_credit_information.mortgage_status_date ) is null" +
                " and ( (select top 1 ms_credit_information.mortgage_status from ms_credit_information where ms_credit_information.loan_id = base.loan_id order by ms_credit_information.mortgage_status ) is not null" +
                " and (select top 1 ms_credit_information.mortgage_status from ms_credit_information where ms_credit_information.loan_id = base.loan_id order by ms_credit_information.mortgage_status ) != '') )";
            return query;
        }

        public static string collections_missing_mortgage_status_code_conditions()
        {
            string query = " and ( " + MsLoan.MS_LOAN_PRIN_BAL + " > 0" +
                " and ((select top 1 ms_credit_information.mortgage_status from ms_credit_information where ms_credit_information.loan_id = base.loan_id order by ms_credit_information.mortgage_status ) is null" +
                      " or (select top 1 ms_credit_information.mortgage_status from ms_credit_information where ms_credit_information.loan_id = base.loan_id order by ms_credit_information.mortgage_status ) = '') )";
            return query;
        }

        public static string collections_missing_reason_code_conditions()
        {
            string query = " and ( " + MsLoan.MS_LOAN_PRIN_BAL + " > 0" +
                " and (select top 1 ms_credit_information.default_reason_code from ms_credit_information where ms_credit_information.loan_id = base.loan_id order by ms_credit_information.default_reason_code ) is null )";
            return query;
        }

        public static string not_90_day_delq_stop_code_collections_conditions()
        {
            string query = " and ( 1=1" +
                " or (select top 1 msc.stop_description from ms_stop_code msc, ms_loan_stop_code mlsc where mlsc.loan_id = base.loan_id and msc.payment_stop_code_flag = 'Y' and msc.stop_code_id = mlsc.stop_code_id order by replace(msc.stop_description, '90-DAY', '00000') ) != '90-DAY DELQ ORIG REVIEW DONE'" +
                " or (select top 1 msc.stop_description from ms_stop_code msc, ms_loan_stop_code mlsc where mlsc.loan_id = base.loan_id and msc.payment_stop_code_flag = 'Y' and msc.stop_code_id = mlsc.stop_code_id order by replace(msc.stop_description, '90-DAY', '00000') ) is null ) \n" +
                           " and ( (select top 1 msc.stop_description from ms_stop_code msc, ms_loan_stop_code mlsc where mlsc.loan_id = base.loan_id and msc.stop_description like '78-HUD-Claim loss%' and msc.stop_code_id = mlsc.stop_code_id order by msc.stop_description ) is null ) " +
                           " and ( (select top 1 msc.stop_description from ms_stop_code msc, ms_loan_stop_code mlsc where mlsc.loan_id = base.loan_id and msc.stop_description like '78 Partial Claim%' and msc.stop_code_id = mlsc.stop_code_id order by msc.stop_description ) is null ) " +
                           " and ( (select top 1 msc.stop_description from ms_stop_code msc, ms_loan_stop_code mlsc where mlsc.loan_id = base.loan_id and msc.stop_description like '67 Prop. Insp. Fee%' and msc.stop_code_id = mlsc.stop_code_id order by msc.stop_description ) is null ) " +
                           " and ( (select top 1 msc.stop_description from ms_stop_code msc, ms_loan_stop_code mlsc where mlsc.loan_id = base.loan_id and msc.stop_description like '90-DAY%' and msc.stop_code_id = mlsc.stop_code_id order by msc.stop_description ) is null ) " +
                           " and ( " + MsLoan.MS_LOAN_PRIN_BAL + " > 0 )";
            return query;
        }

        public static string fnma_collections_conditions()
        {
            string query = " and ( " + MsLoan.MS_LOAN_INV_CD + " = '901' ) " +
                           " and ( " + MsLoan.MS_LOAN_PRIN_BAL + " > 0 )";
            query = " ";
            return query;
        }

        public static string gnma_collections_conditions()
        {
            string query = " and ( " + MsLoan.MS_LOAN_INV_CD + " = '800' ) " +
                           " and ( " + MsLoan.MS_LOAN_PRIN_BAL + " > 0 )";
            query = " ";
            return query;
        }

        public static string end_of_month_reports_conditions()
        {
            string query = " and ( " + MsLoan.MS_LOAN_PRIN_BAL + " > 0 )";
            return query;
        }

        public static string no_contacts_conditions()
        {
            string query = " and ( " + MsLoan.MS_LOAN_PRIN_BAL + " > 0 )" +
                " and ( (select top 1 mmt.memo_type_desc from ms_loan_memo mlm left join ms_memo_types mmt on mmt.memo_type_id = mlm.memo_type_id where mlm.loan_id = base.loan_id and mlm.memo_subject like 'BC%' order by mlm.memo_create_dt desc) = 'BC - No Contact '" +
                " or (select top 1 mmc.memo_category_desc from ms_loan_memo mlm left join ms_memo_categories mmc on mmc.memo_category_id = mlm.memo_category_id where mlm.loan_id = base.loan_id and mlm.memo_subject like 'BC%' order by mlm.memo_create_dt desc) = '031 Unable to Contact Borrower') " +
                " and " + MsLoan.MS_LOAN_DUE_DATE_NEXT_PAYMENT + " < getDate() ";
            return query;
        }

        public static string single_loan_conditions(decimal loan_id)
        {
            string query = " and ( base.loan_id = " + loan_id + " )";
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
                if (f.loan_id == DEBUG_LOAN_ID)
                    f.loan_id = DEBUG_LOAN_ID;

                if (removed.Contains(f)) continue;
                if (f.demand_letter_due_date.isNull == false && 
                    f.last_reg_paid_dt.date < f.demand_letter_due_date.date.AddDays(-33) &&
                    f.loan.due_date_next_payment.date < f.demand_letter_due_date.date
                    )
                {
                    f.filter_reason = "Demand";
                    removed.Add(f);
                }
            }
        }

        public void remove_foreclosures_standalone(List<BaseCollection> accepted, List<BaseCollection> removed)
        {
            List<BaseCollection> all_accepted = new List<BaseCollection>();
            all_accepted.AddRange(accepted);
            foreach (BaseCollection f in all_accepted)
            {
                if (f.loan_id == DEBUG_LOAN_ID)
                    f.loan_id = DEBUG_LOAN_ID;
                if (f.fcl_type == "A" || f.fcl_type == "C")
                {
                    f.filter_reason = "Foreclosure";
                    removed.Add(f);
                    accepted.Remove(f);
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
            f.last_reg_due_dt = readDBDateObject(reader, pos++);
            f.last_reg_prin_amt = readDBDecimal(reader, pos++);
            f.previous_month_prin_amt = readDBDecimal(reader, pos++);
            f.previous_month_int_amt = readDBDecimal(reader, pos++);
            f.last_reg_cur_amt = readDBDecimal(reader, pos++);
            f.last_after_reg_cur_amt = readDBDecimal(reader, pos++);
            f.last_reg_int_amt = readDBDecimal(reader, pos++);
            f.last_payments_balance = readDBDecimal(reader, pos++);
            f.papers_to_attorney_date = readDBDateObject(reader, pos++);
            f.posting_date = readDBDateObject(reader, pos++);
            f.chapter_filed = readDBString(reader, pos++);
            f.bankruptcy_filed_date = readDBDateObject(reader, pos++);
            f.bankruptcy_discharge_date = readDBDateObject(reader, pos++);
            f.bankruptcy_case_dismissed_date = readDBDateObject(reader, pos++);
            f.bankruptcy_closed_date = readDBDateObject(reader, pos++);
            f.bankruptcy_release_of_stay_date = readDBDateObject(reader, pos++);
            f.sale_date = readDBDateObject(reader, pos++);
            f.ytd_prin_paid_amt = readDBDecimal(reader, pos++);
            f.principal_amt_from_history = readDBDecimal(reader, pos++);
            f.audit_prin_bal = readDBDecimal(reader, pos++);
            f.fc_foreclosure_referral_date = readDBDateObject(reader, pos++);
            f.fc_payoff_date = readDBDateObject(reader, pos++);
            f.appraisal_ordered_date = readDBDateObject(reader, pos++);
            f.foreclosure_date = readDBDateObject(reader, pos++);
            f.lm_stop_code_id = readDBInt(reader, pos++);
            f.bankrupcty_ch_13_stop_code_id = readDBInt(reader, pos++);
            f.memo_fc_third_party_sale_date = readDBDateObject(reader, pos++);
            f.extension_ordered_date = readDBDateObject(reader, pos++);
            f.fcl_deed_in_lieu = readDBString(reader, pos++);
            f.lm_date = readDBDateObject(reader, pos++);
            f.possession_date = readDBDateObject(reader, pos++);
            f.deed_from_borrower_received_date = readDBDateObject(reader, pos++);
            f.mortgage_status = readDBString(reader, pos++);
            f.inv_loan_nbr = readDBString(reader, pos++);
            f.mortgage_status_desc = readDBString(reader, pos++);
            f.last_property_inspection_date = readDBDateObject(reader, pos++);
            f.loan_plan_name = readDBString(reader, pos++);
            f.stop_code_id = readDBInt(reader, pos++);
            f.stop_code_description = readDBString(reader, pos++);
            f.stop_code_description_90_day = readDBString(reader, pos++);
            f.stop_code_description_67_Prop_Insp_Fee = readDBString(reader, pos++);
            f.stop_code_description_78_Partial_Claim = readDBString(reader, pos++);
            f.stop_code_description_78_HUD_Claim_loss = readDBString(reader, pos++);
            f.stop_code_description_FNMA_HAMP_MODIFICATION = readDBString(reader, pos++);
            f.alert_type = readDBString(reader, pos++);
            f.mortgage_status_date = readDBDateObject(reader, pos++);
            f.ci_reason_code = readDBString(reader, pos++);
            f.default_reason_desc = readDBString(reader, pos++);
            f.agency_case = readDBString(reader, pos++);
            f.last_memo_type_desc = readDBString(reader, pos++);
            f.last_memo_category_desc = readDBString(reader, pos++);
            f.last_memo_created_dt = readDBDateObject(reader, pos++);
            f.last_memo_promise_to_pay_notify_dt = readDBDateObject(reader, pos++);
            f.last_current_paid_dt = readDBDateObject(reader, pos++);
            f.inv_bank_cd = readDBString(reader, pos++);
            f.last_attempted_call_dt = readDBDateObject(reader, pos++);
            f.pmi_company_name = readDBString(reader, pos++);
            f.current_month_attempted_calls = readDBInt(reader, pos++);
            f.monthly_payment_amount = readDBDecimal(reader, pos++);
            f.program_end_dt = readDBDateObject(reader, pos++);
            

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

        public decimal getTotalAmt()
        {
            BaseCollection c = this;
            decimal total = 0M;
            if (c.previous_month_prin_amt == 0M)
            {
                total = (c.last_reg_prin_amt + c.last_reg_cur_amt + c.last_after_reg_cur_amt);
            }
            else
            {
                total = (c.previous_month_prin_amt + c.last_reg_cur_amt + c.last_after_reg_cur_amt);
            }
            return total;
        }


        public decimal getTotalIntAmt()
        {
            BaseCollection c = this;
            decimal total = 0M;
            if (c.previous_month_int_amt == 0M)
            {
                total = (c.last_reg_int_amt);
            }
            else
            {
                total = (c.previous_month_int_amt);
            }
            return total;
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


            if (this.type == "Old Collector Listing")
            {
                return this.loan_id.ToString() + ",\"" + this.loan.loan_name + "\"," +
                this.loan.due_date_next_payment.ToString() + "," + this.loan.loan_type + "," + this.collector.collector;
            }

            if (this.report_type == REASON_CODE_REPORT)
            {
                return this.loan_id.ToString() + "," + this.loan.loan_type + ",\"" + this.loan.loan_name + "\"," +
                    this.loan.due_date_next_payment.ToString() + ",\"" + this.loan.prin_bal.ToString("C") + "\"," +
                    this.loan.int_rate.ToString("0.000") + "," + this.loan.occupancy_code + "," +
                    (this.reason_code != null ? this.reason_code.reason_code : "NA") + "," +
                    (this.reason_code != null ? this.reason_code.last_memo_reason_code : "NA") + "," +
                    (this.reason_code != null ? this.reason_code.memo_create_dt.ToString() : "NA");
            }
            if (this.report_type == PROMISED_TO_REPORT)
            {
                return this.loan_id.ToString() + "," + this.loan.loan_type + ",\"" + this.loan.loan_name + "\"," +
                    this.loan.due_date_next_payment.ToString() + "," +
                    ((this.promise_to_pay_events == null) ? "NA" : this.promise_to_pay_events.last_memo_notify_dt.ToString()) + "," +
                    ((this.promise_to_pay_events == null) ? "NA" : this.promise_to_pay_events.no_contact_memo_031_create_dt.ToString()) + ",\"" +
                    this.loan.prin_bal.ToString("C") + "\"";
            }
            if (this.type == DEMAND_TYPE ||
                this.type == BANKRUPTCY_TYPE ||
                this.type == FORECLOSURE_TYPE)
            {
                return this.loan_id.ToString() + ",\"" + this.loan.loan_name + "\"," + this.loan.loan_type + "," +
                    this.loan.due_date_next_payment.ToString() + "," + this.months_late() * 30 + ",\"" + this.loan.prin_bal.ToString("C") + "\"";
            }
            if (this.type == FANNIE_MAE_DELINQUENCY_REPORT)
            {
                return this.loan_id.ToString() + ",\"" +
                    this.inv_loan_nbr + "\",\"" +
                    this.loan.inv_cd + "\",\"" +
                    this.loan.loan_name + "\",\"" +
                    this.loan.due_date_next_payment.ToString() + "\",\"" +
                    this.mortgage_status + " - " + this.mortgage_status_desc + "\",\"" +
                    this.ci_reason_code + " - " + this.default_reason_desc + "\"";
            }
            if (this.type == FNMA_HAMP_REPORTING)
            {

                return this.loan_id.ToString() + ",\"" +
                    "909389186\",\"" +
                    this.loan.loan_name + "\",\"" +
                    this.last_reg_due_dt.ToString() + "\",\"" +
                    this.loan.prin_bal.ToString("C") + "\",\"" +
                    (this.getTotalAmt()).ToString("C") + "\",\"" +
                    this.getTotalIntAmt().ToString("C") + "\"";
            }
            if (this.type == NO_CONTACT_TYPE)
            {
                return this.loan_id.ToString() + ",\"" + this.loan.loan_name + "\"," + this.loan.loan_type + "," +
                    this.loan.due_date_next_payment.ToString() + "," + this.months_late() * 30 + ",\"" + this.loan.prin_bal.ToString("C") + "\",\"" +
                    this.last_memo_created_dt.ToShortDateString() + "\",\"" +
                    this.last_memo_type_desc + "\",\"" + 
                    this.last_memo_category_desc + "\"";
            }
            if (this.type == FNMA_TYPE)
            {
                return this.loan_id.ToString() + ",\"" + this.loan.loan_name + "\",\"" + this.loan.prin_bal.ToString("C") + "\"";
            }
            if (this.type == GNMA_TYPE)
            {
                return this.loan_id.ToString() + ",\"" + this.loan.loan_name + "\",\"" + this.loan.loan_type + "\",\"" + this.loan.prin_bal.ToString("C") + "\"";
            }
            if (this.type == MONTH_CURRENT_WITH_RESTRICT_AUTOPAY_DRAFT)
            {
                return this.loan_id.ToString() + ",\"" + this.loan.loan_name + "\"," + this.loan.loan_type + "," +
                    this.loan.due_date_next_payment.ToString() + ",\"" + this.loan.prin_bal.ToString("C") + "\",Y,\"" + this.stop_code_description + "\"";
            }
            if (this.type == COVID_COLLECTION_LM_TYPE)
            {
                return this.loan_id.ToString() + ",\"" + this.loan.loan_name + "\"," + this.loan.loan_type + "," + this.mortgage_status + "," + this.ci_reason_code + "," + this.mortgage_status_date.ToShortDateString() + "," +
                    this.loan.due_date_next_payment.ToString() + "," + this.program_end_dt.ToShortDateString() + ",\"" + this.monthly_payment_amount.ToString("C") + "\",\"" + this.loan.prin_bal.ToString("C") + "\"";
            }
            if (this.type == FHA_COLLECTION_LM_TYPE)
            {
                return this.loan_id.ToString() + ",\"" + this.loan.loan_name + "\"," + this.mortgage_status + "," + this.mortgage_status_date.ToShortDateString() + "," +
                    this.loan.due_date_next_payment.ToString();
            }
            if (this.type == ALL_COLLECTION_LM_TYPE)
            {
                return this.loan_id.ToString() + ",\"" + this.loan.loan_name + "\",\"" + this.loan.loan_type + "\"," + this.mortgage_status + "," + this.mortgage_status_date.ToShortDateString() + "," +
                    this.loan.due_date_next_payment.ToString();
            }
            if (this.type == FHA_COLLECTION_MISSING_OCCUPANCY_CODE_TYPE)
            {
                return this.loan_id.ToString() + ",\"" + this.loan.loan_name + "\"," + this.loan.due_date_next_payment.ToString();
            }
            if (this.type == FHA_COLLECTION_MISSING_REASON_CODE_TYPE)
            {
                return this.loan_id.ToString() + ",\"" + this.loan.loan_name + "\"," + this.loan.due_date_next_payment.ToString();
            }
            if (this.type == FHA_EXCOLLECTION_HUDDEL_TYPE)
            {
                return this.loan_id.ToString() + ",\"" + this.loan.loan_name + "\",\"" + this.mortgage_status + "\",\"" + this.mortgage_status_date.ToString() + "\",\"" + this.loan.due_date_next_payment.ToString() + "\",\"" + this.last_current_paid_dt.ToString() + "\"";
            }
            if (this.type == BANKRUPTCY_REPORT_TYPE)
            {
                return this.loan_id.ToString() + ",\"" + this.loan.loan_name + "\",\"" + this.loan.due_date_next_payment.ToString() + "\",\""
                    + (this.mortgage_status == "67" ? "67 Chapter 13 Bankruptcy" : "") + "\",\""
                    + (this.bankrupcty_ch_13_stop_code_id == 11 ? "56 Bankruptcy Chapter 13" : "") + "\""; ;
            }
            if (this.type == FHA_COLLECTION_MISSING_MORTGAGE_STATUS_DATE_TYPE)
            {
                return this.loan_id.ToString() + ",\"" + this.loan.loan_name + "\",\"" + this.mortgage_status + "\"," + this.loan.due_date_next_payment.ToString();
            }
            if (this.type == FHA_COLLECTION_MISSING_MORTGAGE_STATUS_CODE_TYPE)
            {
                return this.loan_id.ToString() + ",\"" + this.loan.loan_name + "\",\"" + this.agency_case + "\"," + this.loan.due_date_next_payment.ToString();
            }

            if (this.type == WITH_PROMISED_TO_PAY_CALLING_LIST)
            {
                return this.loan_id.ToString() + ",\"" + this.loan.loan_name + "\"," +
                    this.loan.due_date_next_payment.ToString() + "," +
                    ((this.last_memo_promise_to_pay_notify_dt.isNull == false && this.last_memo_promise_to_pay_notify_dt.date > DateTime.Today.AddDays(-1)) ? this.last_memo_promise_to_pay_notify_dt.ToString() : "") + "," +
                    this.loan.loan_type + "," + this.last_attempted_call_dt.ToString() + "," + this.collector.collector;
            }

            return this.loan_id.ToString() + ",\"" + this.loan.loan_name + "\"," +
                this.loan.due_date_next_payment.ToString() + "," +
                ((this.last_memo_promise_to_pay_notify_dt.isNull == false && this.last_memo_promise_to_pay_notify_dt.date > DateTime.Today.AddDays(-1)) ? this.last_memo_promise_to_pay_notify_dt.ToString() : "") + "," +
                this.loan.loan_type + "," + this.last_attempted_call_dt.ToString() + "," + this.collector.collector;
        }

        public int months_late()
        {
            return (loan.event_todays_date.AddDays(1).Year * 12 + loan.event_todays_date.AddDays(1).Month) -
                   (loan.due_date_next_payment.date.Year * 12 + loan.due_date_next_payment.date.Month);
        }
    }
}