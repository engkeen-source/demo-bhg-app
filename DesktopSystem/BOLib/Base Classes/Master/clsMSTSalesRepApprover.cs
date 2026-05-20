using System;
using System.Data;
using System.Data.SqlClient;
using Csla;
using Csla.Data;
using Csla.Validation;
using System.Collections;
using System.Transactions;

namespace BOLib
{
    public class MSTSalesRepApprover : Csla.BusinessBase<MSTSalesRepApprover>, System.ComponentModel.IDataErrorInfo
    {
        #region Business Properties and Methods

        //declare members
        internal int? _saleRepKey = null;
        internal int? _approverKey = null;
        internal string _saleRep = string.Empty;
        internal string _approver = string.Empty;
        internal decimal? _saleLimit = null;
        internal int? _profitMarginLimit = null;


        internal string _error = string.Empty;

        public int? SaleRepKey
        {
            get
            {
                return _saleRepKey;
            }
            set
            {
                _saleRepKey = value;
                PropertyHasChanged("SaleRepKey");
            }
        }

        public int? ApproverKey
        {
            get
            {
                return _approverKey;
            }
            set
            {
                _approverKey = value;
                PropertyHasChanged("ApproverKey");
            }
        }
   
        public string SaleRep
        {
            get
            {
                return _saleRep;
            }
            set
            {
                _saleRep = value;
                PropertyHasChanged("SaleRep");
            }
        }
        public string Approver
        {
            get
            {
                return _approver;
            }
            set
            {
                _approver = value;
                PropertyHasChanged("Approver");
            }
        }

        public decimal? SaleLimit
        {
            get
            {
                return _saleLimit;
            }
            set
            {
                _saleLimit = value;
                PropertyHasChanged("SaleLimit");
            }
        } 
        
        public int? ProfitMarginLimit
        {
            get
            {
                return _profitMarginLimit;
            }
            set
            {
                _profitMarginLimit = value;
                PropertyHasChanged("ProfitMarginLimit");
            }
        }

        public string Error
        {
            get
            {
                return _error;
            }
            set
            {

                _error = value;

            }
        }

        protected override object GetIdValue()
        {
            return _saleRepKey.ToString() + _approverKey.ToString();
        }

        #endregion //Business Properties and Methods

        #region Factory Methods

        internal MSTSalesRepApprover()
        { /* require use of factory method */ }

        internal static MSTSalesRepApprover New()
        {

            MSTSalesRepApprover child = new MSTSalesRepApprover();

            return child;
        }

        internal static MSTSalesRepApprover NewChild()
        {

            MSTSalesRepApprover child = new MSTSalesRepApprover();
            child.ValidationRules.CheckRules();
            child.MarkAsChild();

            return child;
        }

        internal static MSTSalesRepApprover Get(SafeDataReader dr)
        {

            MSTSalesRepApprover child = new MSTSalesRepApprover();
            child.MarkAsChild();
            child.Fetch(dr);
            return child;
        }

        internal static MSTSalesRepApprover Get(int? eMKey, int? transKey)
        {

            MSTSalesRepApprover child = new MSTSalesRepApprover();
            child.Fetch(new Criteria(eMKey, transKey, 1));
            return child;
        }

        #endregion //Factory Methods

        #region Criteria

        [Serializable()]
        internal class Criteria
        {
            public int? _saleRepKey = null;
            public int? _approverKey = null;
            public int? _option = null;

            internal Criteria()
            {
            }

            internal Criteria(int? SaleRepKey, int? ApproverKey)
            {
                _saleRepKey = SaleRepKey;
                _approverKey = ApproverKey;
            }

            internal Criteria(int? SaleRepKey, int? ApproverKey, int? Option)
            {
                _saleRepKey = SaleRepKey;
                _option = Option;

                if (ApproverKey == null)
                    _approverKey = 0;
                else
                    _approverKey = ApproverKey;
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
            }

            return retValue;
        }

        internal bool Fetch(SqlConnection cn, Criteria criteria)
        {
            bool retValue = false;

            // Using existing sql connection.
            using (SqlCommand cm = cn.CreateCommand())
            {
                cm.CommandType = CommandType.StoredProcedure;
                cm.CommandText = "MSTSalesRepApprovers_Get";
                cm.Parameters.AddWithValue("@Option", criteria._option);
                cm.Parameters.AddWithValue("@EMKey", criteria._saleRepKey);                
                cm.Parameters.AddWithValue("@RetValue", 0);
                cm.Parameters["@RetValue"].Direction = ParameterDirection.Output;

                // Using data reader as record set.
                using (SafeDataReader dr = new SafeDataReader(cm.ExecuteReader()))
                {
                    // If data reader can read, continue...
                    while (dr.Read())
                    {
                        retValue = this.Fetch(dr);
                    }
                }	// Already close and dispose data reader.

                if ((int)cm.Parameters["@RetValue"].Value == (int)GEnum.SpState.Pass)
                {
                    retValue = true;
                }
                else
                {
                    retValue = false;
                }
            }// Already close and dispose sql connection.

            return retValue;
        }

        internal bool Fetch(SafeDataReader dr)
        {
            _saleRepKey = dr.GetInt32("SaleRepKey");
            _approverKey = dr.GetInt32("ApproverKey");
            _saleRep = dr.GetString("SaleRep");
            _approver = dr.GetString("Approver");
            _saleLimit = dr.GetDecimal("SaleLimit");
            _profitMarginLimit = dr.GetInt32("ProfitMarginLimit");
            
            ValidationRules.CheckRules();
            return true;
        }
        #endregion //Data Access - Fetch

        #region Data Access - Insert

        internal bool Insert()
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
                    retValue = this.Insert(cn);
                }// End of SqlConnection

                // No errors - commit transaction
                if (Transaction.Current.TransactionInformation.Status != TransactionStatus.Active) throw new Exception("Transaction has aborted."); scope.Complete();
            }// End of TransactionScope

            return retValue;
        }

        internal bool Insert(SqlConnection cn)
        {
            
            // Using existing sql connection.
            using (SqlCommand cm = cn.CreateCommand())
            {
                cm.CommandType = CommandType.StoredProcedure;
                cm.CommandText = "MSTSalesRepApprover_AddUpdate";
                cm.Parameters.AddWithValue("@Option", 0);
                cm.Parameters.AddWithValue("@RetValue", 0);
                cm.Parameters["@RetValue"].Direction = ParameterDirection.Output;
                
                if (_saleRepKey == null)
                    cm.Parameters.AddWithValue("@SaleRepKey", DBNull.Value);
                else
                    cm.Parameters.AddWithValue("@SaleRepKey", _saleRepKey);

                if (_approverKey == null)
                    cm.Parameters.AddWithValue("@ApproverKey", DBNull.Value);
                else
                    cm.Parameters.AddWithValue("@ApproverKey", _approverKey);

                if (_saleLimit == null)
                    cm.Parameters.AddWithValue("@SaleLimit", DBNull.Value);
                else
                    cm.Parameters.AddWithValue("@SaleLimit", _saleLimit);

                if (_profitMarginLimit == null)
                    cm.Parameters.AddWithValue("@ProfitMarginLimit", DBNull.Value);
                else
                    cm.Parameters.AddWithValue("@ProfitMarginLimit", _profitMarginLimit);

                

                cm.ExecuteNonQuery();
                
                if ((int)cm.Parameters["@RetValue"].Value == (int)GEnum.SpState.Pass)
                    return true;
                else
                    return false;
            }// Already close and dispose sql connection.

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
                if (Transaction.Current.TransactionInformation.Status != TransactionStatus.Active) throw new Exception("Transaction has aborted."); scope.Complete();
            }// End of TransactionScope

            return retValue;
        }

        internal bool Update(SqlConnection cn)
        {
            // Using existing sql connection.
            using (SqlCommand cm = cn.CreateCommand())
            {
                cm.CommandType = CommandType.StoredProcedure;
                cm.CommandText = "MSTSalesRepApprover_AddUpdate";
                cm.Parameters.AddWithValue("@Option", 1);
                cm.Parameters.AddWithValue("@RetValue", 0);
                cm.Parameters["@RetValue"].Direction = ParameterDirection.Output;

                if (_saleRepKey == null)
                    cm.Parameters.AddWithValue("@SaleRepKey", DBNull.Value);
                else
                    cm.Parameters.AddWithValue("@SaleRepKey", _saleRepKey);

                if (_approverKey == null)
                    cm.Parameters.AddWithValue("@ApproverKey", DBNull.Value);
                else
                    cm.Parameters.AddWithValue("@ApproverKey", _approverKey);

                if (_saleLimit == null)
                    cm.Parameters.AddWithValue("@SaleLimit", DBNull.Value);
                else
                    cm.Parameters.AddWithValue("@SaleLimit", _saleLimit);

                if (_profitMarginLimit == null)
                    cm.Parameters.AddWithValue("@ProfitMarginLimit", DBNull.Value);
                else
                    cm.Parameters.AddWithValue("@ProfitMarginLimit", _profitMarginLimit);

                cm.ExecuteNonQuery();

                if ((int)cm.Parameters["@RetValue"].Value == (int)GEnum.SpState.Pass)
                    return true;
                else
                    return false;
            }// Already close and dispose sql connection.

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
                if (Transaction.Current.TransactionInformation.Status != TransactionStatus.Active) throw new Exception("Transaction has aborted."); scope.Complete();
            }// End of TransactionScope

            return retValue;
        }

        internal bool Delete(SqlConnection cn, Criteria criteria)
        {
            // Using existing sql connection.
            using (SqlCommand cm = cn.CreateCommand())
            {
                cm.CommandType = CommandType.StoredProcedure;
                cm.CommandText = "MSTSalesApprover_Delete";
                cm.Parameters.AddWithValue("@RetValue", 0);
                cm.Parameters["@RetValue"].Direction = ParameterDirection.Output;

                cm.Parameters.AddWithValue("@EMKey", criteria._saleRepKey);
                cm.Parameters.AddWithValue("@ApproverKey", criteria._approverKey);
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
