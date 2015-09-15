using System;
using System.Collections.Generic;
using System.Web;

using System.Data.Sql;
using System.Data.SqlClient;
using System.Data.Common;

using com.sp.rmmc.common.models;
namespace com.sp.rmmc.common.models.events
{
    public class CollectionReasonCodeEvents : BaseEvents
    {
        public String reason_code = "";
        public String last_memo_reason_code = "";
        public DateTimeObject memo_create_dt = new DateTimeObject();
        public DateTimeObject not_031_memo_create_dt = new DateTimeObject();
        public String last_memo_reason_code_including_031 = "";
        public String mortgage_status = "";

        private static String[] class_fields = 
            { 
             "(select top 1 ms_loan_info.loan_id from ms_loan_information ms_loan_info where ms_loan_info.loan_id = loan_id_column order by ms_loan_info.loan_id  ) as reason_code_events_loan_id",
             "(select top 1 ms_credit_information.default_reason_code from ms_credit_information where ms_credit_information.loan_id = loan_id_column order by ms_credit_information.default_reason_code ) as reason_code_events_reason_code",
             "(select top 1 ms_memo_categories.memo_category_desc from ms_loan_memo memo, ms_memo_categories, ms_memo_types where memo.loan_id = loan_id_column and memo.memo_category_id = ms_memo_categories.memo_category_id and memo.memo_type_id = ms_memo_types.memo_type_id and ms_memo_categories.memo_category_desc like '0%' and ms_memo_categories.memo_category_desc not like '031%'  order by memo_create_dt desc ) as reason_code_events_last_memo_reason_code",
             "(select top 1 memo.memo_create_dt from ms_loan_memo memo, ms_memo_categories, ms_memo_types where memo.loan_id = loan_id_column and memo.memo_category_id = ms_memo_categories.memo_category_id and memo.memo_type_id = ms_memo_types.memo_type_id and ms_memo_categories.memo_category_desc like '0%'  and ms_memo_categories.memo_category_desc  like '031%' order by memo_create_dt desc ) as reason_code_events_memo_create_dt",
             "(select top 1 memo.memo_create_dt from ms_loan_memo memo, ms_memo_categories, ms_memo_types where memo.loan_id = loan_id_column and memo.memo_category_id = ms_memo_categories.memo_category_id and memo.memo_type_id = ms_memo_types.memo_type_id and ms_memo_categories.memo_category_desc like '0%'  and ms_memo_categories.memo_category_desc  not like '031%' order by memo_create_dt desc ) as reason_code_events_not_031_memo_create_dt",
             "(select top 1 ms_memo_categories.memo_category_desc from ms_loan_memo memo, ms_memo_categories, ms_memo_types where memo.loan_id = loan_id_column and memo.memo_category_id = ms_memo_categories.memo_category_id and memo.memo_type_id = ms_memo_types.memo_type_id and ms_memo_categories.memo_category_desc like '0%' order by memo_create_dt desc ) as reason_code_events_last_memo_reason_code_including_031",
             "(select top 1 ms_credit_information.mortgage_status from ms_credit_information where ms_credit_information.loan_id = loan_id_column order by ms_credit_information.mortgage_status ) as reason_code_events_mortgage_status "
            };
        
        public CollectionReasonCodeEvents()
        {
            this.fields = class_fields;
        }

        private void load_events()
        {
            if (this.loan_id == 144745M)
                this.loan_id = 144745M;
            if( mortgage_status == "12" ) return;
            //if (not_031_memo_create_dt.date < this.loan.due_date_next_payment.date) last_memo_reason_code = "";
            //if (memo_create_dt.date < this.loan.due_date_next_payment.date) last_memo_reason_code_including_031 = "";

            string after_due_date_reason_code = "";
            if (loan.due_date_next_payment.date <= this.not_031_memo_create_dt.date) after_due_date_reason_code = this.last_memo_reason_code;
            if (this.last_memo_reason_code_including_031.StartsWith(this.reason_code) == true && this.reason_code != "") return;
            if (after_due_date_reason_code.StartsWith(this.reason_code) == false ||
                (this.reason_code == "" && this.last_memo_reason_code_including_031 != ""))
            {
                Event e = new Event();
                e.loan = this.loan;
                e.type = Event.ALERT;
                e.name = "Different Reason Codes";
                e.description = "Current Reason Code (" + this.reason_code + ") is different that last Memo Reason Code (" + this.last_memo_reason_code + ")";
                this.events.Add(e);
            }
        }

        public override void read(DbDataReader reader, int pos, MsLoan loan)
        {   
            this.loan = loan;
            this.loan_id = DbModel.readDBDecimal(reader, pos++);
            this.reason_code = DbModel.readDBString(reader, pos++);
            this.last_memo_reason_code = DbModel.readDBString(reader, pos++);
            this.memo_create_dt = DbModel.readDBDateObject(reader, pos++);
            this.not_031_memo_create_dt = DbModel.readDBDateObject(reader, pos++);
            this.last_memo_reason_code_including_031 = DbModel.readDBString(reader, pos++);
            this.mortgage_status = DbModel.readDBString(reader, pos++);
            this.load_events();
        }

        public override void addInsertParameters(SqlCommand cmd)
        {
            cmd.Parameters.AddWithValue("@reason_code_events_loan_id", this.loan_id);
            cmd.Parameters.AddWithValue("@reason_code_events_reason_code", this.reason_code);
            cmd.Parameters.AddWithValue("@reason_code_events_last_memo_reason_code", this.last_memo_reason_code);
            cmd.Parameters.AddWithValue("@reason_code_events_memo_create_dt", this.memo_create_dt.toDBInsert());
            cmd.Parameters.AddWithValue("@reason_code_events_not_031_memo_create_dt", this.not_031_memo_create_dt.toDBInsert());
            cmd.Parameters.AddWithValue("@reason_code_events_last_memo_reason_code_including_031", this.last_memo_reason_code_including_031);
            cmd.Parameters.AddWithValue("@reason_code_events_mortgage_status", this.mortgage_status);
            
        }
    }
}