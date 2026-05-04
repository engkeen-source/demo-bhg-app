using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.SqlClient;

namespace BOLib
{
    public class SECPermUtility
    {
        private static SECUserPermissionVw _SECUserPermissionVw;
        private static SECUserPermissionVws _SECUserPermissionVws;

        public static bool LoadAllUserPermission(out string msgID)
        {
            try
            {
                bool isLoadAllUserPermission = false;

                _SECUserPermissionVws = SECUserPermissionVws.Get(new SECUserPermissionVws.Criteria(AppInfor.securityKey, string.Empty, 0));

                if (_SECUserPermissionVws.Count > 0)
                {
                    msgID = string.Empty;
                    isLoadAllUserPermission = true;
                }
                else
                    msgID = MsgID.Permission.LoadAllUserPermissionFail;

                return isLoadAllUserPermission;
            }
            catch (Exception ex)
            {
                throw Error(ex);
            }
        }

        public static bool Perform(string permID, bool DisplayMsg)
        {
            try
            {                
                using (System.Data.SqlClient.SqlConnection cn = new System.Data.SqlClient.SqlConnection(Database.BossDemoConnection))
                {
                    cn.Open();
                    return Perform(cn, permID, DisplayMsg);
                }                

            }
            catch (Exception ex)
            {
                throw Error(ex);
            }
        }

        public static bool Perform(SqlConnection cn, string permID, bool DisplayMsg)
        {
            string msgID = string.Empty;
            try
            {

                // Fetch data from User Permission
                _SECUserPermissionVw = SECUserPermissionVw.Get(cn, AppInfor.securityKey, permID);


                // Have Perform permission
                if (GFunc.IsNE(_SECUserPermissionVw.CanPerform) ? false : (bool)_SECUserPermissionVw.CanPerform)
                    return true;
                else
                {
                    if (DisplayMsg)
                        MsgBox.Show(cn, MsgID.Permission.PermPerformIsFalse);

                    return false;
                }
            }
            catch (Exception ex)
            {
                throw Error(ex);
            }
        }

        public static bool Read(string permID, bool DisplayMsg)
        {
            try
            {
                using (System.Data.SqlClient.SqlConnection cn = new System.Data.SqlClient.SqlConnection(Database.BossDemoConnection))
                {
                    cn.Open();
                    return Read(cn, permID, DisplayMsg);
                }

            }
            catch (Exception ex)
            {
                throw Error(ex);
            }
        }

        public static bool Read(SqlConnection cn, string permID, bool DisplayMsg)
        {
            string msgID = string.Empty;
            try
            {

                // Fetch data from User Permission
                _SECUserPermissionVw = SECUserPermissionVw.Get(cn, AppInfor.securityKey, permID);



                // Have Read permission
                if (GFunc.IsNE(_SECUserPermissionVw.CanRead) ? false : (bool)_SECUserPermissionVw.CanRead)
                    return true;
                else
                {
                    if (DisplayMsg)
                        MsgBox.Show(cn, MsgID.Permission.PermReadIsFalse);

                    return false;
                }
            }
            catch (Exception ex)
            {
                throw Error(ex);
            }
        }


        public static bool Edit(string permID, bool DisplayMsg)
        {
            try
            {
                using (System.Data.SqlClient.SqlConnection cn = new System.Data.SqlClient.SqlConnection(Database.BossDemoConnection))
                {
                    cn.Open();
                    return Edit(cn, permID, DisplayMsg);
                }

            }
            catch (Exception ex)
            {
                throw Error(ex);
            }
        }
        public static bool Edit(SqlConnection cn, string permID, bool DisplayMsg)
        {
            try
            {

                // Fetch data from User Permission
                _SECUserPermissionVw = SECUserPermissionVw.Get(cn, AppInfor.securityKey, permID);

                // Have Edit permission
                if (GFunc.IsNE(_SECUserPermissionVw.CanEdit) ? false : (bool)_SECUserPermissionVw.CanEdit)
                    return true;
                else
                {
                    if (DisplayMsg)
                        MsgBox.Show(cn, MsgID.Permission.PermEditIsFalse);

                    return false;
                }
            }
            catch (Exception ex)
            {
                throw Error(ex);
            }
        }
        public static bool Add(string permID, bool DisplayMsg)
        {
            try
            {
                using (System.Data.SqlClient.SqlConnection cn = new System.Data.SqlClient.SqlConnection(Database.BossDemoConnection))
                {
                    cn.Open();
                    return Add(cn, permID, DisplayMsg);
                }

            }
            catch (Exception ex)
            {
                throw Error(ex);
            }
        }
        public static bool Add(SqlConnection cn, string permID, bool DisplayMsg)
        {
            try
            {

                // Fetch data from User Permission
                _SECUserPermissionVw = SECUserPermissionVw.Get(cn, AppInfor.securityKey, permID);

                // Have add permission
                if (GFunc.IsNE(_SECUserPermissionVw.CanAdd) ? false : (bool)_SECUserPermissionVw.CanAdd)
                    return true;
                else
                {
                    if (DisplayMsg)
                        MsgBox.Show(cn, MsgID.Permission.PermAddIsFalse);

                    return false;
                }
            }
            catch (Exception ex)
            {
                throw Error(ex);
            }
        }
        public static bool Delete(string permID, bool DisplayMsg)
        {
            try
            {
                using (System.Data.SqlClient.SqlConnection cn = new System.Data.SqlClient.SqlConnection(Database.BossDemoConnection))
                {
                    cn.Open();
                    return Delete(cn, permID, DisplayMsg);
                }

            }
            catch (Exception ex)
            {
                throw Error(ex);
            }
        }
        public static bool Delete(SqlConnection cn, string permID, bool DisplayMsg)
        {
            try
            {

                string msgID = string.Empty;

                // Fetch data from User Permission
                _SECUserPermissionVw = SECUserPermissionVw.Get(cn, AppInfor.securityKey, permID);



                // Have Delete permission
                if (GFunc.IsNE(_SECUserPermissionVw.CanDelete) ? false : (bool)_SECUserPermissionVw.CanDelete)
                    return true;
                else
                {
                    if (DisplayMsg)
                        MsgBox.Show(cn, MsgID.Permission.PermDeletesFalse);

                    return false;
                }
            }
            catch (Exception ex)
            {
                throw Error(ex);
            }
        }
        public static bool Any(string permID, out bool isReadOnly, bool DisplayMsg)
        {
            try
            {
                using (System.Data.SqlClient.SqlConnection cn = new System.Data.SqlClient.SqlConnection(Database.BossDemoConnection))
                {
                    cn.Open();
                    return Any(cn, permID, out isReadOnly, DisplayMsg);
                }

            }
            catch (Exception ex)
            {
                throw Error(ex);
            }
        }
        public static bool Any(SqlConnection cn, string permID, out bool isReadOnly, bool DisplayMsg)
        {
            try
            {

                isReadOnly = true;
                string msgID = string.Empty;

                //Retrieve Permissions (CanList, CanRead,CanPerform,CanEdit,CanAdd,CanDelete) from the database by passing through Login UserKey,App Instance SecurityKey and Permission ID
                _SECUserPermissionVw = SECUserPermissionVw.Get(cn, AppInfor.securityKey, permID);

                //Albert(12-Oct-2010 Found Error IN APRQ)
               // SECPerm SecPer = SECPerm.Get(cn,permID);
                //End Albert.

                // Unable to retrive permission
                if (GFunc.IsNE(msgID) == false || GFunc.IsNE(_SECUserPermissionVw._securityKey))
                {
                    if (DisplayMsg)
                        MsgBox.Show(cn,MsgID.Permission.PermPerformIsFalse);

                    isReadOnly = false;
                    return false;
                }

                // Have any permission
                bool canPerform = GFunc.IsNE(_SECUserPermissionVw.CanPerform) ? false : (bool)_SECUserPermissionVw.CanPerform;
                bool canRead = GFunc.IsNE(_SECUserPermissionVw.CanRead) ? false : (bool)_SECUserPermissionVw.CanRead;
                bool canEdit = GFunc.IsNE(_SECUserPermissionVw.CanEdit) ? false : (bool)_SECUserPermissionVw.CanEdit;
                bool canAdd = GFunc.IsNE(_SECUserPermissionVw.CanAdd) ? false : (bool)_SECUserPermissionVw.CanAdd;
                bool canDelete = GFunc.IsNE(_SECUserPermissionVw.CanDelete) ? false : (bool)_SECUserPermissionVw.CanDelete;

                int intcanPerform = (canPerform) ? 2 : 0;
                int intcanRead = (canRead) ? 4 : 0;
                int intcanEdit = (canEdit) ? 8 : 0;
                int intcanAdd = (canAdd) ? 16 : 0;
                int intcanDelete = (canDelete) ? 32 : 0;

                //Old Code
                //if ((canPerform) || ((canEdit) && (canAdd) && (canDelete)))
                //    isReadOnly = false;
                //else
                //    isReadOnly = true;

                if (_SECUserPermissionVw._permType > 10 && intcanPerform + intcanRead+ intcanEdit + intcanAdd + intcanDelete == 6)
                    isReadOnly = true;
                else
                    isReadOnly = false;

                if (canPerform || canRead || canEdit || canAdd || canDelete)
                    return true;
                else
                {
                    if (DisplayMsg)
                        MsgBox.Show(cn, MsgID.Permission.PermAnyIsFalse);

                    return false;
                }
            }
            catch (Exception ex)
            {
                throw Error(ex);
            }
        }
        public static bool List(string permID, bool DisplayMsg)
        {
            try
            {
                using (System.Data.SqlClient.SqlConnection cn = new System.Data.SqlClient.SqlConnection(Database.BossDemoConnection))
                {
                    cn.Open();
                    return List(cn, permID, DisplayMsg);
                }

            }
            catch (Exception ex)
            {
                throw Error(ex);
            }
        }
        public static bool List(SqlConnection cn, string permID, bool DisplayMsg)
        {
            try
            {

                string msgID = string.Empty;

                // Fetch data from User Permission
                _SECUserPermissionVw = SECUserPermissionVw.Get(cn, AppInfor.securityKey, permID);


                // Have List permission
                if (GFunc.IsNE(_SECUserPermissionVw.CanList) ? false : (bool)_SECUserPermissionVw.CanList)
                    return true;
                else
                {
                    if (DisplayMsg)
                        MsgBox.Show(cn, MsgID.Permission.PermListIsFalse);

                    return false;
                }
            }
            catch (Exception ex)
            {
                throw Error(ex);
            }
        }

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
    }
}
