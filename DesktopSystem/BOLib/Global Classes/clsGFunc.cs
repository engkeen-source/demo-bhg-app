using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.SqlClient;
using System.Data;
using System.IO;
using System.Reflection;
using Infragistics.Win;
using System.Collections;
using System.Xml;
using Infragistics.Win.UltraWinGrid;
using System.Globalization;
using Microsoft.VisualBasic;
using System.Text.RegularExpressions;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;
using System.Runtime.InteropServices;
using TAUtil;


namespace BOLib
{
    public static class DBNullableExtensions
    {
        public static object ToDBValue<T>(this Nullable<T> value) where T : struct
        {
            return value.HasValue ? (object)value.Value : DBNull.Value;
        }
    }
    public class GFunc
    {
        [DllImport("user32.dll")]
        static extern IntPtr GetFocus();
        //public static System.Data.DataSet ExecuteQueryDataSet(string sSql)
        //{

        //    try
        //    {
        //        System.Data.DataSet dsResult = new System.Data.DataSet();
        //        using (System.Data.SqlClient.SqlConnection sqlCon = new System.Data.SqlClient.SqlConnection(Database.BossDemoConnection))
        //        {
        //            System.Data.SqlClient.SqlCommand cm = sqlCon.CreateCommand();
        //            cm.CommandType = System.Data.CommandType.Text;
        //            cm.CommandText = sSql;

        //            System.Data.SqlClient.SqlDataAdapter sqlAdp = new System.Data.SqlClient.SqlDataAdapter(cm);

        //            sqlCon.Open();
        //            sqlAdp.Fill(dsResult);
        //            return dsResult;

        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        throw Error(ex);
        //    }
        //}

        public static string AddrGet(int addrKey, bool returnMultiline)
        {
            string address = string.Empty;
            string newLine = " ,";
            if (returnMultiline)
                newLine = "\n";
            REFAddr refAddr = REFAddr.Get(addrKey);

            if (GFunc.IsNE(refAddr.AddrAttn) == false)
                address = refAddr.AddrAttn + newLine;
            if (GFunc.IsNE(refAddr.AddrStreet) == false)
                address = address + refAddr.AddrStreet + newLine;
            if (GFunc.IsNE(refAddr.AddrPOBox) == false)
                address = address + refAddr.AddrPOBox + newLine;

            if (GFunc.IsNE(refAddr.AddrCity) == false)
                address = address + refAddr.AddrCity + " ,";
            if (GFunc.IsNE(refAddr.AddrRegion) == false)
                address = address + refAddr.AddrRegion + " ,";
            if (GFunc.IsNE(refAddr.AddrState) == false)
                address = address + refAddr.AddrState + " ,";
            if (GFunc.IsNE(refAddr.AddrCountry) == false)
                address = address + refAddr.AddrCountry + " ,";
            if (GFunc.IsNE(refAddr.AddrZipCode) == false)
                address = address + refAddr.AddrZipCode + newLine;

            if (GFunc.IsNE(refAddr.AddrTel1) == false)
                address = address + "Tel1:" + refAddr.AddrTel1 + newLine;
            if (GFunc.IsNE(refAddr.AddrTel2) == false)
                address = address + "Tel2:" + refAddr.AddrTel2 + newLine;

            if (GFunc.IsNE(refAddr.AddrEmail) == false)
                address = address + refAddr.AddrEmail + newLine;
            if (GFunc.IsNE(refAddr.AddrFax) == false)
                address = address + "Fax:" + refAddr.AddrFax + newLine;





            return address;
        }
        public static string AddrGet(string street, string POBox, string City, string State, string Zip, string Country)
        {
            string address = string.Empty;
            bool HaveAddLine = false;

            address = street;


            if (!GFunc.IsNE(POBox))
                address += "\n" + POBox;

            if (!GFunc.IsNE(City))
            {
                address += "\n" + City;
                HaveAddLine = true;
            }

            if (!GFunc.IsNE(State))
            {
                if (HaveAddLine)
                    address += "," + State;
                else
                {
                    address += "\n" + State;
                    HaveAddLine = true;
                }
            }

            if (!GFunc.IsNE(Zip))
            {
                if (HaveAddLine)
                    address += "," + Zip;
                else
                {
                    address += "\n" + Zip;
                    HaveAddLine = true;
                }
            }

            if (!GFunc.IsNE(Country))
            {
                if (HaveAddLine)
                    address += "," + Country;
                else
                {
                    address += "\n" + Country;
                    HaveAddLine = true;
                }
            }

            return address;
        }
        public static string AddrGet(int addrLinkKey)
        {
            string CmpAddr = "";
            DataTable dtAddr = GFunc.ExecuteQuery("Select dbo.GetAddr(" + addrLinkKey + ")");
            if (dtAddr.Rows.Count > 0)
                CmpAddr = GFunc.NEStr(dtAddr.Rows[0][0], "");

            return CmpAddr;

        }


        public static bool isEmail(string inputEmail)
        {
            string strRegex = @"^([a-zA-Z0-9_\-\.]+)@((\[[0-9]{1,3}" +
                  @"\.[0-9]{1,3}\.[0-9]{1,3}\.)|(([a-zA-Z0-9\-]+\" +
                  @".)+))([a-zA-Z]{2,4}|[0-9]{1,3})(\]?)$";
            Regex re = new Regex(strRegex);
            if (re.IsMatch(inputEmail))
                return (true);
            else
                return (false);

        }
        public static bool CompareString(string stringToReference, string stringTocompare)
        {
            return stringToReference.Trim().Equals(stringTocompare.Trim(), StringComparison.OrdinalIgnoreCase);
        }

        public static decimal RndDC(object Value, object Demonimator, int NoOfDecimal)
        {
            //Uses arithmatic rounding. Rounds a number to N decimal places
            //should be in the range 0-4 for proper results

            try
            {
                //decimal? Factor = 0;
                //decimal? a = 0;
                //int ValueSign = 0;

                if (GFunc.IsNEZ(Value) || GFunc.IsNEZ(Demonimator))
                    return 0;
                else
                {
                    //old code
                    // a = Value / Demonimator;                    
                    // ValueSign = System.Math.Sign(a.Value);
                    // a = System.Math.Abs(a.Value);
                    // Factor = (decimal)System.Math.Pow(10, NoOfDecimal);
                    // a = a * Factor + Convert.ToDecimal(10.5m);
                    // a = Convert.ToInt64(a);
                    // return a * ValueSign / Factor;

                    return RndDC(GFunc.NEDec(Value, 0), GFunc.NEDec(Demonimator, 0), NoOfDecimal);
                }
            }
            catch (Exception ex)
            {
                throw Error(ex);
            }
        }
        public static decimal RndDC(decimal Value, decimal Demonimator, int NoOfDecimal)
        {
            //Uses arithmatic rounding. Rounds a number to N decimal places
            //should be in the range 0-4 for proper results
            try
            {
                //decimal? Factor = 0;
                //decimal? a = 0;
                //int ValueSign = 0;

                if (GFunc.IsNEZ(Value) || GFunc.IsNEZ(Demonimator))
                    return 0;
                //old code
                // a = Value / Demonimator;                    
                // ValueSign = System.Math.Sign(a.Value);
                // a = System.Math.Abs(a.Value);
                // Factor = (decimal)System.Math.Pow(10, NoOfDecimal);
                // a = a * Factor + Convert.ToDecimal(10.5m);
                // a = Convert.ToInt64(a);
                // return a * ValueSign / Factor;

                decimal value = 0;
                decimal demonimator = 0;
                value = (decimal)Value;
                demonimator = (decimal)Demonimator;

                return System.Math.Round(value / demonimator, NoOfDecimal,MidpointRounding.AwayFromZero);

            }
            catch (Exception ex)
            {
                throw Error(ex);
            }
        }

        public static decimal RndC(object Value, int NoOfDecimal)
        {
            //Uses arithmatic rounding. Rounds a number to N decimal places
            //should be in the range 0-10 for proper results

            try
            {
                if (GFunc.IsNEZ(Value))
                    return 0;
                else
                {
                    decimal d = 0;
                    if (Decimal.TryParse(Value.ToString(), out d))
                        return RndC(d, NoOfDecimal);
                    else
                        throw new TAUtil.TAException(TAUtil.TAErrorCode.INVALID_NUMERIC);
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
        public static decimal RndC(decimal Value, int NoOfDecimal)
        {

            try
            {
                //Uses arithmatic rounding. Rounds a number to N decimal places
                //should be in the range 0-10 for proper results
                //decimal Factor = 0;
                //decimal a = 0;

                //Factor = (decimal)System.Math.Pow((double)10, (double)NoOfDecimal);
                //a = System.Math.Abs(Value) * Factor + (decimal)0.5;

                //a = Convert.ToInt64(a);
                //return a * System.Math.Sign(Value) / Factor;

                return Math.Round(Value, NoOfDecimal,MidpointRounding.AwayFromZero);

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

        public static decimal RndUD(object Value, int RoundMode)
        {
            //Uses arithmatic rounding. Rounds a number to N decimal places
            //should be in the range 0-10 for proper results

            try
            {
                if (GFunc.IsNEZ(Value))
                    return 0;
                else
                {
                    decimal d = 0;
                    if (Decimal.TryParse(Value.ToString(), out d))
                    {
                        if (RoundMode == 0)
                            return d;
                        else
                            return RndUD((decimal?)d, RoundMode);
                    }
                    else
                        throw new TAUtil.TAException(TAUtil.TAErrorCode.INVALID_NUMERIC);
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
        public static decimal RndUD(decimal Value, int RoundMode)
        {
            //Round up/down a value to the nearest RoundMode
            try
            {
                int SignNum = System.Math.Sign(Value);
                decimal wrkValue = System.Math.Abs(Value);
                decimal Result = 0;
                switch (RoundMode)
                {
                    //No Rounding
                    case 0:
                        Result = Value;
                        break;
                    //Round up to 5 cents
                    case 10:
                        Result = SignNum * (System.Math.Ceiling(20.0M * wrkValue)) / 20.0M;
                        break;
                    //Round down to 5 cents
                    case 20:
                        Result = SignNum * (System.Math.Floor(20.0M * wrkValue)) / 20.0M;
                        break;
                    //Round up to 10 cents
                    case 30:
                        Result = SignNum * (System.Math.Ceiling(10.0M * wrkValue)) / 10.0M;
                        break;
                    //Round down to 10 cents
                    case 40:
                        Result = SignNum * (System.Math.Floor(10.0M * wrkValue)) / 10.0M;
                        break;
                    //Round up to 50 cents
                    case 50:
                        Result = SignNum * (System.Math.Ceiling(2M * wrkValue)) / 2M;
                        break;
                    //Round down to 50 cents
                    case 60:
                        Result = SignNum * (System.Math.Floor(2M * wrkValue)) / 2M;
                        break;
                    //Round up to dollar
                    case 70:
                        Result = SignNum * System.Math.Ceiling(wrkValue);

                        break;
                    //Round down to dollar
                    case 80:
                        Result = SignNum * System.Math.Floor(wrkValue);

                        break;
                }
                return Result;
            }
            catch (Exception ex)
            {
                throw Error(ex);
            }
        }
        public static decimal RndUD(object Value, int RoundMode, int NoOfDecimal)
        {
            //Uses arithmatic rounding. Rounds a number to N decimal places
            //should be in the range 0-10 for proper results

            try
            {
                if (GFunc.IsNEZ(Value))
                    return 0;
                else
                {
                    decimal d = 0;
                    if (Decimal.TryParse(Value.ToString(), out d))
                    {
                        if (RoundMode == 0)
                            return d;
                        else
                            return RndUD(d, RoundMode, NoOfDecimal);
                    }
                    else
                        throw new TAUtil.TAException(TAUtil.TAErrorCode.INVALID_NUMERIC);
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
        public static decimal RndUD(decimal Value, int RoundMode, int NoOfDecimal)
        {
            //Round up/down a value to the nearest RoundMode
            //perform round up/down only if the decimal place is <=2
            try
            {
                if (RoundMode == 0)
                    return Value;

                if (NoOfDecimal <= 2)
                    return RndUD(RndC(Value, NoOfDecimal), RoundMode);
                else
                    return RndC(Value, NoOfDecimal);

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

        public static bool IsNE(object value)
        {
            try
            {
                if (value == null)
                    return true;
                else if (string.IsNullOrEmpty(value.ToString()) && value.GetType() == typeof(string))
                    return true;
                else if (value == DBNull.Value)
                    if (value.GetType() != typeof(DataTable) && value.GetType().BaseType != typeof(DataTable))
                        return true;

                return false;
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
        public static bool IsNEZ(object value)
        {
            try
            {
                if (value == null || string.IsNullOrEmpty(value.ToString()) || value == DBNull.Value)
                    return true;
                else
                {
                    decimal tempValue = 0;
                    Decimal.TryParse(value.ToString(), out tempValue);

                    if (tempValue == 0)
                        return true;
                    else
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
        }
        public static bool IsBetweenDec(decimal value, decimal fromValue, decimal toValue)
        {
            try
            {
                if (fromValue < toValue)
                {
                    if (value >= fromValue && value <= toValue)
                    {
                        return true;
                    }
                    else
                    {
                        return false;
                    }
                }
                else
                {
                    //if (value > toValue && value < fromValue) commented by Jane on 01-Nov-2013. 
                    //Above commented code is not working correctly when [dueamt = (applied + dis)]. Mic to check.
                    if (value >= toValue && value <= fromValue)
                    {
                        return true;
                    }
                    else
                    {
                        return false;
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
        public static void NE(object value, object replaceValue)
        {
            try
            {
                if (value == null || string.IsNullOrEmpty(value.ToString()) || value == DBNull.Value)
                    value = replaceValue;
            }
            catch (Exception ex)
            {
                throw Error(ex);
            }
        }
        public static int NEInt(object value, object replaceValue)
        {
            try
            {
                if (value == null || string.IsNullOrEmpty(value.ToString()) || value == DBNull.Value)
                    return Convert.ToInt32(replaceValue);
                else if (value.ToString().Contains("."))
                    return Decimal.ToInt32(Convert.ToDecimal(value));
                else
                    return Convert.ToInt32(value);
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
        public static short NEShort(object value, object replaceValue)
        {
            try
            {
                if (value == null || string.IsNullOrEmpty(value.ToString()) || value == DBNull.Value)
                    return Convert.ToInt16(replaceValue);
                else
                    return Convert.ToInt16(value);
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
        public static decimal NEDec(object value, object replaceValue)
        {
            try
            {
                if (value == null || string.IsNullOrEmpty(value.ToString()) || value == DBNull.Value)
                    return Convert.ToDecimal(replaceValue);
                else
                    return Convert.ToDecimal(value);
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
        public static string NEStr(object value, object replaceValue)
        {
            try
            {
                if (value == null || string.IsNullOrEmpty(value.ToString()) || value == DBNull.Value)
                    return replaceValue.ToString();
                else
                    return value.ToString();
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
        public static bool NEBool(object value, object replaceValue)
        {
            try
            {

                if (value == null || string.IsNullOrEmpty(value.ToString()) || value == DBNull.Value)
                    return Convert.ToBoolean(replaceValue);
                else
                {
                    int intValue = 0;
                    if (Int32.TryParse(value.ToString(), out intValue))
                        return Convert.ToBoolean(intValue);
                    else
                        return Convert.ToBoolean(value);
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
        public static DateTime NEDateTime(object value, object replaceValue)
        {
            try
            {
                if (value == null || string.IsNullOrEmpty(value.ToString()) || value == DBNull.Value)
                    return Convert.ToDateTime(replaceValue);
                else
                    return Convert.ToDateTime(value);
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

        public static string GetReportParameteDefaultValue(string dataType, string paraValue)
        {
            if (GFunc.IsNE(paraValue))
            {
                switch (dataType.ToUpper())
                {
                    case GVar.ReportCriteriaDataType.String:
                        paraValue = "";
                        break;
                    case GVar.ReportCriteriaDataType.Date:
                        paraValue = DateTime.Today.Date.ToString("dd-MMM-yyyy");
                        break;
                    default:
                        paraValue = "0";
                        break;
                }
            }
            else
            {
                switch (dataType.ToUpper())
                {
                    case GVar.ReportCriteriaDataType.Date:
                        DateTime dt = DateTime.Today.Date;
                        DateTime.TryParse(paraValue, out dt);
                        paraValue = dt.ToString("dd-MMM-yyyy");
                        break;
                }
            }
            return paraValue;
        }
        public static object GetPropertyValue(string propName, object objOwner)
        {
            try
            {
                object retValue = null;
                System.Reflection.PropertyInfo propertyInfo = objOwner.GetType().GetProperty(propName);

                if (propertyInfo != null)
                    retValue = propertyInfo.GetValue(objOwner, null);
                else
                    MsgBox.Show(MsgID.Common.PropertyNotFound + "%" + propName + "%" + objOwner.GetType().ToString());

                return retValue;
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
        public static object GetMemberValue(string propName, object objOwner)
        {
            try
            {


                object retValue = null;
                FieldInfo[] mFieldInfo = objOwner.GetType().GetFields(BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.Public);

                object obj = mFieldInfo.First(o => o.Name.ToLower().Contains(propName.ToLower())).GetValue(objOwner);

                if (obj != null)
                    retValue = obj;
                else
                    MsgBox.Show(MsgID.Common.PropertyNotFound + "%" + propName + "%" + objOwner.GetType().ToString());

                return retValue;
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
        public static int? GetIntPropertyValue(string propName, object objOwner)
        {
            try
            {
                object retValue = null;

                System.Reflection.PropertyInfo propertyInfo = objOwner.GetType().GetProperty(propName);

                if (propertyInfo != null)
                    retValue = propertyInfo.GetValue(objOwner, null);
                else
                    throw new TAException(MsgID.Common.PropertyNotFound + "%" + propName + "%" + objOwner.GetType().ToString());


                return (int?)retValue;

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
        public static string GetStringPropertyValue(string propName, object objOwner)
        {
            try
            {
                object retValue = null;

                System.Reflection.PropertyInfo propertyInfo = objOwner.GetType().GetProperty(propName);

                if (propertyInfo != null)
                    retValue = propertyInfo.GetValue(objOwner, null);
                else
                    throw new TAException(MsgID.Common.PropertyNotFound + "%" + propName + "%" + objOwner.GetType().ToString());

                if (retValue != null)
                    return retValue.ToString();

                return string.Empty;
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
        public static string GetStringPropertyValueOrEmpty(string propName, object objOwner)
        {
            try
            {
                object retValue = null;

                System.Reflection.PropertyInfo propertyInfo = objOwner.GetType().GetProperty(propName);

                if (propertyInfo != null)
                    retValue = propertyInfo.GetValue(objOwner, null);


                if (retValue != null)
                    return retValue.ToString();

                return string.Empty;
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
        public static decimal? GetDecimalPropertyValue(string propName, object objOwner)
        {
            try
            {
                object retValue = null;

                System.Reflection.PropertyInfo propertyInfo = objOwner.GetType().GetProperty(propName);

                if (propertyInfo != null)
                    retValue = propertyInfo.GetValue(objOwner, null);
                else
                    throw new TAException(MsgID.Common.PropertyNotFound + "%" + propName + "%" + objOwner.GetType().ToString());

                return (decimal?)retValue;
              
              
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
        public static DateTime? GetDatePropertyValue(string propName, object objOwner)
        {
            try
            {
                object retValue = null;

                System.Reflection.PropertyInfo propertyInfo = objOwner.GetType().GetProperty(propName);

                if (propertyInfo != null)
                    retValue = propertyInfo.GetValue(objOwner, null);
                else
                    throw new TAException(MsgID.Common.PropertyNotFound + "%" + propName + "%" + objOwner.GetType().ToString());

                return (DateTime?)retValue;
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
        public static void SetPropertyValue(string propName, object objOwner, object value)
        {
            try
            {
                System.Reflection.PropertyInfo propertyInfo = null;

                propertyInfo = objOwner.GetType().GetProperty(propName);

                if (propertyInfo != null)
                    propertyInfo.SetValue(objOwner, value, null);
                else
                    throw new TAException(MsgID.Common.PropertyNotFound + "%" + propName + "%" + objOwner.GetType().ToString());

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

        /// <summary>
        /// This function set the Property Value and reset the Factory original Dirty value.
        /// </summary>
        /// <param name="factory">Factory Object</param>
        /// <param name="property"></param>
        /// <param name="value"></param>
        public static void SetPropertyValue(object factory, object objOwner, string propertyName, object value)
        {
            try
            {
                PropertyInfo fieldInfo = factory.GetType().GetProperty("IsDirty");
                if (fieldInfo != null)
                {
                    bool IsDirty = (bool)fieldInfo.GetValue(factory, null);

                    SetPropertyValue(propertyName, objOwner, value); //set the value to the property

                    if (IsDirty == false)//If original dirty value is false
                        fieldInfo.SetValue(factory, false, null); //Reset the factory dirty property to false
                }
                else
                    SetPropertyValue(propertyName, objOwner, value); //set the value to the property
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

        //public static void SetObjectPrivateValue(string ObjVarName, object objOwner, object value)
        //{
        //    FieldInfo[] fieldInfoArr = objOwner.GetType().GetFields(BindingFlags.NonPublic | BindingFlags.Instance);

        //    FieldInfo fieldInfo=fieldInfoArr.ToList().Find(o => o.Name.ToLower().Equals(ObjVarName.ToLower()));
        //    if (fieldInfo != null)
        //        fieldInfo.SetValue(objOwner, value);
        //    else
        //        throw new TAException("No member Variable exists with the name '"+ObjVarName +"' in "+ objOwner.GetType().ToString()); 
        //}

        public static void GetIndexfromDT(string searchCol, object searchValue, bool getNext, DataTable dt, out string id)
        {
            id = string.Empty;
            try
            {
                if (GFunc.IsNE(searchValue))
                {
                    id = dt.AsEnumerable().Min(p => p.Field<string>(searchCol));
                    return;
                }
                if (getNext)
                {
                    //dt.DefaultView.RowFilter = searchCol + " > '" + searchValue.ToString() + "'";
                    //id = dt.DefaultView.ToTable().AsEnumerable().Min(p => p.Field<string>(searchCol));
                    id = dt.Compute("Min(" + searchCol + ")", searchCol + " > '" + searchValue.ToString() + "'").ToString();
                }
                else
                {
                    //dt.DefaultView.RowFilter = searchCol + "< '" + searchValue.ToString() + "'";
                    //id = dt.DefaultView.ToTable().AsEnumerable().Max(p => p.Field<string>(searchCol));
                    id = dt.Compute("Max(" + searchCol + ")", searchCol + " < '" + searchValue.ToString() + "'").ToString();
                }
                if (GFunc.IsNE(id))
                    id = searchValue.ToString();
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

        public static string GetItmTypeEnName(int itmType)
        {
            switch (itmType)
            {
                case 100: return "Stock";
                case 110: return "StockB";
                case 200: return "Finished GD";
                case 210: return "Finished GDB";
                case 250: return "Assembly";
                case 310: return "Serial StockB";
                case 410: return "Serial Finished GDB";
                case 510: return "Consignment";
                case 600: return "Non Stock";
                case 610: return "Service";
                case 700: return "Charges";
                case 710: return "Discount";
                case 800: return "Header";
                case 810: return "Remark";
                case 820: return "Sub Total";
                case 825: return "Total";
                case 830: return "CF Total";
                case 900: return "Master";
                default: return "";
            }
        }

        public static string PropertyMessage_Merge(UINotifierEventArgs e, SqlConnection cn)
        {
            string msgID = string.Empty;

            foreach (object key in e.PropertyMessage.Keys)
            {
                if (GFunc.IsNE(msgID) == false)
                    msgID += ", ";

                msgID += SysMessageUtility.Get(cn, e.PropertyMessage[key].ToString());
            }
            return msgID;
        }
        public static string PropertyMessage_Merge(UINotifierEventArgs e)
        {
            using (SqlConnection cn = new SqlConnection(Database.BossDemoConnection))
            {
                cn.Open();
                return PropertyMessage_Merge(e, cn);
            }

        }
        internal static bool IsPasswordValid(object Password, object PasswordToCheck)
        {
            try
            {
                bool isPasswordisValid = false;


                if (Password.ToString().Trim() == TAUtil.Decoder.Decode(PasswordToCheck.ToString()))
                {
                    isPasswordisValid = true;
                }
                return isPasswordisValid;
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
        internal static bool IsDatabaseRegCodeIsValid()
        {
            try
            {
                string vRegistrationCode = string.Empty;
                string vDatabaseRegCode = string.Empty;
                string vCompanyName = string.Empty;


                bool isDatabaseRegCodeIsValid = false;
                string msgID = "DatabaseRegCodeInvalid";


                vDatabaseRegCode = SysOptionUtility.GetStr("DatabaseRegCode");
                vCompanyName = SysOptionUtility.GetStr("CompanyName");


                if ((!IsNE(vDatabaseRegCode)) && (!IsNE(vCompanyName)))
                {
                    string companyCode = TAUtil.Encoder.Encode(vCompanyName);
                    if (TAUtil.Decoder.Decode(vDatabaseRegCode) == vCompanyName)
                    {

                        isDatabaseRegCodeIsValid = true;
                    }
                    else
                    {
                        throw new TAException(msgID);
                    }


                }
                else
                {
                    throw new TAException(msgID);
                }

                return isDatabaseRegCodeIsValid;
            }
            catch (TAException tex)
            {
                if (tex.MsgID == MsgID.Common.NoMultiInstanceAllowed)
                {

                }
                else
                {

                }
                throw Error(tex);
            }

            catch (Exception ex)
            {
                throw Error(ex);
            }

        }

        public static string GetDatabaseName()
        {
            SqlConnectionStringBuilder SSB = new SqlConnectionStringBuilder(Database.BossDemoConnection);
            String Catalog = SSB.InitialCatalog;
            return Catalog;
        }

        public static System.Data.DataTable ExecuteQuery(SqlConnection cn, string sSql)
        {

            try
            {
                System.Data.DataTable dtResult = new System.Data.DataTable();


                System.Data.SqlClient.SqlCommand cm = cn.CreateCommand();
                cm.CommandTimeout = 1000;
                cm.CommandType = System.Data.CommandType.Text;
                cm.CommandText = sSql;

                System.Data.SqlClient.SqlDataAdapter sqlAdp = new System.Data.SqlClient.SqlDataAdapter(cm);

                sqlAdp.Fill(dtResult);

                return dtResult;
            }
            catch (Exception ex)
            {
                throw Error(ex);
            }


        }
        public static System.Data.DataTable ExecuteQuery(string sSql)
        {

            try
            {
                System.Data.DataTable dsResult = new System.Data.DataTable();
                using (System.Data.SqlClient.SqlConnection sqlCon = new System.Data.SqlClient.SqlConnection(Database.BossDemoConnection))
                {

                    System.Data.SqlClient.SqlCommand cm = sqlCon.CreateCommand();
                    cm.CommandTimeout = 1000;
                    cm.CommandType = System.Data.CommandType.Text;
                    cm.CommandText = sSql;

                    System.Data.SqlClient.SqlDataAdapter sqlAdp = new System.Data.SqlClient.SqlDataAdapter(cm);

                    sqlCon.Open();
                    sqlAdp.Fill(dsResult);
                    return dsResult;

                }
            }
            catch (Exception ex)
            {
                throw Error(ex);
            }
        }
        public static void ExecuteNonQuery(SqlConnection cn, string sSql)
        {

            try
            {
                using (System.Data.SqlClient.SqlCommand cm = cn.CreateCommand())
                {
                    cm.CommandType = System.Data.CommandType.Text;
                    cm.CommandTimeout = 100;
                    cm.CommandText = sSql;
                    cm.ExecuteNonQuery();
                }
            }
            catch (Exception ex)
            {
                throw Error(ex);
            }
        }
        public static string ExecuteScalar(SqlConnection cn, string sSql)
        {

            try
            {
                using (System.Data.SqlClient.SqlCommand cm = cn.CreateCommand())
                {
                    cm.CommandType = System.Data.CommandType.Text;
                    cm.CommandTimeout = 100;
                    cm.CommandText = sSql;
                    return cm.ExecuteScalar().ToString();
                }
            }
            catch (Exception ex)
            {
                throw Error(ex);
            }
        }

        public static string ExecuteScalarNull(SqlConnection cn, string sSql)
        {
            string strVar = "";
            try
            {
                using (System.Data.SqlClient.SqlCommand cm = cn.CreateCommand())
                {
                    cm.CommandType = System.Data.CommandType.Text;
                    cm.CommandTimeout = 100;
                    cm.CommandText = sSql;

                    var varVar = cm.ExecuteScalar();
                    if (varVar != null)
                        strVar = varVar.ToString();
                }
                return strVar;
            }
            catch (Exception ex)
            {
                throw Error(ex);
            }
        }

        public static string ExecuteScalar( string sSql)
        {

            try
            {
                using (System.Data.SqlClient.SqlConnection cn = new System.Data.SqlClient.SqlConnection(Database.BossDemoConnection))
                {
                    cn.Open();
                    using (System.Data.SqlClient.SqlCommand cm = cn.CreateCommand())
                    {
                        cm.CommandType = System.Data.CommandType.Text;
                        cm.CommandTimeout = 100;
                        cm.CommandText = sSql;
                        return cm.ExecuteScalar().ToString();
                    }
                }
            }
            catch (Exception ex)
            {
                throw Error(ex);
            }
        }

        /* added by YST on 2020/10/15 */
        public static string ExecuteScalar(string procName, List<SqlParameter> parmList)
        {
            /* ExecuteScalar to overcome sql injection error from parameters */
            try
            {
                object result = "";
                using (System.Data.SqlClient.SqlConnection cn = new System.Data.SqlClient.SqlConnection(Database.BossDemoConnection))
                {
                    cn.Open();
                    using (System.Data.SqlClient.SqlCommand cm = cn.CreateCommand())
                    {
                        cm.CommandType = System.Data.CommandType.StoredProcedure;
                        cm.CommandText = procName;
                        cm.CommandTimeout = 100000;
                        if (parmList != null)
                        {
                            foreach (SqlParameter p in parmList)
                            {
                                p.Value = p.Value ?? DBNull.Value; 
                                cm.Parameters.Add(p);
                            }
                        }

                        result = cm.ExecuteScalar();
                        return result != null ? result.ToString() : "";
                    }

                }
            }
            catch (Exception ex)
            {
                throw Error(ex);
            }
        }
        public static string ExecuteScalar(SqlConnection cn, string procName, List<SqlParameter> parmList)
        {
            /* Use parameterized queries with ExecuteScalar to prevent SQL injection vulnerabilities */
            try
            {
                object result = "";
                using (System.Data.SqlClient.SqlCommand cm = cn.CreateCommand())
                {
                    cm.CommandType = System.Data.CommandType.StoredProcedure;
                    cm.CommandText = procName;
                    cm.CommandTimeout = 100000;
                    if (parmList != null)
                    {
                        foreach (SqlParameter p in parmList)
                        {
                            if (p.Value == null)
                            {
                                cm.Parameters.Add("");
                            }
                            else
                                cm.Parameters.Add(p);
                        }
                    }

                    result = cm.ExecuteScalar();
                    return result != null ? result.ToString() : "";
                }
            }
            catch (Exception ex)
            {
                throw Error(ex);
            }
        }        
        public static string ValidateFileName(string fileName, string replaceStr = "") /* added by YST on 2023/07/25 */
        {
            string[] invalidStr = { "\\", "/", ":", "*", "?", "<", ">", "|" }; /* A file name can't contain any of the following characters \/:*?<>| */

            if (!string.IsNullOrEmpty(fileName))
            {
                foreach (string s in invalidStr)
                {
                    if (fileName.Contains(s)) fileName = fileName.Replace(s, replaceStr);
                }
            }
            else
            {
                fileName = "";
            }
            return fileName;
        }
        public static bool ValidateEmail(string email) /* added by YST on 2023/11/09 */
        {
            // Define a regular expression for a basic email format
            string pattern = @"^[a-zA-Z0-9._-]+@[a-zA-Z0-9.-]+\.[a-zA-Z]{2,4}$";

            // Create a Regex object
            Regex regex = new Regex(pattern);

            // Use the Regex.IsMatch method to check if the email matches the pattern
            return regex.IsMatch(email);
        }
        /* end added YST  */

        /* generate random password */
        public static string GenerateRandomCode(int length = 8)
        {
            var chars = "ABCDEFGHJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789";
            var stringChars = new char[length];
            var random = new Random();

            for (int i = 0; i < stringChars.Length; i++)
            {
                stringChars[i] = chars[random.Next(chars.Length)];
            }
            return new String(stringChars);
        }

        public static string GetCodeKeyDescription(SqlConnection cn, int CodeKey)
        {
            string query = "select CodeDesLang1 from SYS_Code(nolock) where CodeKey=" + CodeKey;
            return GFunc.ExecuteScalar(cn, query);
        }
        public static string GetCodeKeyDescription(int CodeKey)
        {
            SqlConnection cn = new SqlConnection(Database.BossDemoConnection); cn.Open();
            return GetCodeKeyDescription(cn, CodeKey);
        }


        public static System.Data.DataSet ExecuteQueryDataSet(string sSql)
        {

            try
            {
                System.Data.DataSet dsResult = new System.Data.DataSet();
                using (System.Data.SqlClient.SqlConnection sqlCon = new System.Data.SqlClient.SqlConnection(Database.BossDemoConnection))
                {
                    using (System.Data.SqlClient.SqlCommand cm = sqlCon.CreateCommand())
                    {

                        cm.CommandType = System.Data.CommandType.Text;
                        cm.CommandText = sSql;
                        cm.CommandTimeout = 100;
                        System.Data.SqlClient.SqlDataAdapter sqlAdp = new System.Data.SqlClient.SqlDataAdapter(cm);

                        sqlCon.Open();
                        sqlAdp.Fill(dsResult);
                        return dsResult;
                    }
                }
            }
            catch (Exception ex)
            {
                throw Error(ex);
            }
        }
        public static System.Data.DataSet ExecuteQueryDataSet(SqlConnection cn, string sSql)
        {

            try
            {
                System.Data.DataSet dsResult = new System.Data.DataSet();

                using (System.Data.SqlClient.SqlCommand cm = cn.CreateCommand())
                {
                    cm.CommandType = System.Data.CommandType.Text;
                    cm.CommandText = sSql;
                    cm.CommandTimeout = 100;
                    System.Data.SqlClient.SqlDataAdapter sqlAdp = new System.Data.SqlClient.SqlDataAdapter(cm);

                    sqlAdp.Fill(dsResult);

                    return dsResult;
                }
            }
            catch (Exception ex)
            {
                throw Error(ex);
            }


        }

        //Jack-- For Developer use , to get a executing sql query based on procedure name and parameters
        public static string ExecuteProcQueryStringGet(string procName, List<SqlParameter> paraList)
        {
            string sampleSQLRunText = procName + " ";
            for (int i = 0; i < paraList.Count; i++)
            {
                switch (paraList.ElementAt(i).DbType)
                {

                    case DbType.Boolean:
                        sampleSQLRunText += paraList.ElementAt(i).ParameterName.ToString() + "=" + (GFunc.NEBool(paraList.ElementAt(i).Value, false) == true ? 1 : 0);
                        sampleSQLRunText += ",";
                        break;

                    case DbType.Date:
                    case DbType.DateTime:
                    case DbType.Xml:
                    case DbType.String:
                        sampleSQLRunText += paraList.ElementAt(i).ParameterName.ToString() + "=" + "'" + paraList.ElementAt(i).Value + "'";
                        sampleSQLRunText += ",";
                        break;

                    case DbType.Decimal:
                    case DbType.Double:
                    case DbType.Int16:
                    case DbType.Int32:
                    case DbType.Int64:
                    case DbType.Single:
                    case DbType.UInt16:
                    case DbType.UInt32:
                    case DbType.UInt64:
                        sampleSQLRunText += paraList.ElementAt(i).ParameterName.ToString() + "=" + paraList.ElementAt(i).Value;
                        sampleSQLRunText += ",";
                        break;

                    default:
                        break;

                }
            }
            sampleSQLRunText = sampleSQLRunText.Remove(sampleSQLRunText.Length - 1); // remove last comma(,)
            return sampleSQLRunText;
        }        

        public static System.Data.DataTable ExecuteProc(string procName, List<SqlParameter> parmList)
        {
            try
            {
                System.Data.DataTable dtResult = new System.Data.DataTable();

                using (System.Data.SqlClient.SqlConnection sqlCon = new System.Data.SqlClient.SqlConnection(Database.BossDemoConnection))
                {
                    using (System.Data.SqlClient.SqlCommand cm = sqlCon.CreateCommand())
                    {
                        cm.CommandType = System.Data.CommandType.StoredProcedure;
                        cm.CommandText = procName;
                        cm.CommandTimeout = 100000;
                        if (parmList != null)
                        {
                            foreach (SqlParameter p in parmList)
                            {
                                if (p.Value == null)
                                {
                                    cm.Parameters.Add(p);
                                }
                                else
                                    cm.Parameters.Add(p);
                            }
                        }

                        using (System.Data.SqlClient.SqlDataAdapter sqlAdp = new System.Data.SqlClient.SqlDataAdapter(cm))
                        {
                            sqlCon.Open();
                            sqlAdp.Fill(dtResult);
                        }
                    }
                }


                return dtResult;

            }
            catch (Exception ex)
            {
                throw Error(ex);
            }
        }
        
        public static System.Data.DataTable ExecuteProc(SqlConnection sqlCon, string procName, List<SqlParameter> parmList)
        {
            try
            {
                System.Data.DataSet dsResult = new System.Data.DataSet();

                using (System.Data.SqlClient.SqlCommand cm = sqlCon.CreateCommand())
                {
                    cm.CommandType = System.Data.CommandType.StoredProcedure;
                    cm.CommandText = procName;
                    cm.CommandTimeout = 100;
                    if (parmList != null)
                    {
                        foreach (SqlParameter p in parmList)
                        {
                            if (p.Value == null)
                            {
                                cm.Parameters.Add(p);
                            }
                            else
                                cm.Parameters.Add(p);
                        }
                    }

                 
                    using (System.Data.SqlClient.SqlDataAdapter sqlAdp = new System.Data.SqlClient.SqlDataAdapter(cm))
                    {                       
                        sqlAdp.Fill(dsResult);
                    }

                    DataTable dtResult = null;

                    if (dsResult.Tables.Count > 0)
                        dtResult = dsResult.Tables[0];

                    return dtResult;
                }
            }
            catch (Exception ex)
            {
                throw Error(ex);
            }
        }
        public static System.Data.DataSet ExecuteProcDataSet(string procName, List<SqlParameter> parmList)
        {
            try
            {
                System.Data.DataSet dsResult = new System.Data.DataSet();

                //using (System.Data.SqlClient.SqlConnection sqlCon = new System.Data.SqlClient.SqlConnection(Database.BossDemoConnection + ";MultipleActiveResultSets=True"))
                using (System.Data.SqlClient.SqlConnection sqlCon = new System.Data.SqlClient.SqlConnection(Database.BossDemoConnection))
                {
                    using (System.Data.SqlClient.SqlCommand cm = sqlCon.CreateCommand())
                    {
                        cm.CommandType = System.Data.CommandType.StoredProcedure;
                        cm.CommandText = procName;
                        cm.CommandTimeout = 0;
                        if (parmList != null)
                        {
                            foreach (SqlParameter p in parmList)
                            {
                                if (p.Value == null)
                                {
                                    cm.Parameters.Add(p);
                                }
                                else
                                    cm.Parameters.Add(p);
                            }
                        }

                        System.Data.SqlClient.SqlDataAdapter sqlAdp = new System.Data.SqlClient.SqlDataAdapter(cm);

                        sqlCon.Open();
                        sqlAdp.Fill(dsResult);

                        return dsResult;
                    }
                }
            }
            catch (Exception ex)
            {
                throw Error(ex);
            }

        }
        public static System.Data.DataTable ExecuteProcReader(string procName, List<SqlParameter> parmList)
        {
            try
            {
                System.Data.DataTable dsResult = new System.Data.DataTable();

                using (System.Data.SqlClient.SqlConnection sqlCon = new System.Data.SqlClient.SqlConnection(Database.BossDemoConnection))
                {
                    using (System.Data.SqlClient.SqlCommand cm = sqlCon.CreateCommand())
                    {
                        cm.CommandType = System.Data.CommandType.StoredProcedure;
                        cm.CommandText = procName;
                        cm.CommandTimeout = 0;
                        if (parmList != null)
                        {
                            foreach (SqlParameter p in parmList)
                            {
                                if (p.Value == null)
                                {
                                    cm.Parameters.Add(p);
                                }
                                else
                                    cm.Parameters.Add(p);
                            }
                        }

                        sqlCon.Open();
                        using(SqlDataReader reader=cm.ExecuteReader())
                        {
                            dsResult.Load(reader,LoadOption.OverwriteChanges);
                        }
                        //System.Data.SqlClient.SqlDataAdapter sqlAdp = new System.Data.SqlClient.SqlDataAdapter(cm);

                        //sqlCon.Open();
                        //sqlAdp.Fill(dsResult);

                        return dsResult;
                    }
                }
            }
            catch (Exception ex)
            {
                throw Error(ex);
            }
        }        

        public string ReaderToXML(SqlDataReader objReader,string ParentNodeName= "")
    {
        //'You Can use OLEDBDataReader as well
        //'If ParentNodeName is not blank, it will be used as
        //'Start End node of the XML

        string sXML="";
        int intColumnCount=0;
        int intCtr=0;
        string sValue="";

        ParentNodeName = ParentNodeName.Trim();
        try
        {
            int ColumnCount = objReader.FieldCount;
            if(ParentNodeName != "") 
                sXML += "<" + ParentNodeName + ">";

            while(objReader.Read())
            {
                //'Loop through each row
                for(int Ctr = 0; Ctr<= intColumnCount - 1;Ctr++)
                {
                    //'Get the Value of each column
                    //'Does not include nodes for null/blank values

                    if(!objReader.IsDBNull(intCtr))
                    {
                        sValue = objReader[intCtr].ToString();
                        if(sValue.ToString() != "")
                            sXML += "<" + objReader.GetName(intCtr) + ">" + XMLizeString(sValue) + "</" + objReader.GetName(intCtr) + ">";                       
                    }
                }
            }
            if (ParentNodeName != "")
                sXML += "</" + ParentNodeName + ">";

        }
        catch(Exception ex)
        {
            sXML = "";
        }
            return sXML;
        }

        private string XMLizeString(string sInput)
        {  
            if(!(IsAlphaNumeric(sInput)))
                return " " + sInput+ "]]>";
            else
                return sInput;        
        }

        private bool IsAlphaNumeric(string TestString)
        {
            string sTemp;
            int iLen;
            int iCtr;
            string sChar="";

            sTemp = TestString;
            iLen = sTemp.Length;
            if(iLen > 0)
            {
                for (iCtr = 0; iCtr < iLen; iCtr++)
                    sChar = sTemp.Substring(0,iCtr);
                    if(!"[0-9A-Za-z.:, ]".Contains(sChar))
                         return true;               
            }
            return false;
        }
        public static System.Data.DataSet ExecuteProcDataSet(SqlConnection cn, string procName, List<SqlParameter> parmList)
        {
            try
            {
                System.Data.DataSet dsResult = new System.Data.DataSet();

                using (System.Data.SqlClient.SqlCommand cm = cn.CreateCommand())
                {
                    cm.CommandType = System.Data.CommandType.StoredProcedure;
                    cm.CommandText = procName;
                    cm.CommandTimeout = 100;
                    if (parmList != null)
                    {
                        foreach (SqlParameter p in parmList)
                        {
                            if (p.Value == null)
                            {
                                cm.Parameters.Add(p);
                            }
                            else
                                cm.Parameters.Add(p);
                        }
                    }

                    System.Data.SqlClient.SqlDataAdapter sqlAdp = new System.Data.SqlClient.SqlDataAdapter(cm);

                    sqlAdp.Fill(dsResult);

                    return dsResult;


                }
            }
            catch (Exception ex)
            {
                throw Error(ex);
            }

        }
        public static void ExecuteNonQueryProc(SqlConnection sqlCon, string procName, List<SqlParameter> parmList)
        {
            try
            {
                using (System.Data.SqlClient.SqlCommand cm = sqlCon.CreateCommand())
                {
                    cm.CommandTimeout = 0;
                    cm.CommandType = System.Data.CommandType.StoredProcedure;
                    cm.CommandText = procName;

                    if (parmList != null)
                    {
                        foreach (SqlParameter p in parmList)
                        {
                            if (p.Value == null)
                            {
                                cm.Parameters.Add(p);
                            }
                            else
                                cm.Parameters.Add(p);
                        }
                    }

                    int recordAffect = cm.ExecuteNonQuery();
                }
            }
            catch (TAException ex)
            {
                throw Error(ex);
            }
            catch (Exception ex)
            {
                throw Error(ex);
            }

        }
        public static void ExecuteNonQueryProc(SqlConnection sqlCon, string procName, List<SqlParameter> parmList,int CmdTimeoutInSecs)
        {
            try
            {
                using (System.Data.SqlClient.SqlCommand cm = sqlCon.CreateCommand())
                {
                    cm.CommandTimeout = CmdTimeoutInSecs;
                    cm.CommandType = System.Data.CommandType.StoredProcedure;
                    cm.CommandText = procName;

                    if (parmList != null)
                    {
                        foreach (SqlParameter p in parmList)
                        {
                            if (p.Value == null)
                            {
                                cm.Parameters.Add(p);
                            }
                            else
                                cm.Parameters.Add(p);
                        }
                    }

                    int recordAffect = cm.ExecuteNonQuery();
                }
            }
            catch (TAException ex)
            {
                throw Error(ex);
            }
            catch (Exception ex)
            {
                throw Error(ex);
            }
        }


        public static void ExecuteNonQueryProc(string procName, List<SqlParameter> parmList)
        {
            try
            {
                using (System.Data.SqlClient.SqlConnection sqlCon = new System.Data.SqlClient.SqlConnection(Database.BossDemoConnection))
                {
                    sqlCon.Open();
                    using (System.Data.SqlClient.SqlCommand cm = sqlCon.CreateCommand())
                    {
                        cm.CommandType = System.Data.CommandType.StoredProcedure;
                        cm.CommandText = procName;
                        cm.CommandTimeout = 0;

                        if (parmList != null)
                        {
                            foreach (SqlParameter p in parmList)
                            {
                                if (p.Value == null)
                                {
                                    cm.Parameters.Add(p);
                                }
                                else
                                    cm.Parameters.Add(p);
                            }
                        }

                        int recordAffect = cm.ExecuteNonQuery();
                    }
                }

            }
            catch (Exception ex)
            {
                throw Error(ex);
            }

        }

        public static DataTable ConvertXMLStringToDataTable(string xmlString)
        {
            DataSet ds = new DataSet();
            XmlDocument doc = new XmlDocument();
            doc.LoadXml(xmlString);
            XmlNodeReader nor = new XmlNodeReader(doc);
            ds.ReadXml(nor);
            return ds.Tables[0];
        }
        public static string ConvertDataTableToXML(DataTable dtBuildSQL)
        {
            try
            {
                DataSet dsBuildSQL = new DataSet();
                StringBuilder sbSQL;
                StringWriter swSQL;
                string XMLformat;
                sbSQL = new StringBuilder();
                swSQL = new StringWriter(sbSQL);

                var ds = new DataSet { Locale = CultureInfo.InvariantCulture };


                bool containsDate = false;
                var target = CopyDataTableStructure(dtBuildSQL);

                foreach (DataColumn col in target.Columns)
                {
                    if (col.DataType == System.Type.GetType("System.DateTime"))
                    {
                        col.DateTimeMode = DataSetDateTime.Utc;
                        containsDate = true;
                    }
                }

                if (containsDate)
                {
                    foreach (DataRow row in dtBuildSQL.Rows)
                        target.ImportRow(row);
                    target.TableName = dtBuildSQL.TableName;
                    ds.Tables.Add(target);
                }
                else
                {
                    ds.Tables.Add(CopyDataTable(dtBuildSQL));
                }

                dsBuildSQL = ds;


                foreach (DataColumn col in dsBuildSQL.Tables[0].Columns)
                {
                    col.ColumnMapping = MappingType.Attribute;

                }
                dsBuildSQL.WriteXml(swSQL, XmlWriteMode.WriteSchema);
                XMLformat = sbSQL.ToString(); return XMLformat;
            }
            catch (Exception ex)
            {
                throw Error(ex);
            }
        }
        public static string ConvertDataTableToXML(DataSet dsBuildSQL)
        {
            try
            {
                StringBuilder sbSQL;
                StringWriter swSQL;
                string XMLformat;
                sbSQL = new StringBuilder();
                swSQL = new StringWriter(sbSQL);

                var ds = new DataSet { Locale = CultureInfo.InvariantCulture };

                foreach (DataTable source in dsBuildSQL.Tables)
                {
                    bool containsDate = false;
                    DataTable target = CopyDataTableStructure(source);
                    target.TableName = source.TableName;
                    foreach (DataColumn col in target.Columns)
                    {
                        if (col.DataType == System.Type.GetType("System.DateTime"))
                        {
                            col.DateTimeMode = DataSetDateTime.Utc;
                            containsDate = true;
                        }
                    }

                    if (containsDate)
                    {
                        foreach (DataRow row in source.Rows)
                            target.ImportRow(row);
                        ds.Tables.Add(target);
                    }
                    else
                    {
                        ds.Tables.Add(CopyDataTable(source));
                    }

                }
                dsBuildSQL = ds;

                foreach (DataTable dt in dsBuildSQL.Tables)
                {
                    foreach (DataColumn col in dt.Columns)
                    {
                        col.ColumnMapping = MappingType.Attribute;
                    }
                }
                dsBuildSQL.WriteXml(swSQL, XmlWriteMode.WriteSchema);
                XMLformat = sbSQL.ToString(); return XMLformat;
            }
            catch (Exception ex)
            {
                throw Error(ex);
            }
        }
        public static string ConvertObjectToXML(object obj)
        {
            string xmlString = "";
            try
            {

                if (obj == null)
                    return xmlString;
                if (obj.GetType().BaseType == typeof(DataTable) || obj.GetType() == typeof(DataTable))
                {
                    DataTable dt = new DataTable();

                    GFunc.CopyDataTableToDetailObject((DataTable)obj, dt);
                    xmlString = GFunc.ConvertDataTableToXML(dt);

                }
                else if (obj.GetType().BaseType.ToString().Contains("BusinessListBase"))
                {
                    ////obj.GetType()
                    ////foreach(object busines in ob
                    ////Type t = obj.GetType().MakeGenericType(typeof(int));
                    //MethodInfo method = obj.GetType().UnderlyingSystemType 
                    //                BindingFlags.Public | BindingFlags.Static);

                    //// Build a method with the specific type argument you're interested in
                    //method = method.MakeGenericMethod(typeOne);


                    //////object objList = Activator.CreateInstance(System.Type.GetType(obj.GetType().Name.Substring(0, obj.GetType().Name.Length - 1)));
                    ////DataTable dt = ((DataTable)obj).Clone();
                    //////System.Type.GetType(obj.GetType().UnderlyingSystemType.Name);
                    Type type = obj.GetType().UnderlyingSystemType;
                    Type eType = type.GetElementType();
                    DataTable dt = new DataTable();
                    switch (type.Name)
                    {
                        case "SYSAttachments":
                            SYSAttachments attList = obj as SYSAttachments;
                            List<SYSAttachment> att = attList.ToList<SYSAttachment>();
                            dt = GFunc.ConvertListToDataTable<SYSAttachment>(att);
                            break;
                    }


                    xmlString = GFunc.ConvertDataTableToXML(dt);

                }
                else
                {
                    DataTable dt = GFunc.ConvertObjectToDataTable(obj, "dtHeader");
                    xmlString = GFunc.ConvertDataTableToXML(dt);
                }
            }
            catch (Exception ex)
            {
                throw Error(ex);
            }
            return xmlString;

        }

        public static void XMLOutput(List<SqlParameter> paraList, string spName)
        {
            try
            {
                string spPara = "exec " + spName + " ";
                string xmlPara = "";
                foreach (SqlParameter para in paraList)
                {

                    if (para.ParameterName.Contains("xml") && para.Value != null)
                    {
                        spPara = spPara + para.ParameterName + "=" + para.ParameterName + "Local,";
                        xmlPara = xmlPara + "\r\n";
                        xmlPara = xmlPara + "BEGIN";
                        xmlPara = xmlPara + "\r\n";
                        xmlPara = xmlPara + "Declare " + para.ParameterName + "Local xml ='" + para.Value.ToString() + "'";
                        //System.IO.File.WriteAllText(@"C:\Temp\" + spName + " Para " + para.ParameterName + ".txt", para.Value.ToString());
                        xmlPara = xmlPara + "\r\n";
                        xmlPara = xmlPara + "END";
                    }
                    else

                        spPara = spPara + para.ParameterName + "=" + (para.DbType == DbType.String ? "'" + para.Value + "'" : para.Value) + ",";
                }
                xmlPara = xmlPara + "\r\n";
                spPara = "BEGIN TRANSACTION" + "\r\n" + spPara.Substring(0, spPara.Length - 1) + "\r\n";
                spPara = spPara + "ROLLBACK TRANSACTION";
                System.IO.File.WriteAllText(@"C:\Temp\" + spName + ".txt", xmlPara + spPara);
            }
            catch (Exception ex)
            {
                throw Error(ex);
            }
            finally
            {
                throw new Exception("Roll back for xml data test");
            }
        }

        public static DataTable ConvertObjectToDataTable(Object obj, string dtName)
        {
            try
            {
                DataTable table = new DataTable(dtName);
                PropertyInfo[] properties = obj.GetType().GetProperties();

                //use to test Type of Property
                //DataTable dt = new DataTable();
                //dt.Columns.Add("PropertiesName", typeof(string));
                //dt.Columns.Add("PropertiesType", typeof(string));

                foreach (PropertyInfo pi in properties)
                {
                    if (GFunc.CompareString(pi.Name, "Item"))
                    {
                        continue;
                    }
                    //use to test Type of Property
                    //DataRow dr = dt.NewRow();
                    //dr[0] = pi.Name;
                    //dr[1] = pi.PropertyType.BaseType;
                    //dt.Rows.Add(dr);
                    if (pi.PropertyType.BaseType == null)
                        continue;

                    if (!pi.PropertyType.BaseType.ToString().ToUpper().Contains("CSLA"))
                    {
                        table.Columns.Add(pi.Name, System.Type.GetType(pi.PropertyType.ToString().Replace("System.Nullable`1[", "").Replace("]", "")));
                    }
                }

                DataRow row = table.NewRow();
                foreach (PropertyInfo pi in properties)
                {
                    if (GFunc.CompareString(pi.Name, "Item"))
                    { continue; }
                    if (pi.PropertyType.BaseType == null)
                        continue;

                    if (!pi.PropertyType.BaseType.ToString().ToUpper().Contains("CSLA"))
                    {
                        row[pi.Name] = IsNE(obj.GetType().InvokeMember(pi.Name, BindingFlags.GetProperty, null, obj, null)) ? System.DBNull.Value : obj.GetType().InvokeMember(pi.Name, BindingFlags.GetProperty, null, obj, null);
                    }

                }
                table.Rows.Add(row);
                return table;
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
        public static DataTable ConvertSqlParamListToDataTable(List<SqlParameter> paraList)
        {
            try
            {
                DataTable table = new DataTable();
                table.Columns.Add("ParameterName");
                table.Columns.Add("ParameterValue");
                table.Columns.Add("ParameterDataType", typeof(string));
                table.PrimaryKey = new DataColumn[] { table.Columns["ParameterName"] };
                foreach (SqlParameter para in paraList)
                {
                    DataRow drNew = table.NewRow();
                    drNew["ParameterName"] = para.ParameterName.Replace("@", "");
                    drNew["ParameterValue"] = para.Value;
                    drNew["ParameterDataType"] = para.SqlDbType.ToString();
                    table.Rows.Add(drNew);
                }

                return table;
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
        public static DataTable ConvertActiveGridRowtToDataTable(UltraGridRow obj)
        {
            try
            {
                DataTable table = new DataTable();
                PropertyInfo[] properties = obj.GetType().GetProperties();

                //use to test Type of Property
                //DataTable dt = new DataTable();
                //dt.Columns.Add("PropertiesName", typeof(string));
                //dt.Columns.Add("PropertiesType", typeof(string));

                foreach (UltraGridCell pi in obj.Cells)
                {
                    table.Columns.Add(pi.Column.Key, pi.Column.DataType);
                }

                DataRow row = table.NewRow();
                foreach (UltraGridCell pi in obj.Cells)
                {

                    if (GFunc.IsNE(pi.Value))
                        continue;


                    row[pi.Column.Key] = pi.Value;


                }
                table.Rows.Add(row);
                return table;
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
        public static DataTable ConvertListToDataTable<T>(List<T> objList)
        {
            DataTable table = new DataTable();
            //            Type type = System.Type.GetType(objList.GetType().UnderlyingSystemType.Name);
            PropertyInfo[] properties = typeof(T).GetProperties();
            foreach (PropertyInfo pi in properties)
            {
                if (GFunc.CompareString(pi.Name, "Item"))
                {
                    continue;
                }
                if (pi.PropertyType.BaseType == null)
                    continue;

                if (!pi.PropertyType.BaseType.ToString().ToUpper().Contains("CSLA"))
                {
                    table.Columns.Add(pi.Name, System.Type.GetType(pi.PropertyType.ToString().Replace("System.Nullable`1[", "").Replace("]", "")));
                }
            }

            foreach (T obj in objList)
            {
                DataRow row = table.NewRow();
                foreach (PropertyInfo pi in properties)
                {
                    if (GFunc.CompareString(pi.Name, "Item"))
                    { continue; }
                    if (pi.PropertyType.BaseType == null)
                        continue;

                    if (!pi.PropertyType.BaseType.ToString().ToUpper().Contains("CSLA"))
                    {
                        row[pi.Name] = GFunc.IsNE(obj.GetType().InvokeMember(pi.Name, BindingFlags.GetProperty, null, obj, null)) ? System.DBNull.Value : obj.GetType().InvokeMember(pi.Name, BindingFlags.GetProperty, null, obj, null);
                    }

                }
                table.Rows.Add(row);
            }
            return table;
        }

        public static void ConvertDataTableToObjectList<T>(DataTable dt, List<T> objList)
        {
            try
            {
                foreach (DataRow dr in dt.Rows)
                {
                    T obj = (T)Activator.CreateInstance(typeof(T), true); ;
                    PropertyInfo[] properties = obj.GetType().GetProperties();
                    foreach (PropertyInfo pi in properties)
                    {
                        if (GFunc.CompareString(pi.Name, "Item"))
                        {
                            continue;
                        }
                        if (dt.Columns.Contains(pi.Name))
                        {
                            if (dr[pi.Name] != System.DBNull.Value)
                            {
                                if (pi.CanWrite)
                                {
                                    pi.SetValue(obj, dr[pi.Name], null);
                                }
                            }
                        }

                        if (pi.Name.StartsWith("Svr"))
                        {
                            if (dt.Columns.Contains(pi.Name.Substring(3)))
                            {
                                if (dr[pi.Name.Substring(3)] != System.DBNull.Value)
                                {
                                    if (pi.CanWrite)
                                    {
                                        pi.SetValue(obj, dr[pi.Name.Substring(3)], null);
                                    }
                                }
                            }
                        }
                    }
                    if (objList == null)
                        objList = new List<T>();
                    objList.Add(obj);
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
        public static Object ConvertDataTableToObject(DataTable dt, Object obj)
        {
            try
            {
                if ((obj.GetType()).AssemblyQualifiedName == "BOLib.SYSAttachments, BOLib, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null")
                {
                    if (dt.Rows.Count == 0)
                        return obj;

                    SYSAttachment objAtt = new SYSAttachment();
                    ((SYSAttachments)obj).Add(objAtt);

                    PropertyInfo[] properties = objAtt.GetType().GetProperties();
                    foreach (PropertyInfo pi in properties)
                    {
                        if (GFunc.CompareString(pi.Name, "Item"))
                        {
                            continue;
                        }
                        
                        if (dt.Columns.Contains(pi.Name))
                        {
                            if (dt.Rows[0][pi.Name] != System.DBNull.Value)
                            {
                                if (pi.CanWrite)
                                {
                                    pi.SetValue(objAtt, dt.Rows[0][pi.Name], null);
                                }
                            }
                        }

                        if (pi.Name.StartsWith("Svr"))
                        {
                            if (dt.Columns.Contains(pi.Name.Substring(3)))
                            {
                                if (dt.Rows[0][pi.Name.Substring(3)] != System.DBNull.Value)
                                {
                                    if (pi.CanWrite)
                                    {
                                        pi.SetValue(objAtt, dt.Rows[0][pi.Name.Substring(3)], null);
                                    }
                                }
                            }
                        }
                    }
                }
                else
                {
                    PropertyInfo[] properties = obj.GetType().GetProperties();
                    foreach (PropertyInfo pi in properties)
                    {
                        if (GFunc.CompareString(pi.Name, "Item"))
                        {
                            continue;
                        }
                        if (dt.Columns.Contains(pi.Name))
                        {
                            if (dt.Rows[0][pi.Name] != System.DBNull.Value)
                            {
                                if (pi.CanWrite)
                                {
                                    pi.SetValue(obj, dt.Rows[0][pi.Name], null);
                                }
                            }
                        }

                        if (pi.Name.StartsWith("Svr"))
                        {
                            if (dt.Columns.Contains(pi.Name.Substring(3)))
                            {
                                if (dt.Rows[0][pi.Name.Substring(3)] != System.DBNull.Value)
                                {
                                    if (pi.CanWrite)
                                    {
                                        pi.SetValue(obj, dt.Rows[0][pi.Name.Substring(3)], null);
                                    }
                                }
                            }
                        }
                    }
                }
                return obj;
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
        public static void CopyDataTableToDetailObject(DataTable dtSource, DataTable dtDest)
        {

            try
            {
                if (dtDest.Columns.Count == 0) // Assume New Table
                {
                    foreach (DataColumn dcSource in dtSource.Columns)
                    {
                        dtDest.Columns.Add(dcSource.ColumnName, dcSource.DataType);
                    }
                }
                foreach (DataRow drSource in dtSource.Rows)
                {
                    DataRow dtNew = dtDest.NewRow();

                    foreach (DataColumn dcSource in dtSource.Columns)
                    {
                        if (dcSource.ColumnName.ToLower() == "itmapplypayamtf")
                        {
                        }
                        if (dtDest.Columns.Contains(dcSource.ColumnName))
                        {
                            if (drSource[dcSource.ColumnName] != null)
                            {
                                dtNew[dcSource.ColumnName] = drSource[dcSource.ColumnName];
                            }
                        }
                    }
                    dtDest.Rows.Add(dtNew);

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
        public static DataTable CopyDataTableStructure(DataTable dtSource)
        {
            DataTable dtDest = new DataTable();
            try
            {

                foreach (DataColumn dcSource in dtSource.Columns)
                {
                    dtDest.Columns.Add(dcSource.ColumnName, dcSource.DataType);
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
            return dtDest;
        }
        public static DataTable CopyDataTable(DataTable dtSource)
        {
            DataTable dtDest = new DataTable();
            dtDest.TableName = dtSource.TableName;
            try
            {
                foreach (DataColumn dcSource in dtSource.Columns)
                {
                    dtDest.Columns.Add(dcSource.ColumnName, dcSource.DataType);
                }

                foreach (DataRow drSource in dtSource.Rows)
                {
                    DataRow dtNew = dtDest.NewRow();
                    foreach (DataColumn dcSource in dtSource.Columns)
                    {
                        if (dtDest.Columns.Contains(dcSource.ColumnName))
                        {
                            if (drSource[dcSource.ColumnName] != null)
                            {
                                dtNew[dcSource.ColumnName] = drSource[dcSource.ColumnName];
                            }
                        }
                    }
                    dtDest.Rows.Add(dtNew);

                }

            }
            catch (Exception ex)
            {
                throw Error(ex);
            }
            return dtDest;
        }
        public static List<SqlParameter> CopyUsedSqlParameter(List<SqlParameter> paraSource)
        {

            try
            {
                List<SqlParameter> copyPara = new List<SqlParameter>();
                foreach (SqlParameter para in paraSource)
                {
                    copyPara.Add(new SqlParameter(para.ParameterName, para.Value));
                }

                return copyPara;
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
        public static int GetExistingRecKey(string value, GEnum.SystemCode code, bool retByID, bool displyAccessMsg)
        {
            try
            {
                string Sql = string.Empty;
                int RecKey = 0;

                if (value.IndexOf("'") > -1)
                    value = value.Replace("'", "''");
                switch (code)
                {
                    //@UserAccessLevel>=@RecAccessLevel OR @UserAccessGroup=@RecAccessGroup
                    case GEnum.SystemCode.Customer:
                        if (retByID)

                            Sql = "SELECT ConKey FROM MST_Con(nolock) WHERE ConID='" + value + "' AND ConType IN (10,30)";
                        else
                            Sql = "SELECT ConKey FROM MST_Con(nolock) WHERE ConNm='" + value + "' AND ConType IN (10,30)";
                        break;
                    case GEnum.SystemCode.Account:
                        if (retByID)
                            Sql = "SELECT AccKey FROM MST_Acc(nolock) WHERE AccID='" + value + "' AND AccCurrKey=1 ";
                        else
                            Sql = "SELECT AccKey FROM MST_Acc(nolock) WHERE AccDes='" + value + "' AND AccCurrKey=1";
                        break;
                    case GEnum.SystemCode.Inventory:
                        if (retByID)
                            Sql = "SELECT ItmKey FROM MST_Itm(nolock) WHERE ItmID='" + value + "'";
                        else
                            Sql = "SELECT ItmKey FROM MST_Itm(nolock) WHERE ItmDes='" + value + "'";
                        break;
                    case GEnum.SystemCode.Job:
                        if (retByID)
                            Sql = "SELECT JobKey FROM MST_Job(nolock) WHERE JobID='" + value + "'";
                        else
                            Sql = "SELECT JobKey FROM MST_Job(nolock) WHERE JobDes='" + value + "'";
                        break;
                    case GEnum.SystemCode.Vendor:
                        if (retByID)
                            Sql = "SELECT ConKey FROM MST_Con(nolock) WHERE ConID='" + value + "' AND ConType IN (20,30)";
                        else
                            Sql = "SELECT ConKey FROM MST_Con(nolock) WHERE ConNm='" + value + "' AND ConType IN (20,30)";
                        break;
                }


                //Changed
                DataTable dt = GFunc.ExecuteQuery(Sql);
                if (dt != null)
                {
                    if (dt.Rows.Count > 0)
                    {
                        Int32.TryParse(dt.Rows[0][0].ToString(), out RecKey);

                        switch (code)
                        {

                            case GEnum.SystemCode.Customer:

                                Sql = string.Format("SELECT ConKey FROM MST_Con(nolock) WHERE ConKey={0} AND ConType IN (10,30) " +
                                                    "AND (AccessLevel<={1} OR (AccessGroup in  (select grpkey from SeC_UserDetGrp(nolock) where userKey= {2}) And AccessLevel={1}))", RecKey, AppInfor.ConAccessLevel, AppInfor.CurrentUserKey);
                                break;
                            case GEnum.SystemCode.Job:
                                Sql = string.Format("SELECT JobKey FROM MST_Job(nolock) WHERE JobKey={0} " +
                                                    "AND (AccessLevel<={1} OR (AccessGroup in  (select grpkey from SeC_UserDetGrp(nolock) where userKey= {2}) And AccessLevel={1}))", RecKey, AppInfor.JobAccessLevel, AppInfor.CurrentUserKey);

                                break;
                            case GEnum.SystemCode.Inventory:
                                Sql = string.Format("SELECT ItmKey FROM MST_Itm(nolock) WHERE ItmKey={0} " +
                                                    "AND (AccessLevel<={1} OR (AccessGroup in  (select grpkey from SeC_UserDetGrp(nolock) where userKey= {2}) And AccessLevel={1}))", RecKey, AppInfor.ItemAccessLevel, AppInfor.CurrentUserKey);
                                break;
                            case GEnum.SystemCode.Vendor:
                                Sql = string.Format("SELECT ConKey FROM MST_Con(nolock) WHERE ConKey={0} AND ConType IN (20,30) " +
                                                    "AND (AccessLevel<={1} OR (AccessGroup in  (select grpkey from SeC_UserDetGrp(nolock) where userKey= {2}) And AccessLevel={1}))", RecKey, AppInfor.ConAccessLevel, AppInfor.CurrentUserKey);
                                break;
                            case GEnum.SystemCode.Account:
                                return RecKey;

                        }//End Switch

                    }//End If
                    else
                        return RecKey;

                }//End If 


                DataTable dtAccess = GFunc.ExecuteQuery(Sql);
                if (dt != null)
                {
                    if (dtAccess.Rows.Count == 0)
                    {
                        if (displyAccessMsg)
                        {
                            MsgBox.Show(MsgID.Permission.PermOpenRecIsFalse);
                            return 0;
                        }
                    }
                }

                return RecKey;
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
        public static DataTable GetExistingRec(string value, GEnum.SystemCode code, bool retByID, bool displyAccessMsg)
        {
            try
            {
                string Sql = string.Empty;
                int RecKey = 0;

                switch (code)
                {
                    //@UserAccessLevel>=@RecAccessLevel OR @UserAccessGroup=@RecAccessGroup
                    case GEnum.SystemCode.Customer:
                        if (retByID)
                            Sql = "SELECT ConKey,ConID,ConNm FROM MST_Con(nolock) WHERE ConID='" + value + "' AND ConType IN (10,30)";
                        else
                            Sql = "SELECT ConKey,ConID,ConNm FROM MST_Con(nolock) WHERE ConNm='" + value + "' AND ConType IN (10,30)";
                        break;
                    case GEnum.SystemCode.Account:
                        if (retByID)
                            Sql = "SELECT AccKey,AccID,AccDes FROM MST_Acc(nolock) WHERE AccID='" + value + "' AND AccCurrKey=1 ";
                        else
                            Sql = "SELECT AccKey,AccID,AccDes FROM MST_Acc(nolock) WHERE AccDes='" + value + "' AND AccCurrKey=1";
                        break;
                    case GEnum.SystemCode.Inventory:
                        if (retByID)
                            Sql = "SELECT ItmKey,ItmID,ItmDes FROM MST_Itm(nolock) WHERE ItmID='" + value + "'";
                        else
                            Sql = "SELECT ItmKey,ItmID,ItmDes FROM MST_Itm(nolock) WHERE ItmDes='" + value + "'";
                        break;
                    case GEnum.SystemCode.Job:
                        if (retByID)
                            Sql = "SELECT JobKey,JobID,JobDes FROM MST_Job(nolock) WHERE JobID='" + value + "'";
                        else
                            Sql = "SELECT JobKey,JobID,JobDes FROM MST_Job(nolock) WHERE JobDes='" + value + "'";
                        break;
                    case GEnum.SystemCode.Vendor:
                        if (retByID)
                            Sql = "SELECT ConKey,ConID,ConNm FROM MST_Con(nolock) WHERE ConID='" + value + "' AND ConType IN (20,30)";
                        else
                            Sql = "SELECT ConKey,ConID,ConNm FROM MST_Con(nolock) WHERE ConNm='" + value + "' AND ConType IN (20,30)";
                        break;
                }


                //Changed 
                DataTable dt = GFunc.ExecuteQuery(Sql);
                if (dt != null)
                {
                    if (dt.Rows.Count > 0)
                    {
                        Int32.TryParse(dt.Rows[0][0].ToString(), out RecKey);

                        switch (code)
                        {

                            case GEnum.SystemCode.Customer:

                                Sql = string.Format("SELECT ConKey FROM MST_Con(nolock) WHERE ConKey={0} AND ConType IN (10,30) " +
                                                    "AND (AccessLevel<={1} OR (AccessGroup in  (select grpkey from SeC_UserDetGrp(nolock) where userKey= {2}) And AccessLevel={1}))", RecKey, AppInfor.ConAccessLevel, AppInfor.CurrentUserKey);
                                break;
                            case GEnum.SystemCode.Job:
                                Sql = string.Format("SELECT JobKey FROM MST_Job(nolock) WHERE JobKey={0} " +
                                                    "AND (AccessLevel<={1} OR (AccessGroup in  (select grpkey from SeC_UserDetGrp(nolock) where userKey= {2}) And AccessLevel={1}))", RecKey, AppInfor.JobAccessLevel, AppInfor.CurrentUserKey);

                                break;
                            case GEnum.SystemCode.Inventory:
                                Sql = string.Format("SELECT ItmKey FROM MST_Itm(nolock) WHERE ItmKey={0} " +
                                                    "AND (AccessLevel<={1} OR (AccessGroup in  (select grpkey from SeC_UserDetGrp(nolock) where userKey= {2}) And AccessLevel={1}))", RecKey, AppInfor.ItemAccessLevel, AppInfor.CurrentUserKey);
                                break;
                            case GEnum.SystemCode.Vendor:
                                Sql = string.Format("SELECT ConKey FROM MST_Con(nolock) WHERE ConKey={0} AND ConType IN (20,30) " +
                                                    "AND (AccessLevel<={1} OR (AccessGroup in  (select grpkey from SeC_UserDetGrp(nolock) where userKey= {2}) And AccessLevel={1}))", RecKey, AppInfor.ConAccessLevel, AppInfor.CurrentUserKey);
                                break;
                            case GEnum.SystemCode.Account:
                                return dt;

                        }//End Switch

                    }//End If
                    return dt;

                }//End If 


                DataTable dtAccess = GFunc.ExecuteQuery(Sql);
                if (dt != null)
                {
                    if (dtAccess.Rows.Count == 0)
                    {
                        if (displyAccessMsg)
                        {
                            MsgBox.Show(MsgID.Permission.PermOpenRecIsFalse);
                            return null;
                        }
                    }
                }

                return dt;
            }
            catch (TAException tex)
            {
                throw Error(tex);
            }
            catch (Exception ex)
            {
                throw Error(ex);
            }

            #region Old Code
            //string Sql = string.Empty;
            //int RecKey = 0;

            //switch (code)
            //{
            //    //@UserAccessLevel>=@RecAccessLevel OR @UserAccessGroup=@RecAccessGroup
            //    case GEnum.SystemCode.Customer:
            //        if (retByID)
            //            Sql = "SELECT * FROM MST_Con WHERE ConID='" + value +
            //                  "' AND ConType IN (10,30) AND (AccessLevel<=" + AppInfor.ConAccessLevel
            //                  + " OR AccessGroup=" + AppInfor.ConAccessGroup + ")";

            //        else
            //            Sql = "SELECT * FROM MST_Con WHERE ConNm='" + value
            //                + "' AND ConType IN (10,30) AND (AccessLevel<=" + AppInfor.ConAccessLevel
            //                + " OR AccessGroup=" + AppInfor.ConAccessGroup + ")";
            //        break;
            //    case GEnum.SystemCode.Account:
            //        if (retByID)
            //            Sql = "SELECT * FROM MST_Acc WHERE AccID='" + value + "' AND AccCurrKey=1 ";
            //        else
            //            Sql = "SELECT * FROM MST_Acc WHERE AccDes='" + value + "' AND AccCurrKey=1";
            //        break;
            //    case GEnum.SystemCode.Inventory:
            //        if (retByID)
            //            Sql = "SELECT * FROM MST_Itm WHERE ItmID='" + value +
            //                "' AND (AccessLevel<=" + AppInfor.ItemAccessLevel
            //                + " OR AccessGroup=" + AppInfor.ItemAccessGroup + ")";
            //        else
            //            Sql = "SELECT * FROM MST_Itm WHERE ItmDes='" + value
            //                + "' AND (AccessLevel<=" + AppInfor.ItemAccessLevel
            //                + " OR AccessGroup=" + AppInfor.ItemAccessGroup + ")";
            //        break;
            //    case GEnum.SystemCode.Job:
            //        if (retByID)
            //            Sql = "SELECT * FROM MST_Job WHERE JobID='" + value +
            //                "' AND (AccessLevel<=" + AppInfor.JobAccessLevel
            //                + " OR AccessGroup=" + AppInfor.JobAccessGroup + ")";
            //        else
            //            Sql = "SELECT * FROM MST_Job WHERE JobDes='" + value
            //                + "' AND (AccessLevel<=" + AppInfor.JobAccessLevel
            //                + " OR AccessGroup=" + AppInfor.JobAccessGroup + ")";
            //        break;
            //    case GEnum.SystemCode.Vendor:
            //        if (retByID)
            //            Sql = "SELECT * FROM MST_Con WHERE ConID='" + value +
            //                "' AND ConType IN (20,30) AND (AccessLevel<=" + AppInfor.ConAccessLevel
            //                + " OR AccessGroup=" + AppInfor.ConAccessGroup + ")";
            //        else
            //            Sql = "SELECT * FROM MST_Con WHERE ConNm='" + value
            //                + "' AND ConType IN (20,30) AND (AccessLevel<=" + AppInfor.ConAccessLevel
            //                + " OR AccessGroup=" + AppInfor.ConAccessGroup + ")";
            //        break;
            //}
            //DataTable dt = GFunc.ExecuteQuery(Sql);

            //return dt; 
            #endregion
        }


        public static System.Data.DataTable Record_GetKey(int docCode, GEnum.RecAccessType AccessType, string searchValue, int conKey, GEnum.ConTypeCust conTypeCust, GEnum.ConTypeVend conTypeVend, GEnum.CCBType ccbType, bool shwMsg)
        {
            try
            {
                #region declaration

                DataTable dt;
                const int Key = 10;
                const int ID = 20;
                const int Des = 30;

                const int Cust = 10;
                const int Vend = 20;
                const int Pros = 40;
                const int Credit = 10;
                const int Cash = 20;
                const int Both = 30;

                int opt = 0;                    //0: Customer, 1: Vendor, 2: Item, 3:Substitute, 4: Job
                int matchOption = 0;            //0: Start, 1:AnyPart
                int searchType = 0;             //10: Key, 20: ID, 30: Des
                int searchKey = 0;
                string searchStr = string.Empty;
                int accessLevel = 0;

                int conCust = 0;
                int conVend = 0;
                int conBoth = 0;
                int prospect = 0;
                int ccbCredit = 0;
                int ccbCash = 0;
                int ccbBoth = 0;

                #endregion



                #region Set ConTypeCust
                switch (AccessType)
                {
                    case GEnum.RecAccessType.CustKey:
                    case GEnum.RecAccessType.CustID:
                    case GEnum.RecAccessType.CustNm:
                        switch (conTypeCust)
                        {
                            case GEnum.ConTypeCust.ALL:
                                conCust = Cust;
                                conBoth = Both;
                                prospect = Pros;
                                break;

                            case GEnum.ConTypeCust.B:
                                conBoth = Both;
                                break;

                            case GEnum.ConTypeCust.C:
                                conCust = Cust;
                                break;

                            case GEnum.ConTypeCust.CB:
                                conCust = Cust;
                                conBoth = Both;
                                break;

                            case GEnum.ConTypeCust.P:
                                prospect = Pros;
                                break;
                        }
                        break;
                }
                #endregion

                #region Set ConTypeVend
                switch (AccessType)
                {
                    case GEnum.RecAccessType.VendKey:
                    case GEnum.RecAccessType.VendID:
                    case GEnum.RecAccessType.VendNm:
                        switch (conTypeVend)
                        {
                            case GEnum.ConTypeVend.ALL:
                                conVend = Vend;
                                conBoth = Both;
                                break;

                            case GEnum.ConTypeVend.B:
                                conBoth = Both;
                                break;

                            case GEnum.ConTypeVend.V:
                                conVend = Vend;
                                break;
                        }
                        break;
                }
                #endregion

                #region Set ccbType
                switch (AccessType)
                {
                    case GEnum.RecAccessType.CustKey:
                    case GEnum.RecAccessType.CustID:
                    case GEnum.RecAccessType.CustNm:
                        switch (ccbType)
                        {
                            case GEnum.CCBType.ALL:
                                ccbCredit = Credit;
                                ccbCash = Cash;
                                ccbBoth = Both;
                                break;

                            case GEnum.CCBType.B:
                                ccbBoth = Both;
                                break;

                            case GEnum.CCBType.CH:
                                ccbCash = Cash;
                                ccbBoth = Both;
                                break;

                            case GEnum.CCBType.CR:
                                ccbCredit = Credit;
                                ccbBoth = Both;
                                break;
                        }
                        break;
                }

                #endregion

                #region Set searchType, searchKey, searchStr
                switch (AccessType)
                {
                    case GEnum.RecAccessType.CustKey:
                    case GEnum.RecAccessType.VendKey:
                    case GEnum.RecAccessType.ItemKey:
                    case GEnum.RecAccessType.ItemKeySub:
                    case GEnum.RecAccessType.JobKey:
                    case GEnum.RecAccessType.AccKey:
                        searchType = Key;
                        searchKey = GFunc.NEInt(searchValue, 0);
                        break;

                    case GEnum.RecAccessType.CustID:
                    case GEnum.RecAccessType.VendID:
                    case GEnum.RecAccessType.ItemID:
                    case GEnum.RecAccessType.ItemIDSub:
                    case GEnum.RecAccessType.JobID:
                    case GEnum.RecAccessType.AccID:
                        searchType = ID;
                        searchStr = searchValue;
                        break;

                    case GEnum.RecAccessType.CustNm:
                    case GEnum.RecAccessType.VendNm:
                    case GEnum.RecAccessType.ItemDes:
                    case GEnum.RecAccessType.ItemDesSub:
                    case GEnum.RecAccessType.JobDes:
                    case GEnum.RecAccessType.AccDes:
                        searchType = Des;
                        searchStr = searchValue;
                        break;
                }
                #endregion

                #region Set option value
                switch (AccessType)
                {
                    case GEnum.RecAccessType.CustKey:
                    case GEnum.RecAccessType.CustID:
                    case GEnum.RecAccessType.CustNm:
                        opt = 0;
                        accessLevel = AppInfor.ConAccessLevel;


                        //if (SysOptionUtility.SearchCustomerMatch == GEnum.SearchMatchOption.StartofField)
                        //    matchOption = 0;
                        //else
                        //    matchOption = 1;

                        break;

                    case GEnum.RecAccessType.VendKey:
                    case GEnum.RecAccessType.VendID:
                    case GEnum.RecAccessType.VendNm:
                        opt = 1;
                        accessLevel = AppInfor.ConAccessLevel;

                        //if (SysOptionUtility.SearchVendorMatch == GEnum.SearchMatchOption.StartofField)
                        //    matchOption = 0;
                        //else
                        //    matchOption = 1;

                        break;

                    case GEnum.RecAccessType.ItemKey:
                    case GEnum.RecAccessType.ItemKeySub:
                    case GEnum.RecAccessType.ItemID:
                        if (docCode > 0)
                            opt = 5;
                        else
                            opt = 2;

                        accessLevel = AppInfor.ItemAccessLevel;


                        //if (SysOptionUtility.SearchItemMatch == GEnum.SearchMatchOption.StartofField)
                        //    matchOption = 0;
                        //else
                        //    matchOption = 1;

                        break;

                    case GEnum.RecAccessType.ItemIDSub:
                    case GEnum.RecAccessType.ItemDes:
                    case GEnum.RecAccessType.ItemDesSub:
                        if (docCode > 0)
                            opt = 6;
                        else
                            opt = 3;

                        accessLevel = AppInfor.ItemAccessGroup;


                        //if (SysOptionUtility.SearchItemMatch == GEnum.SearchMatchOption.StartofField)
                        //    matchOption = 0;
                        //else
                        //    matchOption = 1;

                        break;

                    case GEnum.RecAccessType.JobKey:
                    case GEnum.RecAccessType.JobID:
                    case GEnum.RecAccessType.JobDes:
                        accessLevel = AppInfor.jobAccessLevel;
                        opt = 4;


                        //if (SysOptionUtility.SearchJobMatch == GEnum.SearchMatchOption.StartofField)
                        //    matchOption = 0;
                        //else
                        //    matchOption = 1;

                        break;

                    case GEnum.RecAccessType.AccKey:
                    case GEnum.RecAccessType.AccID:
                    case GEnum.RecAccessType.AccDes:
                        opt = 7;


                        //if (SysOptionUtility.SearchAccountMatch == GEnum.SearchMatchOption.StartofField)
                        //    matchOption = 0;
                        //else
                        //    matchOption = 1;

                        break;
                }
                #endregion

                #region get data
                List<SqlParameter> parmList = new List<SqlParameter>();
                parmList.Add(new SqlParameter("@Option", opt));
                parmList.Add(new SqlParameter("@DocCode", docCode));
                parmList.Add(new SqlParameter("@MatchOption ", matchOption));
                parmList.Add(new SqlParameter("@SearchType ", searchType));
                parmList.Add(new SqlParameter("@SearchKey", searchKey));
                parmList.Add(new SqlParameter("@SearchStr", searchStr));
                parmList.Add(new SqlParameter("@ConCust", conCust));
                parmList.Add(new SqlParameter("@ConVend", conVend));
                parmList.Add(new SqlParameter("@ConBoth", conBoth));
                parmList.Add(new SqlParameter("@Prospect", prospect));
                parmList.Add(new SqlParameter("@CCBCredit", ccbCredit));
                parmList.Add(new SqlParameter("@CCBCash", ccbCash));
                parmList.Add(new SqlParameter("@CCBBoth", ccbBoth));
                parmList.Add(new SqlParameter("@ConKey", conKey));
                parmList.Add(new SqlParameter("@UserKey", AppInfor.CurrentUserKey));
                parmList.Add(new SqlParameter("@UserAccessLevel", accessLevel));
                parmList.Add(new SqlParameter("@RetValue", 0));
                parmList[16].Direction = ParameterDirection.Output;

                dt = GFunc.ExecuteProc("SECRecAccess_Get", parmList);
                if (dt.Rows.Count <= 0)
                {
                    if (shwMsg)
                        MsgBox.Show("Can't find this record");
                }
                #endregion

                return dt;
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

        public static int Record_GetKey(GEnum.RecAccessTypeCust AccessTypeCust, string searchValue, int ConKey, ref string ID, ref string Des, GEnum.ConTypeCust conTypeCust, GEnum.CCBType ccbType, bool shwMsg)
        {
            try
            {
                DataTable dt;

                dt = Record_GetKey(0, (GEnum.RecAccessType)AccessTypeCust, searchValue, ConKey, conTypeCust, GEnum.ConTypeVend.ALL, ccbType, shwMsg);
                if (GFunc.IsNE(dt))
                    return 0;
                else
                {
                    if (dt.Rows.Count == 1)
                    {

                        ID = (string)dt.Rows[0]["ID"];
                        Des = (string)dt.Rows[0]["Des"];
                        return (int)dt.Rows[0]["Key"];


                    }
                    else if (dt.Rows.Count > 1)
                    {
                        return 0;//show search Popup to choose one of Records
                    }
                    else
                        return 0;//no result ,show search Popup

                }

                return 0;
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
        public static int Record_GetKey(GEnum.RecAccessTypeVend AccessTypeVend, string searchValue, int ConKey, ref string ID, ref string Des, GEnum.ConTypeVend conTypeVend, bool shwMsg)
        {
            try
            {
                DataTable dt;

                dt = Record_GetKey(0, (GEnum.RecAccessType)AccessTypeVend, searchValue, ConKey, GEnum.ConTypeCust.ALL, GEnum.ConTypeVend.ALL, GEnum.CCBType.ALL, shwMsg);
                if (GFunc.IsNE(dt))
                    return 0;
                else
                {
                    if (dt.Rows.Count >= 1)
                    {

                        ID = (string)dt.Rows[0]["ID"];
                        Des = (string)dt.Rows[0]["Des"];
                        return (int)dt.Rows[0]["Key"];

                    }
                    //else if (dt.Rows.Count > 1) //ID will not be duplicated and we want to retrieve the exact match
                    //{
                    //    return 0;//show search Popup to choose one of Records
                    //}
                    else
                        return 0;//no result ,show search Popup
                }

                return 0;
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
        public static int Record_GetKey(int docCode, GEnum.RecAccessTypeItem AccessTypeItem, string searchValue, ref string ID, ref string Des, bool shwMsg)
        {
            try
            {
                DataTable dt;

                dt = Record_GetKey(docCode, (GEnum.RecAccessType)AccessTypeItem, searchValue, 0, GEnum.ConTypeCust.ALL, GEnum.ConTypeVend.ALL, GEnum.CCBType.ALL, shwMsg);
                if (GFunc.IsNE(dt))
                    return 0;
                else
                {
                    if (dt.Rows.Count == 1)
                    {

                        ID = (string)dt.Rows[0]["ID"];
                        Des = (string)dt.Rows[0]["Des"];
                        return (int)dt.Rows[0]["Key"];

                    }
                    else if (dt.Rows.Count > 1)
                    {
                        ID = (string)dt.Rows[0]["ID"];
                        Des = (string)dt.Rows[0]["Des"];
                        return (int)dt.Rows[0]["Key"];
                    }
                    else
                        return 0;//no result ,show search Popup

                }

                return 0;
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
        public static int Record_GetKey(GEnum.RecAccessTypeJob AccessTypeJob, string searchValue, int ConKey, ref string ID, ref string Des, bool shwMsg)
        {
            try
            {
                DataTable dt;

                dt = Record_GetKey(0, (GEnum.RecAccessType)AccessTypeJob, searchValue, ConKey, GEnum.ConTypeCust.ALL, GEnum.ConTypeVend.ALL, GEnum.CCBType.ALL, shwMsg);
                if (GFunc.IsNE(dt))
                    return 0;
                else
                {
                    if (dt.Rows.Count == 1)
                    {
                        ID = (string)dt.Rows[0]["ID"];
                        Des = (string)dt.Rows[0]["Des"];
                        return (int)dt.Rows[0]["Key"];

                    }
                    else if (dt.Rows.Count > 1)
                    {
                        return 0;//show search Popup to choose one of Records
                    }
                    else
                        return 0;//no result ,show search Popup

                }

                return 0;
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
        public static int Record_GetKey(GEnum.RecAccessTypeAcc AccessTypeAcc, string searchValue, ref string ID, ref string Des, bool shwMsg)
        {
            try
            {
                DataTable dt;

                dt = Record_GetKey(0, (GEnum.RecAccessType)AccessTypeAcc, searchValue, 0, GEnum.ConTypeCust.ALL, GEnum.ConTypeVend.ALL, GEnum.CCBType.ALL, shwMsg);
                if (GFunc.IsNE(dt))
                    return 0;
                else
                {
                    if (dt.Rows.Count == 1)
                    {
                        ID = (string)dt.Rows[0]["ID"];
                        Des = (string)dt.Rows[0]["Des"];
                        return (int)dt.Rows[0]["Key"];

                    }
                    else if (dt.Rows.Count > 1)
                    {
                        //return 0;//show search Popup to choose one of Records
                        ID = (string)dt.Rows[0]["ID"];
                        Des = (string)dt.Rows[0]["Des"];
                        return (int)dt.Rows[0]["Key"];

                    }
                    else
                        return 0;//no result ,show search Popup
                }

                return 0;
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

        //for Doc Code
        public static int Record_GetKey(int docCode, GEnum.RecAccessType AccessType, string searchValue, int dependentKey, ref string ID, ref string Des, bool shwMsg)
        {
            //Note: dependentKey is use as a criteria to filter the result in the SP. 
            //For Job: Conkey is the dependentKey. For Sales Document a value of 0 will result in no records return, a value >0 will be use as the conKey
            //for purchase document, this dependentKey is not used.
            //in future we may use this dependentKey for Account
            try
            {

                switch (AccessType)
                {
                    case GEnum.RecAccessType.CustID:
                    case GEnum.RecAccessType.CustKey:
                    case GEnum.RecAccessType.CustNm:
                        #region Customer
                        switch (docCode)
                        {
                            case (int)GEnum.SystemCode.Quotation:
                                return Record_GetKey((GEnum.RecAccessTypeCust)AccessType, searchValue, dependentKey, ref ID, ref Des, GEnum.ConTypeCust.ALL, GEnum.CCBType.ALL, shwMsg);

                            case (int)GEnum.SystemCode.Sales_Order:
                            case (int)GEnum.SystemCode.Sales_Order_Adjustment:
                            case (int)GEnum.SystemCode.Delivery_Order:
                                return Record_GetKey((GEnum.RecAccessTypeCust)AccessType, searchValue, dependentKey, ref ID, ref Des, GEnum.ConTypeCust.CB, GEnum.CCBType.CR, shwMsg);

                            case (int)GEnum.SystemCode.Packing_List:
                                return Record_GetKey((GEnum.RecAccessTypeCust)AccessType, searchValue, dependentKey, ref ID, ref Des, GEnum.ConTypeCust.CB, GEnum.CCBType.ALL, shwMsg);

                            case (int)GEnum.SystemCode.Sales_Invoice:
                            case (int)GEnum.SystemCode.Sales_Debit_Note:
                            case (int)GEnum.SystemCode.Sales_Credit_Note:
                            case (int)GEnum.SystemCode.Sales_Adjustment:
                            case (int)GEnum.SystemCode.Payment_Received:
                                return Record_GetKey((GEnum.RecAccessTypeCust)AccessType, searchValue, dependentKey, ref ID, ref Des, GEnum.ConTypeCust.CB, GEnum.CCBType.CR, shwMsg);

                            case (int)GEnum.SystemCode.Contra:
                                return Record_GetKey((GEnum.RecAccessTypeCust)AccessType, searchValue, dependentKey, ref ID, ref Des, GEnum.ConTypeCust.B, GEnum.CCBType.CR, shwMsg);

                            case (int)GEnum.SystemCode.Cash_Sale:
                            case (int)GEnum.SystemCode.Cash_Debit_Note:
                            case (int)GEnum.SystemCode.Cash_Credit_Note:
                            case (int)GEnum.SystemCode.Cash_Adjustment:
                            case (int)GEnum.SystemCode.Cash_Payment_Received:
                            case (int)GEnum.SystemCode.Cash_Contra:
                                return Record_GetKey((GEnum.RecAccessTypeCust)AccessType, searchValue, dependentKey, ref ID, ref Des, GEnum.ConTypeCust.CB, GEnum.CCBType.CH, shwMsg);

                            case (int)GEnum.SystemCode.Purchase_Plan:
                                return Record_GetKey((GEnum.RecAccessTypeCust)AccessType, searchValue, dependentKey, ref ID, ref Des, GEnum.ConTypeCust.ALL, GEnum.CCBType.CH, shwMsg);

                            case (int)GEnum.SystemCode.Purchase_Order:
                            case (int)GEnum.SystemCode.Purchase_Order_Adjustment:
                            case (int)GEnum.SystemCode.Purchase_Delivery:
                            case (int)GEnum.SystemCode.Purchase_Invoice:
                            case (int)GEnum.SystemCode.Purchase_Debit_Note:
                            case (int)GEnum.SystemCode.Purchase_Credit_Note:
                            case (int)GEnum.SystemCode.Purchase_Adjustment:
                            case (int)GEnum.SystemCode.Payment_Issue:
                                return Record_GetKey((GEnum.RecAccessTypeCust)AccessType, searchValue, dependentKey, ref ID, ref Des, GEnum.ConTypeCust.ALL, GEnum.CCBType.ALL, shwMsg);

                            case (int)GEnum.SystemCode.Issue_Consignment:
                            case (int)GEnum.SystemCode.Return_Consignment:
                                return Record_GetKey((GEnum.RecAccessTypeCust)AccessType, searchValue, dependentKey, ref ID, ref Des, GEnum.ConTypeCust.CB, GEnum.CCBType.CR, shwMsg);

                            case (int)GEnum.SystemCode.Order_Consignment:
                            case (int)GEnum.SystemCode.Consignment_Order_Adjustment:
                            case (int)GEnum.SystemCode.Received_Consignment:
                            case (int)GEnum.SystemCode.Consignment_Settlement:
                                return Record_GetKey((GEnum.RecAccessTypeCust)AccessType, searchValue, dependentKey, ref ID, ref Des, GEnum.ConTypeCust.ALL, GEnum.CCBType.ALL, shwMsg);
                                break;
                        }
                        break;

                        #endregion

                    case GEnum.RecAccessType.VendID:
                    case GEnum.RecAccessType.VendKey:
                    case GEnum.RecAccessType.VendNm:
                        #region Vendor
                        switch (docCode)
                        {
                            case (int)GEnum.SystemCode.Quotation:
                            case (int)GEnum.SystemCode.Sales_Order:
                            case (int)GEnum.SystemCode.Sales_Order_Adjustment:
                            case (int)GEnum.SystemCode.Delivery_Order:
                            case (int)GEnum.SystemCode.Packing_List:
                            case (int)GEnum.SystemCode.Sales_Invoice:
                            case (int)GEnum.SystemCode.Sales_Debit_Note:
                            case (int)GEnum.SystemCode.Sales_Credit_Note:
                            case (int)GEnum.SystemCode.Sales_Adjustment:
                            case (int)GEnum.SystemCode.Payment_Received:
                            case (int)GEnum.SystemCode.Contra:
                            case (int)GEnum.SystemCode.Cash_Sale:
                            case (int)GEnum.SystemCode.Cash_Debit_Note:
                            case (int)GEnum.SystemCode.Cash_Credit_Note:
                            case (int)GEnum.SystemCode.Cash_Adjustment:
                            case (int)GEnum.SystemCode.Cash_Payment_Received:
                            case (int)GEnum.SystemCode.Cash_Contra:
                                return Record_GetKey((GEnum.RecAccessTypeVend)AccessType, searchValue, dependentKey, ref ID, ref Des, GEnum.ConTypeVend.ALL, shwMsg);

                            case (int)GEnum.SystemCode.Purchase_Plan:
                            case (int)GEnum.SystemCode.Purchase_Order:
                            case (int)GEnum.SystemCode.Purchase_Order_Adjustment:
                            case (int)GEnum.SystemCode.Purchase_Delivery:
                            case (int)GEnum.SystemCode.Purchase_Invoice:
                            case (int)GEnum.SystemCode.Purchase_Debit_Note:
                            case (int)GEnum.SystemCode.Purchase_Credit_Note:
                            case (int)GEnum.SystemCode.Purchase_Adjustment:
                            case (int)GEnum.SystemCode.Payment_Issue:
                                return Record_GetKey((GEnum.RecAccessTypeVend)AccessType, searchValue, dependentKey, ref ID, ref Des, GEnum.ConTypeVend.B, shwMsg);

                            case (int)GEnum.SystemCode.Issue_Consignment:
                            case (int)GEnum.SystemCode.Return_Consignment:
                                return Record_GetKey((GEnum.RecAccessTypeVend)AccessType, searchValue, dependentKey, ref ID, ref Des, GEnum.ConTypeVend.ALL, shwMsg);

                            case (int)GEnum.SystemCode.Order_Consignment:
                            case (int)GEnum.SystemCode.Consignment_Order_Adjustment:
                            case (int)GEnum.SystemCode.Received_Consignment:
                            case (int)GEnum.SystemCode.Consignment_Settlement:
                                return Record_GetKey((GEnum.RecAccessTypeVend)AccessType, searchValue, dependentKey, ref ID, ref Des, GEnum.ConTypeVend.B, shwMsg);

                        }
                        break;

                        #endregion

                    case GEnum.RecAccessType.JobKey:
                    case GEnum.RecAccessType.JobID:
                    case GEnum.RecAccessType.JobDes:
                        #region Job
                        switch (docCode)
                        {
                            case (int)GEnum.SystemCode.Quotation:
                            case (int)GEnum.SystemCode.Sales_Order:
                            case (int)GEnum.SystemCode.Delivery_Order:
                            case (int)GEnum.SystemCode.Sales_Invoice:
                            case (int)GEnum.SystemCode.Sales_Debit_Note:
                            case (int)GEnum.SystemCode.Sales_Credit_Note:
                            case (int)GEnum.SystemCode.Sales_Adjustment:
                            case (int)GEnum.SystemCode.Payment_Received:
                            case (int)GEnum.SystemCode.Cash_Sale:
                            case (int)GEnum.SystemCode.Cash_Debit_Note:
                            case (int)GEnum.SystemCode.Cash_Credit_Note:
                            case (int)GEnum.SystemCode.Cash_Adjustment:
                            case (int)GEnum.SystemCode.Cash_Payment_Received:
                            case (int)GEnum.SystemCode.Issue_Consignment:
                            case (int)GEnum.SystemCode.Return_Consignment:
                                if (dependentKey > 0)
                                    return Record_GetKey((GEnum.RecAccessTypeJob)AccessType, searchValue, dependentKey, ref ID, ref Des, shwMsg);
                                else
                                    return 0;   //for sales document , the dependentKey is use as the ConKey, Conkey cannot be <=0
                                break;

                            case (int)GEnum.SystemCode.Purchase_Order:
                            case (int)GEnum.SystemCode.Purchase_Delivery:
                            case (int)GEnum.SystemCode.Purchase_Invoice:
                            case (int)GEnum.SystemCode.Purchase_Debit_Note:
                            case (int)GEnum.SystemCode.Purchase_Credit_Note:
                            case (int)GEnum.SystemCode.Purchase_Adjustment:
                            case (int)GEnum.SystemCode.Payment_Issue:
                            case (int)GEnum.SystemCode.Inventory_Adjustment:
                            case (int)GEnum.SystemCode.Journal:
                                return Record_GetKey((GEnum.RecAccessTypeJob)AccessType, searchValue, dependentKey, ref ID, ref Des, shwMsg);

                        }
                        break;
                        #endregion

                    case GEnum.RecAccessType.ItemKey:
                    case GEnum.RecAccessType.ItemID:
                    case GEnum.RecAccessType.ItemDes:
                        #region Item
                        switch (docCode)
                        {
                            case (int)GEnum.SystemCode.Quotation:
                            case (int)GEnum.SystemCode.Sales_Order:
                            case (int)GEnum.SystemCode.Sales_Order_Adjustment:
                            case (int)GEnum.SystemCode.Delivery_Order:
                            case (int)GEnum.SystemCode.Sales_Invoice:
                            case (int)GEnum.SystemCode.Sales_Debit_Note:
                            case (int)GEnum.SystemCode.Sales_Credit_Note:
                            case (int)GEnum.SystemCode.Cash_Sale:
                            case (int)GEnum.SystemCode.Cash_Debit_Note:
                            case (int)GEnum.SystemCode.Cash_Credit_Note:
                            case (int)GEnum.SystemCode.Purchase_Plan:
                            case (int)GEnum.SystemCode.Purchase_Request:
                            case (int)GEnum.SystemCode.Purchase_Order:
                            case (int)GEnum.SystemCode.Purchase_Order_Adjustment:
                            case (int)GEnum.SystemCode.Purchase_Delivery:
                            case (int)GEnum.SystemCode.Purchase_Invoice:
                            case (int)GEnum.SystemCode.Purchase_Debit_Note:
                            case (int)GEnum.SystemCode.Purchase_Credit_Note:
                            case (int)GEnum.SystemCode.Inventory_Adjustment:
                            case (int)GEnum.SystemCode.Inventory_Production:
                            case (int)GEnum.SystemCode.Inventory_Transfer:
                            case (int)GEnum.SystemCode.Issue_Consignment:
                            case (int)GEnum.SystemCode.Return_Consignment:
                            case (int)GEnum.SystemCode.Order_Consignment:
                            case (int)GEnum.SystemCode.Consignment_Order_Adjustment:
                            case (int)GEnum.SystemCode.Received_Consignment:
                            case (int)GEnum.SystemCode.Consignment_Settlement:
                                return Record_GetKey(docCode, (GEnum.RecAccessTypeItem)AccessType, searchValue, ref ID, ref Des, shwMsg);
                                break;
                        }
                        break;
                        #endregion

                    case GEnum.RecAccessType.AccKey:
                    case GEnum.RecAccessType.AccID:
                    case GEnum.RecAccessType.AccDes:

                        return Record_GetKey((GEnum.RecAccessTypeAcc)AccessType, searchValue, ref ID, ref Des, shwMsg);

                }




                return 0;


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

        //---------New Record Get Key functions----------//
        public static DataTable Record_GetKeyNew(GEnum.RecAccessType AccessType, string ListSettingID, int vendorKey,
            int catKey1, int catKey2, int catKey3, int catKey4, int catKey5, int brandKey, int INClass,
            int accTypeKey, int jobConKey, int jobGrpKey, string searchValue, bool shwMsg, out int MatchFound)
        {
            int opt = 0;
            int accessLevel = 0;
            MatchFound = 0;


            #region Set option value
            switch (AccessType)
            {
                case GEnum.RecAccessType.CustKey:
                case GEnum.RecAccessType.CustID:
                case GEnum.RecAccessType.CustNm:
                    opt = 0;
                    accessLevel = AppInfor.ConAccessLevel;

                    break;

                case GEnum.RecAccessType.VendKey:
                case GEnum.RecAccessType.VendID:
                case GEnum.RecAccessType.VendNm:
                    opt = 0;
                    accessLevel = AppInfor.ConAccessLevel;

                    break;

                case GEnum.RecAccessType.ItemKey:
                case GEnum.RecAccessType.ItemKeySub:
                case GEnum.RecAccessType.ItemID:
                    opt = 1;
                    accessLevel = AppInfor.itemAccessLevel;


                    break;

                case GEnum.RecAccessType.ItemIDSub:
                case GEnum.RecAccessType.ItemDes:
                case GEnum.RecAccessType.ItemDesSub:
                    opt = 1;
                    accessLevel = AppInfor.itemAccessLevel;

                    break;

                case GEnum.RecAccessType.JobKey:
                case GEnum.RecAccessType.JobID:
                case GEnum.RecAccessType.JobDes:
                    accessLevel = AppInfor.jobAccessLevel;
                    opt = 3;


                    break;

                case GEnum.RecAccessType.AccKey:
                case GEnum.RecAccessType.AccID:
                case GEnum.RecAccessType.AccDes:
                    opt = 2;

                    break;
            }
            #endregion


            List<SqlParameter> parmList = new List<SqlParameter>();
            parmList.Add(new SqlParameter("@Option", opt));
            parmList.Add(new SqlParameter("@ListSettingID", ListSettingID));
            parmList.Add(new SqlParameter("@VendorKey", vendorKey));
            parmList.Add(new SqlParameter("@CatKey1", catKey1));
            parmList.Add(new SqlParameter("@CatKey2", catKey2));
            parmList.Add(new SqlParameter("@CatKey3", catKey3));
            parmList.Add(new SqlParameter("@CatKey4", catKey4));
            parmList.Add(new SqlParameter("@CatKey5", catKey5));
            parmList.Add(new SqlParameter("@Brandkey", brandKey));
            parmList.Add(new SqlParameter("@INClass", INClass));
            parmList.Add(new SqlParameter("@AccTypeKey", accTypeKey));
            parmList.Add(new SqlParameter("@JobConKey", jobConKey));
            parmList.Add(new SqlParameter("@JobGrpKey", jobGrpKey));
            parmList.Add(new SqlParameter("@SearchValue", searchValue));
            parmList.Add(new SqlParameter("@UserKey", AppInfor.CurrentUserKey));
            parmList.Add(new SqlParameter("@UserAccessLevel", accessLevel));
            parmList.Add(new SqlParameter("@CallFromRecordSearch", false));
            parmList.Add(new SqlParameter("@MatchFound", 0));
            parmList.Add(new SqlParameter("@RetValue", 0));
            parmList[17].Direction = ParameterDirection.Output;
            parmList[18].Direction = ParameterDirection.Output;


            DataTable dt = GFunc.ExecuteProc("SECRecAccess_GetData", parmList);
            MatchFound = GFunc.NEInt(parmList[17].Value, 0);


            if (dt.Rows.Count <= 0)
            {
                if (MatchFound == 0)
                {
                    if (shwMsg)
                        MsgBox.Show("Can't find this record");
                }
                else
                {
                    if (shwMsg)
                        MsgBox.Show("No permission for this record");
                }
            }




            return dt;



        }

        public static int ConRecord_GetKey(GEnum.RecAccessType AccessType, string ListSettingID, string searchValue, ref string ID, ref string Des, bool shwMsg, int DocCodeKey = 0)
        {
            try
            {
                DataTable dt;
                int MatchFound = 0;

                dt = Record_GetKeyNew(AccessType, ListSettingID, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, searchValue, shwMsg, out MatchFound);
                if (dt == null)
                    return 0;
                else
                {
                    if (MatchFound == 1 && dt.Rows.Count > 0)
                    {
                        ////ttm
                        //if (DocCodeKey != 0 && (int)GEnum.SystemCode.Quotation == DocCodeKey)
                        //{
                        //    ID = (string)dt.Rows[0]["ID"];
                        //    Des = (string)dt.Rows[0]["Des"];
                        //    return (int)dt.Rows[0]["Key"];
                        //}
                        //else
                        //{
                            if ((Boolean)dt.Rows[0]["Inactive"] == false)
                            {
                                //if ((Boolean)dt.Rows[0]["ActiveWithProblem"] == false || (DocCodeKey != 0 && (int)GEnum.SystemCode.Quotation == DocCodeKey)) /* commented by yst on 20190613 to check customer in QO like SO suggested by Jasmin */
                                if ((Boolean)dt.Rows[0]["ActiveWithProblem"] == false)
                                {
                                        ID = (string)dt.Rows[0]["ID"];
                                        Des = (string)dt.Rows[0]["Des"];
                                        return (int)dt.Rows[0]["Key"];
                                }
                                else
                                    return 0;
                            }                             
                            else
                            {
                                //check if current active form is master form? 
                                //if master form, return value, else return 0 
                                Form f = GetActiveControlAtActiveForm().FindForm();
                                if (f.Name.ToLower() == "frmmstcon")
                                {
                                    ID = (string)dt.Rows[0]["ID"];
                                    Des = (string)dt.Rows[0]["Des"];
                                    return (int)dt.Rows[0]["Key"];
                                }
                                else
                                    return 0;
                            }
                        //}
                        ////ttm

                    }
                    else
                    {
                        return 0;
                    }

                }

                return 0;
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
        public static int AccRecord_GetKey(GEnum.RecAccessType AccessType, string ListSettingID, string searchValue, ref string ID, ref string Des, bool shwMsg)
        {
            try
            {
                DataTable dt;
                int MatchFound = 0;

                dt = Record_GetKeyNew(AccessType, ListSettingID, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, searchValue, shwMsg, out MatchFound);
                if (GFunc.IsNE(dt))
                    return 0;
                else
                {

                    if (MatchFound == 1 && dt.Rows.Count > 0)
                    {
                        if ((Boolean)dt.Rows[0]["Inactive"] == false)
                        {
                            ID = (string)dt.Rows[0]["ID"];
                            Des = (string)dt.Rows[0]["Des"];
                            return (int)dt.Rows[0]["Key"];
                        }
                        else
                        {
                            //check if current active form is master form? 
                            //if master form, return value, else return 0 
                            Form f = GetActiveControlAtActiveForm().FindForm();
                            if (f.Name.ToLower() == "frmmstacc")
                            {
                                ID = (string)dt.Rows[0]["ID"];
                                Des = (string)dt.Rows[0]["Des"];
                                return (int)dt.Rows[0]["Key"];
                            }
                            else
                                return 0;
                        }
                        
                    }
                    else
                    {
                        return 0;
                    }

                }

                return 0;
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
        public static int ItmRecord_GetKey(GEnum.RecAccessType AccessType, string ListSettingID, string searchValue, int vendorKey, ref string ID, ref string Des, bool shwMsg)
        {
            try
            {
                DataTable dt;
                int MatchFound = 0;

                dt = Record_GetKeyNew(AccessType, ListSettingID, vendorKey, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, searchValue, shwMsg, out MatchFound);                
                if (GFunc.IsNE(dt))
                    return 0;
                else
                {

                    if (MatchFound == 1 && dt.Rows.Count > 0)
                    {
                        if ((Boolean)dt.Rows[0]["Inactive"] == false)
                        {
                            ID = (string)dt.Rows[0]["ID"];
                            Des = (string)dt.Rows[0]["Des"];
                            return (int)dt.Rows[0]["Key"];
                        }
                        else
                        {                            
                            //check if current active form is master form? 
                            //if master form, return value, else return 0 
                            Form f = GetActiveControlAtActiveForm().FindForm();
                            if (f.Name.ToLower() == "frmmstitm")
                            {
                                ID = (string)dt.Rows[0]["ID"];
                                Des = (string)dt.Rows[0]["Des"];
                                return (int)dt.Rows[0]["Key"];
                            }
                            else                            
                                return 0;
                        }
                            
                    }
                    else
                    {
                        return 0;
                    }

                }

                return 0;
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
        public static Control GetActiveControlAtActiveForm()
        {
            try
            {
                //We Can't Use frm.ActiveControl, The frm.ActiveControl is wrong for the control is place inside at 'splitContainer/groupbox/Panel'
                Control FocusControl = null;
                IntPtr handle = GetFocus();
                if (handle != IntPtr.Zero)
                {
                    FocusControl = Control.FromHandle(handle);
                }

                return FocusControl;
            }
            catch (Exception ex)
            {
                throw (ex);
            }
        }
  
        public static int JobRecord_GetKey(GEnum.RecAccessType AccessType, string ListSettingID, string searchValue, int jobConKey, ref string ID, ref string Des, bool shwMsg)
        {
            try
            {
                DataTable dt;
                int MatchFound = 0;

                dt = Record_GetKeyNew(AccessType, ListSettingID, 0, 0, 0, 0, 0, 0, 0, 0, 0, jobConKey, 0, searchValue, shwMsg, out MatchFound);
                if (GFunc.IsNE(dt))
                    return 0;
                else
                {

                    if (MatchFound == 1 && dt.Rows.Count > 0)
                    {
                        ID = (string)dt.Rows[0]["ID"];
                        Des = (string)dt.Rows[0]["Des"];
                        return (int)dt.Rows[0]["Key"];
                    }
                    else
                    {
                        return 0;
                    }

                }

                return 0;
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

        //--------------------------------------------//

        public static DateTime PeriodToDate(int Period)
        {
            try
            {
                string vPeriodStr = Period.ToString();
                DateTime vReval = Convert.ToDateTime("1/1/1900");
                if (vPeriodStr.Length > 0)
                {
                    int vYear = GFunc.NEInt(vPeriodStr.Substring(0, 4), 9999);
                    int vMonth = GFunc.NEInt(vPeriodStr.Substring(4, 2), 01);
                    vReval = new DateTime(vYear, vMonth, 01);
                }

                return vReval;
            }
            catch (TAException tex)
            {
                throw Error(tex);
            }
            catch (Exception ex)
            {
                throw Error(ex);
            }
            return DateTime.Today;
        }

        public static bool DocPeriod_Get(DateTime? DocDate, ref int? DocPeriod)
        {
            try
            {
                using (SqlConnection cn = new SqlConnection(Database.BossDemoConnection))
                {
                    cn.Open();
                    return DocPeriod_Get(cn, DocDate, ref DocPeriod);
                }
            }
            catch (Exception ex)
            {
                throw Error(ex);
            }
        }
        public static bool DocPeriod_Get(SqlConnection cn, DateTime? DocDate, ref int? DocPeriod)
        {
            try
            {
                List<SqlParameter> parmList = new List<SqlParameter>();

                parmList.Add(new SqlParameter("@Option", 2));
                parmList.Add(new SqlParameter("@PeriodStatus", 0));//0=>Open,10=>Lock,99=>Closed
                parmList.Add(new SqlParameter("@DocDate", DocDate.Value.Date));
                SqlParameter p = new SqlParameter("@RetValue", 0);
                p.Direction = ParameterDirection.Output;
                parmList.Add(p);

                DataTable dt = GFunc.ExecuteProc(cn, "ROSysPeriod_Get", parmList);

                if (dt == null)
                    return false;
                else if (dt.Rows.Count == 0)
                    return false;
                else if (GFunc.IsNE(dt.Rows[0]["Period"]))
                    return false;
                else
                    DocPeriod = (int?)dt.Rows[0]["Period"];

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
        public static bool CanAccessDocument(int DocCodeKey, int? DocKey, string DocID)
        {
            try
            {
                string Sql = string.Empty;
                bool SearchByID = false;
                DataTable dt;
                int docConKey = 0;

                if (DocID != String.Empty)
                    SearchByID = true;

                #region Prepare SQL
                switch (DocCodeKey)
                {
                    case (int)GEnum.SystemCode.Quotation:
                        if (SearchByID)
                            Sql = "SELECT DocConKey FROM AR_QO(nolock) WHERE DocID='" + DocID + "' AND DocCodeKey=" + DocCodeKey;
                        else
                            Sql = "SELECT DocConKey FROM AR_QO(nolock) WHERE DocKey=" + DocKey + "AND DocCodeKey=" + DocCodeKey;
                        break;

                    case (int)GEnum.SystemCode.Sales_Order:
                        if (SearchByID)
                            Sql = "SELECT DocConKey FROM AR_SO(nolock) WHERE DocID='" + DocID + "' AND DocCodeKey=" + DocCodeKey;
                        else
                            Sql = "SELECT DocConKey FROM AR_SO(nolock) WHERE DocKey=" + DocKey + "AND DocCodeKey=" + DocCodeKey;
                        break;

                    case (int)GEnum.SystemCode.Sales_Order_Adjustment:
                    case (int)GEnum.SystemCode.Purchase_Order_Adjustment:
                    case (int)GEnum.SystemCode.Consignment_Order_Adjustment:
                        if (SearchByID)
                            Sql = "SELECT DocConKey FROM AP_PJ(nolock) WHERE DocID='" + DocID + "' AND DocCodeKey=" + DocCodeKey;
                        else
                            Sql = "SELECT DocConKey FROM AP_PJ(nolock) WHERE DocKey=" + DocKey + "AND DocCodeKey=" + DocCodeKey;
                        break;

                    case (int)GEnum.SystemCode.Delivery_Order:
                        if (SearchByID)
                            Sql = "SELECT DocConKey FROM AR_DO(nolock) WHERE DocID='" + DocID + "' AND DocCodeKey=" + DocCodeKey;
                        else
                            Sql = "SELECT DocConKey FROM AR_DO(nolock) WHERE DocKey=" + DocKey + "AND DocCodeKey=" + DocCodeKey;
                        break;

                    case (int)GEnum.SystemCode.Packing_List:
                        if (SearchByID)
                            Sql = "SELECT DocConKey FROM AR_PL(nolock) WHERE DocID='" + DocID + "' AND DocCodeKey=" + DocCodeKey;
                        else
                            Sql = "SELECT DocConKey FROM AR_PL(nolock) WHERE DocKey=" + DocKey + "AND DocCodeKey=" + DocCodeKey;
                        break;

                    case (int)GEnum.SystemCode.Sales_Invoice:
                    case (int)GEnum.SystemCode.Sales_Debit_Note:
                    case (int)GEnum.SystemCode.Sales_Credit_Note:
                    case (int)GEnum.SystemCode.Cash_Sale:
                    case (int)GEnum.SystemCode.Cash_Debit_Note:
                    case (int)GEnum.SystemCode.Cash_Credit_Note:
                        if (SearchByID)
                            Sql = "SELECT DocConKey FROM AR_IV(nolock) WHERE DocID='" + DocID + "' AND DocCodeKey=" + DocCodeKey;
                        else
                            Sql = "SELECT DocConKey FROM AR_IV(nolock) WHERE DocKey=" + DocKey + "AND DocCodeKey=" + DocCodeKey;
                        break;

                    case (int)GEnum.SystemCode.Sales_Adjustment:
                    case (int)GEnum.SystemCode.Cash_Adjustment:
                        if (SearchByID)
                            Sql = "SELECT DocConKey FROM AR_ADJ(nolock) WHERE DocID='" + DocID + "' AND DocCodeKey=" + DocCodeKey;
                        else
                            Sql = "SELECT DocConKey FROM AR_ADJ(nolock) WHERE DocKey=" + DocKey + "AND DocCodeKey=" + DocCodeKey;
                        break;

                    case (int)GEnum.SystemCode.Payment_Received:
                    case (int)GEnum.SystemCode.Cash_Payment_Received:
                        if (SearchByID)
                            Sql = "SELECT DocConKey FROM AR_PY(nolock) WHERE DocID='" + DocID + "' AND DocCodeKey=" + DocCodeKey;
                        else
                            Sql = "SELECT DocConKey FROM AR_PY(nolock) WHERE DocKey=" + DocKey + "AND DocCodeKey=" + DocCodeKey;
                        break;

                    case (int)GEnum.SystemCode.Contra:
                    case (int)GEnum.SystemCode.Cash_Contra:
                        if (SearchByID)
                            Sql = "SELECT DocConKey FROM AR_CT(nolock) WHERE DocID='" + DocID + "' AND DocCodeKey=" + DocCodeKey;
                        else
                            Sql = "SELECT DocConKey FROM AR_CT(nolock) WHERE DocKey=" + DocKey + "AND DocCodeKey=" + DocCodeKey;
                        break;

                    case (int)GEnum.SystemCode.Purchase_Plan:
                        if (SearchByID)
                            Sql = "SELECT DocConKey FROM AP_PN(nolock) WHERE DocID='" + DocID + "' AND DocCodeKey=" + DocCodeKey;
                        else
                            Sql = "SELECT DocConKey FROM AP_PN(nolock) WHERE DocKey=" + DocKey + "AND DocCodeKey=" + DocCodeKey;
                        break;

                    case (int)GEnum.SystemCode.Purchase_Order:
                        if (SearchByID)
                            Sql = "SELECT DocConKey FROM AP_PO(nolock) WHERE DocID='" + DocID + "' AND DocCodeKey=" + DocCodeKey;
                        else
                            Sql = "SELECT DocConKey FROM AP_PO(nolock) WHERE DocKey=" + DocKey + "AND DocCodeKey=" + DocCodeKey;
                        break;

                    case (int)GEnum.SystemCode.Purchase_Delivery:
                        if (SearchByID)
                            Sql = "SELECT DocConKey FROM AP_PD(nolock) WHERE DocID='" + DocID + "' AND DocCodeKey=" + DocCodeKey;
                        else
                            Sql = "SELECT DocConKey FROM AP_PD(nolock) WHERE DocKey=" + DocKey + "AND DocCodeKey=" + DocCodeKey;
                        break;

                    case (int)GEnum.SystemCode.Purchase_Invoice:
                    case (int)GEnum.SystemCode.Purchase_Debit_Note:
                    case (int)GEnum.SystemCode.Purchase_Credit_Note:
                        if (SearchByID)
                            Sql = "SELECT DocConKey FROM AP_BL(nolock) WHERE DocID='" + DocID + "' AND DocCodeKey=" + DocCodeKey;
                        else
                            Sql = "SELECT DocConKey FROM AP_BL(nolock) WHERE DocKey=" + DocKey + "AND DocCodeKey=" + DocCodeKey;
                        break;

                    case (int)GEnum.SystemCode.Purchase_Adjustment:
                        if (SearchByID)
                            Sql = "SELECT DocConKey FROM AP_ADJ(nolock) WHERE DocID='" + DocID + "' AND DocCodeKey=" + DocCodeKey;
                        else
                            Sql = "SELECT DocConKey FROM AP_ADJ(nolock) WHERE DocKey=" + DocKey + "AND DocCodeKey=" + DocCodeKey;
                        break;

                    case (int)GEnum.SystemCode.Payment_Issue:
                        if (SearchByID)
                            Sql = "SELECT DocConKey FROM AP_PY(nolock) WHERE DocID='" + DocID + "' AND DocCodeKey=" + DocCodeKey;
                        else
                            Sql = "SELECT DocConKey FROM AP_PY(nolock) WHERE DocKey=" + DocKey + "AND DocCodeKey=" + DocCodeKey;
                        break;

                    case (int)GEnum.SystemCode.Issue_Consignment:
                    case (int)GEnum.SystemCode.Return_Consignment:
                        if (SearchByID)
                            Sql = "SELECT DocConKey FROM CS_CSI(nolock) WHERE DocID='" + DocID + "' AND DocCodeKey=" + DocCodeKey;
                        else
                            Sql = "SELECT DocConKey FROM CS_CSI(nolock) WHERE DocKey=" + DocKey + "AND DocCodeKey=" + DocCodeKey;
                        break;

                    case (int)GEnum.SystemCode.Order_Consignment:
                        if (SearchByID)
                            Sql = "SELECT DocConKey FROM CS_CPO(nolock) WHERE DocID='" + DocID + "' AND DocCodeKey=" + DocCodeKey;
                        else
                            Sql = "SELECT DocConKey FROM CS_CPO(nolock) WHERE DocKey=" + DocKey + "AND DocCodeKey=" + DocCodeKey;
                        break;

                    case (int)GEnum.SystemCode.Received_Consignment:
                        if (SearchByID)
                            Sql = "SELECT DocConKey FROM CS_CPD(nolock) WHERE DocID='" + DocID + "' AND DocCodeKey=" + DocCodeKey;
                        else
                            Sql = "SELECT DocConKey FROM CS_CPD(nolock) WHERE DocKey=" + DocKey + "AND DocCodeKey=" + DocCodeKey;
                        break;

                    case (int)GEnum.SystemCode.Consignment_Settlement:
                        if (SearchByID)
                            Sql = "SELECT DocConKey FROM CS_CPS(nolock) WHERE DocID='" + DocID + "' AND DocCodeKey=" + DocCodeKey;
                        else
                            Sql = "SELECT DocConKey FROM CS_CPS(nolock) WHERE DocKey=" + DocKey + "AND DocCodeKey=" + DocCodeKey;
                        break;
                }
                #endregion

                //Check for Access to document for the DocConKey
                if (Sql != string.Empty)
                {
                    dt = GFunc.ExecuteQuery(Sql);
                    if (dt != null)
                    {
                        if (dt.Rows.Count > 0)
                        {
                            Int32.TryParse(dt.Rows[0][0].ToString(), out docConKey);
                            if (CanAccessRecord(docConKey, (int)GEnum.SystemCode.Customer))
                                return true;
                            else
                                throw new TAException("You have no access to this document");
                        }
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
        }
        public static bool CanAccessRecord(int? recKey, int CodeKey)
        {
            try
            {
                using (SqlConnection cn = new SqlConnection(Database.BossDemoConnection))
                {
                    cn.Open();
                    return CanAccessRecord(cn, recKey, CodeKey);
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
        public static bool CanAccessRecord(SqlConnection cn, int? recKey, int CodeKey)
        {
            bool retValue = false;
            int accessLevel = 0;

            //msgID = MsgID.Permission.PermOpenRecIsFalse;
            int option = -1;

            switch (CodeKey)
            {
                case (int)GEnum.SystemCode.Customer:
                case (int)GEnum.SystemCode.Vendor:
                    option = 0;
                    accessLevel = AppInfor.conAccessLevel;
                    break;
                case (int)GEnum.SystemCode.Inventory:
                    option = 1;
                    accessLevel = AppInfor.ItemAccessLevel;
                    break;
                case (int)GEnum.SystemCode.Job:
                    option = 2;
                    accessLevel = AppInfor.JobAccessLevel;
                    break;
            }

            if (option == -1)
                return true;

            // Using existing sql connection.
            using (SqlCommand cm = cn.CreateCommand())
            {
                cm.CommandType = CommandType.StoredProcedure;
                cm.CommandText = "SECRecAccess_Check";

                cm.Parameters.AddWithValue("@RetValue", 0);
                cm.Parameters["@RetValue"].Direction = ParameterDirection.Output;

                cm.Parameters.AddWithValue("@Option", option);
                cm.Parameters.AddWithValue("@Key", recKey);

                cm.Parameters.AddWithValue("@UserAccessLevel", accessLevel);
                cm.Parameters.AddWithValue("@UserKey", AppInfor.CurrentUserKey);

                cm.ExecuteNonQuery();

                if ((int)cm.Parameters["@RetValue"].Value == (int)GEnum.SpState.Pass)
                    retValue = true;
                else
                    retValue = false;
            }

            return retValue;
        }
        public static bool IsDocReconciled(int DocCodeKey, int DocKey, int DocItmKey)
        {
            bool runProcess = false;
            try
            {
                switch (DocCodeKey)
                {
                    case (int)GEnum.SystemCode.Sales_Invoice:
                    case (int)GEnum.SystemCode.Sales_Debit_Note:
                    case (int)GEnum.SystemCode.Sales_Credit_Note:
                    case (int)GEnum.SystemCode.Sales_Adjustment:
                    case (int)GEnum.SystemCode.Payment_Received:
                    case (int)GEnum.SystemCode.Contra:
                    case (int)GEnum.SystemCode.Cash_Sale:
                    case (int)GEnum.SystemCode.Cash_Debit_Note:
                    case (int)GEnum.SystemCode.Cash_Credit_Note:
                    case (int)GEnum.SystemCode.Cash_Adjustment:
                    case (int)GEnum.SystemCode.Cash_Payment_Received:
                    case (int)GEnum.SystemCode.Cash_Contra:
                    case (int)GEnum.SystemCode.Purchase_Delivery:
                    case (int)GEnum.SystemCode.Purchase_Invoice:
                    case (int)GEnum.SystemCode.Purchase_Debit_Note:
                    case (int)GEnum.SystemCode.Purchase_Credit_Note:
                    case (int)GEnum.SystemCode.Purchase_Adjustment:
                    case (int)GEnum.SystemCode.Payment_Issue:
                    case (int)GEnum.SystemCode.Inventory_Adjustment:
                    case (int)GEnum.SystemCode.Inventory_Production:
                    case (int)GEnum.SystemCode.Inventory_Transfer:
                    case (int)GEnum.SystemCode.Issue_Consignment:
                    case (int)GEnum.SystemCode.Return_Consignment:
                    case (int)GEnum.SystemCode.Journal:
                    case (int)GEnum.SystemCode.Deposit:
                    case (int)GEnum.SystemCode.Bank_Revaluation:
                        runProcess = true;
                        break;
                }

                if (runProcess)
                {
                    using (SqlConnection cn = new SqlConnection(Database.BossDemoConnection))
                    {
                        cn.Open();
                        // Using existing sql connection.
                        using (SqlCommand cm = cn.CreateCommand())
                        {
                            cm.CommandType = CommandType.StoredProcedure;
                            cm.CommandText = "GLLog_IsReconciled";

                            cm.Parameters.AddWithValue("@RetValue", 0);
                            cm.Parameters["@RetValue"].Direction = ParameterDirection.Output;

                            cm.Parameters.AddWithValue("@DocCodeKey", DocCodeKey);
                            cm.Parameters.AddWithValue("@DocKey", DocKey);
                            cm.Parameters.AddWithValue("@DocItmKey", DocItmKey);

                            cm.ExecuteNonQuery();

                            if ((int)cm.Parameters["@RetValue"].Value == (int)GEnum.SpState.Pass)
                                return true;
                            else
                                return false;

                        }
                    }
                }
                else
                    return false;
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
        public static bool IsDocReconciled(SqlConnection cn, int DocCodeKey, int DocKey, int DocItmKey)
        {
            bool runProcess = false;
            try
            {
                switch (DocCodeKey)
                {
                    case (int)GEnum.SystemCode.Sales_Invoice:
                    case (int)GEnum.SystemCode.Sales_Debit_Note:
                    case (int)GEnum.SystemCode.Sales_Credit_Note:
                    case (int)GEnum.SystemCode.Sales_Adjustment:
                    case (int)GEnum.SystemCode.Payment_Received:
                    case (int)GEnum.SystemCode.Contra:
                    case (int)GEnum.SystemCode.Cash_Sale:
                    case (int)GEnum.SystemCode.Cash_Debit_Note:
                    case (int)GEnum.SystemCode.Cash_Credit_Note:
                    case (int)GEnum.SystemCode.Cash_Adjustment:
                    case (int)GEnum.SystemCode.Cash_Payment_Received:
                    case (int)GEnum.SystemCode.Cash_Contra:
                    case (int)GEnum.SystemCode.Purchase_Delivery:
                    case (int)GEnum.SystemCode.Purchase_Invoice:
                    case (int)GEnum.SystemCode.Purchase_Debit_Note:
                    case (int)GEnum.SystemCode.Purchase_Credit_Note:
                    case (int)GEnum.SystemCode.Purchase_Adjustment:
                    case (int)GEnum.SystemCode.Payment_Issue:
                    case (int)GEnum.SystemCode.Inventory_Adjustment:
                    case (int)GEnum.SystemCode.Inventory_Production:
                    case (int)GEnum.SystemCode.Inventory_Transfer:
                    case (int)GEnum.SystemCode.Issue_Consignment:
                    case (int)GEnum.SystemCode.Return_Consignment:
                    case (int)GEnum.SystemCode.Journal:
                    case (int)GEnum.SystemCode.Deposit:
                    case (int)GEnum.SystemCode.Bank_Revaluation:
                        runProcess = true;
                        break;
                }

                if (runProcess)
                {
                    using (SqlCommand cm = cn.CreateCommand())
                    {
                        cm.CommandType = CommandType.StoredProcedure;
                        cm.CommandText = "GLLog_IsReconciled";

                        cm.Parameters.AddWithValue("@RetValue", 0);
                        cm.Parameters["@RetValue"].Direction = ParameterDirection.Output;

                        cm.Parameters.AddWithValue("@DocCodeKey", DocCodeKey);
                        cm.Parameters.AddWithValue("@DocKey", DocKey);
                        cm.Parameters.AddWithValue("@DocItmKey", DocItmKey);

                        cm.ExecuteNonQuery();

                        if ((int)cm.Parameters["@RetValue"].Value == (int)GEnum.SpState.Pass)
                            return true;
                        else
                            return false;

                    }

                }
                else return false;
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
        public static int DocKey_Get(int DocCodeKey, string DocID)
        {
            string Sql = string.Empty;
            DataTable dt;
            int docDKey = 0;
            DocID = DocID.Trim();

            #region Prepare SQL
            switch (DocCodeKey)
            {
                case (int)GEnum.SystemCode.Quotation:
                    Sql = "SELECT DocKey FROM AR_QO(nolock) WHERE DocID='" + DocID + "' AND DocCodeKey=" + DocCodeKey;
                    break;

                case (int)GEnum.SystemCode.Sales_Order:
                    Sql = "SELECT DocKey FROM AR_SO(nolock) WHERE DocID='" + DocID + "' AND DocCodeKey=" + DocCodeKey;
                    break;
                case (int)GEnum.SystemCode.Reserve_Order:
                    Sql = "SELECT DocKey FROM AR_RO(nolock) WHERE DocID='" + DocID + "' AND DocCodeKey=" + DocCodeKey;
                    break;
                case (int)GEnum.SystemCode.Sales_Order_Adjustment:
                case (int)GEnum.SystemCode.Purchase_Order_Adjustment:
                case (int)GEnum.SystemCode.Consignment_Order_Adjustment:
                    Sql = "SELECT DocKey FROM AP_PJ(nolock) WHERE DocID='" + DocID + "' AND DocCodeKey=" + DocCodeKey;
                    break;

                case (int)GEnum.SystemCode.Delivery_Order:
                    Sql = "SELECT DocKey FROM AR_DO(nolock) WHERE DocID='" + DocID + "' AND DocCodeKey=" + DocCodeKey;
                    break;

                case (int)GEnum.SystemCode.Packing_List:
                    Sql = "SELECT DocKey FROM AR_PL(nolock) WHERE DocID='" + DocID + "' AND DocCodeKey=" + DocCodeKey;
                    break;

                case (int)GEnum.SystemCode.Sales_Invoice:
                case (int)GEnum.SystemCode.Sales_Debit_Note:
                case (int)GEnum.SystemCode.Sales_Credit_Note:
                case (int)GEnum.SystemCode.Cash_Sale:
                case (int)GEnum.SystemCode.Cash_Debit_Note:
                case (int)GEnum.SystemCode.Cash_Credit_Note:
                    Sql = "SELECT DocKey FROM AR_IV(nolock) WHERE DocID='" + DocID + "' AND DocCodeKey=" + DocCodeKey;
                    break;

                case (int)GEnum.SystemCode.Sales_Adjustment:
                case (int)GEnum.SystemCode.Cash_Adjustment:
                    Sql = "SELECT DocKey FROM AR_ADJ(nolock) WHERE DocID='" + DocID + "' AND DocCodeKey=" + DocCodeKey;
                    break;

                case (int)GEnum.SystemCode.Payment_Received:
                case (int)GEnum.SystemCode.Cash_Payment_Received:
                    Sql = "SELECT DocKey FROM AR_PY(nolock) WHERE DocID='" + DocID + "' AND DocCodeKey=" + DocCodeKey;
                    break;

                case (int)GEnum.SystemCode.Contra:
                case (int)GEnum.SystemCode.Cash_Contra:
                    Sql = "SELECT DocKey FROM AR_CT(nolock) WHERE DocID='" + DocID + "' AND DocCodeKey=" + DocCodeKey;
                    break;

                case (int)GEnum.SystemCode.Purchase_Plan:
                    Sql = "SELECT DocKey FROM AP_PN(nolock) WHERE DocID='" + DocID + "' AND DocCodeKey=" + DocCodeKey;
                    break;

                case (int)GEnum.SystemCode.Purchase_Request:
                    Sql = "SELECT DocKey FROM AP_RQ(nolock) WHERE DocID='" + DocID + "' AND DocCodeKey=" + DocCodeKey;
                    break;

                case (int)GEnum.SystemCode.Purchase_Order:
                    Sql = "SELECT DocKey FROM AP_PO(nolock) WHERE DocID='" + DocID + "' AND DocCodeKey=" + DocCodeKey;
                    break;

                case (int)GEnum.SystemCode.Purchase_Delivery:
                    Sql = "SELECT DocKey FROM AP_PD(nolock) WHERE DocID='" + DocID + "' AND DocCodeKey=" + DocCodeKey;
                    break;

                case (int)GEnum.SystemCode.Purchase_Invoice:
                case (int)GEnum.SystemCode.Purchase_Debit_Note:
                case (int)GEnum.SystemCode.Purchase_Credit_Note:
                    Sql = "SELECT DocKey FROM AP_BL(nolock) WHERE DocID='" + DocID + "' AND DocCodeKey=" + DocCodeKey;
                    break;

                case (int)GEnum.SystemCode.Purchase_Adjustment:
                    Sql = "SELECT DocKey FROM AP_ADJ(nolock) WHERE DocID='" + DocID + "' AND DocCodeKey=" + DocCodeKey;
                    break;

                case (int)GEnum.SystemCode.Payment_Issue:
                    Sql = "SELECT DocKey FROM AP_PY(nolock) WHERE DocID='" + DocID + "' AND DocCodeKey=" + DocCodeKey;
                    break;

                case (int)GEnum.SystemCode.Inventory_Adjustment:
                    Sql = "SELECT DocKey FROM IN_ADJ(nolock) WHERE DocID='" + DocID + "' AND DocCodeKey=" + DocCodeKey;
                    break;

                case (int)GEnum.SystemCode.Inventory_Production:
                    Sql = "SELECT DocKey FROM IN_MFN(nolock) WHERE DocID='" + DocID + "' AND DocCodeKey=" + DocCodeKey;
                    break;

                case (int)GEnum.SystemCode.Inventory_Transfer:
                    Sql = "SELECT DocKey FROM IN_TRN(nolock) WHERE DocID='" + DocID + "' AND DocCodeKey=" + DocCodeKey;
                    break;

                case (int)GEnum.SystemCode.Issue_Consignment:
                case (int)GEnum.SystemCode.Return_Consignment:
                    Sql = "SELECT DocKey FROM CS_CSI WHERE DocID='" + DocID + "' AND DocCodeKey=" + DocCodeKey;
                    break;

                case (int)GEnum.SystemCode.Order_Consignment:
                    Sql = "SELECT DocKey FROM CS_CPO WHERE DocID='" + DocID + "' AND DocCodeKey=" + DocCodeKey;
                    break;

                case (int)GEnum.SystemCode.Received_Consignment:
                    Sql = "SELECT DocKey FROM CS_CPD WHERE DocID='" + DocID + "' AND DocCodeKey=" + DocCodeKey;
                    break;

                case (int)GEnum.SystemCode.Consignment_Settlement:
                    Sql = "SELECT DocKey FROM CS_CPS WHERE DocID='" + DocID + "' AND DocCodeKey=" + DocCodeKey;
                    break;

                case (int)GEnum.SystemCode.Journal:
                    Sql = "SELECT DocKey FROM GL_JNL(nolock) WHERE DocID='" + DocID + "' AND DocCodeKey=" + DocCodeKey;
                    break;

                case (int)GEnum.SystemCode.Deposit:
                    Sql = "SELECT DocKey FROM GL_DP(nolock) WHERE DocID='" + DocID + "' AND DocCodeKey=" + DocCodeKey;
                    break;

                case (int)GEnum.SystemCode.Bank_Revaluation:
                    Sql = "SELECT DocKey FROM GL_RV(nolock) WHERE DocID='" + DocID + "' AND DocCodeKey=" + DocCodeKey;
                    break;
                case (int)GEnum.SystemCode.Consolidate_Invoice:
                    Sql = "SELECT DocKey FROM AR_IVCSD(nolock) WHERE DocID='" + DocID + "' AND DocCodeKey=" + DocCodeKey;
                    break;

                default:
                    return 0;
            }
            #endregion

            //Check for Access to document for the DocConKey
            if (Sql != string.Empty)
            {
                dt = GFunc.ExecuteQuery(Sql);
                if (dt != null)
                {
                    if (dt.Rows.Count > 0)
                    {
                        Int32.TryParse(dt.Rows[0][0].ToString(), out docDKey);
                        return GFunc.NEInt(docDKey, 0);
                    }
                }
            }
            return 0;
        }

        /// <summary>
        /// Special overload function to get Dockey for APBL,APCN,APDN,APPD
        /// if result is 2 or more records ,function will return first record's dockey by descending DocDate
        /// </summary>
        /// <param name="DocCodeKey"></param>
        /// <param name="DocID"></param>
        /// <param name="DocConKey"></param>
        /// <returns>int DocKey </returns>
        public static int DocKey_Get(int DocCodeKey, int DocConKey, string DocID)
        {
            string Sql = string.Empty;
            DataTable dt;
            int docDKey = 0;
            DocID = DocID.Trim();

            #region Prepare SQL
            switch (DocCodeKey)
            {
                case (int)GEnum.SystemCode.Quotation:
                    Sql = "SELECT DocKey FROM AR_QO(nolock) WHERE DocID='" + DocID + "' AND DocCodeKey=" + DocCodeKey;
                    break;

                case (int)GEnum.SystemCode.Sales_Order:
                    Sql = "SELECT DocKey FROM AR_SO(nolock) WHERE DocID='" + DocID + "' AND DocCodeKey=" + DocCodeKey;
                    break;

                case (int)GEnum.SystemCode.Sales_Order_Adjustment:
                case (int)GEnum.SystemCode.Purchase_Order_Adjustment:
                case (int)GEnum.SystemCode.Consignment_Order_Adjustment:
                    Sql = "SELECT DocKey FROM AP_PJ(nolock) WHERE DocID='" + DocID + "' AND DocCodeKey=" + DocCodeKey;
                    break;

                case (int)GEnum.SystemCode.Delivery_Order:
                    Sql = "SELECT DocKey FROM AR_DO(nolock) WHERE DocID='" + DocID + "' AND DocCodeKey=" + DocCodeKey;
                    break;

                case (int)GEnum.SystemCode.Packing_List:
                    Sql = "SELECT DocKey FROM AR_PL(nolock) WHERE DocID='" + DocID + "' AND DocCodeKey=" + DocCodeKey;
                    break;

                case (int)GEnum.SystemCode.Sales_Invoice:
                case (int)GEnum.SystemCode.Sales_Debit_Note:
                case (int)GEnum.SystemCode.Sales_Credit_Note:
                case (int)GEnum.SystemCode.Cash_Sale:
                case (int)GEnum.SystemCode.Cash_Debit_Note:
                case (int)GEnum.SystemCode.Cash_Credit_Note:
                    Sql = "SELECT DocKey FROM AR_IV(nolock) WHERE DocID='" + DocID + "' AND DocCodeKey=" + DocCodeKey;
                    break;

                case (int)GEnum.SystemCode.Sales_Adjustment:
                case (int)GEnum.SystemCode.Cash_Adjustment:
                    Sql = "SELECT DocKey FROM AR_ADJ(nolock) WHERE DocID='" + DocID + "' AND DocCodeKey=" + DocCodeKey;
                    break;

                case (int)GEnum.SystemCode.Payment_Received:
                case (int)GEnum.SystemCode.Cash_Payment_Received:
                    Sql = "SELECT DocKey FROM AR_PY(nolock) WHERE DocID='" + DocID + "' AND DocCodeKey=" + DocCodeKey;
                    break;

                case (int)GEnum.SystemCode.Contra:
                case (int)GEnum.SystemCode.Cash_Contra:
                    Sql = "SELECT DocKey FROM AR_CT(nolock) WHERE DocID='" + DocID + "' AND DocCodeKey=" + DocCodeKey;
                    break;

                case (int)GEnum.SystemCode.Purchase_Plan:
                    Sql = "SELECT DocKey FROM AP_PN(nolock) WHERE DocID='" + DocID + "' AND DocCodeKey=" + DocCodeKey;
                    break;

                case (int)GEnum.SystemCode.Purchase_Request:
                    Sql = "SELECT DocKey FROM AP_RQ(nolock) WHERE DocID='" + DocID + "' AND DocCodeKey=" + DocCodeKey;
                    break;

                case (int)GEnum.SystemCode.Purchase_Order:
                    Sql = "SELECT DocKey FROM AP_PO(nolock) WHERE DocID='" + DocID + "' AND DocCodeKey=" + DocCodeKey;
                    break;
                case (int)GEnum.SystemCode.Purchase_Shipment:
                    if (DocConKey == 0)
                        Sql = "SELECT DocKey FROM AP_PS(nolock) WHERE (DocID='" + DocID + "' OR DocIVNum='" + DocID + "' )AND DocCodeKey=" + DocCodeKey + " ORDER BY DocDate Desc";
                    else
                        Sql = "SELECT DocKey FROM AP_PS(nolock) WHERE DocID='" + DocID + "' AND DocCodeKey=" + DocCodeKey + " AND DocConKey =" + DocConKey + " ORDER BY DocDate Desc";
                    break;
                case (int)GEnum.SystemCode.Purchase_Delivery:
                    if (DocConKey == 0)
                        Sql = "SELECT DocKey FROM AP_PD(nolock) WHERE (DocID='" + DocID + "' OR DocIVNum='" + DocID + "' )AND DocCodeKey=" + DocCodeKey + " ORDER BY DocDate Desc";
                    else
                        Sql = "SELECT DocKey FROM AP_PD(nolock) WHERE DocID='" + DocID + "' AND DocCodeKey=" + DocCodeKey + " AND DocConKey =" + DocConKey + " ORDER BY DocDate Desc";
                    break;


                case (int)GEnum.SystemCode.Purchase_Invoice:
                case (int)GEnum.SystemCode.Purchase_Debit_Note:
                case (int)GEnum.SystemCode.Purchase_Credit_Note:
                    if (DocConKey == 0)
                        Sql = "SELECT DocKey FROM AP_BL(nolock) WHERE DocID='" + DocID + "' AND DocCodeKey=" + DocCodeKey + " ORDER BY DocDate Desc";
                    else
                        Sql = "SELECT DocKey FROM AP_BL(nolock) WHERE DocID='" + DocID + "' AND DocCodeKey=" + DocCodeKey + " AND DocConKey =" + DocConKey + " ORDER BY DocDate Desc";
                    break;

                case (int)GEnum.SystemCode.Purchase_Adjustment:
                    Sql = "SELECT DocKey FROM AP_ADJ(nolock) WHERE DocID='" + DocID + "' AND DocCodeKey=" + DocCodeKey;
                    break;

                case (int)GEnum.SystemCode.Payment_Issue:
                    Sql = "SELECT DocKey FROM AP_PY(nolock) WHERE DocID='" + DocID + "' AND DocCodeKey=" + DocCodeKey;
                    break;

                case (int)GEnum.SystemCode.Inventory_Adjustment:
                    Sql = "SELECT DocKey FROM IN_ADJ(nolock) WHERE DocID='" + DocID + "' AND DocCodeKey=" + DocCodeKey;
                    break;

                case (int)GEnum.SystemCode.Inventory_Production:
                    Sql = "SELECT DocKey FROM IN_MFN(nolock) WHERE DocID='" + DocID + "' AND DocCodeKey=" + DocCodeKey;
                    break;

                case (int)GEnum.SystemCode.Inventory_Transfer:
                    Sql = "SELECT DocKey FROM IN_TRN(nolock) WHERE DocID='" + DocID + "' AND DocCodeKey=" + DocCodeKey;
                    break;

                case (int)GEnum.SystemCode.Issue_Consignment:
                case (int)GEnum.SystemCode.Return_Consignment:
                    Sql = "SELECT DocKey FROM CS_CSI(nolock) WHERE DocID='" + DocID + "' AND DocCodeKey=" + DocCodeKey;
                    break;

                case (int)GEnum.SystemCode.Order_Consignment:
                    Sql = "SELECT DocKey FROM CS_CPO(nolock) WHERE DocID='" + DocID + "' AND DocCodeKey=" + DocCodeKey;
                    break;

                case (int)GEnum.SystemCode.Received_Consignment:
                    Sql = "SELECT DocKey FROM CS_CPD WHERE DocID='" + DocID + "' AND DocCodeKey=" + DocCodeKey;
                    break;

                case (int)GEnum.SystemCode.Consignment_Settlement:
                    Sql = "SELECT DocKey FROM CS_CPS WHERE DocID='" + DocID + "' AND DocCodeKey=" + DocCodeKey;
                    break;

                case (int)GEnum.SystemCode.Journal:
                    Sql = "SELECT DocKey FROM GL_JNL(nolock) WHERE DocID='" + DocID + "' AND DocCodeKey=" + DocCodeKey;
                    break;

                case (int)GEnum.SystemCode.Deposit:
                    Sql = "SELECT DocKey FROM GL_DP(nolock) WHERE DocID='" + DocID + "' AND DocCodeKey=" + DocCodeKey;
                    break;

                case (int)GEnum.SystemCode.Bank_Revaluation:
                    Sql = "SELECT DocKey FROM GL_RV(nolock) WHERE DocID='" + DocID + "' AND DocCodeKey=" + DocCodeKey;
                    break;

                default:
                    return 0;
            }
            #endregion

            //Check for Access to document for the DocConKey
            if (Sql != string.Empty)
            {
                dt = GFunc.ExecuteQuery(Sql);
                if (dt != null)
                {
                    if (dt.Rows.Count > 0)
                    {
                        Int32.TryParse(dt.Rows[0][0].ToString(), out docDKey);
                        return GFunc.NEInt(docDKey, 0);
                    }
                }
            }
            return 0;
        }

        public static string DocID_Get(int? DocCodeKey, int? DocConKey, int? DocKey)
        {
            string Sql = string.Empty;
            DataTable dt;           


            #region Prepare SQL
            switch (DocCodeKey)
            {
                case (int)GEnum.SystemCode.Quotation:
                    Sql = "Select DocID FROM AR_QO(nolock) WHERE DocKey=" + DocKey + " AND DocCodeKey=" + DocCodeKey;
                    break;

                case (int)GEnum.SystemCode.Sales_Order:
                    Sql = "Select DocID FROM AR_SO(nolock) WHERE DocKey=" + DocKey + " AND DocCodeKey=" + DocCodeKey;
                    break;

                case (int)GEnum.SystemCode.Sales_Order_Adjustment:
                case (int)GEnum.SystemCode.Purchase_Order_Adjustment:
                case (int)GEnum.SystemCode.Consignment_Order_Adjustment:
                    Sql = "Select DocID FROM AP_PJ(nolock) WHERE DocKey=" + DocKey + " AND DocCodeKey=" + DocCodeKey;
                    break;

                case (int)GEnum.SystemCode.Delivery_Order:
                    Sql = "Select DocID FROM AR_DO(nolock) WHERE DocKey=" + DocKey + " AND DocCodeKey=" + DocCodeKey;
                    break;

                case (int)GEnum.SystemCode.Packing_List:
                    Sql = "Select DocID FROM AR_PL(nolock) WHERE DocKey=" + DocKey + " AND DocCodeKey=" + DocCodeKey;
                    break;

                case (int)GEnum.SystemCode.Sales_Invoice:
                case (int)GEnum.SystemCode.Sales_Debit_Note:
                case (int)GEnum.SystemCode.Sales_Credit_Note:
                case (int)GEnum.SystemCode.Cash_Sale:
                case (int)GEnum.SystemCode.Cash_Debit_Note:
                case (int)GEnum.SystemCode.Cash_Credit_Note:
                    Sql = "Select DocID FROM AR_IV(nolock) WHERE DocKey=" + DocKey + " AND DocCodeKey=" + DocCodeKey;
                    break;

                case (int)GEnum.SystemCode.Sales_Adjustment:
                case (int)GEnum.SystemCode.Cash_Adjustment:
                    Sql = "Select DocID FROM AR_ADJ(nolock) WHERE DocKey=" + DocKey + " AND DocCodeKey=" + DocCodeKey;
                    break;

                case (int)GEnum.SystemCode.Payment_Received:
                case (int)GEnum.SystemCode.Cash_Payment_Received:
                    Sql = "Select DocID FROM AR_PY(nolock) WHERE DocKey=" + DocKey + " AND DocCodeKey=" + DocCodeKey;
                    break;

                case (int)GEnum.SystemCode.Contra:
                case (int)GEnum.SystemCode.Cash_Contra:
                    Sql = "Select DocID FROM AR_CT(nolock) WHERE DocKey=" + DocKey + " AND DocCodeKey=" + DocCodeKey;
                    break;

                case (int)GEnum.SystemCode.Purchase_Plan:
                    Sql = "Select DocID FROM AP_PN(nolock) WHERE DocKey=" + DocKey + " AND DocCodeKey=" + DocCodeKey;
                    break;

                case (int)GEnum.SystemCode.Purchase_Request:
                    Sql = "Select DocID FROM AP_RQ(nolock) WHERE DocKey=" + DocKey + " AND DocCodeKey=" + DocCodeKey;
                    break;

                case (int)GEnum.SystemCode.Purchase_Order:
                    Sql = "Select DocID FROM AP_PO(nolock) WHERE DocKey=" + DocKey + " AND DocCodeKey=" + DocCodeKey;
                    break;

                case (int)GEnum.SystemCode.Purchase_Delivery:
                    if (DocConKey == 0)
                        Sql = "Select DocID FROM AP_PD(nolock) WHERE (DocKey=" + DocKey + " OR DocIVNum='" + DocKey + "' )AND DocCodeKey=" + DocCodeKey + " ORDER BY DocDate Desc";
                    else
                        Sql = "Select DocID FROM AP_PD(nolock) WHERE DocKey=" + DocKey + " AND DocCodeKey=" + DocCodeKey + " AND DocConKey =" + DocConKey + " ORDER BY DocDate Desc";
                    break;


                case (int)GEnum.SystemCode.Purchase_Invoice:
                case (int)GEnum.SystemCode.Purchase_Debit_Note:
                case (int)GEnum.SystemCode.Purchase_Credit_Note:
                    if (DocConKey == 0)
                        Sql = "Select DocID FROM AP_BL(nolock) WHERE DocKey=" + DocKey + " AND DocCodeKey=" + DocCodeKey + " ORDER BY DocDate Desc";
                    else
                        Sql = "Select DocID FROM AP_BL(nolock) WHERE DocKey=" + DocKey + " AND DocCodeKey=" + DocCodeKey + " AND DocConKey =" + DocConKey + " ORDER BY DocDate Desc";
                    break;

                case (int)GEnum.SystemCode.Purchase_Adjustment:
                    Sql = "Select DocID FROM AP_ADJ(nolock) WHERE DocKey=" + DocKey + " AND DocCodeKey=" + DocCodeKey;
                    break;

                case (int)GEnum.SystemCode.Payment_Issue:
                    Sql = "Select DocID FROM AP_PY(nolock) WHERE DocKey=" + DocKey + " AND DocCodeKey=" + DocCodeKey;
                    break;

                case (int)GEnum.SystemCode.Inventory_Adjustment:
                    Sql = "Select DocID FROM IN_ADJ(nolock) WHERE DocKey=" + DocKey + " AND DocCodeKey=" + DocCodeKey;
                    break;

                case (int)GEnum.SystemCode.Inventory_Production:
                    Sql = "Select DocID FROM IN_MFN(nolock) WHERE DocKey=" + DocKey + " AND DocCodeKey=" + DocCodeKey;
                    break;

                case (int)GEnum.SystemCode.Inventory_Transfer:
                    Sql = "Select DocID FROM IN_TRN(nolock) WHERE DocKey=" + DocKey + " AND DocCodeKey=" + DocCodeKey;
                    break;

                case (int)GEnum.SystemCode.Issue_Consignment:
                case (int)GEnum.SystemCode.Return_Consignment:
                    Sql = "Select DocID FROM CS_CSI WHERE DocKey=" + DocKey + " AND DocCodeKey=" + DocCodeKey;
                    break;

                case (int)GEnum.SystemCode.Order_Consignment:
                    Sql = "Select DocID FROM CS_CPO WHERE DocKey=" + DocKey + " AND DocCodeKey=" + DocCodeKey;
                    break;

                case (int)GEnum.SystemCode.Received_Consignment:
                    Sql = "Select DocID FROM CS_CPD WHERE DocKey=" + DocKey + " AND DocCodeKey=" + DocCodeKey;
                    break;

                case (int)GEnum.SystemCode.Consignment_Settlement:
                    Sql = "Select DocID FROM CS_CPS WHERE DocKey=" + DocKey + " AND DocCodeKey=" + DocCodeKey;
                    break;

                case (int)GEnum.SystemCode.Journal:
                    Sql = "Select DocID FROM GL_JNL(nolock) WHERE DocKey=" + DocKey + " AND DocCodeKey=" + DocCodeKey;
                    break;

                case (int)GEnum.SystemCode.Deposit:
                    Sql = "Select DocID FROM GL_DP(nolock) WHERE DocKey=" + DocKey + " AND DocCodeKey=" + DocCodeKey;
                    break;

                case (int)GEnum.SystemCode.Bank_Revaluation:
                    Sql = "Select DocID FROM GL_RV(nolock) WHERE DocKey=" + DocKey + " AND DocCodeKey=" + DocCodeKey;
                    break;

                default:
                    return string.Empty;
            }
            #endregion

            //Check for Access to document for the DocConKey
            if (Sql != string.Empty)
            {
                dt = GFunc.ExecuteQuery(Sql);
                if (dt != null)
                {
                    if (dt.Rows.Count > 0)
                    {
                        return dt.Rows[0][0].ToString();
                    }
                }
            }
            return string.Empty;
        }
        public static DataTable GetDefaultDocType(SqlConnection cn, int? codeKey)
        {
            // Initialize output 
            string msgID = MsgID.Common.GetFail;

            // Preapre parameter list (Require parameter at least @MsgID)
            List<SqlParameter> parmList = new List<SqlParameter>();
            parmList.Add(new SqlParameter("@Option", 3));
            parmList.Add(new SqlParameter("@MsgID", SqlDbType.NVarChar, 255));
            parmList.Add(new SqlParameter("@CodeKey", codeKey));
            parmList.Add(new SqlParameter("@DocType", DBNull.Value));
            parmList.Add(new SqlParameter("@RetValue", 0));

            parmList[1].Direction = ParameterDirection.InputOutput;
            parmList[1].Value = msgID;
            parmList[4].Direction = ParameterDirection.Output;

            return GFunc.ExecuteProc(cn, "SYSDocType_Get", parmList);

        }
        public static DataTable GetDefaultDocType(int? codeKey)
        {
            // Initialize output 
            string msgID = MsgID.Common.GetFail;
            SqlConnection cn = new SqlConnection(Database.BossDemoConnection);
            cn.Open();
            // Preapre parameter list (Require parameter at least @MsgID)
            List<SqlParameter> parmList = new List<SqlParameter>();
            parmList.Add(new SqlParameter("@Option", 3));
            parmList.Add(new SqlParameter("@MsgID", SqlDbType.NVarChar, 255));
            parmList.Add(new SqlParameter("@CodeKey", codeKey));
            parmList.Add(new SqlParameter("@DocType", DBNull.Value));
            parmList.Add(new SqlParameter("@RetValue", 0));

            parmList[1].Direction = ParameterDirection.InputOutput;
            parmList[1].Value = msgID;
            parmList[4].Direction = ParameterDirection.Output;

            return GFunc.ExecuteProc(cn, "SYSDocType_Get", parmList);

        }
        public static int DecimalPlace_Get(decimal value)
        {

            if (GFunc.IsNEZ(value) == false)
            {
                return value.ToString().Split(".".ToCharArray())[1].Length;
            }
            return 0;
        }

        public static bool IsBatchItmType(object value)
        {
            bool IsBatchItmType = false;

            if (!GFunc.IsNEZ(value))
            {
                switch (GFunc.NEInt(value, 0))
                {

                    case (int)GEnum.ItemType.StockB:
                    case (int)GEnum.ItemType.Finished_GDB:
                    case (int)GEnum.ItemType.Serial_StockB:
                    case (int)GEnum.ItemType.Serial_Finished_GDB:
                        IsBatchItmType = true;
                        break;
                }
            }
            return IsBatchItmType;
        }
        public static int GetINTypeGroup(object value)
        {
            int INGrpType = (int)GEnum.INTypeGrp.Empty;

            if (!GFunc.IsNEZ(value))
            {
                switch (GFunc.NEInt(value, 0))
                {
                    case (int)GEnum.ItemType.Stock:
                    case (int)GEnum.ItemType.StockB:
                    case (int)GEnum.ItemType.Finished_GD:
                    case (int)GEnum.ItemType.Finished_GDB:
                    case (int)GEnum.ItemType.Serial_StockB:
                    case (int)GEnum.ItemType.Serial_Finished_GDB:
                    //case (int)GEnum.ItemType.Substitute: not use -- comment by pauk
                    case (int)GEnum.ItemType.Consignment:
                        INGrpType = (int)GEnum.INTypeGrp.Stock;
                        break;

                    case (int)GEnum.ItemType.Assembly:
                    case (int)GEnum.ItemType.Non_Stock:
                    case (int)GEnum.ItemType.Service:
                        INGrpType = (int)GEnum.INTypeGrp.Non_Stock;
                        break;

                    case (int)GEnum.ItemType.Charges:
                        INGrpType = (int)GEnum.INTypeGrp.Charges;
                        break;

                    case (int)GEnum.ItemType.Discount:
                        INGrpType = (int)GEnum.INTypeGrp.Discount;
                        break;

                    case (int)GEnum.ItemType.Remark:
                    case (int)GEnum.ItemType.Header:
                        INGrpType = (int)GEnum.INTypeGrp.Remark;
                        break;

                    case (int)GEnum.ItemType.Total:
                    case (int)GEnum.ItemType.Sub_Total:
                    case (int)GEnum.ItemType.BF_Total:
                    case (int)GEnum.ItemType.Master:
                        INGrpType = (int)GEnum.INTypeGrp.Total;
                        break;
                }
            }
            return INGrpType;
        }

        public static string GetItmTypeGyDocCode(int docCodeKey)
        {
            switch (docCodeKey)
            {
                //AR Document
                case 11100: return "100, 110, 200, 210, 250, 310, 410, 500, 510, 600, 610, 700, 710, 800, 810, 820, 825, 830, 0 ";
                case 11150: return "100, 110, 200, 210, 250, 310, 410, 500, 510, 600, 610, 700, 710, 800, 810, 820, 825, 830, 0 ";
                case 11200: return "100, 110, 200, 210, 250, 310, 410, 500, 510, 600, 610, 700, 710, 800, 810, 820, 825, 830, 0 ";
                case 11300: return "100, 110, 200, 210, 250, 310, 410, 500, 510, 600, 610, 700, 710, 800, 810, 820, 825, 830, 0 ";
                case 11350: return "100, 110, 200, 210, 250, 310, 410, 500, 510, 600, 610, 700, 710, 800, 810, 820, 825, 830, 0 ";
                case 11450: return "100, 110, 200, 210, 250, 310, 410, 500, 510, 600, 610, 700, 710, 800, 810, 820, 825, 830, 0 ";
                case 11500: return "100, 110, 200, 210, 250, 310, 410, 500, 510, 600, 610, 700, 710, 800, 810, 820, 825, 830, 0 ";
                case 11550: return "100, 110, 200, 210, 250, 310, 410, 500, 510, 600, 610, 700, 710, 800, 810, 820, 825, 830, 0 ";
                case 11600: return "100, 110, 200, 210, 250, 310, 410, 500, 510, 600, 610, 700, 710, 800, 810, 820, 825, 830, 0 ";
                case 11650: return "100, 110, 200, 210, 250, 310, 410, 500, 510, 600, 610, 700, 710, 800, 810, 820, 825, 830, 0 ";
                case 11700: return "100, 110, 200, 210, 250, 310, 410, 500, 510, 600, 610, 700, 710, 800, 810, 820, 825, 830, 0 ";
                case 11850: return "100, 110, 200, 210, 250, 310, 410, 500, 510, 600, 610, 700, 710, 800, 810, 820, 825, 830, 0 ";

                //Cash Document
                case 12100: return "100, 110, 200, 210, 250, 310, 410, 500, 510, 600, 610, 700, 710, 800, 810, 820, 825, 830, 0 ";
                case 12150: return "100, 110, 200, 210, 250, 310, 410, 500, 510, 600, 610, 700, 710, 800, 810, 820, 825, 830, 0 ";
                case 12200: return "100, 110, 200, 210, 250, 310, 410, 500, 510, 600, 610, 700, 710, 800, 810, 820, 825, 830, 0 ";

                //AP Document
                case 13100: return "100, 110, 200, 210, 0, 310, 410, 500, 0, 600, 610, 700, 710, 800, 810, 820, 825, 830, 0 ";
                case 13200: return "100, 110, 200, 210, 0, 310, 410, 500, 0, 600, 610, 700, 710, 800, 810, 820, 825, 830, 0 ";
                case 13250: return "100, 110, 200, 210, 0, 310, 410, 500, 0, 600, 610, 700, 710, 800, 810, 820, 825, 830, 0 ";
                case 13300: return "100, 110, 200, 210, 0, 310, 410, 500, 0, 600, 610, 700, 710, 800, 810, 820, 825, 830, 0 ";
                case 13450: return "100, 110, 200, 210, 0, 310, 410, 500, 0, 600, 610, 700, 710, 800, 810, 820, 825, 830, 0 ";
                case 13500: return "100, 110, 200, 210, 0, 310, 410, 500, 0, 600, 610, 700, 710, 800, 810, 820, 825, 830, 0 ";
                case 13550: return "100, 110, 200, 210, 0, 310, 410, 500, 0, 600, 610, 700, 710, 800, 810, 820, 825, 830, 0 ";
                case 13600: return "100, 110, 200, 210, 0, 310, 410, 500, 0, 600, 610, 700, 710, 800, 810, 820, 825, 830, 0 ";
                case 13650: return "100, 110, 200, 210, 0, 310, 410, 500, 0, 600, 610, 700, 710, 800, 810, 820, 825, 830, 0 ";
                case 13700: return "100, 110, 200, 210, 0, 310, 410, 500, 0, 600, 610, 700, 710, 800, 810, 820, 825, 830, 0 ";

                //Consignment Document
                case 15100: return " 100, 110, 200, 210, 0, 310, 410, 500, 510, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 ";
                case 15150: return " 100, 110, 200, 210, 0, 310, 410, 500, 510, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 ";

                case 15250: return " 0, 0, 0, 0, 0, 0, 0, 0, 510, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 ";

                case 15400: return " 0, 0, 0, 0, 0, 0, 0, 0, 510, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 ";

                //Job
                case 24100: return " 100, 110, 200, 210, 0, 310, 410, 0, 510, 600, 0, 700, 0, 0, 0, 0, 0, 0, 0 ";
                default:
                    return "";
            }
        }

        public static bool IsPostingItmType(object value)
        {
            if (GFunc.IsNEZ(value))
                return false;
            else
            {
                switch ((int)value)
                {
                    case (int)GEnum.ItemType.Stock:
                    case (int)GEnum.ItemType.StockB:
                    case (int)GEnum.ItemType.Finished_GD:
                    case (int)GEnum.ItemType.Finished_GDB:
                    case (int)GEnum.ItemType.Serial_StockB:
                    case (int)GEnum.ItemType.Serial_Finished_GDB:
                    case (int)GEnum.ItemType.Consignment:
                    case (int)GEnum.ItemType.Assembly:
                    case (int)GEnum.ItemType.Non_Stock:
                    case (int)GEnum.ItemType.Service:
                    case (int)GEnum.ItemType.Charges:
                    case (int)GEnum.ItemType.Discount:
                        return true;

                    default:
                        return false;
                }
            }
        }
        public static bool IsStockGroupItmType(object value)
        {
            if (GFunc.IsNEZ(value))
                return false;
            else
            {
                switch ((int)value)
                {
                    case (int)GEnum.ItemType.Stock:
                    case (int)GEnum.ItemType.StockB:
                    case (int)GEnum.ItemType.Finished_GD:
                    case (int)GEnum.ItemType.Finished_GDB:
                    case (int)GEnum.ItemType.Serial_StockB:
                    case (int)GEnum.ItemType.Serial_Finished_GDB:
                    case (int)GEnum.ItemType.Consignment:
                        return true;

                    default:
                        return false;
                }
            }
        }
        public static DataTable GetSECGrpIDList()
        {
            DataTable dataTable = new DataTable();
            dataTable.Columns.Add("permGrpKey", typeof(int));
            dataTable.Columns.Add("permGrpDesc", typeof(string));

            DataRow dr1 = dataTable.NewRow();
            dr1["permGrpKey"] = 10;
            dr1["permGrpDesc"] = "System";
            dataTable.Rows.Add(dr1);

            DataRow dr2 = dataTable.NewRow();
            dr2["permGrpKey"] = 20;
            dr2["permGrpDesc"] = "Account";
            dataTable.Rows.Add(dr2);

            DataRow dr3 = dataTable.NewRow();
            dr3["permGrpKey"] = 30;
            dr3["permGrpDesc"] = "Contacts";
            dataTable.Rows.Add(dr3);

            DataRow dr4 = dataTable.NewRow();
            dr4["permGrpKey"] = 40;
            dr4["permGrpDesc"] = "Items";
            dataTable.Rows.Add(dr4);

            DataRow dr5 = dataTable.NewRow();
            dr5["permGrpKey"] = 50;
            dr5["permGrpDesc"] = "Job";
            dataTable.Rows.Add(dr5);

            DataRow dr6 = dataTable.NewRow();
            dr6["permGrpKey"] = 60;
            dr6["permGrpDesc"] = "Document AR";
            dataTable.Rows.Add(dr6);

            DataRow dr7 = dataTable.NewRow();
            dr7["permGrpKey"] = 70;
            dr7["permGrpDesc"] = "Document AP";
            dataTable.Rows.Add(dr7);

            DataRow dr8 = dataTable.NewRow();
            dr8["permGrpKey"] = 90;
            dr8["permGrpDesc"] = "Document Inventory";
            dataTable.Rows.Add(dr8);

            DataRow dr9 = dataTable.NewRow();
            dr9["permGrpKey"] = 100;
            dr9["permGrpDesc"] = "Document Consignment";
            dataTable.Rows.Add(dr9);

            DataRow dr10 = dataTable.NewRow();
            dr10["permGrpKey"] = 110;
            dr10["permGrpDesc"] = "Document Account";
            dataTable.Rows.Add(dr10);

            DataRow dr11 = dataTable.NewRow();
            dr11["permGrpKey"] = 120;
            dr11["permGrpDesc"] = "Security";
            dataTable.Rows.Add(dr11);

            DataRow dr12 = dataTable.NewRow();
            dr12["permGrpKey"] = 130;
            dr12["permGrpDesc"] = "Report";
            dataTable.Rows.Add(dr12);

            DataRow dr13 = dataTable.NewRow();
            dr13["permGrpKey"] = 140;
            dr13["permGrpDesc"] = "Equipment";
            dataTable.Rows.Add(dr13);

            DataRow dr14 = dataTable.NewRow();
            dr14["permGrpKey"] = 150;
            dr14["permGrpDesc"] = "Tasks and Alerts";
            dataTable.Rows.Add(dr14);

            return dataTable;
        }
        public static char[] ToBinary(int value)
        {
            // Declare a few variables we're going to need
            int BinaryHolder;
            char[] BinaryArray;
            string BinaryResult = "";

            while (value > 0)
            {
                BinaryHolder = value % 2;
                BinaryResult += BinaryHolder;
                value = value / 2;
            }

            // The algoritm gives us the binary number in reverse order (mirrored)
            // We store it in an array so that we can reverse it back to normal
            BinaryArray = BinaryResult.ToCharArray();
            // Array.Reverse(BinaryArray);
            // BinaryResult = new string(BinaryArray);

            // return BinaryResult;
            return BinaryArray;
        }

        public static string GetCurrencyID(int currkey)
        {

            REFCurr objCurr = REFCurr.New();

            if (objCurr.Fetch(new REFCurr.Criteria(currkey, 1)))
                return objCurr._currID;

            return string.Empty;
        }

        public static string GetDocTableName(int DataGrp, int DocCodekey)
        {
            SYSMsgList objMsgList = SYSMsgList.New();

            if (objMsgList.Fetch(new SYSMsgList.Criteria(DataGrp, DocCodekey, 2)))
                return objMsgList._dataDes;

            return string.Empty;
        }
        public static bool CheckKeyDependantsExists(SqlConnection cn, string KeyName, int KeyValue, string IDValue)
        {
            bool retValue = true;
            string msgID = MsgID.Common.GetFail;

            try
            {
                // Using existing sql connection.
                using (SqlCommand cm = cn.CreateCommand())
                {
                    cm.CommandType = CommandType.StoredProcedure;
                    cm.CommandText = "CheckKey_BeforeDelete";

                    cm.Parameters.Add("@MsgID", SqlDbType.NVarChar, 500);
                    cm.Parameters["@MsgID"].Value = msgID;
                    cm.Parameters.AddWithValue("@KeyName", KeyName);
                    cm.Parameters.AddWithValue("@KeyValue", KeyValue);
                    cm.Parameters.AddWithValue("@IDValue", IDValue);
                    cm.Parameters.AddWithValue("@RetValue", 0);
                    cm.Parameters["@MsgID"].Direction = ParameterDirection.InputOutput;

                    cm.Parameters["@RetValue"].Direction = ParameterDirection.Output;

                    cm.ExecuteNonQuery();

                    if (cm.Parameters["@MsgID"].Value == null)
                        msgID = string.Empty;
                    else
                        msgID = cm.Parameters["@MsgID"].Value.ToString();

                    if ((int)cm.Parameters["@RetValue"].Value == (int)GEnum.SpState.Pass)
                        retValue = false;
                    else
                        MsgBox.Show(cn, msgID);

                }// Already close and dispose sql connection.


            }
            catch (Exception ex)
            {
                throw ex;
            }

            return retValue;
        }
        public static bool CheckBatchDependantsExists(SqlConnection cn, int BatchKey, int DocCodeKey, int DocKey)
        {
            try
            {
                using (SqlCommand cm = cn.CreateCommand())
                {
                    cm.CommandType = CommandType.StoredProcedure;
                    cm.CommandText = "CheckBatchDepandency";

                    cm.Parameters.AddWithValue("@BatchKey", BatchKey);
                    cm.Parameters.AddWithValue("@DocCodeKey", DocCodeKey);
                    cm.Parameters.AddWithValue("@DocKey", DocKey);
                    cm.Parameters.AddWithValue("@RetValue", 0);
                    cm.Parameters["@RetValue"].Direction = ParameterDirection.Output;

                    cm.ExecuteNonQuery();

                    if (GFunc.NEInt(cm.Parameters["@RetValue"].Value, 0) != (int)GEnum.SpState.Pass)
                    {
                        MsgBox.Show(cn, "Batch is used in other transactions and cannot perform your action.");
                        return true;
                    }
                }
                return false;
            }
            catch (Exception ex)
            {
                throw Error(ex);
            }
        }
        public static bool CheckDocTypeDependantsExists(string DocTypeNm)
        {
            try
            {
                using (SqlConnection cn = new SqlConnection(Database.BossDemoConnection))
                {
                    cn.Open();
                    using (SqlCommand cm = cn.CreateCommand())
                    {
                        cm.CommandType = CommandType.StoredProcedure;
                        cm.CommandText = "CheckDocTypeDepandency";

                        cm.Parameters.AddWithValue("@DocTypeNm", DocTypeNm);
                        cm.Parameters.AddWithValue("@ConID", "");
                        cm.Parameters["@ConID"].SqlDbType = SqlDbType.NVarChar;
                        cm.Parameters["@ConID"].Size = 50;
                        cm.Parameters["@ConID"].Direction = ParameterDirection.Output;
                        cm.Parameters.AddWithValue("@ConNm", "");
                        cm.Parameters["@ConNm"].SqlDbType = SqlDbType.NVarChar;
                        cm.Parameters["@ConNm"].Size = 255;
                        cm.Parameters["@ConNm"].Direction = ParameterDirection.Output;
                        cm.Parameters.AddWithValue("@RetValue", 0);
                        cm.Parameters["@RetValue"].Direction = ParameterDirection.Output;
                        
                        cm.ExecuteNonQuery();

                        if (GFunc.NEInt(cm.Parameters["@RetValue"].Value, 0) == 1 )
                        {
                            MsgBox.Show(cn, "DocType is used in Vendor: " + cm.Parameters["@ConID"].Value +" and cannot perform your action.");
                            return true;
                        }
                    }
                }
                return false;
            }
            catch (Exception ex)
            {
                throw Error(ex);
            }
        }
        public static bool CheckBatchDependantsExists(int BatchKey, int DocCodeKey, int DocKey)
        {
            try
            {
                using (SqlConnection cn = new SqlConnection(Database.BossDemoConnection))
                {
                    cn.Open();
                    using (SqlCommand cm = cn.CreateCommand())
                    {
                        cm.CommandType = CommandType.StoredProcedure;
                        cm.CommandText = "CheckBatchDepandency";

                        cm.Parameters.AddWithValue("@BatchKey", BatchKey);
                        cm.Parameters.AddWithValue("@DocCodeKey", DocCodeKey);
                        cm.Parameters.AddWithValue("@DocKey", DocKey);
                        cm.Parameters.AddWithValue("@RetValue", 0);
                        cm.Parameters["@RetValue"].Direction = ParameterDirection.Output;

                        cm.ExecuteNonQuery();

                        if (GFunc.NEInt(cm.Parameters["@RetValue"].Value, 0) != (int)GEnum.SpState.Pass)
                        {
                            MsgBox.Show(cn, "Batch is used in other transactions and cannot perform your action.");
                            return true;
                        }
                    }
                }
                return false;
            }
            catch (Exception ex)
            {
                throw Error(ex);
            }
        }
        public static SYSAttachments Get_Attachments(int docKey, int docCodeKey, Document copyDoc)
        {

            SYSAttachments Attachments = new SYSAttachments();
            try
            {
                DataTable dtAttachment = new DataTable();


                //Get Attachement
                List<SqlParameter> para = new List<SqlParameter>();
                para.Add(new SqlParameter("@DocKey", docKey));
                para.Add(new SqlParameter("@DocCodeKey", docCodeKey));
                para.Add(new SqlParameter("@DocKeyList", ""));
                dtAttachment = GFunc.ExecuteProc("Doc_Attachment_Get", para);

                if (dtAttachment.Rows.Count > 0)
                {
                    DocComUtility.ItmAttachment_Set(dtAttachment, ref Attachments, null);
                    foreach (SYSAttachment attach in Attachments)
                    {
                        attach.DocDC = copyDoc.DocCodeKey;
                        attach.DocDK = copyDoc.DocKey;

                    }
                }
                return Attachments;
            }
            catch (TAException tex)
            {
                throw Error(tex);
            }
            catch (Exception ex)
            {
                throw Error(ex);
            }
            return Attachments;
        }

        public static bool GetPopupListDataWithParams(string lstID, ref DataTable dt, string[] parmList, bool searchByDes)
        {
            try
            {
                // Initialize output               
                if (!IsNE(parmList))
                    parmList = AddAccessLevelParam(parmList, lstID);

                if (GFunc.CompareString(lstID, "frmListGridShipName") == false && GFunc.CompareString(lstID, "frmListGridPrice") == false)//Not for two open ID
                {
                    if (searchByDes)

                        lstID = lstID.Replace("id", "des");
                    else
                        lstID = lstID.Replace("des", "id");
                }

                SYSFormSettingID objSysFormSetting = SYSFormSettingID.Get(lstID);

                if (!GFunc.IsNE(objSysFormSetting))
                {
                    for (int i = 0; i < parmList.Length; i++)
                        objSysFormSetting._listSQL = objSysFormSetting._listSQL.Replace("{" + i + "}", parmList[i]);

                    dt = ExecuteQuery(objSysFormSetting._listSQL);
                }
                else
                    return false;

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

        public static bool GetPopupListDataSettingWithParams(string lstID, ref DataTable dt, string[] parmList)
        {
            // Initialize output 
            try
            {
                int j = 0;
                SYSFormSettingID objSysListSetting = SYSFormSettingID.Get(lstID);

                if (!GFunc.IsNE(objSysListSetting))
                {
                    //if (lstID == "MSTJobSalesByConKey")
                    //{
                    //    for (int i = 1; i < parmList.Length; i++)
                    //    {

                    //        objSysListSetting._listSQL = objSysListSetting._listSQL.Replace("{" + j + "}", parmList[i] == " " ? "" : parmList[i]);
                    //        j++;
                    //    }
                    //}
                    //else
                    //{
                    for (int i = 0; i < parmList.Length; i++)
                        objSysListSetting._listSQL = objSysListSetting._listSQL.Replace("{" + i + "}", parmList[i] == " " ? "" : parmList[i]);
                    //}



                    dt = ExecuteQuery(objSysListSetting._listSQL);
                }
                else
                    return false;

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

        ////This function is mainly used by Record Search form. May modified on 30-Dec-2010, The old code was using old ListSetting table.
        //public static bool GetPopupListDataSettingWithParams(string lstID, ref DataTable dt, string[] parmList, ref string[] colHeaders,
        //    ref string[] colWidths, ref string[] colFormats, out string assignCol, out string assignCol1, bool searchByDes)
        //{
        //    // Initialize output 
        //    try
        //    {
        //        assignCol = string.Empty;
        //        assignCol1 = string.Empty;

        //       // lstID = GetListID(lstID, searchByDes);
        //       // SYSListSetting objSysListSetting = SYSListSetting.Get(lstID);

        //        SYSFormSettingID objSysListSetting = SYSFormSettingID.Get(lstID);

        //        if (!GFunc.IsNE(objSysListSetting))
        //        {
        //            for (int i = 0; i < parmList.Length; i++)
        //                objSysListSetting._listSQL = objSysListSetting._listSQL.Replace("{" + i + "}", parmList[i] == " " ? "" : parmList[i]);

        //            dt = ExecuteQuery(objSysListSetting._listSQL);

        //            //colHeaders = objSysListSetting._colHeader1.Split(',');
        //            //colWidths = objSysListSetting._colWidth.Split(',');
        //            //colFormats = objSysListSetting._colFormat.Split(',');

        //            //assignCol = objSysListSetting._valueColName;
        //            //assignCol1 = objSysListSetting._displayColName;
        //            //msgID = objSysListSetting._programCode;
        //        }
        //        else
        //            return false;

        //        return true;
        //    }
        //    catch (Exception ex)
        //    {
        //        throw Error(ex);
        //    }
        //}
        //public static bool GetPopupListDataSettingWithParams(ref string lstID, ref DataTable dt, string[] parmList, ref string[] colHeaders,
        //    ref string[] colWidths, ref string[] colFormats, out string assignCol, out string assignCol1, bool searchByDes)
        //{
        //    // Initialize output 
        //    try
        //    {
        //        assignCol = string.Empty;
        //        assignCol1 = string.Empty;

        //        lstID = GetListID(lstID, searchByDes);
        //        // SYSListSetting objSysListSetting = SYSListSetting.Get(lstID);
        //        SYSFormSettingID objSysFormSetting = SYSFormSettingID.Get(lstID);

        //        if (!GFunc.IsNE(objSysFormSetting))
        //        {
        //            for (int i = 0; i < parmList.Length; i++)
        //                objSysFormSetting._listSQL = objSysFormSetting._listSQL.Replace("{" + i + "}", parmList[i] == " " ? "" : parmList[i]);

        //            dt = ExecuteQuery(objSysFormSetting._listSQL);

        //            colHeaders = objSysFormSetting._colHeader.Split(',');
        //            colWidths = objSysFormSetting._colWidth.Split(',');
        //            colFormats = objSysFormSetting._colPosition.Split(',');

        //            assignCol = objSysFormSetting._comboMemberValue;
        //            assignCol1 = objSysFormSetting._comboMemberDisplay;
        //            //msgID = objSysListSetting._programCode;
        //        }
        //        else
        //            return false;

        //        return true;
        //    }
        //    catch (Exception ex)
        //    {
        //        throw Error(ex);
        //    }
        //}

        //---New function with FormSettingID---//
        //public static bool GetFormListDataSettingWithParams(string lstID, ref DataTable dt, string[] parmList, ref SYSFormSettingID objSysFormSetting)
        //{
        //    // Initialize output

        //    try
        //    {
        //        //lstID = GetListID(lstID, searchByDes);
        //        objSysFormSetting = SYSFormSettingID.GetFormList(lstID);
        //        string SettingQuery = objSysFormSetting._listSQL;

        //        if (!GFunc.IsNE(objSysFormSetting))
        //        {
        //            for (int i = 0; i < parmList.Length; i++)
        //                SettingQuery = SettingQuery.Replace("{" + i + "}", parmList[i]);

        //            dt = ExecuteQuery(SettingQuery);
        //        }
        //        else
        //            return false;

        //        return true;
        //    }
        //    catch (Exception ex)
        //    {
        //        throw Error(ex);
        //    }
        //}

        //public static bool GetFormListDataSettingWithParams(string lstID, ref DataTable dt, string[] parmList, ref SYSListSetting objSysListSetting)
        //{
        //    // Initialize output 

        //    try
        //    {
        //        //lstID = GetListID(lstID, searchByDes);
        //        objSysListSetting = SYSListSetting.GetFormList(lstID);
        //        string SettingQuery = objSysListSetting._listSQL;

        //        if (!GFunc.IsNE(objSysListSetting))
        //        {
        //            for (int i = 0; i < parmList.Length; i++)
        //                SettingQuery = SettingQuery.Replace("{" + i + "}", parmList[i]);

        //            dt = ExecuteQuery(SettingQuery);
        //        }
        //        else
        //            return false;

        //        return true;
        //    }
        //    catch (Exception ex)
        //    {
        //        throw Error(ex);
        //    }
        //}
        //public static bool GetFormListDataSettingWithParams(string lstID, ref DataTable dt, int?[] parmList, ref SYSListSetting objSysListSetting)
        //{
        //    // Initialize output 

        //    try
        //    {
        //        //lstID = GetListID(lstID, searchByDes);
        //        objSysListSetting = SYSListSetting.GetFormList(lstID);
        //        string SettingQuery = objSysListSetting._listSQL;

        //        if (!GFunc.IsNE(objSysListSetting))
        //        {
        //            for (int i = 0; i < parmList.Length; i++)
        //                SettingQuery = SettingQuery.Replace("{" + i + "}", parmList[i].ToString());

        //            dt = ExecuteQuery(SettingQuery);
        //        }
        //        else
        //            return false;

        //        return true;
        //    }
        //    catch (Exception ex)
        //    {
        //        throw Error(ex);
        //    }
        //}

        public static bool GetFormListDataSetting(string lstID, ref DataTable dt, ref SYSFormSettingID objSysFormSetting)
        {
            // Initialize output
            try
            {

                //lstID = GetListID(lstID, searchByDes);
                objSysFormSetting = SYSFormSettingID.GetFormList(lstID);

                if (!GFunc.IsNE(objSysFormSetting))
                {
                    dt = ExecuteQuery(objSysFormSetting._listSQL);
                }
                else
                    return false;

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

        public static Object CopyDocumentHeader(DataTable dt, Object obj)
        {
            try
            {
                PropertyInfo[] properties = obj.GetType().GetProperties();

                foreach (PropertyInfo pi in properties)
                {
                    if (dt.Columns.Contains(pi.Name))
                    {
                        if (dt.Rows[0][pi.Name] != System.DBNull.Value)
                        {
                            try
                            {
                                pi.SetValue(obj, dt.Rows[0][pi.Name], null);
                            }
                            catch (Exception ex)
                            {
                                throw ex;
                            }
                        }
                    }

                    if (pi.Name.StartsWith("Svr"))
                    {
                        if (dt.Columns.Contains(pi.Name.Substring(3)))
                        {
                            if (dt.Rows[0][pi.Name.Substring(3)] != System.DBNull.Value)
                            {
                                pi.SetValue(obj, dt.Rows[0][pi.Name.Substring(3)], null);

                            }
                        }
                    }
                }
                return obj;
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
        public static void CopyDocumentDetail(DataTable dtSource, DataTable dtDest)
        {
            try
            {
                if (dtDest == null)
                {
                    dtDest = new DataTable();
                }
                if (GFunc.IsNE(dtDest.TableName))
                {
                    dtDest.TableName = dtSource.TableName;
                }
                if (dtDest.Columns.Count == 0) // Assume New Table
                {
                    foreach (DataColumn dcSource in dtSource.Columns)
                    {
                        dtDest.Columns.Add(dcSource.ColumnName, dcSource.DataType);
                    }
                }
                foreach (DataRow drSource in dtSource.Rows)
                {
                    DataRow dtNew = dtDest.NewRow();
                    foreach (DataColumn dcSource in dtSource.Columns)
                    {
                        if (dtDest.Columns.Contains(dcSource.ColumnName))
                        {
                            //if (!dtFilter.Columns.Contains(dcSource.ColumnName))
                            //{
                            dtNew[dcSource.ColumnName] = drSource[dcSource.ColumnName];
                            //}
                        }
                    }
                    dtDest.Rows.Add(dtNew);

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

        public static DataTable GetDefaultHeaderFiledList()
        {
            try
            {
                DataTable dtDefault = new DataTable();
                dtDefault.Columns.Add("FieldName", typeof(string));

                dtDefault.Rows.Add(CreateNewRow(dtDefault, "DocKey"));
                dtDefault.Rows.Add(CreateNewRow(dtDefault, "DocCodeKey"));
                dtDefault.Rows.Add(CreateNewRow(dtDefault, "DocID"));
                dtDefault.Rows.Add(CreateNewRow(dtDefault, "DocType"));
                dtDefault.Rows.Add(CreateNewRow(dtDefault, "DocTypeNm"));
                dtDefault.Rows.Add(CreateNewRow(dtDefault, "DocSign"));
                dtDefault.Rows.Add(CreateNewRow(dtDefault, "DocQuoteStatus"));
                dtDefault.Rows.Add(CreateNewRow(dtDefault, "DocQuoteReason"));
                dtDefault.Rows.Add(CreateNewRow(dtDefault, "DocQONum"));
                dtDefault.Rows.Add(CreateNewRow(dtDefault, "DocSONum"));
                dtDefault.Rows.Add(CreateNewRow(dtDefault, "DocDONum"));
                dtDefault.Rows.Add(CreateNewRow(dtDefault, "DocIVNum"));
                dtDefault.Rows.Add(CreateNewRow(dtDefault, "DocPONum"));
                dtDefault.Rows.Add(CreateNewRow(dtDefault, "DocCPONum"));
                dtDefault.Rows.Add(CreateNewRow(dtDefault, "DocPDNum"));
                dtDefault.Rows.Add(CreateNewRow(dtDefault, "DocBLNum"));
                dtDefault.Rows.Add(CreateNewRow(dtDefault, "DocState"));
                dtDefault.Rows.Add(CreateNewRow(dtDefault, "DocPrinted"));
                dtDefault.Rows.Add(CreateNewRow(dtDefault, "ApproveUserKey"));
                dtDefault.Rows.Add(CreateNewRow(dtDefault, "ApproveDate"));
                dtDefault.Rows.Add(CreateNewRow(dtDefault, "DisapproveUserKey"));
                dtDefault.Rows.Add(CreateNewRow(dtDefault, "DisapproveDate"));
                dtDefault.Rows.Add(CreateNewRow(dtDefault, "DisapproveCount"));
                dtDefault.Rows.Add(CreateNewRow(dtDefault, "DisapproveMsg"));
                dtDefault.Rows.Add(CreateNewRow(dtDefault, "CreateDate"));
                dtDefault.Rows.Add(CreateNewRow(dtDefault, "CreateUserKey"));
                dtDefault.Rows.Add(CreateNewRow(dtDefault, "LastModifiedDate"));
                dtDefault.Rows.Add(CreateNewRow(dtDefault, "LastModifiedUserKey"));
                dtDefault.Rows.Add(CreateNewRow(dtDefault, "PurgeKeep"));
                dtDefault.Rows.Add(CreateNewRow(dtDefault, "PurgeData"));


                return dtDefault;
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
        public static DataTable GetDefaultDetailFiledList()
        {
            try
            {
                DataTable dtDefault = new DataTable();
                dtDefault.Columns.Add("FieldName", typeof(string));

                dtDefault.Rows.Add(CreateNewRow(dtDefault, "DocKey"));
                dtDefault.Rows.Add(CreateNewRow(dtDefault, "ItmQtyLink"));
                dtDefault.Rows.Add(CreateNewRow(dtDefault, "ItmQtyAdj"));
                dtDefault.Rows.Add(CreateNewRow(dtDefault, "ItmOrderStatus"));
                dtDefault.Rows.Add(CreateNewRow(dtDefault, "ItmBatchKey"));
                dtDefault.Rows.Add(CreateNewRow(dtDefault, "ItmBatchQty"));
                dtDefault.Rows.Add(CreateNewRow(dtDefault, "NSLink"));
                dtDefault.Rows.Add(CreateNewRow(dtDefault, "ARQOID"));
                dtDefault.Rows.Add(CreateNewRow(dtDefault, "ARQODK"));
                dtDefault.Rows.Add(CreateNewRow(dtDefault, "ARQODItm"));
                dtDefault.Rows.Add(CreateNewRow(dtDefault, "ARSOID"));
                dtDefault.Rows.Add(CreateNewRow(dtDefault, "ARSODK"));
                dtDefault.Rows.Add(CreateNewRow(dtDefault, "ARSODItm"));
                dtDefault.Rows.Add(CreateNewRow(dtDefault, "ARSOPOID"));
                dtDefault.Rows.Add(CreateNewRow(dtDefault, "ARDOID"));
                dtDefault.Rows.Add(CreateNewRow(dtDefault, "ARDODK"));
                dtDefault.Rows.Add(CreateNewRow(dtDefault, "ARDODItm"));
                dtDefault.Rows.Add(CreateNewRow(dtDefault, "INCPSID"));
                dtDefault.Rows.Add(CreateNewRow(dtDefault, "CSCPSDK"));
                dtDefault.Rows.Add(CreateNewRow(dtDefault, "CSCPSDItm"));
                dtDefault.Rows.Add(CreateNewRow(dtDefault, "CSCSIID"));
                dtDefault.Rows.Add(CreateNewRow(dtDefault, "CSCSIDK"));
                dtDefault.Rows.Add(CreateNewRow(dtDefault, "CSCSIDItm"));
                dtDefault.Rows.Add(CreateNewRow(dtDefault, "CreateDate"));
                dtDefault.Rows.Add(CreateNewRow(dtDefault, "CreateUserKey"));
                dtDefault.Rows.Add(CreateNewRow(dtDefault, "LastModifiedDate"));
                dtDefault.Rows.Add(CreateNewRow(dtDefault, "LastModifiedUserKey"));

                return dtDefault;
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

        public static DataRow CreateNewRow(DataTable dt, string value)
        {
            try
            {
                DataRow dr = dt.NewRow();
                dr[0] = value;
                return dr;
            }
            catch (Exception ex)
            {
                throw Error(ex);
            }
        }

        public static void TAMerge(ref DataTable dtOriginal, DataTable dtModified)
        {

            DataTable dt = dtOriginal.Copy();

            int rowCount = dt.Rows.Count;
            for (int i = rowCount - 1; i > -1; i--)
            {
                dt.Rows[i].Delete();
            }

            foreach (DataRow row in dtModified.Rows)
            {
                dt.ImportRow(row);
            }

            dtOriginal.Merge(dt);
        }


        public static void BindComboValue(TAUtil.TAComboBox cboSource, int DataGroup, int MsgType)
        {
            try
            {
                DataTable dt;

                if (MsgType == 0)
                {
                    dt = BOLib.SYSList.GetMsgList((GEnum.SYSMsgList)DataGroup);

                    cboSource.DataSource = dt;

                    cboSource.ValueMember = "MsgValue";
                    cboSource.DisplayMember = "DataDes";
                }
                else if (MsgType == 1)
                {
                    dt = BOLib.SYSList.GetMsgListText((GEnum.MsgListTextGrp)DataGroup);
                    cboSource.DataSource = dt;

                    cboSource.ValueMember = "MsgValue";
                    cboSource.DisplayMember = "MsgValue";
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
        /// <summary>
        /// Add Value 0 to combo box             
        /// <param name="cboSource">ComboBox object to which start index type will be added.</param>
        /// </summary>        
        public static void AddComboEmptyValue(Infragistics.Win.UltraWinEditors.UltraComboEditor cboSource)
        {
            try
            {
                if (cboSource.DisplayMember.Trim() == string.Empty ||
                    cboSource.ValueMember.Trim() == string.Empty)
                {
                    MsgBox.Show("System Error! Value and Diplay Member must be set to add empty row in Combobox >>>. " + cboSource.Name);
                    return;
                }
                DataTable dtTemp = (DataTable)cboSource.DataSource;
                DataRow drSelect = dtTemp.NewRow();
                dtTemp.DefaultView.Sort = cboSource.DisplayMember;
                drSelect[cboSource.ValueMember] = 0;
                drSelect[cboSource.DisplayMember] = " ";
                dtTemp.Rows.Add(drSelect);
                cboSource.DataSource = dtTemp;
                cboSource.Refresh();
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
        /// <summary>
        /// Add empty row to combo box with 0 Value 
        /// Give a choice to user to hide empty row or not 
        /// If hideEmptyRow value is false, he user will see empty row at 0 index (top of list)
        /// <param name="cboSource">ComboBox object to which start index type will be added.</param>
        /// <param name="hideEmptyRow">bool value .</param>
        /// </summary>        
        public static void AddComboEmptyValue(TAUtil.TAComboBox cboSource, bool hideEmptyRow)
        {
            try
            {
                if (cboSource.DisplayMember.Trim() == string.Empty ||
                cboSource.ValueMember.Trim() == string.Empty)
                {
                    MsgBox.Show("System Error! Value and Diplay Member must be set to add empty row in Combobox >>>. " + cboSource.Name);
                    return;
                }
                DataTable dtTemp = cboSource.DataSource as DataTable;
                DataRow drSelect = dtTemp.NewRow();
                dtTemp.DefaultView.Sort = cboSource.DisplayMember;

                if (dtTemp.Columns[cboSource.ValueMember].DataType == typeof(string))
                    drSelect[cboSource.ValueMember] = string.Empty;
                else
                    drSelect[cboSource.ValueMember] = 0;

                drSelect[cboSource.DisplayMember] = " ";
                dtTemp.Rows.Add(drSelect);
                cboSource.DataSource = dtTemp;
                cboSource.Rows[0].Hidden = hideEmptyRow;
                cboSource.Refresh();
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

        public static TAUtil.TATextBoxEditor GetTextBox(System.Windows.Forms.Form frmOwnerForm, string NameofTextBox)
        {
            try
            {
                System.Windows.Forms.Control[] ctrls = frmOwnerForm.Controls.Find(NameofTextBox, true);
                if (ctrls != null)
                {
                    if (ctrls.Length > 0)
                        if (ctrls[0].GetType() == typeof(TAUtil.TATextBoxEditor))
                            return ctrls[0] as TAUtil.TATextBoxEditor;
                }
                else
                {
                    MsgBox.Show(frmOwnerForm.Name + " Form does not contain TAUtil.TATextBoxEditor \"" + NameofTextBox + "\".");
                }
                return null;
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
        public static TAUtil.TAComboBox GetComboBox(System.Windows.Forms.Form frmOwnerForm, string NameofCombo)
        {
            try
            {
                System.Windows.Forms.Control[] ctrls = frmOwnerForm.Controls.Find(NameofCombo, true);
                if (ctrls != null)
                {
                    if (ctrls.Length > 0)
                        if (ctrls[0].GetType() == typeof(TAUtil.TAComboBox))
                            return ctrls[0] as TAUtil.TAComboBox;
                }
                else
                {
                    MsgBox.Show(frmOwnerForm.Name + " Form does not contain TAUtil.TAComboBox \"" + NameofCombo + "\".");
                }
                return null;
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
        public static TAUtil.TAGridEditor GetGrid(System.Windows.Forms.Form frmOwnerForm, string NameofGrid)
        {
            try
            {
                System.Windows.Forms.Control[] ctrls = frmOwnerForm.Controls.Find(NameofGrid, true);
                if (ctrls != null)
                {
                    if (ctrls.Length > 0)
                        if (ctrls[0].GetType() == typeof(TAUtil.TAComboBox))
                            return ctrls[0] as TAUtil.TAGridEditor;
                }
                else
                {
                    MsgBox.Show(frmOwnerForm.Name + " Form does not contain TAUtil.TAGridEditor \"" + NameofGrid + "\".");
                }
                return null;
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
        public static Infragistics.Win.Misc.UltraLabel GetLabel(System.Windows.Forms.Form frmOwnerForm, string NameofLabel)
        {
            try
            {
                System.Windows.Forms.Control[] ctrls = frmOwnerForm.Controls.Find(NameofLabel, true);
                if (ctrls != null)
                {
                    if (ctrls.Length > 0)
                        if (ctrls[0].GetType() == typeof(Infragistics.Win.Misc.UltraLabel))
                            return ctrls[0] as Infragistics.Win.Misc.UltraLabel;
                }
                else
                {
                    MsgBox.Show(frmOwnerForm.Name + " Form does not contain TAUtil.TAGridEditor \"" + NameofLabel + "\".");
                }
                return null;
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
        public static TAUtil.TANumericEditor GetNumericTextBox(System.Windows.Forms.Form frmOwnerForm, string NameofLabel)
        {
            try
            {
                System.Windows.Forms.Control[] ctrls = frmOwnerForm.Controls.Find(NameofLabel, true);
                if (ctrls != null)
                {
                    if (ctrls.Length > 0)
                        if (ctrls[0].GetType() == typeof(TAUtil.TANumericEditor))
                            return ctrls[0] as TAUtil.TANumericEditor;
                }
                else
                {
                    MsgBox.Show(frmOwnerForm.Name + " Form does not contain TAUtil.TANumericEditor \"" + NameofLabel + "\".");
                }
                return null;
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
        public static TAUtil.TADateEditor GetDateEditor(System.Windows.Forms.Form frmOwnerForm, string NameofDateEditor)
        {
            try
            {
                System.Windows.Forms.Control[] ctrls = frmOwnerForm.Controls.Find(NameofDateEditor, true);
                if (ctrls != null)
                {
                    if (ctrls.Length > 0)
                        if (ctrls[0].GetType() == typeof(TAUtil.TADateEditor))
                            return ctrls[0] as TAUtil.TADateEditor;
                }

                else
                {
                    MsgBox.Show(frmOwnerForm.Name + " Form does not contain TAUtil.TANumericEditor \"" + NameofDateEditor + "\".");

                }
                return null;
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

        public static void SetDefaultValueAtDataTable(DataTable dt)
        {
            try
            {
                foreach (DataRow dr in dt.Rows)
                {
                    foreach (DataColumn col in dt.Columns)
                    {
                        if (dr[col] == DBNull.Value)
                        {
                            if (col.DataType == typeof(DateTime))
                            {

                                dr[col.ColumnName] = DateTime.Parse("01/01/1900");

                            }
                            else
                            {
                                if (col.DataType == typeof(string))
                                {
                                    dr[col.ColumnName] = "";
                                }
                                else
                                {
                                    dr[col.ColumnName] = 0;
                                }
                            }
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
        }
        public static void SetDataTableColumnDefaultValue(DataTable dt)
        {
            try
            {

                foreach (DataColumn col in dt.Columns)
                {

                    if (col.DataType == typeof(DateTime))
                    {

                        col.DefaultValue = DateTime.Today.Date;

                    }
                    else
                    {
                        if (col.DataType == typeof(string))
                        {
                            col.DefaultValue = "";
                        }
                        else
                        {
                            col.DefaultValue = 0;
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
        }

        public static void SetRowError(DataRow ErrorDataRow, string ErrorMessage)
        {
            if (ErrorDataRow.Table.Columns.Contains("LineLinkKey"))
            {
                if (GFunc.NEInt(ErrorDataRow["LineLinkKey"], 0) == 0)
                {
                    ErrorDataRow.RowError = ErrorMessage;
                }
                else
                {
                    DataRow[] ParentRows = ErrorDataRow.Table.Select("DocItmKey=" + GFunc.NEInt(ErrorDataRow["LineLinkKey"], 0));
                    if (ParentRows != null)
                    {
                        ParentRows[0].RowError = ErrorMessage;
                    }
                }
            }
            else
            {
                ErrorDataRow.RowError = ErrorMessage;
            }
        }
        // This is called from Popupform and the form itself also for frmPopup parameter list purpose
        public static string[] AddAccessLevelParam(string[] parmList, string lstID)
        {
            try
            {
                List<string> paramList = parmList.ToList();

                //case GVar.ListSettingID.CustVendBrowse:
                //case GVar.ListSettingID.CustomerBrowse:
                //    paramList.Add(AppInfor.CurrentUserKey.ToString());
                //    paramList.Add(AppInfor.ConAccessLevel.ToString());
                //    break;
                //case GVar.ListSettingID.AccBrowse:
                //case GVar.ListSettingID.AccBrowseByCurr:
                //    break;
                //case GVar.ListSettingID.ItemBrowse:
                //case GVar.ListSettingID.ItemBrowseExcludeItemType:
                //case GVar.ListSettingID.ItemBrowseByIDAnyPart:
                //case GVar.ListSettingID.ItemStockBrowse:
                //case GVar.ListSettingID.ItemStkNStkBrowse:
                //case GVar.ListSettingID.ItemConsignBrowse:
                //case GVar.ListSettingID.ItemFGBrowse:
                //case GVar.ListSettingID.ItemStockBrowseByTypeForID:
                //case GVar.ListSettingID.ItemStockBrowseByTypeForDesc:
                if (GFunc.CompareString(GVar.ListSettingID.MSTItmC_des, lstID) ||
                GFunc.CompareString(GVar.ListSettingID.MSTItmC_id, lstID) ||
                GFunc.CompareString(GVar.ListSettingID.MSTItmF_des, lstID) ||
                GFunc.CompareString(GVar.ListSettingID.MSTItmF_id, lstID) ||
                GFunc.CompareString(GVar.ListSettingID.MSTItmFS_des, lstID) ||
                GFunc.CompareString(GVar.ListSettingID.MSTItmFS_id, lstID) ||
                GFunc.CompareString(GVar.ListSettingID.MSTItmFSC_des, lstID) ||
                GFunc.CompareString(GVar.ListSettingID.MSTItmFSC_id, lstID) ||
                GFunc.CompareString(GVar.ListSettingID.MSTItmFSCAN_des, lstID) ||
                GFunc.CompareString(GVar.ListSettingID.MSTItmFSCAN_id, lstID) ||
                GFunc.CompareString(GVar.ListSettingID.MSTItmFSCANVG_des, lstID) ||
                GFunc.CompareString(GVar.ListSettingID.MSTItmFSCANVG_id, lstID) ||
                GFunc.CompareString(GVar.ListSettingID.MSTItmFSCN_des, lstID) ||
                GFunc.CompareString(GVar.ListSettingID.MSTItmFSCN_id, lstID) ||
                GFunc.CompareString(GVar.ListSettingID.MSTItmFSN_des, lstID) ||
                GFunc.CompareString(GVar.ListSettingID.MSTItmFSN_id, lstID) ||
                GFunc.CompareString(GVar.ListSettingID.MSTItmFSNVG_des, lstID) ||
                GFunc.CompareString(GVar.ListSettingID.MSTItmFSNVG_id, lstID) ||
                GFunc.CompareString(GVar.ListSettingID.MSTItmG_des, lstID) ||
                GFunc.CompareString(GVar.ListSettingID.MSTItmG_id, lstID) ||
                GFunc.CompareString(GVar.ListSettingID.MSTItmJob_des, lstID) ||
                GFunc.CompareString(GVar.ListSettingID.MSTItmJob_id, lstID) ||
                GFunc.CompareString(GVar.ListSettingID.MSTItmM_des, lstID) ||
                GFunc.CompareString(GVar.ListSettingID.MSTItmM_id, lstID) ||
                GFunc.CompareString(GVar.ListSettingID.MSTItmN_des, lstID) ||
                GFunc.CompareString(GVar.ListSettingID.MSTItmN_id, lstID) ||
                GFunc.CompareString(GVar.ListSettingID.MSTItmPack_des, lstID) ||
                GFunc.CompareString(GVar.ListSettingID.MSTItmPack_id, lstID) ||
                GFunc.CompareString(GVar.ListSettingID.MSTItmPurchase_des, lstID) ||
                GFunc.CompareString(GVar.ListSettingID.MSTItmPurchase_id, lstID) ||
                GFunc.CompareString(GVar.ListSettingID.MSTItmSales_des, lstID) ||
                GFunc.CompareString(GVar.ListSettingID.MSTItmSales_id, lstID) ||
                GFunc.CompareString(GVar.ListSettingID.MSTItmType_des, lstID) ||
                GFunc.CompareString(GVar.ListSettingID.MSTItmType_id, lstID) ||
                GFunc.CompareString(GVar.ListSettingID.MSTItmV_des, lstID) ||
                GFunc.CompareString(GVar.ListSettingID.MSTItmV_id, lstID))
                {
                    paramList.Add(AppInfor.CurrentUserKey.ToString());
                    paramList.Add(AppInfor.ConAccessLevel.ToString());
                }
                //case GVar.ListSettingID.JobBrowse:
                //case GVar.ListSettingID.JobList:
                else if (GFunc.CompareString(GVar.ListSettingID.MSTJobAll_id, lstID) ||
                GFunc.CompareString(GVar.ListSettingID.MSTJobSales_id, lstID))
                {
                    paramList.Add(AppInfor.CurrentUserKey.ToString());
                    paramList.Add(AppInfor.ConAccessLevel.ToString());
                }
                //case GVar.ListSettingID.JobDetBrowseSaleByIDAnyPart:
                //case GVar.ListSettingID.JobDetBrowsePurchByIDAnyPart:
                //    paramList.Add(AppInfor.CurrentUserKey.ToString());
                //    paramList.Add(AppInfor.ConAccessLevel.ToString());
                //    break;
                else if (GFunc.CompareString(GVar.ListSettingID.MSTConAll_des, lstID) ||
                GFunc.CompareString(GVar.ListSettingID.MSTConAll_id, lstID) ||
                GFunc.CompareString(GVar.ListSettingID.MSTConBothCash_des, lstID) ||
                GFunc.CompareString(GVar.ListSettingID.MSTConBothCash_id, lstID) ||
                GFunc.CompareString(GVar.ListSettingID.MSTConBothCredit_des, lstID) ||
                GFunc.CompareString(GVar.ListSettingID.MSTConBothCredit_id, lstID) ||
                GFunc.CompareString(GVar.ListSettingID.MSTConPurchase_des, lstID) ||
                GFunc.CompareString(GVar.ListSettingID.MSTConPurchase_id, lstID) ||
                GFunc.CompareString(GVar.ListSettingID.MSTConSales_des, lstID) ||
                GFunc.CompareString(GVar.ListSettingID.MSTConSales_id, lstID) ||
                GFunc.CompareString(GVar.ListSettingID.MSTConSalesAll_des, lstID) ||
                GFunc.CompareString(GVar.ListSettingID.MSTConSalesAll_id, lstID) ||
                GFunc.CompareString(GVar.ListSettingID.MSTConSalesCash_des, lstID) ||
                GFunc.CompareString(GVar.ListSettingID.MSTConSalesCash_id, lstID) ||
                GFunc.CompareString(GVar.ListSettingID.MSTConSalesCredit_des, lstID) ||
                GFunc.CompareString(GVar.ListSettingID.MSTConSalesCredit_id, lstID) ||
                GFunc.CompareString(GVar.ListSettingID.MSTVehicle_des, lstID))
                {
                    //case GVar.ListSettingID.VendorBrowseByDesAnyPart:
                    paramList.Add(AppInfor.CurrentUserKey.ToString());
                    paramList.Add(AppInfor.ConAccessLevel.ToString());
                }
                else
                {
                    paramList.Add(AppInfor.CurrentUserKey.ToString());
                    paramList.Add(AppInfor.ConAccessLevel.ToString());
                }
                return paramList.ToArray();
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


        public static bool AlertCheck(GEnum.AlertApplyTo alertApplyTo, object alertInfo)
        {
            try
            {
                DateTime alertDateTime = DateTime.Now;

                #region Sql Parameters
                List<SqlParameter> parmList = new List<SqlParameter>();
                parmList.Add(new SqlParameter("@Option", 3));
                parmList.Add(new SqlParameter("@AlertApplyTo", (int)alertApplyTo));
                parmList.Add(new SqlParameter("@RetValue", (int)0));
                parmList[2].Direction = ParameterDirection.Output;
                #endregion

                DataTable dtAlert = GFunc.ExecuteProc("TASAlert_Get", parmList);
                string alertDetMsg = "";
                if (dtAlert.Rows.Count > 0)
                {
                    switch (alertApplyTo)
                    {
                        case GEnum.AlertApplyTo.Wrongpasswordlockouts:
                            if (alertInfo.GetType() == typeof(SECUser))
                            {
                                SECUser objUser = (SECUser)alertInfo;
                                alertDetMsg = "User ID=" + objUser.UserID + ", Name=" + objUser.UserName + "'s account has been locked out on " + alertDateTime.ToString("dd MMM yyyy h:mm tt") + ".";
                            }
                            break;
                    }

                    foreach (DataRow dr in dtAlert.Rows)
                    {
                        #region Sql Parameters
                        parmList = new List<SqlParameter>();
                        parmList.Add(new SqlParameter("@Option", 1));
                        parmList.Add(new SqlParameter("@AlertKey", dr["AlertKey"]));
                        parmList.Add(new SqlParameter("@RetValue", (int)0));
                        parmList[2].Direction = ParameterDirection.Output;
                        #endregion

                        DataTable dtAlertUsers = GFunc.ExecuteProc("TASAlertDetSub_Get", parmList);

                        dtAlertUsers.DefaultView.RowFilter = "AlertType=" + (int)GEnum.AlertType.Email;

                        foreach (DataRowView user in dtAlertUsers.DefaultView)
                        {
                            string userEmailBody = GFunc.NEStr(user["EmailStandardMessage"], "") + " : " + alertDetMsg;
                            GEmail.SendSystemEmail(GFunc.NEStr(user["Email"], ""), GFunc.NEStr(user["EmailSubject"], ""), userEmailBody, null);

                            #region Updating to Alert Detail Sub
                            parmList = new List<SqlParameter>();
                            parmList.Add(new SqlParameter("@Option", 2));
                            parmList.Add(new SqlParameter("@AlertKey", dr["AlertKey"]));
                            parmList.Add(new SqlParameter("@UserKey", user["AlertKey"]));
                            parmList.Add(new SqlParameter("@AlertType", (int)GEnum.AlertType.Email));
                            parmList.Add(new SqlParameter("@AlertStatus", GEnum.AlertStatus.Alert));
                            parmList.Add(new SqlParameter("@LastAlertDate", alertDateTime));
                            parmList.Add(new SqlParameter("@DateRead", DBNull.Value));

                            SqlParameter pRetValue = new SqlParameter("@RetValue", (int)0);
                            pRetValue.Direction = ParameterDirection.Output;
                            parmList.Add(pRetValue);

                            GFunc.ExecuteNonQueryProc("TASAlertDetSub_AddUpdate", parmList);
                            #endregion
                        }
                    }
                }
                return true;
            }
            catch (Exception ex)
            {
                Error(ex);
            }
            return false;
        }

        ////Report Related Functions
        //public static bool SendEmail(string emails, string subject, string messageBody, System.Net.Mail.Attachment Attachment)
        //{
        //    try
        //    {
        //        string[] emailAddress = emails.Split(';');
        //        string AuthEmailAcc = SysOptionUtility.GetStr("EmailAccount");
        //        string oMSever = SysOptionUtility.GetStr("OutgoingMailServer");
        //        string iMSever = SysOptionUtility.GetStr("IncomingMailServer");

        //        //Prepering Mail               
        //        System.Net.Mail.MailMessage email = new System.Net.Mail.MailMessage();
        //        for (int i = 0; i < emailAddress.Length; i++)
        //            if (Regex.IsMatch(emailAddress[i], GVar.EmailCheck))
        //            {
        //                email.To.Add(emailAddress[i]);
        //            }
        //            else
        //            {
        //                if (emailAddress[i].Trim() != string.Empty)
        //                {
        //                    MsgBox.Show("The system cannot send to incorrect email address ->" + emailAddress[i] + ". The other valid mails will be sent out.");
        //                }
        //            }

        //        email.From = new System.Net.Mail.MailAddress(SysOptionUtility.GetStr("ReplyTo"), "TECHACE Agent Service");

        //        //Preparing Mail For BCC                 
        //        if (Regex.IsMatch(AuthEmailAcc, GVar.EmailCheck))
        //        {
        //            if (SysOptionUtility.GetBool("BCC"))
        //            { 
        //                email.Bcc.Add(new System.Net.Mail.MailAddress(AuthEmailAcc, "TECHACE Agent Service"));
        //            }
        //        }
        //        else 
        //        {
        //            if (AuthEmailAcc.Trim() != string.Empty)
        //            {
        //                MsgBox.Show("The system cannot send to incorrect BCC email address ->" + AuthEmailAcc + ".");
        //                return false;
        //            }
        //        }
        //        if (oMSever.Trim() == string.Empty)
        //        {
        //            MsgBox.Show("Invalid Outgoing Mail Server.");
        //            return false;
        //        }             

        //        email.Subject = subject;
        //        email.Body = messageBody;
        //        email.IsBodyHtml = true;

        //        if (Attachment != null)
        //            email.Attachments.Add(Attachment);

        //        //Sending Mail
        //        System.Net.Mail.SmtpClient client = new System.Net.Mail.SmtpClient(oMSever, SysOptionUtility.GetInt("OutgoingMailServerPortNumber"));
        //        client.EnableSsl = true;
        //        client.UseDefaultCredentials = false;

        //        client.Credentials = new System.Net.NetworkCredential(AuthEmailAcc,TAUtil.Decoder.Decode(SysOptionUtility.GetStr("EmailPassword")));
        //        System.Net.ServicePointManager.ServerCertificateValidationCallback = delegate(object s, System.Security.Cryptography.X509Certificates.X509Certificate certificate,
        //            System.Security.Cryptography.X509Certificates.X509Chain chain, System.Net.Security.SslPolicyErrors sslPolicyErrors) 
        //            { return true; };


        //        client.Send(email);

        //        return true;
        //    }
        //    catch (TAException tex)
        //    {
        //        throw tex;
        //    }

        //    catch (Exception ex)
        //    { 
        //        throw ex;
        //    }          
        //} //Not Ready

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

        public static DateTime GetSvrDateTime()
        {
            try
            {
                DataTable dt = ExecuteQuery("Select GetDate()");
                if (IsNE(dt) == false && dt.Rows.Count > 0)
                    return NEDateTime(dt.Rows[0][0], DateTime.Now);
                else
                    return DateTime.Now;
            }
            catch (Exception ex)
            {
                throw Error(ex);
            }
        }

        public static DateTime GetSvrDateTime(SqlConnection cn)
        {
            try
            {
                DataTable dt = null;
                dt = ExecuteQuery(cn, "Select GetDate()");
                if (dt != null && dt.Rows.Count > 0)
                    return NEDateTime(dt.Rows[0][0], DateTime.Now);
                else
                    return DateTime.Now;
            }
            catch (Exception ex)
            {
                throw Error(ex);
            }
        }

        public static bool CheckKeyDependcyinOptionTable(SqlConnection cn, string Name, int KeyValue)
        {
            bool retValue = true;
            string msgID = MsgID.Common.GetFail;
            string a = KeyValue.ToString();
            try
            {
                // Using existing sql connection.
                using (SqlCommand cm = cn.CreateCommand())
                {
                    cm.CommandType = CommandType.StoredProcedure;
                    cm.CommandText = "CheckKey_BeforeDeleteForOption";

                    cm.Parameters.Add("@MsgID", SqlDbType.NVarChar, 500);
                    cm.Parameters["@MsgID"].Value = msgID;
                    cm.Parameters.AddWithValue("@Name", Name);
                    cm.Parameters.AddWithValue("@KeyValue", a);
                    cm.Parameters.AddWithValue("@RetValue", 0);
                    cm.Parameters["@MsgID"].Direction = ParameterDirection.InputOutput;

                    cm.Parameters["@RetValue"].Direction = ParameterDirection.Output;

                    cm.ExecuteNonQuery();

                    if (cm.Parameters["@MsgID"].Value == null)
                        msgID = string.Empty;
                    else
                        msgID = cm.Parameters["@MsgID"].Value.ToString();

                    if ((int)cm.Parameters["@RetValue"].Value == (int)GEnum.SpState.Pass)
                        retValue = false;
                    else
                        MsgBox.Show(cn, msgID);

                }// Already close and dispose sql connection.


            }
            catch (TAException tex)
            {
                throw Error(tex);
            }
            catch (Exception ex)
            {
                throw Error(ex);
            }

            return retValue;
        }
        public static bool GetFormSetting(string lstID, ref string[] colHeaders, ref string[] colWidths)
        {
            // Initialize output 


            SYSFormSettingID objSysFormSettingID = SYSFormSettingID.Get(lstID);

            if (!GFunc.IsNE(objSysFormSettingID))
            {
                colHeaders = objSysFormSettingID._colHeader.Split(',');
                colWidths = objSysFormSettingID._colWidth.Split(',');
            }
            else
                return false;

            return true;
        }
        /// <summary>
        /// Used by frmDocCopy and frmImportSelection.
        /// </summary>
        /// <param name="dtExcelData"></param>
        /// <param name="CodeKey"></param>
        /// <returns></returns>
        public static bool ValidateExcelData(DataTable dtExcelData, int CodeKey)
        {
            /* modified by YST on 2023/07/16 to validate ColHeader & match with ColName because ColHeader is user-friendly, ColName is used for sql table column */
            //DataTable dtImportStructure = GFunc.ExecuteQuery("Select * from SyS_DocItem_ImportStructure(nolock) Where Required = 1 and ImportType =" + CodeKey);
            DataTable dtImportColumns = GFunc.ExecuteQuery("Select * from SyS_DocItem_ImportStructure(nolock) Where ImportType =" + CodeKey);
            DataTable dtImportStructure = dtImportColumns.Select("Required = 1").CopyToDataTable();

            string errorMsg = "Can't Import.Please check row - ";

            foreach (DataColumn c in dtExcelData.Columns)
            {
                c.ColumnName = c.ColumnName.Trim();
                if (dtImportColumns.Columns.Contains("ColHeader"))
                {
                    foreach (DataRow dr in dtImportColumns.Rows)
                    {
                        if (GFunc.NEStr(dr["ColHeader"].ToString(), string.Empty) != "")
                        {
                            if (c.ColumnName == dr["ColHeader"].ToString())
                                c.ColumnName = dr["ColName"].ToString();
                        }
                    }
                }
            }
            //check column
            foreach (DataRow dr in dtImportStructure.Rows)
            {
                if (!dtExcelData.Columns.Contains(dr["ColName"].ToString()))
                {                    
                    MsgBox.Show("ERROR:  Column -\"" + dr["ColName"].ToString() + "\" is require to import.<br/>Please check ColHeader and ColName in SyS_DocItem_ImportStructure.");
                    return false;
                }
            }

            foreach (DataRow dr in dtImportStructure.Rows)
            {
                decimal val = 0;
                DateTime dt = new DateTime();
                //check datatype
                switch (dr["ColDataType"].ToString())
                {
                    case "Number":
                        if (dtExcelData.Columns.Contains(dr["ColName"].ToString()))
                        {
                            for (int i = 0; i < dtExcelData.Rows.Count; i++)
                            {
                                if ((bool)dr["Required"])
                                {
                                    if (GFunc.IsNE(dtExcelData.Rows[i][dr["ColName"].ToString()]))
                                    {
                                        if (GFunc.IsNE(dr["DefaultVal"]) == false)
                                        {
                                            dtExcelData.Rows[i][dr["ColName"].ToString()] = dr["DefaultVal"];
                                        }
                                        else
                                        {
                                            MsgBox.Show(errorMsg + (i + 2).ToString() + ", Column -\"" + dr["ColName"].ToString() + "\" missing value.");
                                            return false;
                                        }
                                    }
                                    else
                                    {
                                        if (!Decimal.TryParse((dtExcelData.Rows[i][dr["ColName"].ToString()].ToString()), out val))
                                        {
                                            MsgBox.Show(errorMsg + (i + 2).ToString() + ", Column -\"" + dr["ColName"].ToString() + "\" contains invalid Numeric value.");
                                            return false;
                                        }
                                    }

                                }
                                else
                                {
                                    if (GFunc.IsNE(dtExcelData.Rows[i][dr["ColName"].ToString()]) == false)
                                    {
                                        if (!Decimal.TryParse((dtExcelData.Rows[i][dr["ColName"].ToString()].ToString()), out val))
                                        {
                                            MsgBox.Show(errorMsg + (i + 2).ToString() + ", Column -\"" + dr["ColName"].ToString() + "\" contains invalid Numeric value.");
                                            return false;
                                        }
                                    }
                                }
                            }
                        }
                        break;
                    case "DateTime":
                        if (dtExcelData.Columns.Contains(dr["ColName"].ToString()))
                        {
                            for (int i = 0; i < dtExcelData.Rows.Count; i++)
                            {
                                if ((bool)dr["Required"])
                                {
                                    if (GFunc.IsNE(dtExcelData.Rows[i][dr["ColName"].ToString()]))
                                    {
                                        if (GFunc.IsNE(dr["DefaultVal"]) == false)
                                        {
                                            dtExcelData.Rows[i][dr["ColName"].ToString()] = dr["DefaultVal"];
                                        }
                                        else
                                        {
                                            MsgBox.Show(errorMsg + (i + 2).ToString() + ", Column -\"" + dr["ColName"].ToString() + "\" missing value.");
                                            return false;
                                        }
                                    }
                                    else
                                    {
                                        if (!DateTime.TryParse(dtExcelData.Rows[i][dr["ColName"].ToString()].ToString(), out dt))
                                        {
                                            MsgBox.Show(errorMsg + (i + 2).ToString() + ", Column -\"" + dr["ColName"].ToString() + "\" contains invalid Date value.");
                                            return false;
                                        }
                                    }
                                }
                                else
                                {
                                    if (GFunc.IsNE(dtExcelData.Rows[i][dr["ColName"].ToString()]) == false)
                                    {
                                        if (!DateTime.TryParse(dtExcelData.Rows[i][dr["ColName"].ToString()].ToString(), out dt))
                                        {
                                            MsgBox.Show(errorMsg + (i + 2).ToString() + ", Column -\"" + dr["ColName"].ToString() + "\" contains invalid Date value.");
                                            return false;
                                        }
                                    }
                                }
                            }
                        }
                        break;

                    default://Text
                        for (int i = 0; i < dtExcelData.Rows.Count; i++)
                        {

                            if ((bool)dr["Required"])
                            {
                                if (GFunc.IsNE(dtExcelData.Rows[i][dr["ColName"].ToString()]))
                                {
                                    if (GFunc.IsNE(dr["DefaultVal"]) == false)
                                    {
                                        dtExcelData.Rows[i][dr["ColName"].ToString()] = dr["DefaultVal"];
                                    }
                                    else
                                    {
                                        MsgBox.Show(errorMsg + (i + 2).ToString() + ", Column -\"" + dr["ColName"].ToString() + "\" missing value.");
                                        return false;
                                    }
                                }
                            }
                        }
                        break;
                }
            }

            return true;
        }//Completed
        public static bool ValidateUserAccess(DataTable dtExcelData, int CodeKey)
        {
            string itmTypes = GFunc.GetItmTypeGyDocCode(CodeKey);
            string errorMsg = "Can't Import. Please check Row - ";
            string RowErrMsg = errorMsg;
            bool ok = true;

            if (CodeKey == (int)GEnum.SystemCode.Job) //Added by May
            {
                for (int i = 0; i < dtExcelData.Rows.Count; i++)
                {
                    RowErrMsg = errorMsg;
                    if (GFunc.NEInt(dtExcelData.Rows[i]["ItmAccessLevel"].ToString(), 0) > AppInfor.ItemAccessLevel)
                    {
                        RowErrMsg = (i + 1).ToString() + ", Column ItmID  ,  Value =\"" + dtExcelData.Rows[i]["ItmID"] + "\". Your access level is less than this item access level.";
                        ok = false;
                    }
                    if (!GFunc.NEBool(dtExcelData.Rows[i]["InAccessGroup"].ToString(), false))
                    {
                        RowErrMsg += (i + 1).ToString() + ", Column ItmID  ,  Value =\"" + dtExcelData.Rows[i]["ItmID"] + "\". Your access group not match with this item access group.";

                        ok = false;
                    }
                    if (!itmTypes.Contains(dtExcelData.Rows[i]["EstItmType"].ToString()))
                    {
                        RowErrMsg += (i + 1).ToString() + ", Column ItmID ,  Value =\" " + dtExcelData.Rows[i]["ItmID"] + "\". This Item Type does not allow to import.";
                        ok = false;
                    }
                    if (GFunc.NEInt(dtExcelData.Rows[i]["JobPhaseKey"], 0) == 0 && !GFunc.IsNE(dtExcelData.Rows[i]["PhaseID"]))
                    {
                        RowErrMsg += (i + 1).ToString() + ", Column Phase ID,  Value =\"" + dtExcelData.Rows[i]["PhaseID"] + "\". Invalid Phase ID.";
                        ok = false;
                    }
                    if (GFunc.NEInt(dtExcelData.Rows[i]["JobTaskKey"], 0) == 0 && !GFunc.IsNE(dtExcelData.Rows[i]["TaskID"]))
                    {
                        RowErrMsg += (i + 1).ToString() + ", Column Task ID,  Value =\"" + dtExcelData.Rows[i]["TaskID"] + "\". Invalid Task ID.";
                        ok = false;
                    }
                    if (GFunc.NEInt(dtExcelData.Rows[i]["JobCostTypeKey"], 0) == 0 && !GFunc.IsNE(dtExcelData.Rows[i]["CostTypeID"]))
                    {
                        RowErrMsg += (i + 1).ToString() + ", Column Cost Type ID,  Value =\" " + dtExcelData.Rows[i]["CostTypeID"] + "\". Invalid Cost Type ID.";
                        ok = false;
                    }
                    if (GFunc.NEInt(dtExcelData.Rows[i]["DocVendorKey"], 0) == 0 && !GFunc.IsNE(dtExcelData.Rows[i]["VendorID"]))
                    {
                        RowErrMsg += (i + 1).ToString() + ", Column Vendor ID,  Value =\" " + dtExcelData.Rows[i]["VendorID"] + "\". Invalid Vendor ID.";
                        ok = false;
                    }

                    if (!ok)
                    {
                        dtExcelData.Rows[i].RowError = RowErrMsg;
                        MsgBox.Show(RowErrMsg);
                        return false;
                    }
                }
            }
            else
            {
                for (int i = 0; i < dtExcelData.Rows.Count; i++)
                {
                    if (GFunc.NEInt(dtExcelData.Rows[i]["ItmAccessLevel"].ToString(), 0) > AppInfor.ItemAccessLevel)
                    {
                        MsgBox.Show(errorMsg + (i + 1).ToString() + ", Column ItmID  ,  Value =\"" + dtExcelData.Rows[i]["ItmID"] + "\". Your access level is less than this item access level.");
                        return false;
                    }

                    if (!GFunc.NEBool(dtExcelData.Rows[i]["InAccessGroup"].ToString(), false))
                    {
                        MsgBox.Show(errorMsg + (i + 1).ToString() + ", Column ItmID  ,  Value =\"" + dtExcelData.Rows[i]["ItmID"] + "\". Your access group not match with this item access group.");
                        return false;
                    }
                    if (!itmTypes.Contains(dtExcelData.Rows[i]["ItmType"].ToString()))
                    {
                        MsgBox.Show(errorMsg + (i + 1).ToString() + ", Column ItmID ,  Value =\" " + dtExcelData.Rows[i]["ItmID"] + "\". This Item Type does not allow to import.");
                        return false;
                    }
                }
            }
            return true;
        }

        public static string GridColumnKey_Get(object obj)
        {
            if (obj.GetType() == typeof(TAUtil.TAGridEditor))
            {
                if (((TAUtil.TAGridEditor)obj).ActiveCell != null)
                    return ((TAUtil.TAGridEditor)obj).ActiveCell.Column.Key;
            }
            return string.Empty;
        }
        public static object GetValue(DataRow drSource, DataRow drTarget, string ColName, string DataType)
        {
            object ReturnValue = null;


            if (drSource.Table.Columns.Contains(ColName))
            {
                ReturnValue = drSource[ColName];
            }
            else
                switch (DataType.Trim())
                {
                    case "datetime":
                        ReturnValue = DateTime.Today.Date;
                        break;
                    case "bit":
                        ReturnValue = false;
                        break;
                    case "money":
                    case "int":
                    case "decimal(18,8)":
                        if (ColName.ToLower() == "itmcurrkey" || ColName.ToLower() == "itmcurrrate")
                            ReturnValue = 1;
                        else
                            ReturnValue = 0;
                        break;
                    default:
                        ReturnValue = string.Empty;
                        break;
                }


            return ReturnValue;
        }

        public static T TACopyDataTable<T>(T SourceTable)
        {
            DataTable dtOrginal = SourceTable as DataTable;
            DataTable dtNew = null;
            dtNew = dtOrginal.Copy();
            dtNew.DefaultView.RowFilter = dtOrginal.DefaultView.RowFilter.ToString();
            return (T)Convert.ChangeType(dtNew, typeof(T)); ;
        }

        //For Financial Statement
        public static Infragistics.Win.HAlign Alignment_Get(string AlignmentText)
        {
            try
            {
                switch (AlignmentText.ToLower())
                {
                    case "right":
                    case "r":
                        return Infragistics.Win.HAlign.Right;

                    case "center":
                    case "c":
                        return Infragistics.Win.HAlign.Center;

                    default://default Left
                        return Infragistics.Win.HAlign.Left;
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
        public static string FontInfo_Get(Font font)
        {
            string fontInfo = string.Empty;

            try
            {
                TypeConverter converter = TypeDescriptor.GetConverter(typeof(Font)); // Saving Font object as a string 
                fontInfo = converter.ConvertToString(font); // Load an instance of Font from a string                             
            }
            catch (TAException tex)
            {
                throw Error(tex);
            }
            catch (Exception ex)
            {
                throw Error(ex);
            }
            return fontInfo;
        }//Completed
        public static Font FontInfo_Set(string fontInfo)
        {
            Font font = null;

            try
            {
                TypeConverter converter = TypeDescriptor.GetConverter(typeof(Font));
                font = (Font)converter.ConvertFromString(fontInfo);
            }
            catch (TAException tex)
            {
                throw Error(tex);
            }
            catch (Exception ex)
            {
                throw Error(ex);
            }
            return font;
        }//Completed        
    }

    static class Extensions
    {
        public static DataTable AsDataTable<T>(this IEnumerable<T> enumberable)
        {
            try
            {
                DataTable table = new DataTable("Generated");

                T first = enumberable.FirstOrDefault();
                if (first == null)
                    return table;

                PropertyInfo[] properties = first.GetType().GetProperties();
                foreach (PropertyInfo pi in properties)
                    table.Columns.Add(pi.Name, System.Nullable.GetUnderlyingType(pi.PropertyType) ?? pi.PropertyType);//Modified by May on 25-Apr-2011 to accept Nullable type as original type
                //table.Columns.Add(pi.Name,  pi.PropertyType);

                foreach (T t in enumberable)
                {
                    DataRow row = table.NewRow();
                    foreach (PropertyInfo pi in properties)
                        row[pi.Name] = pi.GetValue(t, null) ?? DBNull.Value; //Modified by May on 25-Apr-2011 to convert Nullable type to DBNull
                    //row[pi.Name] = t.GetType().InvokeMember(pi.Name, BindingFlags.GetProperty, null, t, null);
                    table.Rows.Add(row);
                }

                return table;
            }
            catch (Exception ex)
            {
                throw (ex);
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
