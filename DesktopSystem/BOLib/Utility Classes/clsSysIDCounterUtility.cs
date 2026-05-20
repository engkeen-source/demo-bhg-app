using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.SqlClient;
using System.Data;
using TAUtil;

namespace BOLib
{
    public class SysIDCounterUtility
    {

        //Example. strVal=> "Techace Innovation Pte Ltd", WordNum=2, CharNum=3, BlankChar='_'
        //Result => "Tec_Inn"

        private static string Decode(string strOrigin, int wordNum, int charNum, string blankChar)
        {
            try
            {
                if (GFunc.IsNE(strOrigin))
                    return string.Empty;

                //Assume strOrigin="Techace Innovation Pte Ltd"
                /*  
                    After split, 
                    strValArr[0]-"Techace"
                    strValArr[1]-"Innovation"
                    strValArr[2]-"Pte"
                    strValArr[3]-"LTD"
                 */
                string[] strValArr = strOrigin.Split(' ');

                string resultStr = string.Empty;
                //
                for (int i = 0; i < strValArr.Length; i++)
                {
                    //Although original string has 4 words, we need to process 2 words (Because input WordNum->2)
                    if (i >= wordNum)
                        break;

                    //If char length of each word < charnum, no need to cut
                    if (strValArr[i].Length < charNum)
                        resultStr += strValArr[i]; //resultStr = resultStr+strValArr[i];
                    else
                        //take Left(strValArr[i],charNum)
                        resultStr += strValArr[i].Substring(0, charNum);
                }
                //Replace BlankChar
                resultStr = resultStr.Replace(" ", blankChar);

                return resultStr;
            }
            catch (Exception ex)
            {
                throw Error(ex);
            }
        }

        /// <summary>
        /// Get Auto ID for Master and Reference
        /// </summary>
        /// <param name="msgID"></param>
        /// <param name="autoID"></param>
        /// <param name="codeKey"></param>
        /// <returns></returns>
        public static bool Get(System.Data.SqlClient.SqlConnection cn, bool DisplayMsg, out string autoID, GEnum.SystemCode codeKey, string DataDes)
        {
            // Generate ID for Reference and Master Modules
            try
            {
                string msgID = "GetAutoIDFail";
                bool ProcessOK = true;

                int? vCounterGrp = 1;
                int? vInitial = 1000;
                bool vReset = false;

                string vCounterGrpStr = string.Empty;
                string vBeforeSep = string.Empty;
                string vAfterSep = string.Empty;
                int? vLastCounter = 0;

                bool newIDCounter = false;
                SYSIDcounter objSYSIDCounter = null;

                int a;
                string vAutoID = string.Empty;
                autoID = vAutoID;
                if ((int)codeKey < 20000)
                    msgID = MsgID.Option.InvalidCallOption;

                if (ProcessOK)
                {
                    //Get Infor from SYSCounterGrp
                    //SYSCounterGrp(codeKey,vCounterGrp);
                    SYSCounterGrp objSYSCounterGrp = SYSCounterGrp.New();
                    objSYSCounterGrp.Fetch(cn, new SYSCounterGrp.Criteria(codeKey, vCounterGrp, 2));

                    if (!GFunc.IsNE(objSYSCounterGrp))
                    {
                        vInitial = objSYSCounterGrp.InitialNumber;
                        vReset = (bool)objSYSCounterGrp.Reset;
                    }
                    else
                    {
                        msgID = MsgID.Common.GetInforFail + "%SYSCounterGrp";
                        ProcessOK = false;
                    }
                }

                //Get Infor from SYSCounterGrpDetItm
                if (ProcessOK)
                {
                    //SYSCounterGrpDetItm.Get(codeKey, vCounterGrp);
                    SYSCounterGrpDetItm objSYSCounterGrpDetItm = SYSCounterGrpDetItm.New();
                    objSYSCounterGrpDetItm.Fetch(cn, new SYSCounterGrpDetItm.Criteria(codeKey, vCounterGrp, 1));

                    if (GFunc.IsNE(objSYSCounterGrpDetItm) == false)
                    {
                        vCounterGrpStr = Decode(DataDes, (int)objSYSCounterGrpDetItm.WordNum, (int)objSYSCounterGrpDetItm.CharacterNum,
                                        objSYSCounterGrpDetItm.BlankCharacter);

                        vBeforeSep = objSYSCounterGrpDetItm._beforeFormatSeperator;
                        vAfterSep = objSYSCounterGrpDetItm._afterFormatSeperator;
                    }
                    else
                    {
                        msgID = MsgID.Common.GetInforFail + "%SYSCounterGrpDetItm";
                        ProcessOK = false;
                    }
                }

                //Get LastCounter
                if (ProcessOK)
                {
                    objSYSIDCounter = SYSIDcounter.New();
                    objSYSIDCounter.Fetch(cn, new SYSIDcounter.Criteria(codeKey, 0, vCounterGrpStr, 0, 0, 0, 1));

                    if (GFunc.IsNE(objSYSIDCounter) == false && GFunc.IsNEZ(Convert.ToInt32(objSYSIDCounter._codeKey)) == false)
                    {
                        vLastCounter = objSYSIDCounter.LastCounter;
                        newIDCounter = false;
                    }
                    else
                    {
                        vLastCounter = vInitial;
                        objSYSIDCounter._codeKey = codeKey;
                        objSYSIDCounter._period = 0;
                        objSYSIDCounter._counterGrpStr = vCounterGrpStr;
                        objSYSIDCounter._docGrpKey = 0;
                        objSYSIDCounter._conKey = 0;
                        objSYSIDCounter._eMKey = 0;
                        newIDCounter = true;
                    }
                }

                //AutoID generation
                if (ProcessOK)
                {
                    a = 1;

                    //Check for Duplicate ID
                    bool duplicate = true;
                    while (duplicate)
                    {
                        //Check for exceed maximum tries
                        if (a > 100)
                        {
                            msgID = MsgID.Record.ExceedMaxAutoIDTries;
                            ProcessOK = false;
                        }
                        else
                        {
                            vLastCounter = vLastCounter + 1;
                            vAutoID = vBeforeSep + vCounterGrpStr + vAfterSep + vLastCounter;
                            //if(Decode Find for duplicate ID in respective table)
                            if (DuplicateFound(cn, vAutoID, codeKey, 0, 0, 0))
                                a = a + 1;
                            else
                                duplicate = false;
                        }
                    }
                }

                //Insert or update 
                if (ProcessOK)
                {
                    objSYSIDCounter._lastCounter = vLastCounter;
                    if (newIDCounter)
                    {
                        objSYSIDCounter.Insert(cn);
                    }
                    else
                    {
                        objSYSIDCounter.Update(cn);
                    }
                    autoID = vAutoID;
                }
                else
                {
                    if (DisplayMsg)
                    {
                        MsgBox.Show(cn, msgID);
                    }
                }


                return ProcessOK;
            }
            catch (Exception ex)
            {
                throw Error(ex);
            }

        }
        /// <summary>
        /// Get Auto ID for Master and Reference
        /// </summary>
        /// <param name="msgID"></param>
        /// <param name="autoID"></param>
        /// <param name="codeKey"></param>
        /// <returns></returns>
        public static bool Get(System.Data.SqlClient.SqlConnection cn, bool DisplayMsg, out string autoID, GEnum.SystemCode codeKey, string DataDes, short CatNum)
        {
            // Generate ID for Category Module
            try
            {
                string msgID = "GetAutoIDFail";
                bool ProcessOK = true;

                int? vCounterGrp = 1;
                int? vInitial = 1000;
                bool vReset = false;

                string vCounterGrpStr = string.Empty;
                string vBeforeSep = string.Empty;
                string vAfterSep = string.Empty;
                int? vLastCounter = 0;

                bool newIDCounter = false;
                SYSIDcounter objSYSIDCounter = null;

                int a;
                string vAutoID = string.Empty;
                autoID = vAutoID;
                if ((int)codeKey < 20000)
                    msgID = MsgID.Option.InvalidCallOption;

                if (ProcessOK)
                {
                    //Get Infor from SYSCounterGrp
                    //SYSCounterGrp(codeKey,vCounterGrp);
                    SYSCounterGrp objSYSCounterGrp = SYSCounterGrp.New();
                    objSYSCounterGrp.Fetch(cn, new SYSCounterGrp.Criteria(codeKey, vCounterGrp, 2));

                    if (!GFunc.IsNE(objSYSCounterGrp))
                    {
                        vInitial = objSYSCounterGrp.InitialNumber;
                        vReset = (bool)objSYSCounterGrp.Reset;
                    }
                    else
                    {
                        msgID = MsgID.Common.GetInforFail + "%SYSCounterGrp";
                        ProcessOK = false;
                    }
                }

                //Get Infor from SYSCounterGrpDetItm
                if (ProcessOK)
                {
                    //SYSCounterGrpDetItm.Get(codeKey, vCounterGrp);
                    SYSCounterGrpDetItm objSYSCounterGrpDetItm = SYSCounterGrpDetItm.New();
                    objSYSCounterGrpDetItm.Fetch(cn, new SYSCounterGrpDetItm.Criteria(codeKey, vCounterGrp, 1));

                    if (GFunc.IsNE(objSYSCounterGrpDetItm) == false)
                    {
                        vCounterGrpStr = Decode(DataDes, (int)objSYSCounterGrpDetItm.WordNum, (int)objSYSCounterGrpDetItm.CharacterNum,
                                        objSYSCounterGrpDetItm.BlankCharacter);

                        vBeforeSep = objSYSCounterGrpDetItm._beforeFormatSeperator;
                        vAfterSep = objSYSCounterGrpDetItm._afterFormatSeperator;
                    }
                    else
                    {
                        msgID = MsgID.Common.GetInforFail + "%SYSCounterGrpDetItm";
                        ProcessOK = false;
                    }
                }

                //Get LastCounter
                if (ProcessOK)
                {
                    objSYSIDCounter = SYSIDcounter.New();
                    objSYSIDCounter.Fetch(cn, new SYSIDcounter.Criteria(codeKey, 0, vCounterGrpStr, 0, 0, 0, 1));

                    if (GFunc.IsNE(objSYSIDCounter) == false && GFunc.IsNEZ(Convert.ToInt32(objSYSIDCounter._codeKey)) == false)
                    {
                        vLastCounter = objSYSIDCounter.LastCounter;
                        newIDCounter = false;
                    }
                    else
                    {
                        vLastCounter = vInitial;
                        objSYSIDCounter._codeKey = codeKey;
                        objSYSIDCounter._period = 0;
                        objSYSIDCounter._counterGrpStr = vCounterGrpStr;
                        objSYSIDCounter._docGrpKey = 0;
                        objSYSIDCounter._conKey = 0;
                        objSYSIDCounter._eMKey = 0;
                        newIDCounter = true;
                    }
                }

                //AutoID generation
                if (ProcessOK)
                {
                    a = 1;

                    //Check for Duplicate ID
                    bool duplicate = true;
                    while (duplicate)
                    {
                        //Check for exceed maximum tries
                        if (a > 100)
                        {
                            msgID = MsgID.Record.ExceedMaxAutoIDTries;
                            ProcessOK = false;
                        }
                        else
                        {
                            vLastCounter = vLastCounter + 1;
                            vAutoID = vBeforeSep + vCounterGrpStr + vAfterSep + vLastCounter;
                            //if(Decode Find for duplicate ID in respective table)
                            if (DuplicateFound(vAutoID, codeKey, 0, 0, CatNum))
                                a = a + 1;
                            else
                                duplicate = false;
                        }
                    }
                }

                //Insert or update 
                if (ProcessOK)
                {
                    objSYSIDCounter._lastCounter = vLastCounter;
                    if (newIDCounter)
                    {
                        objSYSIDCounter.Insert(cn);
                    }
                    else
                    {
                        objSYSIDCounter.Update(cn);
                    }
                    autoID = vAutoID;
                }
                else
                {
                    if (DisplayMsg)
                    {
                        MsgBox.Show(cn, msgID);
                    }
                }


                return ProcessOK;
            }
            catch (Exception ex)
            {
                throw Error(ex);
            }

        }

        /// <summary>
        /// Get Auto ID for Documents
        /// </summary>
        /// <param name="msgID"></param>
        /// <param name="codeKey"></param>
        /// <param name="DocTypeNm"></param>
        /// <param name="Period"></param>
        /// <param name="CounterGrp"></param>
        /// <param name="DocGrpKey"></param>
        /// <param name="ConKey"></param>
        /// <param name="EMKey"></param>
        /// <returns></returns>
        public static bool Get(out string AutoID, out string counterGrpStr, out int counterLastID, out int counterPeriod, out int counterDocGrpKey, out int counterConKey, out int counterEMKey, GEnum.SystemCode codeKey, string DocTypeNm, int? DocGrpKey, int? ConKey, int? EMKey, DateTime DocDate)
        {
            // Generate DocID for Document Modules
            try
            {
                using (SqlConnection cn = new SqlConnection(AppInfor.currentDBConnectionStr))
                {
                    cn.Open();
                    return Get(cn, out AutoID, out counterGrpStr, out counterLastID, out counterPeriod, out counterDocGrpKey, out counterConKey, out counterEMKey, codeKey, DocTypeNm, DocGrpKey, ConKey, EMKey, DocDate);
                }
            }
            catch (Exception ex)
            {
                throw Error(ex);
            }
        }
        public static bool Get(SqlConnection cn, out string AutoID, out string counterGrpStr, out int counterLastID, out int counterPeriod, out int counterDocGrpKey, out int counterConKey, out int counterEMKey, GEnum.SystemCode codeKey, string DocTypeNm, int? DocGrpKey, int? ConKey, int? EMKey, DateTime DocDate)
        {
            // Generate DocID for Document Modules

            try
            {
                #region Declaration
                int CounterGrp = 1;
                int Initial = 1000;
                bool Reset = false;

                counterGrpStr = string.Empty;
                counterLastID = 0;
                counterPeriod = 0;
                counterDocGrpKey = 0;
                counterConKey = 0;
                counterEMKey = 0;

                string Prefix = string.Empty;
                string Date = string.Empty;
                string ConID = string.Empty;
                string DocGrpID = string.Empty;
                string EMID = string.Empty;
                string counterBefSep = "";
                string counterAftSep = "";

                AutoID = string.Empty;

                SYSIDcounter objSYSIDCounter = null;
                #endregion

                #region Validation - Return false when AutoID to generate is not calling within document code range
                if ((int)codeKey >= 20000)
                {
                    MsgBox.Show(cn, MsgID.Option.InvalidCallOption);
                    return false;
                }
                #endregion

                #region Get Infor from SYSDocTypeNm
                SYSDocTypeDetNm objSYSDocTypeDetNm = SYSDocTypeDetNm.New();
                objSYSDocTypeDetNm.Fetch(cn, new SYSDocTypeDetNm.Criteria(codeKey, DocTypeNm, 2));

                if (GFunc.IsNE(objSYSDocTypeDetNm))
                {
                    MsgBox.Show(cn, MsgID.Common.GetInforFail + "%SYSDocTypeDetNm");
                    return false;
                }
                else
                {
                    CounterGrp = (int)objSYSDocTypeDetNm._counterGrp;
                    counterGrpStr = Convert.ToString(CounterGrp);
                }
                #endregion

                #region Get Infor from SYSCounterGrp
                SYSCounterGrp objSYSCounterGrp = SYSCounterGrp.New();
                objSYSCounterGrp.Fetch(cn, new SYSCounterGrp.Criteria(codeKey, CounterGrp, 2));

                if (GFunc.IsNE(objSYSCounterGrp))
                {
                    MsgBox.Show(cn, MsgID.Common.GetInforFail + "%1SYS_CounterGrp");
                    return false;
                }
                else
                {
                    Initial = (int)objSYSCounterGrp.InitialNumber;
                    Reset = (bool)objSYSCounterGrp.Reset;
                }
                #endregion

                #region Get Infor from SYSCounterGrpDetItm
                SYSCounterGrpDetItms objSYSCounterGrpDetItms = new SYSCounterGrpDetItms(cn);
                objSYSCounterGrpDetItms.Fetch(cn, new SYSCounterGrpDetItms.Criteria((int?)codeKey, CounterGrp, (int?)1));

                if (GFunc.IsNE(objSYSCounterGrpDetItms))
                {
                    MsgBox.Show(cn, MsgID.Common.GetInforFail + "%1SYS_CounterGrpDetItm");
                    return false;
                }
                else
                {
                    foreach (DataRow objDetail in objSYSCounterGrpDetItms.Rows)
                    {
                        switch (objDetail["SegmentID"].ToString())
                        {
                            #region Get Prefix
                            case "Prefix":
                                if ((bool)objDetail["Selected"])
                                    Prefix = objDetail["beforeFormatSeperator"].ToString() + objDetail["SegmentValue"].ToString() + objDetail["afterFormatSeperator"].ToString();
                                break;
                            #endregion

                            #region Get Date Format
                            case "Date":
                                if ((bool)objDetail["Selected"])
                                {
                                    if (GFunc.NEStr(objDetail["SegmentValue"],"") != "")
                                    {
                                        try
                                        {
                                             //Since date format usually recognize 'Y' as small letter, change it to small one
                                            objDetail["SegmentValue"] = objDetail["SegmentValue"].ToString().Replace('Y', 'y').Replace('m','M');
                                          
                                            counterPeriod = (int)Convert.ToInt32(DocDate.ToString(objDetail["SegmentValue"].ToString()));                                     
                                            Date = objDetail["beforeFormatSeperator"].ToString() + counterPeriod.ToString() + objDetail["afterFormatSeperator"].ToString();
                                        }
                                        catch
                                        {
                                             //Ignore the error when DateFormat is selected and it's SegmmentValue is not a valid format.
                                        }
                                     
                                    }
                                }
                                break;
                            #endregion

                            #region Get DocGrp Format
                            case "DocGrpKey":
                                if ((bool)objDetail["Selected"])
                                {
                                    REFDocGrp objREFDocGrp = REFDocGrp.New();
                                    objREFDocGrp.Fetch(cn, new REFDocGrp.Criteria(DocGrpKey, (int?)1));
                                    if (GFunc.IsNE(objREFDocGrp) == false)
                                    {
                                        DocGrpID = objDetail["beforeFormatSeperator"].ToString() + Decode(objREFDocGrp.DocGrpID, (byte)objDetail["WordNum"], (byte)objDetail["CharacterNum"], objDetail["BlankCharacter"].ToString()) + objDetail["afterFormatSeperator"].ToString();
                                        counterDocGrpKey = (int)DocGrpKey;
                                    }
                                    else
                                        DocGrpID = string.Empty;
                                }
                                break;
                            #endregion

                            #region Get ConKey Format
                            case "DocConKey":
                                if ((bool)objDetail["Selected"])
                                {
                                    MSTCon objMSTCon = MSTCon.New();
                                    objMSTCon.Fetch(cn, new MSTCon.Criteria(ConKey, (int?)1));
                                    if (GFunc.IsNE(objMSTCon) == false)
                                    {
                                        ConID = objDetail["beforeFormatSeperator"].ToString() + Decode(objMSTCon.ConID, (byte)objDetail["WordNum"], (byte)objDetail["CharacterNum"], objDetail["BlankCharacter"].ToString()) + objDetail["afterFormatSeperator"].ToString();
                                        counterDocGrpKey = (int)ConKey;
                                    }
                                    else
                                        ConID = string.Empty;
                                }
                                break;
                            #endregion

                            #region Get JobKey Format
                            case "DocEmKey":
                                if ((bool)objDetail["Selected"])
                                {
                                    //Modified on 22 Aug 2021.. Use DocEmKey for Job because generating Doc ID base on Sales Rep will not be used

                                    // MSTSalesRep objMSTSalesRep = MSTSalesRep.New();
                                    //objMSTSalesRep.Fetch(cn, new MSTSalesRep.Criteria(EMKey, (int?)1));
                                    //if (GFunc.IsNE(objMSTSalesRep) == false)
                                    //{
                                    //    EMID = objDetail["beforeFormatSeperator"].ToString() + Decode(objMSTSalesRep.EmID, (byte)objDetail["WordNum"], (byte)objDetail["CharacterNum"], objDetail["BlankCharacter"].ToString()) + objDetail["afterFormatSeperator"].ToString();
                                    //    counterEMKey = (int)EMKey;
                                    //}
                                    //else
                                    //    EMID = string.Empty;
                                
                                    MSTJob objMSTJob = MSTJob.New();
                                    objMSTJob.Fetch(cn, new MSTJob.Criteria(EMKey, (int?)1));
                                    if (GFunc.IsNE(objMSTJob) == false)
                                    {
                                        EMID = objDetail["beforeFormatSeperator"].ToString() + Decode(objMSTJob.JobID, (byte)objDetail["WordNum"], (byte)objDetail["CharacterNum"], objDetail["BlankCharacter"].ToString()) + objDetail["afterFormatSeperator"].ToString();
                                        counterEMKey = (int)EMKey;
                                    }
                                    else
                                        EMID = string.Empty;
                                   
                                }
                                break;
                            #endregion

                            #region Get last Counter
                            case "Counter":
                                objSYSIDCounter = SYSIDcounter.New();
                                if (Reset)
                                {
                                    objSYSIDCounter.Fetch(cn, new SYSIDcounter.Criteria(codeKey, counterPeriod, counterGrpStr, counterDocGrpKey, counterConKey, counterEMKey, 1));
                                    if (GFunc.IsNE(objSYSIDCounter.CodeKey) == false)
                                    {
                                        counterLastID = (int)objSYSIDCounter.LastCounter;                                             
                                    }
                                    else
                                    {
                                        counterLastID = Initial;
                                        objSYSIDCounter._codeKey = codeKey;
                                        objSYSIDCounter._period = counterPeriod;
                                        objSYSIDCounter._counterGrpStr = counterGrpStr;
                                        objSYSIDCounter._docGrpKey = counterDocGrpKey;
                                        objSYSIDCounter._conKey = counterConKey;
                                        objSYSIDCounter._eMKey = counterEMKey;
                                        objSYSIDCounter._lastCounter = counterLastID;
                                        objSYSIDCounter.Insert(cn);
                                    }
                                }
                                else
                                {
                                    objSYSIDCounter.Fetch(cn, new SYSIDcounter.Criteria(codeKey, 0, counterGrpStr, 0, 0, 0, 1));
                                    if (GFunc.IsNE(objSYSIDCounter.CodeKey) == false)
                                        counterLastID = (int)objSYSIDCounter.LastCounter;
                                    else
                                    {
                                        counterLastID = Initial;
                                        objSYSIDCounter._codeKey = codeKey;
                                        objSYSIDCounter._period = 0;
                                        objSYSIDCounter._counterGrpStr = counterGrpStr;
                                        objSYSIDCounter._docGrpKey = 0;
                                        objSYSIDCounter._conKey = 0;
                                        objSYSIDCounter._eMKey = 0;
                                        objSYSIDCounter._lastCounter = counterLastID;
                                        objSYSIDCounter.Insert(cn);
                                    }
                                }
                                counterBefSep = objDetail["beforeFormatSeperator"].ToString();
                                counterAftSep = objDetail["afterFormatSeperator"].ToString();
                                break;
                            #endregion
                        }
                    }

                    if (Reset == false)
                    {
                        counterPeriod = 0;
                        counterDocGrpKey = 0;
                        counterConKey = 0;
                        counterEMKey = 0;
                    }
                }
                #endregion

                #region AutoID generation
                int a = 1;
                string strCounterLastID = string.Empty;
                bool duplicate = true;

                while (duplicate)
                {
                    //Check for exceed maximum tries
                    if (a > 100)
                    {
                        MsgBox.Show(cn, "ExceedMaxAutoIDTries");
                        return false;
                    }
                    else
                    {
                        counterLastID = counterLastID + 1;
                        if (counterLastID < 100) /* added by YST on 2021/12/15 to start counter 1 digit with format 001,010 requested by SFP */
                        {
                            strCounterLastID = counterLastID > 9 ? "0" : "00";
                        }
                        AutoID = string.Empty;
                        foreach (DataRow objDetail in objSYSCounterGrpDetItms.Rows)
                        {
                            switch (objDetail["SegmentID"].ToString())
                            {
                                case "Prefix":
                                    AutoID = AutoID + Prefix; break;
                                case "Date":
                                    AutoID = AutoID + Date; break;
                                case "DocGrpKey":
                                    AutoID = AutoID + DocGrpID; break;
                                case "DocConKey":
                                    AutoID = AutoID + ConID; break;
                                case "DocEmKey":
                                    AutoID = AutoID + EMID; break;
                                case "Counter":
                                    AutoID = AutoID+ counterBefSep + strCounterLastID + counterLastID + counterAftSep; break;
                            }
                        }

                        //If Decode Find for duplicate ID in respective table
                        if (DuplicateFound(cn, AutoID, codeKey, GFunc.NEInt(ConKey, 0), GFunc.NEInt(DocGrpKey, 0), 0))
                            a = a + 1;
                        else
                            duplicate = false;
                    }
                }
                #endregion

                return true;
            }
            catch (Exception ex)
            {
                throw Error(ex);
            }
        }

        /// <summary>
        /// Checking duplication
        /// </summary>
        /// <param name="cn"></param>
        /// <param name="vAutoID">Auto Generated Number to check duplication</param>
        /// <param name="codeKey"></param>
        /// <param name="vConKey"></param>
        /// <param name="vDocGrpKey"></param>
        /// <param name="vCatNum"></param>
        /// <returns></returns>
        public static bool DuplicateFound(System.Data.SqlClient.SqlConnection cn, string vAutoID, GEnum.SystemCode codeKey, int vConKey, int vDocGrpKey, short vCatNum)
        {
            string msgID = MsgID.Validation.DuplicateRecordIDParams;

            try
            {
                #region CodeKey
                switch (codeKey)
                {
                    //Enquiry Document
                    case GEnum.SystemCode.Quotation:
                        ARQO objARQO = ARQO.New();
                        objARQO.DocID = vAutoID;
                        msgID += "%Document%Quotation";
                        return !(objARQO.Validation(cn, new ARQO.Criteria((int)codeKey, 0, vAutoID, 0), true));

                    //Request Document
                    case GEnum.SystemCode.Purchase_Plan:
                        APPN objAPPN = APPN.New();
                        objAPPN.DocID = vAutoID;
                        msgID += "%Document%Purchase Plan";
                        if (objAPPN.DocState == (int)GEnum.DocState.New)
                            return !(objAPPN.Validation(cn, new APPN.Criteria((int)codeKey, 0, vAutoID, 0), true));
                        else
                            return !(objAPPN.Validation(cn, new APPN.Criteria((int)codeKey, 0, vAutoID, 0), false));

                    case GEnum.SystemCode.Purchase_Request:
                        APRQ objAPRQ = APRQ.New();
                        objAPRQ.DocID = vAutoID;
                        return !(objAPRQ.Validation(cn, new APRQ.Criteria((int)codeKey, 0, vAutoID, 0), true));

                    //Order Document              
                    case GEnum.SystemCode.Sales_Order:
                        ARSO objARSO = ARSO.New(out msgID);
                        objARSO.DocID = vAutoID;
                        return !(objARSO.Validation(cn, new ARSO.Criteria((int)codeKey, 0, vAutoID, 0), out msgID, true));

                    case GEnum.SystemCode.Purchase_Order:
                        APPO objAPPO = APPO.New();
                        objAPPO.DocID = vAutoID;
                        return !(objAPPO.Validation(cn, new APPO.Criteria((int)codeKey, 0, vAutoID, 0), true));

                    case GEnum.SystemCode.Order_Consignment:
                        CSCPO objCSCPO = CSCPO.New();
                        objCSCPO.DocID = vAutoID;
                        return !(objCSCPO.Validation(cn, new CSCPO.Criteria((int)codeKey, 0, vAutoID, 0), true));

                    //Order Adjustment Document  
                    case GEnum.SystemCode.Purchase_Order_Adjustment:
                    case GEnum.SystemCode.Consignment_Order_Adjustment:
                    case GEnum.SystemCode.Sales_Order_Adjustment:
                        APPJ objAPPJ = APPJ.New();
                        objAPPJ.DocID = vAutoID;
                        return !(objAPPJ.Validation(cn, new APPJ.Criteria((int)codeKey, 0, vAutoID, 0), true));

                    //Works Document
                    case GEnum.SystemCode.Works_Order:
                    //FutureFeature
                    //REFWorks_Order objRefWorks_Order = REFWorks_Order.New(out msgID);
                    //objRefWorks_Order._works_orderID = vAutoID;
                    //return !(objRefWorks_Order.Validation(cn, new REFWorks_Order.Criteria(0, vAutoID), out msgID, true));
                    //break;

                    //Delivery Document
                    case GEnum.SystemCode.Delivery_Order:
                        ARDO objARDO = ARDO.New();
                        objARDO.DocID = vAutoID;
                        return !(objARDO.Validation(cn, new ARDO.Criteria((int)codeKey, 0, vAutoID, 0), true));

                    case GEnum.SystemCode.Purchase_Delivery:
                        APPD objAPPD = APPD.New();
                        objAPPD.DocID = vAutoID;
                        return !(objAPPD.Validation(cn, new APPD.Criteria((int)codeKey, 0, vAutoID, vConKey), true));

                    case GEnum.SystemCode.Received_Consignment:
                        CSCPD objCSCPD = CSCPD.New();
                        objCSCPD.DocID = vAutoID;
                        return !(objCSCPD.Validation(cn, new CSCPD.Criteria((int)codeKey, 0, vAutoID, vConKey), true));

                    case GEnum.SystemCode.Issue_Consignment:
                    case GEnum.SystemCode.Return_Consignment:
                        CSCSI objCSCSI = CSCSI.New();
                        objCSCSI.DocID = vAutoID;
                        return !(objCSCSI.Validation(cn, new CSCSI.Criteria((int)codeKey, 0, vAutoID, 0), true));

                    //Transfer/Settlement Document
                    case GEnum.SystemCode.Packing_List:
                        ARPL objARPL = ARPL.New();
                        objARPL.DocID = vAutoID;
                        return !(objARPL.Validation(cn, new ARPL.Criteria((int)codeKey, 0, vAutoID, 0), true));

                    case GEnum.SystemCode.Consignment_Settlement:
                        CSCPS objCSCPS = CSCPS.New();
                        objCSCPS.DocID = vAutoID;
                        return !(objCSCPS.Validation(cn, new CSCPS.Criteria((int)codeKey, 0, vAutoID, 0), true));

                    //Invoice Document
                    case GEnum.SystemCode.Sales_Invoice:
                    case GEnum.SystemCode.Sales_Debit_Note:
                    case GEnum.SystemCode.Sales_Credit_Note:
                    case GEnum.SystemCode.Cash_Sale:
                    case GEnum.SystemCode.Cash_Debit_Note:
                    case GEnum.SystemCode.Cash_Credit_Note:
                        ARIV objARIV = ARIV.New();
                        objARIV.DocID = vAutoID;
                        return !(objARIV.Validation(cn, new ARIV.Criteria((int)codeKey, 0, vAutoID, 0), true));

                    case GEnum.SystemCode.Purchase_Invoice:
                    case GEnum.SystemCode.Purchase_Debit_Note:
                    case GEnum.SystemCode.Purchase_Credit_Note:
                        APBL objAPIV = APBL.New();
                        objAPIV.DocID = vAutoID;
                        return !(objAPIV.Validation(cn, new APBL.Criteria((int)codeKey, 0, vAutoID, vConKey), true));

                    //Adjustment Document
                    case GEnum.SystemCode.Purchase_Adjustment:
                        APADJ objAPADJ = APADJ.New();
                        objAPADJ.DocID = vAutoID;
                        return !(objAPADJ.Validation(cn, new APADJ.Criteria((int)codeKey, 0, vAutoID, 0), true));

                    case GEnum.SystemCode.Sales_Adjustment:
                    case GEnum.SystemCode.Cash_Adjustment:
                        ARADJ objARADJ = ARADJ.New(out msgID);
                        objARADJ.DocID = vAutoID;
                        return !(objARADJ.Validation(cn, new ARADJ.Criteria((int)codeKey, 0, vAutoID, 0), true));
                    //Payment Document
                    case GEnum.SystemCode.Payment_Issue:
                        APPY objAPPY = APPY.New();
                        objAPPY.DocID = vAutoID;
                        return !(objAPPY.Validation(cn, new APPY.Criteria((int)codeKey, 0, vAutoID, 0), true));

                    case GEnum.SystemCode.Payment_Received:
                    case GEnum.SystemCode.Cash_Payment_Received:
                        ARPY objARPY = ARPY.New(out msgID);
                        objARPY.DocID = vAutoID;
                        return !(objARPY.Validation(cn, new ARPY.Criteria((int)codeKey, 0, vAutoID, 0), true));

                    case GEnum.SystemCode.Contra:
                    case GEnum.SystemCode.Cash_Contra:
                        ARCT objARCT = ARCT.New();
                        objARCT.DocID = vAutoID;
                        return !(objARCT.Validation(cn, new ARCT.Criteria((int)codeKey, 0, vAutoID, 0), true));

                    //Inventory Document
                    case GEnum.SystemCode.Inventory_Adjustment:
                        INADJ objINADJ = INADJ.New();
                        objINADJ.DocID = vAutoID;
                        return !(objINADJ.Validation(cn, new INADJ.Criteria((int)codeKey, 0, vAutoID, 0), true));

                    case GEnum.SystemCode.Inventory_Production:
                        INMFN objINMFN = INMFN.New();
                        objINMFN.DocID = vAutoID;
                        if (objINMFN.DocState == (int)GEnum.DocState.New)
                            return !(objINMFN.Validation(cn, new INMFN.Criteria((int)codeKey, 0, vAutoID, 0), true));
                        else
                            return !(objINMFN.Validation(cn, new INMFN.Criteria((int)codeKey, 0, vAutoID, 0), false));

                    case GEnum.SystemCode.Inventory_Transfer:
                        INTRN objINTRN = INTRN.New();
                        objINTRN.DocID = vAutoID;
                        return !(objINTRN.Validation(cn, new INTRN.Criteria((int)codeKey, 0, vAutoID, 0), true));

                    //Account Document
                    case GEnum.SystemCode.Journal:
                        GLJNL objGLJNL = GLJNL.New();
                        objGLJNL.DocID = vAutoID;
                        return !(objGLJNL.Validation(cn, new GLJNL.Criteria((int)codeKey, 0, vAutoID, 0), true));

                    case GEnum.SystemCode.Deposit:

                        GLDP objGLDP = GLDP.New();
                        objGLDP.DocID = vAutoID;
                        return !(objGLDP.Validation(cn, new GLDP.Criteria((int)codeKey, 0, vAutoID, 0), true));


                    case GEnum.SystemCode.Bank_Revaluation:
                        GLRV objGLRV = GLRV.New();
                        objGLRV.DocID = vAutoID;
                        return !(objGLRV.Validation(cn, new GLRV.Criteria((int)codeKey, 0, vAutoID, 0), true));

                    //System
                    case GEnum.SystemCode.Document_Group:
                        REFDocGrp objRefDocument_Group = REFDocGrp.New();
                        objRefDocument_Group._docGrpID = vAutoID;
                        return !(objRefDocument_Group.Validation(cn, new REFDocGrp.Criteria(0, vAutoID), true));


                    case GEnum.SystemCode.General_List: //SysMsgText
                        SYSMsgListText objSYSMsgListText = SYSMsgListText.New();
                        objSYSMsgListText._msgValue = vAutoID;
                        return !(objSYSMsgListText.Validation(cn, new SYSMsgListText.Criteria(vDocGrpKey, vAutoID), true));

                    //Account
                    case GEnum.SystemCode.Account:
                        MSTAcc objRefAccount = MSTAcc.New();
                        objRefAccount._accID = vAutoID;
                        return !(objRefAccount.Validation(cn, new MSTAcc.Criteria(0, vAutoID), true));

                    case GEnum.SystemCode.Branch:
                        MSTAccBranch objMSTAccBranch = MSTAccBranch.New();
                        objMSTAccBranch._branchID = vAutoID;
                        return !(objMSTAccBranch.Validation(cn, new MSTAccBranch.Criteria(0, vAutoID, 0), true));

                    case GEnum.SystemCode.Department:
                        MSTAccDept objMSTAccDept = MSTAccDept.New();
                        objMSTAccDept._deptID = vAutoID;
                        return !(objMSTAccDept.Validation(cn, new MSTAccDept.Criteria(0, vAutoID, 0), true));

                    case GEnum.SystemCode.Currency:
                        REFCurr objRefCurrency = REFCurr.New();
                        objRefCurrency._currID = vAutoID;
                        return !(objRefCurrency.Validation(cn, new REFCurr.Criteria(0, vAutoID), true));

                    case GEnum.SystemCode.Bank:
                        REFBank objRefBank = REFBank.New();
                        objRefBank._bankID = vAutoID;
                        return !(objRefBank.Validation(cn, new REFBank.Criteria(0, vAutoID), true));

                    case GEnum.SystemCode.Payment_Mode:
                        REFPayMode objRefPayment_Mode = REFPayMode.New();
                        objRefPayment_Mode._payModeID = vAutoID;
                        return !(objRefPayment_Mode.Validation(cn, new REFPayMode.Criteria(0, vAutoID), true));

                    case GEnum.SystemCode.Tax_Authority:
                        REFTaxA objRefTax_Authority = REFTaxA.New();
                        objRefTax_Authority._taxID = vAutoID;
                        return !(objRefTax_Authority.Validation(cn, new REFTaxA.Criteria(0, vAutoID), true));

                    case GEnum.SystemCode.Tax_Group:
                        REFTaxGrp objRefTax_Group = REFTaxGrp.New();
                        objRefTax_Group._taxGrpID = vAutoID;
                        return !(objRefTax_Group.Validation(cn, new REFTaxGrp.Criteria(0, vAutoID), true));

                    case GEnum.SystemCode.Overhead:
                        REFOverHead objRefOverhead = REFOverHead.New();
                        objRefOverhead._overHeadID = vAutoID;
                        return !(objRefOverhead.Validation(cn, new REFOverHead.Criteria(0, vAutoID), true));

                    case GEnum.SystemCode.Account_Group:
                        REFAccGrp objRefAccount_Group = REFAccGrp.New();
                        objRefAccount_Group._accGrpID = vAutoID;
                        return !(objRefAccount_Group.Validation(cn, new REFAccGrp.Criteria(0, vAutoID), true));

                    case GEnum.SystemCode.Sales_Representative:
                        MSTSalesRep objMSTSalesRep = MSTSalesRep.New();
                        objMSTSalesRep._emID = vAutoID;
                        return !(objMSTSalesRep.Validation(cn, new MSTSalesRep.Criteria(0, vAutoID), true));

                    case GEnum.SystemCode.Interest_Rate:
                        REFInterest objRefInterestRate = REFInterest.New();
                        objRefInterestRate._intID = vAutoID;
                        return !(objRefInterestRate.Validation(cn, new REFInterest.Criteria(0, vAutoID), true));

                    //Contact
                    case GEnum.SystemCode.Customer:
                        MSTCon objMSTConC = MSTCon.New();
                        objMSTConC._conID = vAutoID;
                        return !(objMSTConC.Validation(cn, new MSTCon.Criteria(0, vAutoID), true));

                    case GEnum.SystemCode.Vendor:
                        MSTCon objMSTConV = MSTCon.New();
                        objMSTConV._conID = vAutoID;
                        return !(objMSTConV.Validation(cn, new MSTCon.Criteria(0, vAutoID), true));

                    case GEnum.SystemCode.Price_List:
                        MSTPriceList objMstPrice_List = MSTPriceList.New();
                        objMstPrice_List._priceID = vAutoID;
                        return !(objMstPrice_List.Validation(cn, new MSTPriceList.Criteria(0, vAutoID, 0), true));

                    case GEnum.SystemCode.Payment_Term:
                        REFTerm objRefPayment_Term = REFTerm.New();
                        objRefPayment_Term._termID = vAutoID;
                        return !(objRefPayment_Term.Validation(cn, new REFTerm.Criteria(0, vAutoID), true));

                    case GEnum.SystemCode.Territory:
                        REFTerritory objRefTerritory = REFTerritory.New();
                        objRefTerritory._territoryID = vAutoID;
                        return !(objRefTerritory.Validation(cn, new REFTerritory.Criteria(0, vAutoID), true));

                    case GEnum.SystemCode.Industry:
                        REFIndustry objRefIndustry = REFIndustry.New();
                        objRefIndustry._industryID = vAutoID;
                        return !(objRefIndustry.Validation(cn, new REFIndustry.Criteria(0, vAutoID), true));

                    case GEnum.SystemCode.Shipping_Mode:
                        REFShipVia objREFShipVia = REFShipVia.New();
                        objREFShipVia._shipViaID = vAutoID;
                        return !(objREFShipVia.Validation(cn, new REFShipVia.Criteria(0, vAutoID), true));

                    case GEnum.SystemCode.Packing_Type:
                        REFPackingType objREFPackingType = REFPackingType.New();
                        objREFPackingType._packingTypeID = vAutoID;
                        return !(objREFPackingType.Validation(cn, new REFPackingType.Criteria(0, vAutoID, 0), true));

                    //Item 
                    case GEnum.SystemCode.Inventory:
                        MSTItm objMSTItm = MSTItm.New();
                        objMSTItm._itmID = vAutoID;
                        return !(objMSTItm.Validation(cn, new MSTItm.Criteria(0, 0, vAutoID), true));

                    case GEnum.SystemCode.Category:
                        REFCat objREFCat = REFCat.New();
                        objREFCat._catID = vAutoID;
                        return !(objREFCat.Validation(cn, new REFCat.Criteria(0, vAutoID, vCatNum, 0), true));

                    case GEnum.SystemCode.Brand:
                        REFBrand objRefBrand = REFBrand.New();
                        objRefBrand._brandID = vAutoID;
                        return !(objRefBrand.Validation(cn, new REFBrand.Criteria(0, vAutoID), true));

                    case GEnum.SystemCode.UOM:
                        REFUOM objRefUOM = REFUOM.New();
                        objRefUOM._uOMID = vAutoID;
                        return !(objRefUOM.Validation(cn, new REFUOM.Criteria(0, vAutoID, 0), true));

                    case GEnum.SystemCode.Color:
                        REFColor objRefColor = REFColor.New();
                        objRefColor._colorID = vAutoID;
                        return !(objRefColor.Validation(cn, new REFColor.Criteria(0, vAutoID), true));

                    case GEnum.SystemCode.Scale:
                        REFScale objRefScale = REFScale.New();
                        objRefScale._scaleID = vAutoID;
                        return !(objRefScale.Validation(cn, new REFScale.Criteria(0, vAutoID), true));

                    case GEnum.SystemCode.Location:
                        REFLoc objRefLocation = REFLoc.New();
                        objRefLocation._locID = vAutoID;
                        return !(objRefLocation.Validation(cn, new REFLoc.Criteria(0, vAutoID), true));

                    //Job
                    case GEnum.SystemCode.Job:
                        MSTJob objMSTJob = MSTJob.New();
                        objMSTJob._jobID = vAutoID;
                        return !(objMSTJob.Validation(cn, new MSTJob.Criteria(0, vAutoID), true));

                    case GEnum.SystemCode.Job_Cost_Type:
                        REFJobCostType objREFJobCostType = REFJobCostType.New();
                        objREFJobCostType._jobCostTypeID = vAutoID;
                        return !(objREFJobCostType.Validation(cn, new REFJobCostType.Criteria(0, vAutoID), true));

                    case GEnum.SystemCode.Job_Phase:
                        REFJobPhase objREFJobPhase = REFJobPhase.New();
                        objREFJobPhase._jobPhaseID = vAutoID;
                        return !(objREFJobPhase.Validation(cn, new REFJobPhase.Criteria(0, vAutoID), true));

                    case GEnum.SystemCode.Job_Task:
                        REFJobTask objREFJobTask = REFJobTask.New();
                        objREFJobTask._jobTaskID = vAutoID;
                        return !(objREFJobTask.Validation(cn, new REFJobTask.Criteria(0, vAutoID), true));

                    case GEnum.SystemCode.Job_Group:
                        REFJobGrp objREFJobGroup = REFJobGrp.New();
                        objREFJobGroup._jobGrpID = vAutoID;
                        return !(objREFJobGroup.Validation(cn, new REFJobGrp.Criteria(0, vAutoID), true));

                    //Task
                    case GEnum.SystemCode.Alerts:
                        TASAlert objTASAlert = TASAlert.New();
                        objTASAlert._alertID = vAutoID;
                        return !(objTASAlert.Validation(cn, new TASAlert.Criteria(0, vAutoID), true));

                    case GEnum.SystemCode.To_Do:
                        //There are no ToDo_ID in the table design, so there is no need to check for duplicate ID
                        return false;//false indicate that there are no duplicate found
                }
                #endregion
            }
            catch (Exception ex)
            {
                throw Error(ex);
            }
            return false;
        }
        public static bool DuplicateFound(System.Data.SqlClient.SqlConnection cn, string vAutoID, GEnum.SystemCode codeKey, int DocKey, int vConKey, int vDocGrpKey, short vCatNum)
        {
            string msgID = MsgID.Validation.DuplicateRecordIDParams;

            try
            {
                switch (codeKey)
                {
                    //Enquiry Document
                    case GEnum.SystemCode.Quotation:
                        ARQO objARQO = ARQO.New();
                        objARQO.DocID = vAutoID;
                        msgID += "%Document%Quotation";
                        return !(objARQO.Validation(cn, new ARQO.Criteria((int)codeKey, DocKey, vAutoID, 0), false));

                    //Request Document
                    case GEnum.SystemCode.Purchase_Plan:
                        APPN objAPPN = APPN.New();
                        objAPPN.DocID = vAutoID;
                        msgID += "%Document%Purchase Plan";
                        return !(objAPPN.Validation(cn, new APPN.Criteria((int)codeKey, DocKey, vAutoID, 0), false));

                    case GEnum.SystemCode.Purchase_Request:
                        APRQ objAPRQ = APRQ.New();
                        objAPRQ.DocID = vAutoID;
                        return !(objAPRQ.Validation(cn, new APRQ.Criteria((int)codeKey, DocKey, vAutoID, 0), false));

                    //Order Document              
                    case GEnum.SystemCode.Sales_Order:
                        ARSO objARSO = ARSO.New(out msgID);
                        objARSO.DocID = vAutoID;
                        return !(objARSO.Validation(cn, new ARSO.Criteria((int)codeKey, DocKey, vAutoID, 0), out msgID, false));

                    case GEnum.SystemCode.Purchase_Order:
                        APPO objAPPO = APPO.New();
                        objAPPO.DocID = vAutoID;
                        return !(objAPPO.Validation(cn, new APPO.Criteria((int)codeKey, DocKey, vAutoID, 0), false));

                    case GEnum.SystemCode.Order_Consignment:
                        CSCPO objCSCPO = CSCPO.New();
                        objCSCPO.DocID = vAutoID;
                        return !(objCSCPO.Validation(cn, new CSCPO.Criteria((int)codeKey, DocKey, vAutoID, 0), false));

                    //Order Adjustment Document  
                    case GEnum.SystemCode.Purchase_Order_Adjustment:
                    case GEnum.SystemCode.Consignment_Order_Adjustment:
                    case GEnum.SystemCode.Sales_Order_Adjustment:
                        APPJ objAPPJ = APPJ.New();
                        objAPPJ.DocID = vAutoID;
                        return !(objAPPJ.Validation(cn, new APPJ.Criteria((int)codeKey, DocKey, vAutoID, 0), false));

                    //Works Document
                    case GEnum.SystemCode.Works_Order:
                        //futurefeature
                        //REFWorks_Order objRefWorks_Order = REFWorks_Order.New(out msgID);
                        //objRefWorks_Order._works_orderID = vAutoID;
                        //return !(objRefWorks_Order.Validation(cn, new REFWorks_Order.Criteria(0, vAutoID), out msgID, true));
                        break;

                    //Delivery Document
                    case GEnum.SystemCode.Delivery_Order:
                        ARDO objARDO = ARDO.New();
                        objARDO.DocID = vAutoID;
                        return !(objARDO.Validation(cn, new ARDO.Criteria((int)codeKey, DocKey, vAutoID, 0), false));

                    case GEnum.SystemCode.Purchase_Delivery:
                        APPD objAPPD = APPD.New();
                        objAPPD.DocID = vAutoID;
                        return !(objAPPD.Validation(cn, new APPD.Criteria((int)codeKey, DocKey, vAutoID, vConKey), false));

                    case GEnum.SystemCode.Received_Consignment:
                        CSCPD objCSCPD = CSCPD.New();
                        objCSCPD.DocID = vAutoID;
                        return !(objCSCPD.Validation(cn, new CSCPD.Criteria((int)codeKey, DocKey, vAutoID, vConKey), false));

                    case GEnum.SystemCode.Issue_Consignment:
                    case GEnum.SystemCode.Return_Consignment:
                        CSCSI objCSCSI = CSCSI.New();
                        objCSCSI.DocID = vAutoID;
                        return !(objCSCSI.Validation(cn, new CSCSI.Criteria((int)codeKey, DocKey, vAutoID, 0), false));

                    //Transfer/Settlement Document
                    case GEnum.SystemCode.Packing_List:
                        ARPL objARPL = ARPL.New();
                        objARPL.DocID = vAutoID;
                        return !(objARPL.Validation(cn, new ARPL.Criteria((int)codeKey, DocKey, vAutoID, 0), false));

                    case GEnum.SystemCode.Consignment_Settlement:
                        CSCPS objCSCPS = CSCPS.New();
                        objCSCPS.DocID = vAutoID;
                        return !(objCSCPS.Validation(cn, new CSCPS.Criteria((int)codeKey, DocKey, vAutoID, 0), false));

                    //Invoice Document
                    case GEnum.SystemCode.Sales_Invoice:
                    case GEnum.SystemCode.Sales_Debit_Note:
                    case GEnum.SystemCode.Sales_Credit_Note:
                    case GEnum.SystemCode.Cash_Sale:
                    case GEnum.SystemCode.Cash_Debit_Note:
                    case GEnum.SystemCode.Cash_Credit_Note:
                        ARIV objARIV = ARIV.New();
                        objARIV.DocID = vAutoID;
                        return !(objARIV.Validation(cn, new ARIV.Criteria((int)codeKey, DocKey, vAutoID, 0), false));

                    case GEnum.SystemCode.Purchase_Invoice:
                    case GEnum.SystemCode.Purchase_Debit_Note:
                    case GEnum.SystemCode.Purchase_Credit_Note:
                        APBL objAPIV = APBL.New();
                        objAPIV.DocID = vAutoID;
                        return !(objAPIV.Validation(cn, new APBL.Criteria((int)codeKey, DocKey, vAutoID, vConKey), false));

                    //Adjustment Document
                    case GEnum.SystemCode.Purchase_Adjustment:
                        APADJ objAPADJ = APADJ.New();
                        objAPADJ.DocID = vAutoID;
                        return !(objAPADJ.Validation(cn, new APADJ.Criteria((int)codeKey, DocKey, vAutoID, 0), false));

                    case GEnum.SystemCode.Sales_Adjustment:
                    case GEnum.SystemCode.Cash_Adjustment:
                        ARADJ objARADJ = ARADJ.New(out msgID);
                        objARADJ.DocID = vAutoID;
                        return !(objARADJ.Validation(cn, new ARADJ.Criteria((int)codeKey, DocKey, vAutoID, 0), false));

                    //Payment Document
                    case GEnum.SystemCode.Payment_Issue:
                        APPY objAPPY = APPY.New();
                        objAPPY.DocID = vAutoID;
                        return !(objAPPY.Validation(cn, new APPY.Criteria((int)codeKey, DocKey, vAutoID, 0), false));

                    case GEnum.SystemCode.Payment_Received:
                    case GEnum.SystemCode.Cash_Payment_Received:
                        ARPY objARPY = ARPY.New(out msgID);
                        objARPY.DocID = vAutoID;
                        return !(objARPY.Validation(cn, new ARPY.Criteria((int)codeKey, DocKey, vAutoID, 0), false));

                    case GEnum.SystemCode.Contra:
                    case GEnum.SystemCode.Cash_Contra:
                        ARCT objARCT = ARCT.New();
                        objARCT.DocID = vAutoID;
                        return !(objARCT.Validation(cn, new ARCT.Criteria((int)codeKey, DocKey, vAutoID, 0), false));


                    //Inventory Document
                    case GEnum.SystemCode.Inventory_Adjustment:
                        INADJ objINADJ = INADJ.New();
                        objINADJ.DocID = vAutoID;
                        return !(objINADJ.Validation(cn, new INADJ.Criteria((int)codeKey, DocKey, vAutoID, 0), false));

                    case GEnum.SystemCode.Inventory_Production:
                        INMFN objINMFN = INMFN.New();
                        objINMFN.DocID = vAutoID;
                        return !(objINMFN.Validation(cn, new INMFN.Criteria((int)codeKey, DocKey, vAutoID, 0), false));

                    case GEnum.SystemCode.Inventory_Transfer:
                        INTRN objINTRN = INTRN.New();
                        objINTRN.DocID = vAutoID;
                        return !(objINTRN.Validation(cn, new INTRN.Criteria((int)codeKey, DocKey, vAutoID, 0), false));

                    //Account Document
                    case GEnum.SystemCode.Journal:
                        GLJNL objGLJNL = GLJNL.New();
                        objGLJNL.DocID = vAutoID;
                        return !(objGLJNL.Validation(cn, new GLJNL.Criteria((int)codeKey, DocKey, vAutoID, 0), false));

                    case GEnum.SystemCode.Deposit:

                        GLDP objGLDP = GLDP.New();
                        objGLDP.DocID = vAutoID;
                        return !(objGLDP.Validation(cn, new GLDP.Criteria((int)codeKey, DocKey, vAutoID, 0), false));


                    case GEnum.SystemCode.Bank_Revaluation:
                        GLRV objGLRV = GLRV.New();
                        objGLRV.DocID = vAutoID;
                        return !(objGLRV.Validation(cn, new GLRV.Criteria((int)codeKey, DocKey, vAutoID, 0), false));

                    //System
                    case GEnum.SystemCode.Document_Group:
                        REFDocGrp objRefDocument_Group = REFDocGrp.New();
                        objRefDocument_Group._docGrpID = vAutoID;
                        return !(objRefDocument_Group.Validation(cn, new REFDocGrp.Criteria(0, vAutoID), false));


                    case GEnum.SystemCode.General_List: //SysMsgText
                        SYSMsgListText objSYSMsgListText = SYSMsgListText.New();
                        objSYSMsgListText._msgValue = vAutoID;
                        return !(objSYSMsgListText.Validation(cn, new SYSMsgListText.Criteria(vDocGrpKey, vAutoID), false));

                    //Account
                    case GEnum.SystemCode.Account:
                        MSTAcc objRefAccount = MSTAcc.New();
                        objRefAccount._accID = vAutoID;
                        return !(objRefAccount.Validation(cn, new MSTAcc.Criteria(0, vAutoID), false));

                    case GEnum.SystemCode.Branch:
                        MSTAccBranch objMSTAccBranch = MSTAccBranch.New();
                        objMSTAccBranch._branchID = vAutoID;
                        return !(objMSTAccBranch.Validation(cn, new MSTAccBranch.Criteria(0, vAutoID, 0), false));

                    case GEnum.SystemCode.Department:
                        MSTAccDept objMSTAccDept = MSTAccDept.New();
                        objMSTAccDept._deptID = vAutoID;
                        return !(objMSTAccDept.Validation(cn, new MSTAccDept.Criteria(0, vAutoID, 0), false));
                        break;

                    case GEnum.SystemCode.Currency:
                        REFCurr objRefCurrency = REFCurr.New();
                        objRefCurrency._currID = vAutoID;
                        return !(objRefCurrency.Validation(cn, new REFCurr.Criteria(0, vAutoID), false));

                    case GEnum.SystemCode.Bank:
                        REFBank objRefBank = REFBank.New();
                        objRefBank._bankID = vAutoID;
                        return !(objRefBank.Validation(cn, new REFBank.Criteria(0, vAutoID), false));

                    case GEnum.SystemCode.Payment_Mode:
                        REFPayMode objRefPayment_Mode = REFPayMode.New();
                        objRefPayment_Mode._payModeID = vAutoID;
                        return !(objRefPayment_Mode.Validation(cn, new REFPayMode.Criteria(0, vAutoID), false));

                    case GEnum.SystemCode.Tax_Authority:
                        REFTaxA objRefTax_Authority = REFTaxA.New();
                        objRefTax_Authority._taxID = vAutoID;
                        return !(objRefTax_Authority.Validation(cn, new REFTaxA.Criteria(0, vAutoID), false));

                    case GEnum.SystemCode.Tax_Group:
                        REFTaxGrp objRefTax_Group = REFTaxGrp.New();
                        objRefTax_Group._taxGrpID = vAutoID;
                        return !(objRefTax_Group.Validation(cn, new REFTaxGrp.Criteria(0, vAutoID), false));

                    case GEnum.SystemCode.Overhead:
                        REFOverHead objRefOverhead = REFOverHead.New();
                        objRefOverhead._overHeadID = vAutoID;
                        return !(objRefOverhead.Validation(cn, new REFOverHead.Criteria(0, vAutoID), false));

                    case GEnum.SystemCode.Account_Group:
                        REFAccGrp objRefAccount_Group = REFAccGrp.New();
                        objRefAccount_Group._accGrpID = vAutoID;
                        return !(objRefAccount_Group.Validation(cn, new REFAccGrp.Criteria(0, vAutoID), false));

                    case GEnum.SystemCode.Sales_Representative:
                        MSTSalesRep objMSTSalesRep = MSTSalesRep.New();
                        objMSTSalesRep._emID = vAutoID;
                        return !(objMSTSalesRep.Validation(cn, new MSTSalesRep.Criteria(0, vAutoID), false));

                    case GEnum.SystemCode.Interest_Rate:
                        REFInterest objRefInterestRate = REFInterest.New();
                        objRefInterestRate._intID = vAutoID;
                        return !(objRefInterestRate.Validation(cn, new REFInterest.Criteria(0, vAutoID), false));

                    //Contact
                    case GEnum.SystemCode.Customer:
                        MSTCon objMSTConC = MSTCon.New();
                        objMSTConC._conID = vAutoID;
                        return !(objMSTConC.Validation(cn, new MSTCon.Criteria(0, vAutoID), true));
                        break;

                    case GEnum.SystemCode.Vendor:
                        MSTCon objMSTConV = MSTCon.New();
                        objMSTConV._conID = vAutoID;
                        return !(objMSTConV.Validation(cn, new MSTCon.Criteria(0, vAutoID), true));

                    case GEnum.SystemCode.Price_List:
                        MSTPriceList objMstPrice_List = MSTPriceList.New();
                        objMstPrice_List._priceID = vAutoID;
                        return !(objMstPrice_List.Validation(cn, new MSTPriceList.Criteria(0, vAutoID, 0), true));
                        break;
                    case GEnum.SystemCode.Payment_Term:
                        REFTerm objRefPayment_Term = REFTerm.New();
                        objRefPayment_Term._termID = vAutoID;
                        return !(objRefPayment_Term.Validation(cn, new REFTerm.Criteria(0, vAutoID), true));

                    case GEnum.SystemCode.Territory:
                        REFTerritory objRefTerritory = REFTerritory.New();
                        objRefTerritory._territoryID = vAutoID;
                        return !(objRefTerritory.Validation(cn, new REFTerritory.Criteria(0, vAutoID), true));
                        break;
                    case GEnum.SystemCode.Industry:
                        REFIndustry objRefIndustry = REFIndustry.New();
                        objRefIndustry._industryID = vAutoID;
                        return !(objRefIndustry.Validation(cn, new REFIndustry.Criteria(0, vAutoID), true));

                    case GEnum.SystemCode.Shipping_Mode:
                        REFShipVia objREFShipVia = REFShipVia.New();
                        objREFShipVia._shipViaID = vAutoID;
                        return !(objREFShipVia.Validation(cn, new REFShipVia.Criteria(0, vAutoID), true));
                        break;
                    case GEnum.SystemCode.Packing_Type:
                        REFPackingType objREFPackingType = REFPackingType.New();
                        objREFPackingType._packingTypeID = vAutoID;
                        return !(objREFPackingType.Validation(cn, new REFPackingType.Criteria(0, vAutoID, 0), true));

                    //Item 
                    case GEnum.SystemCode.Inventory:
                        MSTItm objMSTItm = MSTItm.New();
                        objMSTItm._itmID = vAutoID;
                        return !(objMSTItm.Validation(cn, new MSTItm.Criteria(0, 0, vAutoID), true));

                    case GEnum.SystemCode.Category:
                        REFCat objREFCat = REFCat.New();
                        objREFCat._catID = vAutoID;
                        return !(objREFCat.Validation(cn, new REFCat.Criteria(0, vAutoID, vCatNum, 0), true));

                    case GEnum.SystemCode.Brand:
                        REFBrand objRefBrand = REFBrand.New();
                        objRefBrand._brandID = vAutoID;
                        return !(objRefBrand.Validation(cn, new REFBrand.Criteria(0, vAutoID), true));

                    case GEnum.SystemCode.UOM:
                        REFUOM objRefUOM = REFUOM.New();
                        objRefUOM._uOMID = vAutoID;
                        return !(objRefUOM.Validation(cn, new REFUOM.Criteria(0, vAutoID, 0), true));

                    case GEnum.SystemCode.Color:
                        REFColor objRefColor = REFColor.New();
                        objRefColor._colorID = vAutoID;
                        return !(objRefColor.Validation(cn, new REFColor.Criteria(0, vAutoID), true));

                    case GEnum.SystemCode.Scale:
                        REFScale objRefScale = REFScale.New();
                        objRefScale._scaleID = vAutoID;
                        return !(objRefScale.Validation(cn, new REFScale.Criteria(0, vAutoID), true));

                    case GEnum.SystemCode.Location:
                        REFLoc objRefLocation = REFLoc.New();
                        objRefLocation._locID = vAutoID;
                        return !(objRefLocation.Validation(cn, new REFLoc.Criteria(0, vAutoID), true));

                    //Job
                    case GEnum.SystemCode.Job:
                        MSTJob objMSTJob = MSTJob.New();
                        objMSTJob._jobID = vAutoID;
                        return !(objMSTJob.Validation(cn, new MSTJob.Criteria(0, vAutoID), true));

                    case GEnum.SystemCode.Job_Cost_Type:
                        REFJobCostType objREFJobCostType = REFJobCostType.New();
                        objREFJobCostType._jobCostTypeID = vAutoID;
                        return !(objREFJobCostType.Validation(cn, new REFJobCostType.Criteria(0, vAutoID), true));
                        break;

                    case GEnum.SystemCode.Job_Phase:
                        REFJobPhase objREFJobPhase = REFJobPhase.New();
                        objREFJobPhase._jobPhaseID = vAutoID;
                        return !(objREFJobPhase.Validation(cn, new REFJobPhase.Criteria(0, vAutoID), true));

                    case GEnum.SystemCode.Job_Task:
                        REFJobTask objREFJobTask = REFJobTask.New();
                        objREFJobTask._jobTaskID = vAutoID;
                        return !(objREFJobTask.Validation(cn, new REFJobTask.Criteria(0, vAutoID), true));

                    case GEnum.SystemCode.Job_Group:
                        REFJobGrp objREFJobGroup = REFJobGrp.New();
                        objREFJobGroup._jobGrpID = vAutoID;
                        return !(objREFJobGroup.Validation(cn, new REFJobGrp.Criteria(0, vAutoID), true));

                    //Task
                    case GEnum.SystemCode.Alerts:
                        TASAlert objTASAlert = TASAlert.New();
                        objTASAlert._alertID = vAutoID;
                        return !(objTASAlert.Validation(cn, new TASAlert.Criteria(0, vAutoID), true));

                    case GEnum.SystemCode.To_Do:
                        //There are no ToDo_ID in the table design, so there is no need to check for duplicate ID
                        return false;//false indicate that there are no duplicate found
                }
            }
            catch (TAException tex)
            {
                if (tex.MsgID.Contains(MsgID.Validation.DuplicateRecord))
                    return true;
            }
            catch (Exception ex)
            {
                throw Error(ex);
            }
            return false;
        }
        public static bool DuplicateFound(string vAutoID, GEnum.SystemCode codeKey, int DocKey, int vConKey, int vDocGrpKey, short vCatNum)
        {
            string msgID = MsgID.Validation.DuplicateRecordIDParams;

            try
            {
                switch (codeKey)
                {
                    //Enquiry Document
                    case GEnum.SystemCode.Quotation:
                        ARQO objARQO = ARQO.New();
                        objARQO.DocID = vAutoID;
                        msgID += "%Document%Quotation";
                        return !(objARQO.Validation(new ARQO.Criteria((int)codeKey, DocKey, vAutoID, 0), false));

                    //Request Document
                    case GEnum.SystemCode.Purchase_Plan:
                        APPN objAPPN = APPN.New();
                        objAPPN.DocID = vAutoID;
                        msgID += "%Document%Purchase Plan";
                        return !(objAPPN.Validation(new APPN.Criteria((int)codeKey, DocKey, vAutoID, 0), false));

                    case GEnum.SystemCode.Purchase_Request:
                        APRQ objAPRQ = APRQ.New();
                        objAPRQ.DocID = vAutoID;
                        return !(objAPRQ.Validation(new APRQ.Criteria((int)codeKey, DocKey, vAutoID, 0), false));

                    //Order Document              
                    case GEnum.SystemCode.Sales_Order:
                        ARSO objARSO = ARSO.New(out msgID);
                        objARSO.DocID = vAutoID;
                        return !(objARSO.Validation(new ARSO.Criteria((int)codeKey, DocKey, vAutoID, 0), out msgID, false));

                    case GEnum.SystemCode.Purchase_Order:
                        APPO objAPPO = APPO.New();
                        objAPPO.DocID = vAutoID;
                        return !(objAPPO.Validation(new APPO.Criteria((int)codeKey, DocKey, vAutoID, 0), false));

                    case GEnum.SystemCode.Order_Consignment:
                        CSCPO objCSCPO = CSCPO.New();
                        objCSCPO.DocID = vAutoID;
                        return !(objCSCPO.Validation(new CSCPO.Criteria((int)codeKey, DocKey, vAutoID, 0), false));

                    //Order Adjustment Document  
                    case GEnum.SystemCode.Purchase_Order_Adjustment:
                    case GEnum.SystemCode.Consignment_Order_Adjustment:
                    case GEnum.SystemCode.Sales_Order_Adjustment:
                        APPJ objAPPJ = APPJ.New();
                        objAPPJ.DocID = vAutoID;
                        return !(objAPPJ.Validation(new APPJ.Criteria((int)codeKey, DocKey, vAutoID, 0), false));

                    //Works Document
                    case GEnum.SystemCode.Works_Order:
                        //futurefeature
                        //REFWorks_Order objRefWorks_Order = REFWorks_Order.New(out msgID);
                        //objRefWorks_Order._works_orderID = vAutoID;
                        //return !(objRefWorks_Order.Validation( new REFWorks_Order.Criteria(0, vAutoID), out msgID, true));
                        break;

                    //Delivery Document
                    case GEnum.SystemCode.Delivery_Order:
                        ARDO objARDO = ARDO.New();
                        objARDO.DocID = vAutoID;
                        return !(objARDO.Validation(new ARDO.Criteria((int)codeKey, DocKey, vAutoID, 0), false));

                    case GEnum.SystemCode.Purchase_Delivery:
                        APPD objAPPD = APPD.New();
                        objAPPD.DocID = vAutoID;
                        return !(objAPPD.Validation(new APPD.Criteria((int)codeKey, DocKey, vAutoID, vConKey), false));

                    case GEnum.SystemCode.Received_Consignment:
                        CSCPD objCSCPD = CSCPD.New();
                        objCSCPD.DocID = vAutoID;
                        return !(objCSCPD.Validation(new CSCPD.Criteria((int)codeKey, DocKey, vAutoID, vConKey), false));

                    case GEnum.SystemCode.Issue_Consignment:
                    case GEnum.SystemCode.Return_Consignment:
                        CSCSI objCSCSI = CSCSI.New();
                        objCSCSI.DocID = vAutoID;
                        return !(objCSCSI.Validation(new CSCSI.Criteria((int)codeKey, DocKey, vAutoID, 0), false));

                    //Transfer/Settlement Document
                    case GEnum.SystemCode.Packing_List:
                        ARPL objARPL = ARPL.New();
                        objARPL.DocID = vAutoID;
                        return !(objARPL.Validation(new ARPL.Criteria((int)codeKey, DocKey, vAutoID, 0), false));

                    case GEnum.SystemCode.Consignment_Settlement:
                        CSCPS objCSCPS = CSCPS.New();
                        objCSCPS.DocID = vAutoID;
                        return !(objCSCPS.Validation(new CSCPS.Criteria((int)codeKey, DocKey, vAutoID, 0), false));

                    //Invoice Document
                    case GEnum.SystemCode.Sales_Invoice:
                    case GEnum.SystemCode.Sales_Debit_Note:
                    case GEnum.SystemCode.Sales_Credit_Note:
                    case GEnum.SystemCode.Cash_Sale:
                    case GEnum.SystemCode.Cash_Debit_Note:
                    case GEnum.SystemCode.Cash_Credit_Note:
                        ARIV objARIV = ARIV.New();
                        objARIV.DocID = vAutoID;
                        return !(objARIV.Validation(new ARIV.Criteria((int)codeKey, DocKey, vAutoID, 0), false));

                    case GEnum.SystemCode.Purchase_Invoice:
                    case GEnum.SystemCode.Purchase_Debit_Note:
                    case GEnum.SystemCode.Purchase_Credit_Note:
                        APBL objAPIV = APBL.New();
                        objAPIV.DocID = vAutoID;
                        return !(objAPIV.Validation(new APBL.Criteria((int)codeKey, DocKey, vAutoID, vConKey), false));

                    //Adjustment Document
                    case GEnum.SystemCode.Purchase_Adjustment:
                        APADJ objAPADJ = APADJ.New();
                        objAPADJ.DocID = vAutoID;
                        return !(objAPADJ.Validation(new APADJ.Criteria((int)codeKey, DocKey, vAutoID, 0), false));

                    case GEnum.SystemCode.Sales_Adjustment:
                    case GEnum.SystemCode.Cash_Adjustment:
                        ARADJ objARADJ = ARADJ.New(out msgID);
                        objARADJ.DocID = vAutoID;
                        return !(objARADJ.Validation(new ARADJ.Criteria((int)codeKey, DocKey, vAutoID, 0), false));

                    //Payment Document
                    case GEnum.SystemCode.Payment_Issue:
                        APPY objAPPY = APPY.New();
                        objAPPY.DocID = vAutoID;
                        return !(objAPPY.Validation(new APPY.Criteria((int)codeKey, DocKey, vAutoID, 0), false));

                    case GEnum.SystemCode.Payment_Received:
                    case GEnum.SystemCode.Cash_Payment_Received:
                        ARPY objARPY = ARPY.New(out msgID);
                        objARPY.DocID = vAutoID;
                        return !(objARPY.Validation(new ARPY.Criteria((int)codeKey, DocKey, vAutoID, 0), false));

                    case GEnum.SystemCode.Contra:
                    case GEnum.SystemCode.Cash_Contra:
                        ARCT objARCT = ARCT.New();
                        objARCT.DocID = vAutoID;
                        return !(objARCT.Validation(new ARCT.Criteria((int)codeKey, DocKey, vAutoID, 0), false));


                    //Inventory Document
                    case GEnum.SystemCode.Inventory_Adjustment:
                        INADJ objINADJ = INADJ.New();
                        objINADJ.DocID = vAutoID;
                        return !(objINADJ.Validation(new INADJ.Criteria((int)codeKey, DocKey, vAutoID, 0), false));

                    case GEnum.SystemCode.Inventory_Production:
                        INMFN objINMFN = INMFN.New();
                        objINMFN.DocID = vAutoID;
                        return !(objINMFN.Validation(new INMFN.Criteria((int)codeKey, DocKey, vAutoID, 0), false));

                    case GEnum.SystemCode.Inventory_Transfer:
                        INTRN objINTRN = INTRN.New();
                        objINTRN.DocID = vAutoID;
                        return !(objINTRN.Validation(new INTRN.Criteria((int)codeKey, DocKey, vAutoID, 0), false));

                    //Account Document
                    case GEnum.SystemCode.Journal:
                        GLJNL objGLJNL = GLJNL.New();
                        objGLJNL.DocID = vAutoID;
                        return !(objGLJNL.Validation(new GLJNL.Criteria((int)codeKey, DocKey, vAutoID, 0), false));

                    case GEnum.SystemCode.Deposit:

                        GLDP objGLDP = GLDP.New();
                        objGLDP.DocID = vAutoID;
                        return !(objGLDP.Validation(new GLDP.Criteria((int)codeKey, DocKey, vAutoID, 0), false));


                    case GEnum.SystemCode.Bank_Revaluation:
                        GLRV objGLRV = GLRV.New();
                        objGLRV.DocID = vAutoID;
                        return !(objGLRV.Validation(new GLRV.Criteria((int)codeKey, DocKey, vAutoID, 0), false));

                    //System
                    case GEnum.SystemCode.Document_Group:
                        REFDocGrp objRefDocument_Group = REFDocGrp.New();
                        objRefDocument_Group._docGrpID = vAutoID;
                        return !(objRefDocument_Group.Validation(new REFDocGrp.Criteria(0, vAutoID), false));


                    case GEnum.SystemCode.General_List: //SysMsgText
                        SYSMsgListText objSYSMsgListText = SYSMsgListText.New();
                        objSYSMsgListText._msgValue = vAutoID;
                        return !(objSYSMsgListText.Validation(new SYSMsgListText.Criteria(vDocGrpKey, vAutoID), false));

                    //Account
                    case GEnum.SystemCode.Account:
                        MSTAcc objRefAccount = MSTAcc.New();
                        objRefAccount._accID = vAutoID;
                        return !(objRefAccount.Validation(new MSTAcc.Criteria(0, vAutoID), false));

                    case GEnum.SystemCode.Branch:
                        MSTAccBranch objMSTAccBranch = MSTAccBranch.New();
                        objMSTAccBranch._branchID = vAutoID;
                        return !(objMSTAccBranch.Validation(new MSTAccBranch.Criteria(0, vAutoID, 0), false));


                    case GEnum.SystemCode.Department:
                        MSTAccDept objMSTAccDept = MSTAccDept.New();
                        objMSTAccDept._deptID = vAutoID;
                        return !(objMSTAccDept.Validation(new MSTAccDept.Criteria(0, vAutoID, 0), false));
                        break;

                    case GEnum.SystemCode.Currency:
                        REFCurr objRefCurrency = REFCurr.New();
                        objRefCurrency._currID = vAutoID;
                        return !(objRefCurrency.Validation(new REFCurr.Criteria(0, vAutoID), false));

                    case GEnum.SystemCode.Bank:
                        REFBank objRefBank = REFBank.New();
                        objRefBank._bankID = vAutoID;
                        return !(objRefBank.Validation(new REFBank.Criteria(0, vAutoID), false));

                    case GEnum.SystemCode.Payment_Mode:
                        REFPayMode objRefPayment_Mode = REFPayMode.New();
                        objRefPayment_Mode._payModeID = vAutoID;
                        return !(objRefPayment_Mode.Validation(new REFPayMode.Criteria(0, vAutoID), false));

                    case GEnum.SystemCode.Tax_Authority:
                        REFTaxA objRefTax_Authority = REFTaxA.New();
                        objRefTax_Authority._taxID = vAutoID;
                        return !(objRefTax_Authority.Validation(new REFTaxA.Criteria(0, vAutoID), false));

                    case GEnum.SystemCode.Tax_Group:
                        REFTaxGrp objRefTax_Group = REFTaxGrp.New();
                        objRefTax_Group._taxGrpID = vAutoID;
                        return !(objRefTax_Group.Validation(new REFTaxGrp.Criteria(0, vAutoID), false));

                    case GEnum.SystemCode.Overhead:
                        REFOverHead objRefOverhead = REFOverHead.New();
                        objRefOverhead._overHeadID = vAutoID;
                        return !(objRefOverhead.Validation(new REFOverHead.Criteria(0, vAutoID), false));

                    case GEnum.SystemCode.Account_Group:
                        REFAccGrp objRefAccount_Group = REFAccGrp.New();
                        objRefAccount_Group._accGrpID = vAutoID;
                        return !(objRefAccount_Group.Validation(new REFAccGrp.Criteria(0, vAutoID), false));

                    case GEnum.SystemCode.Sales_Representative:
                        MSTSalesRep objMSTSalesRep = MSTSalesRep.New();
                        objMSTSalesRep._emID = vAutoID;
                        return !(objMSTSalesRep.Validation(new MSTSalesRep.Criteria(0, vAutoID), false));

                    case GEnum.SystemCode.Interest_Rate:
                        REFInterest objRefInterestRate = REFInterest.New();
                        objRefInterestRate._intID = vAutoID;
                        return !(objRefInterestRate.Validation(new REFInterest.Criteria(0, vAutoID), false));

                    //Contact
                    case GEnum.SystemCode.Customer:
                        MSTCon objMSTConC = MSTCon.New();
                        objMSTConC._conID = vAutoID;
                        return !(objMSTConC.Validation(new MSTCon.Criteria(0, vAutoID), true));
                        break;

                    case GEnum.SystemCode.Vendor:
                        MSTCon objMSTConV = MSTCon.New();
                        objMSTConV._conID = vAutoID;
                        return !(objMSTConV.Validation(new MSTCon.Criteria(0, vAutoID), true));

                    case GEnum.SystemCode.Price_List:
                        MSTPriceList objMstPrice_List = MSTPriceList.New();
                        objMstPrice_List._priceID = vAutoID;
                        return !(objMstPrice_List.Validation(new MSTPriceList.Criteria(0, vAutoID, 0), true));
                        break;
                    case GEnum.SystemCode.Payment_Term:
                        REFTerm objRefPayment_Term = REFTerm.New();
                        objRefPayment_Term._termID = vAutoID;
                        return !(objRefPayment_Term.Validation(new REFTerm.Criteria(0, vAutoID), true));

                    case GEnum.SystemCode.Territory:
                        REFTerritory objRefTerritory = REFTerritory.New();
                        objRefTerritory._territoryID = vAutoID;
                        return !(objRefTerritory.Validation(new REFTerritory.Criteria(0, vAutoID), true));
                        break;
                    case GEnum.SystemCode.Industry:
                        REFIndustry objRefIndustry = REFIndustry.New();
                        objRefIndustry._industryID = vAutoID;
                        return !(objRefIndustry.Validation(new REFIndustry.Criteria(0, vAutoID), true));

                    case GEnum.SystemCode.Shipping_Mode:
                        REFShipVia objREFShipVia = REFShipVia.New();
                        objREFShipVia._shipViaID = vAutoID;
                        return !(objREFShipVia.Validation(new REFShipVia.Criteria(0, vAutoID), true));
                        break;
                    case GEnum.SystemCode.Packing_Type:
                        REFPackingType objREFPackingType = REFPackingType.New();
                        objREFPackingType._packingTypeID = vAutoID;
                        return !(objREFPackingType.Validation(new REFPackingType.Criteria(0, vAutoID, 0), true));

                    //Item 
                    case GEnum.SystemCode.Inventory:
                        MSTItm objMSTItm = MSTItm.New();
                        objMSTItm._itmID = vAutoID;
                        return !(objMSTItm.Validation(new MSTItm.Criteria(0, 0, vAutoID), true));

                    case GEnum.SystemCode.Category:
                        REFCat objREFCat = REFCat.New();
                        objREFCat._catID = vAutoID;
                        return !(objREFCat.Validation(new REFCat.Criteria(0, vAutoID, vCatNum, 0), true));

                    case GEnum.SystemCode.Brand:
                        REFBrand objRefBrand = REFBrand.New();
                        objRefBrand._brandID = vAutoID;
                        return !(objRefBrand.Validation(new REFBrand.Criteria(0, vAutoID), true));

                    case GEnum.SystemCode.UOM:
                        REFUOM objRefUOM = REFUOM.New();
                        objRefUOM._uOMID = vAutoID;
                        return !(objRefUOM.Validation(new REFUOM.Criteria(0, vAutoID, 0), true));

                    case GEnum.SystemCode.Color:
                        REFColor objRefColor = REFColor.New();
                        objRefColor._colorID = vAutoID;
                        return !(objRefColor.Validation(new REFColor.Criteria(0, vAutoID), true));

                    case GEnum.SystemCode.Scale:
                        REFScale objRefScale = REFScale.New();
                        objRefScale._scaleID = vAutoID;
                        return !(objRefScale.Validation(new REFScale.Criteria(0, vAutoID), true));

                    case GEnum.SystemCode.Location:
                        REFLoc objRefLocation = REFLoc.New();
                        objRefLocation._locID = vAutoID;
                        return !(objRefLocation.Validation(new REFLoc.Criteria(0, vAutoID), true));

                    //Job
                    case GEnum.SystemCode.Job:
                        MSTJob objMSTJob = MSTJob.New();
                        objMSTJob._jobID = vAutoID;
                        return !(objMSTJob.Validation(new MSTJob.Criteria(0, vAutoID), true));

                    case GEnum.SystemCode.Job_Cost_Type:
                        REFJobCostType objREFJobCostType = REFJobCostType.New();
                        objREFJobCostType._jobCostTypeID = vAutoID;
                        return !(objREFJobCostType.Validation(new REFJobCostType.Criteria(0, vAutoID), true));
                        break;

                    case GEnum.SystemCode.Job_Phase:
                        REFJobPhase objREFJobPhase = REFJobPhase.New();
                        objREFJobPhase._jobPhaseID = vAutoID;
                        return !(objREFJobPhase.Validation(new REFJobPhase.Criteria(0, vAutoID), true));

                    case GEnum.SystemCode.Job_Task:
                        REFJobTask objREFJobTask = REFJobTask.New();
                        objREFJobTask._jobTaskID = vAutoID;
                        return !(objREFJobTask.Validation(new REFJobTask.Criteria(0, vAutoID), true));

                    case GEnum.SystemCode.Job_Group:
                        REFJobGrp objREFJobGroup = REFJobGrp.New();
                        objREFJobGroup._jobGrpID = vAutoID;
                        return !(objREFJobGroup.Validation(new REFJobGrp.Criteria(0, vAutoID), true));

                    //Task
                    case GEnum.SystemCode.Alerts:
                        TASAlert objTASAlert = TASAlert.New();
                        objTASAlert._alertID = vAutoID;
                        return !(objTASAlert.Validation(new TASAlert.Criteria(0, vAutoID), true));

                    case GEnum.SystemCode.To_Do:
                        //There are no ToDo_ID in the table design, so there is no need to check for duplicate ID
                        return false;//false indicate that there are no duplicate found
                }
            }
            catch (TAException tex)
            {
                if (tex.MsgID.Contains(MsgID.Validation.DuplicateRecord))
                    return true;
            }
            catch (Exception ex)
            {
                throw Error(ex);
            }
            return false;
        }
        public static bool DuplicateFound(string vAutoID, GEnum.SystemCode codeKey, int vConKey, int vDocGrpKey, short vCatNum)
        {
            string msgID = MsgID.Validation.DuplicateRecordIDParams;

            try
            {
                switch (codeKey)
                {
                    //Enquiry Document
                    case GEnum.SystemCode.Quotation:
                        ARQO objARQO = ARQO.New();
                        objARQO.DocID = vAutoID;
                        msgID += "%Document%Quotation";
                        return !(objARQO.Validation(new ARQO.Criteria((int)codeKey, 0, vAutoID, 0), true));

                    //Request Document
                    case GEnum.SystemCode.Purchase_Plan:
                        APPN objAPPN = APPN.New();
                        objAPPN.DocID = vAutoID;
                        return !(objAPPN.Validation(new APPN.Criteria((int)codeKey, 0, vAutoID, 0), true));

                    case GEnum.SystemCode.Purchase_Request:
                        APRQ objAPRQ = APRQ.New();
                        objAPRQ.DocID = vAutoID;
                        return !(objAPRQ.Validation(new APRQ.Criteria((int)codeKey, 0, vAutoID, 0), true));

                    //Order Document              
                    case GEnum.SystemCode.Sales_Order:
                        ARSO objARSO = ARSO.New(out msgID);
                        objARSO.DocID = vAutoID;
                        return !(objARSO.Validation(new ARSO.Criteria((int)codeKey, 0, vAutoID, 0), out msgID, true));

                    case GEnum.SystemCode.Purchase_Order:
                        APPO objAPPO = APPO.New();
                        objAPPO.DocID = vAutoID;
                        return !(objAPPO.Validation(new APPO.Criteria((int)codeKey, 0, vAutoID, 0), true));

                    case GEnum.SystemCode.Order_Consignment:
                        CSCPO objCSCPO = CSCPO.New();
                        objCSCPO.DocID = vAutoID;
                        return !(objCSCPO.Validation(new CSCPO.Criteria((int)codeKey, 0, vAutoID, 0), true));

                    //Order Adjustment Document  
                    case GEnum.SystemCode.Purchase_Order_Adjustment:
                    case GEnum.SystemCode.Consignment_Order_Adjustment:
                    case GEnum.SystemCode.Sales_Order_Adjustment:
                        APPJ objAPPJ = APPJ.New();
                        objAPPJ.DocID = vAutoID;
                        return !(objAPPJ.Validation(new APPJ.Criteria((int)codeKey, 0, vAutoID, 0), true));

                    //Works Document
                    case GEnum.SystemCode.Works_Order:
                    //futurefeature
                    //REFWorks_Order objRefWorks_Order = REFWorks_Order.New(out msgID);
                    //objRefWorks_Order._works_orderID = vAutoID;
                    //return !(objRefWorks_Order.Validation(cn, new REFWorks_Order.Criteria(0, vAutoID), out msgID, true));

                    //Delivery Document
                    case GEnum.SystemCode.Delivery_Order:
                        ARDO objARDO = ARDO.New();
                        objARDO.DocID = vAutoID;
                        return !(objARDO.Validation(new ARDO.Criteria((int)codeKey, 0, vAutoID, 0), true));

                    case GEnum.SystemCode.Purchase_Delivery:
                        APPD objAPPD = APPD.New();
                        objAPPD.DocID = vAutoID;
                        return !(objAPPD.Validation(new APPD.Criteria((int)codeKey, 0, vAutoID, vConKey), true));

                    case GEnum.SystemCode.Received_Consignment:
                        CSCPD objCSCPD = CSCPD.New();
                        objCSCPD.DocID = vAutoID;
                        return !(objCSCPD.Validation(new CSCPD.Criteria((int)codeKey, 0, vAutoID, vConKey), true));

                    case GEnum.SystemCode.Issue_Consignment:
                    case GEnum.SystemCode.Return_Consignment:
                        CSCSI objCSCSI = CSCSI.New();
                        objCSCSI.DocID = vAutoID;
                        return !(objCSCSI.Validation(new CSCSI.Criteria((int)codeKey, 0, vAutoID, 0), true));

                    //Transfer/Settlement Document
                    case GEnum.SystemCode.Packing_List:
                        ARPL objARPL = ARPL.New();
                        objARPL.DocID = vAutoID;
                        return !(objARPL.Validation(new ARPL.Criteria((int)codeKey, 0, vAutoID, 0), true));

                    case GEnum.SystemCode.Consignment_Settlement:
                        CSCPS objCSCPS = CSCPS.New();
                        objCSCPS.DocID = vAutoID;
                        return !(objCSCPS.Validation(new CSCPS.Criteria((int)codeKey, 0, vAutoID, 0), true));

                    //Invoice Document
                    case GEnum.SystemCode.Sales_Invoice:
                    case GEnum.SystemCode.Sales_Debit_Note:
                    case GEnum.SystemCode.Sales_Credit_Note:
                    case GEnum.SystemCode.Cash_Sale:
                    case GEnum.SystemCode.Cash_Debit_Note:
                    case GEnum.SystemCode.Cash_Credit_Note:
                        ARIV objARIV = ARIV.New();
                        objARIV.DocID = vAutoID;
                        return !(objARIV.Validation(new ARIV.Criteria((int)codeKey, 0, vAutoID, 0), true));

                    case GEnum.SystemCode.Purchase_Invoice:
                    case GEnum.SystemCode.Purchase_Debit_Note:
                    case GEnum.SystemCode.Purchase_Credit_Note:
                        APBL objAPIV = APBL.New();
                        objAPIV.DocID = vAutoID;
                        return !(objAPIV.Validation(new APBL.Criteria((int)codeKey, 0, vAutoID, vConKey), true));//new APBL.Criteria((int)codeKey, 0, vAutoID, vConKey, 0) --old code

                    //Adjustment Document
                    case GEnum.SystemCode.Purchase_Adjustment:
                        APADJ objAPADJ = APADJ.New();
                        objAPADJ.DocID = vAutoID;
                        return !(objAPADJ.Validation(new APADJ.Criteria((int)codeKey, 0, vAutoID, 0), true));

                    case GEnum.SystemCode.Sales_Adjustment:
                    case GEnum.SystemCode.Cash_Adjustment:
                        ARADJ objARADJ = ARADJ.New(out msgID);
                        objARADJ.DocID = vAutoID;
                        return !(objARADJ.Validation(new ARADJ.Criteria((int)codeKey, 0, vAutoID, 0), true));

                    //Payment Document
                    case GEnum.SystemCode.Payment_Issue:
                        APPY objAPPY = APPY.New();
                        objAPPY.DocID = vAutoID;
                        return !(objAPPY.Validation(new APPY.Criteria((int)codeKey, 0, vAutoID, 0), true));

                    case GEnum.SystemCode.Payment_Received:
                    case GEnum.SystemCode.Cash_Payment_Received:
                        ARPY objARPY = ARPY.New(out msgID);
                        objARPY.DocID = vAutoID;
                        return !(objARPY.Validation(new ARPY.Criteria((int)codeKey, 0, vAutoID, 0), true));

                    case GEnum.SystemCode.Contra:
                    case GEnum.SystemCode.Cash_Contra:
                        ARCT objARCT = ARCT.New();
                        objARCT.DocID = vAutoID;
                        return !(objARCT.Validation(new ARCT.Criteria((int)codeKey, 0, vAutoID, 0), true));

                    //Inventory Document
                    case GEnum.SystemCode.Inventory_Adjustment:
                        INADJ objINADJ = INADJ.New();
                        objINADJ.DocID = vAutoID;
                        return !(objINADJ.Validation(new INADJ.Criteria((int)codeKey, 0, vAutoID, 0), true));

                    case GEnum.SystemCode.Inventory_Production:
                        INMFN objINMFN = INMFN.New();
                        objINMFN.DocID = vAutoID;
                        return !(objINMFN.Validation(new INMFN.Criteria((int)codeKey, 0, vAutoID, 0), true));

                    case GEnum.SystemCode.Inventory_Transfer:
                        INTRN objINTRN = INTRN.New();
                        objINTRN.DocID = vAutoID;
                        return !(objINTRN.Validation(new INTRN.Criteria((int)codeKey, 0, vAutoID, 0), true));

                    //Account Document
                    case GEnum.SystemCode.Journal:
                        GLJNL objGLJNL = GLJNL.New();
                        objGLJNL.DocID = vAutoID;
                        return !(objGLJNL.Validation(new GLJNL.Criteria((int)codeKey, 0, vAutoID, 0), true));

                    case GEnum.SystemCode.Deposit:
                        GLDP objGLDP = GLDP.New();
                        objGLDP.DocID = vAutoID;
                        return !(objGLDP.Validation(new GLDP.Criteria((int)codeKey, 0, vAutoID, 0), true));

                    case GEnum.SystemCode.Bank_Revaluation:
                        GLRV objGLRV = GLRV.New();
                        objGLRV.DocID = vAutoID;
                        return !(objGLRV.Validation(new GLRV.Criteria((int)codeKey, 0, vAutoID, 0), true));

                    //System
                    case GEnum.SystemCode.Document_Group:
                        REFDocGrp objRefDocument_Group = REFDocGrp.New();
                        objRefDocument_Group._docGrpID = vAutoID;
                        return !(objRefDocument_Group.Validation(new REFDocGrp.Criteria(0, vAutoID), true));

                    case GEnum.SystemCode.General_List: //SysMsgText
                        SYSMsgListText objSYSMsgListText = SYSMsgListText.New();
                        objSYSMsgListText._msgValue = vAutoID;
                        return !(objSYSMsgListText.Validation(new SYSMsgListText.Criteria(vDocGrpKey, vAutoID), true));

                    //Account
                    case GEnum.SystemCode.Account:
                        MSTAcc objRefAccount = MSTAcc.New();
                        objRefAccount._accID = vAutoID;
                        return !(objRefAccount.Validation(new MSTAcc.Criteria(0, vAutoID), true));

                    case GEnum.SystemCode.Branch:
                        MSTAccBranch objMSTAccBranch = MSTAccBranch.New();
                        objMSTAccBranch._branchID = vAutoID;
                        return !(objMSTAccBranch.Validation(new MSTAccBranch.Criteria(0, vAutoID, 0), true));

                    case GEnum.SystemCode.Department:
                        MSTAccDept objMSTAccDept = MSTAccDept.New();
                        objMSTAccDept._deptID = vAutoID;
                        return !(objMSTAccDept.Validation(new MSTAccDept.Criteria(0, vAutoID, 0), true));

                    case GEnum.SystemCode.Currency:
                        REFCurr objRefCurrency = REFCurr.New();
                        objRefCurrency._currID = vAutoID;
                        return !(objRefCurrency.Validation(new REFCurr.Criteria(0, vAutoID), true));

                    case GEnum.SystemCode.Bank:
                        REFBank objRefBank = REFBank.New();
                        objRefBank._bankID = vAutoID;
                        return !(objRefBank.Validation(new REFBank.Criteria(0, vAutoID), true));

                    case GEnum.SystemCode.Payment_Mode:
                        REFPayMode objRefPayment_Mode = REFPayMode.New();
                        objRefPayment_Mode._payModeID = vAutoID;
                        return !(objRefPayment_Mode.Validation(new REFPayMode.Criteria(0, vAutoID), true));

                    case GEnum.SystemCode.Tax_Authority:
                        REFTaxA objRefTax_Authority = REFTaxA.New();
                        objRefTax_Authority._taxID = vAutoID;
                        return !(objRefTax_Authority.Validation(new REFTaxA.Criteria(0, vAutoID), true));

                    case GEnum.SystemCode.Tax_Group:
                        REFTaxGrp objRefTax_Group = REFTaxGrp.New();
                        objRefTax_Group._taxGrpID = vAutoID;
                        return !(objRefTax_Group.Validation(new REFTaxGrp.Criteria(0, vAutoID), true));

                    case GEnum.SystemCode.Overhead:
                        REFOverHead objRefOverhead = REFOverHead.New();
                        objRefOverhead._overHeadID = vAutoID;
                        return !(objRefOverhead.Validation(new REFOverHead.Criteria(0, vAutoID), true));

                    case GEnum.SystemCode.Account_Group:
                        REFAccGrp objRefAccount_Group = REFAccGrp.New();
                        objRefAccount_Group._accGrpID = vAutoID;
                        return !(objRefAccount_Group.Validation(new REFAccGrp.Criteria(0, vAutoID), true));

                    case GEnum.SystemCode.Sales_Representative:
                        MSTSalesRep objMSTSalesRep = MSTSalesRep.New();
                        objMSTSalesRep._emID = vAutoID;
                        return !(objMSTSalesRep.Validation(new MSTSalesRep.Criteria(0, vAutoID), true));

                    case GEnum.SystemCode.Interest_Rate:
                        REFInterest objRefInterestRate = REFInterest.New();
                        objRefInterestRate._intID = vAutoID;
                        return !(objRefInterestRate.Validation(new REFInterest.Criteria(0, vAutoID), true));

                    //Contact
                    case GEnum.SystemCode.Customer:
                        MSTCon objMSTConC = MSTCon.New();
                        objMSTConC._conID = vAutoID;
                        return !(objMSTConC.Validation(new MSTCon.Criteria(0, vAutoID), true));

                    case GEnum.SystemCode.Vendor:
                        MSTCon objMSTConV = MSTCon.New();
                        objMSTConV._conID = vAutoID;
                        return !(objMSTConV.Validation(new MSTCon.Criteria(0, vAutoID), true));

                    case GEnum.SystemCode.Price_List:
                        MSTPriceList objMstPrice_List = MSTPriceList.New();
                        objMstPrice_List._priceID = vAutoID;
                        return !(objMstPrice_List.Validation(new MSTPriceList.Criteria(0, vAutoID, 0), true));

                    case GEnum.SystemCode.Payment_Term:
                        REFTerm objRefPayment_Term = REFTerm.New();
                        objRefPayment_Term._termID = vAutoID;
                        return !(objRefPayment_Term.Validation(new REFTerm.Criteria(0, vAutoID), true));

                    case GEnum.SystemCode.Territory:
                        REFTerritory objRefTerritory = REFTerritory.New();
                        objRefTerritory._territoryID = vAutoID;
                        return !(objRefTerritory.Validation(new REFTerritory.Criteria(0, vAutoID), true));

                    case GEnum.SystemCode.Industry:
                        REFIndustry objRefIndustry = REFIndustry.New();
                        objRefIndustry._industryID = vAutoID;
                        return !(objRefIndustry.Validation(new REFIndustry.Criteria(0, vAutoID), true));

                    case GEnum.SystemCode.Shipping_Mode:
                        REFShipVia objREFShipVia = REFShipVia.New();
                        objREFShipVia._shipViaID = vAutoID;
                        return !(objREFShipVia.Validation(new REFShipVia.Criteria(0, vAutoID), true));

                    case GEnum.SystemCode.Packing_Type:
                        REFPackingType objREFPackingType = REFPackingType.New();
                        objREFPackingType._packingTypeID = vAutoID;
                        return !(objREFPackingType.Validation(new REFPackingType.Criteria(0, vAutoID, 0), true));

                    //Item 
                    case GEnum.SystemCode.Inventory:
                        MSTItm objMSTItm = MSTItm.New();
                        objMSTItm._itmID = vAutoID;
                        return !(objMSTItm.Validation(new MSTItm.Criteria(0, vAutoID), true));

                    case GEnum.SystemCode.Category:
                        REFCat objREFCat = REFCat.New();
                        objREFCat._catID = vAutoID;
                        return !(objREFCat.Validation(new REFCat.Criteria(0, vAutoID, vCatNum, 0), true));

                    case GEnum.SystemCode.Brand:
                        REFBrand objRefBrand = REFBrand.New();
                        objRefBrand._brandID = vAutoID;
                        return !(objRefBrand.Validation(new REFBrand.Criteria(0, vAutoID), true));

                    case GEnum.SystemCode.UOM:
                        REFUOM objRefUOM = REFUOM.New();
                        objRefUOM._uOMID = vAutoID;
                        return !(objRefUOM.Validation(new REFUOM.Criteria(0, vAutoID, 0), true));

                    case GEnum.SystemCode.Color:
                        REFColor objRefColor = REFColor.New();
                        objRefColor._colorID = vAutoID;
                        return !(objRefColor.Validation(new REFColor.Criteria(0, vAutoID), true));

                    case GEnum.SystemCode.Scale:
                        REFScale objRefScale = REFScale.New();
                        objRefScale._scaleID = vAutoID;
                        return !(objRefScale.Validation(new REFScale.Criteria(0, vAutoID), true));

                    case GEnum.SystemCode.Location:
                        REFLoc objRefLocation = REFLoc.New();
                        objRefLocation._locID = vAutoID;
                        return !(objRefLocation.Validation(new REFLoc.Criteria(0, vAutoID), true));

                    //Job
                    case GEnum.SystemCode.Job:
                        MSTJob objMSTJob = MSTJob.New();
                        objMSTJob._jobID = vAutoID;
                        return !(objMSTJob.Validation(new MSTJob.Criteria(0, vAutoID), true));

                    case GEnum.SystemCode.Job_Cost_Type:
                        REFJobCostType objREFJobCostType = REFJobCostType.New();
                        objREFJobCostType._jobCostTypeID = vAutoID;
                        return !(objREFJobCostType.Validation(new REFJobCostType.Criteria(0, vAutoID), true));

                    case GEnum.SystemCode.Job_Phase:
                        REFJobPhase objREFJobPhase = REFJobPhase.New();
                        objREFJobPhase._jobPhaseID = vAutoID;
                        return !(objREFJobPhase.Validation(new REFJobPhase.Criteria(0, vAutoID), true));

                    case GEnum.SystemCode.Job_Task:
                        REFJobTask objREFJobTask = REFJobTask.New();
                        objREFJobTask._jobTaskID = vAutoID;
                        return !(objREFJobTask.Validation(new REFJobTask.Criteria(0, vAutoID), true));

                    case GEnum.SystemCode.Job_Group:
                        REFJobGrp objREFJobGroup = REFJobGrp.New();
                        objREFJobGroup._jobGrpID = vAutoID;
                        return !(objREFJobGroup.Validation(new REFJobGrp.Criteria(0, vAutoID), true));

                    //Task
                    case GEnum.SystemCode.Alerts:
                        TASAlert objTASAlert = TASAlert.New();
                        objTASAlert._alertID = vAutoID;
                        return !(objTASAlert.Validation(new TASAlert.Criteria(0, vAutoID), true));

                    case GEnum.SystemCode.To_Do:
                        //There are no ToDo_ID in the table design, so there is no need to check for duplicate ID
                        return false;//false indicate that there are no duplicate found
                }
                return false;
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
