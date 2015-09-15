using System;
using System.Collections.Generic;
using System.Web;
using System.Data.SqlClient;

using System.Data.Common;

using com.sp.rmmc.common.models;
using com.sp.rmmc.common.models.events;

namespace com.sp.rmmc.collections.models
{
    public class BaseEPD: DbModel
    {
        public decimal loan_id = 0M;
        public com.sp.rmmc.common.models.MsLoan loan = new com.sp.rmmc.common.models.MsLoan();

        public DateTimeObject due_date_first_payment = new DateTimeObject();
        public int count_of_payments = 0;
        public int count_of_30_day_late_payments = 0;
        public int count_of_60_day_late_payments = 0;

        protected virtual string default_columns { get { return ""; } }
        protected virtual string epd_query { get { return ""; } }

        public void addInsertParameters(SqlCommand cmd)
        {
            /*
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
            */
            this.loan.addInsertParameters(cmd);
        }

        public void getAll(List<BaseEPD> epds, List<BaseEPD> removed_epds)
        {
            string columns = this.default_columns;
            string query = "select " + columns + " from " + "( " + epd_query + ")base where 1=1 " + epd_conditions();
            query = query + " order by ms_loan_due_date_next_payment, ms_loan_type";
            List<BaseEPD> all = this.get_loan_list(query);
            foreach (BaseEPD f in all) { epds.Add(f); }
        }

        protected string epd_conditions()
        {
            return " and ( ms_loan_prin_bal > 0 )";
        }

        protected virtual List<BaseEPD> get_loan_list(String query)
        {
            List<BaseEPD> list = new List<BaseEPD>();
            return list;
        }

        public BaseEPD read_loan(DbDataReader reader)
        {
            int pos = 0;
            loan_id = readDBDecimal(reader, pos++);
            due_date_first_payment = readDBDateObject(reader, pos++);
            count_of_payments = readDBInt(reader, pos++);
            count_of_30_day_late_payments = readDBInt(reader, pos++);
            count_of_60_day_late_payments = readDBInt(reader, pos++);

            if (loan_id > 0)
            {
                loan = com.sp.rmmc.common.models.MsLoan.read(reader, pos);
                pos += com.sp.rmmc.common.models.MsLoan.COLUMNS;
                
            }
            return this;
        }
    }
}