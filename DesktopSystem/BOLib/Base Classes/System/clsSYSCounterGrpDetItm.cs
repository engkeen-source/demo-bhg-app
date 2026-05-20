using System;
using System.Data;
using System.Data.SqlClient;
using Csla;
using Csla.Data;
using Csla.Validation;
using System.Collections;
using System.Transactions;

namespace BOLib
{
    [Serializable()]
    public class SYSCounterGrpDetItm : Csla.BusinessBase<SYSCounterGrpDetItm>, System.ComponentModel.IDataErrorInfo
    {
        #region Business Properties and Methods

        //declare members
        internal int? _codeKey = null;
        internal int? _counterGrp = 0;
        internal decimal? _seq = 1;
        internal string _segmentID = string.Empty;
        internal string _segmentType = string.Empty;
        internal string _segmentValue = string.Empty;
        internal string _segmentNmLang1 = string.Empty;
        internal string _segmentNmLang2 = string.Empty;
        internal string _segmentNmLang3 = string.Empty;
        internal string _segmentNmLang4 = string.Empty;
        internal string _segmentNmLang5 = string.Empty;
        internal string _segmentNmLang6 = string.Empty;
        internal string _segmentNmLang7 = string.Empty;
        internal string _segmentNmLang8 = string.Empty;
        internal string _segmentNmLang9 = string.Empty;
        internal string _segmentNmLang10 = string.Empty;
        internal string _beforeFormatSeperator = string.Empty;
        internal byte? _wordNum = 0;
        internal byte? _characterNum = 0;
        internal string _blankCharacter = string.Empty;
        internal string _afterFormatSeperator = string.Empty;
        internal bool? _selected = false;
        internal string _error = string.Empty;

        public int? CodeKey
        {
            get
            {
                return _codeKey;
            }
        }

        public int? CounterGrp
        {
            get
            {
                return _counterGrp;
            }
        }

        public decimal? Seq
        {
            get
            {
                return _seq;
            }
            set
            {
                _seq = value;
                PropertyHasChanged("Seq");                
            }
        }

        public string SegmentID
        {
            get
            {
                return _segmentID;
            }
        }

        public string SegmentType
        {
            get
            {
                return _segmentType;
            }
        }

        public string SegmentValue
        {
            get
            {
                return _segmentValue;
            }
            set
            {
                 _segmentValue = value;
                PropertyHasChanged("SegmentValue");                
            }
        }

        public string SegmentNmLang1
        {
            get
            {
                return _segmentNmLang1;
            }
        }

        public string SegmentNmLang2
        {
            get
            {
                return _segmentNmLang2;
            }
        }

        public string SegmentNmLang3
        {
            get
            {
                return _segmentNmLang3;
            }
        }

        public string SegmentNmLang4
        {
            get
            {
                return _segmentNmLang4;
            }
        }

        public string SegmentNmLang5
        {
            get
            {
                return _segmentNmLang5;
            }
        }

        public string SegmentNmLang6
        {
            get
            {
                return _segmentNmLang6;
            }
        }

        public string SegmentNmLang7
        {
            get
            {
                return _segmentNmLang7;
            }
        }

        public string SegmentNmLang8
        {
            get
            {
                return _segmentNmLang8;
            }
        }

        public string SegmentNmLang9
        {
            get
            {
                return _segmentNmLang9;
            }
        }

        public string SegmentNmLang10
        {
            get
            {
                return _segmentNmLang10;
            }
        }

        public string BeforeFormatSeperator
        {
            get
            {
                return _beforeFormatSeperator;
            }
            set
            {
                _beforeFormatSeperator = value;
                PropertyHasChanged("BeforeFormatSeperator");               
            }
        }

        public byte? WordNum
        {
            get
            {
                return _wordNum;
            }
            set
            {
                _wordNum = value;
                PropertyHasChanged("WordNum");
            }
        }

        public byte? CharacterNum
        {
            get
            {
                return _characterNum;
            }
            set
            {
                _characterNum = value;
                PropertyHasChanged("CharacterNum");                
            }
        }
        public string Error
        {
            get
            {
                return _error;
            }
            set
            {
                if (_error != value)
                    _error = value;
            }
        }
        public string BlankCharacter
        {
            get
            {
                return _blankCharacter;
            }
            set
            {
                _blankCharacter = value;
                PropertyHasChanged("BlankCharacter");
            }
        }

        public string AfterFormatSeperator
        {
            get
            {
                return _afterFormatSeperator;
            }
            set
            {
                _afterFormatSeperator = value;
                PropertyHasChanged("AfterFormatSeperator");
            }
        }

        public bool? Selected
        {
            get
            {
                return _selected;
            }
            set
            {
                _selected = value;
                PropertyHasChanged("Selected");
            }
        }

        protected override object GetIdValue()
        {
            return _codeKey.ToString() + _counterGrp.ToString() + _seq.ToString();
        }

        #endregion //Business Properties and Methods
       
        #region Factory Methods

        internal SYSCounterGrpDetItm()
        { /* require use of factory method */ }

        internal static SYSCounterGrpDetItm New()
        {
            
            SYSCounterGrpDetItm child = new SYSCounterGrpDetItm();
            
            return child;
        }

        internal static SYSCounterGrpDetItm NewChild()
        {
           // 
            SYSCounterGrpDetItm child = new SYSCounterGrpDetItm();
            child.ValidationRules.CheckRules();
            child.MarkAsChild();
            //
            return child;
        }

        internal static SYSCounterGrpDetItm Get(SafeDataReader dr)
        {
            //
            SYSCounterGrpDetItm child = new SYSCounterGrpDetItm();
            child.MarkAsChild();
            child.Fetch(dr);
            return child;
        }

        internal static SYSCounterGrpDetItm Get(GEnum.SystemCode codeKey, int? counterGrp, decimal? seq)
        {
            //
            SYSCounterGrpDetItm child = new SYSCounterGrpDetItm();
            child.Fetch(new Criteria(codeKey, counterGrp, seq, 1));
            return child;
        }

        internal static SYSCounterGrpDetItm Get(GEnum.SystemCode codeKey, int? counterGrp)
        {
            //
            SYSCounterGrpDetItm child = new SYSCounterGrpDetItm();
            child.Fetch(new Criteria(codeKey, counterGrp, 1));
            return child;
        }

        #endregion //Factory Methods

        #region Criteria

        [Serializable()]
        internal class Criteria
        {
            public GEnum.SystemCode? _codeKey = null;
            public int? _counterGrp = null;
            public decimal? _seq = null;
            public int? _option = null;

            internal Criteria()
            {
            }

            internal Criteria(GEnum.SystemCode? CodeKey, int? CounterGrp, decimal? Seq)
            {
                _codeKey = CodeKey;
                _counterGrp = CounterGrp;
                _seq = Seq;
            }

            internal Criteria(GEnum.SystemCode? CodeKey, int? CounterGrp, int? Option)
            {
                _codeKey = CodeKey;
                _counterGrp = CounterGrp;
                _option = Option;
            }

            internal Criteria(GEnum.SystemCode? CodeKey, int? CounterGrp, decimal? Seq, int? Option)
            {
                _codeKey = CodeKey;
                _counterGrp = CounterGrp;
                _seq = Seq;
                _option = Option;
            }
        }

        #endregion //Criteria

        #region Data Access - Fetch

        internal bool Fetch(Criteria criteria)
        {
            bool retValue = false;
    
            // Create new sql connection for this method. 
            using (SqlConnection cn = new SqlConnection(Database.BossDemoConnection))
            {
                // Open sql connection. 
                cn.Open();
                retValue = this.Fetch(cn, criteria);
            }

            return retValue;
        }

        internal bool Fetch(SqlConnection cn, Criteria criteria)
        {
            
            // Using existing sql connection.
            using (SqlCommand cm = cn.CreateCommand())
            {
                cm.CommandType = CommandType.StoredProcedure;
                cm.CommandText = "SYSCounterGrpDetItm_Get";

                cm.Parameters.AddWithValue("@Option", criteria._option);

                if (criteria._codeKey == null)
                    cm.Parameters.AddWithValue("@CodeKey", DBNull.Value);
                else
                    cm.Parameters.AddWithValue("@CodeKey", criteria._codeKey);

                if (criteria._counterGrp == null)
                    cm.Parameters.AddWithValue("@CounterGrp", DBNull.Value);
                else
                    cm.Parameters.AddWithValue("@CounterGrp", criteria._counterGrp);

                if (criteria._seq == null)
                    cm.Parameters.AddWithValue("@Seq", DBNull.Value);
                else
                    cm.Parameters.AddWithValue("@Seq", criteria._seq);

                cm.Parameters.AddWithValue("@RetValue", 0);
                cm.Parameters["@RetValue"].Direction = ParameterDirection.Output;


                // Using data reader as record set.
                using (SafeDataReader dr = new SafeDataReader(cm.ExecuteReader()))
                {
                    // If data reader can read, continue...
                    while (dr.Read())
                    {
                        this.Fetch(dr);
                    }

                }	// Already close and dispose data reader.

                if ((int)cm.Parameters["@RetValue"].Value == (int)GEnum.SpState.Pass)
                    return true;
                else
                    return false;
            }// Already close and dispose sql connection.
            
        }

        internal bool Fetch(SafeDataReader dr)
        {
            _codeKey = dr.GetInt32("CodeKey");
            _counterGrp = dr.GetInt32("CounterGrp");
            _seq = dr.GetDecimal("Seq");
            _segmentID = dr.GetString("SegmentID");
            _segmentType = dr.GetString("SegmentType");
            _segmentValue = dr.GetString("SegmentValue");
            _segmentNmLang1 = dr.GetString("SegmentNmLang1");
            _segmentNmLang2 = dr.GetString("SegmentNmLang2");
            _segmentNmLang3 = dr.GetString("SegmentNmLang3");
            _segmentNmLang4 = dr.GetString("SegmentNmLang4");
            _segmentNmLang5 = dr.GetString("SegmentNmLang5");
            _segmentNmLang6 = dr.GetString("SegmentNmLang6");
            _segmentNmLang7 = dr.GetString("SegmentNmLang7");
            _segmentNmLang8 = dr.GetString("SegmentNmLang8");
            _segmentNmLang9 = dr.GetString("SegmentNmLang9");
            _segmentNmLang10 = dr.GetString("SegmentNmLang10");
            _beforeFormatSeperator = dr.GetString("BeforeFormatSeperator");
            _wordNum = dr.GetByte("WordNum");
            _characterNum = dr.GetByte("CharacterNum");
            _blankCharacter = dr.GetString("BlankCharacter");
            _afterFormatSeperator = dr.GetString("AfterFormatSeperator");
            _selected = dr.GetBoolean("Selected");
            ValidationRules.CheckRules();

            return true;
            
        }
        #endregion //Data Access - Fetch

        #region Data Access - Insert

        internal bool Insert()
        {
            bool retValue = false;
            // Create Transaction Scope
            using (TransactionScope scope = new TransactionScope())
            {
                // Create SqlConnection
                using (SqlConnection cn = new SqlConnection(Database.BossDemoConnection))
                {
                    // Open Connection
                    cn.Open();

                    // Call insert method.
                    retValue = this.Insert(cn);
                }// End of SqlConnection

                // No errors - commit transaction
                  if (Transaction.Current.TransactionInformation.Status != TransactionStatus.Active)  throw new Exception("Transaction has aborted."); scope.Complete();
            }// End of TransactionScope

            return retValue;
        }

        internal bool Insert(SqlConnection cn)
        {
            // Using existing sql connection.
            using (SqlCommand cm = cn.CreateCommand())
            {
                cm.CommandType = CommandType.StoredProcedure;
                cm.CommandText = "SYSCounterGrpDetItm_AddUpdate";

                cm.Parameters.AddWithValue("@Option", 0);

                if (_codeKey == null)
                    cm.Parameters.AddWithValue("@CodeKey", DBNull.Value);
                else
                    cm.Parameters.AddWithValue("@CodeKey", _codeKey);

                if (_counterGrp == null)
                    cm.Parameters.AddWithValue("@CounterGrp", DBNull.Value);
                else
                    cm.Parameters.AddWithValue("@CounterGrp", _counterGrp);

                if (_seq == null)
                    cm.Parameters.AddWithValue("@Seq", DBNull.Value);
                else
                    cm.Parameters.AddWithValue("@Seq", _seq);

                if (_segmentID == null)
                    cm.Parameters.AddWithValue("@SegmentID", DBNull.Value);
                else
                    cm.Parameters.AddWithValue("@SegmentID", _segmentID);

                if (_segmentType == null)
                    cm.Parameters.AddWithValue("@SegmentType", DBNull.Value);
                else
                    cm.Parameters.AddWithValue("@SegmentType", _segmentType);

                if (_segmentValue == null)
                    cm.Parameters.AddWithValue("@SegmentValue", DBNull.Value);
                else
                    cm.Parameters.AddWithValue("@SegmentValue", _segmentValue);

                if (_segmentNmLang1 == null)
                    cm.Parameters.AddWithValue("@SegmentNmLang1", DBNull.Value);
                else
                    cm.Parameters.AddWithValue("@SegmentNmLang1", _segmentNmLang1);

                if (_segmentNmLang2 == null)
                    cm.Parameters.AddWithValue("@SegmentNmLang2", DBNull.Value);
                else
                    cm.Parameters.AddWithValue("@SegmentNmLang2", _segmentNmLang2);

                if (_segmentNmLang3 == null)
                    cm.Parameters.AddWithValue("@SegmentNmLang3", DBNull.Value);
                else
                    cm.Parameters.AddWithValue("@SegmentNmLang3", _segmentNmLang3);

                if (_segmentNmLang4 == null)
                    cm.Parameters.AddWithValue("@SegmentNmLang4", DBNull.Value);
                else
                    cm.Parameters.AddWithValue("@SegmentNmLang4", _segmentNmLang4);

                if (_segmentNmLang5 == null)
                    cm.Parameters.AddWithValue("@SegmentNmLang5", DBNull.Value);
                else
                    cm.Parameters.AddWithValue("@SegmentNmLang5", _segmentNmLang5);

                if (_segmentNmLang6 == null)
                    cm.Parameters.AddWithValue("@SegmentNmLang6", DBNull.Value);
                else
                    cm.Parameters.AddWithValue("@SegmentNmLang6", _segmentNmLang6);

                if (_segmentNmLang7 == null)
                    cm.Parameters.AddWithValue("@SegmentNmLang7", DBNull.Value);
                else
                    cm.Parameters.AddWithValue("@SegmentNmLang7", _segmentNmLang7);

                if (_segmentNmLang8 == null)
                    cm.Parameters.AddWithValue("@SegmentNmLang8", DBNull.Value);
                else
                    cm.Parameters.AddWithValue("@SegmentNmLang8", _segmentNmLang8);

                if (_segmentNmLang9 == null)
                    cm.Parameters.AddWithValue("@SegmentNmLang9", DBNull.Value);
                else
                    cm.Parameters.AddWithValue("@SegmentNmLang9", _segmentNmLang9);

                if (_segmentNmLang10 == null)
                    cm.Parameters.AddWithValue("@SegmentNmLang10", DBNull.Value);
                else
                    cm.Parameters.AddWithValue("@SegmentNmLang10", _segmentNmLang10);

                if (_beforeFormatSeperator == null)
                    cm.Parameters.AddWithValue("@BeforeFormatSeperator", DBNull.Value);
                else
                    cm.Parameters.AddWithValue("@BeforeFormatSeperator", _beforeFormatSeperator);

                if (_wordNum == null)
                    cm.Parameters.AddWithValue("@WordNum", DBNull.Value);
                else
                    cm.Parameters.AddWithValue("@WordNum", _wordNum);

                if (_characterNum == null)
                    cm.Parameters.AddWithValue("@CharacterNum", DBNull.Value);
                else
                    cm.Parameters.AddWithValue("@CharacterNum", _characterNum);

                if (_blankCharacter == null)
                    cm.Parameters.AddWithValue("@BlankCharacter", DBNull.Value);
                else
                    cm.Parameters.AddWithValue("@BlankCharacter", _blankCharacter);

                if (_afterFormatSeperator == null)
                    cm.Parameters.AddWithValue("@AfterFormatSeperator", DBNull.Value);
                else
                    cm.Parameters.AddWithValue("@AfterFormatSeperator", _afterFormatSeperator);

                if (_selected == null)
                    cm.Parameters.AddWithValue("@Selected", DBNull.Value);
                else
                    cm.Parameters.AddWithValue("@Selected", _selected);

                cm.Parameters.AddWithValue("@RetValue", 0);
                cm.Parameters["@RetValue"].Direction = ParameterDirection.Output;

                cm.ExecuteNonQuery();


                // _seq = (decimal)cm.Parameters["@NewSeq"].Value;

                if ((int)cm.Parameters["@RetValue"].Value == (int)GEnum.SpState.Pass)
                    return true;
                else
                    return false;
            }// Already close and dispose sql connection.
            
        }

        #endregion //Data Access - Insert

        #region Data Access - Delete

        internal bool Delete(Criteria criteria)
        {
            bool retValue = false;
            // Create Transaction Scope
            using (TransactionScope scope = new TransactionScope())
            {
                // Create SqlConnection
                using (SqlConnection cn = new SqlConnection(Database.BossDemoConnection))
                {
                    // Open Connection
                    cn.Open();

                    // Call delete method.
                    retValue = this.Delete(cn, criteria);
                }// End of SqlConnection

                // No errors - commit transaction
                  if (Transaction.Current.TransactionInformation.Status != TransactionStatus.Active)  throw new Exception("Transaction has aborted."); scope.Complete();
            }// End of TransactionScope              
            return retValue;
        }

        internal bool Delete(SqlConnection cn, Criteria criteria)
        {
            // Using existing sql connection.
            using (SqlCommand cm = cn.CreateCommand())
            {
                cm.CommandType = CommandType.StoredProcedure;
                cm.CommandText = "SYSCounterGrpDetItm_Delete";

                cm.Parameters.AddWithValue("@CodeKey", (int)criteria._codeKey);
              //  cm.Parameters.AddWithValue("@CounterGrp", criteria._counterGrp);
               // cm.Parameters.AddWithValue("@Seq", criteria._seq);

                cm.Parameters.AddWithValue("@RetValue", 0);
                cm.Parameters["@RetValue"].Direction = ParameterDirection.Output;
                cm.ExecuteNonQuery();
                if ((int)cm.Parameters["@RetValue"].Value == (int)GEnum.SpState.Pass)
                    return true;
                else
                    return false;
            }// Already close and dispose sql connection.            
        }

        #endregion //Data Access - Delete
    }
}
