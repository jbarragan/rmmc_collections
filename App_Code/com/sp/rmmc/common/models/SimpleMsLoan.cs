using System;
using System.Collections.Generic;
using System.Web;


using System.Data.OleDb;
using System.Data.Common;
using System.Data.SqlClient;

using com.sp.rmmc.common.models;
namespace com.sp.rmmc.common.models
{
    public class SimpleMsLoan : ForeclosuresCommonModel
    {
        public decimal loan_id = 0M;
        public string loan_name = "";
        public decimal prin_bal = 0M;
        public DateTimeObject due_date_next_payment = new DateTimeObject();
        public string loan_type = "";
        public decimal int_rate = 0M;

        public static int COLUMNS = 6;

        public static string CNV_LOAN_TYPE_LIST = "'CNV', 'RHS'";
        public static string FHA_LOAN_TYPE_LIST = "'FHA', 'FHM'";
        public static string VA_LOAN_TYPE_LIST = "'VA'";

        private static string base_query = @"
select ms_loan_information.loan_id,
    ms_loan_information.loan_name,
    ms_loan_balances.prin_bal,
    ms_loan_information.due_date_next_payment,
    ms_loan_information.loan_type,
    ms_loan_information.int_rate
";
        public SimpleMsLoan()
        {
        }

        public static SimpleMsLoan read(DbDataReader reader, int pos)
        {
            SimpleMsLoan loan = new SimpleMsLoan();
            loan.loan_id = readDBDecimal(reader, pos++);
            loan.loan_name = readDBString(reader, pos++);
            loan.prin_bal = readDBDecimal(reader, pos++);
            loan.due_date_next_payment = readDBDateObject(reader,pos++,DateTime.Now);
            loan.loan_type = readDBString(reader, pos++);
            loan.int_rate = readDBDecimal(reader, pos++);

            return loan;
        }

        public void addInsertParameters(SqlCommand cmd)
        {
            cmd.Parameters.AddWithValue("@ms_loan_id", this.loan_id);
            cmd.Parameters.AddWithValue("@ms_loan_name", this.loan_name);
            cmd.Parameters.AddWithValue("@ms_loan_prin_bal", this.prin_bal);
            cmd.Parameters.AddWithValue("@ms_loan_due_date_next_payment", this.due_date_next_payment.toDBInsert());
            cmd.Parameters.AddWithValue("@ms_loan_type", this.loan_type);
            cmd.Parameters.AddWithValue("@ms_int_rate", this.int_rate);
        }

        public static string columns(string loan_id_column)
        {
            string s = "" +
            " (select top 1 ms_loan_info.loan_id from ms_loan_information ms_loan_info where ms_loan_info.loan_id = " + loan_id_column + " order by ms_loan_info.loan_id  ) as ms_loan_id, " +
            " (select top 1 ms_loan_info.loan_name from ms_loan_information ms_loan_info where ms_loan_info.loan_id = " + loan_id_column + " order by ms_loan_info.loan_name ) as ms_loan_name, " +
            " (select top 1 ms_loan_info.prin_bal from ms_loan_balances ms_loan_info where ms_loan_info.loan_id = " + loan_id_column + " order by ms_loan_info.prin_bal ) as ms_loan_prin_bal, " +
            " (select top 1 ms_loan_info.due_date_next_payment from ms_loan_information ms_loan_info where ms_loan_info.loan_id = " + loan_id_column + " order by ms_loan_info.due_date_next_payment ) as ms_loan_due_date_next_payment, " +
            " (select top 1 ms_loan_info.loan_type from ms_loan_information ms_loan_info where ms_loan_info.loan_id = " + loan_id_column + " order by ms_loan_info.loan_type ) as ms_loan_type, " +
            " (select top 1 ms_loan_info.int_rate from ms_loan_information ms_loan_info where ms_loan_info.loan_id = " + loan_id_column + " order by ms_loan_info.int_rate ) as ms_int_rate ";
            return s;
        }

    }
}
