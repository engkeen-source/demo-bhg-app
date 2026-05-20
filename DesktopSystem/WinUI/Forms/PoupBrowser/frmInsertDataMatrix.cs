using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using BOLib;
using System.Data.SqlClient;
using Infragistics.Win.UltraWinGrid;
using System.Collections;
using Infragistics.Win.UltraWinEditors;
using TAUtil;
namespace WinUI
{
    public partial class frmInsertDataMatrix : Form, DocInterface
    {

        #region Local Variables

        private bool formClose = false;
        private string ContextMenuSetting = string.Empty;
        private Document _objDoc = null;
        private UltraGrid tagrdDetItms = null;
        private DataTable _dtWorkingItems = null;
        private DataTable _dtWorkingColors = null;
        private int _selectedMasterItmKey = 0;

        #endregion

        /// <summary>
        /// Doc Interface method
        /// </summary>
        /// <param name="objDoc"></param>
        /// <param name="detail"></param>       
        public void GetDocInfor(out Document objDoc, out Hashtable detail)
        {
            objDoc = _objDoc;
            detail = null;
        }//Completed

        //Construstor
        public frmInsertDataMatrix(Document objDoc, UltraGrid tagrdDetItms)
        {
            InitializeComponent();
            _objDoc = objDoc;
            this.tagrdDetItms = tagrdDetItms;
            //LastActiveIndex = tagrdDetItms.ActiveRow.Index;
        }
        public frmInsertDataMatrix()
        {
            InitializeComponent();
        }
        
        //Form Events
        private void frmInsertDataMatrix_Load(object sender, EventArgs e)
        {
            // Waiting Cursor
            this.Cursor = Cursors.WaitCursor;

            try
            {
                //Format all grids and filter
                GlobalUI.FormGrids_Set(this, (int)_objDoc.DocCodeKey, out ContextMenuSetting);


                //Set ContextMenu & Grid Setting                           
                //GlobalUI.cmnuGlobal_Set(this);
                ContextMenuSetting += GlobalUI.ContextMenuSetting_GetNew((int)_objDoc.DocCodeKey);


                switch ((int)_objDoc.DocCodeKey)
                {
                    case (int)GEnum.SystemCode.Order_Consignment:
                    case (int)GEnum.SystemCode.Received_Consignment:                        
                        GlobalUI.BindComboValue(ItmKey, GVar.ListSettingID.MSTItmMCSG_id + "%" + AppInfor.ConAccessLevel + "%" + AppInfor.CurrentUserKey, "ID", "Key", _objDoc.DocCodeKey.Value);
                        break;
                    default:
                        GlobalUI.BindComboValue(ItmKey, GVar.ListSettingID.MSTItmM_id, "ID", "Key", _objDoc.DocCodeKey.Value);
                        break;
                }

                
                GlobalUI.FormReadOnly_Set(tagrdMasterItmList);


            }
            catch (Exception ex)
            {
                Error(ex, true);
            }
            finally
            {
                // Default Cursor
                this.Cursor = Cursors.Default;
            }
        }
        private void frmInsertDataMatrix_KeyDown(object sender, KeyEventArgs e)
        {
            try
            {
                //Set Focus Next Control
                GlobalUI.SelectNextControl(this, e);
            }
            catch (TAException tex)
            {
                Error(tex, true);
            }
            catch (Exception ex)
            {
                Error(ex, true);
            }
        }

        //Button Events
        private void btnCancel_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Cancel;
        }
        private void btnAppend_Click(object sender, EventArgs e)
        {
            try
            {
                tagrdMasterItmList.PerformAction(UltraGridAction.ExitEditMode);
                tagrdMasterItmList.UpdateData();

                tagrdItmSelected.PerformAction(UltraGridAction.ExitEditMode);
                tagrdItmSelected.UpdateData();

                if (_selectedMasterItmKey > 0)
                {
                    if (this.AppendData(_selectedMasterItmKey))
                    {
                        this.DialogResult = DialogResult.OK;
                    }
                }
            }
            catch (Exception ex)
            {
                Error(ex, true);
            }

        }

        //Grid Events
        private void tagrdMasterItmList_CustomCellUpdate(object sender, BeforeCellUpdateEventArgs e)
        {
            try
            {
                int ItmKey = 0;
                if (e.Cell.Row.Cells["Key"].Value != null)
                {
                    if (int.TryParse(e.Cell.Row.Cells["Key"].Value.ToString(), out ItmKey))
                    {
                        _selectedMasterItmKey = ItmKey;
                        this.GetItmMatrixData(_selectedMasterItmKey);
                    }
                }
            }
            catch (TAException tex)
            {
                Error(tex, true);
            }
            catch (Exception ex)
            {
                Error(ex, true);
            }
        }

        //Control Events
        private void Combo_NotInListAdd(object sender, ValidationErrorEventArgs e)
        {
            try
            {
                GlobalUI.ItemNotInList(sender as TAUtil.TAComboBox, e, true, 0);
            }
            catch (TAException tex)
            {
                Error(tex, true);
            }
            catch (Exception ex)
            {
                Error(ex, true);
            }
        }//CodeCompleted        
        private void ItmKey_CustomUpdate(object sender, CancelEventArgs e)
        {
            try
            {
                if (GFunc.IsNE(ItmKey.SelectedRow) == false)
                {
                    // get the row from the grid which represents the column for which the filter was modified.
                    UltraGridRow colRow = this.tagrdMasterItmList.Rows.OfType<UltraGridRow>().ToList().Find(row => row.Cells["ID"].Text.Equals(ItmKey.SelectedRow.Cells["ID"].Value.ToString(), StringComparison.CurrentCultureIgnoreCase));
                    this.tagrdMasterItmList.ActiveRow = colRow;
                    tagrdMasterItmList_CustomCellUpdate(null, new BeforeCellUpdateEventArgs(this.tagrdMasterItmList.ActiveRow.Cells["Key"], this.tagrdMasterItmList.ActiveRow.Cells["Key"].Value));
                }
            }
            catch (TAException tex)
            {
                Error(tex, true);
            }
            catch (Exception ex)
            {
                Error(ex, true);
            }
            
        }//CodeCompleted
        private void ItmKey_EditorButtonClick(object sender, EditorButtonEventArgs e)
        {            
            try
            {
                int itmkey = 0; 
                string id = string.Empty;
                string des = string.Empty;
                string listSettingID = GlobalUI.ListSettingID_Get(ContextMenuSetting, ((Control)sender).Name);

                if (DocHDRUtil.EditorButton_Popup(0, ItmKey.Text, listSettingID, (int)GEnum.PopupType.ItmID, ref itmkey, ref id, ref des))
                {
                    ItmKey.SetValueTrigger(id, false);
                }

            }
            catch (TAException tex)
            {
                Error(tex, true);
            }
            catch (Exception ex)
            {
                Error(ex, true);
            }
        }

        //Functions
        private bool GetItmMatrixData(int MasterItm)
        {
            int retValue = 0;
            // Waiting Cursor
            this.Cursor = Cursors.WaitCursor;

            try
            {
                List<SqlParameter> paraList = new List<SqlParameter>();
                paraList.Add(new SqlParameter("@MasterItmKey", MasterItm));
                SqlParameter para = new SqlParameter("@RetValue", retValue);
                para.Direction = ParameterDirection.Output;
                paraList.Add(para);
                //that SP will be return 3 DataTable,
                // Table 0 contain child items for selected masterItem is dataSource for Grid,
                //Table 1 is Mst_Itm with selected masterKey
                //Table 2 is REF_Color with selected masterKey
                DataSet ds = GFunc.ExecuteProcDataSet("Doc_ItmMatrix_Get", paraList);

                FormatGrid(ds.Tables[0]);

                if (ds.Tables.Count > 1)
                {
                    //Assign Mst_itm
                    _dtWorkingItems = ds.Tables[1];
                }

                if (ds.Tables.Count > 2)
                {
                    //Assign Ref_color
                    _dtWorkingColors = ds.Tables[2];
                }

            }
            catch (TAException tex)
            {
                throw Error(tex, false);
            }
            catch (Exception ex)
            {
                throw Error(ex, false);
            }
            finally
            {
                // Default Cursor
                this.Cursor = Cursors.Default;
            }
            return true;
        }
        private void FormatGrid(DataTable dt)
        {
            //this function to format the grid,user can only edit data in AliceBlue color cell,
            //by row 0 data and column header  pointed the actual ItmID of the cell data.
            if (dt.Rows.Count > 0)
            {
                this.tagrdItmSelected.DataSource = dt;
                Infragistics.Win.Appearance appearence = new Infragistics.Win.Appearance();
                appearence.AlphaLevel = ((short)(255));
                appearence.BackColor = System.Drawing.Color.AliceBlue;
                appearence.BackColor2 = System.Drawing.Color.AliceBlue;
                appearence.BorderColor = System.Drawing.Color.LightSteelBlue;
                appearence.BorderColor2 = System.Drawing.Color.LightSteelBlue;
                appearence.ForeColor = System.Drawing.Color.Black;

                appearence.FontData.Bold = Infragistics.Win.DefaultableBoolean.True;
                appearence.TextHAlignAsString = "Center";
                tagrdItmSelected.DisplayLayout.Override.HeaderAppearance = appearence;
                tagrdItmSelected.AutoAddNewRow = false;
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    foreach (DataColumn dc in dt.Columns)
                    {

                        if (dc.Ordinal > 0)
                        {
                            int value = 0;
                            int.TryParse(dt.Rows[i][dc.ColumnName].ToString(), out value);
                            if (value > 0)
                            {

                                this.tagrdItmSelected.Rows[i].Cells[dc.ColumnName].Activation = Activation.AllowEdit;
                                this.tagrdItmSelected.Rows[i].Cells[dc.ColumnName].Appearance.BackColor = Color.SkyBlue;
                                this.tagrdItmSelected.Rows[i].Cells[dc.ColumnName].Value = 0;

                            }
                            else
                            {
                                this.tagrdItmSelected.Rows[i].Cells[dc.ColumnName].Activation = Activation.Disabled;
                                this.tagrdItmSelected.Rows[i].Cells[dc.ColumnName].Appearance.BackColor = Color.White;
                                this.tagrdItmSelected.Rows[i].Cells[dc.ColumnName].Appearance.ForeColor = Color.White;
                                this.tagrdItmSelected.Rows[i].Cells[dc.ColumnName].Appearance.ForegroundAlpha = Infragistics.Win.Alpha.Transparent;



                            }

                            this.tagrdItmSelected.Rows[i].Update();

                        }
                        else
                        {
                            this.tagrdItmSelected.Rows[i].Cells[dc.ColumnName].Activation = Activation.ActivateOnly;

                        }

                    }
                }
            }
            else
            {
                tagrdItmSelected.DataSource = null;
            }


        }
        private bool AppendData(int masterItmKey)
        {
            bool bAppend = false;
            int colorKey = 0;

            try
            {
                //Create temp table to pass DocTransferData
                DataTable dtAppend = new DataTable();
                dtAppend.Columns.Add("DocItmKey", typeof(int));
                dtAppend.Columns.Add("LineLinkKey", typeof(int));
                dtAppend.Columns.Add("LineType", typeof(int));
                dtAppend.Columns.Add("ItmSN", typeof(decimal));
                dtAppend.Columns.Add("ItmKey", typeof(int));
                dtAppend.Columns.Add("ItmKeySelect", typeof(int));
                dtAppend.Columns.Add("ItmID", typeof(string));
                dtAppend.Columns.Add("ItmType", typeof(int));
                dtAppend.Columns.Add("ItmDes", typeof(string));
                dtAppend.Columns.Add("ItmQty", typeof(decimal));
                dtAppend.Columns.Add("ItmUOMKey", typeof(int));
                dtAppend.Columns.Add("ItmJobKey", typeof(int));
                dtAppend.Columns.Add("ItmJobPhaseKey", typeof(int));
                dtAppend.Columns.Add("ItmJobTaskKey", typeof(int));
                dtAppend.Columns.Add("ItmJobCostTypeKey", typeof(int));
                dtAppend.Columns.Add("ItmVendorKey", typeof(int));
                dtAppend.Columns.Add("ItmUOMID", typeof(string));
                dtAppend.Columns.Add("ItmIGrpQtyLock", typeof(bool));
                dtAppend.Columns.Add("ItmIGrpToPrint", typeof(bool));
                dtAppend.Columns.Add("ItmIGrpQtySet", typeof(decimal));
                dtAppend.Columns.Add("ItmIGrpAmtSet", typeof(decimal));
                dtAppend.Columns.Add("ItmDetSN", typeof(decimal));
                dtAppend.Columns.Add("ItmPriceAfter", typeof(decimal));
                dtAppend.Columns.Add("Remarks", typeof(string));

                foreach (UltraGridRow row in tagrdItmSelected.Rows)
                {
                    foreach (UltraGridColumn col in tagrdItmSelected.DisplayLayout.Bands[0].Columns)
                    {
                        if ((col.Index > 0) && (GFunc.NEDec(row.Cells[col.Key].Value, 0) > 0))
                        {
                            //_dtWorkingColors.DefaultView.RowFilter = "ColorID = '" + row.Cells[0].Value.ToString() + "'";
                            //colorKey = GFunc.NEInt(_dtWorkingColors.DefaultView[0]["ColorKey"], 0);
                            //_dtWorkingColors.DefaultView.RowFilter = "";
                            IEnumerable<DataRow> dtFilter = _dtWorkingColors.AsEnumerable().Where(r => r.Field<string>("ColorID").Equals(row.Cells[0].Value.ToString()));
                            if (dtFilter.Count() > 0)
                                colorKey = GFunc.NEInt(dtFilter.ElementAt(0)["ColorKey"], 0);

                            //_dtWorkingItems.DefaultView.RowFilter = "MasterItmKey = " + masterItmKey.ToString() + "And ColorKey=" + colorKey.ToString() + " And ScaleSize='" + col.Header.Caption + "'";
                            dtFilter = _dtWorkingItems.AsEnumerable().Where(r => r.Field<int>("MasterItmKey") == masterItmKey && r.Field<int>("ColorKey") == colorKey && r.Field<string>("ScaleSize").Equals(col.Header.Caption));
                            if (dtFilter.Count() > 0)
                            {
                                DataRow drNew = dtAppend.NewRow();
                                drNew["ItmKey"] = GFunc.NEInt(dtFilter.ElementAt(0)["ItmKey"], 0);
                                drNew["ItmKeySelect"] = GFunc.NEInt(dtFilter.ElementAt(0)["ItmKey"], 0);
                                drNew["ItmType"] = GFunc.NEInt(dtFilter.ElementAt(0)["ItmType"], 0);
                                drNew["ItmUOMKey"] = GFunc.NEInt(dtFilter.ElementAt(0)["BUOMKey"], 0);
                                drNew["LineType"] = (int)GEnum.RecDetailType.DItems;
                                drNew["ItmID"] = GFunc.NEStr(dtFilter.ElementAt(0)["ItmID"], string.Empty);
                                drNew["ItmQty"] = GFunc.NEDec(row.Cells[col.Key].Value, 0);

                                dtAppend.Rows.Add(drNew);

                            }
                            //_dtWorkingItems.DefaultView.RowFilter = "";

                        }
                    }
                }

                dtAppend.AcceptChanges();
                bAppend = DocHDRUtil.DocTransferData((int)_objDoc.DocCodeKey, (int)_objDoc.DocKey, GFunc.NEInt(GFunc.GetPropertyValue("DocConKey", _objDoc), 0), dtAppend, _objDoc, tagrdDetItms, 0, "", false, false);

            }
            catch (Exception ex)
            {
                bAppend = false;
                throw Error(ex, false);

            }
            return bAppend;
        }
        
        //Error Functions
        private Exception Error(Exception ex, bool ShowMessage)
        {
            Exception l_tmpex = ex;
            try
            {
                if (this.ActiveControl != null)
                {
                    if (this.ActiveControl.GetType() != typeof(TAUtil.TAGridEditor))
                    {
                        l_tmpex = SysAuditLogUtility.ModifyException(ex, ShowMessage, new object[] { this.Name, this.ActiveControl });
                    }
                    else
                    {
                        l_tmpex = SysAuditLogUtility.ModifyException(ex, ShowMessage, new object[] { this.Name, this.ActiveControl.Name, GFunc.GridColumnKey_Get(this.ActiveControl) });
                    }
                }
                else
                {
                    l_tmpex = SysAuditLogUtility.ModifyException(ex, ShowMessage, new object[] { });
                }
                if (ShowMessage)
                {
                    SysAuditLogUtility.AddErrorLog(l_tmpex);
                }

                return l_tmpex;
            }
            catch (Exception nex)
            {
                MsgBox.Show(nex.Message);
            }
            return l_tmpex;
        }
        private TAException Error(TAException ex, bool ShowMessage)
        {
            try
            {
                TAException l_tmpex = ex;
                if (this.ActiveControl != null)
                {
                    if (this.ActiveControl.GetType() != typeof(TAUtil.TAGridEditor))
                    {
                        l_tmpex = SysAuditLogUtility.ModifyTAException(ex, ShowMessage, new object[] { this.Name, this.ActiveControl });
                    }
                    else
                    {
                        l_tmpex = SysAuditLogUtility.ModifyTAException(ex, ShowMessage, new object[] { this.Name, this.ActiveControl.Name, GFunc.GridColumnKey_Get(this.ActiveControl) });
                    }
                }
                else
                {
                    l_tmpex = SysAuditLogUtility.ModifyTAException(ex, ShowMessage, new object[] { });
                }
                if (ShowMessage)
                {
                    SysAuditLogUtility.AddErrorLog(l_tmpex);
                }

                return l_tmpex;
            }
            catch (Exception nex)
            {
                MsgBox.Show(nex.Message);
            }
            return ex;
        }

        
        


        

        

       







    }
}
