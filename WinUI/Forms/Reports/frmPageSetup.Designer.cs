namespace WinUI
{
    partial class frmPageSetup
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
            Infragistics.Win.Appearance appearance7 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance8 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance6 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance9 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance1 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance4 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance5 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance3 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance2 = new Infragistics.Win.Appearance();
            this.gbPrinter = new System.Windows.Forms.GroupBox();
            this.btnProperties = new System.Windows.Forms.Button();
            this.PrinterNm = new TAUtil.TAComboBox();
            this.StatusValue = new System.Windows.Forms.Label();
            this.CommentValue = new System.Windows.Forms.Label();
            this.WhereValue = new System.Windows.Forms.Label();
            this.TypeValue = new System.Windows.Forms.Label();
            this.lblComment = new System.Windows.Forms.Label();
            this.lblWhere = new System.Windows.Forms.Label();
            this.lblType = new System.Windows.Forms.Label();
            this.lblStatus = new System.Windows.Forms.Label();
            this.lblName = new System.Windows.Forms.Label();
            this.gbPaper = new System.Windows.Forms.GroupBox();
            this.lblSource = new System.Windows.Forms.Label();
            this.lblSize = new System.Windows.Forms.Label();
            this.PaperSource = new TAUtil.TAComboBox();
            this.PaperSize = new TAUtil.TAComboBox();
            this.gbOrientation = new System.Windows.Forms.GroupBox();
            this.Preview = new System.Windows.Forms.PictureBox();
            this.Landscape = new System.Windows.Forms.RadioButton();
            this.Portrait = new System.Windows.Forms.RadioButton();
            this.btnSetDefult = new System.Windows.Forms.Button();
            this.btnOK = new System.Windows.Forms.Button();
            this.btnCancel = new System.Windows.Forms.Button();
            this.NoPrinter = new TAUtil.TACheckBoxEditor();
            this.panel1 = new System.Windows.Forms.Panel();
            this.MargingroupBox = new System.Windows.Forms.GroupBox();
            this.MarginIn = new TAUtil.TAComboBox();
            this.RightMarginValue = new TAUtil.TANumericEditor();
            this.BottomMarginValue = new TAUtil.TANumericEditor();
            this.LeftMarginValue = new TAUtil.TANumericEditor();
            this.TopMarginValue = new TAUtil.TANumericEditor();
            this.lblBottom = new System.Windows.Forms.Label();
            this.lblRight = new System.Windows.Forms.Label();
            this.lblLeft = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.lblTop = new System.Windows.Forms.Label();
            this.gbPrinter.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.PrinterNm)).BeginInit();
            this.gbPaper.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.PaperSource)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.PaperSize)).BeginInit();
            this.gbOrientation.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.Preview)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.NoPrinter)).BeginInit();
            this.panel1.SuspendLayout();
            this.MargingroupBox.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.MarginIn)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.RightMarginValue)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.BottomMarginValue)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.LeftMarginValue)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.TopMarginValue)).BeginInit();
            this.SuspendLayout();
            // 
            // gbPrinter
            // 
            this.gbPrinter.Controls.Add(this.btnProperties);
            this.gbPrinter.Controls.Add(this.PrinterNm);
            this.gbPrinter.Controls.Add(this.StatusValue);
            this.gbPrinter.Controls.Add(this.CommentValue);
            this.gbPrinter.Controls.Add(this.WhereValue);
            this.gbPrinter.Controls.Add(this.TypeValue);
            this.gbPrinter.Controls.Add(this.lblComment);
            this.gbPrinter.Controls.Add(this.lblWhere);
            this.gbPrinter.Controls.Add(this.lblType);
            this.gbPrinter.Controls.Add(this.lblStatus);
            this.gbPrinter.Controls.Add(this.lblName);
            this.gbPrinter.Font = new System.Drawing.Font("Calibri", 9.75F, ((System.Drawing.FontStyle)((System.Drawing.FontStyle.Bold | System.Drawing.FontStyle.Italic))), System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.gbPrinter.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            this.gbPrinter.Location = new System.Drawing.Point(14, 39);
            this.gbPrinter.Name = "gbPrinter";
            this.gbPrinter.Size = new System.Drawing.Size(592, 145);
            this.gbPrinter.TabIndex = 0;
            this.gbPrinter.TabStop = false;
            this.gbPrinter.Text = "Printer";
            // 
            // btnProperties
            // 
            this.btnProperties.Location = new System.Drawing.Point(399, 20);
            this.btnProperties.Name = "btnProperties";
            this.btnProperties.Size = new System.Drawing.Size(124, 25);
            this.btnProperties.TabIndex = 1;
            this.btnProperties.Text = "&Properties";
            this.btnProperties.UseVisualStyleBackColor = true;
            this.btnProperties.Click += new System.EventHandler(this.btnProperties_Click);
            // 
            // PrinterNm
            // 
            appearance7.BorderColor = System.Drawing.Color.LightGray;
            this.PrinterNm.Appearance = appearance7;
            this.PrinterNm.AutoCompleteMode = Infragistics.Win.AutoCompleteMode.Append;
            this.PrinterNm.AutoSize = false;
            this.PrinterNm.ComboIsDirty = false;
            this.PrinterNm.DisplayStyle = Infragistics.Win.EmbeddableElementDisplayStyle.Office2007;
            this.PrinterNm.Font = new System.Drawing.Font("Calibri", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.PrinterNm.ItemMatchingMode = Infragistics.Win.ValueListItemMatchingMode.DoNotConvertDataValueToString;
            this.PrinterNm.Location = new System.Drawing.Point(93, 20);
            this.PrinterNm.Name = "PrinterNm";
            this.PrinterNm.Size = new System.Drawing.Size(300, 25);
            this.PrinterNm.TabIndex = 0;
            this.PrinterNm.TextRenderingMode = Infragistics.Win.TextRenderingMode.GDI;
            this.PrinterNm.UserInputText = "";
            this.PrinterNm.CustomUpdate += new System.ComponentModel.CancelEventHandler(this.PrinterName_CustomUpdate);
            this.PrinterNm.ItemNotInList += new Infragistics.Win.UltraWinGrid.ItemNotInListEventHandler(this.Combo_NotInList);
            // 
            // StatusValue
            // 
            this.StatusValue.AutoSize = true;
            this.StatusValue.Font = new System.Drawing.Font("Calibri", 10F, System.Drawing.FontStyle.Italic, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.StatusValue.Location = new System.Drawing.Point(93, 47);
            this.StatusValue.Name = "StatusValue";
            this.StatusValue.Size = new System.Drawing.Size(43, 17);
            this.StatusValue.TabIndex = 2;
            this.StatusValue.Text = "Status";
            // 
            // CommentValue
            // 
            this.CommentValue.AutoSize = true;
            this.CommentValue.Font = new System.Drawing.Font("Calibri", 10F, System.Drawing.FontStyle.Italic, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.CommentValue.Location = new System.Drawing.Point(93, 115);
            this.CommentValue.Name = "CommentValue";
            this.CommentValue.Size = new System.Drawing.Size(97, 17);
            this.CommentValue.TabIndex = 5;
            this.CommentValue.Text = "Comment Value";
            // 
            // WhereValue
            // 
            this.WhereValue.AutoSize = true;
            this.WhereValue.Font = new System.Drawing.Font("Calibri", 10F, System.Drawing.FontStyle.Italic, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.WhereValue.Location = new System.Drawing.Point(93, 91);
            this.WhereValue.Name = "WhereValue";
            this.WhereValue.Size = new System.Drawing.Size(80, 17);
            this.WhereValue.TabIndex = 4;
            this.WhereValue.Text = "Where Value";
            // 
            // TypeValue
            // 
            this.TypeValue.AutoSize = true;
            this.TypeValue.Font = new System.Drawing.Font("Calibri", 10F, System.Drawing.FontStyle.Italic, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.TypeValue.Location = new System.Drawing.Point(93, 69);
            this.TypeValue.Name = "TypeValue";
            this.TypeValue.Size = new System.Drawing.Size(68, 17);
            this.TypeValue.TabIndex = 3;
            this.TypeValue.Text = "Type Value";
            // 
            // lblComment
            // 
            this.lblComment.AutoSize = true;
            this.lblComment.Font = new System.Drawing.Font("Calibri", 10F, System.Drawing.FontStyle.Italic, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblComment.Location = new System.Drawing.Point(17, 115);
            this.lblComment.Name = "lblComment";
            this.lblComment.Size = new System.Drawing.Size(67, 17);
            this.lblComment.TabIndex = 9;
            this.lblComment.Text = "Comment:";
            // 
            // lblWhere
            // 
            this.lblWhere.AutoSize = true;
            this.lblWhere.Font = new System.Drawing.Font("Calibri", 10F, System.Drawing.FontStyle.Italic, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblWhere.Location = new System.Drawing.Point(17, 93);
            this.lblWhere.Name = "lblWhere";
            this.lblWhere.Size = new System.Drawing.Size(50, 17);
            this.lblWhere.TabIndex = 7;
            this.lblWhere.Text = "Where:";
            // 
            // lblType
            // 
            this.lblType.AutoSize = true;
            this.lblType.Font = new System.Drawing.Font("Calibri", 10F, System.Drawing.FontStyle.Italic, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblType.Location = new System.Drawing.Point(17, 70);
            this.lblType.Name = "lblType";
            this.lblType.Size = new System.Drawing.Size(38, 17);
            this.lblType.TabIndex = 5;
            this.lblType.Text = "Type:";
            // 
            // lblStatus
            // 
            this.lblStatus.AutoSize = true;
            this.lblStatus.Font = new System.Drawing.Font("Calibri", 10F, System.Drawing.FontStyle.Italic, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblStatus.Location = new System.Drawing.Point(17, 47);
            this.lblStatus.Name = "lblStatus";
            this.lblStatus.Size = new System.Drawing.Size(47, 17);
            this.lblStatus.TabIndex = 3;
            this.lblStatus.Text = "Status:";
            // 
            // lblName
            // 
            this.lblName.AutoSize = true;
            this.lblName.Font = new System.Drawing.Font("Calibri", 10F, System.Drawing.FontStyle.Italic, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblName.Location = new System.Drawing.Point(17, 24);
            this.lblName.Name = "lblName";
            this.lblName.Size = new System.Drawing.Size(46, 17);
            this.lblName.TabIndex = 0;
            this.lblName.Text = "Name:";
            // 
            // gbPaper
            // 
            this.gbPaper.Controls.Add(this.lblSource);
            this.gbPaper.Controls.Add(this.lblSize);
            this.gbPaper.Controls.Add(this.PaperSource);
            this.gbPaper.Controls.Add(this.PaperSize);
            this.gbPaper.Font = new System.Drawing.Font("Calibri", 9.75F, ((System.Drawing.FontStyle)((System.Drawing.FontStyle.Bold | System.Drawing.FontStyle.Italic))));
            this.gbPaper.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            this.gbPaper.Location = new System.Drawing.Point(14, 190);
            this.gbPaper.Name = "gbPaper";
            this.gbPaper.Size = new System.Drawing.Size(412, 82);
            this.gbPaper.TabIndex = 1;
            this.gbPaper.TabStop = false;
            this.gbPaper.Text = "Paper";
            // 
            // lblSource
            // 
            this.lblSource.AutoSize = true;
            this.lblSource.Font = new System.Drawing.Font("Calibri", 10F, System.Drawing.FontStyle.Italic);
            this.lblSource.Location = new System.Drawing.Point(17, 48);
            this.lblSource.Name = "lblSource";
            this.lblSource.Size = new System.Drawing.Size(50, 17);
            this.lblSource.TabIndex = 2;
            this.lblSource.Text = "Source:";
            // 
            // lblSize
            // 
            this.lblSize.AutoSize = true;
            this.lblSize.Font = new System.Drawing.Font("Calibri", 10F, System.Drawing.FontStyle.Italic);
            this.lblSize.Location = new System.Drawing.Point(17, 25);
            this.lblSize.Name = "lblSize";
            this.lblSize.Size = new System.Drawing.Size(34, 17);
            this.lblSize.TabIndex = 0;
            this.lblSize.Text = "Size:";
            // 
            // PaperSource
            // 
            appearance8.BorderColor = System.Drawing.Color.LightGray;
            appearance8.FontData.Name = "Calibri";
            appearance8.FontData.SizeInPoints = 11F;
            this.PaperSource.Appearance = appearance8;
            this.PaperSource.AutoCompleteMode = Infragistics.Win.AutoCompleteMode.Append;
            this.PaperSource.AutoSize = false;
            this.PaperSource.ComboIsDirty = false;
            this.PaperSource.DisplayStyle = Infragistics.Win.EmbeddableElementDisplayStyle.Office2007;
            this.PaperSource.Font = new System.Drawing.Font("Calibri", 11F);
            this.PaperSource.ItemMatchingMode = Infragistics.Win.ValueListItemMatchingMode.DoNotConvertDataValueToString;
            this.PaperSource.Location = new System.Drawing.Point(93, 44);
            this.PaperSource.Name = "PaperSource";
            this.PaperSource.Size = new System.Drawing.Size(300, 25);
            this.PaperSource.TabIndex = 1;
            this.PaperSource.TextRenderingMode = Infragistics.Win.TextRenderingMode.GDI;
            this.PaperSource.UserInputText = "";
            this.PaperSource.CustomUpdate += new System.ComponentModel.CancelEventHandler(this.PaperSource_CustomUpdate);
            this.PaperSource.ItemNotInList += new Infragistics.Win.UltraWinGrid.ItemNotInListEventHandler(this.Combo_NotInList);
            // 
            // PaperSize
            // 
            appearance6.BorderColor = System.Drawing.Color.LightGray;
            appearance6.FontData.Name = "Calibri";
            appearance6.FontData.SizeInPoints = 11F;
            this.PaperSize.Appearance = appearance6;
            this.PaperSize.AutoCompleteMode = Infragistics.Win.AutoCompleteMode.Append;
            this.PaperSize.AutoSize = false;
            this.PaperSize.ComboIsDirty = false;
            this.PaperSize.DisplayStyle = Infragistics.Win.EmbeddableElementDisplayStyle.Office2007;
            this.PaperSize.Font = new System.Drawing.Font("Calibri", 11F);
            this.PaperSize.ItemMatchingMode = Infragistics.Win.ValueListItemMatchingMode.DoNotConvertDataValueToString;
            this.PaperSize.Location = new System.Drawing.Point(93, 19);
            this.PaperSize.Name = "PaperSize";
            this.PaperSize.Size = new System.Drawing.Size(300, 25);
            this.PaperSize.TabIndex = 0;
            this.PaperSize.TextRenderingMode = Infragistics.Win.TextRenderingMode.GDI;
            this.PaperSize.UserInputText = "";
            this.PaperSize.CustomUpdate += new System.ComponentModel.CancelEventHandler(this.PaperSize_CustomUpdate);
            this.PaperSize.ItemNotInList += new Infragistics.Win.UltraWinGrid.ItemNotInListEventHandler(this.Combo_NotInList);
            // 
            // gbOrientation
            // 
            this.gbOrientation.Controls.Add(this.Preview);
            this.gbOrientation.Controls.Add(this.Landscape);
            this.gbOrientation.Controls.Add(this.Portrait);
            this.gbOrientation.Font = new System.Drawing.Font("Calibri", 9.75F, ((System.Drawing.FontStyle)((System.Drawing.FontStyle.Bold | System.Drawing.FontStyle.Italic))));
            this.gbOrientation.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            this.gbOrientation.Location = new System.Drawing.Point(432, 190);
            this.gbOrientation.Name = "gbOrientation";
            this.gbOrientation.Size = new System.Drawing.Size(174, 82);
            this.gbOrientation.TabIndex = 2;
            this.gbOrientation.TabStop = false;
            this.gbOrientation.Text = "Orientation";
            // 
            // Preview
            // 
            this.Preview.Location = new System.Drawing.Point(17, 19);
            this.Preview.Name = "Preview";
            this.Preview.Size = new System.Drawing.Size(49, 50);
            this.Preview.TabIndex = 1;
            this.Preview.TabStop = false;
            // 
            // Landscape
            // 
            this.Landscape.AutoSize = true;
            this.Landscape.Font = new System.Drawing.Font("Calibri", 10F, System.Drawing.FontStyle.Italic);
            this.Landscape.Location = new System.Drawing.Point(84, 47);
            this.Landscape.Name = "Landscape";
            this.Landscape.Size = new System.Drawing.Size(85, 21);
            this.Landscape.TabIndex = 1;
            this.Landscape.Text = "Landscape";
            this.Landscape.UseVisualStyleBackColor = true;
            this.Landscape.CheckedChanged += new System.EventHandler(this.Landscape_CheckedChanged);
            // 
            // Portrait
            // 
            this.Portrait.AutoSize = true;
            this.Portrait.Checked = true;
            this.Portrait.Font = new System.Drawing.Font("Calibri", 10F, System.Drawing.FontStyle.Italic);
            this.Portrait.Location = new System.Drawing.Point(84, 24);
            this.Portrait.Name = "Portrait";
            this.Portrait.Size = new System.Drawing.Size(70, 21);
            this.Portrait.TabIndex = 0;
            this.Portrait.TabStop = true;
            this.Portrait.Text = "Portrait";
            this.Portrait.UseVisualStyleBackColor = true;
            this.Portrait.CheckedChanged += new System.EventHandler(this.Portrait_CheckedChanged);
            // 
            // btnSetDefult
            // 
            this.btnSetDefult.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.btnSetDefult.Font = new System.Drawing.Font("Calibri", 9.75F, ((System.Drawing.FontStyle)((System.Drawing.FontStyle.Bold | System.Drawing.FontStyle.Italic))));
            this.btnSetDefult.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            this.btnSetDefult.Location = new System.Drawing.Point(15, 380);
            this.btnSetDefult.Name = "btnSetDefult";
            this.btnSetDefult.Size = new System.Drawing.Size(119, 25);
            this.btnSetDefult.TabIndex = 3;
            this.btnSetDefult.Text = "&Reset to Default";
            this.btnSetDefult.UseVisualStyleBackColor = true;
            this.btnSetDefult.Click += new System.EventHandler(this.btnSetDefult_Click);
            // 
            // btnOK
            // 
            this.btnOK.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.btnOK.Font = new System.Drawing.Font("Calibri", 9.75F, ((System.Drawing.FontStyle)((System.Drawing.FontStyle.Bold | System.Drawing.FontStyle.Italic))));
            this.btnOK.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            this.btnOK.Image = global::WinUI.Properties.Resources.OK;
            this.btnOK.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnOK.Location = new System.Drawing.Point(438, 380);
            this.btnOK.Name = "btnOK";
            this.btnOK.Size = new System.Drawing.Size(81, 25);
            this.btnOK.TabIndex = 4;
            this.btnOK.Text = "&OK";
            this.btnOK.UseVisualStyleBackColor = true;
            this.btnOK.Click += new System.EventHandler(this.btnOK_Click);
            // 
            // btnCancel
            // 
            this.btnCancel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.btnCancel.Font = new System.Drawing.Font("Calibri", 9.75F, ((System.Drawing.FontStyle)((System.Drawing.FontStyle.Bold | System.Drawing.FontStyle.Italic))));
            this.btnCancel.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            this.btnCancel.Image = global::WinUI.Properties.Resources.Cancel;
            this.btnCancel.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnCancel.Location = new System.Drawing.Point(525, 380);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(81, 25);
            this.btnCancel.TabIndex = 5;
            this.btnCancel.Text = "&Cancel";
            this.btnCancel.UseVisualStyleBackColor = true;
            this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
            // 
            // NoPrinter
            // 
            appearance9.FontData.ItalicAsString = "True";
            appearance9.FontData.Name = "Calibri";
            appearance9.FontData.SizeInPoints = 10F;
            appearance9.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            this.NoPrinter.Appearance = appearance9;
            this.NoPrinter.cancelUpdate = false;
            this.NoPrinter.Location = new System.Drawing.Point(14, 13);
            this.NoPrinter.Name = "NoPrinter";
            this.NoPrinter.Size = new System.Drawing.Size(120, 20);
            this.NoPrinter.TabIndex = 0;
            this.NoPrinter.Text = "No Printer";
            this.NoPrinter.TextRenderingMode = Infragistics.Win.TextRenderingMode.GDI;
            this.NoPrinter.CheckedChanged += new System.EventHandler(this.NoPrinter_CheckedChanged);
            // 
            // panel1
            // 
            this.panel1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(243)))), ((int)(((byte)(234)))), ((int)(((byte)(226)))));
            this.panel1.Controls.Add(this.MargingroupBox);
            this.panel1.Controls.Add(this.NoPrinter);
            this.panel1.Controls.Add(this.btnSetDefult);
            this.panel1.Controls.Add(this.gbPrinter);
            this.panel1.Controls.Add(this.btnCancel);
            this.panel1.Controls.Add(this.gbPaper);
            this.panel1.Controls.Add(this.btnOK);
            this.panel1.Controls.Add(this.gbOrientation);
            this.panel1.Location = new System.Drawing.Point(12, 12);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(623, 417);
            this.panel1.TabIndex = 0;
            // 
            // MargingroupBox
            // 
            this.MargingroupBox.Controls.Add(this.MarginIn);
            this.MargingroupBox.Controls.Add(this.RightMarginValue);
            this.MargingroupBox.Controls.Add(this.BottomMarginValue);
            this.MargingroupBox.Controls.Add(this.LeftMarginValue);
            this.MargingroupBox.Controls.Add(this.TopMarginValue);
            this.MargingroupBox.Controls.Add(this.lblBottom);
            this.MargingroupBox.Controls.Add(this.lblRight);
            this.MargingroupBox.Controls.Add(this.lblLeft);
            this.MargingroupBox.Controls.Add(this.label3);
            this.MargingroupBox.Controls.Add(this.lblTop);
            this.MargingroupBox.Font = new System.Drawing.Font("Calibri", 9.75F, ((System.Drawing.FontStyle)((System.Drawing.FontStyle.Bold | System.Drawing.FontStyle.Italic))), System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.MargingroupBox.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            this.MargingroupBox.Location = new System.Drawing.Point(15, 278);
            this.MargingroupBox.Name = "MargingroupBox";
            this.MargingroupBox.Size = new System.Drawing.Size(591, 92);
            this.MargingroupBox.TabIndex = 6;
            this.MargingroupBox.TabStop = false;
            this.MargingroupBox.Text = "Margins";
            // 
            // MarginIn
            // 
            appearance1.BorderColor = System.Drawing.Color.LightGray;
            this.MarginIn.Appearance = appearance1;
            this.MarginIn.AutoCompleteMode = Infragistics.Win.AutoCompleteMode.Append;
            this.MarginIn.AutoSize = false;
            this.MarginIn.ComboIsDirty = false;
            this.MarginIn.DisplayStyle = Infragistics.Win.EmbeddableElementDisplayStyle.Office2007;
            this.MarginIn.Font = new System.Drawing.Font("Calibri", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.MarginIn.ItemMatchingMode = Infragistics.Win.ValueListItemMatchingMode.DoNotConvertDataValueToString;
            this.MarginIn.Location = new System.Drawing.Point(92, 31);
            this.MarginIn.Name = "MarginIn";
            this.MarginIn.Size = new System.Drawing.Size(109, 28);
            this.MarginIn.TabIndex = 0;
            this.MarginIn.Text = "Centimeters";
            this.MarginIn.TextRenderingMode = Infragistics.Win.TextRenderingMode.GDI;
            this.MarginIn.UserInputText = "Centimeters";
            // 
            // RightMarginValue
            // 
            appearance4.BorderColor = System.Drawing.Color.LightGray;
            appearance4.TextHAlignAsString = "Right";
            this.RightMarginValue.Appearance = appearance4;
            this.RightMarginValue.AutoSize = false;
            this.RightMarginValue.DisplayStyle = Infragistics.Win.EmbeddableElementDisplayStyle.Office2007;
            this.RightMarginValue.Font = new System.Drawing.Font("Calibri", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.RightMarginValue.ForceExitByRestoreValue = false;
            this.RightMarginValue.Format = "0.##";
            this.RightMarginValue.Location = new System.Drawing.Point(462, 47);
            this.RightMarginValue.Name = "RightMarginValue";
            this.RightMarginValue.NullText = "1";
            this.RightMarginValue.NumberType = TAUtil.NumericType.Decimal;
            this.RightMarginValue.Size = new System.Drawing.Size(109, 27);
            this.RightMarginValue.TabIndex = 4;
            this.RightMarginValue.Text = "1";
            this.RightMarginValue.TextRenderingMode = Infragistics.Win.TextRenderingMode.GDI;
            this.RightMarginValue.ZeroIfEmpty = false;
            // 
            // BottomMarginValue
            // 
            appearance5.BorderColor = System.Drawing.Color.LightGray;
            appearance5.TextHAlignAsString = "Right";
            this.BottomMarginValue.Appearance = appearance5;
            this.BottomMarginValue.AutoSize = false;
            this.BottomMarginValue.DisplayStyle = Infragistics.Win.EmbeddableElementDisplayStyle.Office2007;
            this.BottomMarginValue.Font = new System.Drawing.Font("Calibri", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.BottomMarginValue.ForceExitByRestoreValue = false;
            this.BottomMarginValue.Format = "0.##";
            this.BottomMarginValue.Location = new System.Drawing.Point(462, 20);
            this.BottomMarginValue.Name = "BottomMarginValue";
            this.BottomMarginValue.NullText = "1";
            this.BottomMarginValue.NumberType = TAUtil.NumericType.Decimal;
            this.BottomMarginValue.Size = new System.Drawing.Size(109, 27);
            this.BottomMarginValue.TabIndex = 3;
            this.BottomMarginValue.Text = "1";
            this.BottomMarginValue.TextRenderingMode = Infragistics.Win.TextRenderingMode.GDI;
            this.BottomMarginValue.ZeroIfEmpty = false;
            // 
            // LeftMarginValue
            // 
            appearance3.BorderColor = System.Drawing.Color.LightGray;
            appearance3.TextHAlignAsString = "Right";
            this.LeftMarginValue.Appearance = appearance3;
            this.LeftMarginValue.AutoSize = false;
            this.LeftMarginValue.DisplayStyle = Infragistics.Win.EmbeddableElementDisplayStyle.Office2007;
            this.LeftMarginValue.Font = new System.Drawing.Font("Calibri", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.LeftMarginValue.ForceExitByRestoreValue = false;
            this.LeftMarginValue.Format = "0.##";
            this.LeftMarginValue.Location = new System.Drawing.Point(271, 47);
            this.LeftMarginValue.Name = "LeftMarginValue";
            this.LeftMarginValue.NullText = "1";
            this.LeftMarginValue.NumberType = TAUtil.NumericType.Decimal;
            this.LeftMarginValue.Size = new System.Drawing.Size(109, 27);
            this.LeftMarginValue.TabIndex = 2;
            this.LeftMarginValue.Text = "1";
            this.LeftMarginValue.TextRenderingMode = Infragistics.Win.TextRenderingMode.GDI;
            this.LeftMarginValue.ZeroIfEmpty = false;
            // 
            // TopMarginValue
            // 
            appearance2.BorderColor = System.Drawing.Color.LightGray;
            appearance2.TextHAlignAsString = "Right";
            this.TopMarginValue.Appearance = appearance2;
            this.TopMarginValue.AutoSize = false;
            this.TopMarginValue.DisplayStyle = Infragistics.Win.EmbeddableElementDisplayStyle.Office2007;
            this.TopMarginValue.Font = new System.Drawing.Font("Calibri", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.TopMarginValue.ForceExitByRestoreValue = false;
            this.TopMarginValue.Format = "0.##";
            this.TopMarginValue.Location = new System.Drawing.Point(271, 20);
            this.TopMarginValue.Name = "TopMarginValue";
            this.TopMarginValue.NullText = "1";
            this.TopMarginValue.NumberType = TAUtil.NumericType.Decimal;
            this.TopMarginValue.Size = new System.Drawing.Size(109, 27);
            this.TopMarginValue.TabIndex = 1;
            this.TopMarginValue.Text = "1";
            this.TopMarginValue.TextRenderingMode = Infragistics.Win.TextRenderingMode.GDI;
            this.TopMarginValue.ZeroIfEmpty = false;
            // 
            // lblBottom
            // 
            this.lblBottom.AutoSize = true;
            this.lblBottom.Font = new System.Drawing.Font("Calibri", 10F, System.Drawing.FontStyle.Italic);
            this.lblBottom.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            this.lblBottom.Location = new System.Drawing.Point(396, 26);
            this.lblBottom.Name = "lblBottom";
            this.lblBottom.Size = new System.Drawing.Size(50, 17);
            this.lblBottom.TabIndex = 6;
            this.lblBottom.Text = "Bottom";
            // 
            // lblRight
            // 
            this.lblRight.AutoSize = true;
            this.lblRight.Font = new System.Drawing.Font("Calibri", 10F, System.Drawing.FontStyle.Italic);
            this.lblRight.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            this.lblRight.Location = new System.Drawing.Point(396, 53);
            this.lblRight.Name = "lblRight";
            this.lblRight.Size = new System.Drawing.Size(38, 17);
            this.lblRight.TabIndex = 8;
            this.lblRight.Text = "Right";
            // 
            // lblLeft
            // 
            this.lblLeft.AutoSize = true;
            this.lblLeft.Font = new System.Drawing.Font("Calibri", 10F, System.Drawing.FontStyle.Italic);
            this.lblLeft.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            this.lblLeft.Location = new System.Drawing.Point(224, 53);
            this.lblLeft.Name = "lblLeft";
            this.lblLeft.Size = new System.Drawing.Size(30, 17);
            this.lblLeft.TabIndex = 4;
            this.lblLeft.Text = "Left";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Font = new System.Drawing.Font("Calibri", 10F, System.Drawing.FontStyle.Italic);
            this.label3.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            this.label3.Location = new System.Drawing.Point(24, 38);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(62, 17);
            this.label3.TabIndex = 0;
            this.label3.Text = "Margin in";
            // 
            // lblTop
            // 
            this.lblTop.AutoSize = true;
            this.lblTop.Font = new System.Drawing.Font("Calibri", 10F, System.Drawing.FontStyle.Italic);
            this.lblTop.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            this.lblTop.Location = new System.Drawing.Point(224, 26);
            this.lblTop.Name = "lblTop";
            this.lblTop.Size = new System.Drawing.Size(28, 17);
            this.lblTop.TabIndex = 2;
            this.lblTop.Text = "Top";
            // 
            // frmPageSetup
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.White;
            this.ClientSize = new System.Drawing.Size(644, 441);
            this.Controls.Add(this.panel1);
            this.KeyPreview = true;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.MinimumSize = new System.Drawing.Size(539, 349);
            this.Name = "frmPageSetup";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Page Setup";
            this.Load += new System.EventHandler(this.frmPageSetup_Load);
            this.KeyDown += new System.Windows.Forms.KeyEventHandler(this.frmPageSetup_KeyDown);
            this.gbPrinter.ResumeLayout(false);
            this.gbPrinter.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.PrinterNm)).EndInit();
            this.gbPaper.ResumeLayout(false);
            this.gbPaper.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.PaperSource)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.PaperSize)).EndInit();
            this.gbOrientation.ResumeLayout(false);
            this.gbOrientation.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.Preview)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.NoPrinter)).EndInit();
            this.panel1.ResumeLayout(false);
            this.MargingroupBox.ResumeLayout(false);
            this.MargingroupBox.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.MarginIn)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.RightMarginValue)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.BottomMarginValue)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.LeftMarginValue)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.TopMarginValue)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private TAUtil.TACheckBoxEditor NoPrinter;
        private System.Windows.Forms.GroupBox gbPrinter;
        private System.Windows.Forms.Button btnProperties;
        private TAUtil.TAComboBox PrinterNm;
        private System.Windows.Forms.Label StatusValue;
        private System.Windows.Forms.Label WhereValue;
        private System.Windows.Forms.Label TypeValue;
        private System.Windows.Forms.Label lblWhere;
        private System.Windows.Forms.Label lblType;
        private System.Windows.Forms.Label lblStatus;
        private System.Windows.Forms.Label lblName;
        private System.Windows.Forms.Label CommentValue;
        private System.Windows.Forms.Label lblComment;
        private System.Windows.Forms.GroupBox gbPaper;
        private System.Windows.Forms.Label lblSource;
        private System.Windows.Forms.Label lblSize;
        private TAUtil.TAComboBox PaperSource;
        private TAUtil.TAComboBox PaperSize;
        private System.Windows.Forms.GroupBox gbOrientation;
        private System.Windows.Forms.RadioButton Landscape;
        private System.Windows.Forms.RadioButton Portrait;
        private System.Windows.Forms.Button btnSetDefult;
        private System.Windows.Forms.Button btnOK;
        private System.Windows.Forms.Button btnCancel;
        private System.Windows.Forms.PictureBox Preview;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.GroupBox MargingroupBox;
        private TAUtil.TAComboBox MarginIn;
        private TAUtil.TANumericEditor RightMarginValue;
        private TAUtil.TANumericEditor BottomMarginValue;
        private TAUtil.TANumericEditor LeftMarginValue;
        private TAUtil.TANumericEditor TopMarginValue;
        private System.Windows.Forms.Label lblBottom;
        private System.Windows.Forms.Label lblRight;
        private System.Windows.Forms.Label lblLeft;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label lblTop;
    }
}