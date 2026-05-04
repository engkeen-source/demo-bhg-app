using System;
using System.Data;
using System.Data.SqlClient;
using System.Transactions;
using TAUtil;

namespace BOLib
{
    [Serializable()]
    public class REFAddrs : DataTable
    {

        #region Factory Methods

        public REFAddrs()
        {
           // 
            
        }
        //public REFAddrs()
        //{
        //    this.Fetch(new Criteria(0, 1));
        //}

        public REFAddrs(SqlConnection cn)
        {
            this.Fetch(cn, new Criteria(0, 1));
        }
      
        internal static REFAddrs Get()
        {
            REFAddrs obj = new REFAddrs();
            obj.Fetch(new Criteria(0, 0));
            return obj;
        }

        public static REFAddrs Get( int? headerKey)
        {
           
            REFAddrs obj = new REFAddrs();
            obj.Fetch(new Criteria(headerKey, 1));
            return obj;
        }

        public static REFAddrs Get(string AddrID)
        {

            REFAddrs obj = new REFAddrs();
            obj.Fetch(new Criteria(AddrID, 4));
            return obj;
        }

        public static REFAddrs New()
        {
            REFAddrs obj = new REFAddrs();
            return obj;
        }
        public static REFAddrs New(SqlConnection cn)
        {
            REFAddrs obj = new REFAddrs();
            obj.Fetch(cn,new Criteria(0, 1));
            return obj;
        }
        #endregion //Factory Methods

        #region Criteria

        [Serializable()]
        internal class Criteria
        {
            public int? _headerKey = null;
            public int? _option = null;

            public int? _addrKey = null;
            public int? _addrLinkType = null;
            public int? _addrLinkKey = null;
            public string _addrID = null;

            internal Criteria()
            {
            }

            internal Criteria(int? HeaderKey, int? Option)
            {
                _headerKey = HeaderKey;
                _option = Option;
            }

            internal Criteria(string AddrID, int? Option)
            {
                _addrID = AddrID;
                _addrKey = 0;
                _addrLinkType = 0;
                _addrLinkKey = 0; 
                _option = Option;
            }

            internal Criteria(int? AddrKey, int? AddrLinkType, int? AddrLinkKey, string AddrID, int? Option)
            {
                _addrKey = AddrKey;
                _addrLinkType = AddrLinkType;
                _addrLinkKey = AddrLinkKey;
                _addrID = AddrID;
                _option = Option;
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
            
            bool retValue = false;
            using (SqlCommand cm = cn.CreateCommand())
            {
                cm.CommandType = CommandType.StoredProcedure;
                cm.CommandText = "REFAddr_Get";

                cm.Parameters.AddWithValue("@Option", criteria._option);
                cm.Parameters.AddWithValue("@AddrKey", criteria._addrKey);
                cm.Parameters.AddWithValue("@AddrLinkType", criteria._addrLinkType);
                cm.Parameters.AddWithValue("@AddrLinkKey", criteria._addrLinkKey);
                cm.Parameters.AddWithValue("@AddrID", criteria._addrID);
                
                // Additional Parameter for Return Value From StoredProcedure -- Changed By Richard
                cm.Parameters.AddWithValue("@RetValue", 0);
                cm.Parameters["@RetValue"].Direction = ParameterDirection.Output;

                System.Data.SqlClient.SqlDataAdapter sqlAdp = new System.Data.SqlClient.SqlDataAdapter(cm);
                sqlAdp.Fill(this);

                if ((int)cm.Parameters["@RetValue"].Value == (int)GEnum.SpState.Pass)
                    retValue = true;
                else
                    retValue=false;

            }//using            
            return retValue;
        }
        
        #endregion //Data Access - Fetch

        #region Data Access - Insert

        internal bool Insert(Criteria _criteia)
        {
            bool retValue = false;
            
            using (TransactionScope scope = new TransactionScope())
            {
                // Create new sql connection for this method. 
                using (SqlConnection cn = new SqlConnection(Database.BossDemoConnection))
                {
                    // Open sql connection. 
                    cn.Open();
                    retValue = this.Insert(cn, _criteia);
                }
                // No errors - commit transaction
                  if (Transaction.Current.TransactionInformation.Status != TransactionStatus.Active)  throw new Exception("Transaction has aborted."); scope.Complete();
            }// Already close and dispose sql connection.
            
            return retValue;
        }

        internal bool Insert(SqlConnection cn, Criteria _criteria)
        {
            bool retValue = false;
            string msgID = "RecordAddFail";

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
                    cm.CommandText = "REFAddr_AddUpdate";

                    cm.Parameters.AddWithValue("@Option", 0);
                    cm.Parameters.AddWithValue("@NewAddrKey", 0);

                    cm.Parameters.AddWithValue("@AddrKey", dr["AddrKey"]);
                    cm.Parameters.AddWithValue("@AddrLinkType", _criteria._addrLinkType);
                    cm.Parameters.AddWithValue("@AddrLinkKey", _criteria._addrLinkKey);
                    cm.Parameters.AddWithValue("@AddrID", dr["AddrID"]);
                    cm.Parameters.AddWithValue("@AddrType", dr["AddrType"]);
                    cm.Parameters.AddWithValue("@AddrStreet", dr["AddrStreet"]);
                    cm.Parameters.AddWithValue("@AddrPOBox", dr["AddrPOBox"]);
                    cm.Parameters.AddWithValue("@AddrCity", dr["AddrCity"]);
                    cm.Parameters.AddWithValue("@AddrState", dr["AddrState"]);
                    cm.Parameters.AddWithValue("@AddrZipCode", dr["AddrZipCode"]);
                    cm.Parameters.AddWithValue("@AddrCountry", dr["AddrCountry"]);
                    cm.Parameters.AddWithValue("@AddrRegion", dr["AddrRegion"]);
                    cm.Parameters.AddWithValue("@AddrAttn", dr["AddrAttn"]);
                    cm.Parameters.AddWithValue("@AddrTel1", dr["AddrTel1"]);
                    cm.Parameters.AddWithValue("@AddrTel2", dr["AddrTel2"]);
                    cm.Parameters.AddWithValue("@AddrFax", dr["AddrFax"]);
                    cm.Parameters.AddWithValue("@AddrEmail", dr["AddrEmail"]);
                    cm.Parameters.AddWithValue("@AddrShipViaKey", dr["AddrShipViaKey"]);
                    cm.Parameters.AddWithValue("@Custom1", dr["Custom1"]);
                    cm.Parameters.AddWithValue("@Custom2", dr["Custom2"]);
                    cm.Parameters.AddWithValue("@Custom3", dr["Custom3"]);
                    cm.Parameters.AddWithValue("@CreateDate", DBNull.Value);
                    cm.Parameters.AddWithValue("@CreateUserKey", AppInfor.currentUserKey);
                    cm.Parameters.AddWithValue("@LastModifiedDate", DBNull.Value);
                    cm.Parameters.AddWithValue("@LastModifiedUserKey", AppInfor.currentUserKey);

                    cm.Parameters["@NewAddrKey"].Direction = ParameterDirection.Output;
                    cm.Parameters.AddWithValue("@RetValue", 0);
                    cm.Parameters["@RetValue"].Direction = ParameterDirection.Output;
                    // Execute command.
                    cm.ExecuteNonQuery();

                    if ((int)cm.Parameters["@RetValue"].Value == (int)GEnum.SpState.Pass)
                        retValue = true;
                    else
                    {
                        throw new TAException(msgID);
                    }
                }// Already close and dispose sql command.
            }
            return retValue;
        }

        #endregion Insert

        #region Data Access - Update

        internal bool Update(Criteria _criteria)
        {
            bool retValue = false;
            
            using (TransactionScope scope = new TransactionScope())
            {
                //Create new sql connection for this method. 
                using (SqlConnection cn = new SqlConnection(Database.BossDemoConnection))
                {
                    // Open sql connection. 
                    cn.Open();
                    retValue = this.Update(cn,_criteria);
                }
                // No errors - commit transaction
                  if (Transaction.Current.TransactionInformation.Status != TransactionStatus.Active)  throw new Exception("Transaction has aborted."); scope.Complete();
            }// Already close and dispose sql connection.
            
            return retValue;
        }

        internal bool Update(SqlConnection cn,Criteria _criteria)
        {
            bool retValue = false;
            string msgID = "RecordUpdateFail";
            
            if (this.Rows.Count == 0)
            {
                return true;
            }
            foreach (DataRow dr in this.Rows)
            {
                using (SqlCommand cm = cn.CreateCommand())
                {
                    cm.CommandType = CommandType.StoredProcedure;
                    cm.CommandText = "REFAddr_AddUpdate";

                    int AddrKey = 0;
                    Int32.TryParse(dr["AddrKey"].ToString(), out AddrKey);
                    if (AddrKey ==0)
                    {
                        cm.Parameters.AddWithValue("@Option", 0);
                    }
                    else
                    {
                        cm.Parameters.AddWithValue("@Option", 1);
                    }
                    cm.Parameters.AddWithValue("@NewAddrKey", 0);
                    cm.Parameters.AddWithValue("@AddrKey", dr["AddrKey"]);
                    cm.Parameters.AddWithValue("@AddrLinkType", _criteria._addrLinkType);
                    cm.Parameters.AddWithValue("@AddrLinkKey", _criteria._addrLinkKey);
                    cm.Parameters.AddWithValue("@AddrID", dr["AddrID"]);
                    cm.Parameters.AddWithValue("@AddrType", dr["AddrType"]);
                    cm.Parameters.AddWithValue("@AddrStreet", dr["AddrStreet"]);
                    cm.Parameters.AddWithValue("@AddrPOBox", dr["AddrPOBox"]);
                    cm.Parameters.AddWithValue("@AddrCity", dr["AddrCity"]);
                    cm.Parameters.AddWithValue("@AddrState", dr["AddrState"]);
                    cm.Parameters.AddWithValue("@AddrZipCode", dr["AddrZipCode"]);
                    cm.Parameters.AddWithValue("@AddrCountry", dr["AddrCountry"]);
                    cm.Parameters.AddWithValue("@AddrRegion", dr["AddrRegion"]);
                    cm.Parameters.AddWithValue("@AddrAttn", dr["AddrAttn"]);
                    cm.Parameters.AddWithValue("@AddrTel1", dr["AddrTel1"]);
                    cm.Parameters.AddWithValue("@AddrTel2", dr["AddrTel2"]);
                    cm.Parameters.AddWithValue("@AddrFax", dr["AddrFax"]);
                    cm.Parameters.AddWithValue("@AddrEmail", dr["AddrEmail"]);
                    cm.Parameters.AddWithValue("@AddrShipViaKey", dr["AddrShipViaKey"]);
                    cm.Parameters.AddWithValue("@Custom1", dr["Custom1"]);
                    cm.Parameters.AddWithValue("@Custom2", dr["Custom2"]);
                    cm.Parameters.AddWithValue("@Custom3", dr["Custom3"]);
                    cm.Parameters.AddWithValue("@CreateDate", DBNull.Value);
                    cm.Parameters.AddWithValue("@CreateUserKey", AppInfor.currentUserKey);
                    cm.Parameters.AddWithValue("@LastModifiedDate", DBNull.Value);
                    cm.Parameters.AddWithValue("@LastModifiedUserKey", AppInfor.currentUserKey);
                    cm.Parameters["@NewAddrKey"].Direction = ParameterDirection.Output;
                    cm.Parameters.AddWithValue("@RetValue", 0);
                    cm.Parameters["@RetValue"].Direction = ParameterDirection.Output;
                    cm.ExecuteNonQuery();
                    if ((int)cm.Parameters["@RetValue"].Value == (int)GEnum.SpState.Pass)
                        retValue = true;
                    else
                    {
                        throw new TAException(msgID);
                    }
                }
            }// Already close and dispose sql command.
            return retValue;
        }

        #endregion Update

        #region Data Access - Delete

        internal bool Delete(Criteria criteria)
        {
            bool retValue = false;
            
            using (TransactionScope scope = new TransactionScope())
            {
                //Create new sql connection for this method. 
                using (SqlConnection cn = new SqlConnection(Database.BossDemoConnection))
                {
                    // Open sql connection. 
                    cn.Open();
                    retValue = this.Delete(cn, criteria);
                }
                // No errors - commit transaction
                  if (Transaction.Current.TransactionInformation.Status != TransactionStatus.Active)  throw new Exception("Transaction has aborted."); scope.Complete();
            }// Already close and dispose sql connection.
            
            return retValue;
        }

        internal bool Delete(SqlConnection cn, Criteria criteria)
        {
            bool retValue = false;
            using (SqlCommand cm = cn.CreateCommand())
            {
                cm.CommandType = CommandType.StoredProcedure;
                cm.CommandText = "REFAddr_Delete";
                cm.Parameters.AddWithValue("@Option", criteria._option);
                cm.Parameters.AddWithValue("@AddrLinkType", criteria._addrLinkType);
                cm.Parameters.AddWithValue("@AddrLinkKey", criteria._addrLinkKey);
                cm.Parameters.AddWithValue("@AddrKey", criteria._addrKey);

                // Additional Parameter for Return Value From StoredProcedure -- Changed By Richard
                cm.Parameters.AddWithValue("@RetValue", 0);
                cm.Parameters["@RetValue"].Direction = ParameterDirection.Output;
                cm.ExecuteNonQuery();

                if ((int)cm.Parameters["@RetValue"].Value == (int)GEnum.SpState.Pass)
                    retValue = true;
                else
                {
                    throw new TAException(MsgID.Common.DeleteFail);
                }
            }// Already close and dispose sql command.
            return retValue;
        }

        #endregion Delete

    }
}
