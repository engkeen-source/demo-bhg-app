namespace WinUI
{
    partial class frmDOIVConfirm
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
            Infragistics.Win.UltraWinGrid.UltraGridBand ultraGridBand1 = new Infragistics.Win.UltraWinGrid.UltraGridBand("", -1);
            Infragistics.Win.UltraWinGrid.UltraGridColumn ultraGridColumn1 = new Infragistics.Win.UltraWinGrid.UltraGridColumn("DocID", 0);
            Infragistics.Win.UltraWinGrid.UltraGridColumn ultraGridColumn2 = new Infragistics.Win.UltraWinGrid.UltraGridColumn("DoDate", 1);
            Infragistics.Win.UltraWinGrid.UltraGridColumn ultraGridColumn3 = new Infragistics.Win.UltraWinGrid.UltraGridColumn("DocConNm", 2);
            Infragistics.Win.Appearance appearance2 = new Infragistics.Win.Appearance();
            Infragistics.Win.UltraWinEditors.EditorButton editorButton1 = new Infragistics.Win.UltraWinEditors.EditorButton();
            Infragistics.Win.Appearance appearance1 = new Infragistics.Win.Appearance();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.tagrdDeliveryOrder = new TAUtil.TAGridEditor();
            this.btnOK = new System.Windows.Forms.Button();
            this.btnCancel = new System.Windows.Forms.Button();
            this.IVDate = new TAUtil.TADateEditor();
            this.panel1 = new System.Windows.Forms.Panel();
            this.panel2 = new System.Windows.Forms.Panel();
            ((System.ComponentModel.ISupportInitialize)(this.tagrdDeliveryOrder)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.IVDate)).BeginInit();
            this.panel1.SuspendLayout();
            this.panel2.SuspendLayout();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Calibri", 10F, System.Drawing.FontStyle.Italic, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            this.label1.Location = new System.Drawing.Point(5, 6);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(339, 17);
            this.label1.TabIndex = 0;
            this.label1.Text = "The following Delivery Order has been selected for Transfer";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("Calibri", 10F, System.Drawing.FontStyle.Italic, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label2.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            this.label2.Location = new System.Drawing.Point(5, 31);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(240, 17);
            this.label2.TabIndex = 1;
            this.label2.Text = "The new Invoice generated will be dated:";
            // 
            // tagrdDeliveryOrder
            // 
            this.tagrdDeliveryOrder.AutoAddNewRow = false;
            this.tagrdDeliveryOrder.AutoUseCustomControlsInCells = false;
            this.tagrdDeliveryOrder.DefaultValue = null;
            this.tagrdDeliveryOrder.DetailObjectKey = 0;
            ultraGridColumn1.AutoCompleteMode = Infragistics.Win.AutoCompleteMode.Append;
            ultraGridColumn1.Header.VisiblePosition = 0;
            ultraGridColumn1.Width = 100;
            ultraGridColumn2.AutoCompleteMode = Infragistics.Win.AutoCompleteMode.Append;
            ultraGridColumn2.Header.VisiblePosition = 1;
            ultraGridColumn2.Width = 100;
            ultraGridColumn3.AutoCompleteMode = Infragistics.Win.AutoCompleteMode.Append;
            ultraGridColumn3.Header.VisiblePosition = 2;
            ultraGridColumn3.Width = 240;
            ultraGridBand1.Columns.AddRange(new object[] {
            ultraGridColumn1,
            ultraGridColumn2,
            ultraGridColumn3});
            this.tagrdDeliveryOrder.DisplayLayout.BandsSerializer.Add(ultraGridBand1);
            this.tagrdDeliveryOrder.DisplayLayout.CaptionVisible = Infragistics.Win.DefaultableBoolean.False;
            this.tagrdDeliveryOrder.DisplayLayout.Override.FixedHeaderIndicator = Infragistics.Win.UltraWinGrid.FixedHeaderIndicator.None;
            this.tagrdDeliveryOrder.DisplayLayout.Override.SelectTypeCell = Infragistics.Win.UltraWinGrid.SelectType.Single;
            this.tagrdDeliveryOrder.DisplayLayout.Override.SelectTypeRow = Infragistics.Win.UltraWinGrid.SelectType.Single;
            this.tagrdDeliveryOrder.DisplayLayout.ScrollBounds = Infragistics.Win.UltraWinGrid.ScrollBounds.ScrollToFill;
            this.tagrdDeliveryOrder.DisplayLayout.ScrollStyle = Infragistics.Win.UltraWinGrid.ScrollStyle.Immediate;
            this.tagrdDeliveryOrder.DisplayLayout.UseFixedHeaders = true;
            this.tagrdDeliveryOrder.EnterKeyDirection = TAUtil.EnterKeyDirectionEnum.NextCellByTab;
            this.tagrdDeliveryOrder.Font = new System.Drawing.Font("Tahoma", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.tagrdDeliveryOrder.HeaderObjectKey = null;
            this.tagrdDeliveryOrder.Location = new System.Drawing.Point(11, 82);
            this.tagrdDeliveryOrder.Name = "tagrdDeliveryOrder";
            this.tagrdDeliveryOrder.RowUpdateCancelAction = Infragistics.Win.UltraWinGrid.RowUpdateCancelAction.RetainDataAndActivation;
            this.tagrdDeliveryOrder.Size = new System.Drawing.Size(648, 148);
            this.tagrdDeliveryOrder.TabIndex = 0;
            this.tagrdDeliveryOrder.Text = "taGridEditor1";
            this.tagrdDeliveryOrder.UseFlatMode = Infragistics.Win.DefaultableBoolean.True;
            this.tagrdDeliveryOrder.UseOsThemes = Infragistics.Win.DefaultableBoolean.False;
            // 
            // btnOK
            // 
            this.btnOK.Font = new System.Drawing.Font("Calibri", 10F, ((System.Drawing.FontStyle)((System.Drawing.FontStyle.Bold | System.Drawing.FontStyle.Italic))), System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnOK.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            this.btnOK.Image = global::WinUI.Properties.Resources.ok_16;
            this.btnOK.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnOK.Location = new System.Drawing.Point(242, 5);
            this.btnOK.Name = "btnOK";
            this.btnOK.Size = new System.Drawing.Size(85, 29);
            this.btnOK.TabIndex = 0;
            this.btnOK.Text = "OK";
            this.btnOK.UseVisualStyleBackColor = true;
            this.btnOK.Click += new System.EventHandler(this.btnOK_Click);
            // 
            // btnCancel
            // 
            this.btnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btnCancel.Font = new System.Drawing.Font("Calibri", 10F, ((System.Drawing.FontStyle)((System.Drawing.FontStyle.Bold | System.Drawing.FontStyle.Italic))), System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnCancel.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            this.btnCancel.Image = global::WinUI.Properties.Resources.Cancel_16;
            this.btnCancel.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnCancel.Location = new System.Drawing.Point(333, 5);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(72, 29);
            this.btnCancel.TabIndex = 1;
            this.btnCancel.Text = "&Cancel";
            this.btnCancel.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.btnCancel.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
            this.btnCancel.UseVisualStyleBackColor = true;
            this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
            // 
            // IVDate
            // 
            appearance2.BorderColor = System.Drawing.Color.LightGray;
            appearance2.TextHAlignAsString = "Right";
            this.IVDate.Appearance = appearance2;
            this.IVDate.AutoSize = false;
            appearance1.Image = global::WinUI.Properties.Resources.calendar3;
            appearance1.ImageHAlign = Infragistics.Win.HAlign.Right;
            editorButton1.Appearance = appearance1;
            editorButton1.ButtonStyle = Infragistics.Win.UIElementButtonStyle.FlatBorderless;
            this.IVDate.ButtonsRight.Add(editorButton1);
            this.IVDate.calendarContainer = null;
            this.IVDate.DateValue = null;
            this.IVDate.DisplayStyle = Infragistics.Win.EmbeddableElementDisplayStyle.Office2007;
            this.IVDate.Font = new System.Drawing.Font("Calibri", 11F);
            this.IVDate.Format = "";
            this.IVDate.Location = new System.Drawing.Point(334, 26);
            this.IVDate.MaxLength = 20;
            this.IVDate.Name = "IVDate";
            this.IVDate.ReadOnly = true;
            this.IVDate.Size = new System.Drawing.Size(300, 25);
            this.IVDate.TabIndex = 0;
            this.IVDate.TextRenderingMode = Infragistics.Win.TextRenderingMode.GDI;
            // 
            // panel1
            // 
            this.panel1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(243)))), ((int)(((byte)(234)))), ((int)(((byte)(226)))));
            this.panel1.Controls.Add(this.label2);
            this.panel1.Controls.Add(this.IVDate);
            this.panel1.Controls.Add(this.label1);
            this.panel1.Location = new System.Drawing.Point(12, 13);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(647, 63);
            this.panel1.TabIndex = 0;
            // 
            // panel2
            // 
            this.panel2.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(243)))), ((int)(((byte)(234)))), ((int)(((byte)(226)))));
            this.panel2.Controls.Add(this.btnCancel);
            this.panel2.Controls.Add(this.btnOK);
            this.panel2.Location = new System.Drawing.Point(12, 236);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(647, 38);
            this.panel2.TabIndex = 2;
            // 
            // frmDOIVConfirm
            // 
            this.AcceptButton = this.btnOK;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.White;
            this.CancelButton = this.btnCancel;
            this.ClientSize = new System.Drawing.Size(671, 282);
            this.Controls.Add(this.panel2);
            this.Controls.Add(this.panel1);
            this.Controls.Add(this.tagrdDeliveryOrder);
            this.KeyPreview = true;
            this.Name = "frmDOIVConfirm";
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Tag = "frmPopupDOIVConfirm";
            this.Text = "Confirmation";
            this.Load += new System.EventHandler(this.frmDOIVConfirm_Load);
            this.KeyDown += new System.Windows.Forms.KeyEventHandler(this.frmDOIVConfirm_KeyDown);
            ((System.ComponentModel.ISupportInitialize)(this.tagrdDeliveryOrder)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.IVDate)).EndInit();
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this.panel2.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private TAUtil.TAGridEditor tagrdDeliveryOrder;
        private System.Windows.Forms.Button btnOK;
        private System.Windows.Forms.Button btnCancel;
        private TAUtil.TADateEditor IVDate;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Panel panel2;
    }
}