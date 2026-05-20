using System;
using System.Collections.Generic;
using System.Transactions;
using System.Data;
using System.Data.SqlClient;
using System.ComponentModel;
using TAUtil;
namespace BOLib
{
	/// <summary>
	/// Summary description for EStoreQO.
	/// </summary>
	[Serializable]
	public class EStoreQO 
	{
      
        public int entity_id;
        public string quoteid;
        public DateTime quote_date;
        public string company;
        public string payment_term;
        public string sales_id;
        public string reference_no;
        public string vessel_marking;       
        public string doc_remark;
      
        public string comment;
        public string designation;
           
        public decimal? sub_total;
        public decimal? gst_percent;
        public decimal? gst_amount;
        public decimal? grand_total;
        public string curr_id;
        public decimal? currRate;
        public decimal? delivery_charges;
        public string salesrep_email;

        public string status;
        public string street;
        public string poBox;
        public string city;
        public string state;
        public string zipCode;
        public string country;
        public string region;
        public string name;
        public string contact_no;
        public string fax;
        public string email;
        public string docQONum;
        public string skus;
        public string additional_data;
        public string delivery_type;
        public string delivery_chargesDes;
      
        /// <summary>
        /// Default constructor that will initialize all properties with default values.
        /// </summary>

        public EStoreQO()
            :base()
        {
                     
        }     

        /// <summary>
        /// Disposing objects
        /// </summary>
        public void Dispose()
        {
        }
	}
}