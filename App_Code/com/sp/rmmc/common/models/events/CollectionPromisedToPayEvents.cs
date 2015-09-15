using System;
using System.Collections.Generic;
using System.Web;

using System.Data.Sql;
using System.Data.SqlClient;
using System.Data.Common;

using com.sp.rmmc.common.models;
namespace com.sp.rmmc.common.models.events
{
    public class CollectionPromiseToPayEvents : BaseEvents
    {
        public DateTimeObject last_memo_notify_dt = new DateTimeObject();
        public DateTimeObject no_contact_memo_031_create_dt = new DateTimeObject();
        public DateTimeObject last_right_party_contact_create_dt = new DateTimeObject();
        public DateTimeObject last_promise_to_pay_memo_create_dt = new DateTimeObject();
        public DateTimeObject last_reg_paid_dt = new DateTimeObject();
        public DateTimeObject last_current_paid_dt = new DateTimeObject();

        private static String[] class_fields = 
            { 
             "(select top 1 ms_loan_info.loan_id from ms_loan_information ms_loan_info where ms_loan_info.loan_id = loan_id_column order by ms_loan_info.loan_id  ) as collection_promise_to_pay_events_loan_id",
             "(select top 1 memo.memo_notify_dt from ms_loan_memo memo, ms_memo_categories, ms_memo_types where memo.loan_id = loan_id_column and memo.memo_category_id = ms_memo_categories.memo_category_id and memo.memo_type_id = ms_memo_types.memo_type_id and (ms_memo_types.memo_type_desc = 'BC-Promised to Pay')  order by memo_notify_dt desc, memo.memo_create_dt desc ) as collection_promise_to_pay_events_last_memo_notify_dt",
             "(select top 1 memo.memo_create_dt from ms_loan_memo memo, ms_memo_categories, ms_memo_types where memo.loan_id = loan_id_column and memo.memo_category_id = ms_memo_categories.memo_category_id and memo.memo_type_id = ms_memo_types.memo_type_id and ms_memo_categories.memo_category_desc like '031%' order by memo_create_dt desc ) as collection_promise_to_pay_events_no_contact_memo_031_create_dt",
             "(select top 1 memo.memo_create_dt from ms_loan_memo memo, ms_memo_categories, ms_memo_types where memo.loan_id = loan_id_column and memo.memo_category_id = ms_memo_categories.memo_category_id and memo.memo_type_id = ms_memo_types.memo_type_id and (ms_memo_types.memo_type_desc = 'BC-First Right Party Contact')  order by memo_create_dt desc ) as collection_promise_to_pay_events_last_right_party_contact_create_dt",
             "(select top 1 memo.memo_create_dt from ms_loan_memo memo, ms_memo_categories, ms_memo_types where memo.loan_id = loan_id_column and memo.memo_category_id = ms_memo_categories.memo_category_id and memo.memo_type_id = ms_memo_types.memo_type_id and (ms_memo_types.memo_type_desc = 'BC-Promised to Pay')  order by memo_notify_dt desc, memo.memo_create_dt desc ) as collection_promise_to_pay_events_last_promise_to_pay_memo_create_dt",
             "(select top 1 hist.paid_dt from ms_loan_history hist where hist.loan_id = loan_id_column and hist.trans_type_cd = 'REG' order by paid_dt desc) as collection_promise_to_pay_events_last_reg_paid_dt ",
             "(select top 1 hist.paid_dt from ms_loan_history hist where hist.loan_id = loan_id_column and hist.trans_type_cd = 'REG' and (due_dt > paid_dt or ( month(due_dt) = month(paid_dt) and year(due_dt) = year(paid_dt) ) ) order by paid_dt desc) as collection_promise_to_pay_events_last_current_paid_dt "
            };

        public CollectionPromiseToPayEvents()
        {
            this.fields = class_fields;
        }

        private void load_events()
        {
            if (this.last_memo_notify_dt.isNull) return;
            if (this.loan_id == 901801M)
                this.loan_id = 901801M;

            check_last_current();
            
            if (this.last_promise_to_pay_memo_create_dt.isNull == false &&
                this.last_reg_paid_dt.isNull == false &&
                this.last_promise_to_pay_memo_create_dt.date <= this.last_reg_paid_dt.date.AddDays(1)) return;
            if (this.last_memo_notify_dt.isNull == false
                && this.last_memo_notify_dt.date <= this.loan.event_todays_date
                //&& this.last_memo_notify_dt.date <= this.loan.event_todays_date.AddDays(6)
                && (this.last_right_party_contact_create_dt.isNull == true || this.last_right_party_contact_create_dt.date <= this.last_memo_notify_dt.date)
                && this.last_memo_notify_dt.date > this.loan.due_date_next_payment.date
                && (this.no_contact_memo_031_create_dt.isNull == true || this.no_contact_memo_031_create_dt.date <= this.last_memo_notify_dt.date)
                )
            {
                Event e = new Event();
                e.loan = this.loan;
                e.type = Event.ALERT;
                e.name = "Promised To Pay Expired";
                e.description = "Promised To Pay notify date (" + this.last_memo_notify_dt.ToString() + ") expired for this collection.";
                this.events.Add(e);
            }
        }

        private void check_last_current()
        {
            if (this.loan_id == 901801M)
                this.loan_id = 901801M;

            if (this.last_promise_to_pay_memo_create_dt.isNull == false &&
                this.last_current_paid_dt.isNull == false &&
                this.last_promise_to_pay_memo_create_dt.date < this.last_current_paid_dt.date) return;
            if (this.last_memo_notify_dt.isNull == false
                && this.last_memo_notify_dt.date <= this.loan.event_todays_date
                //&& this.last_memo_notify_dt.date <= this.loan.event_todays_date.AddDays(6)
                && (this.last_right_party_contact_create_dt.isNull == true || this.last_right_party_contact_create_dt.date <= this.last_memo_notify_dt.date)
                && this.last_memo_notify_dt.date > this.loan.due_date_next_payment.date
                && (this.no_contact_memo_031_create_dt.isNull == true || this.no_contact_memo_031_create_dt.date <= this.last_memo_notify_dt.date)
                )
            {
                Event e = new Event();
                e.loan = this.loan;
                e.type = Event.ALERT;
                e.name = "Using Current Promised To Pay Expired";
                e.description = "Promised To Pay notify date (" + this.last_memo_notify_dt.ToString() + ") expired for this collection.";
                this.events.Add(e);
            }
        }

        public override void read(DbDataReader reader, int pos, MsLoan loan)
        {   
            this.loan = loan;
            this.loan_id = DbModel.readDBDecimal(reader, pos++);
            this.last_memo_notify_dt = DbModel.readDBDateObject(reader, pos++);
            this.no_contact_memo_031_create_dt = DbModel.readDBDateObject(reader, pos++);
            this.last_right_party_contact_create_dt = DbModel.readDBDateObject(reader, pos++);
            this.last_promise_to_pay_memo_create_dt = DbModel.readDBDateObject(reader, pos++);
            this.last_reg_paid_dt = DbModel.readDBDateObject(reader, pos++);
            this.last_current_paid_dt = DbModel.readDBDateObject(reader, pos++);
            this.load_events();
        }

        public override void addInsertParameters(SqlCommand cmd)
        {
            cmd.Parameters.AddWithValue("@collection_promise_to_pay_events_loan_id", this.loan_id);
            cmd.Parameters.AddWithValue("@collection_promise_to_pay_events_last_memo_notify_dt", this.last_memo_notify_dt.toDBInsert());
            cmd.Parameters.AddWithValue("@collection_promise_to_pay_events_no_contact_memo_031_create_dt", this.no_contact_memo_031_create_dt.toDBInsert());
            cmd.Parameters.AddWithValue("@collection_promise_to_pay_events_last_right_party_contact_create_dt", last_right_party_contact_create_dt.toDBInsert());
            cmd.Parameters.AddWithValue("@collection_promise_to_pay_events_last_promise_to_pay_memo_create_dt", last_promise_to_pay_memo_create_dt.toDBInsert());
            cmd.Parameters.AddWithValue("@collection_promise_to_pay_events_last_reg_paid_dt", last_reg_paid_dt.toDBInsert());
            cmd.Parameters.AddWithValue("@collection_promise_to_pay_events_last_current_paid_dt", last_current_paid_dt.toDBInsert()); 
        }
    }
}