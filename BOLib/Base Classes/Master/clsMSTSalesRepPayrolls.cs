

using System;
using System.Data;
using System.Data.SqlClient;
using Csla;
using Csla.Data;
using System.Transactions;

namespace BOLib
{
    [Serializable()]
    public class MSTSalesRepPayRolls : DataTable
    {

        #region Factory Methods

        //internal MSTSalesRepPayRolls()
        //{
        //}

        //internal static MSTSalesRepPayRolls New()
        //{
            
        //    MSTSalesRepPayRolls obj = new MSTSalesRepPayRolls();          
        //    return obj;
        //}

        //internal static MSTSalesRepPayRolls Get()
        //{            
        //    MSTSalesRepPayRolls obj = new MSTSalesRepPayRolls();
        //    obj.Fetch(new Criteria(0, 0, 0));
        //    return obj;
        //}


         public MSTSalesRepPayRolls()
        {
            this.Fetch(new Criteria(0, 1));          
        }

         public MSTSalesRepPayRolls(SqlConnection cn)
        {
            this.Fetch(cn, new Criteria(0, 1));
        }
     
         public static MSTSalesRepPayRolls Get(int? currKey)
        {
            MSTSalesRepPayRolls obj = new MSTSalesRepPayRolls();
            obj.Fetch(new Criteria(currKey, 1));
            return obj;
        }


        #endregion //Factory Methods

        #region Criteria

        [Serializable()]
        internal class Criteria
        {
            public int? _eMKey = null;
            public int? _transKey = null;
            public int? _option = null;

            internal Criteria()
            {
            }
            internal Criteria(int? EMKey,  int? Option)
            {
                _eMKey = EMKey;
                _transKey = 0;
                _option = Option;
            }
            internal Criteria(int? EMKey, int? TransKey, int? Option)
            {
                _eMKey = EMKey;
                _transKey = TransKey;
                _option = Option;
            }
        }

        #endregion //Criteria

        #region Data Access - Fetch

        private bool Fetch(Criteria criteria)
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
                cm.CommandText = "MSTSalesRepPayRoll_Get";
                cm.Parameters.AddWithValue("@Option", criteria._option);
                cm.Parameters.AddWithValue("@RetValue", 0);
                cm.Parameters["@RetValue"].Direction = ParameterDirection.Output;

                cm.Parameters.AddWithValue("@EMKey", criteria._eMKey);
                cm.Parameters.AddWithValue("@TransKey", criteria._transKey);


                System.Data.SqlClient.SqlDataAdapter sqlAdp = new System.Data.SqlClient.SqlDataAdapter(cm);
                sqlAdp.Fill(this);
                

                //using (SafeDataReader dr = new SafeDataReader(cm.ExecuteReader()))
                //{
                //    while (dr.Read())
                //        this.Add(MSTSalesRepPayRoll.Get(dr));
                //}
                if ((int)cm.Parameters["@RetValue"].Value == (int)GEnum.SpState.Pass)
                    return true;
                else
                    return false;

            }//using            
        }

        #endregion //Data Access - Fetch


        #region Data Access - Insert

        internal bool Insert(int? headerKey)
        {
            bool retValue = false;


            // Create Transaction Scope
            using (TransactionScope scope = new TransactionScope())
            {
                // Create SqlConnection
                using (SqlConnection cn = new SqlConnection(Database.BossDemoConnection))
                {
                    // Open Connection
                    cn.Open();

                    // Call insert method.
                    retValue = this.Insert(cn,headerKey);
                }// End of SqlConnection

                // No errors - commit transaction
                  if (Transaction.Current.TransactionInformation.Status != TransactionStatus.Active)  throw new Exception("Transaction has aborted."); scope.Complete();
            }// End of TransactionScope

            return retValue;
        }

        internal bool Insert(SqlConnection cn,int? headerKey)
        {
            bool retValue = false;
            int transKey = 0;

            if (this.Rows.Count == 0)
            {
                return true;
            }
            foreach (DataRow dr in this.Rows)
            {
                if (dr.RowState == DataRowState.Deleted)
                {
                    retValue = true;
                    continue;
                }
                using (SqlCommand cm = cn.CreateCommand())
                {
                    cm.CommandType = CommandType.StoredProcedure;
                    cm.CommandText = "MSTSalesRepPayRoll_AddUpdate";
                    cm.Parameters.AddWithValue("@Option", 0);
                    cm.Parameters.AddWithValue("@EMKey", headerKey);
                    cm.Parameters.AddWithValue("@TransKey", dr["TransKey"]);
                    cm.Parameters.AddWithValue("@TransType", dr["TransType"].ToString() == "" ? 0 : dr["TransType"]);
                    cm.Parameters.AddWithValue("@TransDate", dr["TransDate"]);
                    cm.Parameters.AddWithValue("@TransDes",  dr["TransDes"]);
                    cm.Parameters.AddWithValue("@TransAmt", dr["TransAmt"].ToString() == "" ? 0 : dr["TransAmt"]);
                    cm.Parameters.AddWithValue("@TransDeptKey", dr["TransDeptKey"].ToString() == "" ? 0 : dr["TransDeptKey"]);
                    cm.Parameters.AddWithValue("@TransGrpKey", dr["TransGrpKey"].ToString() == "" ? 0 : dr["TransGrpKey"]);
                   
                    cm.Parameters.AddWithValue("@RetValue", 0);
                    cm.Parameters["@RetValue"].Direction = ParameterDirection.Output;
                    cm.Parameters.AddWithValue("@NewTransKey", transKey);
                    cm.Parameters["@NewTransKey"].Direction = ParameterDirection.Output;

                    // Execute command.
                    cm.ExecuteNonQuery();

                    transKey = (int)cm.Parameters["@NewTransKey"].Value;
                    if ((int)cm.Parameters["@RetValue"].Value == (int)GEnum.SpState.Pass)
                    {
                        retValue = true;
                    }
                }// Already close and dispose sql command.
            }

            return retValue;


            #region old code
            // Using existing sql connection.
            //using (SqlCommand cm = cn.CreateCommand())
            //{
            //    cm.CommandType = CommandType.StoredProcedure;
            //    cm.CommandText = "MSTSalesRepPayRoll_AddUpdate";
            //    cm.Parameters.AddWithValue("@Option", 0);
            //    cm.Parameters.AddWithValue("@RetValue", 0);
            //    cm.Parameters["@RetValue"].Direction = ParameterDirection.Output;
            //    cm.Parameters.AddWithValue("@NewTransKey", transKey);
            //    cm.Parameters["@NewTransKey"].Direction = ParameterDirection.Output;


            //    if (_eMKey == null)
            //        cm.Parameters.AddWithValue("@EMKey", DBNull.Value);
            //    else
            //        cm.Parameters.AddWithValue("@EMKey", _eMKey);

            //    //if (_transKey == null)
            //    //    cm.Parameters.AddWithValue("@TransKey", DBNull.Value);
            //    //else
            //    cm.Parameters.AddWithValue("@TransKey", transKey);

            //    if (_transType == null)
            //        cm.Parameters.AddWithValue("@TransType", DBNull.Value);
            //    else
            //        cm.Parameters.AddWithValue("@TransType", _transType);

            //    if (_transDate == null)
            //        cm.Parameters.AddWithValue("@TransDate", DBNull.Value);
            //    else
            //        cm.Parameters.AddWithValue("@TransDate", _transDate.Value);

            //    if (_transDes == null)
            //        cm.Parameters.AddWithValue("@TransDes", DBNull.Value);
            //    else
            //        cm.Parameters.AddWithValue("@TransDes", _transDes);

            //    if (_transAmt == null)
            //        cm.Parameters.AddWithValue("@TransAmt", DBNull.Value);
            //    else
            //        cm.Parameters.AddWithValue("@TransAmt", _transAmt);

            //    if (_transDeptKey == null)
            //        cm.Parameters.AddWithValue("@TransDeptKey", DBNull.Value);
            //    else
            //        cm.Parameters.AddWithValue("@TransDeptKey", _transDeptKey);

            //    if (_transGrpKey == null)
            //        cm.Parameters.AddWithValue("@TransGrpKey", DBNull.Value);
            //    else
            //        cm.Parameters.AddWithValue("@TransGrpKey", _transGrpKey);



            //    cm.ExecuteNonQuery();


            //    transKey = (int)cm.Parameters["@NewTransKey"].Value;

            //    if ((int)cm.Parameters["@RetValue"].Value == (int)GEnum.SpState.Pass)
            //        return true;
            //    else
            //        return false; 
            #endregion
            //}// Already close and dispose sql connection.

        }

        #endregion //Data Access - Insert

        #region Data Access - Update

        internal bool Update()
        {
            bool retValue = false;


            // Create Transaction Scope
            using (TransactionScope scope = new TransactionScope())
            {
                // Create SqlConnection
                using (SqlConnection cn = new SqlConnection(Database.BossDemoConnection))
                {
                    // Open Connection
                    cn.Open();

                    // Call update method.
                    retValue = this.Update(cn);
                }// End of SqlConnection

                // No errors - commit transaction
                  if (Transaction.Current.TransactionInformation.Status != TransactionStatus.Active)  throw new Exception("Transaction has aborted."); scope.Complete();
            }// End of TransactionScope

            return retValue;
        }

        internal bool Update(SqlConnection cn)
        {

            bool retValue = false;
            foreach (DataRow dr in this.Rows)
            {
                using (SqlCommand cm = cn.CreateCommand())
                {
                    cm.CommandType = CommandType.StoredProcedure;
                    cm.CommandText = "MSTSalesRepPayRoll_AddUpdate";

                    cm.Parameters.AddWithValue("@Option", 0);
                    cm.Parameters.AddWithValue("@EMKey", dr["EmKey"]);
                    cm.Parameters.AddWithValue("@TransKey", dr["TransKey"]);
                    cm.Parameters.AddWithValue("@TransType", dr["TraTransTypensAmt"].ToString() == "" ? 0 : dr["TransType"]);
                    cm.Parameters.AddWithValue("@TransDate", dr["TransDate"]);
                    cm.Parameters.AddWithValue("@TransDes", dr["TransDes"]);
                    cm.Parameters.AddWithValue("@TransAmt", dr["TransAmt"].ToString() == "" ? 0 : dr["TransAmt"]);
                    cm.Parameters.AddWithValue("@TransDeptKey", dr["TransDeptKey"].ToString() == "" ? 0 : dr["TransDeptKey"]);
                    cm.Parameters.AddWithValue("@TransGrpKey", dr["TransGrpKey"].ToString() == "" ? 0 : dr["TransGrpKey"]);

                    cm.Parameters.AddWithValue("@RetValue", 0);
                    cm.Parameters["@RetValue"].Direction = ParameterDirection.Output;
                    // Execute command.
                    cm.ExecuteNonQuery();


                    if ((int)cm.Parameters["@RetValue"].Value == (int)GEnum.SpState.Pass)
                        retValue = true;
                    else
                        retValue = false;
                }
            }// Already close and dispose sql command.

            return retValue;

            #region Old code

            // Using existing sql connection.
            //using (SqlCommand cm = cn.CreateCommand())
            //{
            //    cm.CommandType = CommandType.StoredProcedure;
            //    cm.CommandText = "MSTSalesRepPayRoll_AddUpdate";
            //    cm.Parameters.AddWithValue("@Option", 1);
            //    cm.Parameters.AddWithValue("@RetValue", 0);
            //    cm.Parameters["@RetValue"].Direction = ParameterDirection.Output;
            //    cm.Parameters.AddWithValue("@NewTransKey", 0);

            //    if (_eMKey == null)
            //        cm.Parameters.AddWithValue("@EMKey", DBNull.Value);
            //    else
            //        cm.Parameters.AddWithValue("@EMKey", _eMKey);

            //    if (_transKey == null)
            //        cm.Parameters.AddWithValue("@TransKey", DBNull.Value);
            //    else
            //        cm.Parameters.AddWithValue("@TransKey", _transKey);

            //    if (_transType == null)
            //        cm.Parameters.AddWithValue("@TransType", DBNull.Value);
            //    else
            //        cm.Parameters.AddWithValue("@TransType", _transType);

            //    if (_transDate == null)
            //        cm.Parameters.AddWithValue("@TransDate", DBNull.Value);
            //    else
            //        cm.Parameters.AddWithValue("@TransDate", _transDate.Value);

            //    if (_transDes == null)
            //        cm.Parameters.AddWithValue("@TransDes", DBNull.Value);
            //    else
            //        cm.Parameters.AddWithValue("@TransDes", _transDes);

            //    if (_transAmt == null)
            //        cm.Parameters.AddWithValue("@TransAmt", DBNull.Value);
            //    else
            //        cm.Parameters.AddWithValue("@TransAmt", _transAmt);

            //    if (_transDeptKey == null)
            //        cm.Parameters.AddWithValue("@TransDeptKey", DBNull.Value);
            //    else
            //        cm.Parameters.AddWithValue("@TransDeptKey", _transDeptKey);

            //    if (_transGrpKey == null)
            //        cm.Parameters.AddWithValue("@TransGrpKey", DBNull.Value);
            //    else
            //        cm.Parameters.AddWithValue("@TransGrpKey", _transGrpKey);

            //    cm.Parameters["@NewTransKey"].Direction = ParameterDirection.Output;

            //    cm.ExecuteNonQuery();

            //    if ((int)cm.Parameters["@RetValue"].Value == (int)GEnum.SpState.Pass)
            //        return true;
            //    else
            //        return false; 

            //}// Already close and dispose sql connection.
            #endregion
        }
        #endregion //Data Access - Update

        #region Data Access - Delete

        internal bool Delete(Criteria criteria)
        {
            bool retValue = false;

            // Create Transaction Scope
            using (TransactionScope scope = new TransactionScope())
            {
                // Create SqlConnection
                using (SqlConnection cn = new SqlConnection(Database.BossDemoConnection))
                {
                    // Open Connection
                    cn.Open();

                    // Call delete method.
                    retValue = this.Delete(cn, criteria);
                }// End of SqlConnection

                // No errors - commit transaction
                  if (Transaction.Current.TransactionInformation.Status != TransactionStatus.Active)  throw new Exception("Transaction has aborted."); scope.Complete();
            }// End of TransactionScope

            return retValue;
        }

        internal bool Delete(SqlConnection cn, Criteria criteria)
        {
            // Using existing sql connection.
            using (SqlCommand cm = cn.CreateCommand())
            {
                cm.CommandType = CommandType.StoredProcedure;
                cm.CommandText = "MSTSalesRepPayRoll_Delete";
                cm.Parameters.AddWithValue("@RetValue", 0);
                cm.Parameters["@RetValue"].Direction = ParameterDirection.Output;

                cm.Parameters.AddWithValue("@EMKey", criteria._eMKey);
                cm.Parameters.AddWithValue("@TransKey", criteria._transKey);
                cm.Parameters.AddWithValue("@Option", criteria._option);

                cm.ExecuteNonQuery();

                if ((int)cm.Parameters["@RetValue"].Value == (int)GEnum.SpState.Pass)
                {
                    return true;
                }
                else
                {
                    return false;
                }

            }// Already close and dispose sql connection.

        }

        #endregion //Data Access - Delete

    }
}

