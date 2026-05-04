using System;
using System.Data;
using System.Data.SqlClient;
using System.Transactions;

namespace BOLib
{
    [Serializable()]
    public class REFContactInfors : DataTable
    {

        #region Factory Methods

        
        public REFContactInfors()
        {
            this.Fetch(new Criteria(0, 1));
        }

        public REFContactInfors(SqlConnection cn)
        {
            this.Fetch(cn, new Criteria(0, 1));
        }
     
        public static REFContactInfors Get( int? headerKey)
        {
            
            REFContactInfors obj = new REFContactInfors();
            obj.Fetch(new Criteria(headerKey, 1));
            return obj;
        }

        public static REFContactInfors New()
        {
            
            REFContactInfors obj = new REFContactInfors();
            return obj;
        }
        public static REFContactInfors New(SqlConnection cn)
        {
           
            REFContactInfors obj = new REFContactInfors(cn);
            //obj.Fetch(cn,new Criteria(0, 1));
          
            return obj;
        }
        #endregion //Factory Methods

        #region Criteria

        [Serializable()]
        internal class Criteria
        {
            public int? _headerKey = null;
            public int? _option = null;

            public int? _uid = null;
            public int? _contactLinkType = null;
            public int? _contactLinkKey = null;
            public string _uids = null;
            internal Criteria()
            {
            }

            internal Criteria(int? HeaderKey, int? Option)
            {
                _headerKey = HeaderKey;
                _option = Option;
            }
            internal Criteria(int? Uid, int? ContactLinkType, int? ContactLinkKey, int? Option)
            {
                _uid = Uid;
                _contactLinkType = ContactLinkType;
                _contactLinkKey = ContactLinkKey;
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
            using (SqlCommand cm = cn.CreateCommand())
            {
                cm.CommandType = CommandType.StoredProcedure;
                cm.CommandText = "REFContactInfor_Get";

                cm.Parameters.AddWithValue("@Option", criteria._option);
                cm.Parameters.AddWithValue("@ContactLinkType", criteria._contactLinkType);
                cm.Parameters.AddWithValue("@ContactLinkKey", criteria._contactLinkKey);

                // Additional Parameter for Return Value From StoredProcedure -- Changed By Richard
                cm.Parameters.AddWithValue("@RetValue", 0);
                cm.Parameters["@RetValue"].Direction = ParameterDirection.Output;

                System.Data.SqlClient.SqlDataAdapter sqlAdp = new System.Data.SqlClient.SqlDataAdapter(cm);
                sqlAdp.Fill(this);

                if ((int)cm.Parameters["@RetValue"].Value == (int)GEnum.SpState.Pass)
                    return true;
                else
                    return false;
            }//using
            
        }

        #endregion //Data Access - Fetch

        #region Data Access - Insert

        internal bool Insert(Criteria _criteria )
        {
            bool retValue = false;
          
            
            using (TransactionScope scope = new TransactionScope())
            {
                // Create new sql connection for this method. 
                using (SqlConnection cn = new SqlConnection(Database.BossDemoConnection))
                {
                    // Open sql connection. 
                    cn.Open();
                    retValue = this.Insert(cn, _criteria);
                }
                // No errors - commit transaction
                  if (Transaction.Current.TransactionInformation.Status != TransactionStatus.Active)  throw new Exception("Transaction has aborted."); scope.Complete();
            }// Already close and dispose sql connection.
            
            return retValue;
        }

        internal bool Insert(SqlConnection cn, Criteria _criteria)
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
                    cm.CommandText = "REFContactInfor_AddUpdate";

                    cm.Parameters.AddWithValue("@Option", 0);
                    cm.Parameters.AddWithValue("@NewUid", 0);

                    cm.Parameters.AddWithValue("@UID", dr["UID"]);
                    cm.Parameters.AddWithValue("@ContactLinkType", _criteria._contactLinkType);
                    cm.Parameters.AddWithValue("@ContactLinkKey", _criteria._contactLinkKey);
                    cm.Parameters.AddWithValue("@ContactPerson", dr["ContactPerson"]);
                    cm.Parameters.AddWithValue("@ContactType", dr["ContactType"].ToString() == "" ? 10 : dr["ContactType"]);
                    cm.Parameters.AddWithValue("@ContactNum", dr["ContactNum"]);
                    cm.Parameters.AddWithValue("@Custom1", dr["Custom1"]);
                    cm.Parameters.AddWithValue("@Custom2", dr["Custom2"]);
                    cm.Parameters.AddWithValue("@Custom3", dr["Custom3"]);
                    cm.Parameters.AddWithValue("@CreateDate", DBNull.Value);
                    cm.Parameters.AddWithValue("@CreateUserKey", AppInfor.currentUserKey);
                    cm.Parameters.AddWithValue("@LastModifiedDate", DBNull.Value);
                    cm.Parameters.AddWithValue("@LastModifiedUserKey", AppInfor.currentUserKey);

                    cm.Parameters["@NewUid"].Direction = ParameterDirection.Output;

                    cm.Parameters.AddWithValue("@RetValue", 0);
                    cm.Parameters["@RetValue"].Direction = ParameterDirection.Output;
                    // Execute command.
                    cm.ExecuteNonQuery();

                    if ((int)cm.Parameters["@RetValue"].Value == (int)GEnum.SpState.Pass)
                    {
                        retValue = true;                        
                    }
                    else
                    {
                        retValue=false;
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
            bool retValue = false ;
            
            
            if (this.Rows.Count == 0)
            {
                retValue = true;
            }
            string strUpdateID = "";
            foreach (DataRow dr in this.Rows)
            {
                using (SqlCommand cm = cn.CreateCommand())
                {
                    cm.CommandType = CommandType.StoredProcedure;
                    cm.CommandText = "REFContactInfor_AddUpdate";
                    if (dr["UID"].ToString() == "")
                    {
                        cm.Parameters.AddWithValue("@Option", 0);
                    }
                    else
                    {
                        cm.Parameters.AddWithValue("@Option", 1);
                    }

                    strUpdateID = strUpdateID + dr["UID"].ToString() + ",";

                    cm.Parameters.AddWithValue("@NewUid", 0);
                    cm.Parameters.AddWithValue("@UID", dr["UID"]);
                    cm.Parameters.AddWithValue("@ContactLinkType", _criteria._contactLinkType);
                    cm.Parameters.AddWithValue("@ContactLinkKey", _criteria._contactLinkKey);
                    cm.Parameters.AddWithValue("@ContactPerson", dr["ContactPerson"]);
                    cm.Parameters.AddWithValue("@ContactType", dr["ContactType"].ToString() == "" ? 10 : dr["ContactType"]);
                    cm.Parameters.AddWithValue("@ContactNum", dr["ContactNum"]);
                    cm.Parameters.AddWithValue("@CreateDate", dr["CreateDate"]);
                    cm.Parameters.AddWithValue("@CreateUserKey", dr["CreateUserKey"]);
                    cm.Parameters.AddWithValue("@LastModifiedDate", DBNull.Value);
                    cm.Parameters.AddWithValue("@LastModifiedUserKey", AppInfor.currentUserKey);
                    cm.Parameters.AddWithValue("@Custom1", dr["Custom1"]);
                    cm.Parameters.AddWithValue("@Custom2", dr["Custom2"]);
                    cm.Parameters.AddWithValue("@Custom3", dr["Custom3"]);

                    cm.Parameters["@NewUid"].Direction = ParameterDirection.Output;

                    cm.Parameters.AddWithValue("@RetValue", 0);
                    cm.Parameters["@RetValue"].Direction = ParameterDirection.Output;
                    // cm.Parameters["@NewDocKey"].Direction = ParameterDirection.Output;
                    // Execute command.
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
           
            
            using (SqlCommand cm = cn.CreateCommand())
            {
                cm.CommandType = CommandType.StoredProcedure;
                cm.CommandText = "REFContactInfor_Delete";

                cm.Parameters.AddWithValue("@ContactLinkType", criteria._contactLinkType);
                cm.Parameters.AddWithValue("@ContactLinkKey", criteria._contactLinkKey);

                // Additional Parameter for Return Value From StoredProcedure -- Changed By Richard
                cm.Parameters.AddWithValue("@RetValue", 0);
                cm.Parameters["@RetValue"].Direction = ParameterDirection.Output;
                // Execute command.
                cm.ExecuteNonQuery();


                if ((int)cm.Parameters["@RetValue"].Value == (int)GEnum.SpState.Pass)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }// Already close and dispose sql command.
            
        }

        #endregion Delete
             

    }
}
