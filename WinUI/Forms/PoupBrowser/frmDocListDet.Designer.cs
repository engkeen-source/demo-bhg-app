namespace WinUI.Forms.PoupBrowser
{
    partial class frmDocListDet
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
            this.tagrdItmInfo = new TAUtil.TAGridEditor();
            ((System.ComponentModel.ISupportInitialize)(this.tagrdItmInfo)).BeginInit();
            this.SuspendLayout();
            // 
            // tagrdItmInfo
            // 
            this.tagrdItmInfo.ActiveConnection = null;
            this.tagrdItmInfo.AutoAddNewRow = false;
            this.tagrdItmInfo.AutoUseCustomControlsInCells = false;
            this.tagrdItmInfo.DefaultValue = null;
            this.tagrdItmInfo.DetailObjectKey = 0;
            this.tagrdItmInfo.DisplayLayout.CaptionVisible = Infragistics.Win.DefaultableBoolean.False;
            this.tagrdItmInfo.DisplayLayout.Override.FixedHeaderIndicator = Infragistics.Win.UltraWinGrid.FixedHeaderIndicator.None;
            this.tagrdItmInfo.DisplayLayout.Override.SelectTypeCell = Infragistics.Win.UltraWinGrid.SelectType.Single;
            this.tagrdItmInfo.DisplayLayout.Override.SelectTypeRow = Infragistics.Win.UltraWinGrid.SelectType.Single;
            this.tagrdItmInfo.DisplayLayout.ScrollBounds = Infragistics.Win.UltraWinGrid.ScrollBounds.ScrollToFill;
            this.tagrdItmInfo.DisplayLayout.ScrollStyle = Infragistics.Win.UltraWinGrid.ScrollStyle.Immediate;
            this.tagrdItmInfo.DisplayLayout.UseFixedHeaders = true;
            this.tagrdItmInfo.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tagrdItmInfo.EnterKeyDirection = TAUtil.EnterKeyDirectionEnum.NextCellByTab;
            this.tagrdItmInfo.Font = new System.Drawing.Font("Tahoma", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.tagrdItmInfo.HeaderObjectKey = null;
            this.tagrdItmInfo.Location = new System.Drawing.Point(0, 0);
            this.tagrdItmInfo.Name = "tagrdItmInfo";
            this.tagrdItmInfo.RowUpdateCancelAction = Infragistics.Win.UltraWinGrid.RowUpdateCancelAction.RetainDataAndActivation;
            this.tagrdItmInfo.Size = new System.Drawing.Size(642, 269);
            this.tagrdItmInfo.TabIndex = 1;
            this.tagrdItmInfo.Text = "taGridEditor1";
            this.tagrdItmInfo.UseFlatMode = Infragistics.Win.DefaultableBoolean.True;
            this.tagrdItmInfo.UseOsThemes = Infragistics.Win.DefaultableBoolean.False;
            // 
            // frmDocListDet
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.White;
            this.ClientSize = new System.Drawing.Size(642, 269);
            this.Controls.Add(this.tagrdItmInfo);
            this.KeyPreview = true;
            this.Name = "frmDocListDet";
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Tag = "frmDocListDet";
            this.Text = "Item List Information";
            this.Load += new System.EventHandler(this.frmDocListDet_Load);
            this.KeyDown += new System.Windows.Forms.KeyEventHandler(this.frmDocListDet_KeyDown);
            ((System.ComponentModel.ISupportInitialize)(this.tagrdItmInfo)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private TAUtil.TAGridEditor tagrdItmInfo;

    }
}