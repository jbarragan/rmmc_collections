using System;
using System.Collections.Generic;
using System.Web;


using System.Data.OleDb;
using System.Data.Common;

using com.sp.rmmc.common.models;
namespace com.sp.rmmc.foreclosures.models
{
    public class MsLoan : ForeclosuresCommonModel
    {
        public decimal loan_id = 0M;
        public string loan_name = "";
        public decimal prin_bal = 0M;
        public DateTimeObject due_date_next_payment = new DateTimeObject();
        public string loan_type = "";
        public decimal int_rate = 0M;
        public string property_address = "";
        public string property_city = "";
        public string property_state = "";
        public string property_zip = "";

        public static int COLUMNS = 10;

        private static string base_query = @"
select ms_loan_information.loan_id,
    ms_loan_information.loan_name,
    ms_loan_balances.prin_bal,
    ms_loan_information.due_date_next_payment,
    ms_loan_information.loan_type,
    ms_loan_information.int_rate,
    ms_property_information.property_address,
    ms_property_information.property_city,
    ms_property_information.property_state,
    ms_property_information.property_zip
from ms_loan_information, ms_loan_balances, ms_property_information
where ms_loan_information.loan_id = ms_loan_balances.loan_id
and   ms_loan_information.loan_id = ms_property_information.loan_id
";
        public MsLoan()
        {
            //
            // TODO: Add constructor logic here
            //
        }

        public static MsLoan get_ms_loan(decimal loan_id)
        {
            MsLoan loan = new MsLoan();
            string query = base_query + " and ms_loan_information.loan_id = " + loan_id.ToString();
            OleDbDataReader reader = getReader(query);
            if (reader.Read())
            {
                loan = read(reader, 0);
            }
            close_connection();
            return loan;
        }

        public static MsLoan read(DbDataReader reader, int pos)
        {
            MsLoan loan = new MsLoan();
            loan.loan_id = readDBDecimal(reader, pos++);
            loan.loan_name = readDBString(reader, pos++);
            loan.prin_bal = readDBDecimal(reader, pos++);
            loan.due_date_next_payment = readDBDateObject(reader,pos++,DateTime.Now);
            loan.loan_type = readDBString(reader, pos++);
            loan.int_rate = readDBDecimal(reader, pos++);
            loan.property_address = readDBString(reader, pos++);
            loan.property_city = readDBString(reader, pos++);
            loan.property_state = readDBString(reader, pos++);
            loan.property_zip = readDBString(reader, pos++);

            return loan;
        }

        public static string columns(string loan_id_column)
        {
            string s = "" +
            " (select top 1 ms_loan_info.loan_id from ms_loan_information ms_loan_info where ms_loan_info.loan_id = " + loan_id_column + " order by ms_loan_info.loan_id  ) as ms_loan_id, " +
            " (select top 1 ms_loan_info.loan_name from ms_loan_information ms_loan_info where ms_loan_info.loan_id = " + loan_id_column + " order by ms_loan_info.loan_name ) as ms_loan_name, " +
            " (select top 1 ms_loan_info.prin_bal from ms_loan_balances ms_loan_info where ms_loan_info.loan_id = " + loan_id_column + " order by ms_loan_info.prin_bal ) as ms_loan_prin_bal, " +
            " (select top 1 ms_loan_info.due_date_next_payment from ms_loan_information ms_loan_info where ms_loan_info.loan_id = " + loan_id_column + " order by ms_loan_info.due_date_next_payment ) as ms_loan_due_date_next_payment, " +
            " (select top 1 ms_loan_info.loan_type from ms_loan_information ms_loan_info where ms_loan_info.loan_id = " + loan_id_column + " order by ms_loan_info.loan_type ) as ms_loan_type, " +
            " (select top 1 ms_loan_info.int_rate from ms_loan_information ms_loan_info where ms_loan_info.loan_id = " + loan_id_column + " order by ms_loan_info.int_rate ) as ms_int_rate, " +
            " (select top 1 ms_prop.property_address from ms_property_information ms_prop where ms_prop.loan_id = " + loan_id_column + " order by ms_prop.property_address ) as property_address, " +
            " (select top 1 ms_prop.property_city from ms_property_information ms_prop where ms_prop.loan_id = " + loan_id_column + " order by ms_prop.property_city ) as property_city, " +
            " (select top 1 ms_prop.property_state from ms_property_information ms_prop where ms_prop.loan_id = " + loan_id_column + " order by ms_prop.property_state ) as property_state, " +
            " (select top 1 ms_prop.property_zip from ms_property_information ms_prop where ms_prop.loan_id = " + loan_id_column + " order by ms_prop.property_zip ) as property_zip ";
            return s;
        }
    }
}