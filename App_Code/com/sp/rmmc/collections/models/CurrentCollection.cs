using System;
using System.Collections.Generic;
using System.Web;

using System.Data.Common;

using com.sp.rmmc.common.models;
using com.sp.rmmc.common.models.events;

namespace com.sp.rmmc.collections.models
{
    public class CurrentCollection : BaseCollection
    {
        protected override string default_columns
        {
            get
            {
                return "" +
" base.loan_id as loan_id, " +
"(select top 1 misc.misc_dt_value from ms_loan_misc_fields misc where misc_field_id = 10020 and misc.loan_id = base.loan_id order by misc.misc_dt_value ) as demand_letter_due_date, " +
"(select top 1 misc.misc_field_value from ms_loan_misc_fields misc where misc_field_id = 10023 and misc.loan_id = base.loan_id order by misc.misc_field_value ) as foreclosure_officers, " +
"(select top 1 ms_fcl_info.fcl_type from ms_fcl_info where ms_fcl_info.loan_id = base.loan_id order by ms_fcl_info.fcl_type) as fcl_type, " +
"(select top 1 hist.paid_dt from ms_loan_history hist where hist.loan_id = base.loan_id and hist.trans_type_cd = 'REG' order by paid_dt desc) as last_reg_paid_dt, " +
"(select sum(hist.pmt_amt) from ms_loan_history hist where hist.loan_id = base.loan_id and (hist.trans_type_cd = 'REG' or  hist.trans_type_cd = 'REGR') and hist.paid_dt >=  (select top 1 hist.paid_dt from ms_loan_history hist where hist.loan_id = base.loan_id and hist.trans_type_cd = 'REG' order by paid_dt desc) ) as last_payments_balance, " +
"(select top 1 ms_fcl.fcl_item_date from ms_fcl_item_amts_1 ms_fcl where ms_fcl.item_id = 1000 and ms_fcl.loan_id = base.loan_id order by ms_fcl.fcl_item_date ) as papers_to_attorney_date, " +
"(select top 1 ms_fcl.fcl_item_date from ms_fcl_item_amts_1 ms_fcl where ms_fcl.item_id = 1200 and ms_fcl.loan_id = base.loan_id order by ms_fcl.fcl_item_date ) as posting_date, " +
"(select top 1 ms_credit_information.chapter_filed from ms_credit_information where ms_credit_information.loan_id = base.loan_id order by ms_credit_information.chapter_filed ) as chapter_filed, " +
"(select top 1 ms_credit_information.bankruptcy_filed_date from ms_credit_information where ms_credit_information.loan_id = base.loan_id order by ms_credit_information.bankruptcy_filed_date ) as bankruptcy_filed_date, " +
"(select top 1 ms_bankruptcy_info.discharge_date from ms_bankruptcy_info where ms_bankruptcy_info.loan_id = base.loan_id order by ms_bankruptcy_info.discharge_date ) as bankruptcy_discharge_date, " +
"(select top 1 ms_bankruptcy_info.case_dismissed_date from ms_bankruptcy_info where ms_bankruptcy_info.loan_id = base.loan_id order by ms_bankruptcy_info.case_dismissed_date ) as bankruptcy_case_dismissed_date, " +
"(select top 1 ms_bankruptcy_info.closed_date from ms_bankruptcy_info where ms_bankruptcy_info.loan_id = base.loan_id order by ms_bankruptcy_info.closed_date ) as bankruptcy_closed_date, " +
"(select top 1 mfia1.fcl_item_date from ms_fcl_item_amts_1 mfia1 where mfia1.item_id = 1600 and mfia1.loan_id = base.loan_id order by mfia1.fcl_item_date ) as sale_date, " +
"(select top 1 ms_loan_balances.ytd_prin_paid_amt from ms_loan_balances where ms_loan_balances.loan_id = base.loan_id order by ms_loan_balances.ytd_prin_paid_amt) as ytd_prin_paid_amt, " +
"(select top 1 abs(ms_loan_history.prin_amt) from ms_loan_history where ms_loan_history.loan_id = base.loan_id and ms_loan_history.trans_type_cd = 'PIF'  order by ms_loan_history.paid_dt desc) as principal_amt_from_history, " +
"(select top 1 abs(convert(numeric, uad_audit_data_bf_chg)) from ms_user_audit_history_data where uad_audit_field_name = 'prin_bal' and loan_id = base.loan_id and isnumeric(uad_audit_data_bf_chg) = 1 order by uad_audit_chg_dt desc) as audit_prin_bal, " +
"(select top 1 memo.memo_create_dt from ms_loan_memo memo where memo.loan_id = base.loan_id and memo.memo_type_id = 139 order by memo_id desc) as fc_foreclosure_referral_date, " +
"(select top 1 memo.memo_create_dt from ms_loan_memo memo where memo.loan_id = base.loan_id and memo.memo_type_id = 148 order by memo_id desc) as fc_payoff_date, " +
"(select top 1 ms_fcl.fcl_item_date from ms_fcl_item_amts_1 ms_fcl where ms_fcl.item_id = 1100 and ms_fcl.loan_id = base.loan_id order by ms_fcl.fcl_item_date )  as appraisal_ordered_date, " +
"(select top 1 ms_fcl.fcl_item_recvd_expd from ms_fcl_item_amts_1 ms_fcl where ms_fcl.item_id = 1200 and ms_fcl.loan_id = base.loan_id order by ms_fcl.fcl_item_recvd_expd ) as foreclosure_date, " +
"(select top 1 mlsc.stop_code_id from ms_loan_stop_code mlsc where mlsc.stop_code_id = 153 and mlsc.loan_id = base.loan_id order by mlsc.stop_code_id ) as lm_stop_code_id, " +
"(select top 1 memo.memo_create_dt  from ms_loan_memo memo join ms_memo_types on memo.memo_type_id = ms_memo_types.memo_type_id where memo.loan_id = base.loan_id and ms_memo_types.memo_type_desc like 'FC-3rd%' order by memo.memo_create_dt desc ) as memo_fc_third_party_sale_date, " +
"(select top 1 ms_fcl.fcl_item_ord_req from ms_fcl_item_amts_2 ms_fcl where ms_fcl.item_id = 10800 and ms_fcl.loan_id = base.loan_id order by ms_fcl.fcl_item_ord_req )  as extension_ordered_date, " +
"(select top 1 ms_fcl_info.fcl_deed_in_lieu from ms_fcl_info where ms_fcl_info.fcl_deed_in_lieu = 'Y' and ms_fcl_info.loan_id = base.loan_id order by ms_fcl_info.fcl_deed_in_lieu )  as fcl_deed_in_lieu, " +
"(null)  as lm_date, " +
"(select top 1 ms_fcl.fcl_item_date from ms_fcl_item_amts_1 ms_fcl where ms_fcl.item_id = 1900 and ms_fcl.loan_id = base.loan_id order by ms_fcl.fcl_item_date )  as possession_date, " +
"(select top 1 ms_fcl.fcl_item_date from ms_fcl_item_amts_1 ms_fcl where ms_fcl.item_id = 3900 and ms_fcl.loan_id = base.loan_id order by ms_fcl.fcl_item_date )  as deed_from_borrower_received_date, " +
"(select top 1 ms_credit_information.mortgage_status from ms_credit_information where ms_credit_information.loan_id = base.loan_id order by ms_credit_information.mortgage_status ) as mortgage_status, " +
com.sp.rmmc.common.models.MsLoan.columns("base.loan_id") + " ";
            }
        }

        protected override string collections_query
        {
            get
            {
                return
                    "( " +
                    " select base.loan_id " +
                    " from ms_loan_information as base, ms_credit_information " +
                    //" where (base.due_date_next_payment < getDate() or base.loan_id = 144745) " +
                    " where (base.due_date_next_payment < getDate()) " +
                    " and ms_credit_information.loan_id = base.loan_id " +
                    " and (base.loan_type != 'FHA' or ms_credit_information.mortgage_status in ('42', '11', '12', '', '67', '98') or ms_credit_information.mortgage_status is null ) " +
                    ") ";
            }
        }

        protected override string collections_4_month_delinquent_query
        {
            get
            {
                return
                    "( " +
                    " select base.loan_id " +
                    " from ms_loan_information as base, ms_credit_information " +
                    " where ( base.due_date_next_payment <= dateadd(mm, -3, getDate()) )" +
                    " and ms_credit_information.loan_id = base.loan_id " +
                    " and (base.loan_type != 'FHA' or ms_credit_information.mortgage_status in ('42', '11', '12', '', '67', '98', 'AP', 'A0', 'AQ') or ms_credit_information.mortgage_status is null ) " +
                    ") ";
            }
        }

        protected override string collections_2_month_fha_delinquent_query
        {
            get
            {
                return
                    "( " +
                    " select base.loan_id " +
                    " from ms_loan_information as base, ms_credit_information " +
                    " where ( base.due_date_next_payment <= dateadd(mm, -1, getDate()) )" +
                    " and   ( base.due_date_next_payment >= dateadd(mm, -2, getDate()) )" +
                    " and   ( dateadd(mm, 1, base.due_date_next_payment) <= dateadd(dd, -5, getDate()) )" +
                    " and ms_credit_information.loan_id = base.loan_id " +
                    " and (base.loan_type = 'FHA') " +
                    ") ";
            }
        }

        protected override string collections_3_month_fha_hud_delinquent_query
        {
            get
            {
                return
                    "( " +
                    " select base.loan_id " +
                    " from ms_loan_information as base, ms_credit_information " +
                    " where ( base.due_date_next_payment <= dateadd(mm, -2, getDate()) )" +
                    " and   ( base.due_date_next_payment >= dateadd(mm, -3, getDate()) )" +
                    " and   ( dateadd(mm, 1, base.due_date_next_payment) <= dateadd(dd, -2, getDate()) )" +
                    " and ms_credit_information.loan_id = base.loan_id " +
                    " and (base.loan_type = 'FHA') " +
                    ") ";
            }
        }

        private string collections_17_days_delinquent_query
        {
            get
            {
                return
                    " select base.loan_id " +
                    " from ms_loan_information as base, ms_credit_information " +
                    " where ( base.due_date_next_payment <= dateadd(dd, -16, getDate()) )" +
                    " and ( base.due_date_next_payment > dateadd(mm, -1, getDate()) )" +
                    " and ms_credit_information.loan_id = base.loan_id ";
            }
        }

        protected override string collections_17_days_fha_va_delinquent_query
        {
            get
            {
                return
                    "( " +
                    collections_17_days_delinquent_query +
                    " and (base.loan_type = 'FHA' or  base.loan_type = 'VA') " +
                    ") ";
            }
        }

        protected override string collections_17_days_cnv_non_c_mcm_delinquent_query
        {
            get
            {
                return
                    "( " +
                    collections_17_days_delinquent_query +
                    " and base.loan_type = 'CNV' " +
                    " and base.loan_plan_name not like 'C-MCM%' " +
                    ") ";
            }
        }

        protected override string collections_17_days_cnv_c_mcm_delinquent_query
        {
            get
            {
                return
                    "( " +
                    collections_17_days_delinquent_query +
                    " and base.loan_type = 'CNV' " +
                    " and base.loan_plan_name like 'C-MCM%' " +
                    ") ";
            }
        }

        protected override string all_types_query
        {
            get
            {
                return
                    collections_query +
                    " UNION " +
                    collections_4_month_delinquent_query +
                    " ";
            }
        }


        public CurrentCollection()
        {
            
        }

        protected override List<BaseCollection> get_loan_list(String query)
        {
            List<BaseCollection> list = new List<BaseCollection>();
            SybaseModel model = new SybaseModel();

            DbDataReader reader = model.getReader(query);
            while (reader.Read())
            {
                BaseCollection f = new BaseCollection();
                f.type = this.type;
                f.read_loan(reader);
                list.Add(f);
            }
            reader.Close();
            model.close_connection();
            return list;
        }

        protected override string getEventsColumns()
        {
            string s = "";
            foreach (BaseEvents be in this.getEventsTypes())
            {
                s += ", \n" + be.query("base.loan_id");
            }
            return s;
        }

        protected override void get_lm_date(List<BaseCollection> accepted)
        {
            foreach (BaseCollection f in accepted)
            {
                if (f.lm_stop_code_id > 0)
                {
                    f.lm_date.set_date(DateTime.Today);
                }
            }
            /*
            List<LMReception> all_lm_receptions = LMReception.getLMReceptions(accepted);
            Dictionary<decimal, LMReception> loan_lm_receptions = new Dictionary<decimal, LMReception>();
            foreach (LMReception lm_reception in all_lm_receptions)
                loan_lm_receptions.Add(lm_reception.loan_id, lm_reception);
            foreach (BaseCollection f in accepted)
            {
                if (loan_lm_receptions.ContainsKey(f.loan_id))
                {
                    f.lm_date = loan_lm_receptions[f.loan_id].date;
                }
            }
            */ 
        }
    }
}
