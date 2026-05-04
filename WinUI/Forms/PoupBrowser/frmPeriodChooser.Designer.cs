namespace WinUI
{
    partial class frmPeriodChooser
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
            Infragistics.Win.Appearance appearance60 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance4 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance3 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance7 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance6 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance2 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance1 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance5 = new Infragistics.Win.Appearance();
            this.ultraLabel9 = new Infragistics.Win.Misc.UltraLabel();
            this.ultraLabel3 = new Infragistics.Win.Misc.UltraLabel();
            this.ultraLabel2 = new Infragistics.Win.Misc.UltraLabel();
            this.btnCancel = new Infragistics.Win.Misc.UltraButton();
            this.btnOk = new Infragistics.Win.Misc.UltraButton();
            this.Period = new TAUtil.TAComboBox();
            this.PeriodDifference = new TAUtil.TANumericEditor();
            this.PeriodType = new TAUtil.TAComboBox();
            this.panel1 = new System.Windows.Forms.Panel();
            ((System.ComponentModel.ISupportInitialize)(this.Period)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.PeriodDifference)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.PeriodType)).BeginInit();
            this.panel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // ultraLabel9
            // 
            appearance60.BackColor = System.Drawing.Color.Transparent;
            appearance60.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            appearance60.TextVAlignAsString = "Middle";
            this.ultraLabel9.Appearance = appearance60;
            this.ultraLabel9.Font = new System.Drawing.Font("Calibri", 10F, System.Drawing.FontStyle.Italic);
            this.ultraLabel9.Location = new System.Drawing.Point(18, 68);
            this.ultraLabel9.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.ultraLabel9.Name = "ultraLabel9";
            this.ultraLabel9.Size = new System.Drawing.Size(149, 20);
            this.ultraLabel9.TabIndex = 78;
            this.ultraLabel9.Text = "Period";
            this.ultraLabel9.TextRenderingMode = Infragistics.Win.TextRenderingMode.GDI;
            // 
            // ultraLabel3
            // 
            appearance4.BackColor = System.Drawing.Color.Transparent;
            appearance4.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            appearance4.TextVAlignAsString = "Middle";
            this.ultraLabel3.Appearance = appearance4;
            this.ultraLabel3.Font = new System.Drawing.Font("Calibri", 10F, System.Drawing.FontStyle.Italic);
            this.ultraLabel3.Location = new System.Drawing.Point(18, 40);
            this.ultraLabel3.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.ultraLabel3.Name = "ultraLabel3";
            this.ultraLabel3.Size = new System.Drawing.Size(149, 20);
            this.ultraLabel3.TabIndex = 79;
            this.ultraLabel3.Text = "Period Difference:";
            this.ultraLabel3.TextRenderingMode = Infragistics.Win.TextRenderingMode.GDI;
            // 
            // ultraLabel2
            // 
            appearance3.BackColor = System.Drawing.Color.Transparent;
            appearance3.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            appearance3.TextVAlignAsString = "Middle";
            this.ultraLabel2.Appearance = appearance3;
            this.ultraLabel2.Font = new System.Drawing.Font("Calibri", 10F, System.Drawing.FontStyle.Italic);
            this.ultraLabel2.Location = new System.Drawing.Point(18, 11);
            this.ultraLabel2.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.ultraLabel2.Name = "ultraLabel2";
            this.ultraLabel2.Size = new System.Drawing.Size(149, 20);
            this.ultraLabel2.TabIndex = 80;
            this.ultraLabel2.Text = "Period Type:";
            this.ultraLabel2.TextRenderingMode = Infragistics.Win.TextRenderingMode.GDI;
            // 
            // btnCancel
            // 
            appearance7.Image = global::WinUI.Properties.Resources.Cancel_16;
            this.btnCancel.Appearance = appearance7;
            this.btnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btnCancel.Font = new System.Drawing.Font("Calibri", 10F, System.Drawing.FontStyle.Italic);
            this.btnCancel.Location = new System.Drawing.Point(201, 113);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(75, 30);
            this.btnCancel.TabIndex = 83;
            this.btnCancel.Text = "Cancel";
            this.btnCancel.TextRenderingMode = Infragistics.Win.TextRenderingMode.GDI;
            this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
            // 
            // btnOk
            // 
            appearance6.Image = global::WinUI.Properties.Resources.ok_16;
            this.btnOk.Appearance = appearance6;
            this.btnOk.Font = new System.Drawing.Font("Calibri", 10F, System.Drawing.FontStyle.Italic);
            this.btnOk.Location = new System.Drawing.Point(120, 113);
            this.btnOk.Name = "btnOk";
            this.btnOk.Size = new System.Drawing.Size(75, 30);
            this.btnOk.TabIndex = 82;
            this.btnOk.Text = "OK";
            this.btnOk.TextRenderingMode = Infragistics.Win.TextRenderingMode.GDI;
            this.btnOk.Click += new System.EventHandler(this.btnOk_Click);
            // 
            // Period
            // 
            this.Period.AllowNull = Infragistics.Win.DefaultableBoolean.True;
            appearance2.BackColor = System.Drawing.Color.White;
            appearance2.BorderColor = System.Drawing.Color.LightGray;
            appearance2.FontData.Name = "Calibri";
            appearance2.FontData.SizeInPoints = 11F;
            appearance2.ForeColor = System.Drawing.Color.Black;
            this.Period.Appearance = appearance2;
            this.Period.AutoCompleteMode = Infragistics.Win.AutoCompleteMode.Append;
            this.Period.AutoSize = false;
            this.Period.ComboIsDirty = false;
            this.Period.DisplayStyle = Infragistics.Win.EmbeddableElementDisplayStyle.Office2007;
            this.Period.Font = new System.Drawing.Font("Tahoma", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Period.ItemMatchingMode = Infragistics.Win.ValueListItemMatchingMode.DoNotConvertDataValueToString;
            this.Period.LimitToList = true;
            this.Period.Location = new System.Drawing.Point(173, 62);
            this.Period.Name = "Period";
            this.Period.Size = new System.Drawing.Size(200, 25);
            this.Period.TabIndex = 77;
            this.Period.TextRenderingMode = Infragistics.Win.TextRenderingMode.GDI;
            this.Period.UserInputText = "";
            this.Period.ItemNotInList += new Infragistics.Win.UltraWinGrid.ItemNotInListEventHandler(this.Combo_NotInList);
            // 
            // PeriodDifference
            // 
            appearance1.BackColor = System.Drawing.Color.White;
            appearance1.BorderColor = System.Drawing.Color.LightGray;
            appearance1.FontData.Name = "Calibri";
            appearance1.FontData.SizeInPoints = 11F;
            appearance1.ForeColor = System.Drawing.Color.Black;
            appearance1.TextHAlignAsString = "Right";
            this.PeriodDifference.Appearance = appearance1;
            this.PeriodDifference.AutoSize = false;
            this.PeriodDifference.BackColor = System.Drawing.Color.White;
            this.PeriodDifference.DisplayStyle = Infragistics.Win.EmbeddableElementDisplayStyle.Office2007;
            this.PeriodDifference.ForceExitByRestoreValue = false;
            this.PeriodDifference.Format = "0";
            this.PeriodDifference.Location = new System.Drawing.Point(173, 37);
            this.PeriodDifference.Name = "PeriodDifference";
            this.PeriodDifference.NumberType = TAUtil.NumericType.Integer16Bit;
            this.PeriodDifference.Size = new System.Drawing.Size(200, 25);
            this.PeriodDifference.TabIndex = 81;
            this.PeriodDifference.Text = "0";
            this.PeriodDifference.TextRenderingMode = Infragistics.Win.TextRenderingMode.GDI;
            this.PeriodDifference.ZeroIfEmpty = false;
            // 
            // PeriodType
            // 
            this.PeriodType.AllowNull = Infragistics.Win.DefaultableBoolean.False;
            appearance5.BackColor = System.Drawing.Color.White;
            appearance5.BorderColor = System.Drawing.Color.LightGray;
            appearance5.FontData.Name = "Calibri";
            appearance5.FontData.SizeInPoints = 11F;
            appearance5.ForeColor = System.Drawing.Color.Black;
            this.PeriodType.Appearance = appearance5;
            this.PeriodType.AutoCompleteMode = Infragistics.Win.AutoCompleteMode.Append;
            this.PeriodType.AutoSize = false;
            this.PeriodType.ComboIsDirty = false;
            this.PeriodType.DisplayStyle = Infragistics.Win.EmbeddableElementDisplayStyle.Office2007;
            this.PeriodType.DropDownStyle = Infragistics.Win.UltraWinGrid.UltraComboStyle.DropDown;
            this.PeriodType.Font = new System.Drawing.Font("Tahoma", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.PeriodType.ItemMatchingMode = Infragistics.Win.ValueListItemMatchingMode.DoNotConvertDataValueToString;
            this.PeriodType.LimitToList = true;
            this.PeriodType.Location = new System.Drawing.Point(173, 12);
            this.PeriodType.Name = "PeriodType";
            this.PeriodType.Size = new System.Drawing.Size(200, 25);
            this.PeriodType.TabIndex = 76;
            this.PeriodType.TextRenderingMode = Infragistics.Win.TextRenderingMode.GDI;
            this.PeriodType.UserInputText = "";
            this.PeriodType.ItemNotInList += new Infragistics.Win.UltraWinGrid.ItemNotInListEventHandler(this.Combo_NotInList);
            this.PeriodType.CustomUpdate += new System.ComponentModel.CancelEventHandler(this.PeriodType_CustomUpdate);
            // 
            // panel1
            // 
            this.panel1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(243)))), ((int)(((byte)(234)))), ((int)(((byte)(226)))));
            this.panel1.Controls.Add(this.ultraLabel2);
            this.panel1.Controls.Add(this.btnCancel);
            this.panel1.Controls.Add(this.ultraLabel3);
            this.panel1.Controls.Add(this.btnOk);
            this.panel1.Controls.Add(this.ultraLabel9);
            this.panel1.Controls.Add(this.Period);
            this.panel1.Controls.Add(this.PeriodType);
            this.panel1.Controls.Add(this.PeriodDifference);
            this.panel1.Location = new System.Drawing.Point(12, 7);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(396, 163);
            this.panel1.TabIndex = 84;
            // 
            // frmPeriodChooser
            // 
            this.AcceptButton = this.btnOk;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.White;
            this.CancelButton = this.btnCancel;
            this.ClientSize = new System.Drawing.Size(420, 187);
            this.Controls.Add(this.panel1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "frmPeriodChooser";
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Tag = "frmPeriodChooser";
            this.Text = "frmPeriodChooser";
            this.Load += new System.EventHandler(this.frmPeriodChooser_Load);
            this.KeyDown += new System.Windows.Forms.KeyEventHandler(this.frmPeriodChooser_KeyDown);
            ((System.ComponentModel.ISupportInitialize)(this.Period)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.PeriodDifference)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.PeriodType)).EndInit();
            this.panel1.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private TAUtil.TAComboBox Period;
        private TAUtil.TANumericEditor PeriodDifference;
        private TAUtil.TAComboBox PeriodType;
        private Infragistics.Win.Misc.UltraLabel ultraLabel9;
        private Infragistics.Win.Misc.UltraLabel ultraLabel3;
        private Infragistics.Win.Misc.UltraLabel ultraLabel2;
        private Infragistics.Win.Misc.UltraButton btnCancel;
        private Infragistics.Win.Misc.UltraButton btnOk;
        private System.Windows.Forms.Panel panel1;
    }
}