using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using com.sp.rmmc.common.models;
using System.Data.Common;

/// <summary>
/// Summary description for UnfilteredCollection
/// </summary>
public class DifferentReasonCodes : DbModel
{
    public decimal loan_id = 0M;
    public String loan_name = "";
    public DateTimeObject due_date_next_payment = new DateTimeObject();
    public String loan_type = "";

    public decimal prin_bal = 0M;
    public decimal int_rate = 0M;
    public String occupancy_code = "";
    public String reason_code = "";
    public String last_memo_reason_code = "";
    public DateTimeObject memo_create_dt = new DateTimeObject();

    public DifferentReasonCodes()
    {

    }

    public List<DifferentReasonCodes> getLoanWithDifferentReasonCodes()
    {
        string query = @"
            with 
            memo_last_not_031_reason_code as (
	            select loan_id,
		            max(memo_create_dt) as memo_create_dt
	            from ms_loan_memo
	            join ms_memo_categories on ms_memo_categories.memo_category_id = ms_loan_memo.memo_category_id
	            where ms_memo_categories.memo_category_desc like '0%'  and ms_memo_categories.memo_category_desc  not like '031%'
	            group by loan_id
            ),
            memo_last_not_031_reason_code_data as (
	            select ms_loan_memo.loan_id,
		            max(memo_category_desc) as memo_category_desc,
		            max(ms_loan_memo.memo_create_dt) as memo_create_dt
	            from ms_loan_memo
	            join ms_memo_types on ms_loan_memo.memo_type_id = ms_memo_types.memo_type_id
	            join ms_memo_categories on ms_memo_categories.memo_category_id = ms_loan_memo.memo_category_id
	            join memo_last_not_031_reason_code on memo_last_not_031_reason_code.loan_id = ms_loan_memo.loan_id and memo_last_not_031_reason_code.memo_create_dt = ms_loan_memo.memo_create_dt
	            where ms_memo_categories.memo_category_desc like '0%'  and ms_memo_categories.memo_category_desc  not like '031%'
	            group by ms_loan_memo.loan_id
            ),
            memo_last_031_reason_code_data as (
	            select ms_loan_memo.loan_id,
		            max(ms_loan_memo.memo_create_dt) as memo_create_dt
	            from ms_loan_memo
	            join ms_memo_categories on ms_memo_categories.memo_category_id = ms_loan_memo.memo_category_id
	            where ms_memo_categories.memo_category_desc  like '031%'
	            group by ms_loan_memo.loan_id
            ),
            memo_last_not_0_reason_code as (
	            select loan_id,
		            max(memo_create_dt) as memo_create_dt
	            from ms_loan_memo
	            join ms_memo_categories on ms_memo_categories.memo_category_id = ms_loan_memo.memo_category_id
	            where ms_memo_categories.memo_category_desc like '0%'
	            group by loan_id
            ),
            memo_last_0_reason_code_data as (
	            select ms_loan_memo.loan_id,
		            max(memo_category_desc) as memo_category_desc
	            from ms_loan_memo
	            join ms_memo_categories on ms_memo_categories.memo_category_id = ms_loan_memo.memo_category_id
	            join memo_last_not_0_reason_code on memo_last_not_0_reason_code.loan_id = ms_loan_memo.loan_id and memo_last_not_0_reason_code.memo_create_dt = ms_loan_memo.memo_create_dt
	            where ms_memo_categories.memo_category_desc  like '0%'
	            group by ms_loan_memo.loan_id
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
            select base.loan_id,
	               base.loan_type,
	               base.loan_name,
	               base.due_date_next_payment,
	               ms_loan_balances.prin_bal,
	               base.int_rate,
	               case
			            when ms_loan_occupancy.va_occ_cd = '0' then 'Abandonment'
			            when ms_loan_occupancy.va_occ_cd = '1' then 'Original Veteran'
			            when ms_loan_occupancy.va_occ_cd = '2' then 'Transferree'
			            when ms_loan_occupancy.va_occ_cd = '4' then 'Vacant'
			            when ms_loan_occupancy.va_occ_cd = '8' then 'Tenant'
			            else ms_occupancy_types.occ_description
	               end as occupancy_code,
                   ms_credit_information.default_reason_code as reason_code,
                   memo_last_not_031_reason_code_data.memo_category_desc as last_memo_reason_code,
                   memo_last_031_reason_code_data.memo_create_dt as memo_create_dt
            from ms_loan_information as base
            join ms_credit_information on ms_credit_information.loan_id = base.loan_id
            join ms_loan_balances on ms_loan_balances.loan_id = base.loan_id  
            left join memo_last_not_031_reason_code_data on memo_last_not_031_reason_code_data.loan_id = base.loan_id
            left join memo_last_031_reason_code_data  on memo_last_031_reason_code_data.loan_id = base.loan_id
            left join memo_last_0_reason_code_data on memo_last_0_reason_code_data.loan_id = base.loan_id
            left join ms_loan_occupancy on ms_loan_occupancy.loan_id = base.loan_id
            left join ms_occupancy_types on ms_occupancy_types.fha_occ_code = ms_loan_occupancy.fha_occ_code 
            LEFT JOIN is_bankruptcy ON is_bankruptcy.loan_id = base.loan_id
            where 
	            base.due_date_next_payment < getDate()
	            and ms_loan_balances.prin_bal > 0  
	            and (base.loan_type != 'FHA' or ms_credit_information.mortgage_status in ('42', '11', '12', '', '67', '98') or ms_credit_information.mortgage_status is null)
                and (ms_credit_information.default_reason_code is not null)
	            and (
		            ms_credit_information.default_reason_code is null
		            or charindex(ms_credit_information.default_reason_code, memo_last_0_reason_code_data.memo_category_desc) = 0 
	            )
	            and (
		            1=0 
					-- or base.due_date_next_payment >= memo_last_not_031_reason_code_data.memo_create_dt 
		            or ms_credit_information.default_reason_code is null
		            or charindex(ms_credit_information.default_reason_code, memo_last_not_031_reason_code_data.memo_category_desc) = 0 
	            )
	            AND ms_loan_balances.prin_bal > 0
	            AND bankruptcy = 0
        ";
        return get_loan_list(query);
    }


    private List<DifferentReasonCodes> get_loan_list(String query)
    {
        List<DifferentReasonCodes> list = new List<DifferentReasonCodes>();
        SybaseModel model = new SybaseModel();

        DbDataReader reader = model.getReader(query);
        while (reader.Read())
        {
            DifferentReasonCodes l = new DifferentReasonCodes();
            l.read_loan(reader);
            list.Add(l);
        }
        reader.Close();
        model.close_connection();
        return list;
    }

    private DifferentReasonCodes read_loan(DbDataReader reader)
    {
        int pos = 0;
        this.loan_id = readDBDecimal(reader, pos++);
        this.loan_type = readDBString(reader, pos++);
        this.loan_name = readDBString(reader, pos++);
        this.due_date_next_payment = readDBDateObject(reader, pos++);
        this.prin_bal = readDBDecimal(reader, pos++);
        this.int_rate = readDBDecimal(reader, pos++);
        this.occupancy_code = readDBString(reader, pos++);
        this.reason_code = readDBString(reader, pos++);
        this.last_memo_reason_code = readDBString(reader, pos++);
        this.memo_create_dt = readDBDateObject(reader, pos++);
        return this;
    }


}