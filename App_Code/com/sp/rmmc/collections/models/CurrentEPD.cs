using System;
using System.Collections.Generic;
using System.Web;

using System.Data.Common;

using com.sp.rmmc.common.models;
using com.sp.rmmc.common.models.events;

namespace com.sp.rmmc.collections.models
{
    public class CurrentEPD : BaseEPD
    {
        protected override string default_columns
        {
            get
            {
                return "" +
" base.loan_id as loan_id, " +
"(select top 1 ms_loan_info.due_date_first_payment from ms_loan_information ms_loan_info where ms_loan_info.loan_id = base.loan_id order by ms_loan_info.due_date_first_payment ) as due_date_first_payment, " +
"(select count(*) from ms_loan_history hist where hist.loan_id = base.loan_id and hist.trans_type_cd = 'REG') as count_of_payments, " +
"(select count(*) from ms_loan_history hist where hist.loan_id = base.loan_id and hist.trans_type_cd = 'REG' and hist.due_dt < dateadd(dd, -30, hist.paid_dt) ) as count_of_30_day_late_payments, " +
"(select count(*) from ms_loan_history hist where hist.loan_id = base.loan_id and hist.trans_type_cd = 'REG' and hist.due_dt < dateadd(dd, -60, hist.paid_dt) ) as count_of_60_day_late_payments, " +
com.sp.rmmc.common.models.MsLoan.columns("base.loan_id") + " ";
            }
        }

        protected override string epd_query
        {
            get
            {
                return
                    "( " +
                    " select base.loan_id " +
                    " from ms_loan_information base " +
                    " where due_date_first_payment > dateadd(mm, -9, getdate() ) " +
                    " and " +
                    " ( " +
                    "   due_date_next_payment < dateadd(dd, -60, getdate() ) " +
                    "   or (select count(*) from ms_loan_history hist where hist.loan_id = base.loan_id and hist.trans_type_cd = 'REG' and hist.due_dt < dateadd(dd, -30, hist.paid_dt) ) >= 2 " +
                    "   or (select count(*) from ms_loan_history hist where hist.loan_id = base.loan_id and hist.trans_type_cd = 'REG' and hist.due_dt < dateadd(dd, -60, hist.paid_dt) ) >= 1 " +
                    " ) " +
                    ") ";
            }
        }

        public CurrentEPD()
        {
            
        }

        protected override List<BaseEPD> get_loan_list(String query)
        {
            List<BaseEPD> list = new List<BaseEPD>();
            SybaseModel model = new SybaseModel();

            DbDataReader reader = model.getReader(query);
            while (reader.Read())
            {
                BaseEPD b = new BaseEPD();
                b.read_loan(reader);
                list.Add(b);
            }
            reader.Close();
            model.close_connection();
            return list;
        }

    }
}
