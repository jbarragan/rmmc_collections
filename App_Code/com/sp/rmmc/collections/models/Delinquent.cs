using System;
using System.Collections.Generic;
using System.Web;

using System.Data.Common;

using com.sp.rmmc.common.models;

namespace com.sp.rmmc.collections.models
{
    public class Delinquent : ForeclosuresCommonModel
    {
        public decimal loan_id = 0M;
        public DateTimeObject due_date_first_payment = new DateTimeObject();
        public decimal unapplied_bal = 0M;
        public decimal last_payments_balance = 0M;
        public decimal late_chrg_due_amt = 0M;
        public String default_reason_code = "";
        public MsLoan loan = new MsLoan();

        private static string base_query = "" +
"select base.loan_id, " +
"base.due_date_first_payment, " +
"(select top 1 ms_loan_balances.unapplied_bal from ms_loan_balances where ms_loan_balances.loan_id = base.loan_id order by ms_loan_balances.unapplied_bal ) as unapplied_bal, " +
"(select sum(hist.pmt_amt) from ms_loan_history hist where hist.loan_id = base.loan_id and hist.trans_type_cd in ('REG', 'REGR') and hist.paid_dt >=  dateadd(dd, -30, getdate())) as last_payments_balance, " +
"(select top 1 ms_loan_balances.late_chrg_due_amt from ms_loan_balances where ms_loan_balances.loan_id = base.loan_id order by ms_loan_balances.late_chrg_due_amt ) as late_chrg_due_amt, " +
"(select top 1 ms_credit_information.default_reason_code from ms_credit_information where ms_credit_information.loan_id = base.loan_id order by ms_credit_information.default_reason_code ) as default_reason_code, " +

MsLoan.columns("base.loan_id") +  " " +
"from ms_loan_information as base " +
"where 1 = 1 ";

        public static string in_three_month_delinquent_conditions()
        {
            string query = " and ( ms_loan_due_date_next_payment <= dateadd(mm, -3,getdate()) ) \n" +
                           " and ( ms_loan_prin_bal > 0 ) \n";
            return query;
        }

        public static List<Delinquent> in_three_month_delinquent_mode()
        {
            String query = base_query + in_three_month_delinquent_conditions();
            query += " order by ms_loan_due_date_next_payment";
            return get_list(query);
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
            delinquent.due_date_first_payment = readDBDateObject(reader, pos++, DateTime.Now);
            delinquent.unapplied_bal = readDBDecimal(reader, pos++);
            delinquent.last_payments_balance = readDBDecimal(reader, pos++);
            delinquent.late_chrg_due_amt = readDBDecimal(reader, pos++);
            delinquent.default_reason_code = readDBString(reader, pos++);

            if (delinquent.loan_id > 0)
            {
                delinquent.loan = MsLoan.read(reader, pos);
            }
            return delinquent;
        }
    }
}