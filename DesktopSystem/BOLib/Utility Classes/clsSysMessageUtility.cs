using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Transactions;
using TAUtil;

namespace BOLib
{
    public class SysMessageUtility
    {
        #region public functions

        /// <summary>
        /// Retrive Message Value with parameter MsgID
        /// </summary>
        /// <param name="MsgID"></param>    
        /// <returns>string, Message Vaule </returns>
        public static string Get(string MsgID)
        {
            string msgValue = string.Empty;

            try
            {
                using (TransactionScope scope = new TransactionScope())
                {
                    using (System.Data.SqlClient.SqlConnection cn = new System.Data.SqlClient.SqlConnection(Database.BossDemoConnection))
                    {
                        cn.Open();
                        msgValue= Get(cn, MsgID);
                    }
                      if (Transaction.Current.TransactionInformation.Status != TransactionStatus.Active)  throw new Exception("Transaction has aborted."); scope.Complete();
                }

            }
            catch (Exception ex)
            {
                throw Error(ex);
            }
            return msgValue;
        }
        public static string Get(string MsgID,ref string MsgTitle)
        {
            string msgValue = string.Empty;

            try
            {
                using (TransactionScope scope = new TransactionScope())
                {
                    using (System.Data.SqlClient.SqlConnection cn = new System.Data.SqlClient.SqlConnection(Database.BossDemoConnection))
                    {
                        cn.Open();
                        msgValue = Get(cn, MsgID, ref MsgTitle);
                    }
                      if (Transaction.Current.TransactionInformation.Status != TransactionStatus.Active)  throw new Exception("Transaction has aborted."); scope.Complete();
                }

            }
            catch (Exception ex)
            {
                throw Error(ex);
            }
            return msgValue;
        }
        /// <summary>
        /// Retrive Message Value with parameter MsgID
        /// </summary>
        /// <param name="MsgID"></param>      
        /// <returns>string, Message Vaule</returns>
        public static string Get(System.Data.SqlClient.SqlConnection cn, string MsgID)
        {

            try
            {
                string vMsgStr = string.Empty;
                //string vOpValue = string.Empty;
                string vMsgTitle = string.Empty;
                bool processOk;
                SYSMsgApp objSYSMsgApp;

                if (MsgID.Contains(' ') && (!MsgID.Contains("%")))
                    return MsgID;

                string[] vMsgParameter = MsgID.Split('%');
                /*  MsgID="ScaleSizeIsEmpty%10"
                    vMsgParameter={"ScaleSizeIsEmpty","10"}
                    After split, msgID will be at index 0. The rest are parameters.
                */
                if (vMsgParameter.Length > 0)
                {
                    //Use index 0 to retrieve msg value.
                    objSYSMsgApp = SYSMsgApp.New();
                    processOk = objSYSMsgApp.Fetch(cn, new SYSMsgApp.Criteria(vMsgParameter[0], 1));

                    //In Database Table, msgID->"ScaleSizeIsEmpty", msgValue->"Scale Size {0} cannot be empty"
                    if (processOk)
                    {
                        if (SysOptionUtility.LanguageUsed.ToString() != string.Empty)
                        {
                            switch (SysOptionUtility.LanguageUsed.ToString())
                            {
                                case "1": vMsgStr = objSYSMsgApp.MsgText1;
                                    vMsgTitle = objSYSMsgApp.MsgTitle1;
                                    break;
                                case "2": vMsgStr = objSYSMsgApp.MsgText2;
                                    vMsgTitle = objSYSMsgApp.MsgTitle2;
                                    break;
                                case "3": vMsgStr = objSYSMsgApp.MsgText3;
                                    vMsgTitle = objSYSMsgApp.MsgTitle3;
                                    break;
                                case "4": vMsgStr = objSYSMsgApp.MsgText4;
                                    vMsgTitle = objSYSMsgApp.MsgTitle4;
                                    break;
                                case "5": vMsgStr = objSYSMsgApp.MsgText5;
                                    vMsgTitle = objSYSMsgApp.MsgTitle5;
                                    break;
                                case "6": vMsgStr = objSYSMsgApp.MsgText6;
                                    vMsgTitle = objSYSMsgApp.MsgTitle6;
                                    break;
                                case "7": vMsgStr = objSYSMsgApp.MsgText7;
                                    vMsgTitle = objSYSMsgApp.MsgTitle7;
                                    break;
                                case "8": vMsgStr = objSYSMsgApp.MsgText8;
                                    vMsgTitle = objSYSMsgApp.MsgTitle8;
                                    break;
                                case "9": vMsgStr = objSYSMsgApp.MsgText9;
                                    vMsgTitle = objSYSMsgApp.MsgTitle9;
                                    break;
                                case "10": vMsgStr = objSYSMsgApp.MsgText10;
                                    vMsgTitle = objSYSMsgApp.MsgTitle10;
                                    break;
                            }
                            //Replace  msgValue->"Scale Size {0} cannot be empty" to msgValue-> "Scale Size 10 cannot be empty"
                            for (int i = 1; i < vMsgParameter.Length; i++)
                            {
                                string oldVal = "{" + (i - 1) + "}";
                                vMsgStr = vMsgStr.Replace(oldVal, vMsgParameter[i]);
                            }
                            //if Msg Value can be found in database, vMsgStr's length will be greater than 0
                            if (vMsgStr.Trim().Length > 0)
                                return vMsgStr;
                        }
                    }
                }
                //if Msg Value cannot be found in database, return original MsgID instead of blank
                return MsgID;
            }
            catch (Exception ex)
            {
                throw Error(ex);
            }
        }
        public static string Get(System.Data.SqlClient.SqlConnection cn, string MsgID,ref string msgTitle)
        {

            try
            {
                string vMsgStr = string.Empty;
                //string vOpValue = string.Empty;
                string vMsgTitle = string.Empty;
                bool processOk;
                SYSMsgApp objSYSMsgApp;

                if (MsgID.Contains(' ') && (!MsgID.Contains("%")))
                    return MsgID;

                string[] vMsgParameter = MsgID.Split('%');
                /*  MsgID="ScaleSizeIsEmpty%10"
                    vMsgParameter={"ScaleSizeIsEmpty","10"}
                    After split, msgID will be at index 0. The rest are parameters.
                */
                if (vMsgParameter.Length > 0)
                {
                    //Use index 0 to retrieve msg value.
                    objSYSMsgApp = SYSMsgApp.New();
                    processOk = objSYSMsgApp.Fetch(cn, new SYSMsgApp.Criteria(vMsgParameter[0], 1));

                    //In Database Table, msgID->"ScaleSizeIsEmpty", msgValue->"Scale Size {0} cannot be empty"
                    if (processOk)
                    {
                        if (SysOptionUtility.LanguageUsed.ToString() != string.Empty)
                        {
                            switch (SysOptionUtility.LanguageUsed.ToString())
                            {
                                case "1": vMsgStr = objSYSMsgApp.MsgText1;
                                    vMsgTitle = objSYSMsgApp.MsgTitle1;
                                    break;
                                case "2": vMsgStr = objSYSMsgApp.MsgText2;
                                    vMsgTitle = objSYSMsgApp.MsgTitle2;
                                    break;
                                case "3": vMsgStr = objSYSMsgApp.MsgText3;
                                    vMsgTitle = objSYSMsgApp.MsgTitle3;
                                    break;
                                case "4": vMsgStr = objSYSMsgApp.MsgText4;
                                    vMsgTitle = objSYSMsgApp.MsgTitle4;
                                    break;
                                case "5": vMsgStr = objSYSMsgApp.MsgText5;
                                    vMsgTitle = objSYSMsgApp.MsgTitle5;
                                    break;
                                case "6": vMsgStr = objSYSMsgApp.MsgText6;
                                    vMsgTitle = objSYSMsgApp.MsgTitle6;
                                    break;
                                case "7": vMsgStr = objSYSMsgApp.MsgText7;
                                    vMsgTitle = objSYSMsgApp.MsgTitle7;
                                    break;
                                case "8": vMsgStr = objSYSMsgApp.MsgText8;
                                    vMsgTitle = objSYSMsgApp.MsgTitle8;
                                    break;
                                case "9": vMsgStr = objSYSMsgApp.MsgText9;
                                    vMsgTitle = objSYSMsgApp.MsgTitle9;
                                    break;
                                case "10": vMsgStr = objSYSMsgApp.MsgText10;
                                    vMsgTitle = objSYSMsgApp.MsgTitle10;
                                    break;
                            }
                            //Replace  msgValue->"Scale Size {0} cannot be empty" to msgValue-> "Scale Size 10 cannot be empty"
                            for (int i = 1; i < vMsgParameter.Length; i++)
                            {
                                string oldVal = "{" + (i - 1) + "}";
                                vMsgStr = vMsgStr.Replace(oldVal, vMsgParameter[i]);
                            }
                            //if Msg Value can be found in database, vMsgStr's length will be greater than 0
                            if (vMsgStr.Trim().Length > 0)
                            {
                                msgTitle = vMsgTitle;
                                return vMsgStr;
                            }
                        }
                    }
                }
                //if Msg Value cannot be found in database, return original MsgID instead of blank
                
                return MsgID;
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
        /// <summary>
        /// Retrive Message Value with parameter MsgID
        /// </summary>
        /// <param name="MsgID"></param>
        /// <param name="MsgValue"></param>
        /// <param name="MsgTitle"></param>
        /// <returns></returns>
        //public static bool Get(string MsgID, out string MsgValue, out string MsgTitle)
        //{
        //    MsgValue = "ERROR: Cannot locate Message for display";

        //    string vMsgStr = string.Empty;
        //    string vOpValue = string.Empty;
        //    string vMsgTitle = string.Empty;
        //    MsgTitle = string.Empty;
        //    SYSMsgApp objSYSMsgApp;

        //    string[] vMsgParameter = MsgID.Split('%');
        //    /*  MsgID="ScaleSizeIsEmpty%10"
        //        vMsgParameter={"ScaleSizeIsEmpty","10"}
        //    */
        //    //After split, msgID will be at index 0. The rest are parameters.

        //    if (vMsgParameter.Length > 0)
        //    {
        //        //Use index 0 to retrieve msg value.
        //        objSYSMsgApp = SYSMsgApp.Get(vMsgParameter[0]);

        //        //In Database Table, msgID->"ScaleSizeIsEmpty", msgValue->"Scale Size {0} cannot be empty"

        //        MsgValue = "ERROR: Cannot determine System Language";

        //        if (SysOptionUtility.LanguageOption != string.Empty)
        //        {
        //            switch (SysOptionUtility.LanguageOption)
        //            {
        //                case "1": vMsgStr = objSYSMsgApp.MsgText1;
        //                    vMsgTitle = objSYSMsgApp.MsgTitle1;
        //                    break;
        //                case "2": vMsgStr = objSYSMsgApp.MsgText2;
        //                    vMsgTitle = objSYSMsgApp.MsgTitle2;
        //                    break;
        //                case "3": vMsgStr = objSYSMsgApp.MsgText3;
        //                    vMsgTitle = objSYSMsgApp.MsgTitle3;
        //                    break;
        //                case "4": vMsgStr = objSYSMsgApp.MsgText4;
        //                    vMsgTitle = objSYSMsgApp.MsgTitle4;
        //                    break;
        //                case "5": vMsgStr = objSYSMsgApp.MsgText5;
        //                    vMsgTitle = objSYSMsgApp.MsgTitle5;
        //                    break;
        //                case "6": vMsgStr = objSYSMsgApp.MsgText6;
        //                    vMsgTitle = objSYSMsgApp.MsgTitle6;
        //                    break;
        //                case "7": vMsgStr = objSYSMsgApp.MsgText7;
        //                    vMsgTitle = objSYSMsgApp.MsgTitle7;
        //                    break;
        //                case "8": vMsgStr = objSYSMsgApp.MsgText8;
        //                    vMsgTitle = objSYSMsgApp.MsgTitle8;
        //                    break;
        //                case "9": vMsgStr = objSYSMsgApp.MsgText9;
        //                    vMsgTitle = objSYSMsgApp.MsgTitle9;
        //                    break;
        //                case "10": vMsgStr = objSYSMsgApp.MsgText10;
        //                    vMsgTitle = objSYSMsgApp.MsgTitle10;
        //                    break;
        //            }
        //            //Replace  msgValue->"Scale Size {0} cannot be empty" to msgValue-> "Scale Size 10 cannot be empty"

        //            for (int i = 1; i < vMsgParameter.Length; i++)
        //            {
        //                string oldVal = "{" + (i-1) + "}";
        //                vMsgStr= vMsgStr.Replace(oldVal, vMsgParameter[i]);
        //            }
        //            MsgValue = vMsgStr;
        //            MsgTitle = vMsgTitle;
        //            return true;
        //        }
        //    }
        //    return false;
        //}
        #endregion

        ///// <summary>
        ///// Retrive Message Value with parameter MsgID
        ///// </summary>
        ///// <param name="MsgID"></param>
        ///// <param name="MsgValue">output</param>
        ///// <returns></returns>
        //public static bool Get(string MsgID, out string MsgValue)
        //{
        //    MsgValue = "ERROR: Cannot locate Message for display";

        //    string vMsgStr = string.Empty;
        //    string vOpValue = string.Empty;
        //    string vMsgTitle = string.Empty;
        //    SYSMsgApp objSYSMsgApp;


        //    string[] vMsgParameter = MsgID.Split('%');
        //    /*  MsgID="ScaleSizeIsEmpty%10"
        //        vMsgParameter={"ScaleSizeIsEmpty","10"}
        //        After split, msgID will be at index 0. The rest are parameters.
        //    */
        //    if (vMsgParameter.Length > 0)
        //    {
        //        //Use index 0 to retrieve msg value.
        //        objSYSMsgApp = SYSMsgApp.Get(vMsgParameter[0]);

        //        //In Database Table, msgID->"ScaleSizeIsEmpty", msgValue->"Scale Size {0} cannot be empty"

        //        MsgValue = "ERROR: Cannot determine System Language";

        //        if (SysOptionUtility.LanguageOption != string.Empty)
        //        {
        //            switch (SysOptionUtility.LanguageOption)
        //            {
        //                case "1": vMsgStr = objSYSMsgApp.MsgText1;
        //                    vMsgTitle = objSYSMsgApp.MsgTitle1;
        //                    break;
        //                case "2": vMsgStr = objSYSMsgApp.MsgText2;
        //                    vMsgTitle = objSYSMsgApp.MsgTitle2;
        //                    break;
        //                case "3": vMsgStr = objSYSMsgApp.MsgText3;
        //                    vMsgTitle = objSYSMsgApp.MsgTitle3;
        //                    break;
        //                case "4": vMsgStr = objSYSMsgApp.MsgText4;
        //                    vMsgTitle = objSYSMsgApp.MsgTitle4;
        //                    break;
        //                case "5": vMsgStr = objSYSMsgApp.MsgText5;
        //                    vMsgTitle = objSYSMsgApp.MsgTitle5;
        //                    break;
        //                case "6": vMsgStr = objSYSMsgApp.MsgText6;
        //                    vMsgTitle = objSYSMsgApp.MsgTitle6;
        //                    break;
        //                case "7": vMsgStr = objSYSMsgApp.MsgText7;
        //                    vMsgTitle = objSYSMsgApp.MsgTitle7;
        //                    break;
        //                case "8": vMsgStr = objSYSMsgApp.MsgText8;
        //                    vMsgTitle = objSYSMsgApp.MsgTitle8;
        //                    break;
        //                case "9": vMsgStr = objSYSMsgApp.MsgText9;
        //                    vMsgTitle = objSYSMsgApp.MsgTitle9;
        //                    break;
        //                case "10": vMsgStr = objSYSMsgApp.MsgText10;
        //                    vMsgTitle = objSYSMsgApp.MsgTitle10;
        //                    break;
        //            }

        //            //Replace  msgValue->"Scale Size {0} cannot be empty" to msgValue-> "Scale Size 10 cannot be empty"
        //            for (int i = 1; i < vMsgParameter.Length; i++)
        //            {
        //                string oldVal = "{" + (i-1) + "}";
        //                vMsgStr= vMsgStr.Replace(oldVal, vMsgParameter[i]);
        //            }
        //            MsgValue = vMsgStr;
        //            return true;
        //        }
        //    }
        //    return false;
        //}
    }
}
