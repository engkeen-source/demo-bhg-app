namespace WinUI
{
    partial class frmReportViewer
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
            this.tolRptViewer = new System.Windows.Forms.ToolStrip();
            this.tsbClose = new System.Windows.Forms.ToolStripButton();
            this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
            this.tscExport = new System.Windows.Forms.ToolStripDropDownButton();
            this.portableDocumentPDFToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.mSWordDOCToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.mSExcelXLSToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.mSExcelXLSDataOnlyToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.crystalReportRPTToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.cSVTabToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.tsbEmail = new System.Windows.Forms.ToolStripButton();
            this.tsbPageSetup = new System.Windows.Forms.ToolStripButton();
            this.tsbPrint = new System.Windows.Forms.ToolStripButton();
            this.toolStripSeparator3 = new System.Windows.Forms.ToolStripSeparator();
            this.tsbFirstPage = new System.Windows.Forms.ToolStripButton();
            this.tsbPreviousPage = new System.Windows.Forms.ToolStripButton();
            this.tsbNextPage = new System.Windows.Forms.ToolStripButton();
            this.tsbLastPage = new System.Windows.Forms.ToolStripButton();
            this.toolStripSeparator4 = new System.Windows.Forms.ToolStripSeparator();
            this.tsbSearch = new System.Windows.Forms.ToolStripButton();
            this.tscZoom = new System.Windows.Forms.ToolStripDropDownButton();
            this.toolStripLabel1 = new System.Windows.Forms.ToolStripLabel();
            this.toolStripSeparator2 = new System.Windows.Forms.ToolStripSeparator();
            this.tstGoToPage = new System.Windows.Forms.ToolStripTextBox();
            this.crRptViewer = new CrystalDecisions.Windows.Forms.CrystalReportViewer();
            this.cSVDelimitedToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.tolRptViewer.SuspendLayout();
            this.SuspendLayout();
            // 
            // tolRptViewer
            // 
            this.tolRptViewer.AutoSize = false;
            this.tolRptViewer.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(231)))), ((int)(((byte)(214)))), ((int)(((byte)(197)))));
            this.tolRptViewer.ImageScalingSize = new System.Drawing.Size(32, 32);
            this.tolRptViewer.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.tsbClose,
            this.toolStripSeparator1,
            this.tscExport,
            this.tsbEmail,
            this.tsbPageSetup,
            this.tsbPrint,
            this.toolStripSeparator3,
            this.tsbFirstPage,
            this.tsbPreviousPage,
            this.tsbNextPage,
            this.tsbLastPage,
            this.toolStripSeparator4,
            this.tsbSearch,
            this.tscZoom,
            this.toolStripLabel1,
            this.toolStripSeparator2,
            this.tstGoToPage});
            this.tolRptViewer.Location = new System.Drawing.Point(0, 0);
            this.tolRptViewer.Name = "tolRptViewer";
            this.tolRptViewer.Padding = new System.Windows.Forms.Padding(5, 0, 1, 0);
            this.tolRptViewer.Size = new System.Drawing.Size(900, 70);
            this.tolRptViewer.TabIndex = 0;
            this.tolRptViewer.Text = "toolStrip1";
            // 
            // tsbClose
            // 
            this.tsbClose.AutoSize = false;
            this.tsbClose.BackColor = System.Drawing.Color.Transparent;
            this.tsbClose.Font = new System.Drawing.Font("Calibri", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.tsbClose.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            this.tsbClose.Image = global::WinUI.Properties.Resources.close_a_32;
            this.tsbClose.ImageScaling = System.Windows.Forms.ToolStripItemImageScaling.None;
            this.tsbClose.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.tsbClose.Name = "tsbClose";
            this.tsbClose.Overflow = System.Windows.Forms.ToolStripItemOverflow.Never;
            this.tsbClose.RightToLeftAutoMirrorImage = true;
            this.tsbClose.Size = new System.Drawing.Size(70, 55);
            this.tsbClose.Text = "&Close";
            this.tsbClose.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText;
            this.tsbClose.Click += new System.EventHandler(this.tsbClose_Click);
            // 
            // toolStripSeparator1
            // 
            this.toolStripSeparator1.Name = "toolStripSeparator1";
            this.toolStripSeparator1.Size = new System.Drawing.Size(6, 70);
            // 
            // tscExport
            // 
            this.tscExport.AutoSize = false;
            this.tscExport.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.portableDocumentPDFToolStripMenuItem,
            this.mSWordDOCToolStripMenuItem,
            this.mSExcelXLSToolStripMenuItem,
            this.mSExcelXLSDataOnlyToolStripMenuItem,
            this.crystalReportRPTToolStripMenuItem,
            this.cSVTabToolStripMenuItem,
            this.cSVDelimitedToolStripMenuItem});
            this.tscExport.Font = new System.Drawing.Font("Calibri", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.tscExport.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            this.tscExport.Image = global::WinUI.Properties.Resources.export;
            this.tscExport.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.tscExport.Name = "tscExport";
            this.tscExport.Size = new System.Drawing.Size(70, 55);
            this.tscExport.Text = "Export";
            this.tscExport.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText;
            // 
            // portableDocumentPDFToolStripMenuItem
            // 
            this.portableDocumentPDFToolStripMenuItem.AutoSize = false;
            this.portableDocumentPDFToolStripMenuItem.Font = new System.Drawing.Font("Zawgyi-One", 8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.portableDocumentPDFToolStripMenuItem.Name = "portableDocumentPDFToolStripMenuItem";
            this.portableDocumentPDFToolStripMenuItem.Size = new System.Drawing.Size(208, 30);
            this.portableDocumentPDFToolStripMenuItem.Tag = "pdf";
            this.portableDocumentPDFToolStripMenuItem.Text = "Portable Document (PDF)";
            this.portableDocumentPDFToolStripMenuItem.TextDirection = System.Windows.Forms.ToolStripTextDirection.Horizontal;
            this.portableDocumentPDFToolStripMenuItem.Click += new System.EventHandler(this.portableDocumentPDFToolStripMenuItem_Click);
            // 
            // mSWordDOCToolStripMenuItem
            // 
            this.mSWordDOCToolStripMenuItem.AutoSize = false;
            this.mSWordDOCToolStripMenuItem.Font = new System.Drawing.Font("Zawgyi-One", 8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.mSWordDOCToolStripMenuItem.Name = "mSWordDOCToolStripMenuItem";
            this.mSWordDOCToolStripMenuItem.Size = new System.Drawing.Size(208, 30);
            this.mSWordDOCToolStripMenuItem.Tag = "doc";
            this.mSWordDOCToolStripMenuItem.Text = "MS Word (DOC)";
            this.mSWordDOCToolStripMenuItem.Click += new System.EventHandler(this.mSWordDOCToolStripMenuItem_Click);
            // 
            // mSExcelXLSToolStripMenuItem
            // 
            this.mSExcelXLSToolStripMenuItem.AutoSize = false;
            this.mSExcelXLSToolStripMenuItem.Font = new System.Drawing.Font("Zawgyi-One", 8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.mSExcelXLSToolStripMenuItem.Name = "mSExcelXLSToolStripMenuItem";
            this.mSExcelXLSToolStripMenuItem.Size = new System.Drawing.Size(208, 30);
            this.mSExcelXLSToolStripMenuItem.Tag = "xls";
            this.mSExcelXLSToolStripMenuItem.Text = "MS Excel (XLS)";
            this.mSExcelXLSToolStripMenuItem.Click += new System.EventHandler(this.mSExcelXLSToolStripMenuItem_Click);
            // 
            // mSExcelXLSDataOnlyToolStripMenuItem
            // 
            this.mSExcelXLSDataOnlyToolStripMenuItem.Name = "mSExcelXLSDataOnlyToolStripMenuItem";
            this.mSExcelXLSDataOnlyToolStripMenuItem.Size = new System.Drawing.Size(209, 22);
            this.mSExcelXLSDataOnlyToolStripMenuItem.Text = "MS Excel (XLS) Data Only";
            this.mSExcelXLSDataOnlyToolStripMenuItem.Click += new System.EventHandler(this.mSExcelXLSDataOnlyToolStripMenuItem_Click);
            // 
            // crystalReportRPTToolStripMenuItem
            // 
            this.crystalReportRPTToolStripMenuItem.AutoSize = false;
            this.crystalReportRPTToolStripMenuItem.Font = new System.Drawing.Font("Zawgyi-One", 8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.crystalReportRPTToolStripMenuItem.Margin = new System.Windows.Forms.Padding(0, 8, 0, 1);
            this.crystalReportRPTToolStripMenuItem.Name = "crystalReportRPTToolStripMenuItem";
            this.crystalReportRPTToolStripMenuItem.Size = new System.Drawing.Size(208, 30);
            this.crystalReportRPTToolStripMenuItem.Tag = "rpt";
            this.crystalReportRPTToolStripMenuItem.Text = "Crystal Report (RPT)";
            this.crystalReportRPTToolStripMenuItem.Click += new System.EventHandler(this.crystalReportRPTToolStripMenuItem_Click);
            // 
            // cSVTabToolStripMenuItem
            // 
            this.cSVTabToolStripMenuItem.Name = "cSVTabToolStripMenuItem";
            this.cSVTabToolStripMenuItem.Size = new System.Drawing.Size(209, 22);
            this.cSVTabToolStripMenuItem.Text = "CSV (Tab Delimited)";
            this.cSVTabToolStripMenuItem.Click += new System.EventHandler(this.cSVTabToolStripMenuItem_Click);
            // 
            // tsbEmail
            // 
            this.tsbEmail.AutoSize = false;
            this.tsbEmail.Font = new System.Drawing.Font("Calibri", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.tsbEmail.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            this.tsbEmail.Image = global::WinUI.Properties.Resources.white_list_save_32;
            this.tsbEmail.ImageScaling = System.Windows.Forms.ToolStripItemImageScaling.None;
            this.tsbEmail.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.tsbEmail.Name = "tsbEmail";
            this.tsbEmail.Size = new System.Drawing.Size(83, 55);
            this.tsbEmail.Text = "Email";
            this.tsbEmail.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText;
            this.tsbEmail.Click += new System.EventHandler(this.tsbEmail_Click);
            // 
            // tsbPageSetup
            // 
            this.tsbPageSetup.AutoSize = false;
            this.tsbPageSetup.Font = new System.Drawing.Font("Calibri", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.tsbPageSetup.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            this.tsbPageSetup.Image = global::WinUI.Properties.Resources.diskette_config_32;
            this.tsbPageSetup.ImageScaling = System.Windows.Forms.ToolStripItemImageScaling.None;
            this.tsbPageSetup.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.tsbPageSetup.Name = "tsbPageSetup";
            this.tsbPageSetup.Size = new System.Drawing.Size(83, 55);
            this.tsbPageSetup.Text = "Print Setting";
            this.tsbPageSetup.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText;
            this.tsbPageSetup.Click += new System.EventHandler(this.tsbPrintSetup_Click);
            // 
            // tsbPrint
            // 
            this.tsbPrint.AutoSize = false;
            this.tsbPrint.Font = new System.Drawing.Font("Calibri", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.tsbPrint.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            this.tsbPrint.Image = global::WinUI.Properties.Resources.print;
            this.tsbPrint.ImageScaling = System.Windows.Forms.ToolStripItemImageScaling.None;
            this.tsbPrint.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.tsbPrint.Name = "tsbPrint";
            this.tsbPrint.Size = new System.Drawing.Size(70, 55);
            this.tsbPrint.Text = "Print";
            this.tsbPrint.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText;
            this.tsbPrint.Click += new System.EventHandler(this.tsbPrint_Click);
            // 
            // toolStripSeparator3
            // 
            this.toolStripSeparator3.Name = "toolStripSeparator3";
            this.toolStripSeparator3.Padding = new System.Windows.Forms.Padding(5, 0, 5, 0);
            this.toolStripSeparator3.Size = new System.Drawing.Size(6, 70);
            // 
            // tsbFirstPage
            // 
            this.tsbFirstPage.AutoSize = false;
            this.tsbFirstPage.Font = new System.Drawing.Font("Calibri", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.tsbFirstPage.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            this.tsbFirstPage.Image = global::WinUI.Properties.Resources.first_32;
            this.tsbFirstPage.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.tsbFirstPage.Name = "tsbFirstPage";
            this.tsbFirstPage.Size = new System.Drawing.Size(70, 55);
            this.tsbFirstPage.Text = "First Page";
            this.tsbFirstPage.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText;
            this.tsbFirstPage.Click += new System.EventHandler(this.tsbFirstPage_Click);
            // 
            // tsbPreviousPage
            // 
            this.tsbPreviousPage.AutoSize = false;
            this.tsbPreviousPage.Font = new System.Drawing.Font("Calibri", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.tsbPreviousPage.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            this.tsbPreviousPage.Image = global::WinUI.Properties.Resources.rew_32;
            this.tsbPreviousPage.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.tsbPreviousPage.Name = "tsbPreviousPage";
            this.tsbPreviousPage.Size = new System.Drawing.Size(70, 55);
            this.tsbPreviousPage.Text = "Previous";
            this.tsbPreviousPage.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText;
            this.tsbPreviousPage.Click += new System.EventHandler(this.tsbPreviousPage_Click);
            // 
            // tsbNextPage
            // 
            this.tsbNextPage.AutoSize = false;
            this.tsbNextPage.Font = new System.Drawing.Font("Calibri", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.tsbNextPage.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            this.tsbNextPage.Image = global::WinUI.Properties.Resources.fast_forward_32;
            this.tsbNextPage.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.tsbNextPage.Name = "tsbNextPage";
            this.tsbNextPage.Size = new System.Drawing.Size(70, 55);
            this.tsbNextPage.Text = "Next";
            this.tsbNextPage.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText;
            this.tsbNextPage.Click += new System.EventHandler(this.tsbNextPage_Click);
            // 
            // tsbLastPage
            // 
            this.tsbLastPage.AutoSize = false;
            this.tsbLastPage.Font = new System.Drawing.Font("Calibri", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.tsbLastPage.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            this.tsbLastPage.Image = global::WinUI.Properties.Resources.last_32;
            this.tsbLastPage.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.tsbLastPage.Name = "tsbLastPage";
            this.tsbLastPage.Size = new System.Drawing.Size(70, 55);
            this.tsbLastPage.Text = "Last";
            this.tsbLastPage.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText;
            this.tsbLastPage.Click += new System.EventHandler(this.tsbLastPage_Click);
            // 
            // toolStripSeparator4
            // 
            this.toolStripSeparator4.Name = "toolStripSeparator4";
            this.toolStripSeparator4.Size = new System.Drawing.Size(6, 70);
            // 
            // tsbSearch
            // 
            this.tsbSearch.AutoSize = false;
            this.tsbSearch.Font = new System.Drawing.Font("Calibri", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.tsbSearch.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            this.tsbSearch.Image = global::WinUI.Properties.Resources.binoculares_32;
            this.tsbSearch.ImageScaling = System.Windows.Forms.ToolStripItemImageScaling.None;
            this.tsbSearch.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.tsbSearch.Name = "tsbSearch";
            this.tsbSearch.Size = new System.Drawing.Size(70, 55);
            this.tsbSearch.Text = "Find";
            this.tsbSearch.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText;
            this.tsbSearch.Click += new System.EventHandler(this.tsbSearch_Click);
            // 
            // tscZoom
            // 
            this.tscZoom.AutoSize = false;
            this.tscZoom.Font = new System.Drawing.Font("Calibri", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.tscZoom.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            this.tscZoom.Image = global::WinUI.Properties.Resources.preview_32;
            this.tscZoom.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.tscZoom.Name = "tscZoom";
            this.tscZoom.Size = new System.Drawing.Size(70, 55);
            this.tscZoom.Text = "Zoom";
            this.tscZoom.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText;
            this.tscZoom.DropDownItemClicked += new System.Windows.Forms.ToolStripItemClickedEventHandler(this.tscZoom_DropDownItemClicked);
            // 
            // toolStripLabel1
            // 
            this.toolStripLabel1.Font = new System.Drawing.Font("Calibri", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.toolStripLabel1.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            this.toolStripLabel1.Name = "toolStripLabel1";
            this.toolStripLabel1.Size = new System.Drawing.Size(69, 15);
            this.toolStripLabel1.Text = "Go To Page ";
            // 
            // toolStripSeparator2
            // 
            this.toolStripSeparator2.Name = "toolStripSeparator2";
            this.toolStripSeparator2.Size = new System.Drawing.Size(6, 70);
            // 
            // tstGoToPage
            // 
            this.tstGoToPage.AutoSize = false;
            this.tstGoToPage.BackColor = System.Drawing.Color.White;
            this.tstGoToPage.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.tstGoToPage.Name = "tstGoToPage";
            this.tstGoToPage.Size = new System.Drawing.Size(80, 23);
            this.tstGoToPage.TextBoxTextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            this.tstGoToPage.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.tstGoToPage_KeyPress);
            // 
            // crRptViewer
            // 
            this.crRptViewer.ActiveViewIndex = -1;
            this.crRptViewer.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.crRptViewer.Cursor = System.Windows.Forms.Cursors.Default;
            this.crRptViewer.DisplayBackgroundEdge = false;
            this.crRptViewer.DisplayToolbar = false;
            this.crRptViewer.Dock = System.Windows.Forms.DockStyle.Fill;
            this.crRptViewer.EnableDrillDown = false;
            this.crRptViewer.Location = new System.Drawing.Point(0, 70);
            this.crRptViewer.Name = "crRptViewer";
            this.crRptViewer.ShowCloseButton = false;
            this.crRptViewer.ShowExportButton = false;
            this.crRptViewer.ShowGotoPageButton = false;
            this.crRptViewer.ShowGroupTreeButton = false;
            this.crRptViewer.ShowPageNavigateButtons = false;
            this.crRptViewer.ShowParameterPanelButton = false;
            this.crRptViewer.ShowPrintButton = false;
            this.crRptViewer.ShowRefreshButton = false;
            this.crRptViewer.ShowTextSearchButton = false;
            this.crRptViewer.ShowZoomButton = false;
            this.crRptViewer.Size = new System.Drawing.Size(900, 542);
            this.crRptViewer.TabIndex = 1;
            this.crRptViewer.ToolPanelView = CrystalDecisions.Windows.Forms.ToolPanelViewType.None;
            this.crRptViewer.ToolPanelWidth = 100;
            this.crRptViewer.ClickPage += new CrystalDecisions.Windows.Forms.PageMouseEventHandler(this.crRptViewer_ClickPage);
            // 
            // cSVDelimitedToolStripMenuItem
            // 
            this.cSVDelimitedToolStripMenuItem.Name = "cSVDelimitedToolStripMenuItem";
            this.cSVDelimitedToolStripMenuItem.Size = new System.Drawing.Size(209, 22);
            this.cSVDelimitedToolStripMenuItem.Text = "CSV (| Delimited)";
            this.cSVDelimitedToolStripMenuItem.Click += new System.EventHandler(this.cSVDelimitedToolStripMenuItem_Click);
            // 
            // frmReportViewer
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(900, 612);
            this.Controls.Add(this.crRptViewer);
            this.Controls.Add(this.tolRptViewer);
            this.KeyPreview = true;
            this.Name = "frmReportViewer";
            this.Text = "BOSS Report Viewer";
            this.WindowState = System.Windows.Forms.FormWindowState.Maximized;
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.frmReportViewer_FormClosing);
            this.Load += new System.EventHandler(this.frmReportViewer_Load);
            this.tolRptViewer.ResumeLayout(false);
            this.tolRptViewer.PerformLayout();
            this.ResumeLayout(false);

        }

       
       

        #endregion

        private System.Windows.Forms.ToolStrip tolRptViewer;
        private System.Windows.Forms.ToolStripButton tsbPrint;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator3;
        private System.Windows.Forms.ToolStripButton tsbFirstPage;
        private System.Windows.Forms.ToolStripButton tsbPreviousPage;
        private System.Windows.Forms.ToolStripButton tsbNextPage;
        private System.Windows.Forms.ToolStripButton tsbLastPage;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator4;
        private System.Windows.Forms.ToolStripButton tsbSearch;
        private System.Windows.Forms.ToolStripDropDownButton tscZoom;
        private System.Windows.Forms.ToolStripButton tsbClose;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator1;
        private System.Windows.Forms.ToolStripDropDownButton tscExport;
        private System.Windows.Forms.ToolStripMenuItem portableDocumentPDFToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem mSWordDOCToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem mSExcelXLSToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem crystalReportRPTToolStripMenuItem;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator2;
        private System.Windows.Forms.ToolStripTextBox tstGoToPage;
        private System.Windows.Forms.ToolStripLabel toolStripLabel1;
        private System.Windows.Forms.ToolStripMenuItem mSExcelXLSDataOnlyToolStripMenuItem;
        private CrystalDecisions.Windows.Forms.CrystalReportViewer crRptViewer;
        private System.Windows.Forms.ToolStripButton tsbPageSetup;
        private System.Windows.Forms.ToolStripMenuItem cSVTabToolStripMenuItem;
        private System.Windows.Forms.ToolStripButton tsbEmail;
        private System.Windows.Forms.ToolStripMenuItem cSVDelimitedToolStripMenuItem;



    }
}