using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Infragistics.Win.UltraWinGrid;
using System.Data;
using System.Transactions;
using System.Data.SqlClient;
using System.Windows.Forms;
using System.Reflection;
using System.Collections;
using BOLib;
using Infragistics.Win.UltraWinMaskedEdit;
using TAUtil;

namespace WinUI
{
    public class ComFrmUtility
    {
        //Set Error Methods
        private static Exception Error(Exception ex, bool ShowMessage)
        {
            try
            {
                ex = SysAuditLogUtility.ModifyException(ex, false);

                if (ShowMessage)
                {
                    SysAuditLogUtility.AddErrorLog(ex);
                }
            }
            catch (Exception nex)
            {
               MsgBox.Show(nex.Message);
            }
            return ex;

        }
        private static TAException Error(TAException ex, bool ShowMessage)
        {
            try
            {
                ex = SysAuditLogUtility.ModifyTAException(ex, false);

                if (ShowMessage)
                {
                    SysAuditLogUtility.AddErrorLog(ex);
                }
            }
            catch (Exception nex)
            {
                MsgBox.Show(nex.Message);
            }
            return ex;
        }

        //Function has NOT been match with TBS  --------------------------PLEASE CREATE NEW FUNCTION BELOW THIS LINE-----------------------------------------------


    }

}
