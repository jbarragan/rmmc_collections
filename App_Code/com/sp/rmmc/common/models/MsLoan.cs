using System;
using System.Collections.Generic;
using System.Web;


using System.Data.OleDb;
using System.Data.Common;
using System.Data.SqlClient;

using com.sp.rmmc.common.models;
namespace com.sp.rmmc.common.models
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
        public string inv_cd = "";
        public string borrower_phone = "";
        public DateTime event_todays_date = DateTime.Today;
        public String va_occupancy_code = "";
        public String fha_occupancy_code = "";

        public static int COLUMNS = 14;

        public static string MS_LOAN_PRIN_BAL = "(select top 1 prin_bal from ms_loan_balances where ms_loan_balances.loan_id = base.loan_id order by ms_loan_balances.prin_bal) ";
        public static string MS_LOAN_INV_CD = "(select top 1 ms_investor_loan.inv_cd from ms_investor_loan where ms_investor_loan.loan_id = base.loan_id  order by ms_investor_loan.inv_cd )";
        public static string MS_LOAN_DUE_DATE_NEXT_PAYMENT = "(select top 1 ms_loan_info.due_date_next_payment from ms_loan_information ms_loan_info where ms_loan_info.loan_id = base.loan_id order by ms_loan_info.due_date_next_payment )";

        public static string CNV_LOAN_TYPE_LIST = "'CNV', 'RHS'";
        public static string FHA_LOAN_TYPE_LIST = "'FHA', 'FHM'";
        public static string VA_LOAN_TYPE_LIST = "'VA'";

        public string occupancy_code { get { return (this.loan_type == "VA" ? va_occupancy_code_description() : fha_occupancy_code); } }

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
    ms_property_information.property_zip,
    ms_investor_loan.inv_cd,
    (select top 1 ms_borrower_information.borrower_phone from ms_borrower_information where ms_borrower_information.loan_id = ms_loan_information.loan_id  and len(ms_borrower_information.borrower_phone) >= 10 order by ms_borrower_information.borrower_id asc ) as borrower_phone,
    (select top 1 ms_loan_occupancy.va_occ_cd from ms_loan_occupancy where ms_loan_occupancy.loan_id = ms_loan_information.loan_id  order by ms_loan_occupancy.prop_insp_date desc, ms_loan_occupancy.fcl_hud_notice_of_occ_sent desc) as loan_va_occupancy_code,
    (select top 1 ms_occupancy_types.occ_description from ms_loan_occupancy, ms_occupancy_types where ms_occupancy_types.fha_occ_code = ms_loan_occupancy.fha_occ_code and ms_loan_occupancy.loan_id = ms_loan_information.loan_id  order by ms_loan_occupancy.fcl_hud_notice_of_occ_sent desc, ms_loan_occupancy.prop_insp_date desc  ) as loan_fha_occupancy_code
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

        public string getFullAddress()
        {
            return this.property_address + " " +
                this.property_city + ", " + this.property_state + " " +
                this.property_zip;
        }

        public bool isGNMA()
        {
            return this.inv_cd == "800";
        }

        public string va_occupancy_code_description()
        {
            if (this.va_occupancy_code == "0") return "Abandonment";
            else if (this.va_occupancy_code == "1") return "Original Veteran";
            else if (this.va_occupancy_code == "2") return "Transferree";
            else if (this.va_occupancy_code == "4") return "Vacant";
            else if (this.va_occupancy_code == "8") return "Tenant";
            return "";
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
            loan.due_date_next_payment = readDBDateObject(reader, pos++, DateTime.Now);
            loan.loan_type = readDBString(reader, pos++);
            loan.int_rate = readDBDecimal(reader, pos++);
            loan.property_address = readDBString(reader, pos++);
            loan.property_city = readDBString(reader, pos++);
            loan.property_state = readDBString(reader, pos++);
            loan.property_zip = readDBString(reader, pos++);
            loan.inv_cd = readDBString(reader, pos++);
            loan.borrower_phone = readDBString(reader, pos++);
            loan.va_occupancy_code = readDBString(reader, pos++);
            loan.fha_occupancy_code = readDBString(reader, pos++);

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
            cmd.Parameters.AddWithValue("@property_address", this.property_address);
            cmd.Parameters.AddWithValue("@property_city", this.property_city);
            cmd.Parameters.AddWithValue("@property_state", this.property_state);
            cmd.Parameters.AddWithValue("@property_zip", this.property_zip);
            cmd.Parameters.AddWithValue("@loan_inv_cd", this.inv_cd);
            cmd.Parameters.AddWithValue("@borrower_phone", this.borrower_phone);
            cmd.Parameters.AddWithValue("@loan_va_occupancy_code", this.va_occupancy_code);
            cmd.Parameters.AddWithValue("@loan_fha_occupancy_code", this.fha_occupancy_code);
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
            " (select top 1 ms_prop.property_zip from ms_property_information ms_prop where ms_prop.loan_id = " + loan_id_column + " order by ms_prop.property_zip ) as property_zip, " +
            " (select top 1 ms_investor_loan.inv_cd from ms_investor_loan where ms_investor_loan.loan_id = " + loan_id_column + " order by ms_investor_loan.inv_cd ) as loan_inv_cd, " +
            " (select top 1 'h:' + isnull(ms_borrower_information.borrower_phone, '') + '  b:' +   isnull(ms_borrower_information.borrower_bus_phone, '') + ' c:' + isnull(ms_borrower_information.borrower_cell_phone, '') from ms_borrower_information where ms_borrower_information.loan_id = " + loan_id_column + " and (len(ms_borrower_information.borrower_phone) >= 10 or len(ms_borrower_information.borrower_bus_phone) >= 10 or len(ms_borrower_information.borrower_cell_phone) >= 10 ) order by ms_borrower_information.borrower_id asc ) as borrower_phone, " +
            " (select top 1 ms_loan_occupancy.va_occ_cd from ms_loan_occupancy where ms_loan_occupancy.loan_id = " + loan_id_column + " order by ms_loan_occupancy.prop_insp_date desc, ms_loan_occupancy.fcl_hud_notice_of_occ_sent desc) as loan_va_occupancy_code, " +
            " (select top 1 ms_occupancy_types.occ_description from ms_loan_occupancy, ms_occupancy_types where ms_occupancy_types.fha_occ_code = ms_loan_occupancy.fha_occ_code and ms_loan_occupancy.loan_id = " + loan_id_column + " order by ms_loan_occupancy.fcl_hud_notice_of_occ_sent desc, ms_loan_occupancy.prop_insp_date desc  ) as loan_fha_occupancy_code ";
            return s;
        }

    }
}