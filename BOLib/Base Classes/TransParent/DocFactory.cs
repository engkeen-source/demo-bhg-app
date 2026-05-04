using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Transactions;

namespace BOLib
{
    public class DocFactory
    {
        internal GEnum.InstanceMode _instanceMode = GEnum.InstanceMode.Normal;
        internal int _guID = 0;
        internal bool _isError = false;
        internal GEnum.SystemCode _codeKey = 0;
        internal string _permID = "";
        internal bool approvalRequired = false;
        internal bool _reloadRefCombos = false;

        public bool ApprovalRequired
        {
            get
            {
                return this.approvalRequired;
            }
        }
        public bool IsError
        {
            get
            {
                return _isError;
            }
        }
        public int CodeKey
        {
            get
            {
                return (int)_codeKey;
            }
        }
        public string PermID
        {
            get
            {
                return _permID;
            }
        }
        internal GEnum.InstanceMode InstanceMode
        {
            get
            {
                return this._instanceMode;
            }
        }
        public bool ReloadRefCombo
        {
            get
            {
                return _reloadRefCombos;
            }
        }

        public bool Initialisation(out bool isReadonly)
        {
            try
            {
                if (SECPermUtility.Any(_permID, out isReadonly, true) == false)
                    return false;

                using (TransactionScope scope = new TransactionScope())
                {
                    using (SqlConnection cn = new SqlConnection(Database.BossDemoConnection))
                    {
                        cn.Open();

                        using (SqlCommand cm = cn.CreateCommand())
                        {
                            cm.CommandType = CommandType.StoredProcedure;
                            cm.CommandText = "DocFactory_Initialization";

                            cm.Parameters.AddWithValue("@CodeKey", _codeKey);
                            cm.Parameters.AddWithValue("@LoginUserKey", AppInfor.currentUserKey);
                            cm.Parameters.AddWithValue("@LockingGUID", _guID);                            
                            cm.Parameters.AddWithValue("@ReloadRefCombos", _reloadRefCombos);
                            cm.Parameters["@LockingGUID"].Direction = ParameterDirection.Output;                           
                            cm.Parameters["@ReloadRefCombos"].Direction = ParameterDirection.Output;

                            cm.ExecuteNonQuery();

                            _guID = GFunc.NEInt(cm.Parameters["@LockingGUID"].Value,0);                         
                            _reloadRefCombos = GFunc.NEBool(cm.Parameters["@ReloadRefCombos"].Value, false);

                            scope.Complete();
                        }
                    }
                }
                return true;
            }            
            catch (Exception ex)
            {
                throw (ex);
            }
        }
    }
}
