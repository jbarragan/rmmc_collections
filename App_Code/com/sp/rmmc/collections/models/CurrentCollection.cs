using System;
using System.Collections.Generic;
using System.Web;

using System.Data.Common;

using com.sp.rmmc.common.models;
using com.sp.rmmc.common.models.events;

namespace com.sp.rmmc.collections.models
{
    public class CurrentCollection : BaseCollection
    {
        protected override string default_columns
        {
            get
            {
                return @"
with 
history_reg as (
	select loan_id,
		max(paid_dt) as paid_dt,
		max(due_dt) as due_dt,
		max(prin_amt) as prin_amt,
		max(int_amt) as int_amt,
		max(curtailment_amount) as curtailment_amount
	from ms_loan_history
	where trans_type_cd = 'REG'
	and paid_dt = (select max(paid_dt) from ms_loan_history m where m.loan_id = ms_loan_history.loan_id and m.trans_type_cd = 'REG' group by loan_id)
	and due_dt = (select max(due_dt) from ms_loan_history m where m.loan_id = ms_loan_history.loan_id and m.trans_type_cd = 'REG' group by loan_id)
	group by loan_id
),
history_previous_month_reg as (
	select loan_id,
	sum(prin_amt) as prin_amt,
	sum(int_amt) as int_amt
	from ms_loan_history
	where trans_type_cd = 'REG' and year(paid_dt)*12 + month(paid_dt) = year(getdate())*12 + month(getdate()) -1
	group by loan_id
),
history_current_month_reg as (
	select loan_id,
	max(paid_dt) as paid_dt
	from ms_loan_history
	where trans_type_cd = 'REG' and year(paid_dt)*12 + month(paid_dt) = year(getdate())*12 + month(getdate())
	group by loan_id
),
history_after_cur as (
	select loan_id,
	sum(prin_amt + curtailment_amount) as prin_cur_amt
	from ms_loan_history
	where trans_type_cd = 'CUR'
	and paid_dt >= (select max(paid_dt) from ms_loan_history m where m.loan_id = ms_loan_history.loan_id and m.trans_type_cd = 'REG' group by loan_id)
	group by loan_id
),
history_after_reg_regr as (
	select loan_id,
		sum(pmt_amt) as pmt_amt
	from ms_loan_history
	where (trans_type_cd = 'REG' or trans_type_cd = 'REGR')
	and paid_dt >= (select max(paid_dt) from ms_loan_history m where m.loan_id = ms_loan_history.loan_id and m.trans_type_cd = 'REG' group by loan_id)
	group by loan_id
),
history_pif as (
	select loan_id,
		abs(sum(prin_amt)) as prin_amt
	from ms_loan_history
	where trans_type_cd = 'PIF'
	group by loan_id
),
history_prin_bal as (
	select loan_id,
		abs(convert(numeric, uad_audit_data_bf_chg)) as prin_bl
	from ms_user_audit_history_data
	where uad_audit_field_name = 'prin_bal'
	and isnumeric(uad_audit_data_bf_chg) = 1 
	and ms_user_audit_history_data.uad_audit_actual_chg_dt = (select max(m.uad_audit_actual_chg_dt) from ms_user_audit_history_data m where m.loan_id = ms_user_audit_history_data.loan_id and m.uad_audit_field_name = 'prin_bal' and isnumeric(m.uad_audit_data_bf_chg) = 1  group by m.loan_id)
),
history_last_current_paid as (
	select loan_id,
	max(paid_dt) as paid_dt
	from ms_loan_history
	where trans_type_cd = 'REG' and (due_dt > paid_dt or (month(due_dt) = month(paid_dt) and year(due_dt) = year(paid_dt)))
	group by loan_id
),
memo_fc_referral_date as (
	select loan_id,
		max(memo_create_dt) as memo_create_dt
	from ms_loan_memo
	where memo_type_id = 139
	group by loan_id
),
memo_fc_payoff_date as (
	select loan_id,
		max(memo_create_dt) as memo_create_dt
	from ms_loan_memo
	where memo_type_id = 148
	group by loan_id
),
lm_stop_code as (
	select loan_id,
		max(stop_code_id) as stop_code_id
	from ms_loan_stop_code
	where stop_code_id = 153
	and ms_loan_stop_code.last_updated = (select max(m.last_updated) from ms_loan_stop_code m where m.loan_id = ms_loan_stop_code.loan_id and m.stop_code_id = 153)
	group by loan_id
),
bk_ch13_stop_code as (
	select loan_id,
		max(stop_code_id) as stop_code_id
	from ms_loan_stop_code
	where stop_code_id in (1,11,14)
	and ms_loan_stop_code.last_updated = (select max(m.last_updated) from ms_loan_stop_code m where m.loan_id = ms_loan_stop_code.loan_id and m.stop_code_id in (1,11,14))
	group by loan_id
),
stop_code_description as (
	select loan_id,
		max(ms_stop_code.stop_description) as stop_description
	from ms_loan_stop_code
	join ms_stop_code on ms_stop_code.stop_code_id = ms_loan_stop_code.stop_code_id
	where ms_stop_code.payment_stop_code_flag = 'Y'
	group by loan_id
),
stop_code_description_90_day as (
	select loan_id,
		max(ms_stop_code.stop_description) as stop_description,
        max(ms_loan_stop_code.stop_code_id) as stop_code_id
	from ms_loan_stop_code
	join ms_stop_code on ms_stop_code.stop_code_id = ms_loan_stop_code.stop_code_id
	where ms_stop_code.stop_description like '90-DAY%'
	group by loan_id
),
stop_code_description_67_Prop_Insp_Fee as (
	select loan_id,
		max(ms_stop_code.stop_description) as stop_description
	from ms_loan_stop_code
	join ms_stop_code on ms_stop_code.stop_code_id = ms_loan_stop_code.stop_code_id
	where ms_stop_code.stop_description like '67 Prop. Insp. Fee%'
	group by loan_id
),
stop_code_description_78_Partial_Claim as (
	select loan_id,
		max(ms_stop_code.stop_description) as stop_description
	from ms_loan_stop_code
	join ms_stop_code on ms_stop_code.stop_code_id = ms_loan_stop_code.stop_code_id
	where ms_stop_code.stop_description like '78 Partial Claim%'
	group by loan_id
),
stop_code_description_78_HUD_Claim_loss as (
	select loan_id,
		max(ms_stop_code.stop_description) as stop_description
	from ms_loan_stop_code
	join ms_stop_code on ms_stop_code.stop_code_id = ms_loan_stop_code.stop_code_id
	where ms_stop_code.stop_description like '78-HUD-Claim loss%'
	group by loan_id
),
stop_code_description_FNMA_HAMP_MODIFICATION as (
	select loan_id,
		max(ms_stop_code.stop_description) as stop_description
	from ms_loan_stop_code
	join ms_stop_code on ms_stop_code.stop_code_id = ms_loan_stop_code.stop_code_id
	where ms_stop_code.stop_description like '78-HUD-Claim loss%'
	group by loan_id
),
memo_fc_third_party_sale as (
	select loan_id,
		max(memo_create_dt) as memo_create_dt
	from ms_loan_memo
	join ms_memo_types on ms_loan_memo.memo_type_id = ms_memo_types.memo_type_id
	where ms_memo_types.memo_type_desc like 'FC-3rd%'
	group by loan_id
),
memo_last_property_inspection as (
	select loan_id,
		max(memo_notify_dt) as memo_notify_dt
	from ms_loan_memo
	join ms_memo_types on ms_loan_memo.memo_type_id = ms_memo_types.memo_type_id
	join ms_memo_categories on ms_memo_categories.memo_category_id = ms_loan_memo.memo_category_id
	where ms_memo_types.memo_type_desc = 'PI-Initial Exterior Inspection'
	or ms_memo_categories.memo_category_desc = 'Property Inspection'
	group by loan_id
),
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
memo_last_first_right_party_contact as (
	select loan_id,
		max(memo_create_dt) as memo_create_dt
	from ms_loan_memo
	join ms_memo_types on ms_loan_memo.memo_type_id = ms_memo_types.memo_type_id
	join ms_memo_categories on ms_memo_categories.memo_category_id = ms_loan_memo.memo_category_id
	where ms_memo_types.memo_type_desc = 'BC-First Right Party Contact'
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
memo_last_bc as (
	select loan_id,
		max(memo_create_dt) as memo_create_dt
	from ms_loan_memo
	where ms_loan_memo.memo_subject like 'BC%'
	group by loan_id
),
memo_last_bc_data as (
	select ms_loan_memo.loan_id,
		max(ms_memo_types.memo_type_desc) as memo_type_desc,
		max(memo_category_desc) as memo_category_desc,
		max(ms_loan_memo.memo_create_dt) as memo_create_dt
	from ms_loan_memo
	join ms_memo_types on ms_loan_memo.memo_type_id = ms_memo_types.memo_type_id
	join ms_memo_categories on ms_memo_categories.memo_category_id = ms_loan_memo.memo_category_id
	join memo_last_bc on memo_last_bc.loan_id = ms_loan_memo.loan_id and memo_last_bc.memo_create_dt = ms_loan_memo.memo_create_dt
	where ms_loan_memo.memo_subject like 'BC%'
	group by ms_loan_memo.loan_id
),
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
pmi_company as (
	select loan_id, 
		max(business_associates.ba_name) as ba_name
	from ms_loan_ti_disbursements
	join business_associates on business_associates.ba_id = ms_loan_ti_disbursements.ba_id
	join ba_types on ba_types.ba_type_id = ms_loan_ti_disbursements.ba_type_id
	where ba_type = 'PMI'
	and ti_disb_id = (
		select max(ti_disb_id)
		from ms_loan_ti_disbursements m
		join business_associates ba on ba.ba_id = m.ba_id
        join ba_types bt on bt.ba_type_id = m.ba_type_id
		where m.loan_id = ms_loan_ti_disbursements.loan_id
        and bt.ba_type = 'PMI'
		group by m.loan_id
	)
	group by loan_id
),
lm_programs as (
	select loan_id,
		max(program_end_dt) as program_end_dt
	from ms_loss_mitigation_programs
	group by loan_id
)
select base.loan_id,
ms_loan_misc_fields.misc_dt_value as demand_letter_due_date,
mslmf_foreclosure_officers.misc_field_value as foreclosure_officers,
ms_fcl_info.fcl_type,
history_reg.paid_dt as last_reg_paid_dt,
history_reg.due_dt as last_reg_due_dt,
history_reg.prin_amt as last_reg_prin_amt,
history_previous_month_reg.prin_amt as previous_month_prin_amt,
history_previous_month_reg.int_amt as previous_month_int_amt,
history_reg.curtailment_amount as last_reg_cur_amt,
history_after_cur.prin_cur_amt as last_after_reg_cur_amt, 
history_reg.int_amt as last_reg_int_amt,
history_after_reg_regr.pmt_amt as last_payments_balance,
ms_fcl_item_amts_1.fcl_item_date as papers_to_attorney_date,
mfia1_posting.fcl_item_date as posting_date,
ms_credit_information.chapter_filed,
ms_credit_information.bankruptcy_filed_date,
ms_bankruptcy_info.discharge_date as bankruptcy_discharge_date,
ms_bankruptcy_info.case_dismissed_date as bankruptcy_case_dismissed_date,
ms_bankruptcy_info.closed_date as bankruptcy_closed_date,
ms_bankruptcy_info.release_of_stay_date as bankruptcy_release_of_stay_date,
mfia1_sale_date.fcl_item_date as sale_date,
ms_loan_balances.ytd_prin_paid_amt as ytd_prin_paid_amt,
history_pif.prin_amt as principal_amt_from_history,
history_prin_bal.prin_bl as audit_prin_bal,
memo_fc_referral_date.memo_create_dt as fc_foreclosure_referral_date,
memo_fc_payoff_date.memo_create_dt as fc_payoff_date,
mfia1_appraisal_ordered.fcl_item_date as appraisal_ordered_date,
mfia1_posting.fcl_item_recvd_expd as foreclosure_date,
lm_stop_code.stop_code_id as lm_stop_code_id,
bk_ch13_stop_code.stop_code_id as bankrupcty_ch_13_stop_code_id,
memo_fc_third_party_sale.memo_create_dt as memo_fc_third_party_sale_date,
mfia2_extension_ordered.fcl_item_ord_req as extension_ordered_date,
ms_fcl_info_deed_in_lieu.fcl_deed_in_lieu as fcl_deed_in_lieu,
(null)  as lm_date,
mfia1_possession.fcl_item_date as possession_date,
mfia1_deed_from_borrower.fcl_item_date as deed_from_borrower_received_date,
ms_credit_information.mortgage_status,
ms_investor_loan.inv_loan_nbr,
ms_mortgage_status.mortgage_status_desc,
memo_last_property_inspection.memo_notify_dt as last_property_inspection_date,
ms_loan_information.loan_plan_name,
stop_code_description_90_day.stop_code_id,
stop_code_description.stop_description,
stop_code_description_90_day.stop_description as stop_code_description_90_day,
stop_code_description_67_Prop_Insp_Fee.stop_description as stop_code_description_67_Prop_Insp_Fee,
stop_code_description_78_Partial_Claim.stop_description as stop_code_description_78_Partial_Claim,
stop_code_description_78_HUD_Claim_loss.stop_description as stop_code_description_78_HUD_Claim_loss,
stop_code_description_FNMA_HAMP_MODIFICATION.stop_description as stop_code_description_FNMA_HAMP_MODIFICATION,
ms_loan_alerts.alert_type,
ms_credit_information.mortgage_status_date,
ms_credit_information.default_reason_code as ci_reason_code,
ms_default_reason.default_reason_desc,
ms_loan_information.agency_case,
memo_last_bc_data.memo_type_desc as last_memo_type_desc,
memo_last_bc_data.memo_category_desc as last_memo_category_desc,
memo_last_bc_data.memo_create_dt as last_memo_created_dt,
memo_last_promise_to_pay.memo_notify_dt as last_memo_promise_to_pay_notify_dt,
history_current_month_reg.paid_dt as last_current_paid_dt,
ms_investor_loan.inv_bank_cd,
memo_last_attempted_call.actual_create_dt as last_attempted_call_dt,
pmi_company.ba_name as pmi_company_name,
memo_current_month_attempted_call.current_month_attempted_calls as current_month_attempted_calls,
isnull(ms_loan_information.prin_int_payment, 0)  + isnull(ms_loan_information.tax_ins_payment, 0) + isnull(ms_loan_information.misc_ins_payment, 0) as monthly_payment_amount,
lm_programs.program_end_dt as program_end_dt,
" + com.sp.rmmc.common.models.MsLoan.columns("base.loan_id") + " ";
            }
        }

        public static string default_joins = @"
left join ms_loan_misc_fields on ms_loan_misc_fields.loan_id = base.loan_id and ms_loan_misc_fields.misc_field_id = 10020
left join ms_loan_misc_fields mslmf_foreclosure_officers on mslmf_foreclosure_officers.loan_id = base.loan_id and mslmf_foreclosure_officers.misc_field_id = 10023
left join ms_fcl_info on ms_fcl_info.loan_id = base.loan_id
left join history_reg on history_reg.loan_id = base.loan_id 
left join history_previous_month_reg on history_previous_month_reg.loan_id = base.loan_id
left join history_current_month_reg on history_current_month_reg.loan_id = base.loan_id
left join history_after_cur on history_after_cur.loan_id = base.loan_id 
left join history_after_reg_regr on history_after_reg_regr.loan_id = base.loan_id
left join history_pif on history_pif.loan_id = base.loan_id
left join history_last_current_paid on history_last_current_paid.loan_id = base.loan_id
left join ms_fcl_item_amts_1 on ms_fcl_item_amts_1.loan_id = base.loan_id and ms_fcl_item_amts_1.item_id = 1000
left join ms_fcl_item_amts_1 mfia1_posting on mfia1_posting.loan_id = base.loan_id and mfia1_posting.item_id = 1200
left join ms_fcl_item_amts_1 mfia1_sale_date on mfia1_sale_date.loan_id = base.loan_id and mfia1_sale_date.item_id = 1600
left join ms_fcl_item_amts_1 mfia1_appraisal_ordered on mfia1_appraisal_ordered.loan_id = base.loan_id and mfia1_appraisal_ordered.item_id = 1600
left join ms_fcl_item_amts_1 mfia1_possession on mfia1_possession.loan_id = base.loan_id and mfia1_possession.item_id = 1900
left join ms_fcl_item_amts_1 mfia1_deed_from_borrower on mfia1_deed_from_borrower.loan_id = base.loan_id and mfia1_deed_from_borrower.item_id = 3900
left join ms_fcl_item_amts_2 mfia2_extension_ordered on mfia2_extension_ordered.loan_id = base.loan_id and mfia2_extension_ordered.item_id = 10800
left join ms_credit_information on ms_credit_information.loan_id = base.loan_id
left join ms_mortgage_status on ms_mortgage_status.mortgage_status_code = ms_credit_information.mortgage_status 
left join ms_default_reason on ms_default_reason.default_reason_code = ms_credit_information.default_reason_code
left join ms_bankruptcy_info on ms_bankruptcy_info.loan_id = base.loan_id
left join ms_loan_balances on ms_loan_balances.loan_id = base.loan_id
left join history_prin_bal on history_prin_bal.loan_id = base.loan_id
left join memo_fc_referral_date on memo_fc_referral_date.loan_id = base.loan_id
left join memo_fc_payoff_date on memo_fc_payoff_date.loan_id = base.loan_id
left join lm_stop_code on lm_stop_code.loan_id = base.loan_id
left join bk_ch13_stop_code on bk_ch13_stop_code.loan_id = base.loan_id
left join stop_code_description on stop_code_description.loan_id = base.loan_id
left join stop_code_description_90_day on stop_code_description_90_day.loan_id = base.loan_id
left join stop_code_description_67_Prop_Insp_Fee on stop_code_description_67_Prop_Insp_Fee.loan_id = base.loan_id
left join stop_code_description_78_Partial_Claim on stop_code_description_78_Partial_Claim.loan_id = base.loan_id
left join stop_code_description_78_HUD_Claim_loss on stop_code_description_78_HUD_Claim_loss.loan_id = base.loan_id
left join stop_code_description_FNMA_HAMP_MODIFICATION on stop_code_description_FNMA_HAMP_MODIFICATION.loan_id = base.loan_id
left join memo_fc_third_party_sale on memo_fc_third_party_sale.loan_id = base.loan_id
left join memo_last_property_inspection on memo_last_property_inspection.loan_id = base.loan_id
left join memo_last_bc_data on memo_last_bc_data.loan_id = base.loan_id
left join memo_last_promise_to_pay on memo_last_promise_to_pay.loan_id = base.loan_id
left join memo_last_attempted_call on memo_last_attempted_call.loan_id = base.loan_id
left join memo_current_month_attempted_call on memo_current_month_attempted_call.loan_id = base.loan_id
left join memo_last_not_031_reason_code_data on memo_last_not_031_reason_code_data.loan_id = base.loan_id
left join memo_last_031_reason_code_data on memo_last_031_reason_code_data.loan_id = base.loan_id
left join memo_last_0_reason_code_data on memo_last_0_reason_code_data.loan_id = base.loan_id
left join memo_last_first_right_party_contact on memo_last_first_right_party_contact.loan_id = base.loan_id
left join ms_fcl_info ms_fcl_info_deed_in_lieu on ms_fcl_info_deed_in_lieu.fcl_deed_in_lieu = 'Y' and ms_fcl_info_deed_in_lieu.loan_id = base.loan_id
left join ms_investor_loan on ms_investor_loan.loan_id = base.loan_id
left join ms_loan_information on ms_loan_information.loan_id = base.loan_id
left join ms_loan_alerts on ms_loan_alerts.alert_id = ms_loan_information.alert_id
left join pmi_company on pmi_company.loan_id = base.loan_id
left join lm_programs on lm_programs.loan_id = base.loan_Id
";

        protected override string all_collections_query
        {
            get
            {
                return
                    "( " +
                    " select base.loan_id " +
                    " from ms_loan_information as base " +
                    " where base.due_date_next_payment < getDate() " +
                    ") ";
            }
        }

        protected override string month_current_with_restrict_autopay_draft_query
        {
            get
            {
                return
                    "( " +
                    " select base.loan_id " +
                    " from ms_loan_information as base " +
                    " where base.restrict_autopay_draft = 'Y' " +
                    " and dateadd(Month, 1, base.due_date_next_payment) > getDate()  " +
                    ") ";
            }
        }

        protected override string all_bankruptcies_query
        {
            get
            {
                return
                    "( " +
                    " select base.loan_id " +
                    " from ms_loan_information as base " +
                    " join ms_credit_information mci on mci.loan_id = base.loan_id " +
                    " left join ms_bankruptcy_info mbi on mbi.loan_id = base.loan_id " +
                    " where base.due_date_next_payment < getDate() " +
                    " and (year(base.due_date_next_payment) < year(dateadd(Day, 1, getDate())) or month(base.due_date_next_payment) < month(dateadd(Day, 1, getDate()))) " +
                    " and mci.bankruptcy_filed_date is not null " +
                    " and (mbi.discharge_date is null or mbi.discharge_date < mci.bankruptcy_filed_date) " +
                    " and (mbi.case_dismissed_date is null or mbi.case_dismissed_date < mci.bankruptcy_filed_date) " +
                    " and (mbi.closed_date is null or mbi.closed_date < mci.bankruptcy_filed_date) " +
                    ") ";
            }
        }

        protected override string all_bankruptcies_txvet_nation_star_query
        {
            get
            {
                return
                    "( " +
                    " select base.loan_id " +
                    " from ms_loan_information as base " +
                    " join ms_credit_information mci on mci.loan_id = base.loan_id " +
                    " left join ms_bankruptcy_info mbi on mbi.loan_id = base.loan_id " +
                    " where base.due_date_next_payment < getDate() " +
                    " and (year(base.due_date_next_payment) < year(dateadd(Day, 1, getDate())) or month(base.due_date_next_payment) < month(dateadd(Day, 1, getDate()))) " +
                    " and mci.bankruptcy_filed_date is not null " +
                    " and (mbi.discharge_date is null or mbi.discharge_date < mci.bankruptcy_filed_date) " +
                    " and (mbi.case_dismissed_date is null or mbi.case_dismissed_date < mci.bankruptcy_filed_date) " +
                    " and (mbi.closed_date is null or mbi.closed_date < mci.bankruptcy_filed_date) " +
                    " and (base.loan_plan_name = 'TXVET') " +
                    ") ";
            }
        }

        protected override string all_demands_query
        {
            get
            {
                return
                    "( " +
                    " select base.loan_id " +
                    " from ms_loan_information as base " +
                    " join ms_loan_stop_code mlsc on mlsc.loan_id = base.loan_id " +
                    " join ms_stop_code msc on mlsc.stop_code_id = msc.stop_code_id " +
                    " where base.due_date_next_payment < getDate() " +
                    " and (year(base.due_date_next_payment) < year(dateadd(Day, 1, getDate())) or month(base.due_date_next_payment) < month(dateadd(Day, 1, getDate()))) " +
                    " and msc.stop_description like '15 Demanded' " +
                    ") ";
            }
        }

        protected override string all_no_contact_query
        {
            get
            {
                return
                    "( " +
                    " select distinct mlm.loan_id " +
                    " from ms_loan_memo mlm " +
                    " where year(mlm.memo_create_dt) = year(getDate()) and month(mlm.memo_create_dt) = month(getDate()) and mlm.memo_subject like 'BC%' " +
                    ") ";
            }
        }

        protected override string all_foreclosures_query
        {
            get
            {
                return
                    "( " +
                    " select distinct base.loan_id " +
                    " from ms_loan_information as base " +
                    " join ms_loan_stop_code mlsc on mlsc.loan_id = base.loan_id " +
                    " join ms_stop_code msc on mlsc.stop_code_id = msc.stop_code_id " +
                    " join ms_credit_information mci on mci.loan_id = base.loan_id " +
                    " where base.due_date_next_payment < getDate() " +
                    " and (year(base.due_date_next_payment) < year(dateadd(Day, 1, getDate())) or month(base.due_date_next_payment) < month(dateadd(Day, 1, getDate()))) " +
                    " and (msc.stop_description like '3-%'  or (mci.mortgage_status = '68' and base.loan_type = 'FHA')) " +
                    ") ";
            }
        }

        protected override string collections_query
        {
            get
            {
                return
                    "( " +
                    " select base.loan_id, ROW_NUMBER() OVER (ORDER BY base.loan_id) AS RowNum " +
                    " from ms_loan_information as base " +
                    " join ms_credit_information on ms_credit_information.loan_id = base.loan_id " +
                    " join ms_loan_balances on ms_loan_balances.loan_id = base.loan_id " +
                    " where (base.due_date_next_payment < getDate()) " +
                    " and ms_loan_balances.prin_bal > 0 " +
                    " and (base.loan_type != 'FHA' or ms_credit_information.mortgage_status in ('42', '11', '12', '', '67', '98') or ms_credit_information.mortgage_status is null ) " +
                    ") ";
            }
        }

        protected override string promised_to_pay_query
        {
            get
            {
                return
                    "( " +
                    " select base.loan_id, ROW_NUMBER() OVER (ORDER BY base.loan_id) AS RowNum " +
                    " from ms_loan_information as base " +
                    " join ms_credit_information on ms_credit_information.loan_id = base.loan_id " +
                    " join ms_loan_balances on ms_loan_balances.loan_id = base.loan_id " +
                    " join memo_last_promise_to_pay on memo_last_promise_to_pay.loan_id = base.loan_id " +
                    " join history_last_current_paid on history_last_current_paid.loan_id = base.loan_id " +
                    " where (base.due_date_next_payment < getDate()) " +
                    " and memo_last_promise_to_pay.memo_create_dt is not null " +
                    " and memo_last_promise_to_pay.memo_create_dt > history_last_current_paid.paid_dt " +
                    " and memo_last_promise_to_pay.memo_notify_dt <= getdate() " +
                    " and ms_loan_balances.prin_bal > 0 " +
                    " and (base.loan_type != 'FHA' or ms_credit_information.mortgage_status in ('42', '11', '12', '', '67', '98') or ms_credit_information.mortgage_status is null ) " +
                    ") ";
            }
        }

        protected override string fha_collections_in_loss_mitigation_query
        {
            get
            {
                return
                    "( " +
                    " select base.loan_id " +
                    " from ms_loan_information as base, ms_credit_information " +
                    " where ms_credit_information.loan_id = base.loan_id " +
                    //" where (base.due_date_next_payment < getDate()) " +
                    //" and ms_credit_information.loan_id = base.loan_id " +
                    " and (base.loan_type = 'FHA' and ms_credit_information.mortgage_status in ('05', '06','08','09','10','11','12','15','28','36','37','39','41','44','78','98','AA','AH','AQ', 'AO', 'AP', 'AS') ) " +
                    ") ";
            }
        }

        protected override string covid_collections_in_loss_mitigation_query
        {
            get
            {
                return
                    "( " +
                    @"SELECT distinct base.loan_id 
                      FROM ms_loan_information base
                      left join ms_loan_detail_alerts mlda on base.loan_id = mlda.loan_id
                      left join ms_loan_alerts mla on mla.alert_id = mlda.alert_id
                      left join ms_loss_mitigation_programs mlmp on base.loan_id = mlmp.loan_id 
                      where ( mlmp.program_code = 43 ) or mla.alert_type = 'COVID'" +
                    ") ";
                //where (mlmp.workout_type = 1 or mlmp.program_code = 43 or mlmp.program_end_dt is not null) or mla.alert_type = 'COVID'" +
            }
        }

        protected override string all_collections_in_loss_mitigation_query
        {
            get
            {
                return
                    "( " +
                    " select base.loan_id " +
                    " from ms_loan_information as base, ms_credit_information " +
                    " where ms_credit_information.loan_id = base.loan_id " +
                    //" where (base.due_date_next_payment < getDate()) " +
                    //" and ms_credit_information.loan_id = base.loan_id " +
                    " and (ms_credit_information.mortgage_status in ('05','06','08','09','10','11','12','15','28','36','37','39','41','44', '78','98','AA','AH','AQ', 'AO', 'AP', 'AS') ) " +
                    ") ";
            }
        }

        protected override string fha_excollections_huddel42_query
        {
            get
            {
                return
                    "( " +
                    " select base.loan_id " +
                    " from ms_loan_information as base, ms_credit_information " +
                    " where (base.due_date_next_payment > getDate()) " +
                    " and ms_credit_information.loan_id = base.loan_id " +
                    " and (base.loan_type = 'FHA' and ms_credit_information.mortgage_status = '42' ) " +
                    ") ";
            }
        }

        protected override string bankrupcty_report_query
        {
            get
            {
                return
                    "( " +
                    " select distinct base.loan_id " +
                    " from ms_loan_information as base " +
                    " join ms_credit_information on ms_credit_information.loan_id = base.loan_id " +
                    " join ms_loan_stop_code mlsc on mlsc.loan_id = base.loan_id " +
                    " where (base.due_date_next_payment > getDate()) " +
                    " and (ms_credit_information.mortgage_status = '67' " +
                    " or mlsc.stop_code_id in (1,11,14) ) " +
                    ") ";
            }
        }


        protected override string fha_collections_query
        {
            get
            {
                return
                    "( " +
                    " select base.loan_id " +
                    " from ms_loan_information as base, ms_credit_information " +
                    " where (base.due_date_next_payment < getDate()) " +
                    " and ms_credit_information.loan_id = base.loan_id " +
                    " and ( base.loan_type = 'FHA' ) " +
                    ") ";
            }
        }

        protected override string collections_4_month_delinquent_query
        {
            get
            {
                return
                    "( " +
                    " select base.loan_id " +
                    " from ms_loan_information as base, ms_credit_information " +
                    " where ( base.due_date_next_payment <= dateadd(mm, -3, getDate()) )" +
                    " and ms_credit_information.loan_id = base.loan_id " +
                    " and (base.loan_type != 'FHA' or ms_credit_information.mortgage_status in ('42', '11', '12', '', '67', '98', 'AP', 'A0', 'AQ') or ms_credit_information.mortgage_status is null ) " +
                    ") ";
            }
        }

        private string collections_2_month_delinquent_query
        {
            get
            {
                return
                    " select base.loan_id " +
                    " from ms_loan_information as base, ms_credit_information " +
                    " where ( base.due_date_next_payment <= dateadd(mm, -1, getDate()) )" +
                    " and   ( base.due_date_next_payment >= dateadd(mm, -2, getDate()) )" +
                    " and ms_credit_information.loan_id = base.loan_id ";
            }
        }

        protected override string collections_2_month_fha_delinquent_query
        {
            get
            {
                return
                    "( " +
                    collections_2_month_delinquent_query +
                    " and ( dateadd(mm, 1, base.due_date_next_payment) <= dateadd(dd, -3, getDate()) )" +
                    " and (base.loan_type = 'FHA') " +
                    ") ";
            }
        }

        protected override string collections_2_month_conv_delinquent_query
        {
            get
            {
                return
                    "( " +
                    collections_2_month_delinquent_query +
                    " and (base.loan_type = 'CNV') " +
                    ") ";
            }
        }

        protected override string collections_2_month_va_delinquent_query
        {
            get
            {
                return
                    "( " +
                    collections_2_month_delinquent_query +
                    " and (base.loan_type = 'VA') " +
                    ") ";
            }
        }

        protected override string collections_cnv_1_month_or_more_delinquent_query
        {
            get
            {
                return
                    " select base.loan_id " +
                    " from ms_loan_information as base, ms_investor_loan " +
                    " where ( base.due_date_next_payment <= dateadd(mm, -1, getDate()) )" +
                    " and base.loan_id = ms_investor_loan.loan_id " +
                    " and (base.loan_type = 'CNV' or (base.loan_type = 'FHA' and ms_investor_loan.inv_cd = '901') ) and ms_investor_loan.inv_cd = '901' and ms_investor_loan.inv_bank_cd = '02' " +
                    "";
            }
        }

        protected override string collections_fnma_hamp_stop_query
        {
            get
            {
                return
                    " select base.loan_id " +
                    " from ms_loan_information as base " +
                    " where 1=1 " +
                    "";
            }
        }

        private string collections_3_month_delinquent_query
        {
            get
            {
                return
                    " select base.loan_id " +
                    " from ms_loan_information as base, ms_credit_information " +
                    " where ( base.due_date_next_payment <= dateadd(mm, -2, getDate()) )" +
                    " and base.due_date_next_payment >= dateadd(mm, -3, getDate()) " +
                    " and ms_credit_information.loan_id = base.loan_id " +
                    //" and (ms_credit_information.mortgage_status in ('42', '11', '12', '', '67', '98', 'AP', 'A0', 'AQ') or ms_credit_information.mortgage_status is null ) " +
                    "";
            }
        }

        protected override string collections_3_month_or_more_delinquent_query
        {
            get
            {
                return
                    " select base.loan_id " +
                    " from ms_loan_information as base, ms_credit_information, ms_loan_balances, ms_investor_loan " +
                    " where ( base.due_date_next_payment <= dateadd(mm, -2, getDate()) )" +
                    " and ms_credit_information.loan_id = base.loan_id " +
                    " and ms_loan_balances.loan_id = base.loan_id " +
                    " and ms_investor_loan.loan_id = base.loan_id " +
                    " and ms_investor_loan.inv_cd = '901' " +
                    " and ms_loan_balances.prin_bal > 0 " +
                    "";
            }
        }

        protected override string collections_3_month_or_more_delinquent_query_gnma
        {
            get
            {
                return
                    " select base.loan_id " +
                    " from ms_loan_information as base, ms_credit_information, ms_loan_balances, ms_investor_loan " +
                    " where ( base.due_date_next_payment <= dateadd(mm, -2, getDate()) )" +
                    " and ms_credit_information.loan_id = base.loan_id " +
                    " and ms_loan_balances.loan_id = base.loan_id " +
                    " and ms_investor_loan.loan_id = base.loan_id " +
                    " and ms_investor_loan.inv_cd = '800' " +
                    " and ms_loan_balances.prin_bal > 0 " +
                    "";
            }
        }

        protected override string collections_3_month_fha_hud_delinquent_query
        {
            get
            {
                return
                    "( " +
                    collections_3_month_delinquent_query +
                    " and   ( dateadd(mm, 1, base.due_date_next_payment) <= dateadd(dd, -2, getDate()) )" +
                    " and (base.loan_type = 'FHA') " +
                    ") ";
            }
        }

        protected override string collections_3_month_conv_delinquent_query
        {
            get
            {
                return
                    "( " +
                    collections_3_month_delinquent_query +
                    " and ( dateadd(mm, 1, base.due_date_next_payment) <= dateadd(dd, -2, getDate()) )" +
                    " and (base.loan_type = 'CNV') " +
                    ") ";
            }
        }

        protected override string collections_3_month_va_delinquent_query
        {
            get
            {
                return
                    "( " +
                    collections_3_month_delinquent_query +
                    " and ( dateadd(mm, 1, base.due_date_next_payment) <= dateadd(dd, -2, getDate()) )" +
                    " and   ( base.due_date_next_payment >= dateadd(mm, -3, getDate()) )" +
                    " and (base.loan_type = 'VA') " +
                    ") ";
            }
        }

        private string collections_17_days_delinquent_query_with_rownum
        {
            get
            {
                return
                    " select base.loan_id, ROW_NUMBER() OVER (ORDER BY base.loan_id) AS RowNum " +
                    " from ms_loan_information as base, ms_credit_information " +
                    " where ( base.due_date_next_payment <= dateadd(dd, -16, getDate()) )" +
                    " and ( base.due_date_next_payment > dateadd(mm, -1, getDate()) )" +
                    " and ms_credit_information.loan_id = base.loan_id ";
            }
        }

        private string collections_17_days_delinquent_query
        {
            get
            {
                return
                    " select base.loan_id " +
                    " from ms_loan_information as base, ms_credit_information " +
                    " where ( base.due_date_next_payment <= dateadd(dd, -16, getDate()) )" +
                    " and ( base.due_date_next_payment > dateadd(mm, -1, getDate()) )" +
                    " and ms_credit_information.loan_id = base.loan_id ";
            }
        }

        protected override string collections_17_days_fha_va_delinquent_query
        {
            get
            {
                return
                    "( " +
                    collections_17_days_delinquent_query_with_rownum +
                    //" and (base.loan_type = 'FHA' or  base.loan_type = 'VA') " +
                    " and ( (base.loan_type = 'FHA' and base.loan_id > 1) or (base.loan_type = 'VA' and base.loan_id > 1) ) " +
                    ") ";
            }
        }

        protected override string collections_17_days_fha_delinquent_query
        {
            get
            {
                return
                    "( " +
                    collections_17_days_delinquent_query_with_rownum  +
                    //" and (base.loan_type = 'FHA' or  base.loan_type = 'VA') " +
                    " and (base.loan_type = 'FHA' and base.loan_id > 1) " +
                    ") ";
            }
        }

        protected override string collections_17_days_fha_delinquent_query_part1
        {
            get
            {
                return
                    "( " +
                    collections_17_days_delinquent_query +
                    //" and (base.loan_type = 'FHA' or  base.loan_type = 'VA') " +
                    " and (base.loan_type = 'FHA' and base.loan_id <= 210852) " +
                    ") ";
            }
        }

        protected override string collections_17_days_fha_delinquent_query_part2
        {
            get
            {
                return
                    "( " +
                    collections_17_days_delinquent_query +
                    //" and (base.loan_type = 'FHA' or  base.loan_type = 'VA') " +
                    " and (base.loan_type = 'FHA' and base.loan_id > 210852) " +
                    ") ";
            }
        }

        protected override string collections_17_days_va_delinquent_query
        {
            get
            {
                return
                    "( " +
                    collections_17_days_delinquent_query +
                    //" and (base.loan_type = 'FHA' or  base.loan_type = 'VA') " +
                    " and (base.loan_type = 'VA' and base.loan_id > 1) " +
                    ") ";
            }
        }

        protected override string collections_17_days_cnv_non_c_mcm_delinquent_query
        {
            get
            {
                return
                    "( " +
                    collections_17_days_delinquent_query_with_rownum +
                    " and base.loan_type = 'CNV' " +
                    " and base.loan_plan_name not like 'C-MCM%' " +
                    ") ";
            }
        }

        protected override string collections_17_days_cnv_c_mcm_delinquent_query
        {
            get
            {
                return
                    "( " +
                    collections_17_days_delinquent_query +
                    " and base.loan_type = 'CNV' " +
                    " and base.loan_plan_name like 'C-MCM%' " +
                    ") ";
            }
        }

        protected override string all_types_query
        {
            get
            {
                return
                    collections_query +
                    " UNION " +
                    collections_4_month_delinquent_query +
                    " ";
            }
        }


        public CurrentCollection()
        {
            
        }

        protected override List<BaseCollection> get_loan_list(String query)
        {
            List<BaseCollection> list = new List<BaseCollection>();
            MsSqlModelSybase model = new MsSqlModelSybase();

            int max_tries = 1;
            int current_try = 0;
            DbDataReader reader = null;
            do 
            {
                reader = model.getReader(query);
                current_try++;
            } while( reader == null && current_try < max_tries);


            while (reader.Read())
            {
                BaseCollection f = new BaseCollection();
                f.type = this.type;
                f.read_loan(reader);
                list.Add(f);
            }
            reader.Close();
            model.close_connection();
            return list;
        }

        protected override string getEventsColumns()
        {
            string s = "";
            foreach (BaseEvents be in this.getEventsTypes())
            {
                s += ", \n" + be.query("base.loan_id");
            }
            return s;
        }

        protected override void get_lm_date(List<BaseCollection> accepted)
        {
            foreach (BaseCollection f in accepted)
            {
                if (f.lm_stop_code_id > 0)
                {
                    f.lm_date.set_date(DateTime.Today);
                }
            }
            /*
            List<LMReception> all_lm_receptions = LMReception.getLMReceptions(accepted);
            Dictionary<decimal, LMReception> loan_lm_receptions = new Dictionary<decimal, LMReception>();
            foreach (LMReception lm_reception in all_lm_receptions)
                loan_lm_receptions.Add(lm_reception.loan_id, lm_reception);
            foreach (BaseCollection f in accepted)
            {
                if (loan_lm_receptions.ContainsKey(f.loan_id))
                {
                    f.lm_date = loan_lm_receptions[f.loan_id].date;
                }
            }
            */ 
        }
    }
}
