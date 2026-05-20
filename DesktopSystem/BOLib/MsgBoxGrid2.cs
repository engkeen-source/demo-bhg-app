using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.IO;
using System.Data.SqlClient;
using Infragistics.Win.UltraWinGrid;
using Infragistics.Win;


namespace BOLib
{
    public partial class MsgBoxGrid : Form
    {

        private GEnum.MsgBoxButton btnReturn;
        private GEnum.MsgBoxDefaultButton btnDefault;
        private Infragistics.Win.Misc.UltraButton[] btns;
        System.Data.SqlClient.SqlConnection ActiveConnection;
        #region +++ Constructor +++

        public MsgBoxGrid()
        {
            InitializeComponent();
        }

        public MsgBoxGrid(SqlConnection cn, string msgID, DataTable dtErrolist, GEnum.MsgBoxIcon icon, GEnum.MsgBoxButton[] msgButtons)
        {
            InitializeComponent();

            string outMsgID = string.Empty;

            MsgBoxFactory objMsgBoxFactory = new MsgBoxFactory();
            objMsgBoxFactory.Connection = cn;
            objMsgBoxFactory.Initialisation((GEnum.InstanceMode?)GEnum.InstanceMode.Normal, msgID, msgButtons);
            objMsgBoxFactory.FormWidth = this.Width;
            objMsgBoxFactory.PrepareData();

            #region +++ Build UI +++
            this.Width = objMsgBoxFactory.FormWidth;
            this.Text = objMsgBoxFactory.Title;

            pic.Visible = false;

            this.SuspendLayout();

            this.Width = objMsgBoxFactory.FormWidth;
            this.Text = objMsgBoxFactory.Title;

            utxtMsgBody.Value = objMsgBoxFactory.BodyText;

            btns = new Infragistics.Win.Misc.UltraButton[msgButtons.Length];

            pnlButton.SuspendLayout();
            for (int i = 0; i < msgButtons.Length; i++)
            {
                btns[i] = new Infragistics.Win.Misc.UltraButton();
                btns[i].Text = objMsgBoxFactory.ButtonCaptions[i];
                btns[i].Left = utxtMsgBody.Left + objMsgBoxFactory.ButtonLefts[i];
                btns[i].Width = objMsgBoxFactory.ButtonWidth;
                btns[i].Height = objMsgBoxFactory.ButtonHeight;
                //btns[i].Top = objMsgBoxFactory.ButtonTop;

                btns[i].Visible = true;
                btns[i].Click += new EventHandler(Button_Click);
                btns[i].Tag = objMsgBoxFactory.buttonNos[i];
                this.pnlButton.Controls.Add(btns[i]);
            }
            pnlButton.ResumeLayout(false);
            this.tagrdInfo.DataSource = dtErrolist;
            if (dtErrolist == null)
                this.utcMsg.Tabs[1].Visible = false;
            //int rowCount = dtErrolist.Rows.Count;
            //if (rowCount > 10)
            //    rowCount = 10;

            //this.Height = pnlButton.Height + rowCount * this.tagrdInfo.DisplayLayout.Rows[0].Height + 100;
            this.ResumeLayout(true);

            #endregion

            this.btnDefault = GEnum.MsgBoxDefaultButton.DefaultButton1;

        }
        public MsgBoxGrid(string msgID, DataTable dtErrolist, GEnum.MsgBoxIcon icon, GEnum.MsgBoxButton[] msgButtons)
            : this(null, msgID, dtErrolist, icon, msgButtons)
        {
        }

        #endregion

        #region +++ UI Control Events +++

        private void Button_Click(object sender, EventArgs e)
        {
            Infragistics.Win.Misc.UltraButton Btn = sender as Infragistics.Win.Misc.UltraButton;
            this.btnReturn = (GEnum.MsgBoxButton)Btn.Tag;
            this.DialogResult = DialogResult.OK;
        }
        private void MsgBox_Shown(object sender, EventArgs e)
        {
            #region Default Button

            switch (btnDefault)
            {
                case GEnum.MsgBoxDefaultButton.DefaultButton1:
                    this.AcceptButton = btns[0];
                    btns[0].Focus();
                    break;

                case GEnum.MsgBoxDefaultButton.DefaultButton2:
                    this.AcceptButton = btns[1];
                    btns[1].Focus();
                    break;

                case GEnum.MsgBoxDefaultButton.DefaultButton3:
                    this.AcceptButton = btns[2];
                    btns[2].Focus();
                    break;
                case GEnum.MsgBoxDefaultButton.DefaultButton4:
                    this.AcceptButton = btns[3];
                    btns[3].Focus();
                    break;
            }
            #endregion
        }
      
        #endregion

        private static GEnum.MsgBoxButton ShowMe(MsgBoxGrid frm)
        {
            try
            {
                frm.TopMost = true;
                frm.ShowDialog();
                GEnum.MsgBoxButton btnReturn = frm.btnReturn;
                frm.Dispose();
                return btnReturn;
            }
            catch (Exception ex)
            {
                MsgBox.Show(ex.Message);
                //SysAuditLogUtility.AddErrorLog( GEnum.AuditLogMode.Error, GEnum.SystemCode.Alert_Log, msgID, null);                    
            }
            return GEnum.MsgBoxButton.Cancel;
        }


        public static GEnum.MsgBoxButton Show(string msgID, DataTable dt, GEnum.MsgBoxIcon icon, params GEnum.MsgBoxButton[] msgButtons)
        {
            MsgBoxGrid frm = new MsgBoxGrid(msgID, dt, icon, msgButtons);
            return ShowMe(frm);
        }
        public static GEnum.MsgBoxButton Show(SqlConnection cn, string msgID, DataTable dt, GEnum.MsgBoxIcon icon, params GEnum.MsgBoxButton[] msgButtons)
        {
            MsgBoxGrid frm = new MsgBoxGrid(cn, msgID, dt, icon, msgButtons);
            return ShowMe(frm);
        }

        public static void Grid_Format(TAUtil.TAGridEditor tagrd, string listSettingID, bool fillData, bool readOnly)
        {
            try
            {
                SYSFormSettingID objListSettingDet;

                string[] ListParameters = listSettingID.Split('%');
                SYSFormSettingIDDet objFormSettingDet = null;
                if (ListParameters.Length > 0)
                {
                    //Use index 0 to List Setting.
                    objListSettingDet = SYSFormSettingID.Get(ListParameters[0]);
                    objFormSettingDet = SYSFormSettingIDDet.Get(ListParameters[0]);
                }
                else
                    objListSettingDet = SYSFormSettingID.Get(listSettingID);


                DataTable dtdet = (DataTable)objFormSettingDet;

                if (!GFunc.IsNE(objListSettingDet))
                {
                    for (int i = 1; i < ListParameters.Length; i++)
                    {
                        string oldVal = "{" + (i - 1) + "}";
                        objListSettingDet.ListSQL = objListSettingDet.ListSQL.Replace(oldVal, ListParameters[i]);
                    }
                }

                if (!GFunc.IsNE(objListSettingDet.ListSQL))
                {
                    tagrd.DataSource = GFunc.ExecuteQuery(objListSettingDet.ListSQL);
                }

                if (dtdet == null)
                {
                    MsgBox.Show("FormSettingIDDet haven't filled!");
                }
                else
                {
                    switch (listSettingID)
                    {
                        ////Skip ListID that don't want to format grid
                        //case "frmMSTItmGridPrice":
                        //    break;
                        default:
                            Grid_Format(tagrd, dtdet, readOnly);
                            break;
                    }

                }
            }
            catch (Exception ex)
            {
                throw SysAuditLogUtility.ModifyException(ex, false, new object[] { });
            }
        }
        public static void Grid_Format(TAUtil.TAGridEditor grd, DataTable dtDet, bool readOnly)
        {
            bool hidden = false;
            string header = string.Empty;
            int width = 0;
            string format = string.Empty;
            string alignment = string.Empty;

            int totalColWidth = 0;

            //hide all grid column before format - pauk
            foreach (UltraGridColumn dc in grd.DisplayLayout.Bands[0].Columns)
            {
                dc.Hidden = true;
            }

            foreach (DataRow dr in dtDet.Rows)
            {
                if (grd.DisplayLayout.Bands[0].Columns.Exists(GFunc.NEStr(dr["ColFldNm"], "")))
                {
                    UltraGridColumn c = grd.DisplayLayout.Bands[0].Columns[GFunc.NEStr(dr["ColFldNm"], "")];
                    header = GFunc.NEStr(dr["ColHeader"], c.Key);
                    hidden = GFunc.NEBool(dr["ColHide"], false);

                    width = GFunc.NEInt(dr["ColWidth"], 0);
                    format = GFunc.NEStr(dr["ColDataFormat"], "");
                    alignment = GFunc.NEStr(dr["ColAlignment"], "");


                    c.Hidden = hidden || (width == 0);
                    c.Header.Caption = header;
                    c.Width = width;
                    if (!c.Hidden)
                        totalColWidth += c.Width;
                    //Format string is from the FormSetting tables
                    if (format != string.Empty)
                    {
                        if (format.Trim().StartsWith("?"))
                        {
                            c.Nullable = Infragistics.Win.UltraWinGrid.Nullable.Null; //null store as DBNull
                            format = format.Trim().Replace("?", "");
                        }
                        else
                            c.Nullable = Infragistics.Win.UltraWinGrid.Nullable.Disallow;
                        c.Format = format;
                    }

                    if (alignment != string.Empty)
                    {
                        switch (alignment.ToLower())
                        {
                            case "l":
                                c.CellAppearance.TextHAlign = HAlign.Left;
                                break;
                            case "r":
                                c.CellAppearance.TextHAlign = HAlign.Right;
                                break;
                            case "c":
                                c.CellAppearance.TextHAlign = HAlign.Center;
                                break;
                        }
                        c.Header.Appearance = c.CellAppearance;
                    }

                    if (readOnly)
                    {
                        c.CellActivation = Activation.NoEdit;
                        if (c.DataType == typeof(bool))
                        {
                            c.CellAppearance.BackColorDisabled = System.Drawing.Color.Gray;
                        }
                    }

                    if (GFunc.NEInt(dr["ColPosition"], 0) > 0) //if dr["ColPosition"] is DBNull, will not change Visible Position
                        c.Header.VisiblePosition = Convert.ToInt32(dr["ColPosition"]);

                    c.TabStop = Convert.ToBoolean(GFunc.NEBool(dr["ColTabStop"], true));
                }
            }

            if (grd.Parent.GetType() == typeof(SplitterPanel) && (((SplitterPanel)grd.Parent)).Location.X < 100//Checking left or right side panel, Left Panel loction is always 0, less than 100
               && ((System.Windows.Forms.SplitContainer)(((System.Windows.Forms.SplitterPanel)(grd.Parent)).Parent)).Orientation == Orientation.Vertical)
            {
                ((SplitContainer)(grd.Parent.Parent)).SplitterDistance = totalColWidth;
            }


            //Set Appearence
            //Header
            Infragistics.Win.Appearance appearence_Header = new Infragistics.Win.Appearance();
            appearence_Header.AlphaLevel = ((short)(255));
            appearence_Header.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(243)))), ((int)(((byte)(234)))), ((int)(((byte)(226)))));
            appearence_Header.BackColor2 = System.Drawing.Color.FromArgb(((int)(((byte)(243)))), ((int)(((byte)(234)))), ((int)(((byte)(226)))));
            appearence_Header.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(243)))), ((int)(((byte)(234)))), ((int)(((byte)(226)))));
            appearence_Header.BorderColor2 = System.Drawing.Color.FromArgb(((int)(((byte)(243)))), ((int)(((byte)(234)))), ((int)(((byte)(226)))));
            appearence_Header.FontData.ItalicAsString = "True";
            appearence_Header.FontData.Name = "Calibri";
            appearence_Header.FontData.SizeInPoints = 10F;
            appearence_Header.ForeColor = System.Drawing.Color.Black;
            //appearence_Header.TextHAlignAsString = "LEFT";
            grd.DisplayLayout.Override.HeaderAppearance = appearence_Header;

            //Row
            Infragistics.Win.Appearance appearence_Row = new Infragistics.Win.Appearance();
            appearence_Row.FontData.Name = "Calibri";
            appearence_Row.FontData.SizeInPoints = 11F;
            appearence_Row.ForeColor = System.Drawing.Color.Black;
            appearence_Row.TextHAlignAsString = "LEFT";
            appearence_Row.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(243)))), ((int)(((byte)(234)))), ((int)(((byte)(226)))));
            appearence_Row.BorderColor2 = System.Drawing.Color.FromArgb(((int)(((byte)(243)))), ((int)(((byte)(234)))), ((int)(((byte)(226)))));
            grd.DisplayLayout.Override.RowAppearance = appearence_Row;

            //Cell
            Infragistics.Win.Appearance appearence_Cell = new Infragistics.Win.Appearance();
            appearence_Cell.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(243)))), ((int)(((byte)(234)))), ((int)(((byte)(226)))));
            appearence_Cell.BorderColor2 = System.Drawing.Color.FromArgb(((int)(((byte)(243)))), ((int)(((byte)(234)))), ((int)(((byte)(226)))));
            grd.DisplayLayout.Override.CellAppearance = appearence_Cell;

            grd.DisplayLayout.Override.RowSelectors = Infragistics.Win.DefaultableBoolean.False;
            //Row Selector
            //Infragistics.Win.Appearance appearence_RowSelector = new Infragistics.Win.Appearance();
            //appearence_RowSelector.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(243)))), ((int)(((byte)(234)))), ((int)(((byte)(226)))));
            //appearence_RowSelector.BackColor2 = System.Drawing.Color.FromArgb(((int)(((byte)(243)))), ((int)(((byte)(234)))), ((int)(((byte)(226)))));
            //grd.DisplayLayout.Override.RowSelectorAppearance = appearence_RowSelector;

            //Appearence
            Infragistics.Win.Appearance appearence = new Infragistics.Win.Appearance();
            appearence.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(243)))), ((int)(((byte)(234)))), ((int)(((byte)(226)))));
            appearence.BackColor2 = System.Drawing.Color.FromArgb(((int)(((byte)(243)))), ((int)(((byte)(234)))), ((int)(((byte)(226)))));
            grd.DisplayLayout.Appearance = appearence;

            //Row Header Selector
            //Infragistics.Win.Appearance appearence_RowHeaderSelector = new Infragistics.Win.Appearance();
            //appearence_RowHeaderSelector.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(243)))), ((int)(((byte)(234)))), ((int)(((byte)(226)))));
            //appearence_RowHeaderSelector.BackColor2 = System.Drawing.Color.FromArgb(((int)(((byte)(243)))), ((int)(((byte)(234)))), ((int)(((byte)(226)))));
            //grd.DisplayLayout.Override.RowSelectorHeaderAppearance = appearence_RowHeaderSelector;

            //Selected Row Appearence
            Infragistics.Win.Appearance appearence_ActiveRow = new Infragistics.Win.Appearance();
            appearence_ActiveRow.BackColor = System.Drawing.Color.BlanchedAlmond;
            appearence_ActiveRow.BackColor2 = SystemColors.Window;
            appearence_ActiveRow.ForeColor = Color.Black;
            appearence_ActiveRow.BackGradientStyle = GradientStyle.GlassBottom50Bright;
            grd.DisplayLayout.Override.ActiveRowAppearance = appearence_ActiveRow;

            grd.DisplayLayout.AutoFitStyle = AutoFitStyle.None;
            grd.TextRenderingMode = TextRenderingMode.GDI;

        }

        private void MsgBoxGrid_KeyDown(object sender, KeyEventArgs e)
        {
            try
            {

                if (e.KeyCode == Keys.Escape)
                {
                    if (this.Modal == true)
                        this.DialogResult = DialogResult.Cancel;

                }
            }
            catch (Exception ex)
            {
                MsgBox.Show(ex.Message);
            }
        }


        #region Commented
        //private string[] GetButtonCaptions(int msgButtons,out int[]msgBtnVals)
        //{
        //    char[] msgBtnBinarys = ToBinary(msgButtons);
        //    int[] msgBtns = new int[msgBtnBinarys.Length];
        //    int j = 0;

        //    for (int i = 0; i < msgBtnBinarys.Length; i++)
        //        if (msgBtnBinarys[i].Equals('1'))
        //        {
        //            msgBtns[j] = Convert.ToInt32(Math.Pow(2, i));
        //            j++;
        //        }

        //    msgBtnVals = msgBtns;

        //    if (j <= 0) return null;
        //    if (j <= 1) return new string[] { GetBtnCaption(msgBtns[0]) };
        //    if (j <= 2) return new string[] { GetBtnCaption(msgBtns[0]), GetBtnCaption(msgBtns[1]) };
        //    if (j <= 3) return new string[] { GetBtnCaption(msgBtns[0]), GetBtnCaption(msgBtns[1]), GetBtnCaption(msgBtns[2]) };
        //    if (j <= 4) return new string[] { GetBtnCaption(msgBtns[0]), GetBtnCaption(msgBtns[1]), GetBtnCaption(msgBtns[2]), GetBtnCaption(msgBtns[3]) };
        //    return null;
        //}


        //// private string ToBinary(Int64 Decimal)
        //private char[] ToBinary(Int64 Decimal)
        //{
        //    // Declare a few variables we're going to need
        //    Int64 BinaryHolder;
        //    char[] BinaryArray;
        //    string BinaryResult = "";

        //    while (Decimal > 0)
        //    {
        //        BinaryHolder = Decimal % 2;
        //        BinaryResult += BinaryHolder;
        //        Decimal = Decimal / 2;
        //    }

        //    // The algoritm gives us the binary number in reverse order (mirrored)
        //    // We store it in an array so that we can reverse it back to normal
        //    BinaryArray = BinaryResult.ToCharArray();
        //    // Array.Reverse(BinaryArray);
        //    // BinaryResult = new string(BinaryArray);

        //    // return BinaryResult;
        //    return BinaryArray;
        //}

        //private void AdjustSize()
        //{     
        //    this.AutoSize = false;
        //    this.ubtn1.AutoSize = false;
        //    this.ubtn2.AutoSize = false;
        //    this.ubtn3.AutoSize = false;
        //    this.ubtn4.AutoSize = false;
        //    if ((nBtnWidth * nBtnCount) + (20 * nBtnCount) > this.Width)
        //        this.Width = (nBtnWidth * nBtnCount) + (20 * nBtnCount);
        //    if (nBtnWidth < 50)
        //        nBtnWidth = 50;
        //    this.ubtn1.Width = nBtnWidth;
        //    this.ubtn2.Width = nBtnWidth;
        //    this.ubtn3.Width = nBtnWidth;
        //    this.ubtn4.Width = nBtnWidth;
        //    this.ubtn1.Height += 5;
        //    this.ubtn2.Height += 5;
        //    this.ubtn3.Height += 5;
        //    this.ubtn4.Height += 5;

        //    if (nBtnCount == 1)
        //    {
        //        this.ubtn1.Left = this.Width / 2 - this.ubtn1.Width / 2;
        //    }
        //    if (nBtnCount == 2)
        //    {
        //        //this.ubtn1.Left = this.Width / 3 - this.ubtn1.Width / 2;
        //        //this.ubtn2.Left = ((this.Width / 3) * 2) - this.ubtn1.Width / 2;

        //        this.ubtn1.Left = this.Width / 2 - this.ubtn1.Width - 10;
        //        this.ubtn2.Left = ubtn1.Left + ubtn1.Width + 10; //this.Width / 2 + 10;
        //    }
        //    if (nBtnCount == 3)
        //    {
        //        //this.ubtn1.Left = this.Width / 4 - this.ubtn1.Width / 2;
        //        //this.ubtn2.Left = ((this.Width / 4) * 2) - this.ubtn2.Width / 2;
        //        //this.ubtn3.Left = ((this.Width / 4) * 3) - this.ubtn3.Width / 2;
        //        this.ubtn1.Left = this.Width / 3 - this.ubtn1.Width - 10;
        //        this.ubtn2.Left = ubtn1.Left + ubtn1.Width + 10;
        //        this.ubtn3.Left = ubtn2.Left + ubtn2.Width + 10;
        //    }
        //    if (nBtnCount == 4)
        //    {
        //        this.ubtn1.Left = this.Width / 4 - this.ubtn1.Width - 15;
        //        this.ubtn2.Left = ubtn1.Left + ubtn1.Width + 10;
        //        this.ubtn3.Left = ubtn2.Left + ubtn2.Width + 10;
        //        this.ubtn4.Left = ubtn3.Left + ubtn3.Width + 10;
        //    }
        //}
        #endregion

        private void tagrdInfo_InitializeLayout(object sender, InitializeLayoutEventArgs e)
        {
            UltraGridLayout layout = e.Layout;
            UltraGridBand band = layout.Bands[0];
            foreach (UltraGridColumn column in band.Columns)
            {
                if (!column.Header.Caption.ToLower().Contains("description"))
                    column.PerformAutoResize(PerformAutoSizeType.AllRowsInBand, AutoResizeColumnWidthOptions.All);
                else
                    column.Width = 180;
            }

            layout.AutoFitStyle = AutoFitStyle.ExtendLastColumn;
        }
    }
}
