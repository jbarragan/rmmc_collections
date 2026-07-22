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
    public class BaseForbearance : DbModel, ICSVExport
    {
        public decimal loan_id = 0M;
        public DateTimeObject min_due_date = new DateTimeObject();
        public DateTimeObject borrower_request_date = new DateTimeObject();

        public string forbearance_plan_name = "";
        public string alert_desc = "";
        public string loan_plan_name = "";

        public com.sp.rmmc.common.models.MsLoan loan = new com.sp.rmmc.common.models.MsLoan();
        
        
        protected string default_columns { 
            get{ 
                return "" +
" base.loan_id, " +
" base.min_due_date, " +
" base.borrower_request_date, " +
"(select top 1 mlfp.forbearance_plan_name from ms_loan_forbearance_plan mlfp where mlfp.loan_id = base.loan_id order by mlfp.initial_start_date desc) as forbearance_plan_name, " +
"(select top 1 mla.alert_description from ms_loan_detail_alerts mlda left join ms_loan_alerts mla on mlda.alert_id = mla.alert_id where mlda.loan_id = base.loan_id and mla.alert_description in ('COVID PARTAIL CLAIM', 'COVID PARTIAL CLAIM', 'FORBEARANCE PAYMENT RELEIF', 'FORBEARANCE PAYMENT RELIEF') order by mla.alert_description) as alert_desc, " +
"(select top 1 mli.loan_plan_name from ms_loan_information mli where mli.loan_id = base.loan_id order by mli.loan_plan_name) as loan_plan_name, " +
com.sp.rmmc.common.models.MsLoan.columns("base.loan_id") + " ";
            } 
        }

        protected string base_query
        {
            get
            {
                return
                    "( " +
                    " SELECT base01.loan_id, base01.max_forbearance_plan_id, MIN(mlfip.due_date) AS min_due_date, mlfp.borrower_request_date, mlfp.initial_end_date" +
                    " FROM " +
                    " ( " +
                    " SELECT mlfp.loan_id, MAX(mlfp.forbearance_plan_id) AS max_forbearance_plan_id " +
                    " FROM ms_loan_forbearance_plan mlfp " +
                    " JOIN ms_loan_forbearance_initial_terms mlfip ON mlfip.forbearance_plan_id = mlfp.forbearance_plan_id AND mlfip.loan_id = mlfp.loan_id " +
                    " GROUP BY  mlfp.loan_id " +
                    " ) base01 " +
                    " JOIN ms_loan_forbearance_initial_terms mlfip ON mlfip.forbearance_plan_id = base01.max_forbearance_plan_id AND mlfip.loan_id = base01.loan_id " +
                    " JOIN ms_loan_forbearance_plan mlfp ON mlfp.forbearance_plan_id = base01.max_forbearance_plan_id AND mlfp.loan_id = base01.loan_id " +
                    " GROUP BY  base01.loan_id, base01.max_forbearance_plan_id, mlfp.borrower_request_date, mlfp.initial_end_date " +
                    ")";
            }
        }

        public void getFromToDates(DateTime start_date, DateTime end_date, List<BaseForbearance> accepted, List<BaseForbearance> removed)
        {
            DateTimeObject start = new DateTimeObject(); start.set_date(start_date);
            DateTimeObject end = new DateTimeObject(); end.set_date(end_date);

            string columns = this.default_columns;
            string query = "select " + columns + " from " + "( " + base_query + ")base where 1=1 " +
                " and base.borrower_request_date <= " + end.toSQLAnywhereDBValue() +
                " and base.initial_end_date > " + start.toSQLAnywhereDBValue() +
                modification_conditions();

            query = query + " order by base.loan_id";
            List<BaseForbearance> all = this.get_loan_list(query);
            foreach (BaseForbearance f in all) {
                if (f.borrower_request_date.isNull == false &&
                    f.borrower_request_date.date.Year * 12 + f.borrower_request_date.date.Month <=
                    f.min_due_date.date.Year * 12 + f.min_due_date.date.Month)
                {
                    accepted.Add(f);
                }
                else
                {
                    removed.Add(f);
                }
            }
        }

        public void getVaTexasVetFromToDates(DateTime start_date, DateTime end_date, List<BaseForbearance> accepted, List<BaseForbearance> removed)
        {
            DateTimeObject start = new DateTimeObject(); start.set_date(start_date);
            DateTimeObject end = new DateTimeObject(); end.set_date(end_date);

            string columns = this.default_columns;
            string query = "select " + columns + " from " + "( " + base_query + ")base where 1=1 " +
                " and base.borrower_request_date >= " + start.toSQLAnywhereDBValue() +
                " and base.borrower_request_date <= " + end.toSQLAnywhereDBValue() +
                " and (select top 1 mli.loan_plan_name from ms_loan_information mli where mli.loan_id = base.loan_id order by mli.loan_plan_name) = 'TXVET' " +
                modification_conditions();

            query = query + " order by base.loan_id";
            List<BaseForbearance> all = this.get_loan_list(query);
            foreach (BaseForbearance f in all)
            {
                if (f.borrower_request_date.isNull == false &&
                    f.borrower_request_date.date.Year * 12 + f.borrower_request_date.date.Month <=
                    f.min_due_date.date.Year * 12 + f.min_due_date.date.Month)
                {
                    accepted.Add(f);
                }
                else
                {
                    removed.Add(f);
                }
            }
        }

        public void getAll(List<BaseForbearance> accepted, List<BaseForbearance> removed)
        {
            string columns = this.default_columns;
            string query = "select " + columns + " from " + "( " + base_query + ")base where 1=1 " + modification_conditions();
            query = query + " order by base.loan_id";
            List<BaseForbearance> all = this.get_loan_list(query);
            foreach (BaseForbearance f in all) { accepted.Add(f); }
        }

        protected string modification_conditions()
        {
            return " "; // " and ( ms_loan_prin_bal > 0 )";
        }

        protected List<BaseForbearance> get_loan_list(String query)
        {
            List<BaseForbearance> list = new List<BaseForbearance>();
            SybaseModel model = new SybaseModel();

            DbDataReader reader = model.getReader(query);
            while (reader.Read())
            {
                BaseForbearance b = new BaseForbearance();
                b.read_loan(reader);
                list.Add(b);
            }
            reader.Close();
            model.close_connection();
            return list;
        }

        public BaseForbearance read_loan(DbDataReader reader)
        {
            int pos = 0;
            loan_id = readDBDecimal(reader, pos++);
            min_due_date = readDBDateObject(reader, pos++);
            borrower_request_date = readDBDateObject(reader, pos++);

            forbearance_plan_name = readDBString(reader, pos++);
            alert_desc = readDBString(reader, pos++);
            loan_plan_name = readDBString(reader, pos++);

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
                "\"" + this.loan.loan_type + "\"," +
                "\"" + this.forbearance_plan_name + "\"," +
                "\"" + this.loan.prin_bal.ToString("C") + "\"," +
                "\"" + this.min_due_date.ToShortDateString() + "\"," +
                "\"" + this.borrower_request_date.ToShortDateString() + "\"," +
                "\"" + this.alert_desc + "\"";
        }

    }
}