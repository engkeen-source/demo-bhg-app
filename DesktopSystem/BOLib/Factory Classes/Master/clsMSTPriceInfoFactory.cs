using System;
using System.Data;
using System.Data.SqlClient;
using Csla;
using Csla.Data;
using System.Transactions;
using System.Linq;
using System.Linq.Expressions;
using System.Collections;
using System.ComponentModel;
using System.Collections.Generic;
using Infragistics.Win.UltraWinGrid;
using TAUtil;


namespace BOLib
{
    [Serializable()]
    public class MSTPriceInfoFactory : CommandBase
    {
        #region Member variables and constants

        private MSTPriceList _MSTPriceList = null;
        //private MSTPriceLists _MSTPriceLists = null;
        //private MSTPriceListDetRatio _MSTPriceListDetRatio = null;
        private MSTPriceListDetRatios _MSTPriceListDetRatios = null;
        //private MSTPriceListDetValue _MSTPriceListDetValue = null;
        private MSTPriceListDetValues _MSTPriceListDetValues = null;
        private GEnum.InstanceMode _instanceMode = GEnum.InstanceMode.Normal;
        private bool _isDirty = false;
        private bool _isValid = false;
        private bool _isNew = false;
        private bool _isReadOnly = false;
        private int _guID = 0;

        // System Code Key for this Factory.
        private GEnum.SystemCode constCodeKey = GEnum.SystemCode.Price_List;
        public GEnum.SystemCode ConstantCodeKey { get { return constCodeKey; } }

        // Permission ID for this Factory.
        private string constPermID = GVar.PermissionID.Price_List;
        public string PermID { get { return constPermID; } }

        // Custom Event Declaration 
        public GVar.DirtyEvent dirtyEvent = null; 
        public GVar.UINotifierEvent ErrorNotifierHeader_Set = null;
        public GVar.UINotifierEvent ErrorNotifierHeader_Clear = null;

        //public GVar.UINotifierEvent valueListNotifier = null;
        //public GVar.UINotifierEvent ratioListNotifier = null;
        //public GVar.ErrorEvent errorEvent = null;

        #endregion // Member variables and constant

        #region Factory Properties

        public MSTPriceList ObjMSTPriceList
        {
            get
            {
                return this._MSTPriceList;
            }
            set
            {
                this._MSTPriceList = value;
            }
        }

        //public MSTPriceLists ObjMSTPriceLists
        //{
        //    get
        //    {
        //        return this._MSTPriceLists;
        //    }
        //}

        //public MSTPriceListDetRatio ObjMSTPriceListDetRatio
        //{
        //    get
        //    {
        //        return this._MSTPriceListDetRatio;
        //    }
        //    set
        //    {
        //        this._MSTPriceListDetRatio = value;
        //    }
        //}

        public MSTPriceListDetRatios ObjMSTPriceListDetRatios
        {
            get
            {
                return this._MSTPriceListDetRatios;
            }
        }

        //public MSTPriceListDetValue ObjMSTPriceListDetValue
        //{
        //    get
        //    {
        //        return this._MSTPriceListDetValue;
        //    }
        //    set
        //    {
        //        this._MSTPriceListDetValue = value;
        //    }
        //}

        public MSTPriceListDetValues ObjMSTPriceListDetValues
        {
            get
            {
                return this._MSTPriceListDetValues;
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
                this._isDirty = value;
            }
        }

        public bool IsValid
        {
            get
            {
                return this._isValid;
            }
        }

        public bool IsNew
        {
            get
            {
                return this._isNew;
            }
        }

        public bool IsReadOnly
        {
            get
            {
                return this._isReadOnly;
            }
        }

        public int GUID
        {
            get
            {
                return this._guID;
            }
        }

        //public string ErrorMessageID
        //{
        //    get;
        //    set;
        //}

        #endregion // Constructors

        //Constructors, Initialisation
        public MSTPriceInfoFactory(GEnum.InstanceMode instanceMode)
        {
            try
            {
                this._instanceMode = instanceMode;
                this.Initialisation();
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
        public bool Initialisation()
        {
            try
            {
                if (this.InstanceMode == GEnum.InstanceMode.Normal)
                {
                    if (!SECPermUtility.Any(constPermID, out this._isReadOnly, true))
                        return false;

                    using (TransactionScope scope = new TransactionScope())
                    {
                        using (SqlConnection cn = new SqlConnection(Database.BossDemoConnection))
                        {
                            cn.Open();

                            // Get Instance GUID
                            //FORM will check for GUID > 0 to indicate Factory is valid
                            if ((this._guID = SysOptionUtility.GetNewLockingGUID(cn)) == 0)
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

                            // Commit Process
                            this._MSTPriceList = new MSTPriceList();
                            this._MSTPriceListDetRatios = new MSTPriceListDetRatios(cn);
                            this._MSTPriceListDetValues = new MSTPriceListDetValues(cn);
                            this._isNew = false;                        
                            this._isReadOnly = false;
                              if (Transaction.Current.TransactionInformation.Status != TransactionStatus.Active)  throw new Exception("Transaction has aborted."); scope.Complete();
                        }
                    }
                }
                else if (this.InstanceMode == GEnum.InstanceMode.InternalCall)
                {
                    if (!SECPermUtility.Any(constPermID, out this._isReadOnly, true))
                        return false;

                    using (TransactionScope scope = new TransactionScope())
                    {
                        using (SqlConnection cn = new SqlConnection(Database.BossDemoConnection))
                        {
                            cn.Open();

                            // Get Instance GUID
                            //FORM will check for GUID > 0 to indicate Factory is valid
                            if ((this._guID = SysOptionUtility.GetNewLockingGUID(cn)) == 0)
                            {
                                this._guID = -1;
                                return false;
                            }

                            // Locking
                            if (SysLockUtility.CheckInProgressLock(cn, true, constCodeKey))
                            {
                                this._guID = -1;
                                return true;
                            } 

                            // Commit Process
                            this._MSTPriceList = new MSTPriceList();
                            this._MSTPriceListDetRatios = new MSTPriceListDetRatios(cn);
                            this._MSTPriceListDetValues = new MSTPriceListDetValues(cn);
                            this._isNew = false;
                            this._isReadOnly = false;
                              if (Transaction.Current.TransactionInformation.Status != TransactionStatus.Active)  throw new Exception("Transaction has aborted."); scope.Complete();
                        }
                    }
                }
                else
                {
                    //Use for situation where no locking and GUID is required but the factory is needed for some internal call
                    //for future use only
                    this._guID = 0;                
                    this._isReadOnly = false;
                }
                return true;
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

        //Methods
        public bool New()
        {
            #region Declaration
            bool restoreFlag = false;
            BOLib.MSTPriceList copyMSTPriceList = null;
            BOLib.MSTPriceListDetValues copyMSTPriceListDetValues = null;
            BOLib.MSTPriceListDetRatios copyMSTPriceListDetRatios = null;
            #endregion

            try
            {
                #region Make backup of objects for restore purpose
                if (this._MSTPriceList != null)
                    copyMSTPriceList = this._MSTPriceList.Clone();

                if (this._MSTPriceListDetValues != null)
                    copyMSTPriceListDetValues = GFunc.TACopyDataTable(_MSTPriceListDetValues);

                if (this._MSTPriceListDetRatios != null)
                    copyMSTPriceListDetRatios = GFunc.TACopyDataTable(_MSTPriceListDetRatios);
                #endregion

                #region Check Security Permission
                if (SECPermUtility.Any(constPermID, out this._isReadOnly, true) == false)
                    return false;
                #endregion

                using (TransactionScope scope = new TransactionScope())
                {
                    using (SqlConnection cn = new SqlConnection(Database.BossDemoConnection))
                    {
                        #region Prepare New Instance
                        cn.Open();

                        //Turn on restore flag to restore objects if any error occurs
                        restoreFlag = true;

                        //Remove all locks by GUID except inprogress Locking
                        if (SysLockUtility.RemoveLockGUIDKeepIP(cn, true, GUID, constCodeKey) == false)
                            return false;

                        //prepare new instance           
                        this._MSTPriceList = MSTPriceList.New();

                        _MSTPriceListDetValues.Clear();
                        if (this._MSTPriceListDetValues.Fetch(cn, new MSTPriceListDetValues.Criteria(0, 1)) == false)
                            throw new TAException(MsgID.Common.NewFail);

                        _MSTPriceListDetRatios.Clear();
                        if (this._MSTPriceListDetRatios.Fetch(cn, new MSTPriceListDetRatios.Criteria(0, 1)) == false)
                            throw new TAException(MsgID.Common.NewFail);

                          if (Transaction.Current.TransactionInformation.Status != TransactionStatus.Active)  throw new Exception("Transaction has aborted."); scope.Complete();

                        //Set Flags
                        this._isDirty = false;
                        this._isNew = true;
                        this._isReadOnly = false;

                        //Attach Events
                        this._MSTPriceList.PropertyChanged += new System.ComponentModel.PropertyChangedEventHandler(Obj_PropertyChanged);
                        #endregion
                    }
                }
                restoreFlag = false;
                return true;

            }
            catch (TAException tex)
            {
                throw Error(tex);
            }
            catch (Exception ex)
            {
                throw Error(ex);
            }
            finally
            {
                #region resetore data to Obj and dtTables
                if (restoreFlag == true)
                {
                    this._MSTPriceList = copyMSTPriceList;
                    this._MSTPriceListDetValues = copyMSTPriceListDetValues;
                    this._MSTPriceListDetRatios = copyMSTPriceListDetRatios;
                }
                #endregion

                #region Dispose Backup Objects
                copyMSTPriceList = null;
                copyMSTPriceListDetValues = null;
                copyMSTPriceListDetRatios = null;
                #endregion
            }
        }//Completed
        public bool GetEdit(int key)
        {
            #region Declaration
            bool restoreFlag = false;
            BOLib.MSTPriceList copyMSTPriceList = null;
            BOLib.MSTPriceListDetValues copyMSTPriceListDetValues = null;
            BOLib.MSTPriceListDetRatios copyMSTPriceListDetRatios = null;
            #endregion

            try
            {
                #region Make backup of objects for restore purpose
                if (this._MSTPriceList != null)
                    copyMSTPriceList = this._MSTPriceList.Clone();

                if (this._MSTPriceListDetValues != null)
                    copyMSTPriceListDetValues = GFunc.TACopyDataTable(_MSTPriceListDetValues);

                if (this._MSTPriceListDetRatios != null)
                    copyMSTPriceListDetRatios = GFunc.TACopyDataTable(_MSTPriceListDetRatios);
                #endregion

                #region Check Security Permission
                if (SECPermUtility.Edit(constPermID, true) == false)
                    return false;
                #endregion

                using (TransactionScope scope = new TransactionScope())
                {
                    using (SqlConnection cn = new SqlConnection(Database.BossDemoConnection))
                    {
                        #region Get Data
                        cn.Open();

                        //Turn on restore flag to restore objects if any error occurs
                        restoreFlag = true;

                        //Check Lock
                        if (SysLockUtility.IsLock(cn, true, GEnum.SysLockOption.ByCodeKeyandDataKey, constCodeKey, key, 0, _guID))
                            return false;

                        //Remove Lock
                        if (SysLockUtility.RemoveLockGUIDKeepIP(cn, true, this._guID, constCodeKey) == false)
                            return false;

                        //Add Lock
                        if (SysLockUtility.AddLock(cn, true, this._guID, constCodeKey, key) == false)
                            return false;

                        //Get Record                                 
                        if (this._MSTPriceList.Fetch(cn, new MSTPriceList.Criteria(key, string.Empty, 1)) == false)
                        {
                            MsgBox.Show(cn, MsgID.Common.GetFail);
                            return false;
                        }

                        //Record Not Found
                        if (GFunc.NEInt(this._MSTPriceList._priceKey, 0) == 0)
                        {
                            restoreFlag = false;
                            throw new TAException(MsgID.Common.GetFail);
                        }

                        _MSTPriceListDetValues.Clear();
                        if (_MSTPriceListDetValues.Fetch(cn, new MSTPriceListDetValues.Criteria(key, 2)) == false)
                        {
                            MsgBox.Show(cn, MsgID.Common.GetFail);
                            return false;
                        }
                        _MSTPriceListDetRatios.Clear();
                        if (_MSTPriceListDetRatios.Fetch(cn, new MSTPriceListDetRatios.Criteria(key, 1)) == false)
                        {
                            MsgBox.Show(cn, MsgID.Common.GetFail);
                            return false;
                        }

                          if (Transaction.Current.TransactionInformation.Status != TransactionStatus.Active)  throw new Exception("Transaction has aborted."); scope.Complete();

                        //Set Flags
                        this._isDirty = false;
                        this._isNew = false;
                        this._isReadOnly = false;

                        //Attach Events
                        this._MSTPriceList.PropertyChanged += new PropertyChangedEventHandler(Obj_PropertyChanged);
                        
                        #endregion
                    }
                }
                restoreFlag = false;
                return true;

            }
            catch (TAException tex)
            {
                throw Error(tex);
            }
            catch (Exception ex)
            {
                throw Error(ex);
            }
            finally
            {
                #region resetore data to Obj and dtTables
                if (restoreFlag == true)
                {
                    this._MSTPriceList = copyMSTPriceList;
                    this._MSTPriceListDetValues = copyMSTPriceListDetValues;
                    this._MSTPriceListDetRatios = copyMSTPriceListDetRatios;
                }
                #endregion

                #region Dispose Backup Objects
                copyMSTPriceList = null;
                copyMSTPriceListDetValues = null;
                copyMSTPriceListDetRatios = null;
                #endregion
            }
        }//Completed
        public bool GetReadOnly(int key)
        {
            #region Declaration
            bool restoreFlag = false;
            BOLib.MSTPriceList copyMSTPriceList = null;
            BOLib.MSTPriceListDetValues copyMSTPriceListDetValues = null;
            BOLib.MSTPriceListDetRatios copyMSTPriceListDetRatios = null;
            #endregion

            try
            {
                #region Make backup of objects for restore purpose
                if (this._MSTPriceList != null)
                    copyMSTPriceList = this._MSTPriceList.Clone();

                if (this._MSTPriceListDetValues != null)
                    copyMSTPriceListDetValues = GFunc.TACopyDataTable(_MSTPriceListDetValues);

                if (this._MSTPriceListDetRatios != null)
                    copyMSTPriceListDetRatios = GFunc.TACopyDataTable(_MSTPriceListDetRatios);
                #endregion


                #region Check Security Permission
                if (SECPermUtility.Read(constPermID, true) == false)
                    return false;
                #endregion

                using (TransactionScope scope = new TransactionScope())
                {
                    using (SqlConnection cn = new SqlConnection(Database.BossDemoConnection))
                    {
                        #region Get Data
                        cn.Open();

                        //Turn on restore flag to restore objects if any error occurs
                        restoreFlag = true;

                        //Remove all locks by GUID except inprogress Locking
                        if (SysLockUtility.RemoveLockGUIDKeepIP(cn, true, this._guID, constCodeKey) == false)
                            return false;

                        //Get record
                        if (this._MSTPriceList.Fetch(cn, new MSTPriceList.Criteria(key,string.Empty, 1)) == false)
                        {
                            MsgBox.Show(cn, MsgID.Common.GetFail);
                            return false;
                        }
                        _MSTPriceListDetValues.Clear();
                        if (_MSTPriceListDetValues.Fetch(cn, new MSTPriceListDetValues.Criteria(key, 2)) == false)
                        {
                            MsgBox.Show(cn, MsgID.Common.GetFail);
                            return false;
                        }
                        _MSTPriceListDetRatios.Clear();
                        if (_MSTPriceListDetRatios.Fetch(cn, new MSTPriceListDetRatios.Criteria(key, 1)) == false)
                        {
                            MsgBox.Show(cn, MsgID.Common.GetFail);
                            return false;
                        }

                          if (Transaction.Current.TransactionInformation.Status != TransactionStatus.Active)  throw new Exception("Transaction has aborted."); scope.Complete();

                        //Set Flags
                        this._isDirty = false;
                        this._isNew = false;
                        this._isReadOnly = true;

                        #endregion
                    }
                }
                restoreFlag = false;
                return true;

            }
            catch (TAException tex)
            {
                throw Error(tex);
            }
            catch (Exception ex)
            {
                throw Error(ex);
            }
            finally
            {
                #region resetore data to Obj and dtTables
                if (restoreFlag == true)
                {
                    this._MSTPriceList = copyMSTPriceList;
                    this._MSTPriceListDetValues = copyMSTPriceListDetValues;
                    this._MSTPriceListDetRatios = copyMSTPriceListDetRatios;
                }
                #endregion

                #region Dispose Backup Objects
                copyMSTPriceList = null;
                copyMSTPriceListDetValues = null;
                copyMSTPriceListDetRatios = null;
                #endregion
            }
        }//Completed
        public bool SetReadOnlyData(DataTable dtHeader, DataSet dsDetail)
        {
            try
            {

                #region Check Security Permission
                if (SECPermUtility.Read(constPermID, true) == false)
                    return false;
                #endregion

                using (TransactionScope scope = new TransactionScope())
                {
                    using (SqlConnection cn = new SqlConnection(Database.BossDemoConnection))
                    {
                        #region Get Data
                        cn.Open();

                        //Remove all locks by GUID except inprogress Locking
                        if (SysLockUtility.RemoveLockGUIDKeepIP(cn, true, this._guID, constCodeKey) == false)
                            return false;

                        if (GFunc.IsNE(_MSTPriceList))
                            _MSTPriceList = MSTPriceList.New();
                        GFunc.ConvertDataTableToObject(dtHeader, _MSTPriceList);

                        _MSTPriceListDetValues = new MSTPriceListDetValues(cn);
                        GFunc.CopyDataTableToDetailObject(dsDetail.Tables[0], _MSTPriceListDetValues);

                        _MSTPriceListDetRatios = new MSTPriceListDetRatios(cn);
                        GFunc.CopyDataTableToDetailObject(dsDetail.Tables[1], _MSTPriceListDetRatios);
                          if (Transaction.Current.TransactionInformation.Status != TransactionStatus.Active)  throw new Exception("Transaction has aborted."); scope.Complete();

                        //Set Flags
                        this._isDirty = false;
                        this._isNew = false;
                        this._isReadOnly = true;

                        #endregion
                    }
                }
                return true;

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
        public bool Save()
        {
            //For Master and Reference the RecordKey is obtain after saving. 
            //But we do not need to update this Record to the Record Detail as
            //the saving process will delete all details in the server and append the detail from local to server again
            //Note: this will only work if the detail in the server is never updated by other user.
            //So for example : Item Location, this will not work
            #region Declaration
            bool restoreFlag = false;
            bool isNewRecord = this.IsNew;//Because this flag will be changed during saving, we need to know the original value when error occurs
            int newRecordKey = 0;
            string autoID = string.Empty;
            BOLib.MSTPriceList copyMSTPriceList = null;
            BOLib.MSTPriceListDetValues copyMSTPriceListDetValues = null;
            BOLib.MSTPriceListDetRatios copyMSTPriceListDetRatios = null;

            #endregion

            try
            {
                #region Make backup of objects for restore purpose
                if (this._MSTPriceList != null)
                    copyMSTPriceList = this._MSTPriceList.Clone();

                if (this._MSTPriceListDetValues != null)
                    copyMSTPriceListDetValues = GFunc.TACopyDataTable(_MSTPriceListDetValues);

                if (this._MSTPriceListDetRatios != null)
                    copyMSTPriceListDetRatios = GFunc.TACopyDataTable(_MSTPriceListDetRatios);
                #endregion


                #region Check Permission
                if (this.IsReadOnly)
                {
                    MsgBox.Show(MsgID.Common.RecordIsReadOnly);
                    return false;
                }
                else
                {
                    if (this.IsNew)
                    {
                        if (SECPermUtility.Add(constPermID, true) == false)
                            return false;
                    }
                    else
                    {
                        if (SECPermUtility.Edit(constPermID, true) == false)
                            return false;
                    }
                }
                #endregion

                using (TransactionScope scope = new TransactionScope())
                {
                    using (SqlConnection cn = new SqlConnection(Database.BossDemoConnection))
                    {
                        #region Save Data
                        cn.Open();

                        //Turn on restore flag to restore objects if any error occurs
                        restoreFlag = true;

                        #region Get Auto Record ID
                        if (this.IsNew && GFunc.IsNE(_MSTPriceList._priceID))
                        {
                            if (SysIDCounterUtility.Get(cn, true, out autoID, constCodeKey, _MSTPriceList._priceDes) == false)
                                return false;

                            _MSTPriceList._priceID = autoID;
                        }
                        #endregion

                        #region Set default value for fields that cannot be empty but can have a general default value
                        _MSTPriceList._currKey = GFunc.NEInt(_MSTPriceList.CurrKey, 1);

                        //Get Server Date and Time (sdt)
                        DateTime svrDateTime = GFunc.GetSvrDateTime(cn);
                        _MSTPriceList._createDate = GFunc.NEDateTime(_MSTPriceList.CreateDate, svrDateTime);
                        _MSTPriceList._createUserKey = GFunc.NEInt(_MSTPriceList.CreateUserKey, AppInfor.currentUserKey);
                        _MSTPriceList._lastModifiedDate = svrDateTime;
                        _MSTPriceList._lastModifiedUserKey = AppInfor.currentUserKey;

                        //Update Ratio
                        if (_MSTPriceList.PriceType == 20) //Percentage
                        {
                            foreach (DataRow dr in ((DataTable)_MSTPriceListDetRatios).Rows)
                            {
                                if (dr["Cat1"].ToString() == string.Empty) dr["Cat1"] = "0";
                                if (dr["Cat2"].ToString() == string.Empty) dr["Cat2"] = "0";
                                if (dr["Cat3"].ToString() == string.Empty) dr["Cat3"] = "0";
                                if (dr["Cat4"].ToString() == string.Empty) dr["Cat4"] = "0";
                                if (dr["Cat5"].ToString() == string.Empty) dr["Cat5"] = "0";

                                if (!GFunc.IsNEZ(dr["Percentage"]))
                                {
                                    decimal value = 1 * GFunc.NEDec(dr["Percentage"], 0);
                                    value = value / 100;

                                    if (dr["RatioType"].ToString() == "10")
                                        value = 1 - value;
                                    else
                                        value += 1;

                                    dr["Ratio"] = value;
                                }

                                if (!GFunc.IsNEZ(dr["EffPercentage"]))
                                {
                                    decimal value = 1 * GFunc.NEDec(dr["EffPercentage"], 0);
                                    value = value / 100;

                                    if (dr["RatioType"].ToString() == "10")
                                        value = 1 - value;
                                    else
                                        value += 1;

                                    dr["EffRatio"] = value;
                                }
                            }
                        }
                        #endregion

                        #region Validation

                        if (Validation_Header(cn) == false)
                            return false;

                        if (this.ObjMSTPriceList.PriceType == 10)
                        {
                            if (Validation_Detail("tagrdPriceValueList", (DataTable)this.ObjMSTPriceListDetValues, cn) == false)
                                return false;
                        }
                        else
                        {
                            if (Validation_Detail("tagrdPriceRatioList", (DataTable)this.ObjMSTPriceListDetRatios, cn) == false)
                                return false;
                        }
                        #endregion

                        #region Save Record
                        //Note: there is no delete permission for Sales Rep Payroll (only Read,Edit)
                        if (IsNew)
                        {
                            if (_MSTPriceList.Insert(cn, out newRecordKey) == false)
                            {
                                MsgBox.Show(cn,MsgID.Common.SaveFail);
                                return false;
                            }
                            if (this.ObjMSTPriceList.PriceType == 10)
                            {
                                if (_MSTPriceListDetValues.Insert(cn, newRecordKey) == false)
                                {
                                    MsgBox.Show(cn, MsgID.Common.SaveFail);
                                    return false;
                                }
                            }
                            else
                            {
                                if (_MSTPriceListDetRatios.Insert(cn, newRecordKey) == false)
                                {
                                    MsgBox.Show(cn, MsgID.Common.SaveFail);
                                    return false;
                                }
                            }
                        }
                        else
                        {
                            if (_MSTPriceList.Update(cn) == false)
                            {
                                MsgBox.Show(cn, MsgID.Common.SaveFail);
                                return false;
                            }
                            if (this.ObjMSTPriceList.PriceType == 10)
                            {
                                if (_MSTPriceListDetValues.Delete(cn, new MSTPriceListDetValues.Criteria(_MSTPriceList._priceKey, 0)) == false)
                                {
                                    MsgBox.Show(cn, MsgID.Common.SaveFail);
                                    return false;
                                }
                                if (_MSTPriceListDetValues.Insert(cn, _MSTPriceList._priceKey) == false)
                                {
                                    MsgBox.Show(cn, MsgID.Common.SaveFail);
                                    return false;
                                }
                            }
                            else
                            {
                                if (_MSTPriceListDetRatios.Delete(cn, new MSTPriceListDetRatios.Criteria(_MSTPriceList._priceKey, 0)) == false)
                                {
                                    MsgBox.Show(cn, MsgID.Common.SaveFail);
                                    return false;
                                }
                                if (_MSTPriceListDetRatios.Insert(cn, _MSTPriceList._priceKey) == false)
                                {
                                    MsgBox.Show(cn, MsgID.Common.SaveFail);
                                    return false;
                                }
                            }
                        }
                        #endregion

                        #region For New Record perform: Locking, set new recordKey
                        if (IsNew)
                        {
                            if (SysLockUtility.AddLock(cn, true, GUID, constCodeKey, newRecordKey))
                                _MSTPriceList._priceKey = newRecordKey;
                            else
                                return false;
                        }
                        #endregion

                          if (Transaction.Current.TransactionInformation.Status != TransactionStatus.Active)  throw new Exception("Transaction has aborted."); scope.Complete();

                        #region Set Flags
                        this._isDirty = false;
                        this._isNew = false;
                        #endregion

                        #endregion
                    }
                }

                #region Update Auditlog
                if (isNewRecord)
                    SysAuditLogUtility.AddAuditLog(GEnum.AuditLogMode.Add, constCodeKey, _MSTPriceList.PriceKey, _MSTPriceList.PriceID, GFunc.GetCodeKeyDescription((int)ConstantCodeKey), new object[] { _MSTPriceList, _MSTPriceListDetValues, _MSTPriceListDetRatios });
                else
                    SysAuditLogUtility.AddAuditLog(GEnum.AuditLogMode.Edit, constCodeKey, _MSTPriceList.PriceKey, _MSTPriceList.PriceID, GFunc.GetCodeKeyDescription((int)ConstantCodeKey), new object[] { _MSTPriceList, _MSTPriceListDetValues, _MSTPriceListDetRatios });
                #endregion

                restoreFlag = false;
                return true;

            }
            catch (TAException tex)
            {
                throw Error(tex);
            }
            catch (Exception ex)
            {
                throw Error(ex);
            }
            finally
            {
                #region resetore data to Obj and dtTables
                if (restoreFlag == true)
                {
                    this._MSTPriceList = copyMSTPriceList;
                    this._MSTPriceListDetValues = copyMSTPriceListDetValues;
                    this._MSTPriceListDetRatios = copyMSTPriceListDetRatios;
                }
                #endregion

                #region Dispose Backup Objects
                copyMSTPriceList = null;
                copyMSTPriceListDetValues = null;
                copyMSTPriceListDetRatios = null;
                #endregion
            }
        }//Completed
        public bool Delete()
        {
            #region Declaration
            bool restoreFlag = false;
            BOLib.MSTPriceList copyMSTPriceList = null;
            BOLib.MSTPriceListDetValues copyMSTPriceListDetValues = null;
            BOLib.MSTPriceListDetRatios copyMSTPriceListDetRatios = null;
            #endregion

            try
            {
                #region Make backup of objects for restore purpose
                if (this._MSTPriceList != null)
                    copyMSTPriceList = this._MSTPriceList.Clone();

                if (this._MSTPriceListDetValues != null)
                    copyMSTPriceListDetValues = GFunc.TACopyDataTable(_MSTPriceListDetValues);

                if (this._MSTPriceListDetRatios != null)
                    copyMSTPriceListDetRatios = GFunc.TACopyDataTable(_MSTPriceListDetRatios);
                #endregion

                #region Check IsReadOnly, IsNew and Security Permission
                if (this.IsReadOnly)
                {
                    MsgBox.Show(MsgID.Common.RecordIsReadOnly);
                    return false;
                }
                else
                {
                    if (IsNew)
                    {
                        return false;
                    }
                    else
                    {
                        if (SECPermUtility.Delete(constPermID, true) == false)
                            return false;
                    }
                }
                #endregion

                using (TransactionScope scope = new TransactionScope())
                {
                    using (SqlConnection cn = new SqlConnection(Database.BossDemoConnection))
                    {
                        #region Delete Data
                        cn.Open();

                        //Turn on restore flag to restore objects if any error occurs
                        restoreFlag = true;

                        //Record Locking
                        if (SysLockUtility.CheckAddLock(cn, true, 0, constCodeKey, _MSTPriceList._priceKey, GUID) == false)
                            return false;

                        //Check the record is used in other dependency tables
                        if (GFunc.CheckKeyDependantsExists(cn, "PriceKey", _MSTPriceList._priceKey.Value, _MSTPriceList._priceID))
                            return false;

                        //Check for Option Table
                        if (GFunc.CheckKeyDependcyinOptionTable(cn, "MSTPrice", _MSTPriceList._priceKey.Value))
                            return false;

                        //Delete Record
                        if (_MSTPriceList.Delete(cn, new MSTPriceList.Criteria(_MSTPriceList._priceKey)) == false)
                        {
                            MsgBox.Show(cn, MsgID.Common.DeleteFail);
                            return false;
                        }

                        //Remove Lock
                        if (SysLockUtility.RemoveLockGUIDKeepIP(cn, true, GUID, constCodeKey) == false)
                            return false;

                        //Create New
                        this._MSTPriceList = MSTPriceList.New();

                        _MSTPriceListDetValues.Clear();
                        if (this._MSTPriceListDetValues.Fetch(cn, new MSTPriceListDetValues.Criteria(0, 1)) == false)
                            throw new TAException(MsgID.Common.DeleteFail);

                        _MSTPriceListDetRatios.Clear();
                        if (this._MSTPriceListDetRatios.Fetch(cn, new MSTPriceListDetRatios.Criteria(0, 1)) == false)
                            throw new TAException(MsgID.Common.DeleteFail);

                          if (Transaction.Current.TransactionInformation.Status != TransactionStatus.Active)  throw new Exception("Transaction has aborted."); scope.Complete();

                        #region Set Flag
                        this._isDirty = false;
                        this._isNew = true;
                        this._isReadOnly = false;
                        #endregion

                        #endregion
                    }
                }

                //Audit Log                    
                SysAuditLogUtility.AddAuditLog(GEnum.AuditLogMode.Delete, constCodeKey, copyMSTPriceList.PriceKey, copyMSTPriceList.PriceID, GFunc.GetCodeKeyDescription((int)ConstantCodeKey), new object[] { copyMSTPriceList, copyMSTPriceListDetValues, copyMSTPriceListDetRatios });

                restoreFlag = false;
                return true;

            }
            catch (TAException tex)
            {
                throw Error(tex);
            }
            catch (Exception ex)
            {
                throw Error(ex);
            }
            finally
            {
                #region resetore data to Obj and dtTables
                if (restoreFlag == true)
                {
                    this._MSTPriceList = copyMSTPriceList;
                    this._MSTPriceListDetValues = copyMSTPriceListDetValues;
                    this._MSTPriceListDetRatios = copyMSTPriceListDetRatios;
                }
                #endregion

                #region Dispose Backup Objects
                copyMSTPriceList = null;
                copyMSTPriceListDetValues = null;
                copyMSTPriceListDetRatios = null;
                #endregion
            }
        }//Completed
        public bool Dispose()
        {
            if (SysLockUtility.RemoveLock(true, GEnum.SysLockOption.ByGUID, constCodeKey, GUID, 0, 0) == false)
                return false;
            return true;
        }//Completed

        //Attached Events
        private void Obj_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            try
            {
                if (this._isReadOnly == false)
                {
                    if (this.dirtyEvent != null)
                        this.dirtyEvent.Invoke(this, e);

                    this._isDirty = true;
                }
            }
            catch (TAException tex)
            {
                MsgBox.Show(tex.Message);
                Error(tex);
            }
            catch (Exception ex)
            {
                MsgBox.Show(ex.Message);
                Error(ex);
            }
        }

        //Validation
        public bool Validation_Header(SqlConnection cn)
        {
            //fieldNameToCheck = string.empty to check for all fields
            string processOK = GVar.gcPass;
            string errorMsgID = string.Empty;
            UINotifierEventArgs e = new UINotifierEventArgs(new Hashtable());

            try
            {
                //Clear Error in UL
                if (GFunc.IsNE(this.ErrorNotifierHeader_Clear) == false)
                    this.ErrorNotifierHeader_Clear.Invoke(this, e);

                #region Validation for each Field
                if (this.IsNew)
                {
                    processOK = BaseUtility.Validation(processOK, false, out errorMsgID, this._MSTPriceList.PriceKey, "PriceKey", GEnum.DataType.Integer, GEnum.Require.No, null, null, null, null, null, e, cn);
                    processOK = BaseUtility.Validation(processOK, false, out errorMsgID, this._MSTPriceList.PriceID, "PriceID", GEnum.DataType.String, GEnum.Require.No, 50, null, null, null, null, e, cn);
                }
                else
                {
                    processOK = BaseUtility.Validation(processOK, false, out errorMsgID, this._MSTPriceList.PriceKey, "PriceKey", GEnum.DataType.Integer, GEnum.Require.Yes, null, GEnum.CompareOperator.GreatherThan, 0, null, null, e, cn);
                    processOK = BaseUtility.Validation(processOK, false, out errorMsgID, this._MSTPriceList.PriceID, "PriceID", GEnum.DataType.String, GEnum.Require.Yes, 50, null, null, null, null, e, cn);
                }

                processOK = BaseUtility.Validation(processOK, false, out errorMsgID, this._MSTPriceList.PriceDes, "PriceDes", GEnum.DataType.String, GEnum.Require.Yes, 255, null, null, null, null, e, cn);
                processOK = BaseUtility.Validation(processOK, false, out errorMsgID, this._MSTPriceList.PriceType, "PriceType", GEnum.DataType.Integer, GEnum.Require.Yes, null, GEnum.CompareOperator.GreatherThan, 0, null, null, e, cn);
                processOK = BaseUtility.Validation(processOK, false, out errorMsgID, this._MSTPriceList.CurrKey, "CurrKey", GEnum.DataType.Integer, GEnum.Require.Yes, null, GEnum.CompareOperator.GreatherThan, 0, null, null, e, cn);
                processOK = BaseUtility.Validation(processOK, false, out errorMsgID, this._MSTPriceList.BuildInCode, "BuildInCode", GEnum.DataType.Integer, GEnum.Require.Yes, null, GEnum.CompareOperator.GreatherThan, 0, null, null, e, cn);
                processOK = BaseUtility.Validation(processOK, false, out errorMsgID, this._MSTPriceList.Custom1, "Custom1", GEnum.DataType.String, GEnum.Require.No, 255, null, null, null, null, e, cn);
                processOK = BaseUtility.Validation(processOK, false, out errorMsgID, this._MSTPriceList.Custom2, "Custom2", GEnum.DataType.String, GEnum.Require.No, 255, null, null, null, null, e, cn);
                processOK = BaseUtility.Validation(processOK, false, out errorMsgID, this._MSTPriceList.Custom3, "Custom3", GEnum.DataType.String, GEnum.Require.No, 255, null, null, null, null, e, cn);
                #endregion

                if (e.PropertyMessage.Count == 0)
                {
                    //StoreProcedure Validation
                    if (_MSTPriceList.Validation(cn, new MSTPriceList.Criteria(_MSTPriceList.PriceKey, _MSTPriceList.PriceID), this.IsNew))
                    {
                        return true;
                    }
                    else
                    {
                        e.PropertyMessage.Add("PriceID", SysMessageUtility.Get(cn, MsgID.Validation.DuplicateRecordID + "PriceID"));
                        if (GFunc.IsNE(this.ErrorNotifierHeader_Set) == false)
                            this.ErrorNotifierHeader_Set.Invoke(this, e);

                        return false;
                    }
                }
                else
                {
                    if (GFunc.IsNE(this.ErrorNotifierHeader_Set) == false)
                        this.ErrorNotifierHeader_Set.Invoke(this, e);

                    return false;
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
        private bool Validation_Detail(string grdNm, DataTable dt, SqlConnection cn)
        {
            //Validation Check for calls from Factory (Save method)
            string msgID = string.Empty;
            bool processOK = true;
            try
            {
                foreach (DataRow dr in dt.Rows)
                {
                    msgID = string.Empty;
                    processOK = true;

                    if (dr.RowState == DataRowState.Deleted)
                        continue;
                    else
                    {
                        //Check Column values
                        UINotifierEventArgs e = new UINotifierEventArgs(new Hashtable());
                        foreach (DataColumn c in dr.Table.Columns)
                        {
                            Validation_DetailCheck(grdNm, dr[c.ColumnName.ToString()], c.ColumnName.ToString(), false, ref processOK, e);
                        }

                        // Check Effective Start Date and End Date  
                        if (processOK)
                        {
                            if (!GFunc.IsNE(dr["EffStartDate"]) && !GFunc.IsNE(dr["EffEndDate"]))
                                processOK = BaseUtility.Validation(out msgID, dr["EffEndDate"], "EffEndDate", GEnum.DataType.DateTime, GEnum.Require.No, null, GEnum.CompareOperator.GreatherThanEqual, dr["EffStartDate"], null, null);
                        }

                        //Check for Duplicate records
                        if (processOK)
                        {
                            if (_MSTPriceList.PriceType == 10)
                                Validation_DetailRelation(dr, grdNm, dr["ItmKey"], false, ref processOK, e);
                            else
                                Validation_DetailRelation(dr, grdNm, 0, false, ref processOK, e);
                        }

                        //Set RowError Text
                        if (processOK == false)
                        {
                            dr.RowError = GFunc.PropertyMessage_Merge(e, cn);
                            throw new TAException(BOLib.MsgID.Common.ValidationFail);
                        }
                        else
                            dr.RowError = string.Empty;
                    }
                }
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
        public bool Validation_Detail(string grdNm, UltraGridRow grdrow, string fieldToCheck)
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
                    foreach (UltraGridCell c in grdrow.Cells)
                    {
                        Validation_DetailCheck(grdNm, c.Value, c.Column.Key, false, ref processOK, e);
                    }
                }
                else
                    Validation_DetailCheck(grdNm, grdrow.Cells[fieldToCheck].Value, fieldToCheck, false, ref processOK, e);

                // Check Effective Start Date and End Date  
                if (processOK)
                {
                    if (grdNm == "tagrdPriceRatioList")
                    {
                        if (!GFunc.IsNE(grdrow.Cells["EffStartDate"].Value) && !GFunc.IsNE(grdrow.Cells["EffEndDate"].Value))
                            BaseUtility.Validation(grdrow.Cells["EffEndDate"].Value, "EffEndDate", "EffEndDate", GEnum.DataType.DateTime, GEnum.Require.No, null, GEnum.CompareOperator.GreatherThanEqual, grdrow.Cells["EffStartDate"].Value, null, null, ref processOK, false, e);
                    }
                }

                //Check for Duplicate records when fieldToCheck is Empty (meaning RowBeforeUpdate)
                if (processOK && fieldToCheck == string.Empty)
                {
                    DataRow drow = ((DataTable)grdrow.Band.Layout.Grid.DataSource).DefaultView[grdrow.Index].Row;
                    if (_MSTPriceList.PriceType == 10)
                        Validation_DetailRelation(drow, grdNm, grdrow.Cells["ItmKey"].Value, grdrow.IsAddRow, ref processOK, e);
                    else
                        Validation_DetailRelation(drow, grdNm, 0, grdrow.IsAddRow, ref processOK, e);
                }

                //Set RowError Text
                if (processOK == false)
                {
                    ((DataRowView)(grdrow.ListObject)).Row.RowError = GFunc.PropertyMessage_Merge(e);
                    throw new TAException(BOLib.MsgID.Common.ValidationFail);
                }
                else
                    ((DataRowView)(grdrow.ListObject)).Row.RowError = string.Empty;

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
        public bool Validation_DetailCheck(string grdNm, object propValue, string CheckNm, bool failonError, ref bool processOK, UINotifierEventArgs e)
        {
            try
            {
                switch (grdNm)
                {
                    #region tagrdPriceValueList Validation
                    case "tagrdPriceValueList":
                        BaseUtility.Validation(propValue, "ItmKey", CheckNm, GEnum.DataType.Integer, GEnum.Require.Yes, null, GEnum.CompareOperator.GreatherThan, 0, null, null, ref processOK, failonError, e);
                        BaseUtility.Validation(propValue, "ItmType", CheckNm, GEnum.DataType.Integer, GEnum.Require.Yes, null, GEnum.CompareOperator.GreatherThan, 0, null, null, ref processOK, failonError, e);
                        BaseUtility.Validation(propValue, "ItmDes", CheckNm, GEnum.DataType.String, GEnum.Require.No, 4000, null, null, null, null, ref processOK, failonError, e);
                        BaseUtility.Validation(propValue, "ItmQty", CheckNm, GEnum.DataType.Decimel, GEnum.Require.No, null, null, null, null, null, ref processOK, failonError, e);
                        BaseUtility.Validation(propValue, "ItmPrice", CheckNm, GEnum.DataType.Decimel, GEnum.Require.Yes, null, GEnum.CompareOperator.GreatherThanEqual, 0, null, null, ref processOK, failonError, e);
                        BaseUtility.Validation(propValue, "CustomPrice", CheckNm, GEnum.DataType.Decimel, GEnum.Require.Yes, null, GEnum.CompareOperator.GreatherThanEqual, 0, null, null, ref processOK, failonError, e);
                        BaseUtility.Validation(propValue, "LastUpdatedDate", CheckNm, GEnum.DataType.DateTime, GEnum.Require.No, null, null, null, null, null, ref processOK, failonError, e);
                        BaseUtility.Validation(propValue, "IgnorePriceUpdate", CheckNm, GEnum.DataType.Boolean, GEnum.Require.No, null, null, null, null, null, ref processOK, failonError, e);
                        BaseUtility.Validation(propValue, "EffItmPrice", CheckNm, GEnum.DataType.Decimel, GEnum.Require.Yes, null, GEnum.CompareOperator.GreatherThanEqual, 0, null, null, ref processOK, failonError, e);
                        BaseUtility.Validation(propValue, "Custom1", CheckNm, GEnum.DataType.String, GEnum.Require.No, 255, null, null, null, null, ref processOK, failonError, e);
                        BaseUtility.Validation(propValue, "Custom2", CheckNm, GEnum.DataType.String, GEnum.Require.No, 255, null, null, null, null, ref processOK, failonError, e);
                        BaseUtility.Validation(propValue, "Custom3", CheckNm, GEnum.DataType.String, GEnum.Require.No, 255, null, null, null, null, ref processOK, failonError, e);
                        break;
                    #endregion

                    #region tagrdPriceRatioList Validation
                    case "tagrdPriceRatioList":
                        BaseUtility.Validation(propValue, "Cat1", CheckNm, GEnum.DataType.Integer, GEnum.Require.Yes, null, GEnum.CompareOperator.GreatherThanEqual, 0, null, null, ref processOK, failonError, e);
                        BaseUtility.Validation(propValue, "Cat2", CheckNm, GEnum.DataType.Integer, GEnum.Require.Yes, null, GEnum.CompareOperator.GreatherThanEqual, 0, null, null, ref processOK, failonError, e);
                        BaseUtility.Validation(propValue, "Cat3", CheckNm, GEnum.DataType.Integer, GEnum.Require.Yes, null, GEnum.CompareOperator.GreatherThanEqual, 0, null, null, ref processOK, failonError, e);
                        BaseUtility.Validation(propValue, "Cat4", CheckNm, GEnum.DataType.Integer, GEnum.Require.Yes, null, GEnum.CompareOperator.GreatherThanEqual, 0, null, null, ref processOK, failonError, e);
                        BaseUtility.Validation(propValue, "Cat5", CheckNm, GEnum.DataType.Integer, GEnum.Require.Yes, null, GEnum.CompareOperator.GreatherThanEqual, 0, null, null, ref processOK, failonError, e);
                        BaseUtility.Validation(propValue, "RatioType", CheckNm, GEnum.DataType.Integer, GEnum.Require.Yes, null, GEnum.CompareOperator.GreatherThan, 0, null, null, ref processOK, failonError, e);
                        BaseUtility.Validation(propValue, "Percentage", CheckNm, GEnum.DataType.Decimel, GEnum.Require.Yes, null, GEnum.CompareOperator.GreatherThanEqual, 0, null, null, ref processOK, failonError, e);
                        BaseUtility.Validation(propValue, "Ratio", CheckNm, GEnum.DataType.Decimel, GEnum.Require.Yes, null, GEnum.CompareOperator.GreatherThan, 0, null, null, ref processOK, failonError, e);
                        BaseUtility.Validation(propValue, "EffStartDate", CheckNm, GEnum.DataType.DateTime, GEnum.Require.No, null, null, 0, null, null, ref processOK, failonError, e);
                        BaseUtility.Validation(propValue, "EffEndDate", CheckNm, GEnum.DataType.DateTime, GEnum.Require.No, null, null, 0, null, null, ref processOK, failonError, e);
                        BaseUtility.Validation(propValue, "EffPercentage", CheckNm, GEnum.DataType.Decimel, GEnum.Require.Yes, null, GEnum.CompareOperator.GreatherThanEqual, 0, null, null, ref processOK, failonError, e);
                        BaseUtility.Validation(propValue, "EffRatio", CheckNm, GEnum.DataType.Decimel, GEnum.Require.Yes, null, GEnum.CompareOperator.GreatherThan, 0, null, null, ref processOK, failonError, e);
                        BaseUtility.Validation(propValue, "CreateDate", CheckNm, GEnum.DataType.DateTime, GEnum.Require.No, null, null, 0, null, null, ref processOK, failonError, e);
                        BaseUtility.Validation(propValue, "CreateUserKey", CheckNm, GEnum.DataType.Integer, GEnum.Require.No, null, GEnum.CompareOperator.GreatherThan, 0, null, null, ref processOK, failonError, e);
                        BaseUtility.Validation(propValue, "LastModifiedDate", CheckNm, GEnum.DataType.DateTime, GEnum.Require.No, null, null, 0, null, null, ref processOK, failonError, e);
                        BaseUtility.Validation(propValue, "LastModifiedUserKey", CheckNm, GEnum.DataType.Integer, GEnum.Require.No, null, GEnum.CompareOperator.GreatherThan, 0, null, null, ref processOK, failonError, e);
                        BaseUtility.Validation(propValue, "Custom1", CheckNm, GEnum.DataType.String, GEnum.Require.No, 255, null, 0, null, null, ref processOK, failonError, e);
                        BaseUtility.Validation(propValue, "Custom2", CheckNm, GEnum.DataType.String, GEnum.Require.No, 255, null, 0, null, null, ref processOK, failonError, e);
                        BaseUtility.Validation(propValue, "Custom3", CheckNm, GEnum.DataType.String, GEnum.Require.No, 255, null, 0, null, null, ref processOK, failonError, e);
                        break;
                    #endregion
                }
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
        public bool Validation_DetailRelation(DataRow dr, string grdNm, object propValue, bool IsAddRow, ref bool processOK, UINotifierEventArgs e)
        {
            string msgID = string.Empty;
            bool errorFound = false;

            switch (grdNm)
            {
                #region tagrdPriceValueList
                case "tagrdPriceValueList":
                    var dupList = ObjMSTPriceListDetValues.AsEnumerable().ToList().FindAll(o => (o.Field<int>("ItmKey") == int.Parse(propValue.ToString())));

                    if (IsAddRow)
                    {
                        if (dupList.Count > 0)
                            errorFound = true;
                    }
                    else
                    {
                        if (dupList.Count > 1)
                            errorFound = true;
                    }
                    if (errorFound)
                    {
                        e.PropertyMessage.Add("rowError", "Item" + MsgID.Validation.DuplicateRecord);
                        processOK = false;
                    }
                    break;

                case "tagrdPriceRatioList":
                    if (!GFunc.IsNE(dr["EffStartDate"]) && !GFunc.IsNE(dr["EffEndDate"]))
                        processOK = BaseUtility.Validation(out msgID, dr["EffEndDate"], "EffEndDate", GEnum.DataType.DateTime, GEnum.Require.No, null, GEnum.CompareOperator.GreatherThanEqual, dr["EffStartDate"], null, null);
                    
                    // Check Duplicate Cat1 + Cat2 + cat3 + cat4 + cat5 (this combination cannot occur more than once
                    if (processOK)
                    {
                        var dupCat = this._MSTPriceListDetRatios.AsEnumerable().ToList().FindAll(o =>
                            (o.Field<int>("Cat1").Equals(dr["Cat1"])) &&
                            (o.Field<int>("Cat2").Equals(dr["Cat2"])) &&
                            (o.Field<int>("Cat3").Equals(dr["Cat3"])) &&
                            (o.Field<int>("Cat4").Equals(dr["Cat4"])) &&
                            (o.Field<int>("Cat5").Equals(dr["Cat5"])));

                        if (IsAddRow)
                        {
                            if (dupCat.Count > 0)
                                errorFound = true;
                        }
                        else
                        {
                            if (dupCat.Count > 1)
                                errorFound = true;
                        }
                        if (errorFound)
                        {
                            e.PropertyMessage.Add("rowError", "Category" + MsgID.Validation.DuplicateRecord);
                            processOK = false;
                        }

                    }
                    break;
                #endregion
            }
            return processOK;
        }//Completed

        public void SetBuildInCode(int buildInCode)
        {
            this.ObjMSTPriceList._buildInCode = buildInCode;
        }
        public void SetPriceType(int priceType)
        {
            this.ObjMSTPriceList._priceType = priceType;
        }

        //Error
        private Exception Error(Exception ex)
        {
            try
            {
                ex = SysAuditLogUtility.ModifyException(ex, false, new object[] { _MSTPriceList, _MSTPriceListDetRatios, _MSTPriceListDetValues }, ConstantCodeKey);
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
                ex = SysAuditLogUtility.ModifyTAException(ex, false, new object[] { _MSTPriceList, _MSTPriceListDetRatios, _MSTPriceListDetValues }, ConstantCodeKey);
            }
            catch (Exception nex)
            {
                MsgBox.Show(nex.Message);
            }
            return ex;
        }
    }
}
