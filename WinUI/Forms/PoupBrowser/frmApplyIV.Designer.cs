namespace WinUI
{
    partial class frmApplyIV
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            Infragistics.Win.Appearance appearance1 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance2 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance3 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance4 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance5 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance6 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance7 = new Infragistics.Win.Appearance();
            Infragistics.Win.UltraWinEditors.EditorButton editorButton1 = new Infragistics.Win.UltraWinEditors.EditorButton();
            Infragistics.Win.Appearance appearance8 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance9 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance10 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance11 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance12 = new Infragistics.Win.Appearance();
            Infragistics.Win.UltraWinEditors.EditorButton editorButton2 = new Infragistics.Win.UltraWinEditors.EditorButton();
            Infragistics.Win.Appearance appearance13 = new Infragistics.Win.Appearance();
            Infragistics.Win.UltraWinEditors.EditorButton editorButton3 = new Infragistics.Win.UltraWinEditors.EditorButton();
            Infragistics.Win.Appearance appearance14 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance15 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance16 = new Infragistics.Win.Appearance();
            Infragistics.Win.UltraWinEditors.EditorButton editorButton4 = new Infragistics.Win.UltraWinEditors.EditorButton();
            Infragistics.Win.Appearance appearance17 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance18 = new Infragistics.Win.Appearance();
            this.btnApply = new Infragistics.Win.Misc.UltraButton();
            this.btnClose = new Infragistics.Win.Misc.UltraButton();
            this.ultraLabel7 = new Infragistics.Win.Misc.UltraLabel();
            this.ultraLabel5 = new Infragistics.Win.Misc.UltraLabel();
            this.DocumentSource = new TAUtil.TAComboBox();
            this.ultraLabel1 = new Infragistics.Win.Misc.UltraLabel();
            this.DocApplyGainAccDes = new TAUtil.TATextBoxEditor();
            this.DocApplyGainAmt = new TAUtil.TANumericEditor();
            this.ultraLabel84 = new Infragistics.Win.Misc.UltraLabel();
            this.ultraLabel4 = new Infragistics.Win.Misc.UltraLabel();
            this.panel2 = new System.Windows.Forms.Panel();
            this.DocApplyGainAccKey = new TAUtil.TAComboBox();
            this.panel3 = new System.Windows.Forms.Panel();
            this.DocApplyIVDate = new TAUtil.TADateEditor();
            this.ultraLabel3 = new Infragistics.Win.Misc.UltraLabel();
            this.DocApplyIVID = new TAUtil.TATextBoxEditor();
            this.panel4 = new System.Windows.Forms.Panel();
            this.btnDelete = new Infragistics.Win.Misc.UltraButton();
            ((System.ComponentModel.ISupportInitialize)(this.DocumentSource)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.DocApplyGainAccDes)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.DocApplyGainAmt)).BeginInit();
            this.panel2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.DocApplyGainAccKey)).BeginInit();
            this.panel3.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.DocApplyIVDate)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.DocApplyIVID)).BeginInit();
            this.panel4.SuspendLayout();
            this.SuspendLayout();
            // 
            // btnApply
            // 
            appearance1.Image = global::WinUI.Properties.Resources.OK;
            this.btnApply.Appearance = appearance1;
            this.btnApply.Font = new System.Drawing.Font("Calibri", 9.75F, System.Drawing.FontStyle.Italic, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnApply.ImageSize = new System.Drawing.Size(14, 14);
            this.btnApply.Location = new System.Drawing.Point(12, 233);
            this.btnApply.Name = "btnApply";
            this.btnApply.Size = new System.Drawing.Size(80, 25);
            this.btnApply.TabIndex = 3;
            this.btnApply.Text = "Apply";
            this.btnApply.TextRenderingMode = Infragistics.Win.TextRenderingMode.GDI;
            this.btnApply.Click += new System.EventHandler(this.btnApply_Click);
            // 
            // btnClose
            // 
            appearance2.Image = global::WinUI.Properties.Resources.Close_16;
            this.btnClose.Appearance = appearance2;
            this.btnClose.ButtonStyle = Infragistics.Win.UIElementButtonStyle.WindowsVistaButton;
            this.btnClose.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btnClose.Font = new System.Drawing.Font("Calibri", 9.75F, System.Drawing.FontStyle.Italic, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnClose.ImageSize = new System.Drawing.Size(14, 14);
            this.btnClose.Location = new System.Drawing.Point(280, 233);
            this.btnClose.Name = "btnClose";
            this.btnClose.Size = new System.Drawing.Size(80, 25);
            this.btnClose.TabIndex = 5;
            this.btnClose.Text = "Close";
            this.btnClose.TextRenderingMode = Infragistics.Win.TextRenderingMode.GDI;
            this.btnClose.Click += new System.EventHandler(this.btnClose_Click);
            // 
            // ultraLabel7
            // 
            appearance3.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            appearance3.TextVAlignAsString = "Middle";
            this.ultraLabel7.Appearance = appearance3;
            this.ultraLabel7.Font = new System.Drawing.Font("Calibri", 10F, System.Drawing.FontStyle.Italic);
            this.ultraLabel7.Location = new System.Drawing.Point(8, 12);
            this.ultraLabel7.Name = "ultraLabel7";
            this.ultraLabel7.Size = new System.Drawing.Size(113, 23);
            this.ultraLabel7.TabIndex = 0;
            this.ultraLabel7.Text = "Gain Account ID";
            this.ultraLabel7.TextRenderingMode = Infragistics.Win.TextRenderingMode.GDI;
            // 
            // ultraLabel5
            // 
            appearance4.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            appearance4.TextVAlignAsString = "Middle";
            this.ultraLabel5.Appearance = appearance4;
            this.ultraLabel5.Font = new System.Drawing.Font("Calibri", 10F, System.Drawing.FontStyle.Italic);
            this.ultraLabel5.Location = new System.Drawing.Point(8, 10);
            this.ultraLabel5.Name = "ultraLabel5";
            this.ultraLabel5.Size = new System.Drawing.Size(113, 23);
            this.ultraLabel5.TabIndex = 0;
            this.ultraLabel5.Text = "Document Source";
            this.ultraLabel5.TextRenderingMode = Infragistics.Win.TextRenderingMode.GDI;
            // 
            // DocumentSource
            // 
            appearance5.BorderColor = System.Drawing.Color.LightGray;
            this.DocumentSource.Appearance = appearance5;
            this.DocumentSource.AutoCompleteMode = Infragistics.Win.AutoCompleteMode.Append;
            this.DocumentSource.ComboIsDirty = false;
            this.DocumentSource.DisplayLayout.ScrollStyle = Infragistics.Win.UltraWinGrid.ScrollStyle.Immediate;
            this.DocumentSource.DisplayStyle = Infragistics.Win.EmbeddableElementDisplayStyle.Office2007;
            this.DocumentSource.Font = new System.Drawing.Font("Calibri", 10F);
            this.DocumentSource.ItemMatchingMode = Infragistics.Win.ValueListItemMatchingMode.DoNotConvertDataValueToString;
            this.DocumentSource.Location = new System.Drawing.Point(128, 11);
            this.DocumentSource.Name = "DocumentSource";
            this.DocumentSource.Size = new System.Drawing.Size(204, 25);
            this.DocumentSource.TabIndex = 0;
            this.DocumentSource.TextRenderingMode = Infragistics.Win.TextRenderingMode.GDI;
            this.DocumentSource.UserInputText = "";
            this.DocumentSource.ItemNotInList += new Infragistics.Win.UltraWinGrid.ItemNotInListEventHandler(this.Combo_NotInList);
            this.DocumentSource.CustomUpdate += new System.ComponentModel.CancelEventHandler(this.DocumentSource_CustomUpdate);
            // 
            // ultraLabel1
            // 
            appearance6.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            appearance6.TextVAlignAsString = "Middle";
            this.ultraLabel1.Appearance = appearance6;
            this.ultraLabel1.Font = new System.Drawing.Font("Calibri", 10F, System.Drawing.FontStyle.Italic);
            this.ultraLabel1.Location = new System.Drawing.Point(8, 35);
            this.ultraLabel1.Name = "ultraLabel1";
            this.ultraLabel1.Size = new System.Drawing.Size(113, 23);
            this.ultraLabel1.TabIndex = 2;
            this.ultraLabel1.Text = "Gain Account Des";
            this.ultraLabel1.TextRenderingMode = Infragistics.Win.TextRenderingMode.GDI;
            // 
            // DocApplyGainAccDes
            // 
            appearance7.BorderColor = System.Drawing.Color.LightGray;
            this.DocApplyGainAccDes.Appearance = appearance7;
            appearance8.Image = global::WinUI.Properties.Resources.open3;
            appearance8.ImageHAlign = Infragistics.Win.HAlign.Right;
            editorButton1.Appearance = appearance8;
            editorButton1.ButtonStyle = Infragistics.Win.UIElementButtonStyle.Office2007ScrollbarButton;
            this.DocApplyGainAccDes.ButtonsRight.Add(editorButton1);
            this.DocApplyGainAccDes.Font = new System.Drawing.Font("Calibri", 10F);
            this.DocApplyGainAccDes.Format = "";
            this.DocApplyGainAccDes.IsDirty = false;
            this.DocApplyGainAccDes.IsEmailTextBox = false;
            this.DocApplyGainAccDes.Location = new System.Drawing.Point(128, 34);
            this.DocApplyGainAccDes.Multiline = true;
            this.DocApplyGainAccDes.Name = "DocApplyGainAccDes";
            this.DocApplyGainAccDes.Size = new System.Drawing.Size(204, 22);
            this.DocApplyGainAccDes.TabIndex = 1;
            this.DocApplyGainAccDes.TextRenderingMode = Infragistics.Win.TextRenderingMode.GDI;
            this.DocApplyGainAccDes.CustomUpdate += new System.ComponentModel.CancelEventHandler(this.DocApplyGainAccDes_CustomUpdate);
            this.DocApplyGainAccDes.EditorButtonClick += new Infragistics.Win.UltraWinEditors.EditorButtonEventHandler(this.DocApplyGainAccDes_EditorButtonClick);
            // 
            // DocApplyGainAmt
            // 
            appearance9.BackColor = System.Drawing.Color.White;
            appearance9.BorderColor = System.Drawing.Color.LightGray;
            appearance9.TextHAlignAsString = "Right";
            this.DocApplyGainAmt.Appearance = appearance9;
            this.DocApplyGainAmt.BackColor = System.Drawing.Color.White;
            this.DocApplyGainAmt.DisplayStyle = Infragistics.Win.EmbeddableElementDisplayStyle.Office2007;
            this.DocApplyGainAmt.Enabled = false;
            this.DocApplyGainAmt.Font = new System.Drawing.Font("Calibri", 10F);
            this.DocApplyGainAmt.ForceExitByRestoreValue = false;
            this.DocApplyGainAmt.Format = "#,##0.00\r\n";
            this.DocApplyGainAmt.Location = new System.Drawing.Point(128, 30);
            this.DocApplyGainAmt.MaxLength = 50;
            this.DocApplyGainAmt.Multiline = true;
            this.DocApplyGainAmt.Name = "DocApplyGainAmt";
            this.DocApplyGainAmt.Nullable = false;
            this.DocApplyGainAmt.NumberType = TAUtil.NumericType.Decimal;
            this.DocApplyGainAmt.ReadOnly = true;
            this.DocApplyGainAmt.Size = new System.Drawing.Size(204, 23);
            this.DocApplyGainAmt.TabIndex = 1;
            this.DocApplyGainAmt.Text = "0.00";
            this.DocApplyGainAmt.TextRenderingMode = Infragistics.Win.TextRenderingMode.GDI;
            this.DocApplyGainAmt.ZeroIfEmpty = false;
            // 
            // ultraLabel84
            // 
            appearance10.BackColor = System.Drawing.Color.Transparent;
            appearance10.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            appearance10.TextVAlignAsString = "Middle";
            this.ultraLabel84.Appearance = appearance10;
            this.ultraLabel84.Font = new System.Drawing.Font("Calibri", 10F, System.Drawing.FontStyle.Italic);
            this.ultraLabel84.Location = new System.Drawing.Point(16, 30);
            this.ultraLabel84.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.ultraLabel84.Name = "ultraLabel84";
            this.ultraLabel84.Size = new System.Drawing.Size(104, 23);
            this.ultraLabel84.TabIndex = 2;
            this.ultraLabel84.Text = "Apply Gain Amt";
            this.ultraLabel84.TextRenderingMode = Infragistics.Win.TextRenderingMode.GDI;
            // 
            // ultraLabel4
            // 
            appearance11.BackColor = System.Drawing.Color.Transparent;
            appearance11.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            appearance11.TextVAlignAsString = "Middle";
            this.ultraLabel4.Appearance = appearance11;
            this.ultraLabel4.Font = new System.Drawing.Font("Calibri", 10F, System.Drawing.FontStyle.Italic);
            this.ultraLabel4.Location = new System.Drawing.Point(16, 7);
            this.ultraLabel4.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.ultraLabel4.Name = "ultraLabel4";
            this.ultraLabel4.Size = new System.Drawing.Size(104, 23);
            this.ultraLabel4.TabIndex = 0;
            this.ultraLabel4.Text = "Apply IV ID";
            this.ultraLabel4.TextRenderingMode = Infragistics.Win.TextRenderingMode.GDI;
            // 
            // panel2
            // 
            this.panel2.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(243)))), ((int)(((byte)(234)))), ((int)(((byte)(226)))));
            this.panel2.Controls.Add(this.DocApplyGainAccKey);
            this.panel2.Controls.Add(this.ultraLabel7);
            this.panel2.Controls.Add(this.ultraLabel1);
            this.panel2.Controls.Add(this.DocApplyGainAccDes);
            this.panel2.Location = new System.Drawing.Point(12, 159);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(346, 68);
            this.panel2.TabIndex = 2;
            // 
            // DocApplyGainAccKey
            // 
            appearance12.BackColor = System.Drawing.Color.White;
            appearance12.BorderColor = System.Drawing.Color.LightGray;
            appearance12.FontData.Name = "Calibri";
            appearance12.FontData.SizeInPoints = 11F;
            appearance12.ForeColor = System.Drawing.Color.Black;
            this.DocApplyGainAccKey.Appearance = appearance12;
            this.DocApplyGainAccKey.AutoCompleteMode = Infragistics.Win.AutoCompleteMode.Append;
            this.DocApplyGainAccKey.AutoSize = false;
            editorButton2.Visible = false;
            this.DocApplyGainAccKey.ButtonsRight.Add(editorButton2);
            this.DocApplyGainAccKey.ComboIsDirty = false;
            this.DocApplyGainAccKey.DisplayLayout.ScrollStyle = Infragistics.Win.UltraWinGrid.ScrollStyle.Immediate;
            this.DocApplyGainAccKey.DisplayStyle = Infragistics.Win.EmbeddableElementDisplayStyle.Office2007;
            this.DocApplyGainAccKey.Font = new System.Drawing.Font("Calibri", 10F);
            this.DocApplyGainAccKey.ItemMatchingMode = Infragistics.Win.ValueListItemMatchingMode.DoNotConvertDataValueToString;
            this.DocApplyGainAccKey.Location = new System.Drawing.Point(128, 11);
            this.DocApplyGainAccKey.Name = "DocApplyGainAccKey";
            this.DocApplyGainAccKey.Size = new System.Drawing.Size(204, 23);
            this.DocApplyGainAccKey.TabIndex = 0;
            this.DocApplyGainAccKey.TextRenderingMode = Infragistics.Win.TextRenderingMode.GDI;
            this.DocApplyGainAccKey.UserInputText = "";
            this.DocApplyGainAccKey.ItemNotInList += new Infragistics.Win.UltraWinGrid.ItemNotInListEventHandler(this.Combo_NotInListAdd);
            this.DocApplyGainAccKey.EditorButtonClick += new Infragistics.Win.UltraWinEditors.EditorButtonEventHandler(this.DocApplyGainAccKey_EditorButtonClick);
            this.DocApplyGainAccKey.CustomUpdate += new System.ComponentModel.CancelEventHandler(this.DocApplyGainAccKey_CustomUpdate);
            // 
            // panel3
            // 
            this.panel3.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(243)))), ((int)(((byte)(234)))), ((int)(((byte)(226)))));
            this.panel3.Controls.Add(this.DocApplyIVDate);
            this.panel3.Controls.Add(this.DocApplyGainAmt);
            this.panel3.Controls.Add(this.ultraLabel3);
            this.panel3.Controls.Add(this.DocApplyIVID);
            this.panel3.Controls.Add(this.ultraLabel84);
            this.panel3.Controls.Add(this.ultraLabel4);
            this.panel3.Location = new System.Drawing.Point(12, 60);
            this.panel3.Name = "panel3";
            this.panel3.Size = new System.Drawing.Size(346, 93);
            this.panel3.TabIndex = 1;
            // 
            // DocApplyIVDate
            // 
            appearance13.BackColor = System.Drawing.Color.White;
            appearance13.BorderColor = System.Drawing.Color.LightGray;
            appearance13.TextHAlignAsString = "Right";
            this.DocApplyIVDate.Appearance = appearance13;
            this.DocApplyIVDate.BackColor = System.Drawing.Color.White;
            appearance14.Image = global::WinUI.Properties.Resources.calendar3;
            appearance14.ImageHAlign = Infragistics.Win.HAlign.Right;
            editorButton3.Appearance = appearance14;
            editorButton3.ButtonStyle = Infragistics.Win.UIElementButtonStyle.FlatBorderless;
            this.DocApplyIVDate.ButtonsRight.Add(editorButton3);
            this.DocApplyIVDate.calendarContainer = null;
            this.DocApplyIVDate.DateValue = null;
            this.DocApplyIVDate.DisplayStyle = Infragistics.Win.EmbeddableElementDisplayStyle.Office2007;
            this.DocApplyIVDate.Font = new System.Drawing.Font("Calibri", 10F);
            this.DocApplyIVDate.Format = "";
            this.DocApplyIVDate.Location = new System.Drawing.Point(128, 53);
            this.DocApplyIVDate.MaxLength = 20;
            this.DocApplyIVDate.Name = "DocApplyIVDate";
            this.DocApplyIVDate.ReadOnly = true;
            this.DocApplyIVDate.Size = new System.Drawing.Size(204, 25);
            this.DocApplyIVDate.TabIndex = 2;
            this.DocApplyIVDate.TextRenderingMode = Infragistics.Win.TextRenderingMode.GDI;
            // 
            // ultraLabel3
            // 
            appearance15.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            appearance15.TextVAlignAsString = "Middle";
            this.ultraLabel3.Appearance = appearance15;
            this.ultraLabel3.Font = new System.Drawing.Font("Calibri", 10F, System.Drawing.FontStyle.Italic);
            this.ultraLabel3.Location = new System.Drawing.Point(16, 53);
            this.ultraLabel3.Name = "ultraLabel3";
            this.ultraLabel3.Size = new System.Drawing.Size(65, 23);
            this.ultraLabel3.TabIndex = 4;
            this.ultraLabel3.Text = "Doc Date";
            this.ultraLabel3.TextRenderingMode = Infragistics.Win.TextRenderingMode.GDI;
            // 
            // DocApplyIVID
            // 
            appearance16.BorderColor = System.Drawing.Color.LightGray;
            this.DocApplyIVID.Appearance = appearance16;
            appearance17.Image = global::WinUI.Properties.Resources.open3;
            appearance17.ImageHAlign = Infragistics.Win.HAlign.Right;
            editorButton4.Appearance = appearance17;
            editorButton4.ButtonStyle = Infragistics.Win.UIElementButtonStyle.Office2007ScrollbarButton;
            this.DocApplyIVID.ButtonsRight.Add(editorButton4);
            this.DocApplyIVID.Font = new System.Drawing.Font("Calibri", 10F);
            this.DocApplyIVID.Format = "";
            this.DocApplyIVID.IsDirty = false;
            this.DocApplyIVID.IsEmailTextBox = false;
            this.DocApplyIVID.Location = new System.Drawing.Point(128, 7);
            this.DocApplyIVID.Multiline = true;
            this.DocApplyIVID.Name = "DocApplyIVID";
            this.DocApplyIVID.ReadOnly = true;
            this.DocApplyIVID.Size = new System.Drawing.Size(204, 23);
            this.DocApplyIVID.TabIndex = 0;
            this.DocApplyIVID.TextRenderingMode = Infragistics.Win.TextRenderingMode.GDI;
            this.DocApplyIVID.EditorButtonClick += new Infragistics.Win.UltraWinEditors.EditorButtonEventHandler(this.DocApplyIVID_EditorButtonClick);
            // 
            // panel4
            // 
            this.panel4.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(243)))), ((int)(((byte)(234)))), ((int)(((byte)(226)))));
            this.panel4.Controls.Add(this.DocumentSource);
            this.panel4.Controls.Add(this.ultraLabel5);
            this.panel4.Location = new System.Drawing.Point(12, 6);
            this.panel4.Name = "panel4";
            this.panel4.Size = new System.Drawing.Size(346, 48);
            this.panel4.TabIndex = 0;
            // 
            // btnDelete
            // 
            appearance18.Image = global::WinUI.Properties.Resources.Delete_16;
            this.btnDelete.Appearance = appearance18;
            this.btnDelete.Font = new System.Drawing.Font("Calibri", 9.75F, System.Drawing.FontStyle.Italic, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnDelete.ImageSize = new System.Drawing.Size(14, 14);
            this.btnDelete.Location = new System.Drawing.Point(98, 233);
            this.btnDelete.Name = "btnDelete";
            this.btnDelete.Size = new System.Drawing.Size(80, 25);
            this.btnDelete.TabIndex = 4;
            this.btnDelete.Text = "Delete";
            this.btnDelete.TextRenderingMode = Infragistics.Win.TextRenderingMode.GDI;
            this.btnDelete.Click += new System.EventHandler(this.btnDelete_Click);
            // 
            // frmApplyIV
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.White;
            this.ClientSize = new System.Drawing.Size(369, 267);
            this.Controls.Add(this.btnDelete);
            this.Controls.Add(this.panel4);
            this.Controls.Add(this.panel3);
            this.Controls.Add(this.panel2);
            this.Controls.Add(this.btnApply);
            this.Controls.Add(this.btnClose);
            this.KeyPreview = true;
            this.MaximizeBox = false;
            this.Name = "frmApplyIV";
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Tag = "frmApplyIV";
            this.Text = "Apply IV";
            this.Load += new System.EventHandler(this.frmApplyIV_Load);
            this.KeyDown += new System.Windows.Forms.KeyEventHandler(this.frmApplyIV_KeyDown);
            ((System.ComponentModel.ISupportInitialize)(this.DocumentSource)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.DocApplyGainAccDes)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.DocApplyGainAmt)).EndInit();
            this.panel2.ResumeLayout(false);
            this.panel2.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.DocApplyGainAccKey)).EndInit();
            this.panel3.ResumeLayout(false);
            this.panel3.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.DocApplyIVDate)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.DocApplyIVID)).EndInit();
            this.panel4.ResumeLayout(false);
            this.panel4.PerformLayout();
            this.ResumeLayout(false);

        }        
        

        #endregion

        private Infragistics.Win.Misc.UltraButton btnApply;
        private Infragistics.Win.Misc.UltraButton btnClose;
        private Infragistics.Win.Misc.UltraLabel ultraLabel7;
        private Infragistics.Win.Misc.UltraLabel ultraLabel5;
        private TAUtil.TAComboBox DocumentSource;
        private Infragistics.Win.Misc.UltraLabel ultraLabel1;
        private TAUtil.TATextBoxEditor DocApplyGainAccDes;
        private TAUtil.TANumericEditor DocApplyGainAmt;
        private Infragistics.Win.Misc.UltraLabel ultraLabel84;
        private Infragistics.Win.Misc.UltraLabel ultraLabel4;
        private System.Windows.Forms.Panel panel2;
        private System.Windows.Forms.Panel panel3;
        private System.Windows.Forms.Panel panel4;
        private TAUtil.TAComboBox DocApplyGainAccKey;
        private TAUtil.TADateEditor DocApplyIVDate;
        private Infragistics.Win.Misc.UltraLabel ultraLabel3;
        private TAUtil.TATextBoxEditor DocApplyIVID;
        private Infragistics.Win.Misc.UltraButton btnDelete;
    }
}