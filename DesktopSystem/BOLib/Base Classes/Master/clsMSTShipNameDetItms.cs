

using System;
using System.Data;
using System.Data.SqlClient;
using Csla;
using Csla.Data;
using System.Transactions;
using System.ComponentModel;
using TAUtil;
namespace BOLib
{
    [Serializable()]
    public class MSTShipNameDetItms : DataTable
    {

        #region Factory Methods

        internal MSTShipNameDetItms()
        {
            this.Fetch(new Criteria(0, 0, 1));
        }

        internal MSTShipNameDetItms(SqlConnection cn)
        {
            this.Fetch(cn, new Criteria(0, 0, 1));
        }

        internal static MSTShipNameDetItms New()
        {

            MSTShipNameDetItms obj = new MSTShipNameDetItms();

            return obj;
        }

        internal static MSTShipNameDetItms New(SqlConnection cn)
        {

            MSTShipNameDetItms obj = new MSTShipNameDetItms(cn);

            return obj;
        }

        internal static MSTShipNameDetItms Get()
        {

            MSTShipNameDetItms obj = new MSTShipNameDetItms();
            obj.Fetch(new Criteria(0, 0, 0));
            return obj;
        }

        #endregion //Factory Methods

        #region Criteria

        [Serializable()]
        internal class Criteria
        {
            public int? _shipNameKey = null;
            public int? _shipMark = null;
            public int? _option = null;

            internal Criteria()
            {
            }
            internal Criteria(int? ShipNameKey, int? Option)
            {
                _shipNameKey = ShipNameKey;
                _option = Option;
            }
            internal Criteria(int? ShipNameKey, int? ShipMark, int? Option)
            {
                _shipNameKey = ShipNameKey;
                _shipMark = ShipMark;
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
                cm.CommandText = "MSTShipNameDetItm_Get";
                cm.Parameters.AddWithValue("@Option", criteria._option);
                cm.Parameters.AddWithValue("@RetValue", 0);
                cm.Parameters["@RetValue"].Direction = ParameterDirection.Output;
                cm.Parameters.AddWithValue("@ShipNameKey", criteria._shipNameKey);
                cm.Parameters.AddWithValue("@ShipMark", criteria._shipMark);

                System.Data.SqlClient.SqlDataAdapter sqlAdp = new System.Data.SqlClient.SqlDataAdapter(cm);
                try
                {
                    sqlAdp.Fill(this);

                }
                catch (Exception ex)
                {
                    throw (ex);
                }

                if ((int)cm.Parameters["@RetValue"].Value == (int)GEnum.SpState.Pass)
                {
                    return true;
                }
                else
                {
                    throw new TAException(MsgID.Common.GetFail);
                }
                return false;
            }//using            
        }

        #endregion //Data Access - Fetch

        #region Data Access - Insert

        public bool Insert(int? shipNameKey)
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
                    retValue = this.Insert(cn, shipNameKey);
                }// End of SqlConnection

                // No errors - commit transaction
                  if (Transaction.Current.TransactionInformation.Status != TransactionStatus.Active)  throw new Exception("Transaction has aborted."); scope.Complete();
            }// End of TransactionScope

            return retValue;
        }

        public bool Insert(SqlConnection cn, int? shipNameKey)
        {
            bool retValue = false;

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
                    cm.CommandText = "MSTShipNameDetItm_AddUpdate";
                    cm.Parameters.AddWithValue("@Option", 0);
                    cm.Parameters.AddWithValue("@RetValue", 0);
                    cm.Parameters["@RetValue"].Direction = ParameterDirection.Output;

                    cm.Parameters.AddWithValue("@ShipNameKey", shipNameKey);

                    if (dr["ShipMark"] == null)
                        cm.Parameters.AddWithValue("@ShipMark", DBNull.Value);
                    else
                        cm.Parameters.AddWithValue("@ShipMark", dr["shipMark"]);


                    cm.Parameters.AddWithValue("@CreateDate", DateTime.Now);

                    if (AppInfor.currentUserKey == null)
                        cm.Parameters.AddWithValue("@CreateUserKey", DBNull.Value);
                    else
                        cm.Parameters.AddWithValue("@CreateUserKey", AppInfor.currentUserKey);

                    if (dr["lastModifiedDate"] == null)
                        cm.Parameters.AddWithValue("@LastModifiedDate", DBNull.Value);
                    else
                        cm.Parameters.AddWithValue("@LastModifiedDate", dr["lastModifiedDate"]);

                    if (dr["lastModifiedUserKey"] == null)
                        cm.Parameters.AddWithValue("@LastModifiedUserKey", DBNull.Value);
                    else
                        cm.Parameters.AddWithValue("@LastModifiedUserKey", AppInfor.currentUserKey);

                    if (dr["custom1"] == null)
                        cm.Parameters.AddWithValue("@Custom1", DBNull.Value);
                    else
                        cm.Parameters.AddWithValue("@Custom1", dr["custom1"]);

                    if (dr["custom2"] == null)
                        cm.Parameters.AddWithValue("@Custom2", DBNull.Value);
                    else
                        cm.Parameters.AddWithValue("@Custom2", dr["custom2"]);

                    if (dr["custom3"] == null)
                        cm.Parameters.AddWithValue("@Custom3", DBNull.Value);
                    else
                        cm.Parameters.AddWithValue("@Custom3", dr["custom3"]);

                    cm.ExecuteNonQuery();

                    if ((int)cm.Parameters["@RetValue"].Value == (int)GEnum.SpState.Pass)
                    {
                        retValue = true;
                    }
                    else
                    {
                        retValue = false;
                    }
                }

            }// Already close and dispose sql connection.
            return retValue;
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
                // Using existing sql connection.
                using (SqlCommand cm = cn.CreateCommand())
                {
                    cm.CommandType = CommandType.StoredProcedure;
                    cm.CommandText = "MSTShipNameDetItm_AddUpdate";
                    cm.Parameters.AddWithValue("@Option", 1);
                    cm.Parameters.AddWithValue("@RetValue", 0);
                    cm.Parameters["@RetValue"].Direction = ParameterDirection.Output;

                    if (dr["ShipNameKey"] == null)
                        cm.Parameters.AddWithValue("@ShipNameKey", DBNull.Value);
                    else
                        cm.Parameters.AddWithValue("@ShipNameKey", dr["shipNameKey"]);

                    if (dr["ShipMark"] == null)
                        cm.Parameters.AddWithValue("@ShipMark", DBNull.Value);
                    else
                        cm.Parameters.AddWithValue("@ShipMark", dr["shipMark"]);


                    cm.Parameters.AddWithValue("@CreateDate", DateTime.Now);

                    if (AppInfor.currentUserKey == null)
                        cm.Parameters.AddWithValue("@CreateUserKey", DBNull.Value);
                    else
                        cm.Parameters.AddWithValue("@CreateUserKey", AppInfor.currentUserKey);

                    if (dr["lastModifiedDate"] == null)
                        cm.Parameters.AddWithValue("@LastModifiedDate", DBNull.Value);
                    else
                        cm.Parameters.AddWithValue("@LastModifiedDate", dr["lastModifiedDate.Value"]);

                    if (dr["lastModifiedUserKey"] == null)
                        cm.Parameters.AddWithValue("@LastModifiedUserKey", DBNull.Value);
                    else
                        cm.Parameters.AddWithValue("@LastModifiedUserKey", dr["lastModifiedUserKey"]);

                    if (dr["custom1"] == null)
                        cm.Parameters.AddWithValue("@Custom1", DBNull.Value);
                    else
                        cm.Parameters.AddWithValue("@Custom1", dr["custom1"]);

                    if (dr["custom2"] == null)
                        cm.Parameters.AddWithValue("@Custom2", DBNull.Value);
                    else
                        cm.Parameters.AddWithValue("@Custom2", dr["custom2"]);

                    if (dr["custom3"] == null)
                        cm.Parameters.AddWithValue("@Custom3", DBNull.Value);
                    else
                        cm.Parameters.AddWithValue("@Custom3", dr["custom3"]);

                    cm.ExecuteNonQuery();

                    if ((int)cm.Parameters["@RetValue"].Value == (int)GEnum.SpState.Pass)
                        return true;
                    else
                        return false;

                }// Already close and dispose sql connection.
            }
            return retValue;
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
                cm.CommandText = "MSTShipNameDetItm_Delete";
                cm.Parameters.AddWithValue("@RetValue", 0);
                cm.Parameters["@RetValue"].Direction = ParameterDirection.Output;

                cm.Parameters.AddWithValue("@ShipNameKey", criteria._shipNameKey);
                cm.Parameters.AddWithValue("@ShipMark", criteria._shipMark);
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

