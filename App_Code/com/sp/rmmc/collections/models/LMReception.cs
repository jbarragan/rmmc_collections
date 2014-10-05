using System;
using System.Collections.Generic;
using System.Web;
using com.sp.rmmc.common.models;
using System.Data.Common;


namespace com.sp.rmmc.collections.models
{
    public class LMReception : ForeclosuresCommonModel
    {

        public decimal loan_id = 0M;
        public DateTimeObject date = new DateTimeObject();

        private static string base_query = "" +
"select " +
"loan_id, " +
"received " +
"from receptions ";

        private static string mssqlConnectionString =
            System.Configuration.ConfigurationManager.ConnectionStrings["loss_mitigation"].ConnectionString;

        public LMReception()
        {
        }

        public static List<LMReception> get_delinquent_lm_receptions(List<Delinquent> demand_letters)
        {
            if (demand_letters.Count > 0)
            {
                string query = base_query + " where 1 = 1 and (";
                foreach (Delinquent d in demand_letters)
                {
                    DateTimeObject minimal_completed_date = new DateTimeObject();
                    minimal_completed_date.set_date(d.due_date_next_payment.date);
                    query += " (loan_id = " + d.loan_id + " and received >  " + minimal_completed_date.toDBValue() + ") OR ";
                }
                query += " (1 = 2))";
                return get_lm_receptions(query);
            }
            else
                return new List<LMReception>();
        }

        public static List<LMReception> get_lm_receptions(String query)
        {
            List<LMReception> lm_receptions = new List<LMReception>();
            DbDataReader reader = getMsSQLReader(mssqlConnectionString, query);
            while (reader.Read())
            {
                LMReception lm_reception = new LMReception();
                lm_reception = read(reader);
                lm_receptions.Add(lm_reception);
            }
            reader.Close();
            closeMsSQLConnection();
            return lm_receptions;
        }

        private static LMReception read(DbDataReader reader)
        {
            LMReception lm_reception = new LMReception();
            int pos = 0;
            lm_reception.loan_id = readDBDecimal(reader, pos++);
            lm_reception.date = readDBDateObject(reader, pos++, DateTime.Now);
            return lm_reception;
        }

        public bool was_received()
        {
            return this.loan_id > 0M;
        }


    }
}