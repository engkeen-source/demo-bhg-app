namespace WinUI
{
    partial class frmPopupEstoreInfo
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
            Infragistics.Win.Appearance appearance2 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance667 = new Infragistics.Win.Appearance();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmPopupEstoreInfo));
            Infragistics.Win.Appearance appearance604 = new Infragistics.Win.Appearance();
            this.lblSystemEstorePrice = new System.Windows.Forms.Label();
            this.numEstorePriceSynTable = new TAUtil.TANumericEditor();
            this.numEstorePriceControlPrice = new TAUtil.TANumericEditor();
            this.lblControlPrice = new System.Windows.Forms.Label();
            this.lblKhaki = new System.Windows.Forms.Label();
            this.lblOrange = new System.Windows.Forms.Label();
            this.lblRed = new System.Windows.Forms.Label();
            this.lblTransparent = new System.Windows.Forms.Label();
            this.linkEstore = new System.Windows.Forms.LinkLabel();
            this.lblMsgTransparent = new System.Windows.Forms.Label();
            this.lblMsgOrange = new System.Windows.Forms.Label();
            this.lblMsgRed = new System.Windows.Forms.Label();
            this.lblMsgKhaki = new System.Windows.Forms.Label();
            this.lblItmID = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.lblEstorePriceWeb = new System.Windows.Forms.Label();
            this.chkVerifyWebsitePrice = new TAUtil.TACheckBoxEditor();
            ((System.ComponentModel.ISupportInitialize)(this.numEstorePriceSynTable)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numEstorePriceControlPrice)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.chkVerifyWebsitePrice)).BeginInit();
            this.SuspendLayout();
            // 
            // lblSystemEstorePrice
            // 
            this.lblSystemEstorePrice.Font = new System.Drawing.Font("Calibri", 10.5F, System.Drawing.FontStyle.Bold);
            this.lblSystemEstorePrice.ForeColor = System.Drawing.Color.DimGray;
            this.lblSystemEstorePrice.Location = new System.Drawing.Point(12, 70);
            this.lblSystemEstorePrice.Name = "lblSystemEstorePrice";
            this.lblSystemEstorePrice.Size = new System.Drawing.Size(269, 24);
            this.lblSystemEstorePrice.TabIndex = 487;
            this.lblSystemEstorePrice.Text = "eStore Price";
            this.lblSystemEstorePrice.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // numEstorePriceSynTable
            // 
            appearance2.BackColor = System.Drawing.Color.White;
            appearance2.BackColorDisabled = System.Drawing.Color.White;
            appearance2.BorderColor = System.Drawing.Color.LightGray;
            appearance2.FontData.Name = "Calibri";
            appearance2.FontData.SizeInPoints = 11F;
            appearance2.ForeColor = System.Drawing.Color.Black;
            appearance2.ForeColorDisabled = System.Drawing.Color.Black;
            appearance2.TextHAlignAsString = "Right";
            this.numEstorePriceSynTable.Appearance = appearance2;
            this.numEstorePriceSynTable.BackColor = System.Drawing.Color.White;
            this.numEstorePriceSynTable.DisplayStyle = Infragistics.Win.EmbeddableElementDisplayStyle.Office2007;
            this.numEstorePriceSynTable.Font = new System.Drawing.Font("Calibri", 11F);
            this.numEstorePriceSynTable.ForceExitByRestoreValue = false;
            this.numEstorePriceSynTable.Format = "#,##0.0000######\r\n";
            this.numEstorePriceSynTable.Location = new System.Drawing.Point(287, 68);
            this.numEstorePriceSynTable.Name = "numEstorePriceSynTable";
            this.numEstorePriceSynTable.NumberType = TAUtil.NumericType.Decimal;
            this.numEstorePriceSynTable.Size = new System.Drawing.Size(90, 26);
            this.numEstorePriceSynTable.TabIndex = 490;
            this.numEstorePriceSynTable.TabStop = false;
            this.numEstorePriceSynTable.TextRenderingMode = Infragistics.Win.TextRenderingMode.GDI;
            this.numEstorePriceSynTable.ZeroIfEmpty = false;
            // 
            // numEstorePriceControlPrice
            // 
            appearance667.BackColor = System.Drawing.Color.White;
            appearance667.BackColorDisabled = System.Drawing.Color.White;
            appearance667.BorderColor = System.Drawing.Color.LightGray;
            appearance667.FontData.Name = "Calibri";
            appearance667.FontData.SizeInPoints = 11F;
            appearance667.ForeColor = System.Drawing.Color.Black;
            appearance667.ForeColorDisabled = System.Drawing.Color.Black;
            appearance667.TextHAlignAsString = "Right";
            this.numEstorePriceControlPrice.Appearance = appearance667;
            this.numEstorePriceControlPrice.BackColor = System.Drawing.Color.White;
            this.numEstorePriceControlPrice.DisplayStyle = Infragistics.Win.EmbeddableElementDisplayStyle.Office2007;
            this.numEstorePriceControlPrice.Font = new System.Drawing.Font("Calibri", 11F);
            this.numEstorePriceControlPrice.ForceExitByRestoreValue = false;
            this.numEstorePriceControlPrice.Format = "#,##0.0000######\r\n";
            this.numEstorePriceControlPrice.Location = new System.Drawing.Point(287, 39);
            this.numEstorePriceControlPrice.Name = "numEstorePriceControlPrice";
            this.numEstorePriceControlPrice.NumberType = TAUtil.NumericType.Decimal;
            this.numEstorePriceControlPrice.Size = new System.Drawing.Size(90, 26);
            this.numEstorePriceControlPrice.TabIndex = 491;
            this.numEstorePriceControlPrice.TabStop = false;
            this.numEstorePriceControlPrice.TextRenderingMode = Infragistics.Win.TextRenderingMode.GDI;
            this.numEstorePriceControlPrice.ZeroIfEmpty = false;
            // 
            // lblControlPrice
            // 
            this.lblControlPrice.Font = new System.Drawing.Font("Calibri", 10.5F, System.Drawing.FontStyle.Bold);
            this.lblControlPrice.ForeColor = System.Drawing.Color.DimGray;
            this.lblControlPrice.Location = new System.Drawing.Point(12, 41);
            this.lblControlPrice.Name = "lblControlPrice";
            this.lblControlPrice.Size = new System.Drawing.Size(269, 24);
            this.lblControlPrice.TabIndex = 492;
            this.lblControlPrice.Text = "eStore Control Price";
            this.lblControlPrice.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // lblKhaki
            // 
            this.lblKhaki.BackColor = System.Drawing.Color.Khaki;
            this.lblKhaki.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.lblKhaki.Font = new System.Drawing.Font("Calibri", 10.5F, System.Drawing.FontStyle.Bold);
            this.lblKhaki.Location = new System.Drawing.Point(14, 176);
            this.lblKhaki.Name = "lblKhaki";
            this.lblKhaki.Size = new System.Drawing.Size(33, 25);
            this.lblKhaki.TabIndex = 495;
            this.lblKhaki.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // lblOrange
            // 
            this.lblOrange.BackColor = System.Drawing.Color.Orange;
            this.lblOrange.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.lblOrange.Font = new System.Drawing.Font("Calibri", 10.5F, System.Drawing.FontStyle.Bold);
            this.lblOrange.Location = new System.Drawing.Point(14, 143);
            this.lblOrange.Name = "lblOrange";
            this.lblOrange.Size = new System.Drawing.Size(33, 25);
            this.lblOrange.TabIndex = 496;
            this.lblOrange.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // lblRed
            // 
            this.lblRed.BackColor = System.Drawing.Color.Red;
            this.lblRed.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.lblRed.Font = new System.Drawing.Font("Calibri", 10.5F, System.Drawing.FontStyle.Bold);
            this.lblRed.Location = new System.Drawing.Point(14, 209);
            this.lblRed.Name = "lblRed";
            this.lblRed.Size = new System.Drawing.Size(33, 25);
            this.lblRed.TabIndex = 497;
            this.lblRed.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // lblTransparent
            // 
            this.lblTransparent.BackColor = System.Drawing.Color.Transparent;
            this.lblTransparent.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.lblTransparent.Font = new System.Drawing.Font("Calibri", 10.5F, System.Drawing.FontStyle.Bold);
            this.lblTransparent.Location = new System.Drawing.Point(14, 242);
            this.lblTransparent.Name = "lblTransparent";
            this.lblTransparent.Size = new System.Drawing.Size(33, 25);
            this.lblTransparent.TabIndex = 498;
            this.lblTransparent.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // linkEstore
            // 
            this.linkEstore.Font = new System.Drawing.Font("Calibri", 9.5F);
            this.linkEstore.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.linkEstore.Location = new System.Drawing.Point(35, 105);
            this.linkEstore.Name = "linkEstore";
            this.linkEstore.Size = new System.Drawing.Size(309, 25);
            this.linkEstore.TabIndex = 499;
            this.linkEstore.TabStop = true;
            this.linkEstore.Text = "go to bh-estore to view item detail";
            this.linkEstore.Click += new System.EventHandler(this.linkEstore_Click);
            // 
            // lblMsgTransparent
            // 
            this.lblMsgTransparent.Font = new System.Drawing.Font("Calibri", 10F, System.Drawing.FontStyle.Strikeout);
            this.lblMsgTransparent.ForeColor = System.Drawing.Color.DimGray;
            this.lblMsgTransparent.Location = new System.Drawing.Point(53, 243);
            this.lblMsgTransparent.Name = "lblMsgTransparent";
            this.lblMsgTransparent.Size = new System.Drawing.Size(335, 24);
            this.lblMsgTransparent.TabIndex = 500;
            this.lblMsgTransparent.Tag = "transparent";
            this.lblMsgTransparent.Text = "This bh code has matching prices. ";
            this.lblMsgTransparent.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // lblMsgOrange
            // 
            this.lblMsgOrange.Font = new System.Drawing.Font("Calibri", 10F, System.Drawing.FontStyle.Strikeout);
            this.lblMsgOrange.ForeColor = System.Drawing.Color.DimGray;
            this.lblMsgOrange.Location = new System.Drawing.Point(53, 144);
            this.lblMsgOrange.Name = "lblMsgOrange";
            this.lblMsgOrange.Size = new System.Drawing.Size(381, 24);
            this.lblMsgOrange.TabIndex = 501;
            this.lblMsgOrange.Tag = "orange";
            this.lblMsgOrange.Text = "This bh code has not been uploaded for sales on the eStore.";
            this.lblMsgOrange.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // lblMsgRed
            // 
            this.lblMsgRed.Font = new System.Drawing.Font("Calibri", 10F, System.Drawing.FontStyle.Strikeout);
            this.lblMsgRed.ForeColor = System.Drawing.Color.DimGray;
            this.lblMsgRed.Location = new System.Drawing.Point(53, 210);
            this.lblMsgRed.Name = "lblMsgRed";
            this.lblMsgRed.Size = new System.Drawing.Size(335, 24);
            this.lblMsgRed.TabIndex = 502;
            this.lblMsgRed.Tag = "red";
            this.lblMsgRed.Text = "Estore prices do not match. ";
            this.lblMsgRed.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // lblMsgKhaki
            // 
            this.lblMsgKhaki.Font = new System.Drawing.Font("Calibri", 10F, System.Drawing.FontStyle.Strikeout);
            this.lblMsgKhaki.ForeColor = System.Drawing.Color.DimGray;
            this.lblMsgKhaki.Location = new System.Drawing.Point(53, 177);
            this.lblMsgKhaki.Name = "lblMsgKhaki";
            this.lblMsgKhaki.Size = new System.Drawing.Size(345, 24);
            this.lblMsgKhaki.TabIndex = 503;
            this.lblMsgKhaki.Tag = "khaki";
            this.lblMsgKhaki.Text = "This bh code is available on the eStore but has no price.";
            this.lblMsgKhaki.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // lblItmID
            // 
            this.lblItmID.Font = new System.Drawing.Font("Calibri", 11.5F, System.Drawing.FontStyle.Bold);
            this.lblItmID.ForeColor = System.Drawing.Color.Black;
            this.lblItmID.Location = new System.Drawing.Point(12, 8);
            this.lblItmID.Name = "lblItmID";
            this.lblItmID.Size = new System.Drawing.Size(217, 24);
            this.lblItmID.TabIndex = 504;
            this.lblItmID.Text = "ABCD";
            this.lblItmID.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // label1
            // 
            this.label1.Image = ((System.Drawing.Image)(resources.GetObject("label1.Image")));
            this.label1.Location = new System.Drawing.Point(16, 105);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(16, 18);
            this.label1.TabIndex = 506;
            // 
            // lblEstorePriceWeb
            // 
            this.lblEstorePriceWeb.Font = new System.Drawing.Font("Calibri", 10.5F);
            this.lblEstorePriceWeb.ForeColor = System.Drawing.Color.Navy;
            this.lblEstorePriceWeb.Location = new System.Drawing.Point(287, 131);
            this.lblEstorePriceWeb.Name = "lblEstorePriceWeb";
            this.lblEstorePriceWeb.Size = new System.Drawing.Size(90, 24);
            this.lblEstorePriceWeb.TabIndex = 508;
            this.lblEstorePriceWeb.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.lblEstorePriceWeb.Visible = false;
            // 
            // chkVerifyWebsitePrice
            // 
            appearance604.FontData.BoldAsString = "False";
            appearance604.FontData.ItalicAsString = "False";
            appearance604.FontData.Name = "Calibri";
            appearance604.FontData.SizeInPoints = 10F;
            appearance604.ForeColor = System.Drawing.Color.Navy;
            appearance604.TextVAlignAsString = "Top";
            this.chkVerifyWebsitePrice.Appearance = appearance604;
            this.chkVerifyWebsitePrice.cancelUpdate = false;
            this.chkVerifyWebsitePrice.GlyphInfo = Infragistics.Win.UIElementDrawParams.Office2007CheckBoxGlyphInfo;
            this.chkVerifyWebsitePrice.Location = new System.Drawing.Point(15, 120);
            this.chkVerifyWebsitePrice.Name = "chkVerifyWebsitePrice";
            this.chkVerifyWebsitePrice.Size = new System.Drawing.Size(252, 20);
            this.chkVerifyWebsitePrice.TabIndex = 509;
            this.chkVerifyWebsitePrice.TabStop = false;
            this.chkVerifyWebsitePrice.Text = "Verify the price on the estore website";
            this.chkVerifyWebsitePrice.TextRenderingMode = Infragistics.Win.TextRenderingMode.GDI;
            this.chkVerifyWebsitePrice.Visible = false;
            this.chkVerifyWebsitePrice.CustomUpdate += new System.EventHandler(this.chkVerifyWebsitePrice_CustomUpdate);
            // 
            // frmPopupEstoreInfo
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(243)))), ((int)(((byte)(234)))), ((int)(((byte)(226)))));
            this.ClientSize = new System.Drawing.Size(438, 298);
            this.Controls.Add(this.chkVerifyWebsitePrice);
            this.Controls.Add(this.lblEstorePriceWeb);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.lblItmID);
            this.Controls.Add(this.lblMsgKhaki);
            this.Controls.Add(this.lblMsgRed);
            this.Controls.Add(this.lblMsgOrange);
            this.Controls.Add(this.lblMsgTransparent);
            this.Controls.Add(this.linkEstore);
            this.Controls.Add(this.lblTransparent);
            this.Controls.Add(this.lblRed);
            this.Controls.Add(this.lblOrange);
            this.Controls.Add(this.lblKhaki);
            this.Controls.Add(this.lblControlPrice);
            this.Controls.Add(this.numEstorePriceControlPrice);
            this.Controls.Add(this.numEstorePriceSynTable);
            this.Controls.Add(this.lblSystemEstorePrice);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.Name = "frmPopupEstoreInfo";
            this.ShowIcon = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "EStore Price";
            this.Load += new System.EventHandler(this.frmPopupEstoreInfo_Load);
            ((System.ComponentModel.ISupportInitialize)(this.numEstorePriceSynTable)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numEstorePriceControlPrice)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.chkVerifyWebsitePrice)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.Label lblSystemEstorePrice;
        private TAUtil.TANumericEditor numEstorePriceSynTable;
        private TAUtil.TANumericEditor numEstorePriceControlPrice;
        private System.Windows.Forms.Label lblControlPrice;
        private System.Windows.Forms.Label lblKhaki;
        private System.Windows.Forms.Label lblOrange;
        private System.Windows.Forms.Label lblRed;
        private System.Windows.Forms.Label lblTransparent;
        private System.Windows.Forms.LinkLabel linkEstore;
        private System.Windows.Forms.Label lblMsgTransparent;
        private System.Windows.Forms.Label lblMsgOrange;
        private System.Windows.Forms.Label lblMsgRed;
        private System.Windows.Forms.Label lblMsgKhaki;
        private System.Windows.Forms.Label lblItmID;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label lblEstorePriceWeb;
        private TAUtil.TACheckBoxEditor chkVerifyWebsitePrice;
    }
}