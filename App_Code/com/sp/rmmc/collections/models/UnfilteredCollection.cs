using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using com.sp.rmmc.common.models;
using System.Data.Common;

/// <summary>
/// Summary description for UnfilteredCollection
/// </summary>
public class UnfilteredCollection: DbModel
{
    public decimal loan_id = 0M;
    public String loan_name = "";
    public DateTimeObject due_date_next_payment = new DateTimeObject();
    public String loan_type = "";
    public DateTimeObject last_memo_promise_to_pay_notify_dt = new DateTimeObject();
    public DateTimeObject last_attempted_call_dt = new DateTimeObject();
    public int current_month_attempted_calls = 0;

    public UnfilteredCollection()
    {

    }

    public List<UnfilteredCollection> getFHAVA17DayDelinquent()
    {
        string query = @"
            with 
            memo_last_promise_to_pay as (
	            select loan_id,
		            max(memo_notify_dt) as memo_notify_dt,
		            max(memo_create_dt) as memo_create_dt
	            from ms_loan_memo
	            join ms_memo_types on ms_loan_memo.memo_type_id = ms_memo_types.memo_type_id
	            join ms_memo_categories on ms_memo_categories.memo_category_id = ms_loan_memo.memo_category_id
	            where ms_memo_types.memo_type_desc = 'BC-Promised to Pay'
	            group by loan_id
            ),
            memo_last_attempted_call as (
	            select loan_id,
		            max(actual_create_dt) as actual_create_dt
	            from ms_loan_memo
	            where memo_subject like '%OUTBOUND%CALL%' or memo_subject like '%O%EMAIL%'
	            group by loan_id
            ),
            memo_current_month_attempted_call as (
	            select loan_id,
		            count(actual_create_dt) as current_month_attempted_calls
	            from ms_loan_memo
	            where (memo_subject like '%OUTBOUND%CALL%' or memo_subject like '%O%EMAIL%')
	            and month(ms_loan_memo.actual_create_dt) = month(getdate()) and year(ms_loan_memo.actual_create_dt) = year(getdate())
	            group by loan_id
            ),
            is_bankruptcy AS (
                select mci.loan_id,
                    CASE
                        WHEN mci.bankruptcy_filed_date IS NOT NULL
                             AND (mbi.discharge_date IS NULL OR mbi.discharge_date < mci.bankruptcy_filed_date)
                             AND (mbi.case_dismissed_date IS NULL OR mbi.case_dismissed_date < mci.bankruptcy_filed_date)
                             AND (mbi.closed_date IS NULL OR mbi.closed_date < mci.bankruptcy_filed_date) THEN 1
                        ELSE 0
                    END AS bankruptcy
                FROM ms_credit_information mci
                LEFT JOIN ms_bankruptcy_info mbi on mbi.loan_id = mci.loan_id
            )
            SELECT mli.loan_id, mli.loan_name, mli.due_date_next_payment, mli.loan_type,
            memo_last_promise_to_pay.memo_notify_dt as last_memo_promise_to_pay_notify_dt,
            memo_last_attempted_call.actual_create_dt as last_attempted_call_dt,
            memo_current_month_attempted_call.current_month_attempted_calls as current_month_attempted_calls
            FROM ms_loan_information mli 
            JOIN ms_credit_information mci ON mci.loan_id = mli.loan_id
            JOIN ms_loan_balances mlb ON mlb.loan_id = mli.loan_id
            LEFT JOIN memo_last_promise_to_pay on memo_last_promise_to_pay.loan_id = mli.loan_id
            LEFT JOIN memo_last_attempted_call on memo_last_attempted_call.loan_id = mli.loan_id
            LEFT JOIN memo_current_month_attempted_call on memo_current_month_attempted_call.loan_id = mli.loan_id
			LEFT JOIN is_bankruptcy ON is_bankruptcy.loan_id = mli.loan_id
            WHERE mli.due_date_next_payment <= dateadd(dd, -16, getDate())
            AND mli.due_date_next_payment > dateadd(mm, -1, getDate()) 
            AND( (mli.loan_type = 'FHA') OR (mli.loan_type = 'VA') )
            AND mlb.prin_bal > 0
			AND bankruptcy = 0;
        ";
        return get_loan_list(query);
    }

    public List<UnfilteredCollection> getCONVNonCMCM17DayDelinquent()
    {
        string query = @"
            with 
            memo_last_promise_to_pay as (
	            select loan_id,
		            max(memo_notify_dt) as memo_notify_dt,
		            max(memo_create_dt) as memo_create_dt
	            from ms_loan_memo
	            join ms_memo_types on ms_loan_memo.memo_type_id = ms_memo_types.memo_type_id
	            join ms_memo_categories on ms_memo_categories.memo_category_id = ms_loan_memo.memo_category_id
	            where ms_memo_types.memo_type_desc = 'BC-Promised to Pay'
	            group by loan_id
            ),
            memo_last_attempted_call as (
	            select loan_id,
		            max(actual_create_dt) as actual_create_dt
	            from ms_loan_memo
	            where memo_subject like '%OUTBOUND%CALL%' or memo_subject like '%O%EMAIL%'
	            group by loan_id
            ),
            memo_current_month_attempted_call as (
	            select loan_id,
		            count(actual_create_dt) as current_month_attempted_calls
	            from ms_loan_memo
	            where (memo_subject like '%OUTBOUND%CALL%' or memo_subject like '%O%EMAIL%')
	            and month(ms_loan_memo.actual_create_dt) = month(getdate()) and year(ms_loan_memo.actual_create_dt) = year(getdate())
	            group by loan_id
            ),
            is_bankruptcy AS (
                select mci.loan_id,
                    CASE
                        WHEN mci.bankruptcy_filed_date IS NOT NULL
                             AND (mbi.discharge_date IS NULL OR mbi.discharge_date < mci.bankruptcy_filed_date)
                             AND (mbi.case_dismissed_date IS NULL OR mbi.case_dismissed_date < mci.bankruptcy_filed_date)
                             AND (mbi.closed_date IS NULL OR mbi.closed_date < mci.bankruptcy_filed_date) THEN 1
                        ELSE 0
                    END AS bankruptcy
                FROM ms_credit_information mci
                LEFT JOIN ms_bankruptcy_info mbi on mbi.loan_id = mci.loan_id
            )
            SELECT mli.loan_id, mli.loan_name, mli.due_date_next_payment, mli.loan_type,
            memo_last_promise_to_pay.memo_notify_dt as last_memo_promise_to_pay_notify_dt,
            memo_last_attempted_call.actual_create_dt as last_attempted_call_dt,
            memo_current_month_attempted_call.current_month_attempted_calls as current_month_attempted_calls
            FROM ms_loan_information mli 
            JOIN ms_credit_information mci ON mci.loan_id = mli.loan_id
            JOIN ms_loan_balances mlb ON mlb.loan_id = mli.loan_id
            LEFT JOIN memo_last_promise_to_pay on memo_last_promise_to_pay.loan_id = mli.loan_id
            LEFT JOIN memo_last_attempted_call on memo_last_attempted_call.loan_id = mli.loan_id
            LEFT JOIN memo_current_month_attempted_call on memo_current_month_attempted_call.loan_id = mli.loan_id
			LEFT JOIN is_bankruptcy ON is_bankruptcy.loan_id = mli.loan_id
            WHERE mli.due_date_next_payment <= dateadd(dd, -16, getDate())
            AND mli.due_date_next_payment > dateadd(mm, -1, getDate()) 
            AND( (mli.loan_type = 'CNV') AND (mli.loan_plan_name not like 'C-MCM%') )
            AND mlb.prin_bal > 0
			AND bankruptcy = 0;
        ";
        return get_loan_list(query);
    }



    private List<UnfilteredCollection> get_loan_list(String query)
    {
        List<UnfilteredCollection> list = new List<UnfilteredCollection>();
        SybaseModel model = new SybaseModel();

        DbDataReader reader = model.getReader(query);
        while (reader.Read())
        {
            UnfilteredCollection l = new UnfilteredCollection();
            l.read_loan(reader);
            list.Add(l);
        }
        reader.Close();
        model.close_connection();
        return list;
    }

    private UnfilteredCollection read_loan(DbDataReader reader)
    {
        int pos = 0;
        this.loan_id = readDBDecimal(reader, pos++);
        this.loan_name = readDBString(reader, pos++);
        this.due_date_next_payment = readDBDateObject(reader, pos++);
        this.loan_type = readDBString(reader, pos++);
        this.last_memo_promise_to_pay_notify_dt = readDBDateObject(reader, pos++);
        this.last_attempted_call_dt = readDBDateObject(reader, pos++);
        this.current_month_attempted_calls = readDBInt(reader, pos++);
        return this;
    }


}