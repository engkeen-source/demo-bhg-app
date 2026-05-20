


using System;
using System.Collections.Generic;
using System.Transactions;
using System.Data;
using System.Data.SqlClient;
using System.ComponentModel;
namespace BOLib
{
    /// <summary>
    /// Summary description for TASAlertDetSub.
    /// </summary>
    [Serializable]
    public class TASAlertDetSub 
    {
        #region +++  Local variables declaration for the class +++
        protected int _ToDoKey;
        protected int _UserKey;
        protected string _email;

        public event PropertyChangedEventHandler PropertyChanged;

        #endregion

        #region +++  Properties  +++
        private void NotifyPropertyChanged(String info)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(info));
            }
        }        
        public int ToDoKey
        {

            get
            {
                return this._ToDoKey;
            }
            set
            {
                this._ToDoKey = value;
                NotifyPropertyChanged("ToDoKey");
            }
        }
        public int UserKey
        {

            get
            {
                return this._UserKey;
            }
            set
            {
                this._UserKey = value;
                NotifyPropertyChanged("UserKey");
            }
        }
        public string email
        {

            get
            {
                return this._email;
            }
            set
            {
                this._email = value;
                NotifyPropertyChanged("email");
            }
        }
        
        #endregion
    }
}





