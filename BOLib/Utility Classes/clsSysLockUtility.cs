using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.SqlClient;
using TAUtil;

namespace BOLib
{
    public class SysLockUtility
    {
        /// <summary>
        /// Pass Option 6 
        /// </summary>
        /// <param name="cn"></param>
        /// <param name="msgID"></param>
        /// <param name="option"></param>
        /// <param name="codeKey"></param>
        /// <param name="dataKey"></param>
        /// <param name="GUID"></param>
        /// <returns></returns>
        private string _errorMessage = string.Empty;

        public static bool CheckAddLock(bool DisplayMsg, int? option, GEnum.SystemCode codeKey, int? dataKey, int? GUID)
        {
            string msgID = string.Empty;

            try
            {
                //Get new instance
                SYSLock objSysLock = SYSLock.New();

                //Assign object data
                objSysLock._codeKey = codeKey;
                objSysLock._dataKey = dataKey;
                objSysLock._inprogressKey = 0;
                objSysLock._guid = GUID;
                objSysLock._userKey = AppInfor.currentUserKey;
                if (objSysLock.CheckAddLock())
                    return true;
                else
                {
                    SECUser objUser = SECUser.New();
                    if (objUser.Fetch(new SECUser.Criteria(objSysLock._userKey, 1)))
                        msgID = MsgID.Locking.IsLockTrue + "%" + objUser._userID + "%" + objUser._userName + "%" + codeKey;
                    if (DisplayMsg)
                        MsgBox.Show(msgID);

                    return false;
                }
            }
            catch (Exception ex)
            {
                throw Error(ex);
            }
        }

        public static bool CheckAddLock(SqlConnection cn, bool DisplayMsg, int? option, GEnum.SystemCode codeKey, int? dataKey, int? GUID)
        {
            string msgID = string.Empty;

            try
            {
                //Get new instance
                SYSLock objSysLock = SYSLock.New();

                //Assign object data
                objSysLock._codeKey = codeKey;
                objSysLock._dataKey = dataKey;
                objSysLock._inprogressKey = 0;
                objSysLock._guid = GUID;
                objSysLock._userKey = AppInfor.currentUserKey;
                if (objSysLock.CheckAddLock(cn))
                    return true;
                else
                {
                    SECUser objUser = SECUser.New();
                    if (objUser.Fetch(cn, new SECUser.Criteria(objSysLock._userKey, 1)))
                        msgID = MsgID.Locking.IsLockTrue + "%" + objUser._userID + "%" + objUser._userName + "%" + codeKey;
                    if (DisplayMsg)
                        MsgBox.Show(cn,msgID);

                    return false;
                }
            }
            catch (Exception ex)
            {
                throw Error(ex);
            }
        }

        public static bool IsLock(SqlConnection cn, bool DisplayMsg, GEnum.SysLockOption option, GEnum.SystemCode codeKey, int? dataKey, int? inProgressKey, int? GUID)
        {
            string msgID = string.Empty;

            try
            {
                SYSLock objSYSLock = new SYSLock();
                if (objSYSLock.Fetch(cn, new SYSLock.Criteria(GUID, AppInfor.currentUserKey, codeKey, dataKey, inProgressKey, (int?)option)))
                {
                    if (objSYSLock._codeKey != 0)
                    {
                        //Already lock by currentuser
                        if (objSYSLock._guid == GUID)
                        {
                            return false;
                        }                        
                        else
                        {
                            //Fetch user info and modify msgID
                            SECUser objUser = SECUser.New();
                            if (objUser.Fetch(cn, new SECUser.Criteria(objSYSLock._userKey, 1)))
                                msgID = MsgID.Locking.IsLockTrue + "%" + objUser._userID + "%" + objUser._userName + "%" + codeKey;


                            if (DisplayMsg)
                            {
                                MsgBox.Show(cn, msgID);
                                return true;
                            }
                            else
                            {
                                return true;    //Mic - cannot throw as the caller needs to handle condtions when it is lock
                                //throw new TAException(msgID); //Modified by May on 19-Feb-2011
                            }
                        }
                    }
                    return false;
                }
                else
                    return true;
            }
            catch (TAException taex)
            {
                throw Error(taex);
            }
            catch (Exception ex)
            {
                throw Error(ex);
            }
        }
        public static bool IsLockBySameGUID(SqlConnection cn, bool DisplayMsg, GEnum.SysLockOption option, GEnum.SystemCode codeKey, int? dataKey, int? inProgressKey, int? GUID)
        {
            string msgID = string.Empty;

            try
            {
                SYSLock objSYSLock = new SYSLock();
                if (objSYSLock.Fetch(cn, new SYSLock.Criteria(GUID, AppInfor.currentUserKey, codeKey, dataKey, inProgressKey, (int?)option)))
                {
                    if (objSYSLock._codeKey != 0)
                    {
                        //Already lock by currentuser

                        if (objSYSLock._guid == GUID)                     
                        {
                           
                            if (DisplayMsg)
                            {
                                MsgBox.Show(cn, "IsLockTrueCurrentUser");
                                return true;
                            }
                            else
                            {
                                //Mic - just return TRUE so that the caller can handle the lock conditions
                                //RemoveLock(cn, false, (int?)option, codeKey, GUID, dataKey, inProgressKey);
                                //MsgBox.Show(cn, "IsLockTrueCurrentUser");
                                //throw new TAException(MsgID.Common.NoMultiInstanceAllowed);
                                return true;
                            }

                           
                        }
                        else
                        {
                            //Fetch user info and modify msgID
                            SECUser objUser = SECUser.New();
                            if (objUser.Fetch(cn, new SECUser.Criteria(objSYSLock._userKey, 1)))
                                msgID = MsgID.Locking.IsLockTrue + "%" + objUser._userID + "%" + objUser._userName;

                            if (DisplayMsg)
                            {
                                MsgBox.Show(cn, msgID);
                            }
                            return true;
                        }
                    }
                    return false;
                }
                else
                    return true;
            }
            catch (TAException taex)
            {
                throw Error(taex);
            }
            catch (Exception ex)
            {
                throw Error(ex);
            }
        }
        public static bool IsLock(SqlConnection cn, bool DisplayMsg, GEnum.SysLockOption option, GEnum.SystemCode codeKey, int? dataKey, int? inProgressKey, int? GUID, bool ignoreLockBySameGUID)
        {
            string msgID = string.Empty;

            try
            {
                SYSLock objSYSLock = new SYSLock();
                if (objSYSLock.Fetch(cn, new SYSLock.Criteria(GUID, AppInfor.currentUserKey, codeKey, dataKey, inProgressKey, (int?)option)))
                {
                    if (objSYSLock._codeKey != 0)
                    {
                        //Already lock by currentuser

                        if (objSYSLock._guid == GUID)
                        {
                            if (ignoreLockBySameGUID)
                            {
                                return false;
                            }
                            else
                            {
                                if (DisplayMsg)
                                {
                                    MsgBox.Show(cn, "IsLockTrueCurrentUser");
                                    return true;
                                }
                                else
                                {
                                    //Mic - just return TRUE so that the caller can handle the lock conditions
                                    //RemoveLock(cn, false, (int?)option, codeKey, GUID, dataKey, inProgressKey);
                                    //MsgBox.Show(cn, "IsLockTrueCurrentUser");
                                    //throw new TAException(MsgID.Common.NoMultiInstanceAllowed);
                                    return true;
                                }

                            }
                        }
                        else
                        {
                            //Fetch user info and modify msgID
                            SECUser objUser = SECUser.New();
                            if (objUser.Fetch(cn, new SECUser.Criteria(objSYSLock._userKey, 1)))
                                msgID = MsgID.Locking.IsLockTrue + "%" + objUser._userID + "%" + objUser._userName;

                            if (DisplayMsg)
                            {
                                MsgBox.Show(cn, msgID);
                            }
                            return true;
                        }
                    }
                    return false;
                }
                else
                    return true;
            }
            catch (TAException taex)
            {
                throw Error(taex);
            }
            catch (Exception ex)
            {
                throw Error(ex);
            }
        }        
        public static bool AddLock(bool DisplayMsg, int? GUID, GEnum.SystemCode codeKey, int? dataKey)
        {
            string msgID = string.Empty;

            try
            {
                SYSLock objSYSLock = new SYSLock();
                objSYSLock._codeKey = codeKey;
                objSYSLock._dataKey = dataKey;
                objSYSLock._inprogressKey = 0;
                objSYSLock._guid = GUID;

                if (objSYSLock.Insert())
                    return true;
                else
                {
                    if (DisplayMsg)
                        MsgBox.Show(MsgID.Locking.LockAddFail);

                    return false;
                }
            }
            catch (TAException taex)
            {
                throw Error(taex);
            }
            catch (Exception ex)
            {
                throw Error(ex);
            }
        }
        public static bool AddLock(SqlConnection cn, bool DisplayMsg, int? GUID, GEnum.SystemCode codeKey, int? dataKey)
        {
            string msgID = string.Empty;

            try
            {
                SYSLock objSYSLock = new SYSLock();
                objSYSLock._codeKey = codeKey;
                objSYSLock._dataKey = dataKey;
                objSYSLock._inprogressKey = 0;
                objSYSLock._guid = GUID;

                if (objSYSLock.Insert(cn))
                    return true;
                else
                {
                    if (DisplayMsg)
                        MsgBox.Show(cn, MsgID.Locking.LockAddFail);

                    return false;
                }
            }
            catch (TAException taex)
            {
                throw Error(taex);
            }
            catch (Exception ex)
            {
                throw Error(ex);
            }
        }        
        public static bool AddInprogressLock(SqlConnection cn, bool displayMsg, int? GUID, GEnum.SystemCode codeKey)
        {
            string msgID = string.Empty;

            try
            {
                if (GFunc.IsNE(AppInfor.currentUserKey))
                {
                    MsgBox.Show(cn,"Invalid Current User");
                    return true;
                }

                SYSLock objSYSLock = SYSLock.New();
                objSYSLock._codeKey = codeKey;
                objSYSLock._inprogressKey = GUID;
                objSYSLock._dataKey = 0;
                objSYSLock._guid = GUID;
                objSYSLock._userKey = AppInfor.currentUserKey;

                if (Convert.ToBoolean(objSYSLock.Insert(cn)) == true)
                    return true;
                else
                {
                    if (displayMsg)
                        MsgBox.Show(cn,MsgID.Locking.LockAddFail);

                    return false;
                }
            }
            catch (TAException taex)
            {

                throw Error(taex);
            }
            catch (Exception ex)
            {

                throw Error(ex);
            }
        }
        
        public static bool AddListInprogressLock(SqlConnection cn, bool DisplayMsg, int? GUID, GEnum.SystemCode codeKey)
        {
            string msgID = string.Empty;

            try
            {
                if (GFunc.IsNE(AppInfor.currentUserKey))
                {
                    MsgBox.Show(cn,"Invalid Current User");
                    return true;
                }

                SYSLock objSYSLock = SYSLock.New();
                objSYSLock._codeKey = codeKey;
                objSYSLock._inprogressKey = AppInfor.currentUserKey;
                objSYSLock._userKey = AppInfor.currentUserKey;
                objSYSLock._dataKey = -100;
                objSYSLock._guid = GUID;

                if (Convert.ToBoolean(objSYSLock.Insert(cn)) == true)
                    return true;
                else
                {
                    if (DisplayMsg)
                        MsgBox.Show(cn, MsgID.Locking.LockAddFail);

                    return false;
                }
            }
            catch (TAException taex)
            {
                throw Error(taex);
            }
            catch (Exception ex)
            {
                throw Error(ex);
            }
        }
        
        public static bool AddPostingLock(bool DisplayMsg, int? GUID, GEnum.SystemCode codeKey)
        {
            string msgID = string.Empty;

            try
            {
                if (GFunc.IsNE(AppInfor.currentUserKey))
                {
                    MsgBox.Show("Invalid Current User");
                    return true;
                }
                SYSLock objSYSLock = SYSLock.New();
                objSYSLock._codeKey = codeKey;
                objSYSLock._inprogressKey = -1000;
                objSYSLock._userKey = AppInfor.currentUserKey;
                objSYSLock._dataKey = 0;
                objSYSLock._guid = GUID;

                if (Convert.ToBoolean(objSYSLock.Insert()) == true)
                    return true;
                else
                {
                    if (DisplayMsg)
                        MsgBox.Show(MsgID.Locking.LockAddFail);

                    return false;
                }
            }
            catch (TAException taex)
            {

                throw Error(taex);
            }
            catch (Exception ex)
            {

                throw Error(ex);
            }
        }

        public static bool AddPostingLock(SqlConnection cn, bool DisplayMsg, int? GUID, GEnum.SystemCode codeKey)
        {
            string msgID = string.Empty;

            try
            {
                if (GFunc.IsNE(AppInfor.currentUserKey))
                {
                    MsgBox.Show(cn,"Invalid Current User");
                    return true;
                }
                SYSLock objSYSLock = SYSLock.New();
                objSYSLock._codeKey = codeKey;
                objSYSLock._inprogressKey = -1000;
                objSYSLock._userKey = AppInfor.currentUserKey;
                objSYSLock._dataKey = 0;
                objSYSLock._guid = GUID;

                if (Convert.ToBoolean(objSYSLock.Insert(cn)) == true)
                    return true;
                else
                {
                    if (DisplayMsg)
                        MsgBox.Show(cn, MsgID.Locking.LockAddFail);

                    return false;
                }
            }
            catch (TAException taex)
            {

                throw Error(taex);
            }
            catch (Exception ex)
            {

                throw Error(ex);
            }
        }

        ///<summary>
        ///option 0- By All,
        ///option 1- By Code Key,
        ///option 2- By Code Key and Data Key, 
        ///option 3- By CodeKey=@CodeKey And DataKey=@DataKey And InprogressKey=@InprogressKey
        ///option 4- By GUID
        ///option 5- By CodeKey And GUID
        ///option 6- By CodeKey And GUID And Data Key
        ///option 7- By CodeKey And DataKey And InprogressKey And GUID
        ///option 8- By UserKey
        ///option 9- By CodeKey And UserKey
        ///option 10- By CodeKey And DataKey And UserKey
        ///option 11- By CodeKey And DataKey And InprogressKey And UserKey
        ///option 12- By CodeKey And GUID AND DataKey<>0
        ///</summary>
        
        public static bool RemoveLock(bool DisplayMsg, GEnum.SysLockOption option, GEnum.SystemCode codeKey, int? GUID, int? dataKey, int? inProgressKey)
        {
            string msgID = string.Empty;

            try
            {
                SYSLock objSYSLock = new SYSLock();
                if (objSYSLock.Delete(new SYSLock.Criteria(GUID, AppInfor.currentUserKey, codeKey, dataKey, inProgressKey, (int?)option)))
                    return true;
                else
                    if (DisplayMsg)
                        MsgBox.Show(MsgID.Locking.LockDeleteFail);

                return false;
            }
            catch (TAException taex)
            {
                throw Error(taex);
            }
            catch (Exception ex)
            {
                throw Error(ex);
            }
        }

        //RemoveLock by GUID
        
        public static bool RemoveLock(SqlConnection cn, bool DisplayMsg, int? option, GEnum.SystemCode codeKey, int? GUID, int? dataKey, int? inProgressKey)
        {
            string msgID = string.Empty;

            try
            {
                SYSLock objSYSLock = new SYSLock();
                if (objSYSLock.Delete(cn, new SYSLock.Criteria(GUID, AppInfor.currentUserKey, codeKey, dataKey, inProgressKey, option)))
                    return true;
                else
                    if (DisplayMsg)
                        MsgBox.Show(cn, MsgID.Locking.LockDeleteFail);

                return false;
            }
            catch (TAException taex)
            {
                throw Error(taex);
            }
            catch (Exception ex)
            {
                throw Error(ex);
            }
        }

        //RemoveLock by codeKey and dataKey=0 
        public static bool RemoveLockGUIDKeepIP(bool DisplayMsg, int? guid, GEnum.SystemCode codeKey)
        {
            string msgID = string.Empty;

            try
            {
                SYSLock objSYSLock = SYSLock.New();
                if (objSYSLock.Delete(new SYSLock.Criteria(guid, AppInfor.currentUserKey, codeKey, 0, 0, (int?)GEnum.SysLockOption.ByCodeKeyAndGUIDANDDataKeyNotEqual0)))
                    return true;
                else
                {
                    if (DisplayMsg)
                        MsgBox.Show(MsgID.Locking.LockDeleteFail);

                    return false;
                }
            }
            catch (TAException taex)
            {
                throw Error(taex);
            }
            catch (Exception ex)
            {
                throw Error(ex);
            }
        }
        
        public static bool RemoveLockGUIDKeepIP(SqlConnection cn, bool DisplayMsg, int? guid, GEnum.SystemCode codeKey)
        {
            string msgID = string.Empty;

            try
            {
                SYSLock objSYSLock = SYSLock.New();
                if (objSYSLock.Delete(cn, new SYSLock.Criteria(guid, AppInfor.currentUserKey, codeKey, 0, 0, (int?)GEnum.SysLockOption.ByCodeKeyAndGUIDANDDataKeyNotEqual0)))
                    return true;
                else
                {
                    if (DisplayMsg)
                        MsgBox.Show(cn, MsgID.Locking.LockDeleteFail);

                    return false;
                }
            }
            catch (TAException taex)
            {
                throw Error(taex);
            }
            catch (Exception ex)
            {
                throw Error(ex);
            }
        }

        //Check process lock and multiinstace
        public static bool IsProcessLock(bool DisplayMsg, GEnum.SysLockOption option, GEnum.SystemCode codeKey, int? GUID)
        {
            string msgID = string.Empty;
            bool multiInstance = false;
            bool listMultiInstance = false;
            bool multiUser = false;

            try
            {
                using (SqlConnection cn = new SqlConnection(Database.BossDemoConnection))
                {
                    cn.Open();
                    switch (codeKey)
                    {
                        #region No Multi-Instance, No Multi-User
                        //Utility
                        case GEnum.SystemCode.Account_Unreconciled_Trans:
                        case GEnum.SystemCode.Period:
                        case GEnum.SystemCode.Bank_Reconcilation:
                        case GEnum.SystemCode.COSBatchPost:
                        case GEnum.SystemCode.Transaction_Group:
                        case GEnum.SystemCode.ARAP_Revaluation:
                        case GEnum.SystemCode.Financial_Charge:
                        case GEnum.SystemCode.RecordAccess:

                        //Document
                        case GEnum.SystemCode.Untrack_SO:
                        case GEnum.SystemCode.DO_to_IV_Transfer:
                        case GEnum.SystemCode.Untrack_PO:
                        case GEnum.SystemCode.Untrack_Issue_Consignment:
                        case GEnum.SystemCode.Untrack_Consignment_Order:

                        //Master
                        case GEnum.SystemCode.Price_Update:
                        case GEnum.SystemCode.Account_Opening_Balance:
                        case GEnum.SystemCode.AR_Opening_Balance:
                        case GEnum.SystemCode.AR_Cash_Opening_Balance:
                        case GEnum.SystemCode.AP_Opening_Balance:

                            multiInstance = false;
                            listMultiInstance = false;
                            multiUser = false;
                            break;
                        #endregion

                        #region No Multi-Instance, Allow Multi-User
                        //Security
                        case GEnum.SystemCode.Security_User:
                        case GEnum.SystemCode.Security_Group:
                        case GEnum.SystemCode.Security_ChangePassword:

                        //Utility
                        case GEnum.SystemCode.Main_Screen:
                        case GEnum.SystemCode.System_Code:
                        case GEnum.SystemCode.CounterGrp:
                        case GEnum.SystemCode.System_Option:
                        case GEnum.SystemCode.Screen_Customisation:
                        case GEnum.SystemCode.Company_Setup_Check_List:
                        case GEnum.SystemCode.Audit_Log:
                        case GEnum.SystemCode.Message_List:
                        case GEnum.SystemCode.Other_Report_Setting:
                        case GEnum.SystemCode.Report_ID_Format:
                        case GEnum.SystemCode.Report_Set_Rpt_Files:
                        case GEnum.SystemCode.Purchase_Requisition:
                        case GEnum.SystemCode.AP_PO_Confirm_Number:
                        case GEnum.SystemCode.Stock_Count:
                        case GEnum.SystemCode.Cash_Flow:
                        case GEnum.SystemCode.Import_Data:

                        //Master
                        case GEnum.SystemCode.Branch:
                        case GEnum.SystemCode.Department:
                        case GEnum.SystemCode.Inventory_Opening_Balance:
                        case GEnum.SystemCode.Job_Timesheet:

                        //Reference
                        case GEnum.SystemCode.Document_Group:
                        case GEnum.SystemCode.General_List:
                        case GEnum.SystemCode.Currency:
                        case GEnum.SystemCode.Bank:
                        case GEnum.SystemCode.Payment_Mode:
                        case GEnum.SystemCode.Tax_Authority:
                        case GEnum.SystemCode.Tax_Group:
                        case GEnum.SystemCode.Overhead:
                        case GEnum.SystemCode.Account_Group:
                        case GEnum.SystemCode.Sales_Representative:
                        case GEnum.SystemCode.Budget:
                        case GEnum.SystemCode.Payment_Term:
                        case GEnum.SystemCode.Territory:
                        case GEnum.SystemCode.Industry:
                        case GEnum.SystemCode.Shipping_Mode:
                        case GEnum.SystemCode.Packing_Type:
                        case GEnum.SystemCode.Category:
                        case GEnum.SystemCode.Brand:
                        case GEnum.SystemCode.UOM:
                        case GEnum.SystemCode.Color:
                        case GEnum.SystemCode.Scale:
                        case GEnum.SystemCode.Location:
                        case GEnum.SystemCode.Job_Cost_Type:
                        case GEnum.SystemCode.Job_Phase:
                        case GEnum.SystemCode.Job_Task:
                        case GEnum.SystemCode.Job_Group:
                        case GEnum.SystemCode.Machine_Type_List:
                        case GEnum.SystemCode.Alerts:
                        case GEnum.SystemCode.Alert_Log:
                        case GEnum.SystemCode.To_Do:
                        case GEnum.SystemCode.To_Do_Log:
                        case GEnum.SystemCode.Interest_Rate:

                            multiInstance = false;
                            listMultiInstance = false;
                            multiUser = true;
                            break;
                        #endregion

                        #region Allow Multi-Instance, Allow Multi-User
                        //Docment
                        case GEnum.SystemCode.Quotation:
                        case GEnum.SystemCode.Sales_Order:
                        case GEnum.SystemCode.Sales_Order_Adjustment:
                        case GEnum.SystemCode.Works_Order:
                        case GEnum.SystemCode.Delivery_Order:
                        case GEnum.SystemCode.Packing_List:
                        case GEnum.SystemCode.Sales_Invoice:
                        case GEnum.SystemCode.Sales_Debit_Note:
                        case GEnum.SystemCode.Sales_Credit_Note:
                        case GEnum.SystemCode.Sales_Adjustment:
                        case GEnum.SystemCode.Payment_Received:
                        case GEnum.SystemCode.Contra:
                        case GEnum.SystemCode.Cash_Sale:
                        case GEnum.SystemCode.Cash_Debit_Note:
                        case GEnum.SystemCode.Cash_Credit_Note:
                        case GEnum.SystemCode.Cash_Adjustment:
                        case GEnum.SystemCode.Cash_Payment_Received:
                        case GEnum.SystemCode.Cash_Contra:
                        case GEnum.SystemCode.Purchase_Plan:
                        case GEnum.SystemCode.Purchase_Request:
                        case GEnum.SystemCode.Purchase_Order:
                        case GEnum.SystemCode.Purchase_Order_Adjustment:
                        case GEnum.SystemCode.Purchase_Delivery:
                        case GEnum.SystemCode.Purchase_Invoice:
                        case GEnum.SystemCode.Purchase_Debit_Note:
                        case GEnum.SystemCode.Purchase_Credit_Note:
                        case GEnum.SystemCode.Purchase_Adjustment:
                        case GEnum.SystemCode.Payment_Issue:
                        case GEnum.SystemCode.Inventory_Adjustment:
                        case GEnum.SystemCode.Inventory_Production:
                        case GEnum.SystemCode.Inventory_Transfer:
                        case GEnum.SystemCode.Issue_Consignment:
                        case GEnum.SystemCode.Return_Consignment:
                        case GEnum.SystemCode.Order_Consignment:
                        case GEnum.SystemCode.Consignment_Order_Adjustment:
                        case GEnum.SystemCode.Received_Consignment:
                        case GEnum.SystemCode.Consignment_Settlement:
                        case GEnum.SystemCode.Journal:
                        case GEnum.SystemCode.Deposit:
                        case GEnum.SystemCode.Bank_Revaluation:
                            multiInstance = true;
                            listMultiInstance = false;
                            multiUser = true;
                            break;
                        //Master
                        case GEnum.SystemCode.Inventory:
                        case GEnum.SystemCode.Account:
                        case GEnum.SystemCode.Customer:
                        case GEnum.SystemCode.Vendor:
                        case GEnum.SystemCode.Price_List:
                        case GEnum.SystemCode.Job:
                        case GEnum.SystemCode.Machine_List:
                        case GEnum.SystemCode.Ship_Name:

                            multiInstance = false;
                            listMultiInstance = false;
                            multiUser = true;
                            break;
                        #endregion

                        default:
                            multiInstance = true;
                            listMultiInstance = false;
                            multiUser = true;
                            break;
                    }

                    if (CheckInProgressLock(cn, DisplayMsg, codeKey))
                        return true;

                    #region Check Disallow multi-instance
                    if (!multiInstance) //Disallow multi-instance
                    {
                        if (IsLock(cn, false, GEnum.SysLockOption.ByCodeKeyAndDataKeyAndUserKey, codeKey, 0, 0, GUID, false))
                        {
                            if (DisplayMsg)
                                MsgBox.Show(cn, MsgID.Common.NoMultiInstanceAllowed);

                            return true;
                        }
                    }
                    #endregion

                    #region Check Disallow multi-user
                    if (!multiUser) //Disallow multi-user
                    {
                        if (IsLock(cn, DisplayMsg, GEnum.SysLockOption.ByCodeKeyandDataKey, codeKey, 0, 0, 0))
                        {
                            //if (DisplayMsg)
                            //    MsgBox.Show(cn, MsgID.Common.NoMultiInstanceAllowed);

                            return true;
                        }
                    }
                    #endregion

                    #region Check Disallow List multi-instance
                    if (!listMultiInstance)//Disallow List multi-instance
                    {
                        if (IsLock(cn, false, GEnum.SysLockOption.ByCodeKeyAndDataKeyAndInprogressKeyAndUserKey, codeKey, 0, -100, 0))
                        {
                            if (DisplayMsg)
                                MsgBox.Show(cn,MsgID.Common.NoMultiInstanceAllowed);

                            return true;
                        }
                    }
                    #endregion
                }
                return false;
            }
            catch (TAException taex)
            {
                throw Error(taex);
            }
            catch (Exception ex)
            {
                throw Error(ex);
            }
        }
        public static bool IsProcessLock(SqlConnection cn, bool DisplayMsg, GEnum.SysLockOption option, GEnum.SystemCode codeKey, int? GUID)
        {
            string msgID = string.Empty;
            bool multiInstance = false;
            bool listMultiInstance = false;
            bool multiUser = false;

            try
            {
                switch (codeKey)
                {
                    #region No Multi-Instance, No Multi-User
                    //Utility
                    case GEnum.SystemCode.Account_Unreconciled_Trans:
                    case GEnum.SystemCode.Period:
                    case GEnum.SystemCode.Bank_Reconcilation:
                    case GEnum.SystemCode.COSBatchPost:
                    case GEnum.SystemCode.Transaction_Group:
                    case GEnum.SystemCode.ARAP_Revaluation:
                    case GEnum.SystemCode.Financial_Charge:
                    case GEnum.SystemCode.RecordAccess:

                    //Document
                    case GEnum.SystemCode.Untrack_SO:
                    case GEnum.SystemCode.DO_to_IV_Transfer:
                    case GEnum.SystemCode.Untrack_PO:
                    case GEnum.SystemCode.Untrack_Issue_Consignment:
                    case GEnum.SystemCode.Untrack_Consignment_Order:

                    //Master
                    case GEnum.SystemCode.Price_Update:
                    case GEnum.SystemCode.Account_Opening_Balance:
                    case GEnum.SystemCode.AR_Opening_Balance:
                    case GEnum.SystemCode.AR_Cash_Opening_Balance:
                    case GEnum.SystemCode.AP_Opening_Balance:

                        multiInstance = false;
                        listMultiInstance = false;
                        multiUser = false;
                        break;
                    #endregion

                    #region No Multi-Instance, Allow Multi-User
                    //Security
                    case GEnum.SystemCode.Security_User:
                    case GEnum.SystemCode.Security_Group:
                    case GEnum.SystemCode.Security_ChangePassword:
                    case GEnum.SystemCode.MRO_TopCust:

                    //Utility
                    case GEnum.SystemCode.Main_Screen:
                    case GEnum.SystemCode.System_Code:
                    case GEnum.SystemCode.CounterGrp:
                    case GEnum.SystemCode.System_Option:
                    case GEnum.SystemCode.Screen_Customisation:
                    case GEnum.SystemCode.Company_Setup_Check_List:
                    case GEnum.SystemCode.Audit_Log:
                    case GEnum.SystemCode.Message_List:
                    case GEnum.SystemCode.Other_Report_Setting:
                    case GEnum.SystemCode.Report_ID_Format:
                    case GEnum.SystemCode.Report_Set_Rpt_Files:
                    case GEnum.SystemCode.Purchase_Requisition:
                    case GEnum.SystemCode.AP_PO_Confirm_Number:
                    case GEnum.SystemCode.Stock_Count:
                    case GEnum.SystemCode.Cash_Flow:
                    case GEnum.SystemCode.Import_Data:

                    //Master
                    case GEnum.SystemCode.Branch:
                    case GEnum.SystemCode.Department:
                    case GEnum.SystemCode.Inventory_Opening_Balance:
                    case GEnum.SystemCode.Job_Timesheet:

                    //Reference
                    case GEnum.SystemCode.Document_Group:
                    case GEnum.SystemCode.General_List:
                    case GEnum.SystemCode.Currency:
                    case GEnum.SystemCode.Bank:
                    case GEnum.SystemCode.Payment_Mode:
                    case GEnum.SystemCode.Tax_Authority:
                    case GEnum.SystemCode.Tax_Group:
                    case GEnum.SystemCode.Overhead:
                    case GEnum.SystemCode.Account_Group:
                    case GEnum.SystemCode.Sales_Representative:
                    case GEnum.SystemCode.Budget:
                    case GEnum.SystemCode.Payment_Term:
                    case GEnum.SystemCode.Territory:
                    case GEnum.SystemCode.Industry:
                    case GEnum.SystemCode.Shipping_Mode:
                    case GEnum.SystemCode.Packing_Type:
                    case GEnum.SystemCode.Category:
                    case GEnum.SystemCode.Brand:
                    case GEnum.SystemCode.UOM:
                    case GEnum.SystemCode.Color:
                    case GEnum.SystemCode.Scale:
                    case GEnum.SystemCode.Location:
                    case GEnum.SystemCode.Job_Cost_Type:
                    case GEnum.SystemCode.Job_Phase:
                    case GEnum.SystemCode.Job_Task:
                    case GEnum.SystemCode.Job_Group:
                    case GEnum.SystemCode.Machine_Type_List:
                    case GEnum.SystemCode.Alerts:
                    case GEnum.SystemCode.Alert_Log:
                    case GEnum.SystemCode.To_Do:
                    case GEnum.SystemCode.To_Do_Log:
                    case GEnum.SystemCode.Interest_Rate:
                    case GEnum.SystemCode.Works_Order_Type:
                    case GEnum.SystemCode.WO_Request_Type:
                    case GEnum.SystemCode.Vehicle:

                        multiInstance = false;
                        listMultiInstance = false;
                        multiUser = true;
                        break;
                    #endregion

                    #region Allow Multi-Instance, Allow Multi-User
                    //Docment
                    case GEnum.SystemCode.Quotation:
                    case GEnum.SystemCode.Sales_Order:
                    case GEnum.SystemCode.Sales_Order_Adjustment:
                    case GEnum.SystemCode.Works_Order:
                    case GEnum.SystemCode.Delivery_Order:
                    case GEnum.SystemCode.Packing_List:
                    case GEnum.SystemCode.Sales_Invoice:
                    case GEnum.SystemCode.Sales_Debit_Note:
                    case GEnum.SystemCode.Sales_Credit_Note:
                    case GEnum.SystemCode.Sales_Adjustment:
                    case GEnum.SystemCode.Payment_Received:
                    case GEnum.SystemCode.Contra:
                    case GEnum.SystemCode.Cash_Sale:
                    case GEnum.SystemCode.Cash_Debit_Note:
                    case GEnum.SystemCode.Cash_Credit_Note:
                    case GEnum.SystemCode.Cash_Adjustment:
                    case GEnum.SystemCode.Cash_Payment_Received:
                    case GEnum.SystemCode.Cash_Contra:
                    case GEnum.SystemCode.Purchase_Plan:
                    case GEnum.SystemCode.Purchase_Request:
                    case GEnum.SystemCode.Purchase_Order:
                    case GEnum.SystemCode.Purchase_Order_Adjustment:
                    case GEnum.SystemCode.Purchase_Delivery:
                    case GEnum.SystemCode.Purchase_Invoice:
                    case GEnum.SystemCode.Purchase_Debit_Note:
                    case GEnum.SystemCode.Purchase_Credit_Note:
                    case GEnum.SystemCode.Purchase_Adjustment:
                    case GEnum.SystemCode.Payment_Issue:
                    case GEnum.SystemCode.Inventory_Adjustment:
                    case GEnum.SystemCode.Inventory_Production:
                    case GEnum.SystemCode.Inventory_Transfer:
                    case GEnum.SystemCode.Issue_Consignment:
                    case GEnum.SystemCode.Return_Consignment:
                    case GEnum.SystemCode.Order_Consignment:
                    case GEnum.SystemCode.Consignment_Order_Adjustment:
                    case GEnum.SystemCode.Received_Consignment:
                    case GEnum.SystemCode.Consignment_Settlement:
                    case GEnum.SystemCode.Journal:
                    case GEnum.SystemCode.Deposit:
                    case GEnum.SystemCode.Bank_Revaluation:
                        multiInstance = true;
                        listMultiInstance = false;
                        multiUser = true;
                        break;
                    //Master
                    case GEnum.SystemCode.Inventory:
                    case GEnum.SystemCode.Account:
                    case GEnum.SystemCode.Customer:
                    case GEnum.SystemCode.Vendor:
                    case GEnum.SystemCode.Price_List:
                    case GEnum.SystemCode.Job:
                    case GEnum.SystemCode.Machine_List:
                    case GEnum.SystemCode.Ship_Name:

                        multiInstance = false;
                        listMultiInstance = false;
                        multiUser = true;
                        break;
                    #endregion

                    default:
                        multiInstance = true;
                        listMultiInstance = false;
                        multiUser = true;
                        break;
                }

                if (CheckInProgressLock(cn, DisplayMsg, codeKey))
                    return true;

                #region Check Disallow multi-instance
                if (!multiInstance) //Disallow multi-instance
                {
                    if (IsLock(cn, false, GEnum.SysLockOption.ByCodeKeyAndDataKeyAndUserKey, codeKey, 0, 0, GUID, false))
                    {
                        if (DisplayMsg)
                            MsgBox.Show(cn, MsgID.Common.NoMultiInstanceAllowed);

                        return true;
                    }
                }
                #endregion

                #region Check Disallow multi-user
                if (!multiUser) //Disallow multi-user
                {
                    if (IsLock(cn, DisplayMsg, GEnum.SysLockOption.ByCodeKeyandDataKey, codeKey, 0, 0, 0))
                    {
                        //if (DisplayMsg)
                        //    MsgBox.Show(cn, MsgID.Common.NoMultiInstanceAllowed);

                        return true;
                    }
                }
                #endregion

                #region Check Disallow List multi-instance
                if (!listMultiInstance)//Disallow List multi-instance
                {
                    if (IsLock(cn, false, GEnum.SysLockOption.ByCodeKeyAndDataKeyAndInprogressKeyAndUserKey, codeKey, 0, -100, 0))
                    {
                        if (DisplayMsg)
                            MsgBox.Show(cn,MsgID.Common.NoMultiInstanceAllowed);

                            return true;
                    }
                }
                #endregion

                return false;
            }
            catch (TAException taex)
            {
                throw Error(taex);
            }
            catch (Exception ex)
            {
                throw Error(ex);
            }
        }
                
        public static bool CheckInProgressLock(SqlConnection cn, bool DisplayMsg, GEnum.SystemCode codeKey)
        {
            bool isProcessInUse = false;


            try
            {
                switch (codeKey)
                {
                    #region ARSO,APPO,CSPO
                    case GEnum.SystemCode.Sales_Order:
                        isProcessInUse = IsLock(cn, DisplayMsg, GEnum.SysLockOption.ByCodeKeyandDataKey, GEnum.SystemCode.Untrack_SO, 0, 0, 0);
                        if (!isProcessInUse)
                            isProcessInUse = IsLock(cn, DisplayMsg, GEnum.SysLockOption.ByCodeKeyDataKeyAndInprogressKey, GEnum.SystemCode.Sales_Order_Adjustment, 0, -1000, 0);
                        break;
                    case GEnum.SystemCode.Purchase_Order:
                        isProcessInUse = IsLock(cn, DisplayMsg, GEnum.SysLockOption.ByCodeKeyandDataKey, GEnum.SystemCode.Untrack_PO, 0, 0, 0);
                        if (!isProcessInUse)
                            isProcessInUse = IsLock(cn, DisplayMsg, GEnum.SysLockOption.ByCodeKeyDataKeyAndInprogressKey, GEnum.SystemCode.Purchase_Order_Adjustment, 0, -1000, 0);
                        if (!isProcessInUse)
                            isProcessInUse = IsLock(cn, DisplayMsg, GEnum.SysLockOption.ByCodeKeyDataKeyAndInprogressKey, GEnum.SystemCode.Purchase_Plan, 0, -1000, 0);
                        if (!isProcessInUse)
                            isProcessInUse = IsLock(cn, DisplayMsg, GEnum.SysLockOption.ByCodeKeyandDataKey, GEnum.SystemCode.AP_PO_Confirm_Number, 0, 0, 0);
                        break;
                    case GEnum.SystemCode.Order_Consignment:
                        isProcessInUse = IsLock(cn, DisplayMsg, GEnum.SysLockOption.ByCodeKeyandDataKey, GEnum.SystemCode.Untrack_Consignment_Order, 0, 0, 0);
                        if (!isProcessInUse)
                            isProcessInUse = IsLock(cn, DisplayMsg, GEnum.SysLockOption.ByCodeKeyDataKeyAndInprogressKey, GEnum.SystemCode.Consignment_Order_Adjustment, 0, -1000, 0);
                        break;
                    #endregion

                    #region APSJ,APPJ,CSPJ
                    case GEnum.SystemCode.Sales_Order_Adjustment:
                        isProcessInUse = IsLock(cn, DisplayMsg, GEnum.SysLockOption.ByCodeKeyandDataKey, GEnum.SystemCode.Untrack_SO, 0, 0, 0);
                        break;
                    case GEnum.SystemCode.Purchase_Order_Adjustment:
                        isProcessInUse = IsLock(cn, DisplayMsg, GEnum.SysLockOption.ByCodeKeyandDataKey, GEnum.SystemCode.Untrack_PO, 0, 0, 0);
                        break;
                    case GEnum.SystemCode.Consignment_Order_Adjustment:
                        isProcessInUse = IsLock(cn, DisplayMsg, GEnum.SysLockOption.ByCodeKeyandDataKey, GEnum.SystemCode.Untrack_Consignment_Order, 0, 0, 0);
                        break;
                    #endregion

                    #region Untrack ARSO,APPO,CSPO,CSSI
                    case GEnum.SystemCode.Untrack_SO:
                        isProcessInUse = IsLock(cn, DisplayMsg, GEnum.SysLockOption.ByCodeKeyandDataKey, GEnum.SystemCode.Sales_Order, 0, 0, 0);
                        if (!isProcessInUse)
                            isProcessInUse = IsLock(cn, DisplayMsg, GEnum.SysLockOption.ByCodeKeyDataKeyAndInprogressKey, GEnum.SystemCode.Sales_Order_Adjustment, 0, -1000, 0);
                        break;
                    case GEnum.SystemCode.Untrack_PO:
                        isProcessInUse = IsLock(cn, DisplayMsg, GEnum.SysLockOption.ByCodeKeyandDataKey, GEnum.SystemCode.Purchase_Order, 0, 0, 0);
                        if (!isProcessInUse)
                            isProcessInUse = IsLock(cn, DisplayMsg, GEnum.SysLockOption.ByCodeKeyDataKeyAndInprogressKey, GEnum.SystemCode.Purchase_Order_Adjustment, 0, -1000, 0);
                        break;
                    case GEnum.SystemCode.Untrack_Consignment_Order:
                        isProcessInUse = IsLock(cn, DisplayMsg, GEnum.SysLockOption.ByCodeKeyandDataKey, GEnum.SystemCode.Order_Consignment, 0, 0, 0);
                        if (!isProcessInUse)
                            isProcessInUse = IsLock(cn, DisplayMsg, GEnum.SysLockOption.ByCodeKeyDataKeyAndInprogressKey, GEnum.SystemCode.Consignment_Order_Adjustment, 0, -1000, 0);
                        break;
                    case GEnum.SystemCode.Untrack_Issue_Consignment:
                        isProcessInUse = IsLock(cn, DisplayMsg, GEnum.SysLockOption.ByCodeKeyandDataKey, GEnum.SystemCode.Issue_Consignment, 0, 0, 0);
                        if (!isProcessInUse)
                            isProcessInUse = IsLock(cn, DisplayMsg, GEnum.SysLockOption.ByCodeKeyandDataKey, GEnum.SystemCode.Return_Consignment, 0, 0, 0);
                        break;
                    #endregion

                    #region ARDO,APPD,CSSI,CSPD
                    case GEnum.SystemCode.Delivery_Order:
                        isProcessInUse = IsLock(cn, DisplayMsg, GEnum.SysLockOption.ByCodeKeyDataKeyAndInprogressKey, GEnum.SystemCode.Sales_Order_Adjustment, 0, -1000, 0);
                        break;
                    case GEnum.SystemCode.Purchase_Delivery:
                        isProcessInUse = IsLock(cn, DisplayMsg, GEnum.SysLockOption.ByCodeKeyDataKeyAndInprogressKey, GEnum.SystemCode.Purchase_Order_Adjustment, 0, -1000, 0);
                        break;
                    case GEnum.SystemCode.Issue_Consignment:
                    case GEnum.SystemCode.Return_Consignment:
                        isProcessInUse = IsLock(cn, DisplayMsg, GEnum.SysLockOption.ByCodeKeyandDataKey, GEnum.SystemCode.Untrack_Issue_Consignment, 0, 0, 0);
                        break;
                    case GEnum.SystemCode.Received_Consignment:
                        isProcessInUse = IsLock(cn, DisplayMsg, GEnum.SysLockOption.ByCodeKeyDataKeyAndInprogressKey, GEnum.SystemCode.Consignment_Order_Adjustment, 0, -1000, 0);
                        if (!isProcessInUse)
                            isProcessInUse = IsLock(cn, DisplayMsg, GEnum.SysLockOption.ByCodeKeyandDataKey, GEnum.SystemCode.Untrack_Consignment_Order, 0, 0, 0);
                        if (!isProcessInUse)
                            isProcessInUse = IsLock(cn, DisplayMsg, GEnum.SysLockOption.ByCodeKeyandDataKey, GEnum.SystemCode.Consignment_Settlement, 0, 0, 0);
                        break;
                    #endregion

                    #region Transfer DOIV, Consignment Settlement
                    case GEnum.SystemCode.DO_to_IV_Transfer:
                        isProcessInUse = IsLock(cn, DisplayMsg, GEnum.SysLockOption.ByCodeKeyandDataKey, GEnum.SystemCode.DO_to_IV_Transfer, 0, -1000, 0);
                        if (!isProcessInUse)
                            isProcessInUse = IsLock(cn, DisplayMsg, GEnum.SysLockOption.ByCodeKeyDataKeyAndInprogressKey, GEnum.SystemCode.Financial_Charge, 0, -1000, 0);
                        if (!isProcessInUse)
                            isProcessInUse = IsLock(cn, DisplayMsg, GEnum.SysLockOption.ByCodeKeyDataKeyAndInprogressKey, GEnum.SystemCode.ARAP_Revaluation, 0, -1000, 0);
                        break;

                    case GEnum.SystemCode.Consignment_Settlement:
                        isProcessInUse = IsLock(cn, DisplayMsg, GEnum.SysLockOption.ByCodeKeyandDataKey, GEnum.SystemCode.Sales_Invoice, 0, 0, 0);
                        if (!isProcessInUse)
                            isProcessInUse = IsLock(cn, DisplayMsg, GEnum.SysLockOption.ByCodeKeyandDataKey, GEnum.SystemCode.Cash_Sale, 0, 0, 0);
                        if (!isProcessInUse)
                            isProcessInUse = IsLock(cn, DisplayMsg, GEnum.SysLockOption.ByCodeKeyandDataKey, GEnum.SystemCode.Sales_Debit_Note, 0, 0, 0);
                        if (!isProcessInUse)
                            isProcessInUse = IsLock(cn, DisplayMsg, GEnum.SysLockOption.ByCodeKeyandDataKey, GEnum.SystemCode.Cash_Debit_Note, 0, 0, 0);
                        if (!isProcessInUse)
                            isProcessInUse = IsLock(cn, DisplayMsg, GEnum.SysLockOption.ByCodeKeyandDataKey, GEnum.SystemCode.Sales_Credit_Note, 0, 0, 0);
                        if (!isProcessInUse)
                            isProcessInUse = IsLock(cn, DisplayMsg, GEnum.SysLockOption.ByCodeKeyandDataKey, GEnum.SystemCode.Cash_Credit_Note, 0, 0, 0);
                        if (!isProcessInUse)
                            isProcessInUse = IsLock(cn, DisplayMsg, GEnum.SysLockOption.ByCodeKeyandDataKey, GEnum.SystemCode.Received_Consignment, 0, 0, 0);
                        break;
                    #endregion

                    #region Sales Invoice
                    case GEnum.SystemCode.Sales_Invoice:
                        isProcessInUse = IsLock(cn, DisplayMsg, GEnum.SysLockOption.ByCodeKeyandDataKey, GEnum.SystemCode.Consignment_Settlement, 0, 0, 0);
                        if (!isProcessInUse)
                            isProcessInUse = IsLock(cn, DisplayMsg, GEnum.SysLockOption.ByCodeKeyDataKeyAndInprogressKey, GEnum.SystemCode.Sales_Order_Adjustment, 0, -1000, 0);
                        if (!isProcessInUse)
                            isProcessInUse = IsLock(cn, DisplayMsg, GEnum.SysLockOption.ByCodeKeyandDataKey, GEnum.SystemCode.DO_to_IV_Transfer, 0, 0, 0);
                        if (!isProcessInUse)
                            isProcessInUse = IsLock(cn, DisplayMsg, GEnum.SysLockOption.ByCodeKeyDataKeyAndInprogressKey, GEnum.SystemCode.Financial_Charge, 0, -1000, 0);
                        if (!isProcessInUse)
                            isProcessInUse = IsLock(cn, DisplayMsg, GEnum.SysLockOption.ByCodeKeyDataKeyAndInprogressKey, GEnum.SystemCode.ARAP_Revaluation, 0, -1000, 0);
                        break;
                    #endregion

                    #region Cash Sale, Cash DN, Sales DN
                    case GEnum.SystemCode.Cash_Sale:
                    case GEnum.SystemCode.Cash_Debit_Note:
                    case GEnum.SystemCode.Sales_Debit_Note:
                        isProcessInUse = IsLock(cn, DisplayMsg, GEnum.SysLockOption.ByCodeKeyandDataKey, GEnum.SystemCode.Consignment_Settlement, 0, 0, 0);
                        if (!isProcessInUse)
                            isProcessInUse = IsLock(cn, DisplayMsg, GEnum.SysLockOption.ByCodeKeyDataKeyAndInprogressKey, GEnum.SystemCode.Sales_Order_Adjustment, 0, -1000, 0);
                        if (!isProcessInUse)
                            isProcessInUse = IsLock(cn, DisplayMsg, GEnum.SysLockOption.ByCodeKeyDataKeyAndInprogressKey, GEnum.SystemCode.Financial_Charge, 0, -1000, 0);
                        if (!isProcessInUse)
                            isProcessInUse = IsLock(cn, DisplayMsg, GEnum.SysLockOption.ByCodeKeyDataKeyAndInprogressKey, GEnum.SystemCode.ARAP_Revaluation, 0, -1000, 0);
                        break;
                    #endregion

                    #region Purchase Invoice, Purchase DN
                    case GEnum.SystemCode.Purchase_Invoice:
                    case GEnum.SystemCode.Purchase_Debit_Note:
                        isProcessInUse = IsLock(cn, DisplayMsg, GEnum.SysLockOption.ByCodeKeyDataKeyAndInprogressKey, GEnum.SystemCode.Purchase_Order_Adjustment, 0, -1000, 0);
                        if (!isProcessInUse)
                            isProcessInUse = IsLock(cn, DisplayMsg, GEnum.SysLockOption.ByCodeKeyDataKeyAndInprogressKey, GEnum.SystemCode.ARAP_Revaluation, 0, -1000, 0);
                        break;
                    #endregion

                    #region Sales and Cash CN
                    case GEnum.SystemCode.Sales_Credit_Note:
                        isProcessInUse = IsLock(cn, DisplayMsg, GEnum.SysLockOption.ByCodeKeyandDataKey, GEnum.SystemCode.Consignment_Settlement, 0, 0, 0);
                        if (!isProcessInUse)
                            isProcessInUse = IsLock(cn, DisplayMsg, GEnum.SysLockOption.ByCodeKeyDataKeyAndInprogressKey, GEnum.SystemCode.ARAP_Revaluation, 0, -1000, 0);
                        if (!isProcessInUse)
                            isProcessInUse = IsLock(cn, DisplayMsg, GEnum.SysLockOption.ByCodeKeyDataKeyAndInprogressKey, GEnum.SystemCode.Sales_Order_Adjustment, 0, -1000, 0);
                        if (!isProcessInUse)
                            isProcessInUse = IsLock(cn, DisplayMsg, GEnum.SysLockOption.ByCodeKeyDataKeyAndInprogressKey, GEnum.SystemCode.Financial_Charge, 0, -1000, 0);
                         if (!isProcessInUse)
                            isProcessInUse = IsLock(cn, DisplayMsg, GEnum.SysLockOption.ByCodeKeyandDataKey, GEnum.SystemCode.AR_Opening_Balance, 0, 0, 0);
                        break;

                    case GEnum.SystemCode.Cash_Credit_Note:
                        isProcessInUse = IsLock(cn, DisplayMsg, GEnum.SysLockOption.ByCodeKeyandDataKey, GEnum.SystemCode.Consignment_Settlement, 0, 0, 0);
                        if (!isProcessInUse)
                            isProcessInUse = IsLock(cn, DisplayMsg, GEnum.SysLockOption.ByCodeKeyDataKeyAndInprogressKey, GEnum.SystemCode.ARAP_Revaluation, 0, -1000, 0);
                        if (!isProcessInUse)
                            isProcessInUse = IsLock(cn, DisplayMsg, GEnum.SysLockOption.ByCodeKeyDataKeyAndInprogressKey, GEnum.SystemCode.Sales_Order_Adjustment, 0, -1000, 0);
                        if (!isProcessInUse)
                            isProcessInUse = IsLock(cn, DisplayMsg, GEnum.SysLockOption.ByCodeKeyDataKeyAndInprogressKey, GEnum.SystemCode.Financial_Charge, 0, -1000, 0);
                        if (!isProcessInUse)
                            isProcessInUse = IsLock(cn, DisplayMsg, GEnum.SysLockOption.ByCodeKeyandDataKey, GEnum.SystemCode.AR_Cash_Opening_Balance, 0, 0, 0);
                        break;
                    #endregion

                    #region Purchase CN
                    case GEnum.SystemCode.Purchase_Credit_Note:
                        isProcessInUse = IsLock(cn, DisplayMsg, GEnum.SysLockOption.ByCodeKeyDataKeyAndInprogressKey, GEnum.SystemCode.Purchase_Order_Adjustment, 0, -1000, 0);
                        if (!isProcessInUse)
                            isProcessInUse = IsLock(cn, DisplayMsg, GEnum.SysLockOption.ByCodeKeyDataKeyAndInprogressKey, GEnum.SystemCode.ARAP_Revaluation, 0, -1000, 0);
                        if (!isProcessInUse)
                            isProcessInUse = IsLock(cn, DisplayMsg, GEnum.SysLockOption.ByCodeKeyandDataKey, GEnum.SystemCode.AP_Opening_Balance, 0, 0, 0);
                        break;
                    #endregion

                    #region Sales, Cash, Purchase Adjustment
                    case GEnum.SystemCode.Sales_Adjustment:
                    case GEnum.SystemCode.Cash_Adjustment:
                    case GEnum.SystemCode.Purchase_Adjustment:
                        isProcessInUse = IsLock(cn, DisplayMsg, GEnum.SysLockOption.ByCodeKeyDataKeyAndInprogressKey, GEnum.SystemCode.ARAP_Revaluation, 0, -1000, 0);
                        break;
                    #endregion

                    #region Sales, Cash Payment and Contra
                    case GEnum.SystemCode.Contra:
                    case GEnum.SystemCode.Payment_Received:
                        isProcessInUse = IsLock(cn, DisplayMsg, GEnum.SysLockOption.ByCodeKeyandDataKey, GEnum.SystemCode.AR_Opening_Balance, 0, 0, 0);
                        if (!isProcessInUse)
                            isProcessInUse = IsLock(cn, DisplayMsg, GEnum.SysLockOption.ByCodeKeyDataKeyAndInprogressKey, GEnum.SystemCode.ARAP_Revaluation, 0, -1000, 0);
                        if (!isProcessInUse && codeKey == GEnum.SystemCode.DO_to_IV_Transfer)
                            isProcessInUse = IsLock(cn, DisplayMsg, GEnum.SysLockOption.ByCodeKeyDataKeyAndInprogressKey, GEnum.SystemCode.Financial_Charge, 0, -1000, 0);
                        break;

                    case GEnum.SystemCode.Cash_Contra:
                    case GEnum.SystemCode.Cash_Payment_Received:
                        isProcessInUse = IsLock(cn, DisplayMsg, GEnum.SysLockOption.ByCodeKeyandDataKey, GEnum.SystemCode.AR_Cash_Opening_Balance, 0, 0, 0);
                        if (!isProcessInUse)
                            isProcessInUse = IsLock(cn, DisplayMsg, GEnum.SysLockOption.ByCodeKeyDataKeyAndInprogressKey, GEnum.SystemCode.ARAP_Revaluation, 0, -1000, 0);
                        if (!isProcessInUse && codeKey == GEnum.SystemCode.DO_to_IV_Transfer)
                            isProcessInUse = IsLock(cn, DisplayMsg, GEnum.SysLockOption.ByCodeKeyDataKeyAndInprogressKey, GEnum.SystemCode.Financial_Charge, 0, -1000, 0);
                        break;
                    #endregion

                    #region Purchase Payment
                    case GEnum.SystemCode.Payment_Issue:
                        isProcessInUse = IsLock(cn, DisplayMsg, GEnum.SysLockOption.ByCodeKeyandDataKey, GEnum.SystemCode.AP_Opening_Balance, 0, 0, 0);
                        if (!isProcessInUse)
                            isProcessInUse = IsLock(cn, DisplayMsg, GEnum.SysLockOption.ByCodeKeyDataKeyAndInprogressKey, GEnum.SystemCode.ARAP_Revaluation, 0, -1000, 0);
                        break;
                    #endregion

                    #region Customer Opening Balance
                    case GEnum.SystemCode.AR_Opening_Balance:
                        isProcessInUse = IsLock(cn, DisplayMsg, GEnum.SysLockOption.ByCodeKeyandDataKey, GEnum.SystemCode.Sales_Credit_Note, 0, 0, 0);
                        if (!isProcessInUse)
                            isProcessInUse = IsLock(cn, DisplayMsg, GEnum.SysLockOption.ByCodeKeyandDataKey, GEnum.SystemCode.Payment_Received, 0, 0, 0);
                        if (!isProcessInUse)
                            isProcessInUse = IsLock(cn, DisplayMsg, GEnum.SysLockOption.ByCodeKeyandDataKey, GEnum.SystemCode.Contra, 0, 0, 0);
                         if (!isProcessInUse)
                            isProcessInUse = IsLock(cn, DisplayMsg, GEnum.SysLockOption.ByCodeKeyDataKeyAndInprogressKey, GEnum.SystemCode.ARAP_Revaluation, 0, -1000, 0);
                        if (!isProcessInUse && codeKey == GEnum.SystemCode.DO_to_IV_Transfer)
                            isProcessInUse = IsLock(cn, DisplayMsg, GEnum.SysLockOption.ByCodeKeyDataKeyAndInprogressKey, GEnum.SystemCode.Financial_Charge, 0, -1000, 0);
                        break;
                    case GEnum.SystemCode.AR_Cash_Opening_Balance:
                        isProcessInUse = IsLock(cn, DisplayMsg, GEnum.SysLockOption.ByCodeKeyandDataKey, GEnum.SystemCode.Cash_Credit_Note, 0, 0, 0);
                        if (!isProcessInUse)
                            isProcessInUse = IsLock(cn, DisplayMsg, GEnum.SysLockOption.ByCodeKeyandDataKey, GEnum.SystemCode.Cash_Payment_Received, 0, 0, 0);
                        if (!isProcessInUse)
                            isProcessInUse = IsLock(cn, DisplayMsg, GEnum.SysLockOption.ByCodeKeyandDataKey, GEnum.SystemCode.Cash_Contra, 0, 0, 0);
                        if (!isProcessInUse)
                            isProcessInUse = IsLock(cn, DisplayMsg, GEnum.SysLockOption.ByCodeKeyDataKeyAndInprogressKey, GEnum.SystemCode.ARAP_Revaluation, 0, -1000, 0);
                        if (!isProcessInUse && codeKey == GEnum.SystemCode.DO_to_IV_Transfer)
                            isProcessInUse = IsLock(cn, DisplayMsg, GEnum.SysLockOption.ByCodeKeyDataKeyAndInprogressKey, GEnum.SystemCode.Financial_Charge, 0, -1000, 0);
                        break;
                    #endregion

                    #region Vendor Opening Balance
                    case GEnum.SystemCode.AP_Opening_Balance:
                        isProcessInUse = IsLock(cn, DisplayMsg, GEnum.SysLockOption.ByCodeKeyandDataKey, GEnum.SystemCode.Purchase_Credit_Note, 0, 0, 0);
                        if (!isProcessInUse)
                            isProcessInUse = IsLock(cn, DisplayMsg, GEnum.SysLockOption.ByCodeKeyandDataKey, GEnum.SystemCode.Payment_Issue, 0, 0, 0);
                        if (!isProcessInUse)
                            isProcessInUse = IsLock(cn, DisplayMsg, GEnum.SysLockOption.ByCodeKeyandDataKey, GEnum.SystemCode.Contra, 0, 0, 0);
                        if (!isProcessInUse)
                            isProcessInUse = IsLock(cn, DisplayMsg, GEnum.SysLockOption.ByCodeKeyandDataKey, GEnum.SystemCode.Cash_Contra, 0, 0, 0);
                        if (!isProcessInUse)
                            isProcessInUse = IsLock(cn, DisplayMsg, GEnum.SysLockOption.ByCodeKeyDataKeyAndInprogressKey, GEnum.SystemCode.ARAP_Revaluation, 0, -1000, 0);
                        break;
                    #endregion

                    #region Batch COS Posting
                    case GEnum.SystemCode.COSBatchPost:
                        //AR Document
                        isProcessInUse = IsLock(cn, DisplayMsg, GEnum.SysLockOption.ByCodeKeyandDataKey, GEnum.SystemCode.DO_to_IV_Transfer, 0, 0, 0);
                        if (!isProcessInUse)
                            isProcessInUse = IsLock(cn, DisplayMsg, GEnum.SysLockOption.ByCodeKeyandDataKey, GEnum.SystemCode.Sales_Invoice, 0, 0, 0);
                        if (!isProcessInUse)
                            isProcessInUse = IsLock(cn, DisplayMsg, GEnum.SysLockOption.ByCodeKeyandDataKey, GEnum.SystemCode.Sales_Debit_Note, 0, 0, 0);
                        if (!isProcessInUse)
                            isProcessInUse = IsLock(cn, DisplayMsg, GEnum.SysLockOption.ByCodeKeyandDataKey, GEnum.SystemCode.Sales_Credit_Note, 0, 0, 0);
                        if (!isProcessInUse)
                            isProcessInUse = IsLock(cn, DisplayMsg, GEnum.SysLockOption.ByCodeKeyandDataKey, GEnum.SystemCode.Cash_Sale, 0, 0, 0);
                        if (!isProcessInUse)
                            isProcessInUse = IsLock(cn, DisplayMsg, GEnum.SysLockOption.ByCodeKeyandDataKey, GEnum.SystemCode.Cash_Debit_Note, 0, 0, 0);
                        if (!isProcessInUse)
                            isProcessInUse = IsLock(cn, DisplayMsg, GEnum.SysLockOption.ByCodeKeyandDataKey, GEnum.SystemCode.Cash_Credit_Note, 0, 0, 0);

                        //AP Document
                        if (!isProcessInUse)
                            isProcessInUse = IsLock(cn, DisplayMsg, GEnum.SysLockOption.ByCodeKeyandDataKey, GEnum.SystemCode.Purchase_Delivery, 0, 0, 0);
                        if (!isProcessInUse)
                            isProcessInUse = IsLock(cn, DisplayMsg, GEnum.SysLockOption.ByCodeKeyandDataKey, GEnum.SystemCode.Purchase_Invoice, 0, 0, 0);
                        if (!isProcessInUse)
                            isProcessInUse = IsLock(cn, DisplayMsg, GEnum.SysLockOption.ByCodeKeyandDataKey, GEnum.SystemCode.Purchase_Debit_Note, 0, 0, 0);
                        if (!isProcessInUse)
                            isProcessInUse = IsLock(cn, DisplayMsg, GEnum.SysLockOption.ByCodeKeyandDataKey, GEnum.SystemCode.Purchase_Credit_Note, 0, 0, 0);

                        //Inventory Document
                        if (!isProcessInUse)
                            isProcessInUse = IsLock(cn, DisplayMsg, GEnum.SysLockOption.ByCodeKeyandDataKey, GEnum.SystemCode.Inventory_Adjustment, 0, 0, 0);
                        if (!isProcessInUse)
                            isProcessInUse = IsLock(cn, DisplayMsg, GEnum.SysLockOption.ByCodeKeyandDataKey, GEnum.SystemCode.Inventory_Production, 0, 0, 0);
                        if (!isProcessInUse)
                            isProcessInUse = IsLock(cn, DisplayMsg, GEnum.SysLockOption.ByCodeKeyandDataKey, GEnum.SystemCode.Inventory_Transfer, 0, 0, 0);
                        if (!isProcessInUse)
                            isProcessInUse = IsLock(cn, DisplayMsg, GEnum.SysLockOption.ByCodeKeyandDataKey, GEnum.SystemCode.Issue_Consignment, 0, 0, 0);
                        if (!isProcessInUse)
                            isProcessInUse = IsLock(cn, DisplayMsg, GEnum.SysLockOption.ByCodeKeyandDataKey, GEnum.SystemCode.Return_Consignment, 0, 0, 0);
                        if (!isProcessInUse)
                            isProcessInUse = IsLock(cn, DisplayMsg, GEnum.SysLockOption.ByCodeKeyandDataKey, GEnum.SystemCode.Inventory_Opening_Balance, 0, 0, 0);
                        if (!isProcessInUse)
                            isProcessInUse = IsLock(cn, DisplayMsg, GEnum.SysLockOption.ByCodeKeyandDataKey, GEnum.SystemCode.Stock_Count, 0, 0, 0);
                        break;
                    #endregion

                    #region Account Process
                    case GEnum.SystemCode.Account:
                        isProcessInUse = IsLock(cn, DisplayMsg, GEnum.SysLockOption.ByCodeKeyandDataKey, GEnum.SystemCode.Account_Opening_Balance, 0, 0, 0);
                        if (!isProcessInUse)
                            isProcessInUse = IsLock(cn, DisplayMsg, GEnum.SysLockOption.ByCodeKeyandDataKey, GEnum.SystemCode.Bank_Reconcilation, 0, 0, 0);
                        break;
                    case GEnum.SystemCode.Account_Opening_Balance:
                        isProcessInUse = IsLock(cn, DisplayMsg, GEnum.SysLockOption.ByCodeKeyandDataKey, GEnum.SystemCode.Account, 0, 0, 0);
                        break;
                    case GEnum.SystemCode.Account_Unreconciled_Trans:
                        isProcessInUse = IsLock(cn, DisplayMsg, GEnum.SysLockOption.ByCodeKeyandDataKey, GEnum.SystemCode.Bank_Reconcilation, 0, 0, 0);
                        break;
                    case GEnum.SystemCode.Bank_Reconcilation:
                        isProcessInUse = IsLock(cn, DisplayMsg, GEnum.SysLockOption.ByCodeKeyandDataKey, GEnum.SystemCode.Account_Opening_Balance, 0, 0, 0);
                        if (!isProcessInUse)
                            isProcessInUse = IsLock(cn, DisplayMsg, GEnum.SysLockOption.ByCodeKeyandDataKey, GEnum.SystemCode.Account_Unreconciled_Trans, 0, 0, 0);
                        break;
                    #endregion

                    #region ARAP Revaluation
                    case GEnum.SystemCode.ARAP_Revaluation:
                        isProcessInUse = IsLock(cn, DisplayMsg, GEnum.SysLockOption.ByCodeKeyandDataKey, GEnum.SystemCode.Sales_Invoice, 0, 0, 0);
                        //Sales
                        if (!isProcessInUse)
                            isProcessInUse = IsLock(cn, DisplayMsg, GEnum.SysLockOption.ByCodeKeyandDataKey, GEnum.SystemCode.Sales_Debit_Note, 0, 0, 0);
                        if (!isProcessInUse)
                            isProcessInUse = IsLock(cn, DisplayMsg, GEnum.SysLockOption.ByCodeKeyandDataKey, GEnum.SystemCode.Sales_Credit_Note, 0, 0, 0);
                        if (!isProcessInUse)
                            isProcessInUse = IsLock(cn, DisplayMsg, GEnum.SysLockOption.ByCodeKeyandDataKey, GEnum.SystemCode.Sales_Adjustment, 0, 0, 0);
                        if (!isProcessInUse)
                            isProcessInUse = IsLock(cn, DisplayMsg, GEnum.SysLockOption.ByCodeKeyandDataKey, GEnum.SystemCode.Payment_Received, 0, 0, 0);
                        if (!isProcessInUse)
                            isProcessInUse = IsLock(cn, DisplayMsg, GEnum.SysLockOption.ByCodeKeyandDataKey, GEnum.SystemCode.Contra, 0, 0, 0);
                        if (!isProcessInUse)
                            isProcessInUse = IsLock(cn, DisplayMsg, GEnum.SysLockOption.ByCodeKeyandDataKey, GEnum.SystemCode.AR_Opening_Balance, 0, 0, 0);
                        //Cash Sales
                        if (!isProcessInUse)
                            isProcessInUse = IsLock(cn, DisplayMsg, GEnum.SysLockOption.ByCodeKeyandDataKey, GEnum.SystemCode.Cash_Sale, 0, 0, 0);
                        if (!isProcessInUse)
                            isProcessInUse = IsLock(cn, DisplayMsg, GEnum.SysLockOption.ByCodeKeyandDataKey, GEnum.SystemCode.Cash_Debit_Note, 0, 0, 0);
                        if (!isProcessInUse)
                            isProcessInUse = IsLock(cn, DisplayMsg, GEnum.SysLockOption.ByCodeKeyandDataKey, GEnum.SystemCode.Cash_Credit_Note, 0, 0, 0);
                        if (!isProcessInUse)
                            isProcessInUse = IsLock(cn, DisplayMsg, GEnum.SysLockOption.ByCodeKeyandDataKey, GEnum.SystemCode.Cash_Adjustment, 0, 0, 0);
                        if (!isProcessInUse)
                            isProcessInUse = IsLock(cn, DisplayMsg, GEnum.SysLockOption.ByCodeKeyandDataKey, GEnum.SystemCode.Cash_Payment_Received, 0, 0, 0);
                        if (!isProcessInUse)
                            isProcessInUse = IsLock(cn, DisplayMsg, GEnum.SysLockOption.ByCodeKeyandDataKey, GEnum.SystemCode.Cash_Contra, 0, 0, 0);
                        if (!isProcessInUse)
                            isProcessInUse = IsLock(cn, DisplayMsg, GEnum.SysLockOption.ByCodeKeyandDataKey, GEnum.SystemCode.AR_Cash_Opening_Balance, 0, 0, 0);
                        //Purchase
                        if (!isProcessInUse)
                            isProcessInUse = IsLock(cn, DisplayMsg, GEnum.SysLockOption.ByCodeKeyandDataKey, GEnum.SystemCode.Purchase_Invoice, 0, 0, 0);
                        if (!isProcessInUse)
                            isProcessInUse = IsLock(cn, DisplayMsg, GEnum.SysLockOption.ByCodeKeyandDataKey, GEnum.SystemCode.Purchase_Debit_Note, 0, 0, 0);
                        if (!isProcessInUse)
                            isProcessInUse = IsLock(cn, DisplayMsg, GEnum.SysLockOption.ByCodeKeyandDataKey, GEnum.SystemCode.Purchase_Credit_Note, 0, 0, 0);
                        if (!isProcessInUse)
                            isProcessInUse = IsLock(cn, DisplayMsg, GEnum.SysLockOption.ByCodeKeyandDataKey, GEnum.SystemCode.Purchase_Adjustment, 0, 0, 0);
                        if (!isProcessInUse)
                            isProcessInUse = IsLock(cn, DisplayMsg, GEnum.SysLockOption.ByCodeKeyandDataKey, GEnum.SystemCode.Payment_Issue, 0, 0, 0);
                        if (!isProcessInUse)
                            isProcessInUse = IsLock(cn, DisplayMsg, GEnum.SysLockOption.ByCodeKeyandDataKey, GEnum.SystemCode.AP_Opening_Balance, 0, 0, 0);
                        break;
                    #endregion

                    #region Financial Charge
                    case GEnum.SystemCode.Financial_Charge:
                        isProcessInUse = IsLock(cn, DisplayMsg, GEnum.SysLockOption.ByCodeKeyandDataKey, GEnum.SystemCode.Sales_Invoice, 0, 0, 0);
                        if (!isProcessInUse)
                            isProcessInUse = IsLock(cn, DisplayMsg, GEnum.SysLockOption.ByCodeKeyandDataKey, GEnum.SystemCode.Sales_Debit_Note, 0, 0, 0);
                        if (!isProcessInUse)
                            isProcessInUse = IsLock(cn, DisplayMsg, GEnum.SysLockOption.ByCodeKeyandDataKey, GEnum.SystemCode.Sales_Credit_Note, 0, 0, 0);
                        if (!isProcessInUse)
                            isProcessInUse = IsLock(cn, DisplayMsg, GEnum.SysLockOption.ByCodeKeyandDataKey, GEnum.SystemCode.Cash_Sale, 0, 0, 0);
                        if (!isProcessInUse)
                            isProcessInUse = IsLock(cn, DisplayMsg, GEnum.SysLockOption.ByCodeKeyandDataKey, GEnum.SystemCode.Cash_Debit_Note, 0, 0, 0);
                        if (!isProcessInUse)
                            isProcessInUse = IsLock(cn, DisplayMsg, GEnum.SysLockOption.ByCodeKeyandDataKey, GEnum.SystemCode.Cash_Credit_Note, 0, 0, 0);
                        if (!isProcessInUse)
                            isProcessInUse = IsLock(cn, DisplayMsg, GEnum.SysLockOption.ByCodeKeyDataKeyAndInprogressKey, GEnum.SystemCode.ARAP_Revaluation, 0, -1000, 0);
                        if (!isProcessInUse)
                            isProcessInUse = IsLock(cn, DisplayMsg, GEnum.SysLockOption.ByCodeKeyandDataKey, GEnum.SystemCode.DO_to_IV_Transfer, 0, 0, 0);
                        break;
                    #endregion
                }

                return isProcessInUse;
            }
            catch (TAException taex)
            {
                throw Error(taex);
            }
            catch (Exception ex)
            {
                throw Error(ex);
            }
        }

        public static SYSLocks GetSysLocks(GEnum.SystemCode codeKey, int GUID, int DataKey, int InProgressKey, int UserKey, int Option)
        {
            try
            {
                SqlConnection cn = new SqlConnection(Database.BossDemoConnection);
                cn.Open();

                return SYSLocks.Get(cn, new SYSLocks.Criteria(GFunc.NEInt(codeKey, 0), DataKey, InProgressKey, GUID, UserKey, Option));
            }
            catch (TAException taex)
            {
                throw Error(taex);
            }
            catch (Exception ex)
            {
                throw Error(ex);
            }
        }

        public static SYSLocks GetSysLocks(SqlConnection cn, GEnum.SystemCode codeKey, int GUID, int DataKey, int InProgressKey, int UserKey, int Option)
        {
            try
            {
                return SYSLocks.Get(cn,new SYSLocks.Criteria(GFunc.NEInt(codeKey,0),DataKey,InProgressKey,GUID,UserKey,Option));
            }
            catch (TAException taex)
            {
                throw Error(taex);
            }
            catch (Exception ex)
            {
                throw Error(ex);
            }
        }
        //Error Exceptions
        private static Exception Error(Exception ex)
        {
            try
            {
                ex = SysAuditLogUtility.ModifyException(ex, false, new object[] { });
            }
            catch (Exception nex)
            {
                MsgBox.Show(nex.Message);
            }
            return ex;
        }
        private static TAException Error(TAException ex)
        {
            try
            {
                ex = SysAuditLogUtility.ModifyTAException(ex, false, new object[] { });
            }
            catch (Exception nex)
            {
                MsgBox.Show(nex.Message);
            }
            return ex;
        }
    }
}
