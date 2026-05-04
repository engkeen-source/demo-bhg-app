namespace WinUI
{
    partial class frmReportExport
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
            this.lblFormat = new System.Windows.Forms.Label();
            this.btnOk = new System.Windows.Forms.Button();
            this.btnCancel = new System.Windows.Forms.Button();
            this.Format = new TAUtil.TAComboBox();
            this.Destination = new TAUtil.TAComboBox();
            this.lblDestination = new System.Windows.Forms.Label();
            this.panel1 = new System.Windows.Forms.Panel();
            ((System.ComponentModel.ISupportInitialize)(this.Format)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.Destination)).BeginInit();
            this.panel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // lblFormat
            // 
            this.lblFormat.AutoSize = true;
            this.lblFormat.Font = new System.Drawing.Font("Calibri", 10F, System.Drawing.FontStyle.Italic);
            this.lblFormat.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            this.lblFormat.Location = new System.Drawing.Point(13, 5);
            this.lblFormat.Name = "lblFormat";
            this.lblFormat.Size = new System.Drawing.Size(49, 17);
            this.lblFormat.TabIndex = 0;
            this.lblFormat.Text = "Format";
            // 
            // btnOk
            // 
            this.btnOk.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.btnOk.Font = new System.Drawing.Font("Calibri", 9.75F, ((System.Drawing.FontStyle)((System.Drawing.FontStyle.Bold | System.Drawing.FontStyle.Italic))), System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnOk.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            this.btnOk.Image = global::WinUI.Properties.Resources.OK;
            this.btnOk.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnOk.Location = new System.Drawing.Point(85, 113);
            this.btnOk.Name = "btnOk";
            this.btnOk.Size = new System.Drawing.Size(75, 25);
            this.btnOk.TabIndex = 2;
            this.btnOk.Text = "&OK";
            this.btnOk.UseVisualStyleBackColor = true;
            this.btnOk.Click += new System.EventHandler(this.btnOk_Click);
            // 
            // btnCancel
            // 
            this.btnCancel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.btnCancel.Font = new System.Drawing.Font("Calibri", 9.75F, ((System.Drawing.FontStyle)((System.Drawing.FontStyle.Bold | System.Drawing.FontStyle.Italic))), System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnCancel.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            this.btnCancel.Image = global::WinUI.Properties.Resources.Cancel;
            this.btnCancel.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnCancel.Location = new System.Drawing.Point(166, 113);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(69, 25);
            this.btnCancel.TabIndex = 3;
            this.btnCancel.Text = "&Cancel";
            this.btnCancel.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.btnCancel.UseVisualStyleBackColor = true;
            this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
            // 
            // Format
            // 
            appearance1.BorderColor = System.Drawing.Color.LightGray;
            this.Format.Appearance = appearance1;
            this.Format.AutoCompleteMode = Infragistics.Win.AutoCompleteMode.Append;
            this.Format.AutoSize = false;
            this.Format.ComboIsDirty = false;
            this.Format.DisplayStyle = Infragistics.Win.EmbeddableElementDisplayStyle.Office2007;
            this.Format.Font = new System.Drawing.Font("Calibri", 11F);
            this.Format.ItemMatchingMode = Infragistics.Win.ValueListItemMatchingMode.DoNotConvertDataValueToString;
            this.Format.Location = new System.Drawing.Point(13, 25);
            this.Format.Name = "Format";
            this.Format.Size = new System.Drawing.Size(300, 25);
            this.Format.TabIndex = 0;
            this.Format.TextRenderingMode = Infragistics.Win.TextRenderingMode.GDI;
            this.Format.UserInputText = "";
            this.Format.ItemNotInList += new Infragistics.Win.UltraWinGrid.ItemNotInListEventHandler(this.Combo_NotInList);
            // 
            // Destination
            // 
            appearance2.BorderColor = System.Drawing.Color.LightGray;
            this.Destination.Appearance = appearance2;
            this.Destination.AutoCompleteMode = Infragistics.Win.AutoCompleteMode.Append;
            this.Destination.AutoSize = false;
            this.Destination.ComboIsDirty = false;
            this.Destination.DisplayStyle = Infragistics.Win.EmbeddableElementDisplayStyle.Office2007;
            this.Destination.Font = new System.Drawing.Font("Calibri", 11F);
            this.Destination.ItemMatchingMode = Infragistics.Win.ValueListItemMatchingMode.DoNotConvertDataValueToString;
            this.Destination.Location = new System.Drawing.Point(13, 75);
            this.Destination.Name = "Destination";
            this.Destination.Size = new System.Drawing.Size(300, 25);
            this.Destination.TabIndex = 1;
            this.Destination.TextRenderingMode = Infragistics.Win.TextRenderingMode.GDI;
            this.Destination.UserInputText = "";
            this.Destination.ItemNotInList += new Infragistics.Win.UltraWinGrid.ItemNotInListEventHandler(this.Combo_NotInList);
            // 
            // lblDestination
            // 
            this.lblDestination.AutoSize = true;
            this.lblDestination.Font = new System.Drawing.Font("Calibri", 10F, System.Drawing.FontStyle.Italic);
            this.lblDestination.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            this.lblDestination.Location = new System.Drawing.Point(13, 55);
            this.lblDestination.Name = "lblDestination";
            this.lblDestination.Size = new System.Drawing.Size(73, 17);
            this.lblDestination.TabIndex = 2;
            this.lblDestination.Text = "Destination";
            // 
            // panel1
            // 
            this.panel1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(243)))), ((int)(((byte)(234)))), ((int)(((byte)(226)))));
            this.panel1.Controls.Add(this.Format);
            this.panel1.Controls.Add(this.btnCancel);
            this.panel1.Controls.Add(this.Destination);
            this.panel1.Controls.Add(this.btnOk);
            this.panel1.Controls.Add(this.lblFormat);
            this.panel1.Controls.Add(this.lblDestination);
            this.panel1.Location = new System.Drawing.Point(12, 12);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(327, 157);
            this.panel1.TabIndex = 0;
            // 
            // frmReportExport
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.White;
            this.ClientSize = new System.Drawing.Size(352, 182);
            this.Controls.Add(this.panel1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.KeyPreview = true;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "frmReportExport";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Export";
            this.Load += new System.EventHandler(this.frmReportExport_Load);
            this.KeyDown += new System.Windows.Forms.KeyEventHandler(this.frmReportExport_KeyDown);
            ((System.ComponentModel.ISupportInitialize)(this.Format)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.Destination)).EndInit();
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private TAUtil.TAComboBox Format;
        private System.Windows.Forms.Label lblFormat;
        private System.Windows.Forms.Button btnOk;
        private System.Windows.Forms.Button btnCancel;
        private TAUtil.TAComboBox Destination;
        private System.Windows.Forms.Label lblDestination;
        private System.Windows.Forms.Panel panel1;
    }
}