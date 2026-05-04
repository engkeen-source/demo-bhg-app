

using System;
using System.Data;
using System.Data.SqlClient;
using Csla;
using Csla.Data;
using System.Transactions;
using System.Collections.Generic;

namespace BOLib
{
    [Serializable()]
    public class NSSalesPOLinks : DataTable
    {

        #region Factory Methods

        internal NSSalesPOLinks()
        {
        }

        internal static NSSalesPOLinks New()
        {            
            NSSalesPOLinks obj = new NSSalesPOLinks();            
            return obj;
        }
        internal static NSSalesPOLinks New(SqlConnection cn,GEnum.SystemCode codeKey)
        {
            NSSalesPOLinks obj = new NSSalesPOLinks();
            obj.Fetch(cn,new Criteria(DateTime.Today,DateTime.Today,0,0));
            return obj;
        }
        internal static NSSalesPOLinks Get()
        {            
            NSSalesPOLinks obj = new NSSalesPOLinks();
            obj.Fetch(new Criteria(0, 0));
            return obj;
        }

        #endregion //Factory Methods 

        #region Criteria

        [Serializable()]
        internal class Criteria
        {
            public int? _conKey = null;
            public int? _empKey = null;
            public DateTime? _from = null;
            public DateTime? _to = null;          

            internal Criteria()
            {
            }

            internal Criteria(int? ConKey, int? Option)
            {
                _conKey = ConKey;               
            }
            internal Criteria(DateTime from,DateTime to, int? ConKey,int? EmpKey)
            {
                _conKey = ConKey;
                _empKey = EmpKey;
                _from = from;
                _to = to;               
            }
        }

        #endregion //Criteria

        #region Data Access - Fetch

        internal bool Fetch(Criteria criteria)
        {
            bool retValue = false;            
            
            // Create new sql connection for this method. 
            using (SqlConnection cn = new SqlConnection(Database.BossDemoConnection))
            {
                // Open sql connection. 
                cn.Open();

                retValue = this.Fetch(cn, criteria);
            }// End of SqlConnection.
                
            
            return retValue;
        }

        internal bool Fetch(SqlConnection cn, Criteria criteria)
        {
            using (SqlCommand cm = cn.CreateCommand())
            {
                cm.CommandType = CommandType.StoredProcedure;
                cm.CommandText = "Rep_GPConItmNS";
          
                
                cm.Parameters.AddWithValue("@ConKey", criteria._conKey);
                cm.Parameters.AddWithValue("@EmpKey", criteria._empKey);
                cm.Parameters.AddWithValue("@DateFrom", criteria._from);
                cm.Parameters.AddWithValue("@DateTo", criteria._to);         

            
                System.Data.SqlClient.SqlDataAdapter sqlAdp = new System.Data.SqlClient.SqlDataAdapter(cm);
                sqlAdp.Fill(this);
                
                return true;
            }//using            
        }

        #endregion //Data Access - Fetch



        internal bool Save(SqlConnection cn)
        {
            this.DefaultView.RowFilter = "ItmVendorPriceH<>FilterCost";
            DataTable dt = this.DefaultView.ToTable();
            this.DefaultView.RowFilter = "";
            if(dt.Rows.Count==0)
            {
                MsgBox.Show(cn,"No data changes to save.");
                return false;
            }
            dt.TableName = "dtNSSales";
            string XMLMST_ConOpenBal = GFunc.ConvertDataTableToXML(dt);

            //Save the Detail Grid
            List<SqlParameter> parmList = new List<SqlParameter>();
         
            parmList.Add(new SqlParameter("@UserKey", AppInfor.currentUserKey));            
            parmList.Add(new SqlParameter("@xmlDetail", XMLMST_ConOpenBal));
            parmList.Add(new SqlParameter("@RetValue", 0));
            parmList[2].Direction = ParameterDirection.Output;

            GFunc.ExecuteNonQueryProc(cn,"NSSalesPOLink_Save", parmList);

            return true;            
        }
    }
}

