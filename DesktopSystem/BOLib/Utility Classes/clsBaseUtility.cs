using System;
using System.Data;
using System.Data.SqlClient;
using Csla;
using Csla.Data;
using System.Collections.Generic;
using System.IO;
using System.Resources;
using System.Collections;
using System.Windows.Forms;
using System.ComponentModel;
using System.Linq;
//using TAUtil;

namespace BOLib
{
    [Serializable()]
    public class BaseUtility : CommandBase
    {
        public static string Validation(bool throwIfError, string processOK, bool failOnError, out string msgID, object propValue, string propName, GEnum.DataType? propDataType, GEnum.Require? isRequire, int? strMaxLength,
            GEnum.CompareOperator? compareOperator, object compareValue, object minValue, object maxValue, UINotifierEventArgs e, SqlConnection cn)
        {
            try
            {
                bool processResult = false;
                msgID = string.Empty;

                if (failOnError)
                {
                    if (processOK == GVar.gcPass)
                    {
                        processResult = Validation(out msgID, propValue, propName, propDataType, isRequire, strMaxLength, compareOperator, compareValue, minValue, maxValue);
                    }
                    else
                        return GVar.gcCancel;
                }
                else
                {
                    processResult = Validation(out msgID, propValue, propName, propDataType, isRequire, strMaxLength, compareOperator, compareValue, minValue, maxValue);
                }

                if (!processResult)
                {
                    GFunc.NE(msgID, MsgID.Common.ValidationFail);
                    if (throwIfError)
                    {
                        SysMessageUtility.Get(cn, msgID);
                        throw new Exception(msgID);
                    }
                    else
                    {
                        if (cn != null && e != null)   //May added on 4 Nov 2010 for code reuse purpose, since the individual cell checking doesn't need to do get msgValue             
                            e.PropertyMessage.Add(propName, SysMessageUtility.Get(cn, msgID));
                        return GVar.gcCancel;
                    }
                }
                else
                {
                    return GVar.gcPass;
                }
            }
            catch (Exception ex)
            {
                throw Error(ex);
            }
        }
        public static string Validation(string processOK, bool failOnError, out string msgID, object propValue, string propName, GEnum.DataType? propDataType, GEnum.Require? isRequire, int? strMaxLength,
            GEnum.CompareOperator? compareOperator, object compareValue, object minValue, object maxValue, UINotifierEventArgs e, SqlConnection cn)
        {
            try
            {
                bool processResult = false;
                msgID = string.Empty;

                if (failOnError)
                {
                    if (processOK == GVar.gcPass)
                    {
                        processResult = Validation(out msgID, propValue, propName, propDataType, isRequire, strMaxLength, compareOperator, compareValue, minValue, maxValue);
                    }
                    else
                        return GVar.gcCancel;
                }
                else
                {
                    processResult = Validation(out msgID, propValue, propName, propDataType, isRequire, strMaxLength, compareOperator, compareValue, minValue, maxValue);
                }

                if (!processResult)
                {
                    GFunc.NE(msgID, MsgID.Common.ValidationFail);
                    if (cn != null && e != null)   //May added on 4 Nov 2010 for code reuse purpose, since the individual cell checking doesn't need to do get msgValue             
                        e.PropertyMessage.Add(propName, SysMessageUtility.Get(cn, msgID));
                    return GVar.gcCancel;
                }
                else
                {
                    return GVar.gcPass;
                }
            }
            catch (Exception ex)
            {
                throw Error(ex);
            }
        }
        public static void Validation(object propValue, string propName, string FieldToValidate, GEnum.DataType? propDataType, GEnum.Require? isRequire, int? strMaxLength,
            GEnum.CompareOperator? compareOperator, object compareValue, object minValue, object maxValue, ref bool processOK, bool failOnError, UINotifierEventArgs e)
        {
            //When FieldToValidate = string.empty mean to validate all fields
            string msgID = string.Empty;
            try
            {
                if (FieldToValidate == propName)
                {
                    if (failOnError && processOK == false)
                        return;

                    if (Validation(out msgID, propValue, propName, propDataType, isRequire, strMaxLength, compareOperator, compareValue, minValue, maxValue) == false)
                    {
                        GFunc.NE(msgID, MsgID.Common.ValidationFail);
                        if (e != null)   //May added on 4 Nov 2010 for code reuse purpose, since the individual cell checking doesn't need to do get msgValue             
                            e.PropertyMessage.Add(propName, msgID);

                        processOK = false;
                    }
                }
            }
            catch (Exception ex)
            {
                throw Error(ex);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="msgID"></param>
        /// <param name="propValue"></param>
        /// <param name="propName"></param>
        /// <param name="propDataType"></param>
        /// <param name="isRequire"></param>
        /// <param name="strMaxLength">Maximum Length of the property, only used in string data type</param>
        /// <param name="compareOperator"></param>
        /// <param name="compareValue"></param>
        /// <param name="minValue"></param>
        /// <param name="maxValue"></param>
        /// <returns></returns>
        public static bool Validation(out string msgID, object propValue, string propName, GEnum.DataType? propDataType, GEnum.Require? isRequire, int? strMaxLength, GEnum.CompareOperator? compareOperator, object compareValue, object minValue, object maxValue)
        {
            bool isValidation = true;
            msgID = MsgID.Common.UnableToValidate;

            #region Commented
            //// Property Value Validation
            //if (GFunc.IsNE(propValue))
            //{
            //    if (propValue == null || propValue==DBNull.Value)
            //    {
            //        Added By May on 1-April 2009
            //        if (!GFunc.IsNE(propDataType))
            //        {
            //            if (isRequire == GEnum.Require.Yes)
            //            {
            //                msgID = propName + "IsRequire";
            //                isValidation = false;
            //            }
            //            else
            //            {
            //                switch (propDataType)
            //                {
            //                    case GEnum.DataType.Boolean:
            //                        propValue = false; break;
            //                    case GEnum.DataType.DateTime:
            //                        propValue = Convert.ToDateTime("1/1/1900");
            //                        break;
            //                    case GEnum.DataType.Decimel:
            //                    case GEnum.DataType.Integer:
            //                        propValue = 0; break;
            //                    case GEnum.DataType.String:
            //                        propValue = string.Empty; break;
            //                }
            //            }
            //        }
            //        else
            //        {
            //            msgID = MsgID.Validation.ProgramError + "%Property DataType is Missing";
            //            isValidation = false;
            //        }
            //    }

            //}            
            #endregion
            try
            {

                // Property Name Validation
                if (isValidation)
                {
                    if (propName == null)
                    {
                        msgID = MsgID.Validation.ProgramError + "%Property Name is Missing";
                        isValidation = false;
                    }
                }

                // Property DataType Validation
                if (isValidation)
                {
                    if (propDataType == null)
                    {
                        msgID = MsgID.Validation.ProgramError + "%Property DataType is Missing";
                        isValidation = false;
                    }
                }

                // Property Is Require Validation
                if (isValidation)
                {
                    if (isRequire == null)
                    {
                        msgID = MsgID.Validation.ProgramError + "%Property IsRequire is Missing";
                        isValidation = false;
                    }
                }


                // Property Value Validation
                if (isValidation)
                {
                    if (GFunc.IsNE(propValue))
                    {
                        if (propValue == null || propValue == DBNull.Value || GFunc.NEStr(propValue, "") == string.Empty)
                        {
                            if (isRequire == GEnum.Require.Yes)
                            {
                                msgID = propName + "IsRequire";
                                isValidation = false;
                            }
                            else
                            {
                                msgID = string.Empty;
                                return isValidation;
                            }
                        }
                    }
                }

                if (isValidation)
                {
                    switch (propDataType)
                    {
                        case GEnum.DataType.String:
                            isValidation = StringIsValid(out msgID, propValue, propName, propDataType, isRequire, strMaxLength);
                            break;
                        case GEnum.DataType.Boolean:
                            isValidation = BooleanIsValid(out msgID, propValue, propName, propDataType, isRequire);
                            break;
                        case GEnum.DataType.DateTime:
                            isValidation = DateTimeIsValid(out msgID, propValue, propName, propDataType, isRequire, compareOperator, compareValue, minValue, maxValue);
                            break;
                        case GEnum.DataType.Integer:
                            isValidation = IntegerIsValid(out msgID, propValue, propName, propDataType, isRequire, compareOperator, compareValue, minValue, maxValue);
                            break;
                        case GEnum.DataType.Decimel:
                            isValidation = DecimalIsValid(out msgID, propValue, propName, propDataType, isRequire, compareOperator, compareValue, minValue, maxValue);
                            break;
                    }
                }

                //if (isValidation)
                //    msgID = string.Empty;

                return isValidation;
            }
            catch (Exception ex)
            {
                throw Error(ex);
            }
        }

        public static string Validate(bool failOnError, object propValue, string propName, GEnum.DataType? propDataType, GEnum.Require? isRequire, int? strMaxLength,
            GEnum.CompareOperator? compareOperator, object compareValue, object minValue, object maxValue, UINotifierEventArgs e, SqlConnection cn)
        {
            try
            {
                bool processResult = false;
                string msgID = string.Empty;

                if (failOnError)
                {
                    processResult = Validation(out msgID, propValue, propName, propDataType, isRequire, strMaxLength, compareOperator, compareValue, minValue, maxValue);
                }
                else
                {
                    processResult = Validation(out msgID, propValue, propName, propDataType, isRequire, strMaxLength, compareOperator, compareValue, minValue, maxValue);
                }

                if (!processResult)
                {
                    GFunc.NE(msgID, MsgID.Common.ValidationFail);
                    if (failOnError)
                    {
                        SysMessageUtility.Get(cn, msgID);
                        throw new Exception(msgID);
                    }
                    else
                    {
                        if (cn != null && e != null)   //May added on 4 Nov 2010 for code reuse purpose, since the individual cell checking doesn't need to do get msgValue             
                            e.PropertyMessage.Add(propName, SysMessageUtility.Get(cn, msgID));
                        return GVar.gcCancel;
                    }
                }
                else
                {
                    return GVar.gcPass;
                }
            }
            catch (Exception ex)
            {
                throw Error(ex);
            }
        }

        private static bool StringIsValid(out string msgID, object propValue, string propName, GEnum.DataType? propDataType, GEnum.Require? isRequire, int? strMaxLength)
        {
            bool isStringIsValid = true;
            msgID = MsgID.Common.UnableToValidate;
            try
            {

                if (GFunc.IsNE(propValue) == false)
                {
                    // Required Validation
                    if (isStringIsValid)
                    {
                        if (isRequire == GEnum.Require.Yes)
                        {
                            if (propValue.ToString().Trim().Length == 0)
                            {
                                msgID = propName + MsgID.Validation.IsRequire.ToString();
                                isStringIsValid = false;
                            }
                        }
                    }

                    // Max Length Validation
                    if (isStringIsValid)
                    {
                        if (!GFunc.IsNEZ(strMaxLength))
                        {
                            if (propValue.ToString().Length > strMaxLength)
                            {
                                msgID = propName + MsgID.Validation.ExceedMaxChar.ToString();
                                isStringIsValid = false;
                            }
                        }
                    }
                }

                if (isStringIsValid)
                    msgID = string.Empty;

                return isStringIsValid;
            }
            catch (Exception ex)
            {
                throw Error(ex);
            }
        }
        private static bool DateTimeIsValid(out string msgID, object propValue, string propName, GEnum.DataType? propDataType, GEnum.Require? isRequire, GEnum.CompareOperator? compareOperator, object compareValue, object minValue, object maxValue)
        {
            bool isDateTimeIsValid = true;
            msgID = MsgID.Common.UnableToValidate;

            try
            {
                if (GFunc.IsNE(propValue) == false)
                {
                    // Require Field Validation
                    if (isDateTimeIsValid)
                    {
                        if (isRequire == GEnum.Require.Yes)
                        {
                            DateTime dt;
                            if (!DateTime.TryParse(propValue.ToString(), out dt))
                            {
                                msgID = propName + MsgID.Validation.IsRequire.ToString();
                                isDateTimeIsValid = false;
                            }
                            else if (dt < Convert.ToDateTime("1/1/1900"))
                            {
                                msgID = propName + MsgID.Validation.IsRequire.ToString();
                                isDateTimeIsValid = false;
                            }
                        }
                    }

                    // DataType Validation
                    if (isDateTimeIsValid)
                    {
                        if (!IsDataTypeIsValid(propDataType, propValue))
                        {
                            msgID = propName + MsgID.Validation.NotDate.ToString();
                            isDateTimeIsValid = false;
                        }
                    }

                    // DataType Overflow Validation
                    if (isDateTimeIsValid)
                    {
                        if (!IsDataCapacityIsValid(propDataType, propValue))
                        {
                            msgID = propName + MsgID.Validation.DateOverflow.ToString();
                            isDateTimeIsValid = false;
                        }
                    }

                    // Exceed Limit Validation
                    if (isDateTimeIsValid)
                    {
                        if (!GFunc.IsNE(compareOperator) && !GFunc.IsNE(compareValue))
                        {
                            // DataType Validation
                            if (isDateTimeIsValid)
                            {
                                if (!IsDataTypeIsValid(propDataType, compareValue))
                                {
                                    msgID = propName + MsgID.Validation.NotDate.ToString();
                                    isDateTimeIsValid = false;
                                }
                            }

                            // DataType Overflow Validation
                            if (isDateTimeIsValid)
                            {
                                if (!IsDataCapacityIsValid(propDataType, compareValue))
                                {
                                    msgID = propName + MsgID.Validation.DateOverflow.ToString();
                                    isDateTimeIsValid = false;
                                }
                            }

                            if (isDateTimeIsValid)
                            {
                                if (!IsCompareValid(propDataType, propValue, compareValue, compareOperator))
                                {
                                    msgID = propName + MsgID.Validation.DateExceedLimit.ToString();
                                    isDateTimeIsValid = false;
                                }
                            }
                        }
                    }

                    // Range Validation
                    if (isDateTimeIsValid)
                    {
                        if (!GFunc.IsNE(minValue) && !GFunc.IsNE(maxValue))
                        {
                            // DataType Validation
                            if (isDateTimeIsValid)
                            {
                                if (!IsDataTypeIsValid(propDataType, minValue))
                                {
                                    msgID = propName + MsgID.Validation.NotDate.ToString();
                                    isDateTimeIsValid = false;
                                }
                            }

                            // DataType Overflow Validation
                            if (isDateTimeIsValid)
                            {
                                if (!IsDataCapacityIsValid(propDataType, minValue))
                                {
                                    msgID = propName + MsgID.Validation.DateOverflow.ToString();
                                    isDateTimeIsValid = false;
                                }
                            }

                            // DataType Validation
                            if (isDateTimeIsValid)
                            {
                                if (!IsDataTypeIsValid(propDataType, maxValue))
                                {
                                    msgID = propName + MsgID.Validation.NotDate.ToString();
                                    isDateTimeIsValid = false;
                                }
                            }

                            // DataType Overflow Validation
                            if (isDateTimeIsValid)
                            {
                                if (!IsDataCapacityIsValid(propDataType, maxValue))
                                {
                                    msgID = propName + MsgID.Validation.DateOverflow.ToString();
                                    isDateTimeIsValid = false;
                                }
                            }

                            if (isDateTimeIsValid)
                            {
                                if (!IsRangeIsValid(propDataType, propValue, minValue, maxValue))
                                {
                                    msgID = propName + MsgID.Validation.DateOutOfRange.ToString();
                                    isDateTimeIsValid = false;
                                }
                            }
                        }
                    }
                }

                if (isDateTimeIsValid)
                    msgID = string.Empty;

                return isDateTimeIsValid;
            }
            catch (Exception ex)
            {
                throw Error(ex);
            }
        }
        private static bool BooleanIsValid(out string msgID, object propValue, string propName, GEnum.DataType? propDataType, GEnum.Require? isRequire)
        {
            try
            {
                bool isBooleanIsValid = true;
                bool result = false;
                msgID = MsgID.Common.UnableToValidate;

                if (propValue != null)
                {
                    if (isBooleanIsValid)
                    {
                        if (isRequire == GEnum.Require.Yes)
                        {
                            if (propValue.ToString().Trim().Length == 0)
                            {
                                msgID = propName + MsgID.Validation.IsRequire.ToString();
                                isBooleanIsValid = false;
                                
                            }
                            else if (!bool.TryParse(propValue.ToString(), out result))
                            {
                                msgID = propName + MsgID.Validation.NotBoolean.ToString();
                                isBooleanIsValid = false;
                            }
                        }
                    }
                }

                if (isBooleanIsValid)
                    msgID = string.Empty;

                return isBooleanIsValid;
            }
            catch (Exception ex)
            {
                throw Error(ex);
            }
        }
        private static bool IntegerIsValid(out string msgID, object propValue, string propName, GEnum.DataType? propDataType, GEnum.Require? isRequire, GEnum.CompareOperator? compareOperator, object compareValue, object minValue, object maxValue)
        {
            bool isIntegerIsValid = true;
            msgID = MsgID.Common.UnableToValidate;
            try
            {
                if (GFunc.IsNE(propValue) == false)
                {
                    // Required Validation
                    if (isIntegerIsValid)
                    {
                        if (isRequire == GEnum.Require.Yes)
                        {
                            int tmp = 0;
                            if (!Int32.TryParse(propValue.ToString(), out tmp))
                            {
                                msgID = propName + MsgID.Validation.IsRequire.ToString();
                                isIntegerIsValid = false;
                            }
                        }
                        else
                            //Updated by May on 29-Oct-2007, For empty allow numeric data
                            if (propValue.ToString() == string.Empty)
                                propValue = 0;

                    }


                    // DataType Validation
                    if (isIntegerIsValid)
                    {
                        if (!IsDataTypeIsValid(propDataType, propValue))
                        {
                            msgID = propName + MsgID.Validation.NotInteger.ToString();
                            isIntegerIsValid = false;
                        }

                    }

                    // DataType Overflow Validation
                    if (isIntegerIsValid && propValue.ToString() != string.Empty)
                    {
                        if (!IsDataCapacityIsValid(propDataType, propValue))
                        {
                            msgID = propName + MsgID.Validation.IntegerOverflow.ToString();
                            isIntegerIsValid = false;
                        }
                    }

                    // Exceed Limit Validation

                    if (isIntegerIsValid)
                    {
                        if (!GFunc.IsNE(compareOperator) && !GFunc.IsNE(compareValue))
                        {
                            //  DataType Validation
                            if (isIntegerIsValid)
                            {
                                if (!IsDataTypeIsValid(propDataType, compareValue))
                                {
                                    msgID = propName + MsgID.Validation.NotInteger.ToString();
                                    isIntegerIsValid = false;
                                }
                            }

                            // DataType Overflow Validation
                            if (isIntegerIsValid)
                            {
                                if (!IsDataTypeIsValid(propDataType, compareValue))
                                {
                                    msgID = propName + MsgID.Validation.IntegerOverflow.ToString();
                                    isIntegerIsValid = false;
                                }
                            }

                            if (isIntegerIsValid)
                            {
                                if (!IsCompareValid(propDataType, propValue, compareValue, compareOperator))
                                {
                                    msgID = propName + MsgID.Validation.IntegerExceedLimit.ToString();
                                    isIntegerIsValid = false;
                                }
                            }
                        }
                    }

                    // Range Validation
                    if (isIntegerIsValid)
                    {
                        if (!GFunc.IsNE(minValue) && !GFunc.IsNE(maxValue))
                        {
                            // DataType Validation
                            if (isIntegerIsValid)
                            {
                                if (!IsDataTypeIsValid(propDataType, minValue))
                                {
                                    msgID = propName + MsgID.Validation.NotInteger.ToString();
                                    isIntegerIsValid = false;
                                }
                            }

                            // DataType Overflow Validation
                            if (isIntegerIsValid)
                            {
                                if (!IsDataTypeIsValid(propDataType, minValue))
                                {
                                    msgID = propName + MsgID.Validation.IntegerOverflow.ToString();
                                    isIntegerIsValid = false;
                                }
                            }

                            // DataType Validation
                            if (isIntegerIsValid)
                            {
                                if (!IsDataTypeIsValid(propDataType, maxValue))
                                {
                                    msgID = propName + MsgID.Validation.NotInteger.ToString();
                                    isIntegerIsValid = false;
                                }
                            }

                            // DataType Overflow Validation
                            if (isIntegerIsValid)
                            {
                                if (!IsDataTypeIsValid(propDataType, maxValue))
                                {
                                    msgID = propName + MsgID.Validation.IntegerOverflow.ToString();
                                    isIntegerIsValid = false;
                                }
                            }

                            if (isIntegerIsValid)
                            {
                                if (!IsRangeIsValid(propDataType, propValue, minValue, maxValue))
                                {
                                    msgID = propName + MsgID.Validation.IntegerOutOfRange.ToString();
                                    isIntegerIsValid = false;
                                }
                            }
                        }
                    }

                }

                if (isIntegerIsValid)
                    msgID = string.Empty;

                return isIntegerIsValid;
            }
            catch (Exception ex)
            {
                throw Error(ex);
            }
        }
        private static bool DecimalIsValid(out string msgID, object propValue, string propName, GEnum.DataType? propDataType, GEnum.Require? isRequire, GEnum.CompareOperator? compareOperator, object compareValue, object minValue, object maxValue)
        {
            try
            {
                bool isDecimalIsValid = true;
                msgID = MsgID.Common.UnableToValidate;

                if (GFunc.IsNE(propValue) == false)
                {
                    // Required Validation
                    if (isDecimalIsValid)
                    {
                        if (isRequire == GEnum.Require.Yes)
                        {
                            decimal tmp = 0;
                            if (!Decimal.TryParse(propValue.ToString(), out tmp))
                            {
                                msgID = propName + MsgID.Validation.IsRequire.ToString();
                                isDecimalIsValid = false;
                            }
                        }
                        else
                            if (propValue.ToString() == string.Empty)
                                return true;
                    }

                    // DataType Validation
                    if (isDecimalIsValid)
                    {
                        if (!IsDataTypeIsValid(propDataType, propValue))
                        {
                            msgID = propName + MsgID.Validation.NotDecimal.ToString();
                            isDecimalIsValid = false;
                        }
                    }

                    // DataType Overflow Validation
                    if (isDecimalIsValid)
                    {
                        if (!IsDataCapacityIsValid(propDataType, propValue))
                        {
                            msgID = propName + MsgID.Validation.DecimalOverflow.ToString();
                            isDecimalIsValid = false;
                        }
                    }

                    // Exceed Limit Validation
                    if (isDecimalIsValid)
                    {
                        if (!GFunc.IsNE(compareOperator) && !GFunc.IsNE(compareValue))
                        {
                            // DataType Validation
                            if (isDecimalIsValid)
                            {
                                if (!IsDataTypeIsValid(propDataType, compareValue))
                                {
                                    msgID = propName + MsgID.Validation.NotDecimal.ToString();
                                    isDecimalIsValid = false;
                                }
                            }

                            // DataType Overflow Validation
                            if (isDecimalIsValid)
                            {
                                if (!IsDataTypeIsValid(propDataType, compareValue))
                                {
                                    msgID = propName + MsgID.Validation.DecimalOverflow.ToString();
                                    isDecimalIsValid = false;
                                }
                            }

                            if (isDecimalIsValid)
                            {
                                if (!IsCompareValid(propDataType, propValue, compareValue, compareOperator))
                                {
                                    msgID = propName + MsgID.Validation.DecimalExceedLimit.ToString();
                                    isDecimalIsValid = false;
                                }
                            }
                        }
                    }

                    // Range Validation
                    if (isDecimalIsValid)
                    {
                        if (!GFunc.IsNE(minValue) && !GFunc.IsNE(maxValue))
                        {
                            // DataType Validation
                            if (isDecimalIsValid)
                            {
                                if (IsDataTypeIsValid(propDataType, minValue))
                                {
                                    msgID = propName + MsgID.Validation.NotDecimal.ToString();
                                    isDecimalIsValid = false;
                                }
                            }

                            // DataType Overflow Validation
                            if (isDecimalIsValid)
                            {
                                if (IsDataTypeIsValid(propDataType, minValue))
                                {
                                    msgID = propName + MsgID.Validation.DecimalOverflow.ToString();
                                    isDecimalIsValid = false;
                                }
                            }

                            // DataType Validation
                            if (isDecimalIsValid)
                            {
                                if (IsDataTypeIsValid(propDataType, maxValue))
                                {
                                    msgID = propName + MsgID.Validation.NotDecimal.ToString();
                                    isDecimalIsValid = false;
                                }
                            }

                            // DataType Overflow Validation
                            if (isDecimalIsValid)
                            {
                                if (IsDataTypeIsValid(propDataType, maxValue))
                                {
                                    msgID = propName + MsgID.Validation.DecimalOverflow.ToString();
                                    isDecimalIsValid = false;
                                }
                            }

                            // DataType Overflow Validation
                            if (isDecimalIsValid)
                            {
                                if (IsRangeIsValid(propDataType, propValue, minValue, maxValue))
                                {
                                    msgID = propName + MsgID.Validation.DecimalOutOfRange.ToString();
                                    isDecimalIsValid = false;
                                }
                            }
                        }
                    }
                }

                if (isDecimalIsValid)
                    msgID = string.Empty;

                return isDecimalIsValid;
            }
            catch (Exception ex)
            {
                throw Error(ex);
            }
        }

        /// <summary>
        /// Check Range Validation
        /// </summary>
        /// <param name="dataType"></param>
        /// <param name="propValue"></param>
        /// <param name="minValue"></param>
        /// <param name="maxValue"></param>
        /// <returns></returns>
        private static bool IsRangeIsValid(GEnum.DataType? dataType, object propValue, object minValue, object maxValue)
        {
            try
            {
                bool isRangeIsValid = false;

                switch (dataType)
                {
                    case GEnum.DataType.Integer:
                        isRangeIsValid = (Convert.ToInt32(propValue) >= (int)minValue && Convert.ToInt32(propValue) <= (int)maxValue);
                        break;
                    case GEnum.DataType.Decimel:
                        isRangeIsValid = (Convert.ToDecimal(propValue) >= Convert.ToDecimal(minValue) && Convert.ToDecimal(propValue) <= Convert.ToDecimal(maxValue));
                        break;
                    case GEnum.DataType.DateTime:
                        isRangeIsValid = (Convert.ToDateTime(propValue.ToString()) >= (DateTime)minValue && Convert.ToDateTime(propValue.ToString()) <= (DateTime)maxValue);
                        break;
                }

                return isRangeIsValid;
            }
            catch (Exception ex)
            {
                throw Error(ex);
            }
        }

        /// <summary>
        /// Check DataType Capacity Validation
        /// </summary>
        /// <param name="dataType"></param>
        /// <param name="propValue"></param>
        /// <returns></returns>
        private static bool IsDataCapacityIsValid(GEnum.DataType? dataType, object propValue)
        {
            try
            {
                bool isRangeIsValid = false;

                switch (dataType)
                {
                    case GEnum.DataType.Integer:
                        isRangeIsValid = (Convert.ToInt32(propValue) >= int.MinValue && Convert.ToInt32(propValue) <= int.MaxValue);
                        break;
                    case GEnum.DataType.Decimel:
                        isRangeIsValid = (Convert.ToDecimal(propValue) >= decimal.MinValue && Convert.ToDecimal(propValue) <= decimal.MaxValue);
                        break;
                    case GEnum.DataType.DateTime:
                        isRangeIsValid = (Convert.ToDateTime(propValue.ToString()) >= DateTime.MinValue && Convert.ToDateTime(propValue.ToString()) <= DateTime.MaxValue);
                        break;
                }

                return isRangeIsValid;
            }
            catch (Exception ex)
            {
                throw Error(ex);
            }
        }

        /// <summary>
        /// Check Comparison Validation
        /// </summary>
        /// <param name="dataType"></param>
        /// <param name="propValue"></param>
        /// <param name="compareValue"></param>
        /// <param name="compareOperator"></param>
        /// <returns></returns>
        private static bool IsCompareValid(GEnum.DataType? dataType, object propValue, object compareValue, GEnum.CompareOperator? compareOperator)
        {
            bool isCompareValid = false;
            try
            {
                if (GFunc.IsNE(compareValue))
                {
                    return !GFunc.IsNE(propValue);
                }
                switch (compareOperator)
                {
                    case GEnum.CompareOperator.Equal:
                        switch (dataType)
                        {
                            case GEnum.DataType.Integer:
                                isCompareValid = (Convert.ToInt32(propValue) == Convert.ToInt32(compareValue));
                                break;
                            case GEnum.DataType.Decimel:
                                isCompareValid = (Convert.ToDecimal(propValue) == Convert.ToDecimal(compareValue));
                                break;
                            case GEnum.DataType.DateTime:
                                isCompareValid = (Convert.ToDateTime(propValue.ToString()) == Convert.ToDateTime(compareValue));
                                break;
                        }
                        break;
                    case GEnum.CompareOperator.GreatherThan:
                        switch (dataType)
                        {
                            case GEnum.DataType.Integer:
                                isCompareValid = (Convert.ToInt32(propValue) > Convert.ToInt32(compareValue));
                                break;
                            case GEnum.DataType.Decimel:
                                isCompareValid = (Convert.ToDecimal(propValue) > Convert.ToDecimal(compareValue));
                                break;
                            case GEnum.DataType.DateTime:
                                isCompareValid = (Convert.ToDateTime(propValue.ToString()) > Convert.ToDateTime(compareValue));
                                break;
                        }
                        break;
                    case GEnum.CompareOperator.GreatherThanEqual:
                        switch (dataType)
                        {
                            case GEnum.DataType.Integer:
                                isCompareValid = (Convert.ToInt32(propValue) >= Convert.ToInt32(compareValue));
                                break;
                            case GEnum.DataType.Decimel:
                                isCompareValid = (Convert.ToDecimal(propValue) >= Convert.ToDecimal(compareValue));
                                break;
                            case GEnum.DataType.DateTime:
                                isCompareValid = (Convert.ToDateTime(propValue.ToString()) >= Convert.ToDateTime(compareValue));
                                break;
                        }
                        break;
                    case GEnum.CompareOperator.LessThan:
                        switch (dataType)
                        {
                            case GEnum.DataType.Integer:
                                isCompareValid = (Convert.ToInt32(propValue) < Convert.ToInt32(compareValue));
                                break;
                            case GEnum.DataType.Decimel:
                                isCompareValid = (Convert.ToDecimal(propValue) < Convert.ToDecimal(compareValue));
                                break;
                            case GEnum.DataType.DateTime:
                                isCompareValid = (Convert.ToDateTime(propValue.ToString()) < Convert.ToDateTime(compareValue));
                                break;
                        }
                        break;
                    case GEnum.CompareOperator.LessThanEqual:
                        switch (dataType)
                        {
                            case GEnum.DataType.Integer:
                                isCompareValid = (Convert.ToInt32(propValue) <= Convert.ToInt32(compareValue));
                                break;
                            case GEnum.DataType.Decimel:
                                isCompareValid = (Convert.ToDecimal(propValue) <= Convert.ToDecimal(compareValue));
                                break;
                            case GEnum.DataType.DateTime:
                                isCompareValid = (Convert.ToDateTime(propValue.ToString()) <= Convert.ToDateTime(compareValue));
                                break;
                        }
                        break;
                    case GEnum.CompareOperator.NotEqual:
                        switch (dataType)
                        {
                            case GEnum.DataType.Integer:
                                isCompareValid = (Convert.ToInt32(propValue) != Convert.ToInt32(compareValue));
                                break;
                            case GEnum.DataType.Decimel:
                                isCompareValid = (Convert.ToDecimal(propValue) != Convert.ToDecimal(compareValue));
                                break;
                            case GEnum.DataType.DateTime:
                                isCompareValid = (Convert.ToDateTime(propValue.ToString()) != Convert.ToDateTime(compareValue));
                                break;
                        }
                        break;
                    default:
                        break;
                }
                return isCompareValid;
            }
            catch (Exception ex)
            {
                throw Error(ex);
            }
        }

        /// <summary>
        /// Check DataType Validation
        /// </summary>
        /// <param name="dataType"></param>
        /// <param name="propValue"></param>
        /// <returns></returns>
        private static bool IsDataTypeIsValid(GEnum.DataType? dataType, object propValue)
        {
            bool isDataTypeIsValid = false;
            try
            {
                switch (dataType)
                {
                    case GEnum.DataType.Integer:
                        try
                        {
                            int intTmp = Convert.ToInt32(propValue);
                            isDataTypeIsValid = true;
                        }
                        catch//We do not want to get what the exception is
                        {
                            isDataTypeIsValid = false;
                        }
                        break;
                    case GEnum.DataType.Decimel:
                        try
                        {
                            decimal decimalTmp = Convert.ToDecimal(propValue);
                            isDataTypeIsValid = true;
                        }
                        catch//We do not want to get what the exception is
                        {
                            isDataTypeIsValid = false;
                        }
                        break;
                    case GEnum.DataType.DateTime:
                        try
                        {
                            // DateTime dateTimeTmp = Convert.ToDateTime(propValue.ToString()); //Modified By MTS
                            DateTime dateTimeTmp = Convert.ToDateTime(propValue.ToString());
                            isDataTypeIsValid = true;
                        }
                        catch//We do not want to get what the exception is
                        {
                            isDataTypeIsValid = false;
                        }
                        break;
                }
                return isDataTypeIsValid;
            }
            catch (Exception ex)
            {
                throw Error(ex);
            }
        }

        #region +++ Reject Code ++++

        //public static bool Validation(object propValue, string propName, GEnum.DataType? propDataType, GEnum.Require? isRequire, int? strMaxLength, GEnum.CompareOperator? compareOperator, object compareValue, object minValue, object maxValue)
        //{
        //    bool isValidation = false;
        //    switch (propDataType)
        //    {
        //        case GEnum.DataType.String:
        //            isValidation = StringIsValid(propValue, propName, propDataType, isRequire, strMaxLength);
        //            break;
        //        case GEnum.DataType.Boolean:
        //            isValidation = BooleanIsValid(propValue, propName, propDataType, isRequire);
        //            break;
        //        case GEnum.DataType.DateTime:
        //            isValidation = DateTimeIsValid(propValue, propName, propDataType, isRequire, compareOperator, compareValue, minValue, maxValue);
        //            break;
        //        case GEnum.DataType.Integer:
        //            isValidation = IntegerIsValid(propValue, propName, propDataType, isRequire, compareOperator, compareValue, minValue, maxValue);
        //            break;
        //        case GEnum.DataType.Decimel:
        //            isValidation = DecimalIsValid(propValue, propName, propDataType, isRequire, compareOperator, compareValue, minValue, maxValue);
        //            break;
        //    }
        //    return isValidation;
        //}

        //private static bool StringIsValid(object propValue, string propName, GEnum.DataType? propDataType, GEnum.Require? isRequire, int? strMaxLength)
        //{
        //    bool isStringIsValid = true;
        //    if (isRequire == GEnum.Require.Yes)
        //    {
        //        if (propValue.ToString().Length == 0)
        //            isStringIsValid = false;

        //        if (!isStringIsValid)
        //        {
        //            string msgID = propName + GEnum.BaseValidation.IsRequire.ToString();
        //            SYSMsgApp objSYSMsgApp = SYSMsgApp.Get(msgID);
        //            throw (new System.Exception(objSYSMsgApp.MsgText1));
        //        }
        //    }

        //    if (propValue.ToString().Length > strMaxLength)
        //        isStringIsValid = false;

        //    if (!isStringIsValid)
        //    {
        //        string msgID = propName + GEnum.BaseValidation.MaxLength.ToString();
        //        SYSMsgApp objSYSMsgApp = SYSMsgApp.Get(msgID);
        //        throw (new System.Exception(objSYSMsgApp.MsgText1));
        //    }

        //    return isStringIsValid;
        //}

        //private static bool DateTimeIsValid(object propValue, string propName, GEnum.DataType? propDataType, GEnum.Require? isRequire, GEnum.CompareOperator? compareOperator, object compareValue, object minValue, object maxValue)
        //{
        //    bool isDateTimeIsValid = true;

        //    // Check Require Field
        //    if (isRequire == GEnum.Require.Yes)
        //    {
        //        if (propValue.ToString().Length == 0)
        //            isDateTimeIsValid = false;

        //        if (!isDateTimeIsValid)
        //            throw (new System.Exception(propName + " is Require!"));
        //    }

        //    // Check Data Type validation
        //    isDateTimeIsValid = IsDataTypeIsValid(propDataType, propValue);
        //    if (!isDateTimeIsValid)
        //        throw (new System.Exception(propName + " : Data Type is Invalid!"));

        //    // Check DataType Capacity Validation
        //    isDateTimeIsValid = IsDataCapacityIsValid(propDataType, propValue);
        //    if (!isDateTimeIsValid)
        //        throw (new System.Exception(propName + " : Data Capacity is Invalid!"));

        //    // Check Compare Validation
        //    isDateTimeIsValid = IsCompareValid(propDataType, propValue, compareValue, compareOperator);
        //    if (!isDateTimeIsValid)
        //        throw (new System.Exception(propName + " " + compareOperator.ToString() + " " + compareValue.ToString() + "!"));

        //    // Check Range Validation
        //    isDateTimeIsValid = IsRangeIsValid(propDataType, propValue, minValue, maxValue);
        //    if (!isDateTimeIsValid)
        //        throw (new System.Exception(propName + " Out of " + minValue.ToString() + " and " + maxValue.ToString() + "!"));

        //    return isDateTimeIsValid;
        //}

        //private static bool BooleanIsValid(object propValue, string propName, GEnum.DataType? propDataType, GEnum.Require? isRequire)
        //{
        //    bool isBooleanIsValid = true;

        //    // Check Require Field
        //    if (isRequire == GEnum.Require.Yes)
        //    {
        //        try
        //        {
        //            bool ok = Convert.ToBoolean(propValue);
        //            isBooleanIsValid = true;
        //        }
        //        catch
        //        {
        //            isBooleanIsValid = false;
        //        }
        //    }

        //    return isBooleanIsValid;
        //}

        //private static bool IntegerIsValid(object propValue, string propName, GEnum.DataType? propDataType, GEnum.Require? isRequire, GEnum.CompareOperator? compareOperator, object compareValue, object minValue, object maxValue)
        //{
        //    bool isIntegerIsValid = true;

        //    // Check Require Field
        //    if (isRequire == GEnum.Require.Yes)
        //    {
        //        if (propValue.ToString().Length == 0)
        //            isIntegerIsValid = false;

        //        if (!isIntegerIsValid)
        //            throw (new System.Exception(propName + " is Require!"));
        //    }

        //    // Check Data Type validation
        //    isIntegerIsValid = IsDataTypeIsValid(propDataType, propValue);
        //    if (GFunc.IsNE(isIntegerIsValid))
        //        throw (new System.Exception("Data Type is Required!"));
        //    if (!isIntegerIsValid)
        //        throw (new System.Exception(propName + " : Data Type is Invalid!"));

        //    // Check DataType Capacity Validation
        //    isIntegerIsValid = IsDataCapacityIsValid(propDataType, propValue);
        //    if (GFunc.IsNE(isIntegerIsValid))
        //        throw (new System.Exception("Data Type is Required!"));
        //    if (!isIntegerIsValid)
        //        throw (new System.Exception(propName + " : Data Capacity is Invalid!"));

        //    // Check Compare Validation
        //    isIntegerIsValid = IsCompareValid(propDataType, propValue, compareValue, compareOperator);
        //    if (!GFunc.IsNE(isIntegerIsValid))
        //    {
        //        if (!isIntegerIsValid)
        //            throw (new System.Exception(propName + " " + compareOperator.ToString() + " " + compareValue.ToString() + "!"));
        //    }

        //    // Check Range Validation
        //    isIntegerIsValid = IsRangeIsValid(propDataType, propValue, minValue, maxValue);
        //    if (!GFunc.IsNE(isIntegerIsValid))
        //    {
        //        if (!isIntegerIsValid)
        //            throw (new System.Exception(propName + " Out of " + minValue.ToString() + " and " + maxValue.ToString() + "!"));
        //    }

        //    if (GFunc.IsNE(isIntegerIsValid))
        //        isIntegerIsValid = true;

        //    return isIntegerIsValid;
        //}

        //private static bool DecimalIsValid(object propValue, string propName, GEnum.DataType? propDataType, GEnum.Require? isRequire, GEnum.CompareOperator? compareOperator, object compareValue, object minValue, object maxValue)
        //{
        //    bool isDecimalIsValid = true;

        //    // Check Require Field
        //    if (isRequire == GEnum.Require.Yes)
        //    {
        //        if (propValue.ToString().Length == 0)
        //            isDecimalIsValid = false;

        //        if (!isDecimalIsValid)
        //            throw (new System.Exception(propName + " is Require!"));
        //    }

        //    // Check Data Type validation
        //    isDecimalIsValid = IsDataTypeIsValid(propDataType, propValue);
        //    if (!isDecimalIsValid)
        //        throw (new System.Exception(propName + " : Data Type is Invalid!"));

        //    // Check DataType Capacity Validation
        //    isDecimalIsValid = IsDataCapacityIsValid(propDataType, propValue);
        //    if (!isDecimalIsValid)
        //        throw (new System.Exception(propName + " : Data Capacity is Invalid!"));

        //    // Check Compare Validation
        //    isDecimalIsValid = IsCompareValid(propDataType, propValue, compareValue, compareOperator);
        //    if (!isDecimalIsValid)
        //        throw (new System.Exception(propName + " " + compareOperator.ToString() + " " + compareValue.ToString() + "!"));

        //    // Check Range Validation
        //    isDecimalIsValid = IsRangeIsValid(propDataType, propValue, minValue, maxValue);
        //    if (!isDecimalIsValid)
        //        throw (new System.Exception(propName + " Out of " + minValue.ToString() + " and " + maxValue.ToString() + "!"));

        //    return isDecimalIsValid;
        //}

        #endregion

        /// <summary>
        /// 
        /// </summary>
        /// <param name="control"></param>
        /// <param name="name"></param>
        //internal static TAUtil.RequiredFieldValidator AddRequiredFieldValidator(Control control, string name)
        //{
        //    TAUtil.RequiredFieldValidator objRequiredFieldValidator = new TAUtil.RequiredFieldValidator();
        //    objRequiredFieldValidator.ControlToValidate = control;
        //    //objRequiredFieldValidator.ErrorMessage = ctrl.DataBindings[0].BindingMemberInfo.BindingField + " is Require!";
        //    objRequiredFieldValidator.ErrorMessage = name + " is Require!";
        //    return objRequiredFieldValidator;
        //}

        /// <summary>
        /// 
        /// </summary>
        /// <param name="form"></param>
        public static void CheckRORefresh(System.Windows.Forms.Form form)
        {
            try
            {
                foreach (System.Windows.Forms.Control ctrl in form.Controls)
                {
                    if (ctrl.DataBindings.Count > 0)
                    {
                        string bindingFieldName = ctrl.DataBindings[0].BindingMemberInfo.BindingField;
                        System.Reflection.PropertyInfo readOnlyProp = ctrl.GetType().GetProperty("ReadOnly");
                        System.Reflection.PropertyInfo enabledProp = ctrl.GetType().GetProperty("Enabled");

                        System.Reflection.PropertyInfo bindingProp = ((System.Windows.Forms.BindingSource)(ctrl.DataBindings[0].DataSource)).Current.GetType().GetProperty(bindingFieldName);

                        bool isDone = false;
                        if (readOnlyProp != null && bindingProp != null)
                        {
                            if (!isDone)
                            {
                                bool readOnlyValue = !bindingProp.CanWrite;
                                readOnlyProp.SetValue(ctrl, readOnlyValue, null);
                                isDone = true;
                            }
                        }
                        else if (enabledProp != null && bindingProp != null)
                        {
                            if (!isDone)
                            {
                                bool enabledValue = bindingProp.CanWrite;
                                enabledProp.SetValue(ctrl, enabledValue, null);
                            }
                        }

                        ctrl.Validating += new System.ComponentModel.CancelEventHandler(ctrl_Validating);
                        ctrl.TextChanged += new EventHandler(ctrl_TextChanged);
                    }
                    else if (ctrl.HasChildren)
                    {
                        foreach (System.Windows.Forms.Control ctrl1 in ctrl.Controls)
                        {
                            if (ctrl1.DataBindings.Count > 0)
                            {
                                string bindingFieldName = ctrl1.DataBindings[0].BindingMemberInfo.BindingField;
                                System.Reflection.PropertyInfo readOnlyProp = ctrl1.GetType().GetProperty("ReadOnly");
                                System.Reflection.PropertyInfo enabledProp = ctrl1.GetType().GetProperty("Enabled");

                                System.Reflection.PropertyInfo bindingProp = ((System.Windows.Forms.BindingSource)(ctrl1.DataBindings[0].DataSource)).Current.GetType().GetProperty(bindingFieldName);

                                bool isDone = false;
                                if (readOnlyProp != null && bindingProp != null)
                                {
                                    if (!isDone)
                                    {
                                        bool readOnlyValue = !bindingProp.CanWrite;
                                        readOnlyProp.SetValue(ctrl1, readOnlyValue, null);
                                        isDone = true;
                                    }
                                }
                                else if (enabledProp != null && bindingProp != null)
                                {
                                    if (!isDone)
                                    {
                                        bool enabledValue = bindingProp.CanWrite;
                                        enabledProp.SetValue(ctrl1, enabledValue, null);
                                    }
                                }

                                ctrl1.Validating += new System.ComponentModel.CancelEventHandler(ctrl1_Validating);
                                ctrl1.TextChanged += new EventHandler(ctrl1_TextChanged);
                            }
                            else if (ctrl.HasChildren)
                            {
                                foreach (System.Windows.Forms.Control ctrl2 in ctrl1.Controls)
                                {
                                    if (ctrl2.DataBindings.Count > 0)
                                    {
                                        string bindingFieldName = ctrl2.DataBindings[0].BindingMemberInfo.BindingField;
                                        System.Reflection.PropertyInfo readOnlyProp = ctrl2.GetType().GetProperty("ReadOnly");
                                        System.Reflection.PropertyInfo enabledProp = ctrl2.GetType().GetProperty("Enabled");

                                        System.Reflection.PropertyInfo bindingProp = ((System.Windows.Forms.BindingSource)(ctrl2.DataBindings[0].DataSource)).Current.GetType().GetProperty(bindingFieldName);

                                        bool isDone = false;
                                        if (readOnlyProp != null && bindingProp != null)
                                        {
                                            if (!isDone)
                                            {
                                                bool readOnlyValue = !bindingProp.CanWrite;
                                                readOnlyProp.SetValue(ctrl2, readOnlyValue, null);
                                                isDone = true;
                                            }
                                        }
                                        else if (enabledProp != null && bindingProp != null)
                                        {
                                            if (!isDone)
                                            {
                                                bool enabledValue = bindingProp.CanWrite;
                                                enabledProp.SetValue(ctrl2, enabledValue, null);
                                            }
                                        }

                                        ctrl2.Validating += new System.ComponentModel.CancelEventHandler(ctrl2_Validating);
                                        ctrl2.TextChanged += new EventHandler(ctrl2_TextChanged);
                                    }
                                }
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                throw Error(ex);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private static void ctrl_Validating(object sender, System.ComponentModel.CancelEventArgs e)
        {
            try
            {
                ((System.Windows.Forms.Form)((System.Windows.Forms.Control)sender).Parent).Validate();
            }
            catch (Exception ex)
            {
                throw Error(ex);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private static void ctrl_TextChanged(object sender, EventArgs e)
        {
            try
            {
                ((System.Windows.Forms.Form)((System.Windows.Forms.Control)sender).Parent).Validate();
            }
            catch (Exception ex)
            {
                throw Error(ex);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private static void ctrl1_Validating(object sender, System.ComponentModel.CancelEventArgs e)
        {
            try
            {
                ((System.Windows.Forms.Form)((System.Windows.Forms.Control)sender).Parent.Parent).Validate();
            }
            catch (Exception ex)
            {
                throw Error(ex);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private static void ctrl1_TextChanged(object sender, EventArgs e)
        {
            try
            {
                ((System.Windows.Forms.Form)((System.Windows.Forms.Control)sender).Parent.Parent).Validate();
            }
            catch (Exception ex)
            {
                throw Error(ex);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private static void ctrl2_Validating(object sender, System.ComponentModel.CancelEventArgs e)
        {
            try
            {
                ((System.Windows.Forms.Form)((System.Windows.Forms.Control)sender).Parent.Parent.Parent).Validate();
            }
            catch (Exception ex)
            {
                throw Error(ex);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private static void ctrl2_TextChanged(object sender, EventArgs e)
        {
            try
            {
                ((System.Windows.Forms.Form)((System.Windows.Forms.Control)sender).Parent.Parent.Parent).Validate();
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
