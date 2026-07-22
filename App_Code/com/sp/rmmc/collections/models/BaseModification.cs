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
    public class BaseModification : DbModel, ICSVExport
    {
        public const string DEFAULT = "DEFAULT";
        public const string MODIFICATION_ANALYSIS = "MODIFICATION ANALYSIS";
        public const string SPREAD_DEFICIENCY_SHORTAGE = "SPREAD DEFICIENCY/SHORTAGE";
        public string type = DEFAULT;
        public decimal loan_id = 0M;
        public string mod_plan_name = "";
        public string invagency = "";
        public string hamp_yn = "";
        public DateTimeObject loan_mod_date = new DateTimeObject();
        public DateTimeObject mod_eff_date = new DateTimeObject();
        public int term = 0;
        public decimal mod_rate = 0M;
        public decimal mod_prin_int = 0M;
        public decimal mod_tax_ins = 0M;
        public DateTimeObject mod_maturity_date = new DateTimeObject();
        public string alert_desc = "";

        public string modification_alert_desc = "";
        public string covid_alert_desc = "";
        public string modification_stop_desc = "";
        public string loan_payment_stop_desc = "";

        public DateTimeObject next_ti_analysis_date = new DateTimeObject();
        public int months_spread_shortage = 0;
        public int months_spread_deficiency = 0;

        public com.sp.rmmc.common.models.MsLoan loan = new com.sp.rmmc.common.models.MsLoan();
        
        
        protected string default_columns { 
            get{ 
                return "" +
" base.loan_id, " +
"(select top 1 mlmti.mod_plan_name from ms_loan_mod_term_items mlmti where mlmti.loan_id = base.loan_id order by mlmti.mod_eff_date desc) as mod_plan_name, " +
"(select top 1 mlmti.hamp_yn from ms_loan_mod_term_items mlmti where mlmti.loan_id = base.loan_id order by mlmti.mod_eff_date desc) as hamp_yn, " +
"(select top 1 mli.loan_mod_date  from ms_loan_information mli where mli.loan_id = base.loan_id order by mli.loan_mod_date desc) as loan_mod_date, " +
"(select top 1 mlmti.mod_eff_date from ms_loan_mod_term_items mlmti where mlmti.loan_id = base.loan_id order by mlmti.mod_eff_date desc) as mod_eff_date, " +
"(select top 1 mlmti.mod_term from ms_loan_mod_term_items mlmti where mlmti.loan_id = base.loan_id order by mlmti.mod_eff_date desc) as mod_term, " +
"(select top 1 mlmti.mod_rate from ms_loan_mod_term_items mlmti where mlmti.loan_id = base.loan_id order by mlmti.mod_eff_date desc) as mod_rate, " +
"(select top 1 mlmti.mod_prin_int from ms_loan_mod_term_items mlmti where mlmti.loan_id = base.loan_id order by mlmti.mod_eff_date desc) as mod_prin_int, " +
"(select top 1 mlmti.mod_tax_ins from ms_loan_mod_term_items mlmti where mlmti.loan_id = base.loan_id order by mlmti.mod_eff_date desc) as mod_tax_ins, " +
"(select top 1 mlmti.mod_maturity_date from ms_loan_mod_term_items mlmti where mlmti.loan_id = base.loan_id order by mlmti.mod_eff_date desc) as mod_maturity_date, " +
"(select top 1 mla.alert_description from ms_loan_information mli left join ms_loan_alerts mla on mli.alert_id = mla.alert_id where mli.loan_id = base.loan_id order by mla.alert_description) as alert_desc, " +
"(select top 1 mla.alert_description from ms_loan_detail_alerts mlda left join ms_loan_alerts mla on mlda.alert_id = mla.alert_id where mlda.loan_id = base.loan_id and mla.alert_description in ('PARTIAL CLAIMS', 'LOAN MODIFICATION', 'FNMA COVID DEFERRAL') order by mla.alert_description) as modification_alert_desc, " +
"(select top 1 mla.alert_description from ms_loan_detail_alerts mlda left join ms_loan_alerts mla on mlda.alert_id = mla.alert_id where mlda.loan_id = base.loan_id and mla.alert_description like '%COVID%') as covid_alert_desc, " +
"(select top 1 msc.stop_description from ms_loan_stop_code mlsc join ms_stop_code msc on msc.stop_code_id = mlsc.stop_code_id where mlsc.loan_id = base.loan_id and mlsc.loan_stop_id = 1 order by msc.stop_description desc) as  modification_stop_desc, " +
"(select top 1 msc.stop_description from ms_loan_stop_code mlsc join ms_stop_code msc on msc.stop_code_id = mlsc.stop_code_id where mlsc.loan_id = base.loan_id and msc.stop_description in ( '50 Agree Order', '53 Bankruptcy Chapter 13', '55 Bankruptcy Chapter 7', '56 Bankruptcy Chapter 13' ) order by msc.stop_description) as  loan_payment_stop_desc, " +
"(select top 1 mti.next_ti_analysis_date from ms_ti_information mti where mti.loan_id = base.loan_id order by mti.next_ti_analysis_date desc ) as next_ti_analysis_date, " +
"(select top 1 mtd.months_spread_shortage from ms_ti_disclosure mtd where mtd.loan_id = base.loan_id order by mtd.months_spread_shortage desc ) as months_spread_shortage, " +
"(select top 1 mtd.months_spread_deficiency from ms_ti_disclosure mtd where mtd.loan_id = base.loan_id order by mtd.months_spread_deficiency desc) as months_spread_deficiency, " +
com.sp.rmmc.common.models.MsLoan.columns("base.loan_id") + " ";
            } 
        }
        protected string all_query {
            get {
                return
                    "( " +
                    " select distinct base.loan_id " +
                    " from ms_loan_mod_term_items base" +
                    " where base.mod_eff_date is not null  " +
                    ")";
            } 
        }

        protected string modification_analysis_query
        {
            get
            {
                return
                    "( " +
                    " select distinct base.loan_id " +
                    " from ms_loan_detail_alerts base" +
                    " join ms_loan_alerts on ms_loan_alerts.alert_id = base.alert_id " +
                    " where ms_loan_alerts.alert_description in ('PARTIAL CLAIMS', 'LOAN MODIFICATION', 'FNMA COVID DEFERRAL')  " +
                    ")";
            }
        }

        public string modification_alerts
        {
            get
            {
                string result = "";
                if (this.loan_id == 139188M)
                    result = result + "";
                if (this.modification_alert_desc.Length > 0) result = this.modification_alert_desc;
                if (this.alert_desc.Length > 0 && this.alert_desc != this.modification_alert_desc)
                {
                    if (result.Length > 0) result = result + ", ";
                    result = result + this.alert_desc;
                }
                if (this.covid_alert_desc.Length > 0 && result.IndexOf(this.covid_alert_desc) < 0 )
                {
                    if (result.Length > 0) result = result + ", ";
                    result = result + this.covid_alert_desc;
                }
                return result;
            }
        }

        public void addInsertParameters(SqlCommand cmd)
        {
            
            this.loan.addInsertParameters(cmd);
        }

        public void getAll(List<BaseModification> accepted, List<BaseModification> removed)
        {
            string columns = this.default_columns;
            string query = "select " + columns + " from " + "( " + all_query + ")base where 1=1 " + modification_conditions();
            query = query + " order by ms_loan_due_date_next_payment asc, mod_eff_date";
            List<BaseModification> all = this.get_loan_list(query);
            foreach (BaseModification f in all) { 
                accepted.Add(f); 
            }
        }

        public void getModificationAnalysisLoans(List<BaseModification> accepted, List<BaseModification> removed)
        {
            string columns = this.default_columns;
            string query = "select " + columns + " from " + "( " + modification_analysis_query + ")base where 1=1 " + analysis_conditions();
            query = query + " order by next_ti_analysis_date desc, loan_id";
            List<BaseModification> all = this.get_loan_list(query);
            foreach (BaseModification f in all)
            {
                f.type = MODIFICATION_ANALYSIS;
                accepted.Add(f);
            }
        }

        public void getSpreadDeficiencyShortage(List<BaseModification> accepted, List<BaseModification> removed)
        {
            string columns = this.default_columns;
            string filter_query =
                    "( " +
                    " select distinct base.loan_id " +
                    " from ms_ti_disclosure base " +
                    " where (base.months_spread_shortage is not null and base.months_spread_shortage > 0) " +
                    "    or (base.months_spread_deficiency is not null and base.months_spread_deficiency > 0) " +
                    ")";

            string query = "select " + columns + " from " + "( " + filter_query + ")base where 1=1 " + spread_conditions();
            query = query + " order by loan_id";
            List<BaseModification> all = this.get_loan_list(query);
            foreach (BaseModification f in all)
            {
                f.type = SPREAD_DEFICIENCY_SHORTAGE;
                accepted.Add(f);
            }
        }

        protected string modification_conditions()
        {
            return " and ( " + MsLoan.MS_LOAN_PRIN_BAL + " > 0 )";
        }

        protected string spread_conditions()
        {
            return " and ( " + MsLoan.MS_LOAN_PRIN_BAL + " > 0 )";
        }

        protected string analysis_conditions()
        {
            string next_ti_analysis_date_query = "(select top 1 mti.next_ti_analysis_date from ms_ti_information mti where mti.loan_id = base.loan_id order by mti.next_ti_analysis_date desc )";
            return " and ( " + MsLoan.MS_LOAN_PRIN_BAL + " > 0 " +
                " and dateadd(Month, -2, " + next_ti_analysis_date_query + " ) < getDate() )";
        }

        protected List<BaseModification> get_loan_list(String query)
        {
            List<BaseModification> list = new List<BaseModification>();
            SybaseModel model = new SybaseModel();

            DbDataReader reader = model.getReader(query);
            while (reader.Read())
            {
                BaseModification b = new BaseModification();
                b.read_loan(reader);
                list.Add(b);
            }
            reader.Close();
            model.close_connection();
            return list;
        }

        public BaseModification read_loan(DbDataReader reader)
        {
            int pos = 0;
            loan_id = readDBDecimal(reader, pos++);
            mod_plan_name = readDBString(reader, pos++);
            hamp_yn = readDBString(reader, pos++);
            loan_mod_date = readDBDateObject(reader, pos++);
            mod_eff_date = readDBDateObject(reader, pos++);
            term = readDBInt(reader, pos++);
            mod_rate = readDBDecimal(reader, pos++);
            mod_prin_int = readDBDecimal(reader, pos++);
            mod_tax_ins = readDBDecimal(reader, pos++);
            mod_maturity_date = readDBDateObject(reader, pos++);
            alert_desc = readDBString(reader, pos++);
            modification_alert_desc = readDBString(reader, pos++);
            covid_alert_desc = readDBString(reader, pos++);
            modification_stop_desc = readDBString(reader, pos++);
            loan_payment_stop_desc = readDBString(reader, pos++);
            this.next_ti_analysis_date = readDBDateObject(reader, pos++);
            this.months_spread_shortage = readDBInt(reader, pos++);
            this.months_spread_deficiency = readDBInt(reader, pos++);

            if (loan_id > 0)
            {
                loan = com.sp.rmmc.common.models.MsLoan.read(reader, pos);
                pos += com.sp.rmmc.common.models.MsLoan.COLUMNS;
                
            }
            return this;
        }

        public String to_csv()
        {
            if (this.type == MODIFICATION_ANALYSIS) return modification_analysis_to_csv();
            if (this.type == SPREAD_DEFICIENCY_SHORTAGE) return spread_deficiency_shortage_to_csv();
            return
                "\"" + this.loan_id.ToString() + "\"," +
                "\"" + this.invagency + "\"," +
                "\"" + this.loan.loan_name + "\"," +
                "\"" + this.loan.loan_type + "\"," +
                "\"" + this.mod_plan_name + "\"," +
                "\"" + this.hamp_yn + "\"," +
                "\"" + this.loan_mod_date.ToShortDateString() + "\"," +
                "\"" + this.mod_eff_date.ToShortDateString() + "\"," +
                "\"" + this.loan.due_date_next_payment.ToString() + "\"," +
                "\"" + ((this.term == 0) ? "" : this.term.ToString()) + "\"," +
                "\"" + this.loan.prin_bal.ToString("C") + "\"," +
                "\"" + ((this.mod_rate == 0M) ? "" : this.mod_rate.ToString("#.000")) + "\"," +
                "\"" + this.mod_prin_int.ToString("C") + "\"," +
                "\"" + this.mod_tax_ins.ToString("C") + "\"," +
                "\"" + this.mod_maturity_date.ToShortDateString() + "\"," +
                "\"" + this.alert_desc + "\"";
        }

        private string modification_analysis_to_csv()
        {
            return
                "\"" + this.loan_id.ToString() + "\"," +
                "\"" + this.loan.loan_name + "\"," +
                "\"" + this.loan_mod_date.ToShortDateString() + "\"," +
                "\"" + this.alert_desc + "\"," +
                "\"" + this.modification_stop_desc + "\"," +
                "\"" + this.loan_payment_stop_desc + "\"," +
                "\"" + this.next_ti_analysis_date.ToShortDateString() + "\"," +
                "\"" + this.months_spread_shortage + "\"," +
                "\"" + this.months_spread_deficiency + "\"";
        }

        private string spread_deficiency_shortage_to_csv()
        {
            return
                "\"" + this.loan_id.ToString() + "\"," +
                "\"" + this.loan.loan_name + "\"," +
                "\"" + this.modification_alerts + "\"," +
                "\"" + this.months_spread_shortage + "\"," +
                "\"" + this.months_spread_deficiency + "\"," +
                "\"" + this.next_ti_analysis_date.ToShortDateString() + "\"";
        }

    }
}