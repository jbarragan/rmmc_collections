using System;
using System.Collections.Generic;
using System.Web;
using System.Data.SqlClient;

using System.Data.Common;

using com.sp.rmmc.common.models;
using com.sp.rmmc.common.models.events;
using com.sp.rmmc.common.tools;

namespace com.sp.rmmc.collections.models
{
    public class Base11AccountStatus : DbModel, ICSVExport
    {
        public const string DEFAULT = "DEFAULT";
        public decimal loan_id = 0M;
        public string account_status_desc = "";
        public com.sp.rmmc.common.models.MsLoan loan = new com.sp.rmmc.common.models.MsLoan();
        
        
        protected string default_columns { 
            get{ 
                return "" +
" base.loan_id, " +
" base.account_status_desc, " +
com.sp.rmmc.common.models.MsLoan.columns("base.loan_id") + " ";
            } 
        }
        protected string all_query {
            get {
                return
                    "( " +
                    "select distinct mci.loan_id, mas.account_status_desc " +
                    "from ms_credit_information mci " +
                    "join ms_account_status mas on mas.account_status = mci.account_status " +
                    "where mci.account_status = 11 " +
                    ")";
            } 
        }

        public void getAll(List<Base11AccountStatus> accepted, List<Base11AccountStatus> removed)
        {
            string columns = this.default_columns;
            string query = "select " + columns + " from " + "( " + all_query + ")base where 1=1 " + common_conditions();
            query = query + " order by loan_id";
            List<Base11AccountStatus> all = this.get_loan_list(query);
            foreach (Base11AccountStatus f in all)
            { 
                accepted.Add(f); 
            }
        }

        protected string common_conditions()
        {
            return " and ( ms_loan_prin_bal > 0 )";
        }

        protected List<Base11AccountStatus> get_loan_list(String query)
        {
            List<Base11AccountStatus> list = new List<Base11AccountStatus>();
            SybaseModel model = new SybaseModel();

            DbDataReader reader = model.getReader(query);
            while (reader.Read())
            {
                Base11AccountStatus b = new Base11AccountStatus();
                b.read_loan(reader);
                list.Add(b);
            }
            reader.Close();
            model.close_connection();
            return list;
        }

        public Base11AccountStatus read_loan(DbDataReader reader)
        {
            int pos = 0;
            loan_id = readDBDecimal(reader, pos++);
            account_status_desc = readDBString(reader, pos++);

            if (loan_id > 0)
            {
                loan = com.sp.rmmc.common.models.MsLoan.read(reader, pos);
                pos += com.sp.rmmc.common.models.MsLoan.COLUMNS;
                
            }
            return this;
        }

        public String to_csv()
        {
            return
                "\"" + this.loan_id.ToString() + "\"," +
                "\"" + this.loan.loan_name + "\"," +
                "\"" + this.loan.due_date_next_payment.ToString() + "\"," +
                "\"" + this.account_status_desc + "\"";
        }

    }
}
