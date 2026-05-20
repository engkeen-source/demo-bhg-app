using System;
using System.Collections.Generic;
using System.Transactions;
using System.Data;
using System.Data.SqlClient;
using System.Collections.ObjectModel;
using System.IO;

namespace BOLib
{
    /// <summary>
    /// Summary description for SYSRepRpts.
    /// </summary>
    [Serializable]
    public class MSTConManageDTable : DataTable
    {
        #region Factory Methods

        public MSTConManageDTable()
        {
        }

        public MSTConManageDTable(SqlConnection cn)
        {
            this.Fetch(cn, new Criteria(0, 1));
        }

        public static MSTConManageDTable Get(int? headerKey)
        {
            MSTConManageDTable obj = new MSTConManageDTable();
            obj.Fetch(new Criteria(headerKey, 1));
            return obj;
        }

        public static MSTConManageDTable New()
        {
            MSTConManageDTable obj = new MSTConManageDTable();
            return obj;
        }

        public static MSTConManageDTable New(SqlConnection cn)
        {
            MSTConManageDTable obj = new MSTConManageDTable();
            obj.Fetch(cn, new Criteria(0, 1));
            return obj;
        }
        #endregion //Factory Methods

        #region Criteria
        [Serializable()]
        internal class Criteria
        {
            public int? _Option = null;
            public int? _DueCal = null;
            public int? _CCB = null;
            public string _DateV = string.Empty;
            public string _ConName = string.Empty;

            internal Criteria()
            {
            }
            internal Criteria(int? Option)
            {
                _Option = Option;
            }
            internal Criteria(int? Option, int? DueCal)
            {
                _Option = Option;
                _DueCal = DueCal;
            }
            internal Criteria(int? Option, int? DueCal, int? CCB)
            {
                _Option = Option;
                _DueCal = DueCal;
                _CCB = CCB;
            }
            internal Criteria(int? Option, int? DueCal, int? CCB, string DateV)
            {
                _Option = Option;
                _DueCal = DueCal;
                _CCB = CCB;
                _DateV = DateV;
            }

            internal Criteria(int? Option, int? DueCal, int? CCB, string DateV,string ConName)
            {
                _Option = Option;
                _DueCal = DueCal;
                _CCB = CCB;
                _DateV = DateV;
                _ConName = ConName;
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
                cm.CommandTimeout = 0;
                cm.CommandText = "Rep_ConAge_Manage";
                cm.Parameters.AddWithValue("@Option", criteria._Option);
                cm.Parameters.AddWithValue("@DateV", criteria._DateV);
                cm.Parameters.AddWithValue("@DueCal", criteria._DueCal);
                cm.Parameters.AddWithValue("@CCB", criteria._CCB);
                cm.Parameters.AddWithValue("@UserKey", AppInfor.currentUserKey);
                cm.Parameters.AddWithValue("@ConName", criteria._ConName);
                // Additional Parameter for Return Value From StoredProcedure -- Changed By Richard
                cm.Parameters.AddWithValue("@RetValue", 0);
                //cm.Parameters["@RetValue"].Direction = ParameterDirection.Output;
                System.Data.SqlClient.SqlDataAdapter sqlAdp = new System.Data.SqlClient.SqlDataAdapter(cm);
                sqlAdp.Fill(this);
                if ((int)cm.Parameters["@RetValue"].Value == (int)GEnum.SpState.Pass)
                    return true;
                else
                    return true;
            }//using            
        }

        #endregion //Data Access - Fetch

        #region Data Access - Insert

        internal bool Insert(List<Int32> lstWatch,List<Int32> lstFollowUpDate,List<Int32> lstCustomer, bool CheckUser)
        {
            bool retValue = false;
           
            using (TransactionScope scope = new TransactionScope())
            {
                // Create new sql connection for this method. 
                using (SqlConnection cn = new SqlConnection(Database.BossDemoConnection))
                {
                    // Open sql connection. 
                    cn.Open();
                    retValue = this.InsertCustomerType(cn,lstWatch,lstFollowUpDate,lstCustomer, CheckUser);
                }
                // No errors - commit transaction
                if (Transaction.Current.TransactionInformation.Status != TransactionStatus.Active) throw new Exception("Transaction has aborted."); scope.Complete();
            }// Already close and dispose sql connection.
            return retValue;
        }
        

        internal bool InsertCustomerType(SqlConnection cn,List<Int32> lstWatch,List<Int32> lstFollowUpDate, List<Int32> lstCustomer, bool CheckUser)
        {
            bool retValue = false;
            if (this.Rows.Count == 0)            {
                return true;
            }
            foreach (DataRow dr in this.Rows)
            {
                if (dr.RowState == DataRowState.Deleted)
                {
                    retValue = true;
                    continue;
                }

                if (lstCustomer.Count != 0)
                {

                    for (var i = 0; i < lstCustomer.Count; i++)
                    {
                        if (lstCustomer[i] == Convert.ToInt32(dr["DocConKey"]))
                        {

                            if (CheckUser == true)
                            {
                                using (SqlCommand cm = cn.CreateCommand())
                                {
                                    cm.CommandType = CommandType.StoredProcedure;
                                    cm.CommandText = "MSTConType_Update";
                                    cm.Parameters.AddWithValue("@ConKey", dr["DocConKey"]);
                                    cm.Parameters.AddWithValue("@ActiveWithProblem", dr["ActiveWithProblem"]);
                                    cm.Parameters.AddWithValue("@COOApprovalRequired", dr["COOApprovalRequired"]);
                                    cm.Parameters.AddWithValue("@LastModifiedUserKey", AppInfor.currentUserKey);
                                    cm.Parameters.AddWithValue("@RetValue", 0);
                                    cm.Parameters["@RetValue"].Direction = ParameterDirection.Output;
                                    // Execute command.
                                    cm.ExecuteNonQuery();
                                    if ((int)cm.Parameters["@RetValue"].Value == (int)GEnum.SpState.Pass)
                                        retValue = true;
                                }// Already close and dispose sql command.
                            }

                        }
                    }
                }

                    if (lstWatch.Count != 0)
                    {

                        for (var j = 0; j < lstWatch.Count; j++)
                        {
                            if (lstWatch[j] == Convert.ToInt32(dr["DocConKey"]))
                            {
                                string watch = string.Empty;
                                watch = dr["Watch"].ToString().ToUpper();
                                if (watch == "TRUE")
                                {
                                    using (SqlCommand cm = cn.CreateCommand())
                                    {
                                        cm.CommandType = CommandType.StoredProcedure;
                                        cm.CommandText = "MSTConWatch_Update";
                                        cm.Parameters.AddWithValue("@ConKey", dr["DocConKey"]);
                                        cm.Parameters.AddWithValue("@LastModifiedUserKey", AppInfor.currentUserKey);
                                        cm.Parameters.AddWithValue("@RetValue", 0);
                                        cm.Parameters["@RetValue"].Direction = ParameterDirection.Output;
                                        // Execute command.
                                        cm.ExecuteNonQuery();

                                        if ((int)cm.Parameters["@RetValue"].Value == (int)GEnum.SpState.Pass)
                                            retValue = true;
                                    }// Already close and dispose sql command.
                                }
                            }
                        }//lstWatch
                    }
                    if (lstFollowUpDate.Count != 0)
                    {
                        for (var h = 0; h < lstFollowUpDate.Count; h++)
                        {
                            if (lstFollowUpDate[h] == Convert.ToInt32(dr["DocConKey"]))
                            {
                                using (SqlCommand cm = cn.CreateCommand())
                                {
                                    cm.CommandType = CommandType.StoredProcedure;
                                    cm.CommandText = "MSTConFollowUpDate_AddUpdate";
                                    cm.Parameters.AddWithValue("@ConKey", dr["DocConKey"]);
                                    cm.Parameters.AddWithValue("@FollowUpDate", dr["FollowUpDate"]);                                    
                                    cm.Parameters.AddWithValue("@LastModifiedUserKey", AppInfor.currentUserKey);
                                    cm.Parameters.AddWithValue("@RetValue", 0);
                                    cm.Parameters["@RetValue"].Direction = ParameterDirection.Output;
                                    // Execute command.
                                    cm.ExecuteNonQuery();

                                    if ((int)cm.Parameters["@RetValue"].Value == (int)GEnum.SpState.Pass)
                                        retValue = true;
                                }// Already close and dispose sql command.
                            }
                        }
                    }

                }           

            return retValue;
        }        

        #endregion Insert 



    }
}