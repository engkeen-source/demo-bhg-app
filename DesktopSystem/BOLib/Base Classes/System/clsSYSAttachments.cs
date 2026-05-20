using System;
using System.Data;
using System.Data.SqlClient;
using Csla;
using Csla.Data;
using System.Transactions;

namespace BOLib
{
    [Serializable()]
    public class SYSAttachments : Csla.BusinessListBase<SYSAttachments, SYSAttachment>
    {

        #region Factory Methods

        internal bool _isDirty = false;
        public bool IsDirty
        {
            get
            {
                return this._isDirty;
            }
            set { this._isDirty = value; }
        }

        public SYSAttachments()
        {
        }

        internal static SYSAttachments New()
        {
            
            SYSAttachments obj = new SYSAttachments();
            
            return obj;
        }

        internal static SYSAttachments Get()
        {
            
            SYSAttachments obj = new SYSAttachments();
            obj.Fetch(new Criteria(0, 0, 0));
            return obj;
        }

        #endregion //Factory Methods

        #region Criteria

        [Serializable()]
        public class Criteria
        {
            public int? _docDC = null;
            public int? _docDK = null;         
            public int? _option = null;

            internal Criteria()
            {
            }

            public Criteria(int? DocDC, int? DocDK,int? Option)
            {
                _docDC = DocDC;
                _docDK = DocDK;                
                _option = Option;
            }
            
            //internal Criteria(int? DocDC, int? DocDK, int? DocDItm, int? DocDetailType, int? Seq, int? Option)
            //{
            //    _docDC = DocDC;
            //    _docDK = DocDK;
            //    _docDItm = DocDItm;
            //    _docDetailType = DocDetailType;
            //    _seq = Seq;
            //    _option = Option;
            //}
        }

        #endregion //Criteria

        #region Data Access - Fetch

        public bool Fetch(Criteria criteria)
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

        public bool Fetch(SqlConnection cn, Criteria criteria)
        {
            this.Clear();            
            
            using (SqlCommand cm = cn.CreateCommand())
            {
                cm.CommandType = CommandType.StoredProcedure;
                cm.CommandText = "SYSAttachment_Get";            
             
                cm.Parameters.AddWithValue("@Option", criteria._option);
                
                cm.Parameters.AddWithValue("@DocDC", criteria._docDC);
                cm.Parameters.AddWithValue("@DocDK", criteria._docDK);
                cm.Parameters.AddWithValue("@RetValue",0);
                cm.Parameters["@RetValue"].Direction = ParameterDirection.Output;
             
                using (SafeDataReader dr = new SafeDataReader(cm.ExecuteReader()))
                {
                    while (dr.Read())
                        this.Add(SYSAttachment.Get(dr));
                }

                
                if ((int)cm.Parameters["@RetValue"].Value == (int)GEnum.SpState.Pass)
                    return true;
                else
                    return false;    
            }//using            
        }

        public bool Copy(SqlConnection cn, Criteria criteria,int NewDocCodeKey,int NewDocKey)
        {

            using (SqlCommand cm = cn.CreateCommand())
            {
                cm.CommandType = CommandType.StoredProcedure;
                cm.CommandText = "SYSAttachment_Copy";

                cm.Parameters.AddWithValue("@Option", criteria._option);

                cm.Parameters.AddWithValue("@DocDC", criteria._docDC);
                cm.Parameters.AddWithValue("@DocDK", criteria._docDK);
                cm.Parameters.AddWithValue("@NewDocDC", NewDocCodeKey);
                cm.Parameters.AddWithValue("@NewDocDK", NewDocKey);
                cm.Parameters.AddWithValue("@RetValue", 0);
                cm.Parameters["@RetValue"].Direction = ParameterDirection.Output;

                using (SafeDataReader dr = new SafeDataReader(cm.ExecuteReader()))
                {
                    while (dr.Read())
                        this.Add(SYSAttachment.Get(dr));
                }


                if ((int)cm.Parameters["@RetValue"].Value == (int)GEnum.SpState.Pass)
                    return true;
                else
                    return false;
            }//using    
            

        }

        public bool Copy(Criteria criteria, int NewDocCodeKey, int NewDocKey)
        {
            using (SqlConnection cn = new SqlConnection(Database.BossDemoConnection))
            {
                // Open sql connection. 
                cn.Open();
                using (SqlCommand cm = cn.CreateCommand())
                {
                    cm.CommandType = CommandType.StoredProcedure;
                    cm.CommandText = "SYSAttachment_Copy";

                    cm.Parameters.AddWithValue("@Option", criteria._option);

                    cm.Parameters.AddWithValue("@DocDC", criteria._docDC);
                    cm.Parameters.AddWithValue("@DocDK", criteria._docDK);
                    cm.Parameters.AddWithValue("@NewDocDC", NewDocCodeKey);
                    cm.Parameters.AddWithValue("@NewDocDK", NewDocKey);
                    cm.Parameters.AddWithValue("@RetValue", 0);
                    cm.Parameters["@RetValue"].Direction = ParameterDirection.Output;

                    using (SafeDataReader dr = new SafeDataReader(cm.ExecuteReader()))
                    {
                        while (dr.Read())
                            this.Add(SYSAttachment.Get(dr));
                    }


                    if ((int)cm.Parameters["@RetValue"].Value == (int)GEnum.SpState.Pass)
                        return true;
                    else
                        return false;
                }//using    
            }
        }

        public bool CopyWithDMAS(SqlConnection cn,int? sourceDocCodekey, int? sourceDocKey, int? NewDocCodeKey, int? NewDocKey)
        {
            this.Clear();
            using (SqlCommand cm = cn.CreateCommand())
            {
                cm.CommandType = CommandType.StoredProcedure;
                cm.CommandText = "SYSAttachment_CopyWithDMAS";

                cm.Parameters.AddWithValue("@DocDC", NewDocCodeKey);
                cm.Parameters.AddWithValue("@DocDK", NewDocKey);
                cm.Parameters.AddWithValue("@SourceDC", sourceDocCodekey);
                cm.Parameters.AddWithValue("@SourceDK", sourceDocKey);
                cm.Parameters.AddWithValue("@SecGrp", SysOptionUtility.DatabaseBranchCode + "Sales");
                
                using (SafeDataReader dr = new SafeDataReader(cm.ExecuteReader()))
                {
                    while (dr.Read())
                        this.Add(SYSAttachment.Get(dr));
                }
                return true;
            }//using 
        }

        public bool CopyWithDMAS(int? sourceDocCodekey, int? sourceDocKey, int? NewDocCodeKey, int? NewDocKey)
        {
            using (SqlConnection cn = new SqlConnection(Database.BossDemoConnection))
            {
                // Open sql connection. 
                cn.Open();
                return CopyWithDMAS(cn, sourceDocCodekey, sourceDocKey, NewDocCodeKey, NewDocKey);
            }
        }

        public bool DeleteWithDMAS(SqlConnection cn)
        {
            //Saving or Deleting Attachments in Documents
            try
            {
                using (SqlCommand cm = cn.CreateCommand())
                {
                    cm.CommandType = CommandType.StoredProcedure;
                    cm.CommandText = "SYSAttachment_DeleteWithDMAS";
                    string xmlAttachment = GFunc.ConvertObjectToXML(this);
                    cm.Parameters.AddWithValue("@Attachment", xmlAttachment);
                    cm.Parameters.AddWithValue("@RetValue", 0);
                    cm.Parameters["@RetValue"].Direction = ParameterDirection.Output;
                    cm.ExecuteNonQuery();
                    if ((int)cm.Parameters["@RetValue"].Value == (int)GEnum.SpState.Pass)
                        return true;
                    else
                        return false;
                }                  
            }          
            catch (Exception ex)
            {
                throw (ex);
            }
        }//Completed

        public bool DeleteWithDMAS()
        {
            using (SqlConnection cn = new SqlConnection(Database.BossDemoConnection))
            {
                // Open sql connection. 
                cn.Open();
                return DeleteWithDMAS(cn);
            }
        }

        #endregion //Data Access - Fetch
    }
}
