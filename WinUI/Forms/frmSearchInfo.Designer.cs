namespace WinUI
{
    partial class frmSearchInfo
    {
        internal TAUtil.TACheckBoxEditor chkMatchCase;
        internal System.Windows.Forms.Label lblSearchDirection;
        internal System.Windows.Forms.Label lblMatch;
        internal System.Windows.Forms.Label lblLookIn;

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
            Infragistics.Win.Appearance appearance6 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance4 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance5 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance3 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance2 = new Infragistics.Win.Appearance();
            this.chkMatchCase = new TAUtil.TACheckBoxEditor();
            this.lblSearchDirection = new System.Windows.Forms.Label();
            this.lblMatch = new System.Windows.Forms.Label();
            this.lblLookIn = new System.Windows.Forms.Label();
            this.btnCancel = new Infragistics.Win.Misc.UltraButton();
            this.cmdFindNext = new Infragistics.Win.Misc.UltraButton();
            this.ulblAccGrpID = new Infragistics.Win.Misc.UltraLabel();
            this.cboLookIn = new TAUtil.TAComboBox();
            this.cboSearchDirection = new TAUtil.TAComboBox();
            this.cboMatch = new TAUtil.TAComboBox();
            this.txtFindWhat = new TAUtil.TATextBoxEditor();
            this.panel1 = new System.Windows.Forms.Panel();
            ((System.ComponentModel.ISupportInitialize)(this.cboLookIn)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.cboSearchDirection)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.cboMatch)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtFindWhat)).BeginInit();
            this.panel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // chkMatchCase
            // 
            appearance1.FontData.ItalicAsString = "True";
            appearance1.FontData.Name = "Calibri";
            appearance1.FontData.SizeInPoints = 10F;
            appearance1.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            this.chkMatchCase.Appearance = appearance1;
            this.chkMatchCase.cancelUpdate = false;
            this.chkMatchCase.Location = new System.Drawing.Point(299, 158);
            this.chkMatchCase.Name = "chkMatchCase";
            this.chkMatchCase.Size = new System.Drawing.Size(96, 24);
            this.chkMatchCase.TabIndex = 4;
            this.chkMatchCase.Text = "Match Case";
            this.chkMatchCase.TextRenderingMode = Infragistics.Win.TextRenderingMode.GDI;
            // 
            // lblSearchDirection
            // 
            this.lblSearchDirection.AutoSize = true;
            this.lblSearchDirection.Font = new System.Drawing.Font("Calibri", 10F, System.Drawing.FontStyle.Italic);
            this.lblSearchDirection.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            this.lblSearchDirection.Location = new System.Drawing.Point(17, 126);
            this.lblSearchDirection.Name = "lblSearchDirection";
            this.lblSearchDirection.Size = new System.Drawing.Size(50, 17);
            this.lblSearchDirection.TabIndex = 19;
            this.lblSearchDirection.Text = "Search:";
            // 
            // lblMatch
            // 
            this.lblMatch.AutoSize = true;
            this.lblMatch.Font = new System.Drawing.Font("Calibri", 10F, System.Drawing.FontStyle.Italic);
            this.lblMatch.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            this.lblMatch.Location = new System.Drawing.Point(17, 98);
            this.lblMatch.Name = "lblMatch";
            this.lblMatch.Size = new System.Drawing.Size(49, 17);
            this.lblMatch.TabIndex = 17;
            this.lblMatch.Text = "Match:";
            // 
            // lblLookIn
            // 
            this.lblLookIn.AutoSize = true;
            this.lblLookIn.Font = new System.Drawing.Font("Calibri", 10F, System.Drawing.FontStyle.Italic);
            this.lblLookIn.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            this.lblLookIn.Location = new System.Drawing.Point(17, 68);
            this.lblLookIn.Name = "lblLookIn";
            this.lblLookIn.Size = new System.Drawing.Size(52, 17);
            this.lblLookIn.TabIndex = 15;
            this.lblLookIn.Text = "Look In:";
            // 
            // btnCancel
            // 
            appearance6.Image = global::WinUI.Properties.Resources.Cancel;
            this.btnCancel.Appearance = appearance6;
            this.btnCancel.ButtonStyle = Infragistics.Win.UIElementButtonStyle.Office2003ToolbarButton;
            this.btnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btnCancel.Font = new System.Drawing.Font("Calibri", 10F, System.Drawing.FontStyle.Italic);
            this.btnCancel.Location = new System.Drawing.Point(416, 55);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(75, 30);
            this.btnCancel.TabIndex = 6;
            this.btnCancel.Text = "&Cancel";
            this.btnCancel.TextRenderingMode = Infragistics.Win.TextRenderingMode.GDI;
            this.btnCancel.Click += new System.EventHandler(this.cmdCancel_Click);
            // 
            // cmdFindNext
            // 
            this.cmdFindNext.ButtonStyle = Infragistics.Win.UIElementButtonStyle.OfficeXPToolbarButton;
            this.cmdFindNext.Font = new System.Drawing.Font("Calibri", 10F, System.Drawing.FontStyle.Italic);
            this.cmdFindNext.Location = new System.Drawing.Point(416, 19);
            this.cmdFindNext.Name = "cmdFindNext";
            this.cmdFindNext.Size = new System.Drawing.Size(75, 30);
            this.cmdFindNext.TabIndex = 5;
            this.cmdFindNext.Text = "&Find Next";
            this.cmdFindNext.TextRenderingMode = Infragistics.Win.TextRenderingMode.GDI;
            this.cmdFindNext.Click += new System.EventHandler(this.cmdFindNext_Click);
            // 
            // ulblAccGrpID
            // 
            appearance4.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            appearance4.TextVAlignAsString = "Middle";
            this.ulblAccGrpID.Appearance = appearance4;
            this.ulblAccGrpID.Font = new System.Drawing.Font("Calibri", 10F, ((System.Drawing.FontStyle)((System.Drawing.FontStyle.Bold | System.Drawing.FontStyle.Italic))));
            this.ulblAccGrpID.Location = new System.Drawing.Point(17, 17);
            this.ulblAccGrpID.Name = "ulblAccGrpID";
            this.ulblAccGrpID.Size = new System.Drawing.Size(79, 23);
            this.ulblAccGrpID.TabIndex = 38;
            this.ulblAccGrpID.Text = "Find What:";
            this.ulblAccGrpID.TextRenderingMode = Infragistics.Win.TextRenderingMode.GDI;
            // 
            // cboLookIn
            // 
            appearance5.BackColor = System.Drawing.Color.White;
            this.cboLookIn.Appearance = appearance5;
            this.cboLookIn.AutoCompleteMode = Infragistics.Win.AutoCompleteMode.Append;
            this.cboLookIn.CharacterCasing = System.Windows.Forms.CharacterCasing.Normal;
            this.cboLookIn.ComboIsDirty = false;
            this.cboLookIn.DisplayStyle = Infragistics.Win.EmbeddableElementDisplayStyle.Office2007;
            this.cboLookIn.DropDownStyle = Infragistics.Win.UltraWinGrid.UltraComboStyle.DropDown;
            this.cboLookIn.Font = new System.Drawing.Font("Calibri", 11F);
            this.cboLookIn.Location = new System.Drawing.Point(89, 68);
            this.cboLookIn.Name = "cboLookIn";
            this.cboLookIn.Size = new System.Drawing.Size(300, 26);
            this.cboLookIn.TabIndex = 1;
            this.cboLookIn.TextRenderingMode = Infragistics.Win.TextRenderingMode.GDI;
            this.cboLookIn.UserInputText = "";
            this.cboLookIn.ItemNotInList += new Infragistics.Win.UltraWinGrid.ItemNotInListEventHandler(this.Combo_NotInList);
            // 
            // cboSearchDirection
            // 
            appearance3.BackColor = System.Drawing.Color.White;
            this.cboSearchDirection.Appearance = appearance3;
            this.cboSearchDirection.AutoCompleteMode = Infragistics.Win.AutoCompleteMode.Append;
            this.cboSearchDirection.CharacterCasing = System.Windows.Forms.CharacterCasing.Normal;
            this.cboSearchDirection.ComboIsDirty = false;
            this.cboSearchDirection.DisplayStyle = Infragistics.Win.EmbeddableElementDisplayStyle.Office2007;
            this.cboSearchDirection.DropDownStyle = Infragistics.Win.UltraWinGrid.UltraComboStyle.DropDown;
            this.cboSearchDirection.Font = new System.Drawing.Font("Calibri", 11F);
            this.cboSearchDirection.Location = new System.Drawing.Point(89, 124);
            this.cboSearchDirection.Name = "cboSearchDirection";
            this.cboSearchDirection.Size = new System.Drawing.Size(300, 26);
            this.cboSearchDirection.TabIndex = 3;
            this.cboSearchDirection.TextRenderingMode = Infragistics.Win.TextRenderingMode.GDI;
            this.cboSearchDirection.UserInputText = "";
            this.cboSearchDirection.ItemNotInList += new Infragistics.Win.UltraWinGrid.ItemNotInListEventHandler(this.Combo_NotInList);
            // 
            // cboMatch
            // 
            appearance2.BackColor = System.Drawing.Color.White;
            this.cboMatch.Appearance = appearance2;
            this.cboMatch.AutoCompleteMode = Infragistics.Win.AutoCompleteMode.Append;
            this.cboMatch.CharacterCasing = System.Windows.Forms.CharacterCasing.Normal;
            this.cboMatch.ComboIsDirty = false;
            this.cboMatch.DisplayStyle = Infragistics.Win.EmbeddableElementDisplayStyle.Office2007;
            this.cboMatch.DropDownStyle = Infragistics.Win.UltraWinGrid.UltraComboStyle.DropDown;
            this.cboMatch.Font = new System.Drawing.Font("Calibri", 11F);
            this.cboMatch.Location = new System.Drawing.Point(89, 96);
            this.cboMatch.Name = "cboMatch";
            this.cboMatch.Size = new System.Drawing.Size(300, 26);
            this.cboMatch.TabIndex = 2;
            this.cboMatch.TextRenderingMode = Infragistics.Win.TextRenderingMode.GDI;
            this.cboMatch.UserInputText = "";
            this.cboMatch.ItemNotInList += new Infragistics.Win.UltraWinGrid.ItemNotInListEventHandler(this.Combo_NotInList);
            // 
            // txtFindWhat
            // 
            this.txtFindWhat.Font = new System.Drawing.Font("Calibri", 11F);
            this.txtFindWhat.Format = "";
            this.txtFindWhat.IsDirty = false;
            this.txtFindWhat.IsEmailTextBox = false;
            this.txtFindWhat.Location = new System.Drawing.Point(89, 19);
            this.txtFindWhat.Multiline = true;
            this.txtFindWhat.Name = "txtFindWhat";
            this.txtFindWhat.Size = new System.Drawing.Size(300, 21);
            this.txtFindWhat.TabIndex = 0;
            // 
            // panel1
            // 
            this.panel1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.panel1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(243)))), ((int)(((byte)(234)))), ((int)(((byte)(226)))));
            this.panel1.Controls.Add(this.txtFindWhat);
            this.panel1.Controls.Add(this.btnCancel);
            this.panel1.Controls.Add(this.cmdFindNext);
            this.panel1.Controls.Add(this.lblLookIn);
            this.panel1.Controls.Add(this.cboMatch);
            this.panel1.Controls.Add(this.chkMatchCase);
            this.panel1.Controls.Add(this.lblMatch);
            this.panel1.Controls.Add(this.cboSearchDirection);
            this.panel1.Controls.Add(this.lblSearchDirection);
            this.panel1.Controls.Add(this.cboLookIn);
            this.panel1.Controls.Add(this.ulblAccGrpID);
            this.panel1.Location = new System.Drawing.Point(-2, 12);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(536, 186);
            this.panel1.TabIndex = 39;
            // 
            // frmSearchInfo
            // 
            this.AcceptButton = this.cmdFindNext;
            this.AutoScaleBaseSize = new System.Drawing.Size(5, 13);
            this.BackColor = System.Drawing.Color.White;
            this.CancelButton = this.btnCancel;
            this.ClientSize = new System.Drawing.Size(528, 208);
            this.Controls.Add(this.panel1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.KeyPreview = true;
            this.MaximizeBox = false;
            this.Name = "frmSearchInfo";
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "SearchInfo";
            this.Load += new System.EventHandler(this.frmSearchInfo_Load);
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.frmSearchInfo_FormClosing);
            this.KeyDown += new System.Windows.Forms.KeyEventHandler(this.frmSearchInfo_KeyDown);
            ((System.ComponentModel.ISupportInitialize)(this.cboLookIn)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.cboSearchDirection)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.cboMatch)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtFindWhat)).EndInit();
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private Infragistics.Win.Misc.UltraButton btnCancel;
        private Infragistics.Win.Misc.UltraButton cmdFindNext;
        private Infragistics.Win.Misc.UltraLabel ulblAccGrpID;
        private TAUtil.TAComboBox cboLookIn;
        private TAUtil.TAComboBox cboSearchDirection;
        private TAUtil.TAComboBox cboMatch;
        private TAUtil.TATextBoxEditor txtFindWhat;
        private System.Windows.Forms.Panel panel1;
    }
}