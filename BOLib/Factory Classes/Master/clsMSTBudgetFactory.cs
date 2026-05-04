using System;
using System.Data;
using System.Data.SqlClient;
using Csla;
using Csla.Data;
using System.Transactions;
using System.ComponentModel;
using System.Collections.Generic;
using System.Collections;
using System.Linq;
using Infragistics.Win.UltraWinGrid;
using TAUtil;

namespace BOLib
{
    [Serializable()]
    public class MSTBudgetFactory : CommandBase
    {
        #region Member variables and constants

        private MSTBudget _MSTBudget = null; 
        private MSTBudgets _MSTBudgets = null;
        private MSTBudgets _MSTPrevBudgets = null;
        private MSTAcc _MSTAcc = null;        

        private GEnum.InstanceMode _instanceMode = GEnum.InstanceMode.Normal;
        private bool _isDirty = false;
        private bool _isValid = false;
        private bool _isOpenReadOnly = false;
        private int _guID = 0;
        private GEnum.BudgetModeValue _budgetMode;

        private decimal _amountRatio = 1.00M;
        private decimal _addAmount = 0;

        private decimal _unitRatio = 1.00M;
        private decimal _addUnit = 0;

        //Copy 
        private int _budgettype = 0;
        private int _selectBudgetRecKey = 0;
        private int _selectBudgetRecSubKey = 0;
        private int _targetBudgetRecKey = 0;
        private int _BudgetRecSubKey = 0; 
        private string _fromBranchID = string.Empty;
        private string _toBranchID = string.Empty;
        private string _fromDeptID = string.Empty;
        private string _toDeptID = string.Empty;
        private int _fromBudgetPeriod = DateTime.Today.Year * 100 + DateTime.Today.Month;
        private int _toBudgetPeriod = (DateTime.Today.Year + 1) * 100 + DateTime.Today.Month - 1;
        private int _budgetItmMode = 0;

        // System Code Key for this Factory.
        private const GEnum.SystemCode constCodeKey = GEnum.SystemCode.Budget;
        public GEnum.SystemCode ConstantCodeKey { get { return constCodeKey; } }

        // Permission ID for this Factory.
        public const string constPermID = GVar.PermissionID.Account_Budget;

        // Error Event Declaration
        public GVar.ErrorEvent errorEvent = null;

        // Dirty Event Declaration
        public GVar.DirtyEvent dirtyEvent = null;

        // ReadOnly Event Declaration
        public GVar.ReadOnlyEvent readonlyEvent = null;

        #endregion // Member variables and constant

        #region Factory Properties
        public MSTBudget ObjMSTBudget
        {
            get
            {
                return this._MSTBudget;
            }
            set
            {
                this._MSTBudget = value;
            }
        }
        public MSTBudgets ObjMSTBudgets
        {
            get
            {
                return this._MSTBudgets;
            }
        }
        public MSTBudgets ObjPrevMSTBudgets
        {
            get
            {
                return this._MSTPrevBudgets;
            }
        }
        internal GEnum.InstanceMode InstanceMode
        {
            get
            {
                return this._instanceMode;
            }
        }

        public bool IsDirty
        {
            get
            {
                return this._isDirty;
            }
            set
            {
                _isDirty = value;
            }
        }
        public bool IsValid
        {
            get
            {
                return this._isValid;
            }
        }
        public bool IsOpenReadOnly
        {
            get
            {
                return this._isOpenReadOnly;
            }
        }
        public int GUID
        {
            get
            {
                return this._guID;
            }
        }
        public decimal AmountRatio
        {
            get
            {
                return this._amountRatio;
            }
            set
            {
                this._amountRatio = value;
            }
        }
        public decimal AddAmount
        {
            get
            {
                return this._addAmount;
            }
            set
            {
                this._addAmount = value;
            }
        }

        public decimal UnitRatio
        {
            get
            {
                return this._unitRatio;
            }
            set
            {
                this._unitRatio = value;
            }
        }
        public decimal AddUnit
        {
            get
            {
                return this._addUnit;
            }
            set
            {
                this._addUnit = value;
            }
        }
        public int BudgetType
        {
            get
            {
                return this._budgettype;
            }
            set
            {
                this._budgettype = value;
            }
        }
        public int SelectBudgetRecKey
        {
            get
            {
                return this._selectBudgetRecKey;
            }
            set
            {
                this._selectBudgetRecKey = value;
            }
        }
        public int SelectBudgetRecSubKey
        {
            get
            {
                return this._selectBudgetRecSubKey;
            }
            set
            {
                this._selectBudgetRecSubKey = value;
            }
        }
        public int TargetBudgetRecKey
        {
            get
            {
                return this._targetBudgetRecKey;
            }
            set
            {
                this._targetBudgetRecKey = value;
            }
        }
        public int BudgetRecSubKey
        {
            get
            {
                return this._BudgetRecSubKey;
            }
            set
            {
                this._BudgetRecSubKey = value;
            }
        }
        public string FromBranchID
        {
            get
            {
                return this._fromBranchID; 
            }
            set
            {
                this._fromBranchID = value;
            }
        }
        public string ToBranchID
        {
            get
            {
                return this._toBranchID;
            }
            set
            {
                this._toBranchID = value;
            }
        }
        public string FromDeptID
        {
            get 
            {
                return this._fromDeptID;
            }
            set
            {
                this._fromDeptID = value;
            }
        }
        public string ToDeptID
        {
            get
            {
                return this._toDeptID;
            }
            set 
            {
                this._toDeptID = value;
            }
        }
        public int FromBudgetPeriod
        {
            get
            {
                return this._fromBudgetPeriod;
            }
            set 
            {
                this._fromBudgetPeriod = value;
            }
        }
        public int ToBudgetPeriod
        {
            get
            {
                return this._toBudgetPeriod;
            }
            set
            {
                this._toBudgetPeriod = value;
            }
        }
        public int BudgetItmMode
        {
            get
            {
                return this._budgetItmMode;
            }
            set
            {
                this._budgetItmMode = value;
            }
        }
        public string ErrorMessageID
        {
            get;
            set;
        }
        #endregion // Constructors

        //Constructors, Initialisation
        public MSTBudgetFactory(GEnum.InstanceMode instanceMode)
        {
            try
            {
                this._instanceMode = instanceMode;
                this.Initialisation(instanceMode);
            }
            catch (TAException tex)
            {
                throw Error(tex);
            }
            catch (Exception ex)
            {
                throw Error(ex);
            }            
        }
        public bool Initialisation(GEnum.InstanceMode instanceMode)
        {
            bool isInitialisation = false;
            string msgID = MsgID.Common.InitialisationFail;

            if (this.InstanceMode == GEnum.InstanceMode.Normal)
            {
                try
                {                    
                   // Check Permission
                   if (!SECPermUtility.Any(constPermID, out this._isOpenReadOnly, true))
                       return false;

                    // Create TransactionScope
                    using (TransactionScope scope = new TransactionScope())
                    {
                       // Create SqlConnection
                       using (SqlConnection cn = new SqlConnection(Database.BossDemoConnection))
                       {
                           // Open Connection
                           cn.Open();

                           // Get Instance GUID
                           if ((this._guID=SysOptionUtility.GetNewLockingGUID(cn))==0)
                           {
                               this._guID = -1;
                               return false;
                           }

                           // Locking
                           if (SysLockUtility.IsProcessLock(cn, true, GEnum.SysLockOption.ByCodKey, constCodeKey, this._guID))
                           {
                               this._guID = -1;
                               return false;
                           }

                           // Add Inprogress Lock
                           if (!SysLockUtility.AddInprogressLock(cn, true, this._guID, constCodeKey))
                           {
                               this._guID = -1;
                               return false;
                           }

                           //// Call New for Header
                           this._MSTBudget = MSTBudget.New();
                           this._MSTBudgets = MSTBudgets.New(cn);
                           this._MSTPrevBudgets = MSTBudgets.New(cn);

                           //// Commit Process
                           //this._MSTBudgets.ListChanged += new System.ComponentModel.ListChangedEventHandler(ObjMSTBudget_ListChanged);

                           this._instanceMode = GEnum.InstanceMode.Normal;
                           this._isOpenReadOnly = false;
                           msgID = string.Empty;
                           isInitialisation = true;

                           // No errors - commit transaction
                             if (Transaction.Current.TransactionInformation.Status != TransactionStatus.Active)  throw new Exception("Transaction has aborted."); scope.Complete();
                       }
                    }
                   
                }
                catch (TAException tex)
                {
                    throw Error(tex);
                }
                catch (Exception ex)
                {
                   
                    throw Error(ex);
                }
            }
            else
            {
                this._guID = 0;
                this._instanceMode = GEnum.InstanceMode.InternalCall;
                this._isOpenReadOnly = false;
                msgID = string.Empty;
                isInitialisation = true;
            }
            return isInitialisation;
        }

        //Methods
        public bool GetEdit( int? budgetType, int? budgetBranchKey, int? budgetDeptKey, int? budgetRecKey, int? budgetRecSubKey,
            int? periodFrom, int? periodTo,bool checkLock)
        {            
            // Initialisation
            bool isGetEdit = false;
            string msgID = MsgID.Common.GetFail;

            // Copy original object
            BOLib.MSTBudget copyMSTBudget = null;
            BOLib.MSTBudgets copyMSTBudgets = null;
            BOLib.MSTBudgets copyMSTPrevBudgets = null;

            try
            {
                if (!GFunc.IsNE(this._MSTBudget))
                    copyMSTBudget = this._MSTBudget.Clone();
                else
                    this._MSTBudget = MSTBudget.New();

                if (!GFunc.IsNE(this._MSTBudgets))
                    copyMSTBudgets = GFunc.TACopyDataTable(_MSTBudgets);
                else
                    this._MSTBudgets = MSTBudgets.New();

                if (!GFunc.IsNE(this._MSTPrevBudgets))
                    copyMSTPrevBudgets = GFunc.TACopyDataTable(_MSTPrevBudgets);
                else
                    this._MSTPrevBudgets = MSTBudgets.New();

                try
                {
                    // Check Permission
                    if (!SECPermUtility.Edit(constPermID, true))
                        return false;

                    // Create TransactionScope
                    using (TransactionScope scope = new TransactionScope())
                    {
                        // Create SqlConnection
                        using (SqlConnection cn = new SqlConnection(Database.BossDemoConnection))
                        {
                            // Open Connection
                            cn.Open();

                            if (checkLock)
                            {
                                // Check Lock
                                if (SysLockUtility.IsLock(cn, true, GEnum.SysLockOption.ByCodeKeyandDataKey, constCodeKey, budgetBranchKey, 0, _guID))
                                    return false;

                                bool reval = true;
                                switch ((int)budgetType)
                                {
                                    case (int)GEnum.BudgetType.Document_Group_Sales:
                                    case (int)GEnum.BudgetType.Document_Group_Purchase:
                                    case (int)GEnum.BudgetType.Document_Group_and_Item_Sales:
                                    case (int)GEnum.BudgetType.Document_Group_Item_Purchases:
                                        reval = !SysLockUtility.IsLock(cn, true, GEnum.SysLockOption.ByCodeKeyandDataKey, constCodeKey, budgetRecSubKey, 0, _guID);
                                        break;
                                    default:
                                        reval = !SysLockUtility.IsLock(cn, true, GEnum.SysLockOption.ByCodeKeyandDataKey, constCodeKey, budgetRecKey, 0, _guID);
                                        break;
                                }

                                return reval; 
                                
                                // Remove Lock
                                if (!SysLockUtility.RemoveLockGUIDKeepIP(cn, true, this._guID, constCodeKey))
                                    return false;

                                // Add Lock
                                if (!SysLockUtility.AddLock(cn, true, this._guID, constCodeKey, budgetRecKey))
                                    return false;
                            }

                            #region calculation for period year and mth 
                            int prevPeriodFromYear = periodFrom.Value / 100 - 1;
                            int prevPeriodFromMth = periodFrom.Value % 100;

                            if (prevPeriodFromMth.ToString().Length == 1)
                            {
                                prevPeriodFromYear = int.Parse(prevPeriodFromYear.ToString() + 0 + prevPeriodFromMth.ToString());
                            }
                            else 
                            {
                                prevPeriodFromYear = int.Parse(prevPeriodFromYear.ToString() + prevPeriodFromMth.ToString());
                            }

                            int prevPeriodToYear = periodTo.Value/100 -1;
                            int prevPeriodToMth= periodTo.Value%100;

                            if (prevPeriodToMth.ToString().Length == 1)
                            {
                                prevPeriodToYear = int.Parse(prevPeriodToYear.ToString() + 0 + prevPeriodToMth.ToString());
                            }
                            else
                            {
                                prevPeriodToYear = int.Parse(prevPeriodToYear.ToString() + prevPeriodToMth.ToString());
                            }
                            #endregion

                            // Get Record for current year
                            this._MSTBudgets.Clear();

                            if (!this._MSTBudgets.Fetch(cn, new MSTBudgets.Criteria(budgetType, budgetBranchKey, budgetDeptKey,
                                    budgetRecKey, budgetRecSubKey, periodFrom, periodTo, 2)))
                            {
                                MsgBox.Show(cn, msgID);
                                return false;
                            }

                            if (_MSTBudgets.Rows.Count > 0)
                            {
                                IDataReader dr = _MSTBudgets.CreateDataReader();
                                if(dr.Read())
                                    _MSTBudget.Fetch(dr);//Copy to Header object from the first Row 
                            }
                            
                            //Get Record for previous year
                            DataTable copyCurrent=(MSTBudgets)_MSTBudgets.Copy();
                            foreach (DataRow dr in copyCurrent.Rows)
                            {
                                int currPeriod=(int)dr["BudgetPeriod"] ;
                                dr["BudgetPeriod"] = (currPeriod / 100 - 1) * 100 + currPeriod % 100; //decrease one year
                                dr["PeriodText"] = new DateTime(currPeriod / 100-1, currPeriod % 100, 1).ToString("yyyy MMM");
                                dr["BudgetAmountH"] = 0;
                                dr["BudgetQty"] = 0;
                                dr["BudgetWeight"] = 0;                           
                            }

                            _MSTPrevBudgets.Rows.Clear();
                            if (!this._MSTPrevBudgets.Fetch(cn, new MSTBudgets.Criteria(budgetType, budgetBranchKey, budgetDeptKey, budgetRecKey, budgetRecSubKey,
                                   prevPeriodFromYear, prevPeriodToYear, 2)))
                            {
                                MsgBox.Show(cn,msgID);
                                return false;
                            }
                            copyCurrent.PrimaryKey=new DataColumn[]{copyCurrent.Columns["BudgetPeriod"]};

                            foreach (DataRow dr in _MSTPrevBudgets.Rows)
                            {
                                DataRow drCopy=copyCurrent.Rows.Find(dr["BudgetPeriod"]);
                                if (drCopy != null)
                                {
                                    drCopy.Delete();
                                    copyCurrent.ImportRow(dr);
                                }
                            }
                            copyCurrent.AcceptChanges();
                            _MSTPrevBudgets = (MSTBudgets)copyCurrent;
                            // Commit Process
                            this._MSTBudgets.ColumnChanged += new DataColumnChangeEventHandler(_MSTBudgets_ColumnChanged);

                            this._isDirty = false;
                            this._isOpenReadOnly = false;
                            msgID = string.Empty;
                            isGetEdit = true;

                            // No errors - commit transaction
                              if (Transaction.Current.TransactionInformation.Status != TransactionStatus.Active)  throw new Exception("Transaction has aborted."); scope.Complete();

                            // Set Null to Backup Objects
                            copyMSTBudget = null;
                        }
                    }
            }
            catch (TAException tex)
            {
                throw Error(tex);
            }
            catch (Exception ex)
            {
                throw Error(ex);
            }

            
            }                
            catch (TAException tex)
            {
                // Restore data when error is occur                    
                this._MSTBudget = copyMSTBudget;
                this._MSTBudgets = copyMSTBudgets;
                this._MSTPrevBudgets = copyMSTPrevBudgets;
                throw Error(tex);
            }
            catch (Exception ex)
            {
                
                // Restore data when error is occur                    
                this._MSTBudget = copyMSTBudget;
                this._MSTBudgets = copyMSTBudgets;
                this._MSTPrevBudgets = copyMSTPrevBudgets;
                throw Error(ex);
            }            
            return isGetEdit;
        }
        public bool GetReadOnly(int? budgetType, int? budgetBranchKey, int? budgetDeptKey, int? budgetRecKey, int? budgetRecSubKey, int? periodFrom, int? periodTo)
        {
            bool isGetReadOnly = false;
            string msgID = MsgID.Common.GetFail;

            BOLib.MSTBudget copyMSTBudget = this._MSTBudget.Clone(); 
            
                try
                {
                    // Check Permission
                    if (!SECPermUtility.Read(constPermID, true))
                        return false;

                    // Create TransactionScope
                    using (TransactionScope scope = new TransactionScope())
                    {
                        // Create SqlConnection
                        using (SqlConnection cn = new SqlConnection(Database.BossDemoConnection))
                        {
                            // Open Connection
                            cn.Open();                                

                            // Remove all locks by GUID except inprogress Locking
                            if (!SysLockUtility.RemoveLockGUIDKeepIP(cn, true, GUID, constCodeKey))
                                return false;

                            // Call Record
                            if (!this._MSTBudgets.Fetch(cn, new MSTBudgets.Criteria(budgetType, budgetBranchKey, budgetDeptKey, budgetRecKey, budgetRecSubKey, periodFrom, periodTo, 2)))
                            {
                                throw new TAException(msgID);
                            }

                            int? diff = periodTo / 100 - periodFrom / 100;
                            //Get Record
                            if (!this._MSTPrevBudgets.Fetch(cn, new MSTBudgets.Criteria(budgetType, budgetBranchKey, budgetDeptKey, budgetRecKey, budgetRecSubKey, ((periodFrom / 100) - diff) * 100 + periodFrom % 100, ((periodTo / 100) - diff) * 100 + periodFrom % 100, 2)))
                            {
                                MsgBox.Show(cn,msgID);
                                return false;
                            }

                            //Set Budget Mode
                            this.SetBudgetMode(cn, (int)budgetType, (int)budgetRecKey);
                       
                            if (_MSTBudgets.Rows.Count > 0)
                            {
                                IDataReader dr = _MSTBudgets.CreateDataReader();
                                if (dr.Read())
                                    _MSTBudget.Fetch(dr);
                            }

                            this._isDirty = false;
                            this._isOpenReadOnly = true;
                            msgID = string.Empty;
                            isGetReadOnly = true;

                            // No errors - commit transaction
                              if (Transaction.Current.TransactionInformation.Status != TransactionStatus.Active)  throw new Exception("Transaction has aborted."); scope.Complete();

                            // Set Null to Backup Objects
                            copyMSTBudget = null;
                        }
                    }
                }
                catch (TAException tex)
                {
                    // Restore data when error is occur                    
                    this._MSTBudget = copyMSTBudget;
                    throw Error(tex);
                }
                catch (Exception ex)
                {
                   
                    // Restore data when error is occur                    
                    this._MSTBudget = copyMSTBudget;
                    throw Error(ex);
                }
           
            return isGetReadOnly;
        }
        public bool Clear()
        {
            bool isClear = false;
            string msgID = MsgID.Common.NewFail;

            BOLib.MSTBudget copyMSTBudget = null; 
            BOLib.MSTBudgets copyMSTBudgets = null;
            BOLib.MSTBudgets copyMSTPrevBudgets = null;
       
            try
            {
                if (!GFunc.IsNE(this._MSTBudget))
                    copyMSTBudget = this._MSTBudget.Clone();

                //detail data Table
                if (!GFunc.IsNE(this._MSTBudgets))
                    copyMSTBudgets = this._MSTBudgets;
                else
                {
                    this._MSTBudgets = new MSTBudgets();
                }

                //detail data Table
                if (!GFunc.IsNE(this._MSTPrevBudgets))
                    copyMSTPrevBudgets= this._MSTPrevBudgets;
                else
                {
                    this._MSTPrevBudgets = new MSTBudgets();
                }

                // Check Security Permission 
                if (!SECPermUtility.Any(constPermID, out this._isOpenReadOnly, true))
                {
                    return false;
                }
                
                // Create TransactionScope
                using (TransactionScope scope = new TransactionScope())
                {
                    // Create SqlConnection
                    using (SqlConnection cn = new SqlConnection(Database.BossDemoConnection))
                    {
                        // Open Connection
                        cn.Open();

                        // Remove all locks by GUID except inprogress Locking
                        if (!SysLockUtility.RemoveLockGUIDKeepIP(cn, true, GUID, constCodeKey))
                            return false;

                        //// Call New
                        this._MSTBudget = MSTBudget.New();
                        this._MSTBudgets = MSTBudgets.New();
                        this._MSTPrevBudgets = MSTBudgets.New();
                        
                        this._MSTBudgets.ColumnChanged+=new DataColumnChangeEventHandler(_MSTBudgets_ColumnChanged);

                        this._isDirty = false;
                        this._isOpenReadOnly = false;
                        msgID = string.Empty;
                        isClear = true;

                        // No errors - commit transaction
                          if (Transaction.Current.TransactionInformation.Status != TransactionStatus.Active)  throw new Exception("Transaction has aborted."); scope.Complete();
                    }
                }
            }
            catch (TAException tex)
            {
                // Restore data when error is occur        
                this._MSTBudget = copyMSTBudget;
                this._MSTBudgets = copyMSTBudgets;
                this._MSTPrevBudgets = copyMSTPrevBudgets;
                throw Error(tex);
            }
            catch (Exception ex)
            {                
                // Restore data when error is occur                    
                this._MSTBudget = copyMSTBudget;
                this._MSTBudgets = copyMSTBudgets;
                this._MSTPrevBudgets = copyMSTPrevBudgets;
                throw Error(ex);
            }            
            return isClear;
        }//Completed
        public bool Save(int _budgetRecKey,GEnum.BudgetItemMode _budgetItemMode)
        {
            bool isSave = false;

            ErrorMessageID = MsgID.Common.AddFail;

            bool isCommitTransFail = true;
            string recordID = string.Empty;

            
            try
            {
                if (this.IsOpenReadOnly)
                {
                    MsgBox.Show(MsgID.Common.RecordIsReadOnly);
                    return false;
                }
                else
                {
                    if (!SECPermUtility.Edit(constPermID, true))
                    { return false; }
                }

                // Create TransactionScope                        
                using (TransactionScope scope = new TransactionScope())
                {
                    // Create SqlConnection
                    using (SqlConnection cn = new SqlConnection(Database.BossDemoConnection))
                    {
                        // Open Connection
                        cn.Open();

                        //Set Budget Mode 
                        this.SetBudgetMode(cn, (int)_MSTBudget._budgetType, (int)_budgetRecKey);

                        //Check Missing Value For Field.
                        SetMissingValues();

                        #region Validation
                        if (Validation_Detail(cn) == false)
                            return false;

                        #endregion

                        //Add New Record                                   
                        foreach (DataRow dr in _MSTBudgets.Rows)
                        {
                            MSTBudget objCurrent = new MSTBudget();
                            objCurrent._budgetType = _MSTBudget._budgetType;
                            objCurrent._budgetBranchKey= _MSTBudget._budgetBranchKey;
                            objCurrent._budgetDeptKey = _MSTBudget._budgetDeptKey;
                            objCurrent._budgetRecKey = _budgetRecKey;
                            objCurrent._budgetRecSubKey = _MSTBudget._budgetRecSubKey;
                            objCurrent._budgetMode =(int) this._budgetMode;
                            objCurrent._budgetItmMode = (int)_budgetItemMode ;
                            objCurrent._budgetRecKey = _budgetRecKey;
                            objCurrent._budgetPeriod = (int)dr["BudgetPeriod"];
                            objCurrent._budgetAmountH = (decimal)dr["BudgetAmountH"];
                            objCurrent._budgetQty = (decimal)dr["BudgetQty"];
                            objCurrent._budgetWeight = (decimal)dr["BudgetWeight"];
                            #region Set Server DateTime If Create and Modified Date is null
                            //Get Server Date and Time (sdt)
                            DateTime svrDateTime = GFunc.GetSvrDateTime(cn);
                            //Set Header Obj
                            objCurrent._createDate = GFunc.NEDateTime(_MSTBudget.CreateDate, svrDateTime);
                            objCurrent._createUserKey = GFunc.NEInt(_MSTBudget.CreateUserKey, AppInfor.currentUserKey);
                            objCurrent._lastModifiedDate = svrDateTime;
                            objCurrent._lastModifiedUserKey = AppInfor.currentUserKey;
                            #endregion

                           // if (objCurrent._budgetAmountH!=0 || objCurrent._budgetQty!=0 || objCurrent._budgetWeight!=0) // not ready; Mic Check
                                objCurrent.Update(cn);
                        }

                        // Commit Process
                        this._isDirty = false;
                        ErrorMessageID= string.Empty;
                        isSave = true;

                        // No errors - commit transaction
                          if (Transaction.Current.TransactionInformation.Status != TransactionStatus.Active)  throw new Exception("Transaction has aborted."); scope.Complete();

                        isCommitTransFail = false;

                    }// End of SqlConnection
                }// End of TransactionScope                        

                // Audit Log
                SysAuditLogUtility.AddAuditLog(GEnum.AuditLogMode.Edit, constCodeKey, _MSTBudget._budgetRecKey, _MSTBudget.BudgetType.ToString(), GFunc.GetCodeKeyDescription((int)ConstantCodeKey), new object[] { _MSTBudget });

            }
            catch (TAException tex)
            {
                throw Error(tex);
            }
            catch (Exception ex)
            {
                if (isCommitTransFail)
                    ErrorMessageID = MsgID.Validation.CommitTransFail;

                throw Error(ex);
            }
            
            return isSave;
        }//Completed
        public bool Dispose()
        {
            bool isDispose = false;
            string msgID = string.Empty;

            try
            {
                if (!SysLockUtility.RemoveLock(true, GEnum.SysLockOption.ByGUID, constCodeKey, GUID, 0, 0))
                return false;
            }
            catch (TAException tex)
            {
                Error(tex);
                return false;
            }
            catch (Exception ex)
            {
                Error(ex);
                return false;
            }
            isDispose = true;
            return isDispose;
        }
       
        internal void SetBudgetMode(SqlConnection cn, int BudgetType, int BudgetRecKey)
        {
            try
            {
                switch (BudgetType)
                {
                    case (int)GEnum.BudgetType.Account:
                        CheckBudgetType(cn, BudgetRecKey);
                        break;

                    case (int)GEnum.BudgetType.Item_Purchases:
                    case (int)GEnum.BudgetType.Vendor_Purchase:
                    case (int)GEnum.BudgetType.Industry_Purchase:
                    case (int)GEnum.BudgetType.Territory_Purchase:
                    case (int)GEnum.BudgetType.Document_Group_Purchase:
                    case (int)GEnum.BudgetType.Document_Group_Item_Purchases:
                        this._budgetMode = GEnum.BudgetModeValue.Budget;
                        break;
                    default:
                        this._budgetMode = GEnum.BudgetModeValue.Target;
                        break;
                }
            }
            catch (TAException tex)
            {
                throw Error(tex);
            }
            catch (Exception ex)
            {
                throw Error(ex);
            }
            
        }//Completed
        internal void CheckBudgetType(SqlConnection cn, int BudgetRecKey)
        {
            // Initialisation
            string msgID = MsgID.Common.GetFail;

            try
            {
                //Create New Object
                if (GFunc.IsNE(this._MSTAcc))
                    _MSTAcc = BOLib.MSTAcc.New();

                // Fetch All Account Key
                if (!this._MSTAcc.Fetch(cn, new MSTAcc.Criteria(BudgetRecKey, 1)))
                    throw new TAException(msgID);

                switch (_MSTAcc.AccTypeKey)
                {                        
                    case (int)GEnum.BudgetMode.Equity:
                    case (int)GEnum.BudgetMode.Retain_Earning:
                    case (int)GEnum.BudgetMode.Fixed_Asset:
                    case (int)GEnum.BudgetMode.Current_Asset:
                    case (int)GEnum.BudgetMode.Other_Asset:
                    case (int)GEnum.BudgetMode.Inventory:
                    case (int)GEnum.BudgetMode.Bank:
                    case (int)GEnum.BudgetMode.Petty_Cash:
                    case (int)GEnum.BudgetMode.Temp_Holding_Fund:
                    case (int)GEnum.BudgetMode.Account_Receivable:
                    case (int)GEnum.BudgetMode.Income:
                    case (int)GEnum.BudgetMode.Other_Income:
                        this._budgetMode = GEnum.BudgetModeValue.Target;
                        break;
                    default:
                        this._budgetMode = GEnum.BudgetModeValue.Budget;
                        break;
                }
            }
            catch (TAException tex)
            {
                throw Error(tex);
            }
            catch (Exception ex)
            {
                throw Error(ex);
            }
        }//Completed
        public bool AssignAllPeriod(decimal _AmtRatio, decimal _AddAmt,decimal _unitRatio,decimal _AddUnit,int BudgetItmMode)
        {
            bool isAssign = true;

            try
            {
                if (_MSTBudgets.Rows.Count == _MSTPrevBudgets.Rows.Count)
                {
                    for (int i = 0; i < _MSTBudgets.Rows.Count; i++)
                    {
                        _MSTBudgets.Rows[i]["BudgetAmountH"] = ((decimal)_MSTPrevBudgets.Rows[i]["BudgetAmountH"] * _AmtRatio) + _AddAmt;
                        if (BudgetItmMode == (int)GEnum.BudgetItemMode.Unit)
                        {
                            _MSTBudgets.Rows[i]["BudgetQty"] = ((decimal)_MSTPrevBudgets.Rows[i]["BudgetQty"] * _unitRatio) + _addUnit;
                        }
                        else if (BudgetItmMode == (int)GEnum.BudgetItemMode.Weight)
                        {
                            _MSTBudgets.Rows[i]["BudgetWeight"] = ((decimal)_MSTPrevBudgets.Rows[i]["BudgetWeight"] * _unitRatio) + _addUnit;
                        }
                    }
                }
                
                _MSTBudgets.AcceptChanges();
                return isAssign;
            }
            catch (TAException tex)
            {
                throw Error(tex);
            }
            catch (Exception ex)
            {
                throw Error(ex);
            }
            
        }
        internal void SetMissingValues()
        {
            if (GFunc.IsNE(this._MSTBudget._budgetBranchKey)) this._MSTBudget._budgetBranchKey = 0;
            if (GFunc.IsNE(this._MSTBudget._budgetDeptKey)) this._MSTBudget._budgetDeptKey = 0;
            if (GFunc.IsNE(this._MSTBudget._budgetRecSubKey)) this._MSTBudget._budgetRecSubKey = 0;
            if (GFunc.IsNE(this._MSTBudget._budgetAmountH)) this._MSTBudget._budgetAmountH = 0;
            if (GFunc.IsNE(this._MSTBudget._budgetItmMode)) this._MSTBudget._budgetItmMode = 0;
            if (GFunc.IsNE(this._MSTBudget._budgetQty)) this._MSTBudget._budgetQty = 0;
            if (GFunc.IsNE(this._MSTBudget._budgetWeight)) this._MSTBudget._budgetWeight = 0;
        }//Completed     

      
        void _MSTBudgets_ColumnChanged(object sender, DataColumnChangeEventArgs e)
        {
            _isDirty = true;
        }
        private bool CheckDataForCopy()
        {
            bool isCheck = false;
            try
            {             
                using (SqlConnection cn = new SqlConnection(Database.BossDemoConnection))
                {
                    // Open Connection
                    cn.Open();

                    isCheck = _MSTBudget.CheckData(cn, new MSTBudget.Criteria(_budgettype, _fromBranchID, _toBranchID, _fromDeptID, _toDeptID, _selectBudgetRecKey, _selectBudgetRecSubKey, _fromBudgetPeriod, _toBudgetPeriod, 4));

                    if (isCheck)
                    {
                        isCheck = true;
                    }
                    else
                    {
                        throw new Exception("Copy Fail. There is no budget data for the selected record in the selected period.");
                    }
                }

            }
            catch (TAException tex)
            {
                throw Error(tex);
            }
            catch (Exception ex)
            {
                throw Error(ex);
            }
            return isCheck;
        }

        public bool Copy(TAUtil.TAGridEditor tagrdGroupList)
        {
            bool isCopy = false;

            string msgID = MsgID.Common.CopyFail;

            bool processOK = true;

            bool isCommitTransFail = true;
            string recordID = string.Empty;


            try
            {
                if (this.IsOpenReadOnly)
                {
                    processOK = false;
                    msgID = MsgID.Common.RecordIsReadOnly;
                }
                else
                {
                    processOK = SECPermUtility.Edit(constPermID, true);
                }

                if (processOK)
                    processOK = this.CheckDataForCopy();

                if (processOK)
                {
                    // Create TransactionScope                        
                    using (TransactionScope scope = new TransactionScope())
                    {
                        // Create SqlConnection
                        using (SqlConnection cn = new SqlConnection(Database.BossDemoConnection))
                        {
                            // Open Connection
                            cn.Open();

                            #region Set Server DateTime If Create and Modified Date is null
                            //Get Server Date and Time (sdt)
                            DateTime svrDateTime = GFunc.GetSvrDateTime(cn);
                            #endregion

                            #region Copy process
                            foreach (UltraGridRow row in tagrdGroupList.Rows)
                            {
                                if (GFunc.NEBool(row.Cells["Selected"].Value, false) == true)
                                {
                                    switch (this._budgettype)
                                    {
                                        case (int)(int)GEnum.BudgetType.Account:
                                        case (int)GEnum.BudgetType.Item_Sales:
                                        case (int)GEnum.BudgetType.Item_Purchases:
                                        case (int)GEnum.BudgetType.Customer_Sales:
                                        case (int)GEnum.BudgetType.Vendor_Purchase:
                                        case (int)GEnum.BudgetType.Document_Group_Sales:
                                        case (int)GEnum.BudgetType.Document_Group_Purchase:
                                        case (int)GEnum.BudgetType.Industry_Sales:
                                        case (int)GEnum.BudgetType.Industry_Purchase:
                                        case (int)GEnum.BudgetType.Territory_Sales:
                                        case (int)GEnum.BudgetType.Territory_Purchase:
                                            this.TargetBudgetRecKey = GFunc.NEInt(row.Cells["RecordKey"].Value, 0);
                                            break;

                                        case (int)GEnum.BudgetType.Document_Group_and_Item_Sales:
                                        case (int)GEnum.BudgetType.Document_Group_Item_Purchases:
                                            this.TargetBudgetRecKey = GFunc.NEInt(this.SelectBudgetRecKey, 0);
                                            this.BudgetRecSubKey = GFunc.NEInt(row.Cells["RecordKey"].Value, 0);
                                            break;

                                        default:
                                            break;
                                    }

                                    //Copy Record
                                    using (SqlCommand cm = cn.CreateCommand())
                                    {
                                        // Using existing sql connection.
                                        cm.CommandType = CommandType.StoredProcedure;
                                        cm.CommandText = "MSTBudget_Copy";
                                        cm.Parameters.AddWithValue("@RetValue", 0);
                                        cm.Parameters.AddWithValue("@SelectBudgetRecKey", _selectBudgetRecKey);
                                        cm.Parameters.AddWithValue("@TargetBudgetRecKey", _targetBudgetRecKey);
                                        cm.Parameters.AddWithValue("@BudgetRecSubKey", _BudgetRecSubKey);
                                        cm.Parameters.AddWithValue("@BranchIDFrom", _fromBranchID);
                                        cm.Parameters.AddWithValue("@BranchIDTo", _toBranchID);
                                        cm.Parameters.AddWithValue("@DeptIDFrom", _fromDeptID);
                                        cm.Parameters.AddWithValue("@DeptIDTo", _toDeptID);

                                        cm.Parameters.AddWithValue("@FromBudgetPeriod", _fromBudgetPeriod);
                                        cm.Parameters.AddWithValue("@ToBudgetPeriod", _toBudgetPeriod);

                                        cm.Parameters.AddWithValue("@AmountRatio", _amountRatio);
                                        cm.Parameters.AddWithValue("@AddAmount", _addAmount);

                                        cm.Parameters.AddWithValue("@UnitRatio", _unitRatio);
                                        cm.Parameters.AddWithValue("@AddUnit", _addUnit);

                                        cm.Parameters.AddWithValue("@CreateDate", svrDateTime);
                                        cm.Parameters.AddWithValue("@CreateUserKey", AppInfor.currentUserKey);

                                        cm.Parameters.AddWithValue("@LastModifiedDate", svrDateTime);
                                        cm.Parameters.AddWithValue("@LastModifiedUserKey", AppInfor.currentUserKey);

                                        cm.Parameters["@RetValue"].Direction = ParameterDirection.Output;

                                        cm.ExecuteNonQuery();

                                        if ((int)cm.Parameters["@RetValue"].Value == (int)(int)GEnum.SpState.Pass)
                                        {
                                            processOK = true;
                                            msgID = string.Empty;
                                        }
                                        else
                                            throw new TAException(msgID);
                                    }
                                }
                            }
                            #endregion
                            // Commit Process
                            if (processOK)
                            {
                                this._isDirty = false;
                                msgID = string.Empty;
                                isCopy = true;

                                // No errors - commit transaction
                                if (Transaction.Current.TransactionInformation.Status != TransactionStatus.Active) throw new Exception("Transaction has aborted."); scope.Complete();

                                isCommitTransFail = false;
                            }

                        }// End of SqlConnection
                    }// End of TransactionScope                        

                    // Audit Log
                    if (processOK)
                    {
                        SysAuditLogUtility.AddAuditLog(GEnum.AuditLogMode.Edit, constCodeKey, _MSTBudget._budgetRecKey, _MSTBudget.BudgetType.ToString(), new object[] { _MSTBudget });
                    }
                }
            }
            catch (TAException tex)
            {
                throw Error(tex);
            }
            catch (Exception ex)
            {
                if (isCommitTransFail)
                    msgID = MsgID.Validation.CommitTransFail;
                throw Error(ex);
            }

            return isCopy;
        }

        public bool Validation_Detail(SqlConnection cn)
        {
            //Variable Declaration
            bool processOk = true;

            foreach (DataRow dr in this._MSTBudgets.Rows)
            {               
                if (dr.RowState == DataRowState.Deleted)
                    continue;
                else
                {
                    processOk = Validation_Detail(dr, "");
                }
            }
            return processOk;
        }//Completed
        public bool Validation_Detail(DataRow dr, string fieldToCheck)
        {
            //Validation Check for calls from FORM (CustomCellUpdate, BeforeRowUpdate)
            string msgID = string.Empty;
            bool processOK = true;
            UINotifierEventArgs e = new UINotifierEventArgs(new Hashtable());

            try
            {                
                //Check Column values
                if (fieldToCheck == string.Empty)
                {
                    foreach (DataColumn c in dr.Table.Columns)
                    {
                        switch(c.ColumnName)
                        {
                            case "BudgetAmountH":
                                BaseUtility.Validation(dr[c], c.ColumnName, c.ColumnName, GEnum.DataType.Decimel, GEnum.Require.Yes, null, null, null, null, null, ref processOK, false, e);
                                break;
                            case "BudgetQty":
                                BaseUtility.Validation(dr[c], c.ColumnName, c.ColumnName, GEnum.DataType.Decimel, GEnum.Require.Yes, null, GEnum.CompareOperator.GreatherThanEqual, 0, null, null, ref processOK, false, e);
                                break;
                            case "BudgetWeight":
                                BaseUtility.Validation(dr[c], c.ColumnName, c.ColumnName, GEnum.DataType.Decimel, GEnum.Require.Yes, null, GEnum.CompareOperator.GreatherThanEqual, 0, null, null, ref processOK, false, e);
                                break;
                        }
                    }
                }
                else
                    BaseUtility.Validation(dr[fieldToCheck], fieldToCheck, fieldToCheck, GEnum.DataType.Decimel, GEnum.Require.Yes, null, GEnum.CompareOperator.GreatherThanEqual, 0, null, null, ref processOK, false, e);
             
                //Set RowError Text
                if (processOK == false)
                {
                    using (SqlConnection cn = new SqlConnection(Database.BossDemoConnection))
                    {
                        cn.Open();
                        foreach (object key in e.PropertyMessage.Keys)
                        {
                            if (GFunc.IsNE(msgID) == false)
                                msgID += " and ";

                            msgID += SysMessageUtility.Get(cn, e.PropertyMessage[key].ToString());
                        }
                    }
                    dr.RowError = msgID;
                    throw new TAException(BOLib.MsgID.Common.ValidationFail);
                }
                else
                    dr.RowError = string.Empty;

                return processOK;
            }
            catch (TAException tex)
            {
                throw Error(tex);
            }
            catch (Exception ex)
            {
                throw Error(ex);
            }
            
        }//Completed
        public bool ValidationForCopy(SqlConnection cn)
        {
            bool isValidation = false;
            string msgID = MsgID.Common.ValidationFail;
            string propName = string.Empty;
            this._isValid = false;

            try
            {
                if (this.InstanceMode == GEnum.InstanceMode.Normal)
                {
                    isValidation = _MSTBudget.Validation(cn, new MSTBudget.Criteria(_MSTBudget._budgetType, _MSTBudget._budgetBranchKey, _MSTBudget._budgetDeptKey, _MSTBudget._budgetRecKey, _MSTBudget._budgetRecSubKey, _MSTBudget._budgetPeriod), false);

                    if (isValidation)
                    {
                        msgID = string.Empty;
                        isValidation = true;
                    }
                    else
                    {
                        PropertyChangedEventArgs e = new PropertyChangedEventArgs("BudgetID");
                        this.errorEvent.Invoke(SysMessageUtility.Get(msgID), e);
                    }
                }
                return isValidation;
            }
            catch (TAException tex)
            {
                throw Error(tex);
            }
            catch (Exception ex)
            {
                throw Error(ex);
            }
            
        }//Not Redy (use or nouse)

        #region Error 

        private Exception Error(Exception ex)
        {
            try
            {
                ex = SysAuditLogUtility.ModifyException(ex, false, new object[] { _MSTBudget, _MSTAcc }, constCodeKey);
            }
            catch (Exception nex)
            {
                MsgBox.Show(nex.Message);
            }
            return ex;
        }
        private TAException Error(TAException ex)
        {
            try
            {
                ex = SysAuditLogUtility.ModifyTAException(ex, false, new object[] { _MSTBudget, _MSTAcc }, constCodeKey);
            }
            catch (Exception nex)
            {
                MsgBox.Show(nex.Message);
            }
            return ex;
        }

        #endregion 
    }
}