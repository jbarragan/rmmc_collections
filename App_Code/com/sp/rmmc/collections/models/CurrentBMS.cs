using System;
using System.Collections.Generic;
using System.Web;

using System.Data.Common;

using com.sp.rmmc.common.models;
using com.sp.rmmc.common.models.events;

namespace com.sp.rmmc.collections.models
{
    public class CurrentBMS : BaseBMS
    {
        protected override string default_columns
        {
            get
            {
                return "" +
" base.loan_id as loan_id, " +
"(select top 1 ms_credit_information.mortgage_status from ms_credit_information where ms_credit_information.loan_id = base.loan_id order by ms_credit_information.mortgage_status ) as mortgage_status, " +
com.sp.rmmc.common.models.MsLoan.columns("base.loan_id") + " ";
            }
        }

        protected override string bms_query
        {
            get
            {
                return
                    "( " +
                    " select base.loan_id " +
                    " from ms_loan_information base, ms_credit_information " +
                    " where due_date_next_payment > getdate() " +
                    " and ms_credit_information.loan_id = base.loan_id " +
                    " and ms_credit_information.mortgage_status in ('09', '06', '15', '17', '28', '36', '37', '39', '41', '44', 'AA', 'AH', '08', '12') " +
                    ")";
            }
        }

        public CurrentBMS()
        {
            
        }

        protected override List<BaseBMS> get_loan_list(String query)
        {
            List<BaseBMS> list = new List<BaseBMS>();
            SybaseModel model = new SybaseModel();

            DbDataReader reader = model.getReader(query);
            while (reader.Read())
            {
                BaseBMS b = new BaseBMS();
                b.read_loan(reader);
                list.Add(b);
            }
            reader.Close();
            model.close_connection();
            return list;
        }

    }
}
