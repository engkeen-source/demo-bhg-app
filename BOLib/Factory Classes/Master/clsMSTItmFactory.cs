using System;
using System.Data;
using System.Linq;
using System.Data.SqlClient;
using Csla;
using Csla.Data;
using System.Transactions;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;
using System.IO;
using Infragistics.Win.UltraWinGrid;
using TAUtil;

namespace BOLib
{
    [Serializable()]
    public class MSTItmFactory : CommandBase
    {
        #region Member variables and constants

        private MSTItm _MSTItm = null;
        private MSTItmDetAlts _MSTItmDetAlts = null;
        private MSTItmDetAsss _MSTItmDetAsss = null;
        private MSTItmDetBOM _MSTItmDetBOM = null;
        private MSTItmDetBOMs _MSTItmDetBOMRMs = null;
        private MSTItmDetBOMs _MSTItmDetBOMPMs = null;
        private MSTItmDetBOMs _MSTItmDetBOMLBs = null;
        private MSTItmDetLocs _MSTItmDetLocs = null;
        private MSTItmDetPrice _MSTItmDetPrice = null;
        private MSTItmBatchs _MSTItmBatchs = null;
        private MSTItmSerials _MSTItmSerials = null;

        private GEnum.InstanceMode _instanceMode = GEnum.InstanceMode.Normal;
        private bool _isDirty = false;
        private bool _isValid = false;  //For future use
        private bool _isNew = false;
        private bool _isReadOnly = false;
        private int _guID = 0;

        //System Code Key for this Factory.
        private GEnum.SystemCode constCodeKey = GEnum.SystemCode.Inventory;
        public GEnum.SystemCode ConstantCodeKey { get { return constCodeKey; } }

        //Permission ID for this Factory.
        private string constPermID = GVar.PermissionID.Inventory;
        public string PermID { get { return constPermID; } }

        //Event Declaration 
        public GVar.DirtyEvent dirtyEvent = null;
        public GVar.UINotifierEvent ErrorNotifierHeader_Set = null;
        public GVar.UINotifierEvent ErrorNotifierHeader_Clear = null;

        //Template copied from Master Item
        private DataTable dtItemTemplate = null;

        #endregion // Member variables and constant

        #region Factory Properties

        public MSTItm ObjMSTItm
        {
            get
            {
                return this._MSTItm;
            }
        }
        public MSTItmDetAlts ObjMSTItmDetAlts
        {
            get
            {
                return this._MSTItmDetAlts;
            }
        }
        public MSTItmDetAsss ObjMSTItmDetAsss
        {
            get
            {
                return this._MSTItmDetAsss;
            }
        }
        public MSTItmDetBOMs ObjMSTItmDetBOMRMs
        {
            get
            {
                return this._MSTItmDetBOMRMs;
            }
        }
        public MSTItmDetBOMs ObjMSTItmDetBOMPMs
        {
            get
            {
                return this._MSTItmDetBOMPMs;
            }
        }
        public MSTItmDetBOMs ObjMSTItmDetBOMLBs
        {
            get
            {
                return this._MSTItmDetBOMLBs;
            }
        }
        public MSTItmDetLocs ObjMSTItmDetLocs
        {
            get
            {
                return this._MSTItmDetLocs;
            }
        }
        public MSTItmDetPrice ObjMSTItmDetPrice
        {
            get
            {
                return this._MSTItmDetPrice;
            }
        }
        public MSTItmBatchs ObjMSTItmBatchs
        {
            get
            {
                return this._MSTItmBatchs;
            }
        }
        public MSTItmSerials ObjMSTItmSerials
        {
            get
            {
                return this._MSTItmSerials;
            }
        }
        public DataTable ItemTemplate
        {
            get
            {
                return this.dtItemTemplate;
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

        #endregion

        //Constructors, Initialisation
        public MSTItmFactory(GEnum.InstanceMode instanceMode)
        {
            try
            {
                this._instanceMode = instanceMode;
                Initialisation();
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

                            //Get Instance GUID
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

                            //Commit Process 
                            this._MSTItm = new MSTItm();
                            this._MSTItmBatchs = new MSTItmBatchs(cn);
                            this._MSTItmDetAlts = new MSTItmDetAlts(cn);
                            this._MSTItmDetAsss = new MSTItmDetAsss(cn);
                            this._MSTItmDetBOMLBs = new MSTItmDetBOMs(cn);
                            this._MSTItmDetBOMPMs = new MSTItmDetBOMs(cn);
                            this._MSTItmDetBOMRMs = new MSTItmDetBOMs(cn);
                            this._MSTItmDetLocs = new MSTItmDetLocs(cn);
                            this._MSTItmDetPrice = new MSTItmDetPrice();
                            if (SysOptionUtility.DatabaseBranchCode == "OMS" || SysOptionUtility.DatabaseBranchCode == "OMSTW" || SysOptionUtility.DatabaseBranchCode == DBCode.ITS)
                            {
                                this._MSTItmSerials = new MSTItmSerials(cn);
                            }

                            this._isNew = false;
                            this._isReadOnly = false;
                            if (Transaction.Current.TransactionInformation.Status != TransactionStatus.Active) throw new Exception("Transaction has aborted."); scope.Complete();
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

                            //Get Instance GUID
                            //FORM will check for GUID > 0 to indicate Factory is valid
                            if ((this._guID = SysOptionUtility.GetNewLockingGUID(cn)) == 0)
                            {
                                this._guID = -1;
                                return false;
                            }

                            //Locking
                            if (SysLockUtility.CheckInProgressLock(cn, true, constCodeKey))
                            {
                                this._guID = -1;
                                return true;
                            }

                            //Commit Process 
                            this._MSTItm = new MSTItm();
                            this._MSTItmBatchs = new MSTItmBatchs(cn);
                            this._MSTItmDetAlts = new MSTItmDetAlts(cn);
                            this._MSTItmDetAsss = new MSTItmDetAsss(cn);
                            this._MSTItmDetBOMLBs = new MSTItmDetBOMs(cn);
                            this._MSTItmDetBOMPMs = new MSTItmDetBOMs(cn);
                            this._MSTItmDetBOMRMs = new MSTItmDetBOMs(cn);
                            this._MSTItmDetLocs = new MSTItmDetLocs(cn);
                            this._MSTItmDetPrice = new MSTItmDetPrice();
                            if (SysOptionUtility.DatabaseBranchCode == "OMS" || SysOptionUtility.DatabaseBranchCode == "OMSTW" || SysOptionUtility.DatabaseBranchCode == DBCode.ITS)
                            {
                                this._MSTItmSerials = new MSTItmSerials(cn);
                            }

                            this._isNew = false;
                            this._isReadOnly = false;
                            if (Transaction.Current.TransactionInformation.Status != TransactionStatus.Active) throw new Exception("Transaction has aborted."); scope.Complete();
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
        public bool New(int CreateWithThisItmType)
        {
            //If CreateWithThisItmType = 0 it will use the sysoption default itmtype else it will use this itmtype
            //for setting the defaultvalue in the new header object
            #region Declaration
            bool restoreFlag = false;
            int? ItmType = 0;
            MSTItm copyMSTItm = null;
            MSTItmDetAlts copyMSTItmDetAlts = null;
            MSTItmDetAsss copyMSTItmDetAsss = null;
            MSTItmDetBOMs copyMSTItmDetBOMRMs = null;
            MSTItmDetBOMs copyMSTItmDetBOMPMs = null;
            MSTItmDetBOMs copyMSTItmDetBOMLBs = null;
            MSTItmDetLocs copyMSTItmDetLocs = null;
            MSTItmDetPrice copyMSTItmDetPrice = null;
            MSTItmBatchs copyMSTItmBatchs = null;
            MSTItmSerials copyMSTItmSerials = null;
            #endregion

            try
            {

                #region Make backup of objects for restore purpose

                if (this._MSTItm != null)
                    copyMSTItm = this._MSTItm.Clone();

                if (this._MSTItmDetAlts != null)
                    copyMSTItmDetAlts = GFunc.TACopyDataTable(_MSTItmDetAlts);

                if (this._MSTItmDetAsss != null)
                    copyMSTItmDetAsss = GFunc.TACopyDataTable(_MSTItmDetAsss);

                if (this._MSTItmDetBOMRMs != null)
                    copyMSTItmDetBOMRMs = GFunc.TACopyDataTable(_MSTItmDetBOMRMs);

                if (this._MSTItmDetBOMPMs != null)
                    copyMSTItmDetBOMPMs = GFunc.TACopyDataTable(_MSTItmDetBOMPMs);

                if (this._MSTItmDetBOMLBs != null)
                    copyMSTItmDetBOMLBs = GFunc.TACopyDataTable(_MSTItmDetBOMLBs);

                if (this._MSTItmDetLocs != null)
                    copyMSTItmDetLocs = GFunc.TACopyDataTable(_MSTItmDetLocs);

                if (this._MSTItmDetPrice != null)
                    copyMSTItmDetPrice = this._MSTItmDetPrice.Clone();

                if (this._MSTItmBatchs != null)
                    copyMSTItmBatchs = GFunc.TACopyDataTable(_MSTItmBatchs);

                if (this._MSTItmSerials != null)
                    copyMSTItmSerials = GFunc.TACopyDataTable(_MSTItmSerials);
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
                        if (!SysLockUtility.RemoveLockGUIDKeepIP(cn, true, GUID, constCodeKey))
                            return false;


                        //prepare new instance 
                        this._MSTItm = MSTItm.New();
                        this._MSTItm.Attachments = new SYSAttachments();
                        this._MSTItmDetAlts = new MSTItmDetAlts(cn);
                        this._MSTItmDetAsss = new MSTItmDetAsss(cn);
                        this._MSTItmDetBOMRMs = new MSTItmDetBOMs(cn);
                        this._MSTItmDetBOMPMs = new MSTItmDetBOMs(cn);
                        this._MSTItmDetBOMLBs = new MSTItmDetBOMs(cn);
                        this._MSTItmDetLocs = new MSTItmDetLocs(cn);
                        this._MSTItmDetPrice = MSTItmDetPrice.New();
                        this._MSTItmBatchs = new MSTItmBatchs(cn);
                        if (SysOptionUtility.DatabaseBranchCode == "OMS" || SysOptionUtility.DatabaseBranchCode == "OMSTW" || SysOptionUtility.DatabaseBranchCode == DBCode.ITS)
                        {
                            this._MSTItmSerials = new MSTItmSerials(cn);
                        }

                        //Set Default Value
                        if (CreateWithThisItmType == 0)
                            _MSTItm.ItmType = SysOptionUtility.GetInt("ItemType", cn);
                        else
                            _MSTItm.ItmType = CreateWithThisItmType;


                        if (_MSTItm.ItmType == (int)GEnum.ItemType.Master)
                        {
                            if (copyMSTItm != null)
                            {
                                ItmType = copyMSTItm.MasterItmType;
                            }
                        }
                        else
                            ItmType = _MSTItm.ItmType;


                        SetDefaultValue(cn, (int)ItmType);

                        if (Transaction.Current.TransactionInformation.Status != TransactionStatus.Active) throw new Exception("Transaction has aborted."); scope.Complete();
                        #endregion

                        //Set Flags
                        this._isDirty = false;
                        this._isNew = true;
                        this._isReadOnly = false;

                        //Attach Events
                        this._MSTItm.PropertyChanged += new PropertyChangedEventHandler(Obj_PropertyChanged);
                        this._MSTItm.Attachments.ListChanged += new ListChangedEventHandler(Attachments_ListChanged);
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
                    this._MSTItm = copyMSTItm;
                    this._MSTItmDetAlts = copyMSTItmDetAlts;
                    this._MSTItmDetAsss = copyMSTItmDetAsss;
                    this._MSTItmDetBOMRMs = copyMSTItmDetBOMRMs;
                    this._MSTItmDetBOMPMs = copyMSTItmDetBOMPMs;
                    this._MSTItmDetBOMLBs = copyMSTItmDetBOMLBs;
                    this._MSTItmDetLocs = copyMSTItmDetLocs;
                    this._MSTItmDetPrice = copyMSTItmDetPrice;
                    this._MSTItmBatchs = copyMSTItmBatchs;
                    this._MSTItmSerials = copyMSTItmSerials;
                }
                #endregion

                #region Dispose Backup Objects
                copyMSTItm = null;
                copyMSTItmDetAlts = null;
                copyMSTItmDetAsss = null;
                copyMSTItmDetBOMRMs = null;
                copyMSTItmDetBOMPMs = null;
                copyMSTItmDetBOMLBs = null;
                copyMSTItmDetLocs = null;
                copyMSTItmDetPrice = null;
                copyMSTItmBatchs = null;
                copyMSTItmSerials = null;
                #endregion
            }
        }//Completed
        public bool GetEdit(int? ItmKey, string ItmID)
        {
            #region Declaration
            bool restoreFlag = false;
            MSTItm copyMSTItm = null;
            MSTItmDetAlts copyMSTItmDetAlts = null;
            MSTItmDetAsss copyMSTItmDetAsss = null;
            MSTItmDetBOMs copyMSTItmDetBOMRMs = null;
            MSTItmDetBOMs copyMSTItmDetBOMPMs = null;
            MSTItmDetBOMs copyMSTItmDetBOMLBs = null;
            MSTItmDetLocs copyMSTItmDetLocs = null;
            MSTItmDetPrice copyMSTItmDetPrice = null;
            MSTItmBatchs copyMSTItmBatchs = null;
            MSTItmSerials copyMSTItmSerials = null;
            #endregion

            try
            {
                #region Make backup of objects for restore purpose

                if (this._MSTItm != null)
                    copyMSTItm = this._MSTItm.Clone();

                if (this._MSTItmDetAlts != null)
                    copyMSTItmDetAlts = GFunc.TACopyDataTable(_MSTItmDetAlts);

                if (this._MSTItmDetAsss != null)
                    copyMSTItmDetAsss = GFunc.TACopyDataTable(_MSTItmDetAsss);

                if (this._MSTItmDetBOMRMs != null)
                    copyMSTItmDetBOMRMs = GFunc.TACopyDataTable(_MSTItmDetBOMRMs);

                if (this._MSTItmDetBOMPMs != null)
                    copyMSTItmDetBOMPMs = GFunc.TACopyDataTable(_MSTItmDetBOMPMs);

                if (this._MSTItmDetBOMLBs != null)
                    copyMSTItmDetBOMLBs = GFunc.TACopyDataTable(_MSTItmDetBOMLBs);

                if (this._MSTItmDetLocs != null)
                    copyMSTItmDetLocs = GFunc.TACopyDataTable(_MSTItmDetLocs);

                if (this._MSTItmDetPrice != null)
                    copyMSTItmDetPrice = this._MSTItmDetPrice.Clone();

                if (this._MSTItmBatchs != null)
                    copyMSTItmBatchs = GFunc.TACopyDataTable(_MSTItmBatchs);

                if (this._MSTItmSerials != null)
                    copyMSTItmSerials = GFunc.TACopyDataTable(_MSTItmSerials);

                #endregion

                #region Check Security Permission
                if (SECPermUtility.Edit(constPermID, true) == false)
                    return false;
                #endregion

                #region Get ItmKey to open record and check RecordAccess rights
                if (ItmID != null && ItmID != string.Empty)
                    ItmKey = MSTItm.Get(ItmID).ItmKey;

                if (ItmKey == 0)
                    return false;

                if (_MSTItm.CanAccessRecord(ItmKey) == false)
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

                        // Check Lock
                        if (SysLockUtility.IsLock(cn, true, GEnum.SysLockOption.ByCodeKeyandDataKey, constCodeKey, ItmKey, 0, _guID))
                            return false;

                        // Remove Lock
                        if (!SysLockUtility.RemoveLockGUIDKeepIP(cn, true, this._guID, constCodeKey))
                            return false;

                        // Add Lock
                        if (!SysLockUtility.AddLock(cn, true, this._guID, constCodeKey, ItmKey))
                            return false;

                        #region Get Record
                        if (_MSTItm.Fetch(cn, new MSTItm.Criteria(ItmKey, 1)) == false)
                        {
                            MsgBox.Show(cn, MsgID.Common.GetFail);
                            return false;
                        }

                        //Record Not Found
                        if (GFunc.NEInt(this._MSTItm._itmKey, 0) == 0)
                        {
                            restoreFlag = false;
                            throw new TAException(MsgID.Common.GetFail);
                        }

                        _MSTItmDetAlts.Clear();
                        if (_MSTItmDetAlts.Fetch(cn, new MSTItmDetAlts.Criteria(ItmKey, 1)) == false)
                        {
                            MsgBox.Show(cn, MsgID.Common.GetFail);
                            return false;
                        }

                        _MSTItmDetAsss.Clear();
                        if (_MSTItmDetAsss.Fetch(cn, new MSTItmDetAsss.Criteria(ItmKey, 1)) == false)
                        {
                            MsgBox.Show(cn, MsgID.Common.GetFail);
                            return false;
                        }

                        _MSTItmDetBOMRMs.Clear();
                        if (_MSTItmDetBOMRMs.Fetch(cn, new MSTItmDetBOMs.Criteria(ItmKey, GEnum.BOMLineType.Raw_Material, 1)) == false)
                        {
                            MsgBox.Show(cn, MsgID.Common.GetFail);
                            return false;
                        }

                        _MSTItmDetBOMPMs.Clear();
                        if (_MSTItmDetBOMPMs.Fetch(cn, new MSTItmDetBOMs.Criteria(ItmKey, GEnum.BOMLineType.Packing_Material, 1)) == false)
                        {
                            MsgBox.Show(cn, MsgID.Common.GetFail);
                            return false;
                        }

                        _MSTItmDetBOMLBs.Clear();
                        if (_MSTItmDetBOMLBs.Fetch(cn, new MSTItmDetBOMs.Criteria(ItmKey, GEnum.BOMLineType.Labour, 1)) == false)
                        {
                            MsgBox.Show(cn, MsgID.Common.GetFail);
                            return false;
                        }

                        _MSTItmDetLocs.Clear();
                        if (_MSTItmDetLocs.Fetch(cn, new MSTItmDetLocs.Criteria(ItmKey, 1)) == false)
                        {
                            MsgBox.Show(cn, MsgID.Common.GetFail);
                            return false;
                        }

                        if (_MSTItmDetPrice.Fetch(cn, new MSTItmDetPrice.Criteria(ItmKey, 1)) == false)
                        {
                            MsgBox.Show(cn, MsgID.Common.GetFail);
                            return false;
                        }

                        _MSTItmBatchs.Clear();
                        if (_MSTItmBatchs.Fetch(cn, new MSTItmBatchs.Criteria(ItmKey, 3)) == false)
                        {
                            MsgBox.Show(cn, MsgID.Common.GetFail);
                            return false;
                        }

                        if (SysOptionUtility.DatabaseBranchCode == "OMS" || SysOptionUtility.DatabaseBranchCode == "OMSTW" || SysOptionUtility.DatabaseBranchCode == DBCode.ITS)
                        {
                            _MSTItmSerials.Clear();
                            if (_MSTItmSerials.Fetch(cn, new MSTItmSerials.Criteria(ItmKey, 2)) == false)
                            {
                                MsgBox.Show(cn, MsgID.Common.GetFail);
                                return false;
                            }
                        }

                        this._MSTItm.Attachments.Fetch(cn, new SYSAttachments.Criteria((int)this.constCodeKey, this._MSTItm.ItmKey, 1));
                        #endregion

                        if (Transaction.Current.TransactionInformation.Status != TransactionStatus.Active) throw new Exception("Transaction has aborted."); scope.Complete();

                        //Set Flags
                        this._isDirty = false;
                        this._isNew = false;
                        this._isReadOnly = false;

                        //Attach Events
                        this._MSTItm.PropertyChanged += new PropertyChangedEventHandler(Obj_PropertyChanged);
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
                    this._MSTItm = copyMSTItm;
                    this._MSTItmDetAlts = copyMSTItmDetAlts;
                    this._MSTItmDetAsss = copyMSTItmDetAsss;
                    this._MSTItmDetBOMRMs = copyMSTItmDetBOMRMs;
                    this._MSTItmDetBOMPMs = copyMSTItmDetBOMPMs;
                    this._MSTItmDetBOMLBs = copyMSTItmDetBOMLBs;
                    this._MSTItmDetLocs = copyMSTItmDetLocs;
                    this._MSTItmDetPrice = copyMSTItmDetPrice;
                    this._MSTItmBatchs = copyMSTItmBatchs;
                    this._MSTItmSerials = copyMSTItmSerials;
                }
                #endregion

                #region Dispose Backup Objects
                copyMSTItm = null;
                copyMSTItmDetAlts = null;
                copyMSTItmDetAsss = null;
                copyMSTItmDetBOMRMs = null;
                copyMSTItmDetBOMPMs = null;
                copyMSTItmDetBOMLBs = null;
                copyMSTItmDetLocs = null;
                copyMSTItmDetPrice = null;
                copyMSTItmBatchs = null;
                copyMSTItmSerials = null;
                #endregion
            }
        }//Completed
        public bool GetReadOnly(int? ItmKey, string ItmID)
        {
            #region Declaration
            bool restoreFlag = false;
            MSTItm copyMSTItm = null;
            MSTItmDetAlts copyMSTItmDetAlts = null;
            MSTItmDetAsss copyMSTItmDetAsss = null;
            MSTItmDetBOMs copyMSTItmDetBOMRMs = null;
            MSTItmDetBOMs copyMSTItmDetBOMPMs = null;
            MSTItmDetBOMs copyMSTItmDetBOMLBs = null;
            MSTItmDetLocs copyMSTItmDetLocs = null;
            MSTItmDetPrice copyMSTItmDetPrice = null;
            MSTItmBatchs copyMSTItmBatchs = null;
            MSTItmSerials copyMSTItmSerials = null;
            #endregion

            try
            {
                #region Make backup of objects for restore purpose

                if (this._MSTItm != null)
                    copyMSTItm = this._MSTItm.Clone();

                if (this._MSTItmDetAlts != null)
                    copyMSTItmDetAlts = GFunc.TACopyDataTable(_MSTItmDetAlts);

                if (this._MSTItmDetAsss != null)
                    copyMSTItmDetAsss = GFunc.TACopyDataTable(_MSTItmDetAsss);

                if (this._MSTItmDetBOMRMs != null)
                    copyMSTItmDetBOMRMs = GFunc.TACopyDataTable(_MSTItmDetBOMRMs);

                if (this._MSTItmDetBOMPMs != null)
                    copyMSTItmDetBOMPMs = GFunc.TACopyDataTable(_MSTItmDetBOMPMs);

                if (this._MSTItmDetBOMLBs != null)
                    copyMSTItmDetBOMLBs = GFunc.TACopyDataTable(_MSTItmDetBOMLBs);

                if (this._MSTItmDetLocs != null)
                    copyMSTItmDetLocs = GFunc.TACopyDataTable(_MSTItmDetLocs);

                if (this._MSTItmDetPrice != null)
                    copyMSTItmDetPrice = this._MSTItmDetPrice.Clone();

                if (this._MSTItmBatchs != null)
                    copyMSTItmBatchs = GFunc.TACopyDataTable(_MSTItmBatchs);

                if (SysOptionUtility.DatabaseBranchCode == "OMS" || SysOptionUtility.DatabaseBranchCode == "OMSTW" || SysOptionUtility.DatabaseBranchCode == DBCode.ITS)
                {
                    if (this._MSTItmSerials != null)
                        copyMSTItmSerials = GFunc.TACopyDataTable(_MSTItmSerials);
                }
                #endregion

                #region Check Security Permission
                if (SECPermUtility.Read(constPermID, true) == false)
                    return false;
                #endregion

                #region Get ItmKey to open record and check RecordAccess rights
                if (ItmID != null && ItmID != string.Empty)
                    ItmKey = MSTItm.Get(ItmID).ItmKey;

                if (ItmKey == 0)
                    return false;

                if (_MSTItm.CanAccessRecord(ItmKey) == false)
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

                        // Remove all locks by GUID except inprogress Locking
                        if (SysLockUtility.RemoveLockGUIDKeepIP(cn, true, GUID, constCodeKey) == false)
                            return false;

                        #region Get Data
                        if (_MSTItm.Fetch(cn, new MSTItm.Criteria(ItmKey, 1)) == false)
                        {
                            MsgBox.Show(cn, MsgID.Common.GetFail);
                            return false;
                        }

                        _MSTItmDetAlts.Clear();
                        if (_MSTItmDetAlts.Fetch(cn, new MSTItmDetAlts.Criteria(ItmKey, 1)) == false)
                        {
                            MsgBox.Show(cn, MsgID.Common.GetFail);
                            return false;
                        }

                        _MSTItmDetAsss.Clear();
                        if (_MSTItmDetAsss.Fetch(cn, new MSTItmDetAsss.Criteria(ItmKey, 1)) == false)
                        {
                            MsgBox.Show(cn, MsgID.Common.GetFail);
                            return false;
                        }

                        _MSTItmDetBOMRMs.Clear();
                        if (_MSTItmDetBOMRMs.Fetch(cn, new MSTItmDetBOMs.Criteria(ItmKey, GEnum.BOMLineType.Raw_Material, 1)) == false)
                        {
                            MsgBox.Show(cn, MsgID.Common.GetFail);
                            return false;
                        }

                        _MSTItmDetBOMPMs.Clear();
                        if (_MSTItmDetBOMPMs.Fetch(cn, new MSTItmDetBOMs.Criteria(ItmKey, GEnum.BOMLineType.Packing_Material, 1)) == false)
                        {
                            MsgBox.Show(cn, MsgID.Common.GetFail);
                            return false;
                        }

                        _MSTItmDetBOMLBs.Clear();
                        if (_MSTItmDetBOMLBs.Fetch(cn, new MSTItmDetBOMs.Criteria(ItmKey, GEnum.BOMLineType.Labour, 1)) == false)
                        {
                            MsgBox.Show(cn, MsgID.Common.GetFail);
                            return false;
                        }

                        _MSTItmDetLocs.Clear();
                        if (_MSTItmDetLocs.Fetch(cn, new MSTItmDetLocs.Criteria(ItmKey, 1)) == false)
                        {
                            MsgBox.Show(cn, MsgID.Common.GetFail);
                            return false;
                        }

                        if (_MSTItmDetPrice.Fetch(cn, new MSTItmDetPrice.Criteria(ItmKey, 1)) == false)
                        {
                            MsgBox.Show(cn, MsgID.Common.GetFail);
                            return false;
                        }

                        _MSTItmBatchs.Clear();
                        if (_MSTItmBatchs.Fetch(cn, new MSTItmBatchs.Criteria(ItmKey, 3)) == false)
                        {
                            MsgBox.Show(cn, MsgID.Common.GetFail);
                            return false;
                        }

                        if (SysOptionUtility.DatabaseBranchCode == "OMS" || SysOptionUtility.DatabaseBranchCode == "OMSTW" || SysOptionUtility.DatabaseBranchCode == DBCode.ITS)
                        {
                            _MSTItmSerials.Clear();
                            if (_MSTItmSerials.Fetch(cn, new MSTItmSerials.Criteria(ItmKey, 2)) == false)
                            {
                                MsgBox.Show(cn, MsgID.Common.GetFail);
                                return false;
                            }
                        }

                        this._MSTItm.Attachments.Fetch(cn, new SYSAttachments.Criteria((int)this.constCodeKey, this._MSTItm.ItmKey, 1));
                        #endregion

                        if (Transaction.Current.TransactionInformation.Status != TransactionStatus.Active) throw new Exception("Transaction has aborted."); scope.Complete();

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
                    this._MSTItm = copyMSTItm;
                    this._MSTItmDetAlts = copyMSTItmDetAlts;
                    this._MSTItmDetAsss = copyMSTItmDetAsss;
                    this._MSTItmDetBOMRMs = copyMSTItmDetBOMRMs;
                    this._MSTItmDetBOMPMs = copyMSTItmDetBOMPMs;
                    this._MSTItmDetBOMLBs = copyMSTItmDetBOMLBs;
                    this._MSTItmDetLocs = copyMSTItmDetLocs;
                    this._MSTItmDetPrice = copyMSTItmDetPrice;
                    this._MSTItmBatchs = copyMSTItmBatchs;
                    this._MSTItmSerials = copyMSTItmSerials;
                }
                #endregion

                #region Dispose Backup Objects
                copyMSTItm = null;
                copyMSTItmDetAlts = null;
                copyMSTItmDetAsss = null;
                copyMSTItmDetBOMRMs = null;
                copyMSTItmDetBOMPMs = null;
                copyMSTItmDetBOMLBs = null;
                copyMSTItmDetLocs = null;
                copyMSTItmDetPrice = null;
                copyMSTItmBatchs = null;
                copyMSTItmSerials = null;
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

                        // Remove all locks by GUID except inprogress Locking
                        if (!SysLockUtility.RemoveLockGUIDKeepIP(cn, true, GUID, constCodeKey))
                            return false;

                        if (GFunc.IsNE(_MSTItm))
                            _MSTItm = MSTItm.New();
                        GFunc.ConvertDataTableToObject(dtHeader, _MSTItm);

                        _MSTItmDetAlts = new MSTItmDetAlts(cn);
                        GFunc.CopyDataTableToDetailObject(dsDetail.Tables[0], _MSTItmDetAlts);
                        _MSTItmDetAsss = new MSTItmDetAsss(cn);
                        GFunc.CopyDataTableToDetailObject(dsDetail.Tables[1], _MSTItmDetAsss);
                        _MSTItmBatchs = new MSTItmBatchs();
                        GFunc.CopyDataTableToDetailObject(dsDetail.Tables[2], _MSTItmBatchs);
                        _MSTItmDetBOMRMs = new MSTItmDetBOMs(cn);
                        GFunc.CopyDataTableToDetailObject(dsDetail.Tables[3], _MSTItmDetBOMLBs);
                        _MSTItmDetLocs = new MSTItmDetLocs(cn);
                        GFunc.CopyDataTableToDetailObject(dsDetail.Tables[4], _MSTItmDetBOMPMs);
                        _MSTItmDetBOMLBs = new MSTItmDetBOMs(cn);
                        GFunc.CopyDataTableToDetailObject(dsDetail.Tables[5], _MSTItmDetBOMRMs);
                        _MSTItmDetBOMPMs = new MSTItmDetBOMs(cn);
                        GFunc.CopyDataTableToDetailObject(dsDetail.Tables[6], _MSTItmDetLocs);

                        if (SysOptionUtility.DatabaseBranchCode == "OMS" || SysOptionUtility.DatabaseBranchCode == "OMSTW" || SysOptionUtility.DatabaseBranchCode == DBCode.ITS)
                        {
                            _MSTItmSerials = new MSTItmSerials();
                            GFunc.CopyDataTableToDetailObject(dsDetail.Tables[7], _MSTItmSerials);
                        }
                        // _MSTItmDetPrice = new MSTItmDetPrice();//commented cos' of Audit log Detail  contains only 6 tables
                        //GFunc.ConvertDataTableToObject(dsDetail.Tables[7], _MSTItmDetPrice);

                        // GFunc.ConvertDataTableToObject(dsDetail.Tables[8], _MSTItm.Attachments);




                        if (Transaction.Current.TransactionInformation.Status != TransactionStatus.Active) throw new Exception("Transaction has aborted."); scope.Complete();

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
            //So for example : Item Location, the above will not work
            #region Declaration
            bool restoreFlag = false;
            bool isNewRecord = this.IsNew;//Because this flag will be changed during saving, we need to know the original value when error occurs
            int? newItmKey = 0;
            string autoID = string.Empty;
            string msgID = string.Empty;
            MSTItm copyMSTItm = null;
            MSTItmDetAlts copyMSTItmDetAlts = null;
            MSTItmDetAsss copyMSTItmDetAsss = null;
            MSTItmDetBOMs copyMSTItmDetBOMRMs = null;
            MSTItmDetBOMs copyMSTItmDetBOMPMs = null;
            MSTItmDetBOMs copyMSTItmDetBOMLBs = null;
            MSTItmDetLocs copyMSTItmDetLocs = null;
            MSTItmDetPrice copyMSTItmDetPrice = null;
            MSTItmBatchs copyMSTItmBatchs = null;
            MSTItmSerials copyMSTItmSerials = null;

            #endregion

            try
            {
                #region Make backup of objects for restore purpose

                if (this._MSTItm != null)
                    copyMSTItm = this._MSTItm.Clone();

                if (this._MSTItmDetAlts != null)
                    copyMSTItmDetAlts = GFunc.TACopyDataTable(_MSTItmDetAlts);

                if (this._MSTItmDetAsss != null)
                    copyMSTItmDetAsss = GFunc.TACopyDataTable(_MSTItmDetAsss);

                if (this._MSTItmDetBOMRMs != null)
                    copyMSTItmDetBOMRMs = GFunc.TACopyDataTable(_MSTItmDetBOMRMs);

                if (this._MSTItmDetBOMPMs != null)
                    copyMSTItmDetBOMPMs = GFunc.TACopyDataTable(_MSTItmDetBOMPMs);

                if (this._MSTItmDetBOMLBs != null)
                    copyMSTItmDetBOMLBs = GFunc.TACopyDataTable(_MSTItmDetBOMLBs);

                if (this._MSTItmDetLocs != null)
                    copyMSTItmDetLocs = GFunc.TACopyDataTable(_MSTItmDetLocs);

                if (this._MSTItmDetPrice != null)
                    copyMSTItmDetPrice = this._MSTItmDetPrice.Clone();

                if (this._MSTItmBatchs != null)
                    copyMSTItmBatchs = GFunc.TACopyDataTable(_MSTItmBatchs);

                if (SysOptionUtility.DatabaseBranchCode == "OMS" || SysOptionUtility.DatabaseBranchCode == "OMSTW" || SysOptionUtility.DatabaseBranchCode == DBCode.ITS)
                {
                    if (this._MSTItmSerials != null)
                        copyMSTItmSerials = GFunc.TACopyDataTable(_MSTItmSerials);
                }
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
                        if (this.IsNew && GFunc.IsNE(_MSTItm._itmID))
                        {
                            if (SysIDCounterUtility.Get(cn, true, out autoID, constCodeKey, _MSTItm._itmDes) == false)
                                return false;

                            _MSTItm._itmID = autoID;
                        }
                        #endregion

                        #region Set default value for fields that cannot be empty but can have a general default value

                        DateTime svrDateTime = GFunc.GetSvrDateTime(cn);//Get Server Date and Time (sdt)
                        _MSTItm._masterItmKey = GFunc.NEInt(_MSTItm.MasterItmKey, 0);
                        _MSTItm._substituteItmKey = GFunc.NEInt(_MSTItm.SubstituteItmKey, 0);
                        _MSTItm._accessLevel = GFunc.NEInt(_MSTItm.AccessLevel, 0);
                        _MSTItm._accessGroup = GFunc.NEInt(_MSTItm.AccessGroup, 0);
                        _MSTItm._cSGVendorKey = GFunc.NEInt(_MSTItm.CSGVendorKey, 0);
                        _MSTItm._catKey1 = GFunc.NEInt(_MSTItm.CatKey1, 0);
                        _MSTItm._catKey2 = GFunc.NEInt(_MSTItm.CatKey2, 0);
                        _MSTItm._catKey3 = GFunc.NEInt(_MSTItm.CatKey3, 0);
                        _MSTItm._catKey4 = GFunc.NEInt(_MSTItm.CatKey4, 0);
                        _MSTItm._catKey5 = GFunc.NEInt(_MSTItm.CatKey5, 0);
                        _MSTItm._branchKey = GFunc.NEInt(_MSTItm.BranchKey, 0);
                        _MSTItm._deptKey = GFunc.NEInt(_MSTItm.DeptKey, 0);
                        _MSTItm._openBalCost = GFunc.NEDec(_MSTItm.OpenBalCost, 0);
                        _MSTItm._openBalQty = GFunc.NEDec(_MSTItm.OpenBalQty, 0);
                        _MSTItm._openBalAmtH = GFunc.NEDec(_MSTItm.OpenBalAmtH, 0);
                        switch (_MSTItm.ItmType)
                        {
                            case (int)GEnum.ItemType.Stock:
                            case (int)GEnum.ItemType.StockB:
                            case (int)GEnum.ItemType.Finished_GD:
                            case (int)GEnum.ItemType.Finished_GDB:
                            case (int)GEnum.ItemType.Assembly:
                            case (int)GEnum.ItemType.Serial_StockB:
                            case (int)GEnum.ItemType.Serial_Finished_GDB:
                            case (int)GEnum.ItemType.Consignment:
                            case (int)GEnum.ItemType.Non_Stock:
                            case (int)GEnum.ItemType.Service:
                            case (int)GEnum.ItemType.Charges:
                            case (int)GEnum.ItemType.Discount:
                                _MSTItm._costLatest = GFunc.NEDec(_MSTItm.CostLatest, 0);
                                _MSTItm._costLandedDate = GFunc.NEDateTime(_MSTItm.CostLandedDate, DateTime.Today.Date);
                                break;
                        }

                        _MSTItm._createDate = GFunc.NEDateTime(_MSTItm.CreateDate, svrDateTime);
                        _MSTItm._createUserKey = GFunc.NEInt(_MSTItm.CreateUserKey, AppInfor.currentUserKey);
                        _MSTItm._lastModifiedDate = svrDateTime;
                        _MSTItm._lastModifiedUserKey = AppInfor.currentUserKey;
                        if (_MSTItm.ScaleSizeNum == null || _MSTItm.ScaleSizeNum < 1)
                            _MSTItm._scaleSizeNum = 1;

                        //_MSTItmDetAlts
                        foreach (DataRow dr in _MSTItmDetAlts.Rows)
                        {
                            dr["CreateDate"] = GFunc.NEDateTime(dr["CreateDate"], svrDateTime);
                            dr["CreateUserKey"] = GFunc.NEInt(dr["CreateUserKey"], AppInfor.currentUserKey);
                            dr["LastModifiedDate"] = svrDateTime;
                            dr["LastModifiedUserKey"] = AppInfor.currentUserKey;
                        }
                        //_MSTItmDetAsss
                        foreach (DataRow dr in _MSTItmDetAsss.Rows)
                        {
                            dr["CreateDate"] = GFunc.NEDateTime(dr["CreateDate"], svrDateTime);
                            dr["CreateUserKey"] = GFunc.NEInt(dr["CreateUserKey"], AppInfor.currentUserKey);
                            dr["LastModifiedDate"] = svrDateTime;
                            dr["LastModifiedUserKey"] = AppInfor.currentUserKey;
                        }
                        //_MSTItmDetBOMLBs
                        foreach (DataRow dr in _MSTItmDetBOMLBs.Rows)
                        {
                            dr["BOMQty"] = GFunc.NEDec(dr["BOMQty"], 0);
                            dr["BOMLabourCost"] = GFunc.NEDec(dr["BOMLabourCost"], 0);
                            dr["CreateDate"] = GFunc.NEDateTime(dr["CreateDate"], svrDateTime);
                            dr["CreateUserKey"] = GFunc.NEInt(dr["CreateUserKey"], AppInfor.currentUserKey);
                            dr["LastModifiedDate"] = svrDateTime;
                            dr["LastModifiedUserKey"] = AppInfor.currentUserKey;
                        }
                        //_MSTItmDetBOMPMs
                        foreach (DataRow dr in _MSTItmDetBOMPMs.Rows)
                        {
                            dr["BOMQty"] = GFunc.NEDec(dr["BOMQty"], 0);
                            dr["BOMLabourCost"] = GFunc.NEDec(dr["BOMLabourCost"], 0);
                            dr["CreateDate"] = GFunc.NEDateTime(dr["CreateDate"], svrDateTime);
                            dr["CreateUserKey"] = GFunc.NEInt(dr["CreateUserKey"], AppInfor.currentUserKey);
                            dr["LastModifiedDate"] = svrDateTime;
                            dr["LastModifiedUserKey"] = AppInfor.currentUserKey;
                        }
                        //_MSTItmDetBOMRMs
                        foreach (DataRow dr in _MSTItmDetBOMRMs.Rows)
                        {
                            dr["BOMQty"] = GFunc.NEDec(dr["BOMQty"], 0);
                            dr["BOMLabourCost"] = GFunc.NEDec(dr["BOMLabourCost"], 0);
                            dr["CreateDate"] = GFunc.NEDateTime(dr["CreateDate"], svrDateTime);
                            dr["CreateUserKey"] = GFunc.NEInt(dr["CreateUserKey"], AppInfor.currentUserKey);
                            dr["LastModifiedDate"] = svrDateTime;
                            dr["LastModifiedUserKey"] = AppInfor.currentUserKey;
                        }
                        //_MSTItmDetLocs
                        foreach (DataRow dr in _MSTItmDetLocs.Rows)
                        {
                            dr["LocQtyMin"] = GFunc.NEDec(dr["LocQtyMin"], 0);
                            dr["LocQtyMax"] = GFunc.NEDec(dr["LocQtyMax"], 0);
                            dr["LocQtyOpenBal"] = GFunc.NEDec(dr["LocQtyOpenBal"], 0);
                            dr["CreateDate"] = GFunc.NEDateTime(dr["CreateDate"], svrDateTime);
                            dr["CreateUserKey"] = GFunc.NEInt(dr["CreateUserKey"], AppInfor.currentUserKey);
                            dr["LastModifiedDate"] = svrDateTime;
                            dr["LastModifiedUserKey"] = AppInfor.currentUserKey;
                        }

                        //Generate SKU1
                        if (this.IsNew)
                        {
                            if (GenerateBarCode(out _MSTItm._sku1, cn) == false)
                                return false;
                        }

                        #endregion

                        #region Validation
                        if (Validation_Header(cn) == false)
                            return false;

                        if (Validation_Detail("tagrdDetAssembly", (DataTable)this.ObjMSTItmDetAsss, cn) == false)
                            return false;
                        if (Validation_Detail("tagrdDetAlternates", (DataTable)this.ObjMSTItmDetAlts, cn) == false)
                            return false;
                        if (Validation_Detail("tagrdItmDetBOMPMs", (DataTable)this.ObjMSTItmDetBOMPMs, cn) == false)
                            return false;
                        if (Validation_Detail("tagrdItmDetBOMRMs", (DataTable)this.ObjMSTItmDetBOMRMs, cn) == false)
                            return false;
                        if (Validation_Detail("tagrdItmDetBOMLabour", (DataTable)this.ObjMSTItmDetBOMLBs, cn) == false)
                            return false;
                        if (Validation_Detail("tagrdDetLocaton", (DataTable)this.ObjMSTItmDetLocs, cn) == false)
                            return false;

                        #endregion

                        #region Save Record

                        if (IsNew)
                        {
                            if (!_MSTItm.Insert(cn, out newItmKey))
                                return false;

                            //For Stock item we need to add a record to INLedger
                            switch (_MSTItm.ItmType)
                            {
                                case (int)GEnum.ItemType.Serial_Finished_GDB:
                                case (int)GEnum.ItemType.Serial_StockB:
                                case (int)GEnum.ItemType.Finished_GD:
                                case (int)GEnum.ItemType.Stock:
                                    if (!_MSTItm.InsertOpeningLedger(cn, newItmKey))
                                        return false;
                                    break;
                            }

                            if (!_MSTItmDetAlts.Insert(cn, newItmKey))
                                return false;
                            if (!_MSTItmDetAsss.Insert(cn, newItmKey))
                                return false;
                            if (!_MSTItmDetPrice.Insert(cn, newItmKey))
                                return false;
                            if (!_MSTItmDetBOMRMs.Insert(cn, newItmKey))
                                return false;
                            if (!_MSTItmDetBOMPMs.Insert(cn, newItmKey))
                                return false;
                            if (!_MSTItmDetBOMLBs.Insert(cn, newItmKey))
                                return false;

                            if (_MSTItm.Attachments != null)
                            {
                                foreach (SYSAttachment obj in _MSTItm.Attachments)
                                {
                                    obj._docDK = newItmKey;
                                }
                                DocUtility.AttachmentSave(cn, _MSTItm.Attachments, this.constCodeKey, _MSTItm.ItmKey);
                            }
                        }
                        else
                        {
                            if (!_MSTItm.Update(cn))
                                return false;

                            if (!_MSTItmDetAlts.Delete(cn, new MSTItmDetAlts.Criteria(_MSTItm._itmKey, 0)))
                                return false;
                            if (!_MSTItmDetAlts.Insert(cn, _MSTItm._itmKey))
                                return false;

                            if (!_MSTItmDetAsss.Delete(cn, new MSTItmDetAsss.Criteria(_MSTItm._itmKey, 0)))
                                return false;
                            if (!_MSTItmDetAsss.Insert(cn, _MSTItm._itmKey))
                                return false;

                            if (!_MSTItmDetLocs.Update(cn))
                                return false;

                            _MSTItmDetPrice._itmKey = _MSTItm._itmKey;
                            if (!_MSTItmDetPrice.Update(cn))
                                return false;

                            if (!_MSTItmDetBOMRMs.Delete(cn, new MSTItmDetBOMs.Criteria(_MSTItm._itmKey, GEnum.BOMLineType.Raw_Material, 0)))
                                return false;
                            if (!_MSTItmDetBOMRMs.Insert(cn, _MSTItm._itmKey))
                                return false;

                            if (!_MSTItmDetBOMPMs.Delete(cn, new MSTItmDetBOMs.Criteria(_MSTItm._itmKey, GEnum.BOMLineType.Packing_Material, 0)))
                                return false;
                            if (!_MSTItmDetBOMPMs.Insert(cn, _MSTItm._itmKey))
                                return false;

                            if (!_MSTItmDetBOMLBs.Delete(cn, new MSTItmDetBOMs.Criteria(_MSTItm._itmKey, GEnum.BOMLineType.Labour, 0)))
                                return false;
                            if (!_MSTItmDetBOMLBs.Insert(cn, _MSTItm._itmKey))
                                return false;

                            if (_MSTItm.Attachments != null)
                            {
                                DocUtility.AttachmentSave(cn, _MSTItm.Attachments, this.constCodeKey, _MSTItm.ItmKey);
                            }
                        }
                        #endregion

                        #region For New Record perform: Locking, set new recordKey
                        if (IsNew)
                        {
                            if (SysLockUtility.AddLock(cn, true, GUID, constCodeKey, newItmKey))
                                _MSTItm._itmKey = newItmKey;
                            else
                                return false;
                        }
                        #endregion

                        if (Transaction.Current.TransactionInformation.Status != TransactionStatus.Active) throw new Exception("Transaction has aborted."); scope.Complete();

                        #region Set Flags
                        this._isDirty = false;
                        this._isNew = false;
                        #endregion

                        #endregion
                    }
                }

                #region Update Auditlog
                if (SysOptionUtility.DatabaseBranchCode == "OMS" || SysOptionUtility.DatabaseBranchCode == "OMSTW" || SysOptionUtility.DatabaseBranchCode == DBCode.ITS)
                {
                    if (isNewRecord)
                        SysAuditLogUtility.AddAuditLog(GEnum.AuditLogMode.Add, constCodeKey, _MSTItm.ItmKey, _MSTItm.ItmID, GFunc.GetCodeKeyDescription((int)ConstantCodeKey), new object[] { _MSTItm, _MSTItmDetAsss, _MSTItmBatchs, _MSTItmSerials, _MSTItmDetBOMLBs, _MSTItmDetBOMPMs, _MSTItmDetBOMRMs, _MSTItmDetLocs, _MSTItmDetPrice, _MSTItm.Attachments });
                    else
                        SysAuditLogUtility.AddAuditLog(GEnum.AuditLogMode.Edit, constCodeKey, _MSTItm.ItmKey, _MSTItm.ItmID, GFunc.GetCodeKeyDescription((int)ConstantCodeKey), new object[] { _MSTItm, _MSTItmDetAsss, _MSTItmBatchs, _MSTItmSerials, _MSTItmDetBOMLBs, _MSTItmDetBOMPMs, _MSTItmDetBOMRMs, _MSTItmDetLocs, _MSTItmDetPrice, _MSTItm.Attachments });
                }
                else
                {
                    if (isNewRecord)
                        SysAuditLogUtility.AddAuditLog(GEnum.AuditLogMode.Add, constCodeKey, _MSTItm.ItmKey, _MSTItm.ItmID, GFunc.GetCodeKeyDescription((int)ConstantCodeKey), new object[] { _MSTItm, _MSTItmDetAlts, _MSTItmDetAsss, _MSTItmBatchs, _MSTItmDetBOMLBs, _MSTItmDetBOMPMs, _MSTItmDetBOMRMs, _MSTItmDetLocs, _MSTItmDetPrice, _MSTItm.Attachments });
                    else
                        SysAuditLogUtility.AddAuditLog(GEnum.AuditLogMode.Edit, constCodeKey, _MSTItm.ItmKey, _MSTItm.ItmID, GFunc.GetCodeKeyDescription((int)ConstantCodeKey), new object[] { _MSTItm, _MSTItmDetAlts, _MSTItmDetAsss, _MSTItmBatchs, _MSTItmDetBOMLBs, _MSTItmDetBOMPMs, _MSTItmDetBOMRMs, _MSTItmDetLocs, _MSTItmDetPrice, _MSTItm.Attachments });
                }
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
                    this._MSTItm = copyMSTItm;
                    this._MSTItmDetAlts = copyMSTItmDetAlts;
                    this._MSTItmDetAsss = copyMSTItmDetAsss;
                    this._MSTItmDetBOMRMs = copyMSTItmDetBOMRMs;
                    this._MSTItmDetBOMPMs = copyMSTItmDetBOMPMs;
                    this._MSTItmDetBOMLBs = copyMSTItmDetBOMLBs;
                    this._MSTItmDetLocs = copyMSTItmDetLocs;
                    this._MSTItmDetPrice = copyMSTItmDetPrice;
                    this._MSTItmBatchs = copyMSTItmBatchs;
                    this._MSTItmSerials = copyMSTItmSerials;
                }
                #endregion

                #region Dispose Backup Objects
                copyMSTItm = null;
                copyMSTItmDetAlts = null;
                copyMSTItmDetAsss = null;
                copyMSTItmDetBOMRMs = null;
                copyMSTItmDetBOMPMs = null;
                copyMSTItmDetBOMLBs = null;
                copyMSTItmDetLocs = null;
                copyMSTItmDetPrice = null;
                copyMSTItmBatchs = null;
                copyMSTItmSerials = null;
                #endregion
            }
        }//Completed
        public bool Delete()
        {
            #region Declaration
            bool restoreFlag = false;
            MSTItm copyMSTItm = null;
            MSTItmDetAlts copyMSTItmDetAlts = null;
            MSTItmDetAsss copyMSTItmDetAsss = null;
            MSTItmDetBOMs copyMSTItmDetBOMRMs = null;
            MSTItmDetBOMs copyMSTItmDetBOMPMs = null;
            MSTItmDetBOMs copyMSTItmDetBOMLBs = null;
            MSTItmDetLocs copyMSTItmDetLocs = null;
            MSTItmDetPrice copyMSTItmDetPrice = null;
            MSTItmBatchs copyMSTItmBatchs = null;
            MSTItmSerials copyMSTItmSerials = null;
            #endregion

            try
            {

                #region Make backup of objects for restore purpose

                if (this._MSTItm != null)
                    copyMSTItm = this._MSTItm.Clone();

                if (this._MSTItmDetAlts != null)
                    copyMSTItmDetAlts = GFunc.TACopyDataTable(_MSTItmDetAlts);

                if (this._MSTItmDetAsss != null)
                    copyMSTItmDetAsss = GFunc.TACopyDataTable(_MSTItmDetAsss);

                if (this._MSTItmDetBOMRMs != null)
                    copyMSTItmDetBOMRMs = GFunc.TACopyDataTable(_MSTItmDetBOMRMs);

                if (this._MSTItmDetBOMPMs != null)
                    copyMSTItmDetBOMPMs = GFunc.TACopyDataTable(_MSTItmDetBOMPMs);

                if (this._MSTItmDetBOMLBs != null)
                    copyMSTItmDetBOMLBs = GFunc.TACopyDataTable(_MSTItmDetBOMLBs);

                if (this._MSTItmDetLocs != null)
                    copyMSTItmDetLocs = GFunc.TACopyDataTable(_MSTItmDetLocs);

                if (this._MSTItmDetPrice != null)
                    copyMSTItmDetPrice = this._MSTItmDetPrice.Clone();

                if (this._MSTItmBatchs != null)
                    copyMSTItmBatchs = GFunc.TACopyDataTable(_MSTItmBatchs);

                if (SysOptionUtility.DatabaseBranchCode == "OMS" || SysOptionUtility.DatabaseBranchCode == "OMSTW" || SysOptionUtility.DatabaseBranchCode == DBCode.ITS)
                {
                    if (this._MSTItmSerials != null)
                        copyMSTItmSerials = GFunc.TACopyDataTable(_MSTItmSerials);
                }
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
                        if (SysLockUtility.CheckAddLock(cn, true, 0, constCodeKey, _MSTItm._itmKey, GUID) == false)
                            return false;

                        //Check the record is used in other dependency tables
                        if (GFunc.CheckKeyDependantsExists(cn, "ItmKey", _MSTItm._itmKey.Value, _MSTItm._itmID))
                            return false;

                        //Delete Record
                        if (_MSTItm.Delete(cn, new MSTItm.Criteria(_MSTItm._itmKey)) == false)
                        {
                            MsgBox.Show(cn, MsgID.Common.DeleteFail);
                            return false;
                        }

                        //Delete From Inventory Ledger
                        if (_MSTItm.DeleteOpeningLedger(cn, new MSTItm.Criteria(_MSTItm._itmKey)) == false)
                            return false;

                        // Remove Lock
                        if (SysLockUtility.RemoveLockGUIDKeepIP(cn, true, GUID, constCodeKey) == false)
                            return false;

                        // Create New
                        this._MSTItm = MSTItm.New();
                        this._MSTItmDetAlts = new MSTItmDetAlts(cn);
                        this._MSTItmDetAsss = new MSTItmDetAsss(cn);
                        this._MSTItmDetBOMRMs = new MSTItmDetBOMs(cn);
                        this._MSTItmDetBOMPMs = new MSTItmDetBOMs(cn);
                        this._MSTItmDetBOMLBs = new MSTItmDetBOMs(cn);
                        this._MSTItmDetLocs = new MSTItmDetLocs(cn);
                        this._MSTItmDetPrice = MSTItmDetPrice.New();
                        this._MSTItmBatchs = new MSTItmBatchs(cn);
                        if (SysOptionUtility.DatabaseBranchCode == "OMS" || SysOptionUtility.DatabaseBranchCode == "OMSTW" || SysOptionUtility.DatabaseBranchCode == DBCode.ITS)
                        {
                            this._MSTItmSerials = new MSTItmSerials(cn);
                        }

                        if (Transaction.Current.TransactionInformation.Status != TransactionStatus.Active) throw new Exception("Transaction has aborted."); scope.Complete();

                        #region Set Flag
                        this._isDirty = false;
                        this._isNew = true;
                        this._isReadOnly = false;
                        #endregion

                        #endregion
                    }
                }

                // AuditLog
                if (SysOptionUtility.DatabaseBranchCode == "OMS" || SysOptionUtility.DatabaseBranchCode == "OMSTW" || SysOptionUtility.DatabaseBranchCode == DBCode.ITS)
                {
                    SysAuditLogUtility.AddAuditLog(GEnum.AuditLogMode.Delete, constCodeKey, copyMSTItm.ItmKey, copyMSTItm.ItmID, GFunc.GetCodeKeyDescription((int)ConstantCodeKey), new object[] { copyMSTItm, copyMSTItmDetAsss, copyMSTItmBatchs, copyMSTItmSerials, copyMSTItmDetBOMLBs, copyMSTItmDetBOMPMs, copyMSTItmDetBOMRMs, copyMSTItmDetLocs, copyMSTItmDetPrice });
                }
                else
                {
                    SysAuditLogUtility.AddAuditLog(GEnum.AuditLogMode.Delete, constCodeKey, copyMSTItm.ItmKey, copyMSTItm.ItmID, GFunc.GetCodeKeyDescription((int)ConstantCodeKey), new object[] { copyMSTItm, copyMSTItmDetAlts, copyMSTItmDetAsss, copyMSTItmBatchs, copyMSTItmDetBOMLBs, copyMSTItmDetBOMPMs, copyMSTItmDetBOMRMs, copyMSTItmDetLocs, copyMSTItmDetPrice });
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
                    this._MSTItm = copyMSTItm;
                    this._MSTItmDetAlts = copyMSTItmDetAlts;
                    this._MSTItmDetAsss = copyMSTItmDetAsss;
                    this._MSTItmDetBOMRMs = copyMSTItmDetBOMRMs;
                    this._MSTItmDetBOMPMs = copyMSTItmDetBOMPMs;
                    this._MSTItmDetBOMLBs = copyMSTItmDetBOMLBs;
                    this._MSTItmDetLocs = copyMSTItmDetLocs;
                    this._MSTItmDetPrice = copyMSTItmDetPrice;
                    this._MSTItmBatchs = copyMSTItmBatchs;
                    this._MSTItmSerials = copyMSTItmSerials;
                }
                #endregion

                #region Dispose Backup Objects
                copyMSTItm = null;
                copyMSTItmDetAlts = null;
                copyMSTItmDetAsss = null;
                copyMSTItmDetBOMRMs = null;
                copyMSTItmDetBOMPMs = null;
                copyMSTItmDetBOMLBs = null;
                copyMSTItmDetLocs = null;
                copyMSTItmDetPrice = null;
                copyMSTItmBatchs = null;
                copyMSTItmSerials = null;
                #endregion
            }
        }//Completed
        public bool Dispose()
        {
            try
            {
                if (SysLockUtility.RemoveLock(true, GEnum.SysLockOption.ByGUID, constCodeKey, GUID, 0, 0) == false)
                    return false;
                else
                    return true;
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

        }//Completed

        //Functions
        private void SetDefaultValue(SqlConnection cn, int ItmType)
        {
            try
            {
                switch (ItmType)
                {
                    #region Stock
                    case (int)GEnum.ItemType.Stock:
                    case (int)GEnum.ItemType.Finished_GD:
                    case (int)GEnum.ItemType.Serial_StockB:
                    case (int)GEnum.ItemType.Serial_Finished_GDB:
                        _MSTItm.AccICKey = SysOptionUtility.GetInt("ItemAccIC", cn);
                        _MSTItm.AccINKey = SysOptionUtility.GetInt("ItemAccIN", cn);
                        _MSTItm.AccDSICKey = SysOptionUtility.GetInt("DefaultAccDirectShipmentSales", cn);
                        _MSTItm.AccDSPHKey = SysOptionUtility.GetInt("DefaultAccDirectShipmentPurchase", cn);
                        _MSTItm.Brandkey = SysOptionUtility.GetNullInt("ItemBrand", cn);
                        _MSTItm.BUOMKey = SysOptionUtility.GetNullInt("ItemBUOM", cn);
                        _MSTItm.INClass = SysOptionUtility.GetStr("ItemClass", cn);
                        _MSTItm.CatKey1 = SysOptionUtility.GetInt("ItemCategory1", cn);
                        _MSTItm.CatKey2 = SysOptionUtility.GetInt("ItemCategory2", cn);
                        _MSTItm.CatKey3 = SysOptionUtility.GetInt("ItemCategory3", cn);
                        _MSTItm.CatKey4 = SysOptionUtility.GetInt("ItemCategory4", cn);
                        _MSTItm.CatKey5 = SysOptionUtility.GetInt("ItemCategory5", cn);
                        _MSTItm.CostMethod = SysOptionUtility.GetInt("ItemCostMethod", cn);
                        _MSTItm.QtyMax = SysOptionUtility.GetDec("ItemMaxStk", cn);
                        _MSTItm.QtyMin = SysOptionUtility.GetDec("ItemMinStk", cn);
                        _MSTItm.QtyReOrder = SysOptionUtility.GetDec("ItemReOrderQty", cn);
                        _MSTItm.DefLocPurchase = SysOptionUtility.GetNullInt("ItemPurchaseLocation", cn);
                        _MSTItm.DefLocSale = SysOptionUtility.GetNullInt("ItemSaleLocation", cn);
                        _MSTItm.WeightUOMKey = SysOptionUtility.GetNullInt("ItemWeightUOMKey", cn);
                        switch (SysOptionUtility.InventoryValuationMethod)
                        {
                            case (int)GEnum.InventoryValuationMethod.Continuous:
                            case (int)GEnum.InventoryValuationMethod.COSBatchPosting:
                                _MSTItm.AccPHKey = SysOptionUtility.GetInt("ItemAccCOS", cn);
                                break;
                            default:
                                _MSTItm.AccPHKey = SysOptionUtility.GetInt("ItemAccPH", cn);
                                break;
                        }
                        break;
                    #endregion

                    #region Batch
                    case (int)GEnum.ItemType.StockB:
                    case (int)GEnum.ItemType.Finished_GDB:
                        _MSTItm.AccICKey = SysOptionUtility.GetInt("ItemAccIC", cn);
                        _MSTItm.AccINKey = SysOptionUtility.GetInt("ItemAccIN", cn);
                        _MSTItm.Brandkey = SysOptionUtility.GetInt("ItemBrand", cn);
                        _MSTItm.BUOMKey = SysOptionUtility.GetInt("ItemBUOM", cn);
                        _MSTItm.INClass = SysOptionUtility.GetStr("ItemClass", cn);
                        _MSTItm.CatKey1 = SysOptionUtility.GetInt("ItemCategory1", cn);
                        _MSTItm.CatKey2 = SysOptionUtility.GetInt("ItemCategory2", cn);
                        _MSTItm.CatKey3 = SysOptionUtility.GetInt("ItemCategory3", cn);
                        _MSTItm.CatKey4 = SysOptionUtility.GetInt("ItemCategory4", cn);
                        _MSTItm.CatKey5 = SysOptionUtility.GetInt("ItemCategory5", cn);
                        _MSTItm.CostMethod = 20;//Fix to FIFO method
                        _MSTItm.QtyMax = SysOptionUtility.GetDec("ItemMaxStk", cn);
                        _MSTItm.QtyMin = SysOptionUtility.GetDec("ItemMinStk", cn);
                        _MSTItm.QtyReOrder = SysOptionUtility.GetDec("ItemReOrderQty", cn);
                        _MSTItm.DefLocPurchase = SysOptionUtility.GetInt("ItemPurchaseLocation", cn);
                        _MSTItm.DefLocSale = SysOptionUtility.GetInt("ItemSaleLocation", cn);
                        _MSTItm.WeightUOMKey = SysOptionUtility.GetInt("ItemWeightUOMKey", cn);
                        switch (SysOptionUtility.InventoryValuationMethod)
                        {
                            case (int)GEnum.InventoryValuationMethod.Continuous:
                            case (int)GEnum.InventoryValuationMethod.COSBatchPosting:
                                _MSTItm.AccPHKey = SysOptionUtility.GetInt("ItemAccCOS", cn);
                                break;
                            default:
                                _MSTItm.AccPHKey = SysOptionUtility.GetInt("ItemAccPH", cn);
                                break;
                        }
                        break;
                    #endregion

                    #region Consignment
                    case (int)GEnum.ItemType.Consignment:
                        _MSTItm.AccICKey = SysOptionUtility.GetInt("ItemAccIC", cn);
                        _MSTItm.AccINKey = null;
                        _MSTItm.Brandkey = SysOptionUtility.GetInt("ItemBrand", cn);
                        _MSTItm.BUOMKey = SysOptionUtility.GetInt("ItemBUOM", cn);
                        _MSTItm.INClass = SysOptionUtility.GetStr("ItemClass", cn);
                        _MSTItm.CatKey1 = SysOptionUtility.GetInt("ItemCategory1", cn);
                        _MSTItm.CatKey2 = SysOptionUtility.GetInt("ItemCategory2", cn);
                        _MSTItm.CatKey3 = SysOptionUtility.GetInt("ItemCategory3", cn);
                        _MSTItm.CatKey4 = SysOptionUtility.GetInt("ItemCategory4", cn);
                        _MSTItm.CatKey5 = SysOptionUtility.GetInt("ItemCategory5", cn);
                        _MSTItm.CostMethod = null;
                        _MSTItm.QtyMax = SysOptionUtility.GetDec("ItemMaxStk", cn);
                        _MSTItm.QtyMin = SysOptionUtility.GetDec("ItemMinStk", cn);
                        _MSTItm.QtyReOrder = SysOptionUtility.GetDec("ItemReOrderQty", cn);
                        _MSTItm.DefLocPurchase = SysOptionUtility.GetInt("ItemPurchaseLocation", cn);
                        _MSTItm.DefLocSale = SysOptionUtility.GetInt("ItemSaleLocation", cn);
                        _MSTItm.WeightUOMKey = SysOptionUtility.GetInt("ItemWeightUOMKey", cn);
                        _MSTItm.AccPHKey = null;
                        break;
                    #endregion

                    #region Non Stock
                    case (int)GEnum.ItemType.Assembly:
                    case (int)GEnum.ItemType.Non_Stock:
                    case (int)GEnum.ItemType.Service:
                        _MSTItm.AccICKey = SysOptionUtility.GetInt("ItemAccIC", cn);
                        _MSTItm.AccINKey = null;
                        _MSTItm.Brandkey = SysOptionUtility.GetInt("ItemBrand", cn);
                        _MSTItm.BUOMKey = SysOptionUtility.GetInt("ItemBUOM", cn);
                        _MSTItm.INClass = SysOptionUtility.GetStr("ItemClass", cn);
                        _MSTItm.CatKey1 = SysOptionUtility.GetInt("ItemCategory1", cn);
                        _MSTItm.CatKey2 = SysOptionUtility.GetInt("ItemCategory2", cn);
                        _MSTItm.CatKey3 = SysOptionUtility.GetInt("ItemCategory3", cn);
                        _MSTItm.CatKey4 = SysOptionUtility.GetInt("ItemCategory4", cn);
                        _MSTItm.CatKey5 = SysOptionUtility.GetInt("ItemCategory5", cn);
                        _MSTItm.CostMethod = null;
                        _MSTItm.QtyMax = 0;
                        _MSTItm.QtyMin = 0;
                        _MSTItm.QtyReOrder = 0;
                        _MSTItm.WeightUOMKey = SysOptionUtility.GetInt("ItemWeightUOMKey", cn);
                        _MSTItm.DefLocPurchase = null;
                        _MSTItm.DefLocSale = null;
                        _MSTItm.AccPHKey = SysOptionUtility.GetInt("ItemAccPH", cn);
                        break;
                    #endregion

                    #region Charges and Discount
                    case (int)GEnum.ItemType.Charges:
                    case (int)GEnum.ItemType.Discount:
                        _MSTItm.AccICKey = SysOptionUtility.GetInt("ItemAccIC", cn);
                        _MSTItm.AccINKey = null;
                        _MSTItm.Brandkey = 0;
                        _MSTItm.BUOMKey = null;
                        _MSTItm.INClass = SysOptionUtility.GetStr("ItemClass", cn);
                        _MSTItm.CatKey1 = SysOptionUtility.GetInt("ItemCategory1", cn);
                        _MSTItm.CatKey2 = SysOptionUtility.GetInt("ItemCategory2", cn);
                        _MSTItm.CatKey3 = SysOptionUtility.GetInt("ItemCategory3", cn);
                        _MSTItm.CatKey4 = SysOptionUtility.GetInt("ItemCategory4", cn);
                        _MSTItm.CatKey5 = SysOptionUtility.GetInt("ItemCategory5", cn);
                        _MSTItm.CostMethod = null;
                        _MSTItm.QtyMax = null;
                        _MSTItm.QtyMin = null;
                        _MSTItm.QtyReOrder = null;
                        _MSTItm.WeightUOMKey = null;
                        _MSTItm.DefLocPurchase = null;
                        _MSTItm.DefLocSale = null;
                        _MSTItm.AccPHKey = SysOptionUtility.GetInt("ItemAccPH", cn);
                        break;
                    #endregion

                    #region Remarks, Total ..
                    case (int)GEnum.ItemType.Header:
                    case (int)GEnum.ItemType.Remark:
                    case (int)GEnum.ItemType.Sub_Total:
                    case (int)GEnum.ItemType.Total:
                    case (int)GEnum.ItemType.BF_Total:
                        _MSTItm.AccICKey = null;
                        _MSTItm.AccINKey = null;
                        _MSTItm.Brandkey = null;
                        _MSTItm.BUOMKey = null;
                        _MSTItm.INClass = null;
                        _MSTItm.CatKey1 = 0;
                        _MSTItm.CatKey2 = 0;
                        _MSTItm.CatKey3 = 0;
                        _MSTItm.CatKey4 = 0;
                        _MSTItm.CatKey5 = 0;
                        _MSTItm.CostMethod = null;
                        _MSTItm.QtyMax = null;
                        _MSTItm.QtyMin = null;
                        _MSTItm.QtyReOrder = null;
                        _MSTItm.WeightUOMKey = null;
                        _MSTItm.DefLocPurchase = null;
                        _MSTItm.DefLocSale = null;
                        _MSTItm.AccPHKey = null;
                        break;
                    #endregion

                    default:
                        MsgBox.Show(cn, "Invalid Item Type");
                        return;
                }
                _MSTItm.QtyStock = 0;
                _MSTItm.CostLatest = 0;
                _MSTItm.CostLatestDate = DateTime.Today;
                _MSTItm.ControlPriceH = 0;
                _MSTItm.CostLanded = 0;
                _MSTItm.CostLandedDate = DateTime.Today;
                _MSTItm.LeadTimeInDays = 0;
                _MSTItm.CostAvg = 0;
                _MSTItm.SaleUOMRate = 1;
                _MSTItm.PurchaseUOMRate = 1;
            }
            catch (TAException ex)
            {
                throw Error(ex);
            }
            catch (Exception ex)
            {
                throw Error(ex);
            }
        }//Completed
        private bool GenerateBarCode(out string SKU1, SqlConnection cn)
        {
            //Generate BarCode
            SYSOption objSYSOption = null;
            try
            {
                objSYSOption = SYSOption.New();
                objSYSOption.Fetch(cn, new SYSOption.Criteria(GVar.SystemOption.Item_Defaults.ItemSKULastCounter, AppInfor.currentUserKey, 1));
                objSYSOption._opValue = (objSYSOption._opValue == string.Empty) ? "1" : (Convert.ToInt32(objSYSOption._opValue) + 1).ToString();
                if (objSYSOption.Update(cn))
                {
                    SKU1 = objSYSOption._opValue;
                    return true;
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

            SKU1 = string.Empty;
            return false;
        }//Completed
        public bool SaveTemplateItems()
        {
            bool isSave = false;
            string msgID = string.Empty;
            if (this.IsNew)
                msgID = MsgID.Common.AddFail;

            int? newItmKey = 0;
            string autoID = string.Empty;

            bool isCommitTransFail = true;

            if (this.InstanceMode == GEnum.InstanceMode.Normal)
            {
                try
                {
                    if (this.IsReadOnly)
                    {
                        MsgBox.Show(MsgID.Common.RecordIsReadOnly);
                        return false;
                    }
                    else
                    {
                        if (!SECPermUtility.Add(constPermID, true))
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

                            if (SysLockUtility.IsLock(cn, true, GEnum.SysLockOption.ByCodeKeyandDataKey, constCodeKey, -1000, 0, _guID))
                            { return false; }

                            if (!SysLockUtility.AddLock(cn, true, this._guID, constCodeKey, -1000))
                            { return false; }

                            int iCol = 0;
                            string tmpItmID = string.Empty;
                            Boolean Exit = false;

                            for (int r = 0; r < dtItemTemplate.Rows.Count; r++)
                            {
                                iCol = 3;
                                for (int c = 3; c < dtItemTemplate.Columns.Count; c++)
                                {

                                    if (c < 3)
                                        continue;
                                    if (!GFunc.IsNE(dtItemTemplate.Rows[r][c]))
                                    {
                                        if (Convert.ToBoolean(dtItemTemplate.Rows[r][c]) == false)
                                        //update Inactive TRUE  if checkbox is false
                                        {
                                            if (!GFunc.IsNE(dtItemTemplate.Rows[r][c + 1]))// existing item need to update
                                            {
                                                MSTItm objItemTemp = new MSTItm();
                                                objItemTemp.Fetch(cn, new MSTItm.Criteria(int.Parse(dtItemTemplate.Rows[r][c + 1].ToString()), 1)); ;
                                                objItemTemp._inactive = true;
                                                if (!objItemTemp.Update(cn))
                                                {
                                                    return false;
                                                }
                                                iCol = c + 1;
                                                c = iCol;
                                            }
                                            continue;
                                        }
                                    }
                                    else
                                        continue;
                                    string colorID = string.Empty;
                                    MSTItm objMSTItmTemp = _MSTItm.Clone();
                                    MSTItmDetPrice objItmDetPriceTemp = _MSTItmDetPrice.Clone();

                                    // Update =>   check to know item is existing or not
                                    if (!GFunc.IsNE(dtItemTemplate.Rows[r][c + 1]))// existing item need to update
                                    {
                                        if (!objMSTItmTemp.Update(cn))
                                        {
                                            return false;
                                        }
                                    }
                                    // Insert => new item to insert
                                    else
                                    {
                                        if (dtItemTemplate.Rows[r]["ColorID"].ToString().Length >= 3)
                                            colorID = dtItemTemplate.Rows[r]["ColorID"].ToString().Substring(0, 3);
                                        else
                                            //colorID = dtItemTemplate.Rows[r]["ColorID"].ToString().PadRight(3);
                                            colorID = dtItemTemplate.Rows[r]["ColorID"].ToString();

                                        objMSTItmTemp._itmID = _MSTItm._itmID + colorID + dtItemTemplate.Columns[c].Caption;
                                        //  Validation  
                                        string var = string.Empty;
                                        int MaxID = 0;

                                        // to get ItmID of sub item
                                        MSTItm objItm = new MSTItm();
                                        objItm.Fetch(cn, new MSTItm.Criteria(objMSTItmTemp._itmID, 5));

                                        if (!GFunc.IsNE(objItm._itmID))
                                        {
                                            if (objItm._itmID.Contains("_"))
                                            {
                                                var = objItm._itmID.Substring(objItm._itmID.IndexOf("_") + 1, objItm._itmID.Length - (objItm._itmID.IndexOf("_") + 1));
                                            }

                                            if (var == string.Empty)
                                            {
                                                objMSTItmTemp._itmID = objMSTItmTemp._itmID + "_1";
                                            }
                                            else
                                            {
                                                MaxID = int.Parse(var) + 1;
                                                objMSTItmTemp._itmID = _MSTItm._itmID + colorID + dtItemTemplate.Columns[c].Caption + "_" + MaxID.ToString();


                                            }
                                            if (!this.ValidationForTemplateItem(cn, objMSTItmTemp))
                                            { return false; }

                                        }
                                        // else objMSTItmTemp._itmID = objMSTItmTemp._itmID + "_1";

                                        // Save Header Record
                                        objMSTItmTemp.MasterItmKey = _MSTItm._itmKey;
                                        objMSTItmTemp.MasterItmID = _MSTItm._itmID;
                                        objMSTItmTemp.ItmType = _MSTItm._masterItmType;
                                        objMSTItmTemp.ColorKey = Convert.ToInt32(dtItemTemplate.Rows[r]["ColorKey"]);
                                        objMSTItmTemp.ScaleSizeNum = Convert.ToInt16(dtItemTemplate.Columns[c].ColumnName);
                                        objMSTItmTemp.ScaleSize = dtItemTemplate.Columns[c].Caption;

                                        if (!GenerateBarCode(out objMSTItmTemp._sku1, cn))
                                        { return false; }


                                        if (!objMSTItmTemp.Insert(cn, out newItmKey, 0))
                                        { return false; }

                                        objItmDetPriceTemp._itmKey = newItmKey;
                                        objItmDetPriceTemp.Insert(cn, newItmKey);


                                    }
                                    iCol = c + 1;
                                    c = iCol;
                                }
                            }

                            if (!SysLockUtility.RemoveLockGUIDKeepIP(cn, true, this._guID, constCodeKey))
                            { return false; }

                            // Record Locking
                            if (!SysLockUtility.AddLock(cn, true, _guID, constCodeKey, newItmKey))
                            { return false; }

                            // Commit Process
                            this._isNew = false;
                            msgID = string.Empty;
                            isSave = true;

                            // No errors - commit transaction
                            if (Transaction.Current.TransactionInformation.Status != TransactionStatus.Active) throw new Exception("Transaction has aborted."); scope.Complete();
                            isCommitTransFail = false;
                            _isDirty = false;

                        }// End of SqlConnection
                    }// End of TransactionScope

                    // Audit Log
                    SysAuditLogUtility.AddAuditLog(GEnum.AuditLogMode.Add, constCodeKey, 0, "Template Items", new object[] { dtItemTemplate });

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
            }
            else
            {
                MsgBox.Show(MsgID.Common.WrongInstanceMode);
                return false;
            }
            return isSave;
        }
        public bool BuildMasterItem()
        {
            string itemID = string.Empty;
            string colorID = string.Empty;
            string msgID = string.Empty;

            try
            {
                MSTItms tmpSubItms = MSTItms.Get(_MSTItm._itmKey);
                if (GFunc.IsNEZ(_MSTItm._scaleKey))
                {
                    MsgBox.Show("No Scale to Create Master Item");
                    return false;
                }

                dtItemTemplate = new DataTable();

                //Build Columns
                dtItemTemplate.Columns.Add("ColorKey");
                dtItemTemplate.Columns.Add("ColorID");
                dtItemTemplate.Columns.Add("ColorDes");

                REFScaleDetItms objREFScaleDetItms = REFScaleDetItms.Get(_MSTItm._scaleKey);
                int iCol = 3;
                if (msgID == string.Empty)
                    for (int i = 0; i < objREFScaleDetItms.Count; i++)
                    {
                        dtItemTemplate.Columns.Add(objREFScaleDetItms[i]._sizeNum.ToString());
                        dtItemTemplate.Columns.Add("ItemKey_S" + i.ToString(), typeof(Int32));
                        //In datatable, Scale column started from 3, Column 0->ColorKey, Column 1 -> "ColorID", etc.

                        dtItemTemplate.Columns[iCol].Caption = objREFScaleDetItms[i]._sizeID;
                        iCol = iCol + 2;
                    }

                //Build Rows  
                REFColors objREFColors = REFColors.Get();

                for (int i = 0; i < objREFColors.Count; i++)
                {
                    DataRow dr = dtItemTemplate.NewRow();
                    dr["ColorKey"] = objREFColors[i]._colorKey;
                    dr["ColorID"] = objREFColors[i]._colorID;
                    dr["ColorDes"] = objREFColors[i]._colorDes;

                    dtItemTemplate.Rows.Add(dr);
                }


                for (int k = 0; k < dtItemTemplate.Rows.Count; k++)
                {
                    iCol = 3;
                    //Fill false value, In UI, the user can tick in check boxes for true value
                    for (int j = 3; j < dtItemTemplate.Columns.Count; j++)
                    {

                        for (int s = 0; s < tmpSubItms.Count; s++)
                        {
                            if (int.Parse(dtItemTemplate.Rows[k]["ColorKey"].ToString()) == tmpSubItms[s]._colorKey && tmpSubItms[s]._scaleSize == dtItemTemplate.Columns[j].Caption)
                            {

                                dtItemTemplate.Rows[k][j + 1] = tmpSubItms[s]._itmKey;
                                dtItemTemplate.Rows[k][j] = true;
                            }
                            else
                            {
                                dtItemTemplate.Rows[k][j] = false;
                            }
                        }
                        iCol = j + 1;
                        j = iCol;
                    }
                }
                for (int k = 0; k < dtItemTemplate.Rows.Count; k++)
                {
                    for (int j = 3; j < dtItemTemplate.Columns.Count; j++)
                    {
                        if (!GFunc.IsNEZ(dtItemTemplate.Rows[k][j + 1]))
                        {
                            dtItemTemplate.Rows[k][j] = true;
                        }
                        else
                            dtItemTemplate.Rows[k][j] = false;

                        iCol = j + 1;
                        j = iCol;
                    }
                }

                dtItemTemplate.AcceptChanges();
                msgID = string.Empty;

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


        }

        //Validation
        private bool Validation_Header(SqlConnection cn)
        {
            //fieldNameToCheck = string.empty to check for all fields
            #region Declaration
            string processOK = GVar.gcPass;
            string errorMsgID = string.Empty;
            int ItmType;
            bool runStock = false;
            bool runBatch = false;
            bool runConsignment = false;
            bool runNS = false;
            bool runService = false;
            bool runCharges = false;
            bool runRem = false;
            UINotifierEventArgs e = new UINotifierEventArgs(new Hashtable());
            #endregion

            try
            {
                //Clear Error in UI
                if (GFunc.IsNE(this.ErrorNotifierHeader_Clear) == false)
                    this.ErrorNotifierHeader_Clear.Invoke(this, e);

                #region Validate ItmType
                processOK = BaseUtility.Validation(processOK, false, out errorMsgID, this._MSTItm.ItmType, "ItmType", GEnum.DataType.Integer, GEnum.Require.Yes, null, GEnum.CompareOperator.GreatherThan, 0, null, null, e, cn);

                //Special Condition for Master Item
                if (_MSTItm.ItmType == (int)GEnum.ItemType.Master)
                {
                    processOK = BaseUtility.Validation(processOK, false, out errorMsgID, this._MSTItm._masterItmType, "MasterItmType", GEnum.DataType.Integer, GEnum.Require.Yes, null, GEnum.CompareOperator.GreatherThan, 0, null, null, e, cn);
                    ItmType = (int)_MSTItm._masterItmType;
                }
                else
                    ItmType = (int)_MSTItm.ItmType;
                #endregion

                #region Validate Item Key and ID for New Record or existing record
                if (this.IsNew)
                {
                    processOK = BaseUtility.Validation(processOK, false, out errorMsgID, this._MSTItm.ItmKey, "ItmKey", GEnum.DataType.Integer, GEnum.Require.No, null, GEnum.CompareOperator.GreatherThanEqual, 0, null, null, e, cn);
                    processOK = BaseUtility.Validation(processOK, false, out errorMsgID, this._MSTItm.ItmID, "ItmID", GEnum.DataType.String, GEnum.Require.No, 50, null, null, null, null, e, cn);
                }
                else
                {
                    processOK = BaseUtility.Validation(processOK, false, out errorMsgID, this._MSTItm.ItmKey, "ItmKey", GEnum.DataType.Integer, GEnum.Require.Yes, null, GEnum.CompareOperator.GreatherThan, 0, null, null, e, cn);
                    processOK = BaseUtility.Validation(processOK, false, out errorMsgID, this._MSTItm.ItmID, "ItmID", GEnum.DataType.String, GEnum.Require.Yes, 50, null, null, null, null, e, cn);
                }
                #endregion

                #region set process to run

                //For Substitute item we will treat it as a remark ItmType
                if (ObjMSTItm.SubstituteItmKey > 0)
                    runRem = true;
                else
                {
                    switch (ItmType)
                    {
                        case (int)GEnum.ItemType.Finished_GDB:
                        case (int)GEnum.ItemType.Serial_Finished_GDB:
                        case (int)GEnum.ItemType.Serial_StockB:
                        case (int)GEnum.ItemType.StockB:
                            runBatch = true;
                            break;

                        case (int)GEnum.ItemType.Finished_GD:
                        case (int)GEnum.ItemType.Stock:
                            runStock = true;
                            break;

                        case (int)GEnum.ItemType.Consignment:
                            runConsignment = true;
                            break;

                        case (int)GEnum.ItemType.Assembly:
                        case (int)GEnum.ItemType.Non_Stock:
                            runNS = true;
                            break;

                        case (int)GEnum.ItemType.Service:
                            runService = true;
                            break;

                        case (int)GEnum.ItemType.Charges:
                        case (int)GEnum.ItemType.Discount:
                            runCharges = true;
                            break;

                        case (int)GEnum.ItemType.Remark:
                        case (int)GEnum.ItemType.Header:
                            runRem = true;
                            break;

                        default:
                            break;
                    }
                }

                #endregion

                #region Validation Process

                #region Batch
                if (runBatch)
                {
                    processOK = BaseUtility.Validation(processOK, false, out errorMsgID, this._MSTItm.BOMType, "BOMType", GEnum.DataType.Integer, GEnum.Require.Yes, null, GEnum.CompareOperator.GreatherThan, 0, null, null, e, cn);
                    processOK = BaseUtility.Validation(processOK, false, out errorMsgID, this._MSTItm.BOMMultiplier, "BOMMultiplier", GEnum.DataType.Integer, GEnum.Require.Yes, null, GEnum.CompareOperator.GreatherThan, 0, null, null, e, cn);
                    processOK = BaseUtility.Validation(processOK, false, out errorMsgID, this._MSTItm.DefaultExpDate, "DefaultExpDate", GEnum.DataType.String, GEnum.Require.No, 50, null, null, null, null, e, cn);
                    if (this._MSTItm.DefaultExpDate != string.Empty)
                    {
                        bool DefaultExpDateError = false;
                        string last = _MSTItm.DefaultExpDate.Substring(_MSTItm.DefaultExpDate.Length - 1, 1);
                        int prefix = 0;

                        if (!last.ToUpper().Contains('Y') && !last.ToUpper().Contains('M') && !last.ToUpper().Contains('W') && !last.ToUpper().Contains('D'))
                            DefaultExpDateError = true;
                        else
                        {
                            last = _MSTItm.DefaultExpDate.Substring(0, _MSTItm.DefaultExpDate.Length - 1);
                            if (int.TryParse(last, out prefix) == false)
                                DefaultExpDateError = true;
                        }
                        if (GFunc.IsNE(this.ErrorNotifierHeader_Set) == false && DefaultExpDateError)
                        {
                            errorMsgID = "Invalid format: DefaultExpDate must be e.g: 2Y, 10M, 50W, 100D for 2 Years, 10 Months, 50 Weeks and 100 Days respectively";
                            e.PropertyMessage.Add("DefaultExpDate", errorMsgID);
                        }
                    }
                }
                #endregion

                #region Stock,Batch
                if (runStock || runBatch)
                {
                    processOK = BaseUtility.Validation(processOK, false, out errorMsgID, this._MSTItm.CostMethod, "CostMethod", GEnum.DataType.Integer, GEnum.Require.Yes, null, GEnum.CompareOperator.GreatherThan, 0, null, null, e, cn);
                    processOK = BaseUtility.Validation(processOK, false, out errorMsgID, this._MSTItm.AccINKey, "AccINKey", GEnum.DataType.Integer, GEnum.Require.Yes, null, GEnum.CompareOperator.GreatherThan, 0, null, null, e, cn);
                    processOK = BaseUtility.Validation(processOK, false, out errorMsgID, this._MSTItm.CostAvg, "CostAvg", GEnum.DataType.Decimel, GEnum.Require.Yes, null, null, null, null, null, e, cn);
                }
                #endregion

                #region Stock,Batch,NS
                if (runStock || runBatch || runNS)
                {
                    processOK = BaseUtility.Validation(processOK, false, out errorMsgID, this._MSTItm.Model, "Model", GEnum.DataType.String, GEnum.Require.No, 50, null, null, null, null, e, cn);
                }
                #endregion

                #region Stock,Batch,Consignment
                if (runStock || runBatch || runConsignment)
                {
                    processOK = BaseUtility.Validation(processOK, false, out errorMsgID, this._MSTItm.QtyStock, "QtyStock", GEnum.DataType.Decimel, GEnum.Require.Yes, null, null, null, null, null, e, cn);
                    processOK = BaseUtility.Validation(processOK, false, out errorMsgID, this._MSTItm.QtyMin, "QtyMin", GEnum.DataType.Decimel, GEnum.Require.Yes, null, null, null, null, null, e, cn);
                    processOK = BaseUtility.Validation(processOK, false, out errorMsgID, this._MSTItm.QtyMax, "QtyMax", GEnum.DataType.Decimel, GEnum.Require.Yes, null, null, null, null, null, e, cn);
                    processOK = BaseUtility.Validation(processOK, false, out errorMsgID, this._MSTItm.QtyReOrder, "QtyReOrder", GEnum.DataType.Decimel, GEnum.Require.Yes, null, null, null, null, null, e, cn);
                    processOK = BaseUtility.Validation(processOK, false, out errorMsgID, this._MSTItm.OpenBalCost, "OpenBalCost", GEnum.DataType.Decimel, GEnum.Require.Yes, null, GEnum.CompareOperator.GreatherThanEqual, 0, null, null, e, cn);
                    //   processOK = BaseUtility.Validation(processOK, false, out errorMsgID, this._MSTItm.OpenBalQty, "OpenBalQty", GEnum.DataType.Decimel, GEnum.Require.Yes, null, GEnum.CompareOperator.GreatherThanEqual, 0, null, null, e, cn);
                    processOK = BaseUtility.Validation(processOK, false, out errorMsgID, this._MSTItm.OpenBalAmtH, "OpenBalAmtH", GEnum.DataType.Decimel, GEnum.Require.Yes, null, GEnum.CompareOperator.GreatherThanEqual, 0, null, null, e, cn);
                }
                #endregion

                #region Stock,Batch,NonStock,Consignment, Service
                if (runStock || runBatch || runConsignment || runNS || runService)
                {
                    processOK = BaseUtility.Validation(processOK, false, out errorMsgID, this._MSTItm.BUOMKey, "BUOMKey", GEnum.DataType.Integer, GEnum.Require.Yes, null, GEnum.CompareOperator.GreatherThan, 0, null, null, e, cn);
                    processOK = BaseUtility.Validation(processOK, false, out errorMsgID, this._MSTItm.CostLatest, "CostLatest", GEnum.DataType.Decimel, GEnum.Require.Yes, null, null, null, null, null, e, cn);
                    processOK = BaseUtility.Validation(processOK, false, out errorMsgID, this._MSTItm.CostLatestDate, "CostLatestDate", GEnum.DataType.DateTime, GEnum.Require.Yes, null, null, null, null, null, e, cn);
                    processOK = BaseUtility.Validation(processOK, false, out errorMsgID, this._MSTItm.CostLanded, "CostLanded", GEnum.DataType.Decimel, GEnum.Require.Yes, null, null, null, null, null, e, cn);
                    processOK = BaseUtility.Validation(processOK, false, out errorMsgID, this._MSTItm.CostLandedDate, "CostLandedDate", GEnum.DataType.DateTime, GEnum.Require.Yes, null, null, null, null, null, e, cn);
                }
                #endregion

                #region Stock,Batch,NonStock,Consignment
                if (runStock || runBatch || runConsignment || runNS)
                {
                    processOK = BaseUtility.Validation(processOK, false, out errorMsgID, this._MSTItm.WeightNet, "WeightNet", GEnum.DataType.Decimel, GEnum.Require.Yes, null, GEnum.CompareOperator.GreatherThanEqual, 0, null, null, e, cn);
                    processOK = BaseUtility.Validation(processOK, false, out errorMsgID, this._MSTItm.WeightGross, "WeightGross", GEnum.DataType.Decimel, GEnum.Require.Yes, null, GEnum.CompareOperator.GreatherThanEqual, 0, null, null, e, cn);
                    processOK = BaseUtility.Validation(processOK, false, out errorMsgID, this._MSTItm.WeightUOMKey, "WeightUOMKey", GEnum.DataType.Integer, GEnum.Require.Yes, null, GEnum.CompareOperator.GreatherThan, 0, null, null, e, cn);
                    processOK = BaseUtility.Validation(processOK, false, out errorMsgID, this._MSTItm.INLength, "INLength", GEnum.DataType.Decimel, GEnum.Require.Yes, null, GEnum.CompareOperator.GreatherThanEqual, 0, null, null, e, cn);
                    processOK = BaseUtility.Validation(processOK, false, out errorMsgID, this._MSTItm.INWidth, "INWidth", GEnum.DataType.Decimel, GEnum.Require.Yes, null, GEnum.CompareOperator.GreatherThanEqual, 0, null, null, e, cn);
                    processOK = BaseUtility.Validation(processOK, false, out errorMsgID, this._MSTItm.INHeight, "INHeight", GEnum.DataType.Decimel, GEnum.Require.Yes, null, GEnum.CompareOperator.GreatherThanEqual, 0, null, null, e, cn);
                    processOK = BaseUtility.Validation(processOK, false, out errorMsgID, this._MSTItm.INVolume, "INVolume", GEnum.DataType.Decimel, GEnum.Require.Yes, null, GEnum.CompareOperator.GreatherThanEqual, 0, null, null, e, cn);
                    processOK = BaseUtility.Validation(processOK, false, out errorMsgID, this._MSTItm.INPacking, "INPacking", GEnum.DataType.String, GEnum.Require.No, 255, null, null, null, null, e, cn);
                    processOK = BaseUtility.Validation(processOK, false, out errorMsgID, this._MSTItm.StdPackSize, "StdPackSize", GEnum.DataType.Decimel, GEnum.Require.Yes, null, GEnum.CompareOperator.GreatherThanEqual, 0, null, null, e, cn);
                    processOK = BaseUtility.Validation(processOK, false, out errorMsgID, this._MSTItm.StdPackWeight, "StdPackWeight", GEnum.DataType.Decimel, GEnum.Require.Yes, null, GEnum.CompareOperator.GreatherThanEqual, 0, null, null, e, cn);
                    processOK = BaseUtility.Validation(processOK, false, out errorMsgID, this._MSTItm.StdPackLength, "StdPackLength", GEnum.DataType.Decimel, GEnum.Require.Yes, null, GEnum.CompareOperator.GreatherThanEqual, 0, null, null, e, cn);
                    processOK = BaseUtility.Validation(processOK, false, out errorMsgID, this._MSTItm.StdPackWidth, "StdPackWidth", GEnum.DataType.Decimel, GEnum.Require.Yes, null, GEnum.CompareOperator.GreatherThanEqual, 0, null, null, e, cn);
                    processOK = BaseUtility.Validation(processOK, false, out errorMsgID, this._MSTItm.StdPackHeight, "StdPackHeight", GEnum.DataType.Decimel, GEnum.Require.Yes, null, GEnum.CompareOperator.GreatherThanEqual, 0, null, null, e, cn);
                    processOK = BaseUtility.Validation(processOK, false, out errorMsgID, this._MSTItm.SaleUOM, "SaleUOM", GEnum.DataType.String, GEnum.Require.No, 50, null, null, null, null, e, cn);
                    processOK = BaseUtility.Validation(processOK, false, out errorMsgID, this._MSTItm.SaleUOMRate, "SaleUOMRate", GEnum.DataType.Decimel, GEnum.Require.Yes, null, GEnum.CompareOperator.GreatherThanEqual, 0, null, null, e, cn);
                    processOK = BaseUtility.Validation(processOK, false, out errorMsgID, this._MSTItm.PurchaseUOM, "PurchaseUOM", GEnum.DataType.String, GEnum.Require.No, 50, null, null, null, null, e, cn);
                    processOK = BaseUtility.Validation(processOK, false, out errorMsgID, this._MSTItm.PurchaseUOMRate, "PurchaseUOMRate", GEnum.DataType.Decimel, GEnum.Require.Yes, null, GEnum.CompareOperator.GreatherThanEqual, 0, null, null, e, cn);
                }
                #endregion

                #region Consignment
                if (runConsignment)
                {
                    processOK = BaseUtility.Validation(processOK, false, out errorMsgID, this._MSTItm.CSGVendorKey, "CSGVendorKey", GEnum.DataType.Integer, GEnum.Require.Yes, null, GEnum.CompareOperator.GreatherThan, 0, null, null, e, cn);
                }
                #endregion

                #region Stock,Batch,NonStock,Service,Charges
                if (runStock || runBatch || runNS || runService || runCharges)
                {
                    processOK = BaseUtility.Validation(processOK, false, out errorMsgID, this._MSTItm.AccPHKey, "AccPHKey", GEnum.DataType.Integer, GEnum.Require.Yes, null, GEnum.CompareOperator.GreatherThan, 0, null, null, e, cn);
                }
                #endregion

                #region Stock,Batch,Consignment, NonStock,Service,Charges
                if (runStock || runBatch || runConsignment || runNS || runService || runCharges)
                {
                    processOK = BaseUtility.Validation(processOK, false, out errorMsgID, this._MSTItm.AccICKey, "AccICKey", GEnum.DataType.Integer, GEnum.Require.Yes, null, GEnum.CompareOperator.GreatherThan, 0, null, null, e, cn);
                    processOK = BaseUtility.Validation(processOK, false, out errorMsgID, this._MSTItm.ControlPriceH, "ControlPriceH", GEnum.DataType.Decimel, GEnum.Require.Yes, null, GEnum.CompareOperator.GreatherThanEqual, 0, null, null, e, cn);
                    processOK = BaseUtility.Validation(processOK, false, out errorMsgID, this._MSTItm.Taxable, "Taxable", GEnum.DataType.Boolean, GEnum.Require.Yes, null, null, null, null, null, e, cn);
                    processOK = BaseUtility.Validation(processOK, false, out errorMsgID, this._MSTItm.INAttachment, "INAttachment", GEnum.DataType.Boolean, GEnum.Require.Yes, null, null, null, null, null, e, cn);
                }
                #endregion

                #region Stock,Batch,Consignment, NonStock,Service,Charges,Rem
                if (runStock || runBatch || runConsignment || runNS || runService || runCharges)
                {
                    processOK = BaseUtility.Validation(processOK, false, out errorMsgID, this._MSTItm.ItmDes, "ItmDes", GEnum.DataType.String, GEnum.Require.Yes, 4000, null, null, null, null, e, cn);
                }
                #endregion

                #region All Item Types

                processOK = BaseUtility.Validation(processOK, false, out errorMsgID, this._MSTItm.MasterItmKey, "MasterItmKey", GEnum.DataType.Integer, GEnum.Require.Yes, null, GEnum.CompareOperator.GreatherThanEqual, 0, null, null, e, cn);
                processOK = BaseUtility.Validation(processOK, false, out errorMsgID, this._MSTItm.SubstituteItmKey, "SubstituteItmKey", GEnum.DataType.Integer, GEnum.Require.Yes, null, GEnum.CompareOperator.GreatherThanEqual, 0, null, null, e, cn);
                processOK = BaseUtility.Validation(processOK, false, out errorMsgID, this._MSTItm.ItmRem, "ItmRem", GEnum.DataType.String, GEnum.Require.No, 4000, null, null, null, null, e, cn);
                processOK = BaseUtility.Validation(processOK, false, out errorMsgID, this._MSTItm.AccessLevel, "AccessLevel", GEnum.DataType.Integer, GEnum.Require.Yes, null, GEnum.CompareOperator.GreatherThanEqual, 0, null, null, e, cn);
                processOK = BaseUtility.Validation(processOK, false, out errorMsgID, this._MSTItm.AccessGroup, "AccessGroup", GEnum.DataType.Integer, GEnum.Require.Yes, null, GEnum.CompareOperator.GreatherThanEqual, 0, null, null, e, cn);
                processOK = BaseUtility.Validation(processOK, false, out errorMsgID, this._MSTItm.IndustryPN, "IndustryPN", GEnum.DataType.String, GEnum.Require.No, 50, null, null, null, null, e, cn);
                processOK = BaseUtility.Validation(processOK, false, out errorMsgID, this._MSTItm.SKU1, "SKU1", GEnum.DataType.String, GEnum.Require.No, 50, null, null, null, null, e, cn);
                processOK = BaseUtility.Validation(processOK, false, out errorMsgID, this._MSTItm.SKU2, "SKU2", GEnum.DataType.String, GEnum.Require.No, 50, null, null, null, null, e, cn);
                processOK = BaseUtility.Validation(processOK, false, out errorMsgID, this._MSTItm.CatKey1, "CatKey1", GEnum.DataType.Integer, GEnum.Require.Yes, null, GEnum.CompareOperator.GreatherThanEqual, 0, null, null, e, cn);
                processOK = BaseUtility.Validation(processOK, false, out errorMsgID, this._MSTItm.CatKey2, "CatKey2", GEnum.DataType.Integer, GEnum.Require.Yes, null, GEnum.CompareOperator.GreatherThanEqual, 0, null, null, e, cn);
                processOK = BaseUtility.Validation(processOK, false, out errorMsgID, this._MSTItm.CatKey3, "CatKey3", GEnum.DataType.Integer, GEnum.Require.Yes, null, GEnum.CompareOperator.GreatherThanEqual, 0, null, null, e, cn);
                processOK = BaseUtility.Validation(processOK, false, out errorMsgID, this._MSTItm.CatKey4, "CatKey4", GEnum.DataType.Integer, GEnum.Require.Yes, null, GEnum.CompareOperator.GreatherThanEqual, 0, null, null, e, cn);
                processOK = BaseUtility.Validation(processOK, false, out errorMsgID, this._MSTItm.CatKey5, "CatKey5", GEnum.DataType.Integer, GEnum.Require.Yes, null, GEnum.CompareOperator.GreatherThanEqual, 0, null, null, e, cn);
                processOK = BaseUtility.Validation(processOK, false, out errorMsgID, this._MSTItm.INClass, "INClass", GEnum.DataType.String, GEnum.Require.No, 50, null, null, null, null, e, cn);
                processOK = BaseUtility.Validation(processOK, false, out errorMsgID, this._MSTItm.Inactive, "Inactive", GEnum.DataType.Boolean, GEnum.Require.Yes, null, null, null, null, null, e, cn);
                processOK = BaseUtility.Validation(processOK, false, out errorMsgID, this._MSTItm.BranchKey, "BranchKey", GEnum.DataType.Integer, GEnum.Require.Yes, null, GEnum.CompareOperator.GreatherThanEqual, 0, null, null, e, cn);
                processOK = BaseUtility.Validation(processOK, false, out errorMsgID, this._MSTItm.DeptKey, "DeptKey", GEnum.DataType.Integer, GEnum.Require.Yes, null, GEnum.CompareOperator.GreatherThanEqual, 0, null, null, e, cn);
                processOK = BaseUtility.Validation(processOK, false, out errorMsgID, this._MSTItm.Custom1, "Custom1", GEnum.DataType.String, GEnum.Require.No, 255, null, null, null, null, e, cn);
                processOK = BaseUtility.Validation(processOK, false, out errorMsgID, this._MSTItm.Custom2, "Custom2", GEnum.DataType.String, GEnum.Require.No, 255, null, null, null, null, e, cn);
                processOK = BaseUtility.Validation(processOK, false, out errorMsgID, this._MSTItm.Custom3, "Custom3", GEnum.DataType.String, GEnum.Require.No, 255, null, null, null, null, e, cn);
                processOK = BaseUtility.Validation(processOK, false, out errorMsgID, this._MSTItm.Custom4, "Custom4", GEnum.DataType.String, GEnum.Require.No, 255, null, null, null, null, e, cn);
                processOK = BaseUtility.Validation(processOK, false, out errorMsgID, this._MSTItm.Custom5, "Custom5", GEnum.DataType.String, GEnum.Require.No, 255, null, null, null, null, e, cn);
                processOK = BaseUtility.Validation(processOK, false, out errorMsgID, this._MSTItm.Custom6, "Custom6", GEnum.DataType.String, GEnum.Require.No, 255, null, null, null, null, e, cn);
                processOK = BaseUtility.Validation(processOK, false, out errorMsgID, this._MSTItm.Custom7, "Custom7", GEnum.DataType.String, GEnum.Require.No, 255, null, null, null, null, e, cn);
                processOK = BaseUtility.Validation(processOK, false, out errorMsgID, this._MSTItm.Custom8, "Custom8", GEnum.DataType.String, GEnum.Require.No, 255, null, null, null, null, e, cn);
                processOK = BaseUtility.Validation(processOK, false, out errorMsgID, this._MSTItm.Custom9, "Custom9", GEnum.DataType.String, GEnum.Require.No, 255, null, null, null, null, e, cn);
                processOK = BaseUtility.Validation(processOK, false, out errorMsgID, this._MSTItm.Custom10, "Custom10", GEnum.DataType.String, GEnum.Require.No, 255, null, null, null, null, e, cn);
                #endregion

                #endregion

                #region Check for Duplicate Item ID
                if (e.PropertyMessage.Count == 0)
                {
                    bool DuplicateID = _MSTItm.Validation(cn, new MSTItm.Criteria(0, _MSTItm._itmKey, _MSTItm._itmID), this.IsNew);

                    if (!DuplicateID && !GFunc.IsNE(this.ErrorNotifierHeader_Set))
                    {
                        errorMsgID = "ItemID" + MsgID.Validation.DuplicateRecord;
                        e.PropertyMessage.Add("ItmID", SysMessageUtility.Get(cn, errorMsgID));
                    }
                }
                #endregion

                #region Invoke Notifier
                if (e.PropertyMessage.Count > 0)
                {
                    if (!GFunc.IsNE(this.ErrorNotifierHeader_Set))
                        this.ErrorNotifierHeader_Set.Invoke(this, e);

                    return false;
                }
                else
                    return true;
                #endregion

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

                        //Check for Duplicate records
                        if (processOK)
                        {
                            string cellNm = RelationFieldCheckNm_Get(grdNm);
                            Validation_DetailRelation(grdNm, dr[cellNm], false, ref processOK, e);
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

                //Check for Duplicate records when fieldToCheck is Empty (meaning RowBeforeUpdate)
                if (processOK && fieldToCheck == string.Empty)
                {
                    string cellNm = RelationFieldCheckNm_Get(grdNm);
                    if (cellNm != "")
                        Validation_DetailRelation(grdNm, grdrow.Cells[cellNm].Value, grdrow.IsAddRow, ref processOK, e);
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
                    #region MSTItmDetAlts Validation
                    case "tagrdDetAlternates":
                        BaseUtility.Validation(propValue, "AltItmKey", CheckNm, GEnum.DataType.Integer, GEnum.Require.Yes, null, GEnum.CompareOperator.GreatherThan, 0, null, null, ref processOK, failonError, e);
                        BaseUtility.Validation(propValue, "AltRem", CheckNm, GEnum.DataType.String, GEnum.Require.No, 8000, null, 0, null, null, ref processOK, failonError, e);
                        BaseUtility.Validation(propValue, "CreateDate", CheckNm, GEnum.DataType.DateTime, GEnum.Require.No, null, null, 0, null, null, ref processOK, failonError, e);
                        BaseUtility.Validation(propValue, "CreateUserKey", CheckNm, GEnum.DataType.Integer, GEnum.Require.No, null, GEnum.CompareOperator.GreatherThan, 0, null, null, ref processOK, failonError, e);
                        BaseUtility.Validation(propValue, "LastModifiedDate", CheckNm, GEnum.DataType.DateTime, GEnum.Require.No, null, null, 0, null, null, ref processOK, failonError, e);
                        BaseUtility.Validation(propValue, "LastModifiedUserKey", CheckNm, GEnum.DataType.Integer, GEnum.Require.No, null, GEnum.CompareOperator.GreatherThan, 0, null, null, ref processOK, failonError, e);
                        BaseUtility.Validation(propValue, "Custom1", CheckNm, GEnum.DataType.String, GEnum.Require.No, 255, null, 0, null, null, ref processOK, failonError, e);
                        BaseUtility.Validation(propValue, "Custom2", CheckNm, GEnum.DataType.String, GEnum.Require.No, 255, null, 0, null, null, ref processOK, failonError, e);
                        BaseUtility.Validation(propValue, "Custom3", CheckNm, GEnum.DataType.String, GEnum.Require.No, 255, null, 0, null, null, ref processOK, failonError, e);
                        break;
                    #endregion

                    #region MSTItmDetAsss Validation
                    case "tagrdDetAssembly":
                        BaseUtility.Validation(propValue, "AssItmKey", CheckNm, GEnum.DataType.Integer, GEnum.Require.Yes, null, GEnum.CompareOperator.GreatherThan, 0, null, null, ref processOK, failonError, e);
                        BaseUtility.Validation(propValue, "AssItmType", CheckNm, GEnum.DataType.Integer, GEnum.Require.Yes, null, GEnum.CompareOperator.GreatherThan, 0, null, null, ref processOK, failonError, e);
                        BaseUtility.Validation(propValue, "AssSN", CheckNm, GEnum.DataType.Decimel, GEnum.Require.Yes, null, GEnum.CompareOperator.GreatherThan, 0, null, null, ref processOK, failonError, e);
                        BaseUtility.Validation(propValue, "AssQty", CheckNm, GEnum.DataType.Decimel, GEnum.Require.No, null, GEnum.CompareOperator.GreatherThanEqual, 0, null, null, ref processOK, failonError, e);
                        BaseUtility.Validation(propValue, "AssUOMKey", CheckNm, GEnum.DataType.Integer, GEnum.Require.No, null, GEnum.CompareOperator.GreatherThanEqual, 0, null, null, ref processOK, failonError, e);
                        BaseUtility.Validation(propValue, "DefaultSelection", CheckNm, GEnum.DataType.Boolean, GEnum.Require.Yes, null, null, 0, null, null, ref processOK, failonError, e);
                        BaseUtility.Validation(propValue, "LockQty", CheckNm, GEnum.DataType.Boolean, GEnum.Require.Yes, null, null, 0, null, null, ref processOK, failonError, e);
                        BaseUtility.Validation(propValue, "ToPrint", CheckNm, GEnum.DataType.Boolean, GEnum.Require.Yes, null, null, 0, null, null, ref processOK, failonError, e);
                        BaseUtility.Validation(propValue, "CreateDate", CheckNm, GEnum.DataType.DateTime, GEnum.Require.No, null, null, 0, null, null, ref processOK, failonError, e);
                        BaseUtility.Validation(propValue, "CreateUserKey", CheckNm, GEnum.DataType.Integer, GEnum.Require.No, null, GEnum.CompareOperator.GreatherThan, 0, null, null, ref processOK, failonError, e);
                        BaseUtility.Validation(propValue, "LastModifiedDate", CheckNm, GEnum.DataType.DateTime, GEnum.Require.No, null, null, 0, null, null, ref processOK, failonError, e);
                        BaseUtility.Validation(propValue, "LastModifiedUserKey", CheckNm, GEnum.DataType.Integer, GEnum.Require.No, null, GEnum.CompareOperator.GreatherThan, 0, null, null, ref processOK, failonError, e);
                        BaseUtility.Validation(propValue, "Custom1", CheckNm, GEnum.DataType.String, GEnum.Require.No, 255, null, 0, null, null, ref processOK, failonError, e);
                        BaseUtility.Validation(propValue, "Custom2", CheckNm, GEnum.DataType.String, GEnum.Require.No, 255, null, 0, null, null, ref processOK, failonError, e);
                        BaseUtility.Validation(propValue, "Custom3", CheckNm, GEnum.DataType.String, GEnum.Require.No, 255, null, 0, null, null, ref processOK, failonError, e);
                        break;
                    #endregion

                    #region MSTItmDetBOMRMs/PMs/Labours Validation
                    case "tagrdItmDetBOMPMs":
                    case "tagrdItmDetBOMRMs":
                    case "tagrdItmDetBOMLabours":
                        BaseUtility.Validation(propValue, "BOMItmKey", CheckNm, GEnum.DataType.Integer, GEnum.Require.Yes, null, GEnum.CompareOperator.GreatherThan, 0, null, null, ref processOK, failonError, e);
                        BaseUtility.Validation(propValue, "BOMLineType", CheckNm, GEnum.DataType.Integer, GEnum.Require.Yes, null, GEnum.CompareOperator.GreatherThan, 0, null, null, ref processOK, failonError, e);
                        BaseUtility.Validation(propValue, "BOMItmType", CheckNm, GEnum.DataType.Integer, GEnum.Require.Yes, null, GEnum.CompareOperator.GreatherThan, 0, null, null, ref processOK, failonError, e);
                        BaseUtility.Validation(propValue, "BOMUOMKey", CheckNm, GEnum.DataType.Integer, GEnum.Require.No, null, GEnum.CompareOperator.GreatherThan, 0, null, null, ref processOK, failonError, e);
                        BaseUtility.Validation(propValue, "BOMQty", CheckNm, GEnum.DataType.Decimel, GEnum.Require.Yes, null, GEnum.CompareOperator.GreatherThanEqual, 0, null, null, ref processOK, failonError, e);
                        BaseUtility.Validation(propValue, "BOMLabourCost", CheckNm, GEnum.DataType.Decimel, GEnum.Require.Yes, null, GEnum.CompareOperator.GreatherThanEqual, 0, null, null, ref processOK, failonError, e);
                        BaseUtility.Validation(propValue, "CreateDate", CheckNm, GEnum.DataType.DateTime, GEnum.Require.No, null, null, 0, null, null, ref processOK, failonError, e);
                        BaseUtility.Validation(propValue, "CreateUserKey", CheckNm, GEnum.DataType.Integer, GEnum.Require.No, null, GEnum.CompareOperator.GreatherThan, 0, null, null, ref processOK, failonError, e);
                        BaseUtility.Validation(propValue, "LastModifiedDate", CheckNm, GEnum.DataType.DateTime, GEnum.Require.No, null, null, 0, null, null, ref processOK, failonError, e);
                        BaseUtility.Validation(propValue, "LastModifiedUserKey", CheckNm, GEnum.DataType.Integer, GEnum.Require.No, null, GEnum.CompareOperator.GreatherThan, 0, null, null, ref processOK, failonError, e);
                        BaseUtility.Validation(propValue, "Custom1", CheckNm, GEnum.DataType.String, GEnum.Require.No, 255, null, 0, null, null, ref processOK, failonError, e);
                        BaseUtility.Validation(propValue, "Custom2", CheckNm, GEnum.DataType.String, GEnum.Require.No, 255, null, 0, null, null, ref processOK, failonError, e);
                        BaseUtility.Validation(propValue, "Custom3", CheckNm, GEnum.DataType.String, GEnum.Require.No, 255, null, 0, null, null, ref processOK, failonError, e);
                        break;
                    #endregion

                    #region MSTItmDetLocs Validation
                    case "tagrdDetLocaton":
                        BaseUtility.Validation(propValue, "LocQtyMin", CheckNm, GEnum.DataType.Decimel, GEnum.Require.Yes, null, null, 0, null, null, ref processOK, failonError, e);
                        BaseUtility.Validation(propValue, "LocQtyMax", CheckNm, GEnum.DataType.Decimel, GEnum.Require.Yes, null, null, 0, null, null, ref processOK, failonError, e);
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
        public bool Validation_DetailRelation(string grdNm, object propValue, bool IsAddRow, ref bool processOK, UINotifierEventArgs e)
        {
            bool errorFound = false;
            try
            {
                switch (grdNm)
                {
                    #region Assembly
                    case "tagrdDetAssembly":
                        var dupAss = ObjMSTItmDetAsss.AsEnumerable().ToList().FindAll(o => (o.Field<int>("AssItmKey") == int.Parse(propValue.ToString())));

                        if (IsAddRow)
                        {
                            if (dupAss.Count > 0)
                                errorFound = true;
                        }
                        else
                        {
                            if (dupAss.Count > 1)
                                errorFound = true;
                        }
                        if (errorFound)
                        {
                            e.PropertyMessage.Add("rowError", "Item" + MsgID.Validation.DuplicateRecord);
                            processOK = false;
                        }
                        break;
                    #endregion

                    #region BOM Packing
                    case "tagrdItmDetBOMPMs":
                        var dupPM = ObjMSTItmDetBOMPMs.AsEnumerable().ToList().FindAll(o => (o.Field<int>("BOMItmKey") == int.Parse(propValue.ToString())));

                        if (IsAddRow)
                        {
                            if (dupPM.Count > 0)
                                errorFound = true;
                        }
                        else
                        {
                            if (dupPM.Count > 1)
                                errorFound = true;
                        }
                        if (errorFound)
                        {
                            e.PropertyMessage.Add("rowError", "BOM Packing material Item" + MsgID.Validation.DuplicateRecord);
                            processOK = false;
                        }
                        break;
                    #endregion

                    #region BOM Raw
                    case "tagrdItmDetBOMRMs":
                        var dupRM = ObjMSTItmDetBOMRMs.AsEnumerable().ToList().FindAll(o => (o.Field<int>("BOMItmKey") == int.Parse(propValue.ToString())));
                        if (IsAddRow)
                        {
                            if (dupRM.Count > 0)
                                errorFound = true;
                        }
                        else
                        {
                            if (dupRM.Count > 1)
                                errorFound = true;
                        }
                        if (errorFound)
                        {
                            e.PropertyMessage.Add("rowError", "BOM Raw material Item" + MsgID.Validation.DuplicateRecord);
                            processOK = false;
                        }
                        break;
                    #endregion

                    #region BOM Labour
                    case "tagrdItmDetBOMLabours":
                        var dupLB = ObjMSTItmDetBOMLBs.AsEnumerable().ToList().FindAll(o => (o.Field<int>("BOMItmKey") == int.Parse(propValue.ToString())));

                        if (IsAddRow)
                        {
                            if (dupLB.Count > 0)
                                errorFound = true;
                        }
                        else
                        {
                            if (dupLB.Count > 1)
                                errorFound = true;
                        }
                        if (errorFound)
                        {
                            e.PropertyMessage.Add("rowError", "BOM Labour Item" + MsgID.Validation.DuplicateRecord);
                            processOK = false;
                        }
                        break;
                    #endregion

                    #region Alternate
                    case "tagrdDetAlternates":
                        var dupAlt = ObjMSTItmDetAlts.AsEnumerable().ToList().FindAll(o => (o.Field<int>("AltItmKey") == int.Parse(propValue.ToString())));

                        if (IsAddRow)
                        {
                            if (dupAlt.Count > 0)
                                errorFound = true;
                        }
                        else
                        {
                            if (dupAlt.Count > 1)
                                errorFound = true;
                        }
                        if (errorFound)
                        {
                            e.PropertyMessage.Add("rowError", "Alternate Item" + MsgID.Validation.DuplicateRecord);
                            processOK = false;
                        }
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
        private string RelationFieldCheckNm_Get(string grdNm)
        {
            switch (grdNm)
            {
                case "tagrdDetAssembly":
                    return "AssItmKey";

                case "tagrdItmDetBOMPMs":
                case "tagrdItmDetBOMRMs":
                case "tagrdItmDetBOMLabour":
                    return "BOMItmKey";

                case "tagrdDetAlternates":
                    return "AltItmKey";
                case "tagrdDetLocaton":
                    return "ItmKey";

                default:
                    return string.Empty;
            }

        }//Completed
        private bool ValidationForTemplateItem(SqlConnection cn, MSTItm objMstItm)
        {
            bool isValidation = false;
            try
            {
                isValidation = objMstItm.Validation(cn, new MSTItm.Criteria(1, objMstItm._itmKey, objMstItm._itmID), true);
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
        }

        //Attached Events
        void Obj_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if (this._isReadOnly == false)
            {
                if (this.dirtyEvent != null)
                    this.dirtyEvent.Invoke(this, e);

                _isDirty = true;
            }
        }//Completed
        void Attachments_ListChanged(object sender, ListChangedEventArgs e)
        {
            _isDirty = true;
        }//Completed

        //Error
        private Exception Error(Exception ex)
        {
            try
            {
                if (SysOptionUtility.DatabaseBranchCode == "OMS" || SysOptionUtility.DatabaseBranchCode == "OMSTW" || SysOptionUtility.DatabaseBranchCode == DBCode.ITS)
                {
                    ex = SysAuditLogUtility.ModifyException(ex, false, new object[] { _MSTItm, _MSTItmDetAlts, _MSTItmDetAsss, _MSTItmBatchs, _MSTItmDetBOMLBs, _MSTItmDetBOMPMs, _MSTItmDetBOMRMs, _MSTItmDetLocs, _MSTItmDetPrice, _MSTItmSerials }, constCodeKey);
                }
                else
                {
                    ex = SysAuditLogUtility.ModifyException(ex, false, new object[] { _MSTItm, _MSTItmDetAlts, _MSTItmDetAsss, _MSTItmBatchs, _MSTItmDetBOMLBs, _MSTItmDetBOMPMs, _MSTItmDetBOMRMs, _MSTItmDetLocs, _MSTItmDetPrice }, constCodeKey);
                }
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
                if (SysOptionUtility.DatabaseBranchCode == "OMS" || SysOptionUtility.DatabaseBranchCode == "OMSTW" || SysOptionUtility.DatabaseBranchCode == DBCode.ITS)
                {
                    ex = SysAuditLogUtility.ModifyTAException(ex, false, new object[] { _MSTItm, _MSTItmDetAlts, _MSTItmDetAsss, _MSTItmBatchs, _MSTItmDetBOMLBs, _MSTItmDetBOMPMs, _MSTItmDetBOMRMs, _MSTItmDetLocs, _MSTItmDetPrice, _MSTItmSerials }, constCodeKey);
                }
                else
                {
                    ex = SysAuditLogUtility.ModifyTAException(ex, false, new object[] { _MSTItm, _MSTItmDetAlts, _MSTItmDetAsss, _MSTItmBatchs, _MSTItmDetBOMLBs, _MSTItmDetBOMPMs, _MSTItmDetBOMRMs, _MSTItmDetLocs, _MSTItmDetPrice }, constCodeKey);
                }
            }
            catch (Exception nex)
            {
                MsgBox.Show(nex.Message);
            }
            return ex;
        }

        public DataTable GetSerialList(int? ItmKey)
        {
            DataTable dt = null;
            try
            {
                using (TransactionScope transactionscope = new TransactionScope())
                {
                    using (System.Data.SqlClient.SqlConnection cn = new System.Data.SqlClient.SqlConnection(Database.BossDemoConnection))
                    {
                        List<SqlParameter> paraList = new List<SqlParameter>();
                        paraList.Add(new SqlParameter("@Option", 2));
                        paraList.Add(new SqlParameter("@ItmKey", ItmKey));
                        SqlParameter paraOut = new SqlParameter();
                        paraOut.ParameterName = "@RetValue";
                        paraOut.Value = 0;
                        paraOut.Direction = ParameterDirection.Output;
                        paraList.Add(paraOut);
                        dt = GFunc.ExecuteProc(cn, "MSTItmSerial_Get", paraList);
                        if (Transaction.Current.TransactionInformation.Status != TransactionStatus.Active) throw new Exception("Transaction has aborted."); transactionscope.Complete();
                        return dt;
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
        }//Completed

        public DataTable GetSerialList(int? ItmKey, string SerialFrom, string SerialTo)
        {
            DataTable dt = null;
            try
            {
                using (TransactionScope transactionscope = new TransactionScope())
                {
                    using (System.Data.SqlClient.SqlConnection cn = new System.Data.SqlClient.SqlConnection(Database.BossDemoConnection))
                    {
                        List<SqlParameter> paraList = new List<SqlParameter>();
                        paraList.Add(new SqlParameter("@Option", 3));
                        paraList.Add(new SqlParameter("@ItmKey", ItmKey));
                        paraList.Add(new SqlParameter("@SerialFrom", SerialFrom));
                        paraList.Add(new SqlParameter("@SerialTo", SerialTo));
                        SqlParameter paraOut = new SqlParameter();
                        paraOut.ParameterName = "@RetValue";
                        paraOut.Value = 0;
                        paraOut.Direction = ParameterDirection.Output;
                        paraList.Add(paraOut);
                        dt = GFunc.ExecuteProc(cn, "MSTItmSerial_Get", paraList);
                        if (Transaction.Current.TransactionInformation.Status != TransactionStatus.Active) throw new Exception("Transaction has aborted."); transactionscope.Complete();
                        return dt;
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
        }//Completed

        public bool SaveSerialList(string MFNNumber, int QtyToGenerate, DataTable _dtItmSerial, string lastnumberSerialNo, string lastnumberMACIDHEX, string lastBBID)
        {
            try
            {
                using (TransactionScope transactionscope = new TransactionScope())
                {
                    using (System.Data.SqlClient.SqlConnection cn = new System.Data.SqlClient.SqlConnection(Database.BossDemoConnection))
                    {
                        cn.Open();

                        List<SqlParameter> paraList = new List<SqlParameter>();
                        paraList.Add(new SqlParameter("@Option", "0"));
                        paraList.Add(new SqlParameter("@BatchItmKey", GFunc.IsNE(_dtItmSerial.Rows[0]["ItmKey"]) ? 0 : _dtItmSerial.Rows[0]["ItmKey"]));
                        paraList.Add(new SqlParameter("@BatchKey", "0"));
                        paraList.Add(new SqlParameter("@BatchID", MFNNumber));
                        paraList.Add(new SqlParameter("@BatchMfgDate", DateTime.Today.Date));
                        paraList.Add(new SqlParameter("@BatchExpDate", DateTime.Today.Date.AddYears(1)));
                        paraList.Add(new SqlParameter("@BatchQty", GFunc.NEInt(QtyToGenerate, 0)));
                        paraList.Add(new SqlParameter("@BatchQtyBal", GFunc.NEInt(QtyToGenerate, 0)));
                        paraList.Add(new SqlParameter("@BatchCost", "0"));
                        paraList.Add(new SqlParameter("@BatchStatus", 1));
                        paraList.Add(new SqlParameter("@LogDC", "0"));
                        paraList.Add(new SqlParameter("@LogDK", "0"));
                        paraList.Add(new SqlParameter("@LogDItm", "0"));
                        paraList.Add(new SqlParameter("@LogDocDate", DateTime.Today));
                        paraList.Add(new SqlParameter("@PurgeKeep", "0"));
                        paraList.Add(new SqlParameter("@PurgeData", "0"));

                        SqlParameter paraOut = new SqlParameter("@NewBatchKey", 0);
                        paraOut.Direction = ParameterDirection.Output;
                        paraList.Add(paraOut);

                        SqlParameter paraOut2 = new SqlParameter("@RetValue", 0);
                        paraOut2.Direction = ParameterDirection.Output;
                        paraList.Add(paraOut2);
                        GFunc.ExecuteProc(cn, "MSTItmBatch_AddUpdate", paraList);

                        if (paraOut2.Value.ToString() == "1")
                        {
                            int? newBatchKey = GFunc.NEInt(paraOut.Value, 0);

                            foreach (DataRow dr in _dtItmSerial.Rows)
                            {
                                dr["BatchKey"] = newBatchKey;
                                int? newSerialkey = 0;
                                if (!this._MSTItmSerials.Insert(cn, dr, out newSerialkey, 0))
                                {
                                    return false;
                                }
                            }
                        }

                        // Get all Serial No. of ItmKey
                        if (SysOptionUtility.DatabaseBranchCode == "OMS" || SysOptionUtility.DatabaseBranchCode == "OMSTW" || SysOptionUtility.DatabaseBranchCode == DBCode.ITS)
                        {
                            _MSTItmSerials.Clear();
                            if (_MSTItmSerials.Fetch(cn, new MSTItmSerials.Criteria(GFunc.NEInt(_dtItmSerial.Rows[0]["ItmKey"], 0), 2)) == false)
                            {
                                MsgBox.Show(cn, MsgID.Common.GetFail);
                                return false;
                            }
                        }
                        cn.Close();

                        if (lastnumberSerialNo != "0" && lastnumberMACIDHEX != "0" && lastBBID != "0")
                        {
                             //Save Custom5 and Custom6 (Last number Suffix (Serial No. and MAC I/D HEX))

                            _MSTItm.Custom5 = lastnumberSerialNo;
                            _MSTItm.Custom6 = lastnumberMACIDHEX;
                            _MSTItm.Custom8 = lastBBID;
                            this.Save();
                        }                     

                        if (Transaction.Current.TransactionInformation.Status != TransactionStatus.Active)
                            throw new Exception("Transaction has aborted.");
                        transactionscope.Complete();

                        return true;
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
        }//Completed

        public bool UpdateItmStatus(int? serialKey, bool itmStatus, string custom1, string custom2)
        {
            try
            {
                using (TransactionScope transactionscope = new TransactionScope())
                {
                    using (System.Data.SqlClient.SqlConnection cn = new System.Data.SqlClient.SqlConnection(Database.BossDemoConnection))
                    {
                        List<SqlParameter> paraList = new List<SqlParameter>();
                        paraList.Add(new SqlParameter("@Option", "4"));
                        paraList.Add(new SqlParameter("@SerialKey", serialKey));
                        paraList.Add(new SqlParameter("@ItmKey", DBNull.Value));
                        paraList.Add(new SqlParameter("@BatchKey", DBNull.Value));
                        paraList.Add(new SqlParameter("@SerialID", DBNull.Value));
                        paraList.Add(new SqlParameter("@MACAddress", DBNull.Value));
                        paraList.Add(new SqlParameter("@MfgDate", DBNull.Value));
                        paraList.Add(new SqlParameter("@ExpiryDate", DBNull.Value));
                        paraList.Add(new SqlParameter("@ItmStatus", itmStatus));
                        paraList.Add(new SqlParameter("@CreateDate", DBNull.Value));
                        paraList.Add(new SqlParameter("@CreateUserKey", DBNull.Value));
                        paraList.Add(new SqlParameter("@LastModifiedDate", DateTime.Now));
                        paraList.Add(new SqlParameter("@LastModifiedUserKey", AppInfor.currentUserKey));
                        paraList.Add(new SqlParameter("@Custom1", custom1));    //Remark
                        paraList.Add(new SqlParameter("@Custom2", custom2));    //BBID
                        paraList.Add(new SqlParameter("@Custom3", DBNull.Value));   //Branch
                        paraList.Add(new SqlParameter("@BatchID", DBNull.Value));

                        SqlParameter paraOut = new SqlParameter("@NewSerialKey", 0);
                        paraOut.Direction = ParameterDirection.Output;
                        paraList.Add(paraOut);

                        SqlParameter paraOut2 = new SqlParameter("@RetValue", 0);
                        paraOut2.Direction = ParameterDirection.Output;
                        paraList.Add(paraOut2);
                        GFunc.ExecuteProc(cn, "MSTItmSerial_AddUpdate", paraList);
                        if (Transaction.Current.TransactionInformation.Status != TransactionStatus.Active) throw new Exception("Transaction has aborted."); transactionscope.Complete();
                        if (paraOut2.Value.ToString() == "1")
                        {
                            return true;
                        }
                        else
                        {
                            return false;
                        }
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
        }//Completed
    }
}
