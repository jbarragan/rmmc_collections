using System;
using System.Collections.Generic;
using System.Web;
using System.Data.Common;

using com.sp.rmmc.common.models;
using com.sp.rmmc.common.models.events;
namespace com.sp.rmmc.collections.models
{
    public class HistoryCollection : BaseCollection
    {
        private string connection_string = System.Configuration.ConfigurationManager.ConnectionStrings["collection"].ConnectionString;
    
        public int history_id = 0;
        protected override string default_columns
        {
            get
            {
                return String.Join(", ", (new CurrentCollection()).default_columns_list().ToArray());
            }
        }

        protected override string collections_query
        {
            get
            {
                DateTimeObject history_date = new DateTimeObject();
                history_date.set_date(this.loan.event_todays_date);

                return
                    "( " +
                    " select * " +
                    " from collection_history_loan_details  " +
                    " where ms_loan_due_date_next_payment <= " + history_date.toDBValue() + 
                    " and history_id = " + this.history_id + " " +
                    ") ";
            }
        }

        protected override string collections_4_month_delinquent_query
        {
            get 
            {
                return
                    "( " +
                    " select * " +
                    " from collection_history_loan_details  " +
                    " where ms_loan_due_date_next_payment <= dateadd(mm, -4, getDate()) " +
                    " and history_id = " + this.history_id + " " +
                    ") ";
            }
        }

        protected override string all_types_query
        {
            get
            {
                return
                    collections_query +
                    " ";
            }
        }


        public HistoryCollection(History history)
        {
            this.history_id = history.id;
            this.loan.event_todays_date = history.history_date.date;
        }

        protected override List<BaseCollection> get_loan_list(String query)
        {
            List<BaseCollection> list = new List<BaseCollection>();
            MsSqlModel model = new MsSqlModel();
            model.setConnectionString(this.connection_string);
            DbDataReader reader = model.getReader(query);
            while (reader.Read())
            {
                BaseCollection f = new BaseCollection();
                f.type = this.type;
                f.loan.event_todays_date = this.loan.event_todays_date;
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
                s += ", \n" + be.columns_name_query();
            }
            return s;
        }
    }
}
