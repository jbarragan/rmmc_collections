using System;
using System.Collections.Generic;
using System.Web;

using System.Data.Common;

using com.sp.rmmc.common.models;
using com.sp.rmmc.common.tools;

namespace com.sp.rmmc.collections.models
{
    public class Delinquent : ForeclosuresCommonModel, ICSVExport
    {
        public decimal loan_id = 0M;
        public DateTimeObject due_date_first_payment = new DateTimeObject();
        public DateTimeObject due_date_next_payment = new DateTimeObject();
        public decimal unapplied_bal = 0M;
        public decimal last_payments_balance = 0M;
        public decimal late_chrg_due_amt = 0M;
        public String default_reason_code = "";
        public DateTimeObject last_paid_date_in_3_months = new DateTimeObject();
        public DateTimeObject demand_letter_due_date = new DateTimeObject();
        public DateTimeObject bankruptcy_filed_date = new DateTimeObject();
        public DateTimeObject promised_to_pay_date = new DateTimeObject();
        public DateTimeObject lm_reception = new DateTimeObject();
        public DateTimeObject repay_start_date = new DateTimeObject();
        public MsLoan loan = new MsLoan();

        private static string base_query = "" +
"select base.loan_id, " +
"base.due_date_first_payment, " +
"(select top 1 ms_loan_info.due_date_next_payment from ms_loan_information ms_loan_info where ms_loan_info.loan_id = base.loan_id order by ms_loan_info.due_date_next_payment ) as due_date_next_payment, " +
"(select top 1 ms_loan_balances.unapplied_bal from ms_loan_balances where ms_loan_balances.loan_id = base.loan_id order by ms_loan_balances.unapplied_bal ) as unapplied_bal, " +
"(select sum(hist.pmt_amt) from ms_loan_history hist where hist.loan_id = base.loan_id and hist.trans_type_cd in ('REG', 'REGR') and hist.paid_dt >=  dateadd(dd, -30, getdate())) as last_payments_balance, " +
"(select top 1 ms_loan_balances.late_chrg_due_amt from ms_loan_balances where ms_loan_balances.loan_id = base.loan_id order by ms_loan_balances.late_chrg_due_amt ) as late_chrg_due_amt, " +
"(select top 1 ms_credit_information.default_reason_code from ms_credit_information where ms_credit_information.loan_id = base.loan_id order by ms_credit_information.default_reason_code ) as default_reason_code, " +
"(select top 1 hist.paid_dt from ms_loan_history hist where hist.loan_id = base.loan_id and hist.trans_type_cd in ('REG') and hist.paid_dt >=  dateadd(mm, -3, getdate()) order by hist.paid_dt desc) as last_paid_date_in_3_months, " +
"(select top 1 misc.misc_dt_value from ms_loan_misc_fields misc where misc_field_id = 10020 and misc.loan_id = base.loan_id and misc.misc_dt_value > base.due_date_next_payment order by misc.misc_dt_value desc ) as demand_letter_due_date, " +
"(select top 1 ms_credit_information.bankruptcy_filed_date from ms_credit_information where ms_credit_information.loan_id = base.loan_id and ms_credit_information.bankruptcy_filed_date > base.due_date_next_payment " +
"  and ( select top 1 ms_bankruptcy_info.discharge_date from ms_bankruptcy_info where ms_bankruptcy_info.loan_id = base.loan_id order by ms_bankruptcy_info.discharge_date ) is null " +
"  and ( select top 1 ms_bankruptcy_info.case_dismissed_date from ms_bankruptcy_info where ms_bankruptcy_info.loan_id = base.loan_id order by ms_bankruptcy_info.case_dismissed_date ) is null " +
"  and ( select top 1 ms_bankruptcy_info.closed_date from ms_bankruptcy_info where ms_bankruptcy_info.loan_id = base.loan_id order by ms_bankruptcy_info.closed_date ) is null " +
"  order by ms_credit_information.bankruptcy_filed_date ) as bankruptcy_filed_date, " +
"(select top 1 memo.memo_notify_dt from ms_loan_memo memo, ms_memo_types where memo.loan_id = base.loan_id and memo.memo_type_id = ms_memo_types.memo_type_id and ms_memo_types.memo_type_desc = 'BC-Promised to Pay' and memo_notify_dt > getdate() order by memo_notify_dt desc ) as promised_to_pay_date, " +
"(select top 1 repay.repay_start_date from ms_loan_repayment_plan repay where repay.loan_id = base.loan_id and repay.repay_start_date <= getdate() and repay.repay_end_date >= getdate() order by repay.repay_start_date desc ) as repay_start_date, " +

MsLoan.columns("base.loan_id") +  " " +
"from ms_loan_information as base " +
"where 1 = 1 ";

        public static string in_three_month_delinquent_conditions()
        {
            string query = " and ( ms_loan_due_date_next_payment >= dateadd(mm, -3,getdate()) ) and ms_loan_due_date_next_payment <= dateadd(mm, -1,getdate()) \n" +
                           " and ( ms_loan_prin_bal > 0 ) \n";
            return query;
        }

        public static List<Delinquent> in_three_month_delinquent_mode()
        {
            String query = base_query + in_three_month_delinquent_conditions();
            query += " order by ms_loan_due_date_next_payment";
            List<Delinquent> result_list = get_list(query);
            set_lm_reception(result_list);
            return result_list;
        }

        private static List<Delinquent> get_list(String query)
        {
            List<Delinquent> list = new List<Delinquent>();
            DbDataReader reader = getReader(query);
            while (reader.Read())
            {
                Delinquent delinquent = new Delinquent();
                delinquent = read(reader);
                list.Add(delinquent);
            }
            reader.Close();
            close_connection();
            return list;
        }

        public static Delinquent get(decimal loan_id)
        {
            string query = base_query + " and loan_id = " + loan_id.ToString();
            Delinquent delinquent = new Delinquent();
            DbDataReader reader = getReader(query);
            if (reader.Read())
                delinquent = read(reader);
            reader.Close();
            return delinquent;
        }

        private static Delinquent read(DbDataReader reader)
        {
            Delinquent delinquent = new Delinquent();
            int pos = 0;
            delinquent.loan_id = readDBDecimal(reader, pos++);
            delinquent.due_date_next_payment = readDBDateObject(reader, pos++, DateTime.Now);
            delinquent.due_date_first_payment = readDBDateObject(reader, pos++, DateTime.Now);
            delinquent.unapplied_bal = readDBDecimal(reader, pos++);
            delinquent.last_payments_balance = readDBDecimal(reader, pos++);
            delinquent.late_chrg_due_amt = readDBDecimal(reader, pos++);
            delinquent.default_reason_code = readDBString(reader, pos++);
            delinquent.last_paid_date_in_3_months = readDBDateObject(reader, pos++, DateTime.Now);
            delinquent.demand_letter_due_date = readDBDateObject(reader, pos++, DateTime.Now);
            delinquent.bankruptcy_filed_date = readDBDateObject(reader, pos++, DateTime.Now);
            delinquent.promised_to_pay_date = readDBDateObject(reader, pos++, DateTime.Now);
            delinquent.repay_start_date = readDBDateObject(reader, pos++, DateTime.Now);

            if (delinquent.loan_id > 0)
            {
                delinquent.loan = MsLoan.read(reader, pos);
            }
            return delinquent;
        }

        public String to_csv()
        {
            return this.loan_id.ToString();
        }

        public bool is_3_month_no_activity()
        {
            return this.last_paid_date_in_3_months.isNull && 
                   this.demand_letter_due_date.isNull && 
                   this.bankruptcy_filed_date.isNull && 
                   this.promised_to_pay_date.isNull && 
                   this.lm_reception.isNull && 
                   this.repay_start_date.isNull;
        }

        private static void set_lm_reception(List<Delinquent> list)
        {   
            List<LMReception> lm_receptions = LMReception.get_delinquent_lm_receptions(list);
            List<decimal> list_lm_reception_loan_ids = new List<decimal>();
            List<DateTime> list_lm_reception_dates = new List<DateTime>();
            decimal[] array_lm_reception_loan_ids;
            DateTime[] array_lm_reception_dates;
            foreach (LMReception reception in lm_receptions)
            {
                list_lm_reception_loan_ids.Add(reception.loan_id);
                list_lm_reception_dates.Add(reception.date.date);
            }
            array_lm_reception_loan_ids = list_lm_reception_loan_ids.ToArray();
            array_lm_reception_dates = list_lm_reception_dates.ToArray();
            foreach (Delinquent d in list){
                int index = Array.IndexOf(array_lm_reception_loan_ids, d.loan_id);
                if (index >= 0) d.lm_reception.set_date(array_lm_reception_dates[index]);
            }
        }
    }
}