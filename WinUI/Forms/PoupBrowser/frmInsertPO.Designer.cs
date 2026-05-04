namespace WinUI
{
    partial class frmInsertPO
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
            Infragistics.Win.Appearance appearance28 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance8 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance6 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance1 = new Infragistics.Win.Appearance();
            Infragistics.Win.UltraWinGrid.UltraGridBand ultraGridBand1 = new Infragistics.Win.UltraWinGrid.UltraGridBand("", -1);
            Infragistics.Win.UltraWinGrid.UltraGridColumn ultraGridColumn1 = new Infragistics.Win.UltraWinGrid.UltraGridColumn("DocKey", 0);
            Infragistics.Win.UltraWinGrid.UltraGridColumn ultraGridColumn2 = new Infragistics.Win.UltraWinGrid.UltraGridColumn("DocCodeKey", 1);
            Infragistics.Win.UltraWinGrid.UltraGridColumn ultraGridColumn3 = new Infragistics.Win.UltraWinGrid.UltraGridColumn("DocID", 2, null, 0, Infragistics.Win.UltraWinGrid.SortIndicator.Ascending, false);
            Infragistics.Win.UltraWinGrid.UltraGridColumn ultraGridColumn4 = new Infragistics.Win.UltraWinGrid.UltraGridColumn("DocDate", 3);
            Infragistics.Win.UltraWinGrid.UltraGridColumn ultraGridColumn5 = new Infragistics.Win.UltraWinGrid.UltraGridColumn("DocCustPONum", 4);
            Infragistics.Win.UltraWinGrid.UltraGridColumn ultraGridColumn6 = new Infragistics.Win.UltraWinGrid.UltraGridColumn("DocRef", 5);
            Infragistics.Win.UltraWinGrid.UltraGridColumn ultraGridColumn7 = new Infragistics.Win.UltraWinGrid.UltraGridColumn("DocItmKey", 6);
            Infragistics.Win.UltraWinGrid.UltraGridColumn ultraGridColumn8 = new Infragistics.Win.UltraWinGrid.UltraGridColumn("LineType", 7);
            Infragistics.Win.UltraWinGrid.UltraGridColumn ultraGridColumn9 = new Infragistics.Win.UltraWinGrid.UltraGridColumn("LineLinkKey", 8);
            Infragistics.Win.UltraWinGrid.UltraGridColumn ultraGridColumn10 = new Infragistics.Win.UltraWinGrid.UltraGridColumn("ItmSN", 9);
            Infragistics.Win.UltraWinGrid.UltraGridColumn ultraGridColumn11 = new Infragistics.Win.UltraWinGrid.UltraGridColumn("ItmKey", 10);
            Infragistics.Win.UltraWinGrid.UltraGridColumn ultraGridColumn12 = new Infragistics.Win.UltraWinGrid.UltraGridColumn("ItmKeySelect", 11);
            Infragistics.Win.UltraWinGrid.UltraGridColumn ultraGridColumn13 = new Infragistics.Win.UltraWinGrid.UltraGridColumn("ItmType", 12);
            Infragistics.Win.UltraWinGrid.UltraGridColumn ultraGridColumn14 = new Infragistics.Win.UltraWinGrid.UltraGridColumn("ItmDes", 13);
            Infragistics.Win.UltraWinGrid.UltraGridColumn ultraGridColumn15 = new Infragistics.Win.UltraWinGrid.UltraGridColumn("ItmDeptKey", 14);
            Infragistics.Win.UltraWinGrid.UltraGridColumn ultraGridColumn16 = new Infragistics.Win.UltraWinGrid.UltraGridColumn("ItmTranGrpKey", 15);
            Infragistics.Win.UltraWinGrid.UltraGridColumn ultraGridColumn17 = new Infragistics.Win.UltraWinGrid.UltraGridColumn("ItmAccKey", 16);
            Infragistics.Win.UltraWinGrid.UltraGridColumn ultraGridColumn18 = new Infragistics.Win.UltraWinGrid.UltraGridColumn("ItmLocKey", 17);
            Infragistics.Win.UltraWinGrid.UltraGridColumn ultraGridColumn19 = new Infragistics.Win.UltraWinGrid.UltraGridColumn("ItmStock", 18);
            Infragistics.Win.UltraWinGrid.UltraGridColumn ultraGridColumn20 = new Infragistics.Win.UltraWinGrid.UltraGridColumn("ItmQty", 19);
            Infragistics.Win.UltraWinGrid.UltraGridColumn ultraGridColumn21 = new Infragistics.Win.UltraWinGrid.UltraGridColumn("ItmDeliver", 20);
            Infragistics.Win.UltraWinGrid.UltraGridColumn ultraGridColumn22 = new Infragistics.Win.UltraWinGrid.UltraGridColumn("QtyBalance", 21);
            Infragistics.Win.UltraWinGrid.UltraGridColumn ultraGridColumn23 = new Infragistics.Win.UltraWinGrid.UltraGridColumn("ItmOrderStatus", 22);
            Infragistics.Win.UltraWinGrid.UltraGridColumn ultraGridColumn24 = new Infragistics.Win.UltraWinGrid.UltraGridColumn("ItmReqDate", 23);
            Infragistics.Win.UltraWinGrid.UltraGridColumn ultraGridColumn25 = new Infragistics.Win.UltraWinGrid.UltraGridColumn("ItmPrmDate", 24);
            Infragistics.Win.UltraWinGrid.UltraGridColumn ultraGridColumn26 = new Infragistics.Win.UltraWinGrid.UltraGridColumn("ItmUOMKey", 25);
            Infragistics.Win.UltraWinGrid.UltraGridColumn ultraGridColumn27 = new Infragistics.Win.UltraWinGrid.UltraGridColumn("ItmConRate", 26);
            Infragistics.Win.UltraWinGrid.UltraGridColumn ultraGridColumn28 = new Infragistics.Win.UltraWinGrid.UltraGridColumn("ItmLatestCostF", 27);
            Infragistics.Win.UltraWinGrid.UltraGridColumn ultraGridColumn29 = new Infragistics.Win.UltraWinGrid.UltraGridColumn("ItmLatestCostH", 28);
            Infragistics.Win.UltraWinGrid.UltraGridColumn ultraGridColumn30 = new Infragistics.Win.UltraWinGrid.UltraGridColumn("ItmListPrice", 29);
            Infragistics.Win.UltraWinGrid.UltraGridColumn ultraGridColumn31 = new Infragistics.Win.UltraWinGrid.UltraGridColumn("ItmPriceBefore", 30);
            Infragistics.Win.UltraWinGrid.UltraGridColumn ultraGridColumn32 = new Infragistics.Win.UltraWinGrid.UltraGridColumn("ItmPriceAfter", 31);
            Infragistics.Win.UltraWinGrid.UltraGridColumn ultraGridColumn33 = new Infragistics.Win.UltraWinGrid.UltraGridColumn("ItmDisPercent", 32);
            Infragistics.Win.UltraWinGrid.UltraGridColumn ultraGridColumn34 = new Infragistics.Win.UltraWinGrid.UltraGridColumn("ItmDisValue", 33);
            Infragistics.Win.UltraWinGrid.UltraGridColumn ultraGridColumn35 = new Infragistics.Win.UltraWinGrid.UltraGridColumn("ItmPrice", 34);
            Infragistics.Win.UltraWinGrid.UltraGridColumn ultraGridColumn36 = new Infragistics.Win.UltraWinGrid.UltraGridColumn("ItmPriceUser", 35);
            Infragistics.Win.UltraWinGrid.UltraGridColumn ultraGridColumn37 = new Infragistics.Win.UltraWinGrid.UltraGridColumn("ItmAmtShw", 36);
            Infragistics.Win.UltraWinGrid.UltraGridColumn ultraGridColumn38 = new Infragistics.Win.UltraWinGrid.UltraGridColumn("ItmAmtF", 37);
            Infragistics.Win.UltraWinGrid.UltraGridColumn ultraGridColumn39 = new Infragistics.Win.UltraWinGrid.UltraGridColumn("ItmAmtH", 38);
            Infragistics.Win.UltraWinGrid.UltraGridColumn ultraGridColumn40 = new Infragistics.Win.UltraWinGrid.UltraGridColumn("ItmTaxable", 39);
            Infragistics.Win.UltraWinGrid.UltraGridColumn ultraGridColumn41 = new Infragistics.Win.UltraWinGrid.UltraGridColumn("ItmTaxGrpKey", 40);
            Infragistics.Win.UltraWinGrid.UltraGridColumn ultraGridColumn42 = new Infragistics.Win.UltraWinGrid.UltraGridColumn("ItmTaxGrpRate", 41);
            Infragistics.Win.UltraWinGrid.UltraGridColumn ultraGridColumn43 = new Infragistics.Win.UltraWinGrid.UltraGridColumn("ItmTaxGrpAmtF", 42);
            Infragistics.Win.UltraWinGrid.UltraGridColumn ultraGridColumn44 = new Infragistics.Win.UltraWinGrid.UltraGridColumn("ItmTaxGrpAmtL", 43);
            Infragistics.Win.UltraWinGrid.UltraGridColumn ultraGridColumn45 = new Infragistics.Win.UltraWinGrid.UltraGridColumn("ItmHide", 44);
            Infragistics.Win.UltraWinGrid.UltraGridColumn ultraGridColumn46 = new Infragistics.Win.UltraWinGrid.UltraGridColumn("ItmColorKey", 45);
            Infragistics.Win.UltraWinGrid.UltraGridColumn ultraGridColumn47 = new Infragistics.Win.UltraWinGrid.UltraGridColumn("ItmScaleSize", 46);
            Infragistics.Win.UltraWinGrid.UltraGridColumn ultraGridColumn48 = new Infragistics.Win.UltraWinGrid.UltraGridColumn("ItmPacking", 47);
            Infragistics.Win.UltraWinGrid.UltraGridColumn ultraGridColumn49 = new Infragistics.Win.UltraWinGrid.UltraGridColumn("ItmRem", 48);
            Infragistics.Win.UltraWinGrid.UltraGridColumn ultraGridColumn50 = new Infragistics.Win.UltraWinGrid.UltraGridColumn("ItmMark", 49);
            Infragistics.Win.UltraWinGrid.UltraGridColumn ultraGridColumn51 = new Infragistics.Win.UltraWinGrid.UltraGridColumn("ItmJobKey", 50);
            Infragistics.Win.UltraWinGrid.UltraGridColumn ultraGridColumn52 = new Infragistics.Win.UltraWinGrid.UltraGridColumn("ItmJobPhaseKey", 51);
            Infragistics.Win.UltraWinGrid.UltraGridColumn ultraGridColumn53 = new Infragistics.Win.UltraWinGrid.UltraGridColumn("ItmJobTaskKey", 52);
            Infragistics.Win.UltraWinGrid.UltraGridColumn ultraGridColumn54 = new Infragistics.Win.UltraWinGrid.UltraGridColumn("ItmJobCostTypeKey", 53);
            Infragistics.Win.UltraWinGrid.UltraGridColumn ultraGridColumn55 = new Infragistics.Win.UltraWinGrid.UltraGridColumn("ConfirmID", 54);
            Infragistics.Win.UltraWinGrid.UltraGridColumn ultraGridColumn56 = new Infragistics.Win.UltraWinGrid.UltraGridColumn("ConfirmSN", 55);
            Infragistics.Win.UltraWinGrid.UltraGridColumn ultraGridColumn57 = new Infragistics.Win.UltraWinGrid.UltraGridColumn("ItmAttachment", 56);
            Infragistics.Win.UltraWinGrid.UltraGridColumn ultraGridColumn58 = new Infragistics.Win.UltraWinGrid.UltraGridColumn("ItmBatchKey", 57);
            Infragistics.Win.UltraWinGrid.UltraGridColumn ultraGridColumn59 = new Infragistics.Win.UltraWinGrid.UltraGridColumn("ItmBatchQty", 58);
            Infragistics.Win.UltraWinGrid.UltraGridColumn ultraGridColumn60 = new Infragistics.Win.UltraWinGrid.UltraGridColumn("NSLink", 59);
            Infragistics.Win.UltraWinGrid.UltraGridColumn ultraGridColumn61 = new Infragistics.Win.UltraWinGrid.UltraGridColumn("ARQOID", 60);
            Infragistics.Win.UltraWinGrid.UltraGridColumn ultraGridColumn62 = new Infragistics.Win.UltraWinGrid.UltraGridColumn("ARQODK", 61);
            Infragistics.Win.UltraWinGrid.UltraGridColumn ultraGridColumn63 = new Infragistics.Win.UltraWinGrid.UltraGridColumn("ARQODItm", 62);
            Infragistics.Win.UltraWinGrid.UltraGridColumn ultraGridColumn64 = new Infragistics.Win.UltraWinGrid.UltraGridColumn("ARSOID", 63);
            Infragistics.Win.UltraWinGrid.UltraGridColumn ultraGridColumn65 = new Infragistics.Win.UltraWinGrid.UltraGridColumn("ARSODK", 64);
            Infragistics.Win.UltraWinGrid.UltraGridColumn ultraGridColumn66 = new Infragistics.Win.UltraWinGrid.UltraGridColumn("ARSODItm", 65);
            Infragistics.Win.UltraWinGrid.UltraGridColumn ultraGridColumn67 = new Infragistics.Win.UltraWinGrid.UltraGridColumn("ARSOPOID", 66);
            Infragistics.Win.UltraWinGrid.UltraGridColumn ultraGridColumn68 = new Infragistics.Win.UltraWinGrid.UltraGridColumn("ARDOID", 67);
            Infragistics.Win.UltraWinGrid.UltraGridColumn ultraGridColumn69 = new Infragistics.Win.UltraWinGrid.UltraGridColumn("ARDODK", 68);
            Infragistics.Win.UltraWinGrid.UltraGridColumn ultraGridColumn70 = new Infragistics.Win.UltraWinGrid.UltraGridColumn("ARDODItm", 69);
            Infragistics.Win.UltraWinGrid.UltraGridColumn ultraGridColumn71 = new Infragistics.Win.UltraWinGrid.UltraGridColumn("ARIVID", 70);
            Infragistics.Win.UltraWinGrid.UltraGridColumn ultraGridColumn72 = new Infragistics.Win.UltraWinGrid.UltraGridColumn("ARIVDK", 71);
            Infragistics.Win.UltraWinGrid.UltraGridColumn ultraGridColumn73 = new Infragistics.Win.UltraWinGrid.UltraGridColumn("ARIVDItm", 72);
            Infragistics.Win.UltraWinGrid.UltraGridColumn ultraGridColumn74 = new Infragistics.Win.UltraWinGrid.UltraGridColumn("CostLatest", 73);
            Infragistics.Win.UltraWinGrid.UltraGridColumn ultraGridColumn75 = new Infragistics.Win.UltraWinGrid.UltraGridColumn("UOMID", 74);
            Infragistics.Win.UltraWinGrid.UltraGridColumn ultraGridColumn76 = new Infragistics.Win.UltraWinGrid.UltraGridColumn("ItmID", 75);
            Infragistics.Win.UltraWinGrid.UltraGridColumn ultraGridColumn77 = new Infragistics.Win.UltraWinGrid.UltraGridColumn("PickSeq", 76);
            Infragistics.Win.UltraWinGrid.UltraGridColumn ultraGridColumn78 = new Infragistics.Win.UltraWinGrid.UltraGridColumn("FinalPrice", 77);
            Infragistics.Win.UltraWinGrid.UltraGridColumn ultraGridColumn79 = new Infragistics.Win.UltraWinGrid.UltraGridColumn("Selected", 78);
            Infragistics.Win.UltraWinGrid.UltraGridColumn ultraGridColumn80 = new Infragistics.Win.UltraWinGrid.UltraGridColumn("ItmQtyReceive", 79);
            Infragistics.Win.Appearance appearance3 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance4 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance5 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance26 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance30 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance32 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance33 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance34 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance35 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance36 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance37 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance38 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance39 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance13 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance12 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance31 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance29 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance27 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance148 = new Infragistics.Win.Appearance();
            Infragistics.Win.UltraWinEditors.EditorButton editorButton1 = new Infragistics.Win.UltraWinEditors.EditorButton();
            Infragistics.Win.Appearance appearance149 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance7 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance10 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance9 = new Infragistics.Win.Appearance();
            this.ultraLabel1 = new Infragistics.Win.Misc.UltraLabel();
            this.DocKey = new TAUtil.TAComboBox();
            this.UseSystemPrice = new TAUtil.TACheckBoxEditor();
            this.tagrdItemPO = new TAUtil.TAGridEditor();
            this.btnClose = new Infragistics.Win.Misc.UltraButton();
            this.btnAppend = new Infragistics.Win.Misc.UltraButton();
            this.ultraLabel2 = new Infragistics.Win.Misc.UltraLabel();
            this.ultraLabel3 = new Infragistics.Win.Misc.UltraLabel();
            this.ItmKey = new TAUtil.TAComboBox();
            this.PrmDate = new TAUtil.TADateEditor();
            this.PickSeq = new TAUtil.TACheckBoxEditor();
            this.panel1 = new System.Windows.Forms.Panel();
            this.btnUnSelectAll = new Infragistics.Win.Misc.UltraButton();
            this.btnSelectAll = new Infragistics.Win.Misc.UltraButton();
            this.panel2 = new System.Windows.Forms.Panel();
            ((System.ComponentModel.ISupportInitialize)(this.DocKey)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.UseSystemPrice)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.tagrdItemPO)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.ItmKey)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.PrmDate)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.PickSeq)).BeginInit();
            this.panel1.SuspendLayout();
            this.panel2.SuspendLayout();
            this.SuspendLayout();
            // 
            // ultraLabel1
            // 
            appearance28.BackColor = System.Drawing.Color.Transparent;
            appearance28.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            appearance28.TextVAlignAsString = "Middle";
            this.ultraLabel1.Appearance = appearance28;
            this.ultraLabel1.Font = new System.Drawing.Font("Calibri", 10F, System.Drawing.FontStyle.Italic);
            this.ultraLabel1.Location = new System.Drawing.Point(16, 13);
            this.ultraLabel1.Name = "ultraLabel1";
            this.ultraLabel1.Size = new System.Drawing.Size(79, 23);
            this.ultraLabel1.TabIndex = 48;
            this.ultraLabel1.Text = "ETA Date <=";
            this.ultraLabel1.TextRenderingMode = Infragistics.Win.TextRenderingMode.GDI;
            // 
            // DocKey
            // 
            appearance8.BackColor = System.Drawing.Color.White;
            appearance8.BorderColor = System.Drawing.Color.LightGray;
            appearance8.FontData.Name = "Calibri";
            appearance8.FontData.SizeInPoints = 11F;
            appearance8.ForeColor = System.Drawing.Color.Black;
            this.DocKey.Appearance = appearance8;
            this.DocKey.AutoCompleteMode = Infragistics.Win.AutoCompleteMode.Append;
            this.DocKey.AutoSize = false;
            this.DocKey.ComboIsDirty = false;
            this.DocKey.DisplayStyle = Infragistics.Win.EmbeddableElementDisplayStyle.Office2007;
            this.DocKey.Font = new System.Drawing.Font("Tahoma", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.DocKey.ItemMatchingMode = Infragistics.Win.ValueListItemMatchingMode.DoNotConvertDataValueToString;
            this.DocKey.Location = new System.Drawing.Point(119, 38);
            this.DocKey.Name = "DocKey";
            this.DocKey.Size = new System.Drawing.Size(300, 25);
            this.DocKey.TabIndex = 47;
            this.DocKey.TextRenderingMode = Infragistics.Win.TextRenderingMode.GDI;
            this.DocKey.UserInputText = "";
            this.DocKey.CustomUpdate += new System.ComponentModel.CancelEventHandler(this.DocKey_CustomUpdate);
            this.DocKey.ItemNotInList += new Infragistics.Win.UltraWinGrid.ItemNotInListEventHandler(this.Combo_NotInList);
            // 
            // UseSystemPrice
            // 
            appearance6.FontData.ItalicAsString = "True";
            appearance6.FontData.Name = "Calibri";
            appearance6.FontData.SizeInPoints = 10F;
            appearance6.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            this.UseSystemPrice.Appearance = appearance6;
            this.UseSystemPrice.cancelUpdate = false;
            this.UseSystemPrice.Location = new System.Drawing.Point(20, 5);
            this.UseSystemPrice.Name = "UseSystemPrice";
            this.UseSystemPrice.Size = new System.Drawing.Size(149, 20);
            this.UseSystemPrice.TabIndex = 51;
            this.UseSystemPrice.Text = "Use System Pricing";
            this.UseSystemPrice.TextRenderingMode = Infragistics.Win.TextRenderingMode.GDI;
            // 
            // tagrdItemPO
            // 
            this.tagrdItemPO.ActiveConnection = null;
            this.tagrdItemPO.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.tagrdItemPO.AutoAddNewRow = false;
            this.tagrdItemPO.AutoUseCustomControlsInCells = true;
            this.tagrdItemPO.DefaultValue = null;
            this.tagrdItemPO.DetailObjectKey = 0;
            appearance1.AlphaLevel = ((short)(255));
            appearance1.BackColor = System.Drawing.Color.AliceBlue;
            appearance1.BackColor2 = System.Drawing.Color.AliceBlue;
            appearance1.BorderColor = System.Drawing.Color.LightSteelBlue;
            appearance1.BorderColor2 = System.Drawing.Color.LightSteelBlue;
            this.tagrdItemPO.DisplayLayout.Appearance = appearance1;
            ultraGridColumn1.AutoCompleteMode = Infragistics.Win.AutoCompleteMode.Append;
            ultraGridColumn1.Header.VisiblePosition = 0;
            ultraGridColumn2.AutoCompleteMode = Infragistics.Win.AutoCompleteMode.Append;
            ultraGridColumn2.Header.VisiblePosition = 1;
            ultraGridColumn3.AutoCompleteMode = Infragistics.Win.AutoCompleteMode.Append;
            ultraGridColumn3.Header.VisiblePosition = 2;
            ultraGridColumn4.AutoCompleteMode = Infragistics.Win.AutoCompleteMode.Append;
            ultraGridColumn4.Header.VisiblePosition = 3;
            ultraGridColumn5.AutoCompleteMode = Infragistics.Win.AutoCompleteMode.Append;
            ultraGridColumn5.Header.VisiblePosition = 4;
            ultraGridColumn6.AutoCompleteMode = Infragistics.Win.AutoCompleteMode.Append;
            ultraGridColumn6.Header.VisiblePosition = 5;
            ultraGridColumn7.AutoCompleteMode = Infragistics.Win.AutoCompleteMode.Append;
            ultraGridColumn7.Header.VisiblePosition = 6;
            ultraGridColumn8.AutoCompleteMode = Infragistics.Win.AutoCompleteMode.Append;
            ultraGridColumn8.Header.VisiblePosition = 7;
            ultraGridColumn9.AutoCompleteMode = Infragistics.Win.AutoCompleteMode.Append;
            ultraGridColumn9.Header.VisiblePosition = 8;
            ultraGridColumn10.AutoCompleteMode = Infragistics.Win.AutoCompleteMode.Append;
            ultraGridColumn10.Header.VisiblePosition = 9;
            ultraGridColumn11.AutoCompleteMode = Infragistics.Win.AutoCompleteMode.Append;
            ultraGridColumn11.Header.VisiblePosition = 10;
            ultraGridColumn12.AutoCompleteMode = Infragistics.Win.AutoCompleteMode.Append;
            ultraGridColumn12.Header.VisiblePosition = 11;
            ultraGridColumn13.AutoCompleteMode = Infragistics.Win.AutoCompleteMode.Append;
            ultraGridColumn13.Header.VisiblePosition = 12;
            ultraGridColumn14.AutoCompleteMode = Infragistics.Win.AutoCompleteMode.Append;
            ultraGridColumn14.Header.VisiblePosition = 13;
            ultraGridColumn15.AutoCompleteMode = Infragistics.Win.AutoCompleteMode.Append;
            ultraGridColumn15.Header.VisiblePosition = 14;
            ultraGridColumn16.AutoCompleteMode = Infragistics.Win.AutoCompleteMode.Append;
            ultraGridColumn16.Header.VisiblePosition = 15;
            ultraGridColumn17.AutoCompleteMode = Infragistics.Win.AutoCompleteMode.Append;
            ultraGridColumn17.Header.VisiblePosition = 16;
            ultraGridColumn18.AutoCompleteMode = Infragistics.Win.AutoCompleteMode.Append;
            ultraGridColumn18.Header.VisiblePosition = 17;
            ultraGridColumn19.AutoCompleteMode = Infragistics.Win.AutoCompleteMode.Append;
            ultraGridColumn19.Header.VisiblePosition = 18;
            ultraGridColumn20.AutoCompleteMode = Infragistics.Win.AutoCompleteMode.Append;
            ultraGridColumn20.Header.VisiblePosition = 19;
            ultraGridColumn21.AutoCompleteMode = Infragistics.Win.AutoCompleteMode.Append;
            ultraGridColumn21.Header.VisiblePosition = 20;
            ultraGridColumn22.AutoCompleteMode = Infragistics.Win.AutoCompleteMode.Append;
            ultraGridColumn22.Header.VisiblePosition = 21;
            ultraGridColumn23.AutoCompleteMode = Infragistics.Win.AutoCompleteMode.Append;
            ultraGridColumn23.Header.VisiblePosition = 22;
            ultraGridColumn24.AutoCompleteMode = Infragistics.Win.AutoCompleteMode.Append;
            ultraGridColumn24.Header.VisiblePosition = 23;
            ultraGridColumn25.AutoCompleteMode = Infragistics.Win.AutoCompleteMode.Append;
            ultraGridColumn25.Header.VisiblePosition = 24;
            ultraGridColumn26.AutoCompleteMode = Infragistics.Win.AutoCompleteMode.Append;
            ultraGridColumn26.Header.VisiblePosition = 25;
            ultraGridColumn27.AutoCompleteMode = Infragistics.Win.AutoCompleteMode.Append;
            ultraGridColumn27.Header.VisiblePosition = 26;
            ultraGridColumn28.AutoCompleteMode = Infragistics.Win.AutoCompleteMode.Append;
            ultraGridColumn28.Header.VisiblePosition = 27;
            ultraGridColumn29.AutoCompleteMode = Infragistics.Win.AutoCompleteMode.Append;
            ultraGridColumn29.Header.VisiblePosition = 28;
            ultraGridColumn30.AutoCompleteMode = Infragistics.Win.AutoCompleteMode.Append;
            ultraGridColumn30.Header.VisiblePosition = 29;
            ultraGridColumn31.AutoCompleteMode = Infragistics.Win.AutoCompleteMode.Append;
            ultraGridColumn31.Header.VisiblePosition = 30;
            ultraGridColumn32.AutoCompleteMode = Infragistics.Win.AutoCompleteMode.Append;
            ultraGridColumn32.Header.VisiblePosition = 31;
            ultraGridColumn33.AutoCompleteMode = Infragistics.Win.AutoCompleteMode.Append;
            ultraGridColumn33.Header.VisiblePosition = 32;
            ultraGridColumn34.AutoCompleteMode = Infragistics.Win.AutoCompleteMode.Append;
            ultraGridColumn34.Header.VisiblePosition = 33;
            ultraGridColumn35.AutoCompleteMode = Infragistics.Win.AutoCompleteMode.Append;
            ultraGridColumn35.Header.VisiblePosition = 34;
            ultraGridColumn36.AutoCompleteMode = Infragistics.Win.AutoCompleteMode.Append;
            ultraGridColumn36.Header.VisiblePosition = 35;
            ultraGridColumn37.AutoCompleteMode = Infragistics.Win.AutoCompleteMode.Append;
            ultraGridColumn37.Header.VisiblePosition = 36;
            ultraGridColumn38.AutoCompleteMode = Infragistics.Win.AutoCompleteMode.Append;
            ultraGridColumn38.Header.VisiblePosition = 37;
            ultraGridColumn39.AutoCompleteMode = Infragistics.Win.AutoCompleteMode.Append;
            ultraGridColumn39.Header.VisiblePosition = 38;
            ultraGridColumn40.AutoCompleteMode = Infragistics.Win.AutoCompleteMode.Append;
            ultraGridColumn40.Header.VisiblePosition = 39;
            ultraGridColumn41.AutoCompleteMode = Infragistics.Win.AutoCompleteMode.Append;
            ultraGridColumn41.Header.VisiblePosition = 40;
            ultraGridColumn42.AutoCompleteMode = Infragistics.Win.AutoCompleteMode.Append;
            ultraGridColumn42.Header.VisiblePosition = 41;
            ultraGridColumn43.AutoCompleteMode = Infragistics.Win.AutoCompleteMode.Append;
            ultraGridColumn43.Header.VisiblePosition = 42;
            ultraGridColumn44.AutoCompleteMode = Infragistics.Win.AutoCompleteMode.Append;
            ultraGridColumn44.Header.VisiblePosition = 43;
            ultraGridColumn45.AutoCompleteMode = Infragistics.Win.AutoCompleteMode.Append;
            ultraGridColumn45.Header.VisiblePosition = 44;
            ultraGridColumn46.AutoCompleteMode = Infragistics.Win.AutoCompleteMode.Append;
            ultraGridColumn46.Header.VisiblePosition = 45;
            ultraGridColumn47.AutoCompleteMode = Infragistics.Win.AutoCompleteMode.Append;
            ultraGridColumn47.Header.VisiblePosition = 46;
            ultraGridColumn48.AutoCompleteMode = Infragistics.Win.AutoCompleteMode.Append;
            ultraGridColumn48.Header.VisiblePosition = 47;
            ultraGridColumn49.AutoCompleteMode = Infragistics.Win.AutoCompleteMode.Append;
            ultraGridColumn49.Header.VisiblePosition = 48;
            ultraGridColumn50.AutoCompleteMode = Infragistics.Win.AutoCompleteMode.Append;
            ultraGridColumn50.Header.VisiblePosition = 49;
            ultraGridColumn51.AutoCompleteMode = Infragistics.Win.AutoCompleteMode.Append;
            ultraGridColumn51.Header.VisiblePosition = 50;
            ultraGridColumn52.AutoCompleteMode = Infragistics.Win.AutoCompleteMode.Append;
            ultraGridColumn52.Header.VisiblePosition = 51;
            ultraGridColumn53.AutoCompleteMode = Infragistics.Win.AutoCompleteMode.Append;
            ultraGridColumn53.Header.VisiblePosition = 52;
            ultraGridColumn54.AutoCompleteMode = Infragistics.Win.AutoCompleteMode.Append;
            ultraGridColumn54.Header.VisiblePosition = 53;
            ultraGridColumn55.AutoCompleteMode = Infragistics.Win.AutoCompleteMode.Append;
            ultraGridColumn55.Header.VisiblePosition = 54;
            ultraGridColumn56.AutoCompleteMode = Infragistics.Win.AutoCompleteMode.Append;
            ultraGridColumn56.Header.VisiblePosition = 55;
            ultraGridColumn57.AutoCompleteMode = Infragistics.Win.AutoCompleteMode.Append;
            ultraGridColumn57.Header.VisiblePosition = 56;
            ultraGridColumn58.AutoCompleteMode = Infragistics.Win.AutoCompleteMode.Append;
            ultraGridColumn58.Header.VisiblePosition = 57;
            ultraGridColumn59.AutoCompleteMode = Infragistics.Win.AutoCompleteMode.Append;
            ultraGridColumn59.Header.VisiblePosition = 58;
            ultraGridColumn60.AutoCompleteMode = Infragistics.Win.AutoCompleteMode.Append;
            ultraGridColumn60.Header.VisiblePosition = 59;
            ultraGridColumn61.AutoCompleteMode = Infragistics.Win.AutoCompleteMode.Append;
            ultraGridColumn61.Header.VisiblePosition = 60;
            ultraGridColumn62.AutoCompleteMode = Infragistics.Win.AutoCompleteMode.Append;
            ultraGridColumn62.Header.VisiblePosition = 61;
            ultraGridColumn63.AutoCompleteMode = Infragistics.Win.AutoCompleteMode.Append;
            ultraGridColumn63.Header.VisiblePosition = 62;
            ultraGridColumn64.AutoCompleteMode = Infragistics.Win.AutoCompleteMode.Append;
            ultraGridColumn64.Header.VisiblePosition = 63;
            ultraGridColumn65.AutoCompleteMode = Infragistics.Win.AutoCompleteMode.Append;
            ultraGridColumn65.Header.VisiblePosition = 64;
            ultraGridColumn66.AutoCompleteMode = Infragistics.Win.AutoCompleteMode.Append;
            ultraGridColumn66.Header.VisiblePosition = 65;
            ultraGridColumn67.AutoCompleteMode = Infragistics.Win.AutoCompleteMode.Append;
            ultraGridColumn67.Header.VisiblePosition = 66;
            ultraGridColumn68.AutoCompleteMode = Infragistics.Win.AutoCompleteMode.Append;
            ultraGridColumn68.Header.VisiblePosition = 67;
            ultraGridColumn69.AutoCompleteMode = Infragistics.Win.AutoCompleteMode.Append;
            ultraGridColumn69.Header.VisiblePosition = 68;
            ultraGridColumn70.AutoCompleteMode = Infragistics.Win.AutoCompleteMode.Append;
            ultraGridColumn70.Header.VisiblePosition = 69;
            ultraGridColumn71.AutoCompleteMode = Infragistics.Win.AutoCompleteMode.Append;
            ultraGridColumn71.Header.VisiblePosition = 70;
            ultraGridColumn72.AutoCompleteMode = Infragistics.Win.AutoCompleteMode.Append;
            ultraGridColumn72.Header.VisiblePosition = 71;
            ultraGridColumn73.AutoCompleteMode = Infragistics.Win.AutoCompleteMode.Append;
            ultraGridColumn73.Header.VisiblePosition = 72;
            ultraGridColumn74.AutoCompleteMode = Infragistics.Win.AutoCompleteMode.Append;
            ultraGridColumn74.Header.VisiblePosition = 73;
            ultraGridColumn75.AutoCompleteMode = Infragistics.Win.AutoCompleteMode.Append;
            ultraGridColumn75.Header.VisiblePosition = 74;
            ultraGridColumn76.AutoCompleteMode = Infragistics.Win.AutoCompleteMode.Append;
            ultraGridColumn76.Header.VisiblePosition = 75;
            ultraGridColumn77.AutoCompleteMode = Infragistics.Win.AutoCompleteMode.Append;
            ultraGridColumn77.Header.VisiblePosition = 76;
            ultraGridColumn78.AutoCompleteMode = Infragistics.Win.AutoCompleteMode.Append;
            ultraGridColumn78.Header.VisiblePosition = 77;
            ultraGridColumn79.AutoCompleteMode = Infragistics.Win.AutoCompleteMode.Append;
            ultraGridColumn79.Header.VisiblePosition = 78;
            ultraGridColumn80.AutoCompleteMode = Infragistics.Win.AutoCompleteMode.Append;
            ultraGridColumn80.Header.VisiblePosition = 79;
            ultraGridBand1.Columns.AddRange(new object[] {
            ultraGridColumn1,
            ultraGridColumn2,
            ultraGridColumn3,
            ultraGridColumn4,
            ultraGridColumn5,
            ultraGridColumn6,
            ultraGridColumn7,
            ultraGridColumn8,
            ultraGridColumn9,
            ultraGridColumn10,
            ultraGridColumn11,
            ultraGridColumn12,
            ultraGridColumn13,
            ultraGridColumn14,
            ultraGridColumn15,
            ultraGridColumn16,
            ultraGridColumn17,
            ultraGridColumn18,
            ultraGridColumn19,
            ultraGridColumn20,
            ultraGridColumn21,
            ultraGridColumn22,
            ultraGridColumn23,
            ultraGridColumn24,
            ultraGridColumn25,
            ultraGridColumn26,
            ultraGridColumn27,
            ultraGridColumn28,
            ultraGridColumn29,
            ultraGridColumn30,
            ultraGridColumn31,
            ultraGridColumn32,
            ultraGridColumn33,
            ultraGridColumn34,
            ultraGridColumn35,
            ultraGridColumn36,
            ultraGridColumn37,
            ultraGridColumn38,
            ultraGridColumn39,
            ultraGridColumn40,
            ultraGridColumn41,
            ultraGridColumn42,
            ultraGridColumn43,
            ultraGridColumn44,
            ultraGridColumn45,
            ultraGridColumn46,
            ultraGridColumn47,
            ultraGridColumn48,
            ultraGridColumn49,
            ultraGridColumn50,
            ultraGridColumn51,
            ultraGridColumn52,
            ultraGridColumn53,
            ultraGridColumn54,
            ultraGridColumn55,
            ultraGridColumn56,
            ultraGridColumn57,
            ultraGridColumn58,
            ultraGridColumn59,
            ultraGridColumn60,
            ultraGridColumn61,
            ultraGridColumn62,
            ultraGridColumn63,
            ultraGridColumn64,
            ultraGridColumn65,
            ultraGridColumn66,
            ultraGridColumn67,
            ultraGridColumn68,
            ultraGridColumn69,
            ultraGridColumn70,
            ultraGridColumn71,
            ultraGridColumn72,
            ultraGridColumn73,
            ultraGridColumn74,
            ultraGridColumn75,
            ultraGridColumn76,
            ultraGridColumn77,
            ultraGridColumn78,
            ultraGridColumn79,
            ultraGridColumn80});
            this.tagrdItemPO.DisplayLayout.BandsSerializer.Add(ultraGridBand1);
            this.tagrdItemPO.DisplayLayout.CaptionVisible = Infragistics.Win.DefaultableBoolean.False;
            appearance3.BackColor = System.Drawing.SystemColors.ActiveBorder;
            appearance3.BackColor2 = System.Drawing.SystemColors.ControlDark;
            appearance3.BackGradientStyle = Infragistics.Win.GradientStyle.Vertical;
            appearance3.BorderColor = System.Drawing.SystemColors.Window;
            this.tagrdItemPO.DisplayLayout.GroupByBox.Appearance = appearance3;
            appearance4.ForeColor = System.Drawing.SystemColors.GrayText;
            this.tagrdItemPO.DisplayLayout.GroupByBox.BandLabelAppearance = appearance4;
            this.tagrdItemPO.DisplayLayout.GroupByBox.BorderStyle = Infragistics.Win.UIElementBorderStyle.Solid;
            appearance5.BackColor = System.Drawing.SystemColors.ControlLightLight;
            appearance5.BackColor2 = System.Drawing.SystemColors.Control;
            appearance5.BackGradientStyle = Infragistics.Win.GradientStyle.Horizontal;
            appearance5.ForeColor = System.Drawing.SystemColors.GrayText;
            this.tagrdItemPO.DisplayLayout.GroupByBox.PromptAppearance = appearance5;
            this.tagrdItemPO.DisplayLayout.MaxColScrollRegions = 1;
            this.tagrdItemPO.DisplayLayout.MaxRowScrollRegions = 1;
            appearance26.ForeColor = System.Drawing.SystemColors.ControlText;
            this.tagrdItemPO.DisplayLayout.Override.ActiveCellAppearance = appearance26;
            appearance30.ForeColor = System.Drawing.SystemColors.ControlText;
            this.tagrdItemPO.DisplayLayout.Override.ActiveRowAppearance = appearance30;
            this.tagrdItemPO.DisplayLayout.Override.AllowColMoving = Infragistics.Win.UltraWinGrid.AllowColMoving.WithinBand;
            this.tagrdItemPO.DisplayLayout.Override.AllowDelete = Infragistics.Win.DefaultableBoolean.False;
            this.tagrdItemPO.DisplayLayout.Override.AllowUpdate = Infragistics.Win.DefaultableBoolean.True;
            this.tagrdItemPO.DisplayLayout.Override.ButtonStyle = Infragistics.Win.UIElementButtonStyle.Office2007ScrollbarButton;
            appearance32.BackColor = System.Drawing.SystemColors.Window;
            this.tagrdItemPO.DisplayLayout.Override.CardAreaAppearance = appearance32;
            appearance33.BorderColor = System.Drawing.Color.Silver;
            appearance33.TextTrimming = Infragistics.Win.TextTrimming.EllipsisCharacter;
            this.tagrdItemPO.DisplayLayout.Override.CellAppearance = appearance33;
            this.tagrdItemPO.DisplayLayout.Override.CellPadding = 0;
            this.tagrdItemPO.DisplayLayout.Override.DefaultRowHeight = 25;
            this.tagrdItemPO.DisplayLayout.Override.FixedHeaderIndicator = Infragistics.Win.UltraWinGrid.FixedHeaderIndicator.None;
            appearance34.AlphaLevel = ((short)(255));
            appearance34.BackColor = System.Drawing.Color.AliceBlue;
            appearance34.BackColor2 = System.Drawing.Color.AliceBlue;
            appearance34.BorderColor = System.Drawing.Color.LightSteelBlue;
            appearance34.BorderColor2 = System.Drawing.Color.LightSteelBlue;
            appearance34.ForeColor = System.Drawing.Color.Black;
            appearance34.TextHAlignAsString = "Left";
            this.tagrdItemPO.DisplayLayout.Override.HeaderAppearance = appearance34;
            this.tagrdItemPO.DisplayLayout.Override.HeaderClickAction = Infragistics.Win.UltraWinGrid.HeaderClickAction.SortSingle;
            this.tagrdItemPO.DisplayLayout.Override.HeaderStyle = Infragistics.Win.HeaderStyle.WindowsXPCommand;
            appearance35.BackColor = System.Drawing.Color.White;
            appearance35.BackColor2 = System.Drawing.Color.White;
            appearance35.BorderColor = System.Drawing.Color.LightSteelBlue;
            appearance35.BorderColor2 = System.Drawing.Color.LightSteelBlue;
            appearance35.ForeColor = System.Drawing.Color.Black;
            appearance35.TextVAlignAsString = "Middle";
            this.tagrdItemPO.DisplayLayout.Override.RowAppearance = appearance35;
            appearance36.AlphaLevel = ((short)(255));
            appearance36.BackColor = System.Drawing.Color.AliceBlue;
            appearance36.BackColor2 = System.Drawing.Color.AliceBlue;
            appearance36.BorderColor = System.Drawing.Color.LightSteelBlue;
            appearance36.BorderColor2 = System.Drawing.Color.LightSteelBlue;
            this.tagrdItemPO.DisplayLayout.Override.RowSelectorAppearance = appearance36;
            appearance37.BackColor = System.Drawing.Color.AliceBlue;
            appearance37.BackColor2 = System.Drawing.Color.AliceBlue;
            appearance37.BorderColor = System.Drawing.Color.LightSteelBlue;
            appearance37.BorderColor2 = System.Drawing.Color.LightSteelBlue;
            this.tagrdItemPO.DisplayLayout.Override.RowSelectorHeaderAppearance = appearance37;
            this.tagrdItemPO.DisplayLayout.Override.RowSelectorHeaderStyle = Infragistics.Win.UltraWinGrid.RowSelectorHeaderStyle.SeparateElement;
            this.tagrdItemPO.DisplayLayout.Override.RowSelectors = Infragistics.Win.DefaultableBoolean.True;
            this.tagrdItemPO.DisplayLayout.Override.RowSelectorWidth = 30;
            this.tagrdItemPO.DisplayLayout.Override.RowSizing = Infragistics.Win.UltraWinGrid.RowSizing.Fixed;
            appearance38.BackColor = System.Drawing.Color.Gold;
            appearance38.BackColor2 = System.Drawing.Color.White;
            appearance38.BackGradientStyle = Infragistics.Win.GradientStyle.GlassBottom50Bright;
            this.tagrdItemPO.DisplayLayout.Override.SelectedRowAppearance = appearance38;
            this.tagrdItemPO.DisplayLayout.Override.SelectTypeCell = Infragistics.Win.UltraWinGrid.SelectType.Single;
            this.tagrdItemPO.DisplayLayout.Override.SelectTypeRow = Infragistics.Win.UltraWinGrid.SelectType.Single;
            this.tagrdItemPO.DisplayLayout.Override.SupportDataErrorInfo = Infragistics.Win.UltraWinGrid.SupportDataErrorInfo.RowsOnly;
            appearance39.BackColor = System.Drawing.Color.White;
            appearance39.ForeColor = System.Drawing.Color.Black;
            this.tagrdItemPO.DisplayLayout.Override.TemplateAddRowCellAppearance = appearance39;
            this.tagrdItemPO.DisplayLayout.ScrollBounds = Infragistics.Win.UltraWinGrid.ScrollBounds.ScrollToFill;
            this.tagrdItemPO.DisplayLayout.ScrollStyle = Infragistics.Win.UltraWinGrid.ScrollStyle.Immediate;
            this.tagrdItemPO.DisplayLayout.UseFixedHeaders = true;
            this.tagrdItemPO.DisplayLayout.ViewStyle = Infragistics.Win.UltraWinGrid.ViewStyle.SingleBand;
            this.tagrdItemPO.EnterKeyDirection = TAUtil.EnterKeyDirectionEnum.NextCellByTab;
            this.tagrdItemPO.Font = new System.Drawing.Font("Tahoma", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.tagrdItemPO.HeaderObjectKey = null;
            this.tagrdItemPO.Location = new System.Drawing.Point(12, 118);
            this.tagrdItemPO.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.tagrdItemPO.Name = "tagrdItemPO";
            this.tagrdItemPO.RowUpdateCancelAction = Infragistics.Win.UltraWinGrid.RowUpdateCancelAction.RetainDataAndActivation;
            this.tagrdItemPO.Size = new System.Drawing.Size(903, 340);
            this.tagrdItemPO.TabIndex = 50;
            this.tagrdItemPO.UseFlatMode = Infragistics.Win.DefaultableBoolean.True;
            this.tagrdItemPO.UseOsThemes = Infragistics.Win.DefaultableBoolean.False;
            this.tagrdItemPO.CustomCellUpdate += new Infragistics.Win.UltraWinGrid.BeforeCellUpdateEventHandler(this.tagrdItemPO_CustomCellUpdate);
            // 
            // btnClose
            // 
            appearance13.Image = global::WinUI.Properties.Resources.Close_16;
            this.btnClose.Appearance = appearance13;
            this.btnClose.Font = new System.Drawing.Font("Calibri", 10F, System.Drawing.FontStyle.Italic);
            this.btnClose.Location = new System.Drawing.Point(779, 62);
            this.btnClose.Name = "btnClose";
            this.btnClose.Size = new System.Drawing.Size(106, 26);
            this.btnClose.TabIndex = 53;
            this.btnClose.Text = "Close";
            this.btnClose.TextRenderingMode = Infragistics.Win.TextRenderingMode.GDI;
            this.btnClose.Click += new System.EventHandler(this.btnClose_Click);
            // 
            // btnAppend
            // 
            appearance12.Image = global::WinUI.Properties.Resources.Append_16;
            this.btnAppend.Appearance = appearance12;
            this.btnAppend.Font = new System.Drawing.Font("Calibri", 10F, System.Drawing.FontStyle.Italic);
            this.btnAppend.Location = new System.Drawing.Point(667, 61);
            this.btnAppend.Name = "btnAppend";
            this.btnAppend.Size = new System.Drawing.Size(106, 26);
            this.btnAppend.TabIndex = 52;
            this.btnAppend.Text = "Append";
            this.btnAppend.TextRenderingMode = Infragistics.Win.TextRenderingMode.GDI;
            this.btnAppend.Click += new System.EventHandler(this.btnAppend_Click);
            // 
            // ultraLabel2
            // 
            appearance31.BackColor = System.Drawing.Color.Transparent;
            appearance31.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            appearance31.TextVAlignAsString = "Middle";
            this.ultraLabel2.Appearance = appearance31;
            this.ultraLabel2.Font = new System.Drawing.Font("Calibri", 10F, System.Drawing.FontStyle.Italic);
            this.ultraLabel2.Location = new System.Drawing.Point(16, 42);
            this.ultraLabel2.Name = "ultraLabel2";
            this.ultraLabel2.Size = new System.Drawing.Size(79, 23);
            this.ultraLabel2.TabIndex = 55;
            this.ultraLabel2.Text = "PO Num :";
            this.ultraLabel2.TextRenderingMode = Infragistics.Win.TextRenderingMode.GDI;
            // 
            // ultraLabel3
            // 
            appearance29.BackColor = System.Drawing.Color.Transparent;
            appearance29.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            appearance29.TextVAlignAsString = "Middle";
            this.ultraLabel3.Appearance = appearance29;
            this.ultraLabel3.Font = new System.Drawing.Font("Calibri", 10F, System.Drawing.FontStyle.Italic);
            this.ultraLabel3.Location = new System.Drawing.Point(16, 68);
            this.ultraLabel3.Name = "ultraLabel3";
            this.ultraLabel3.Size = new System.Drawing.Size(79, 23);
            this.ultraLabel3.TabIndex = 59;
            this.ultraLabel3.Text = "Item :";
            this.ultraLabel3.TextRenderingMode = Infragistics.Win.TextRenderingMode.GDI;
            // 
            // ItmKey
            // 
            appearance27.BackColor = System.Drawing.Color.White;
            appearance27.BorderColor = System.Drawing.Color.LightGray;
            appearance27.FontData.Name = "Calibri";
            appearance27.FontData.SizeInPoints = 11F;
            appearance27.ForeColor = System.Drawing.Color.Black;
            this.ItmKey.Appearance = appearance27;
            this.ItmKey.AutoCompleteMode = Infragistics.Win.AutoCompleteMode.Append;
            this.ItmKey.AutoSize = false;
            this.ItmKey.ComboIsDirty = false;
            this.ItmKey.DisplayStyle = Infragistics.Win.EmbeddableElementDisplayStyle.Office2007;
            this.ItmKey.Font = new System.Drawing.Font("Tahoma", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.ItmKey.ItemMatchingMode = Infragistics.Win.ValueListItemMatchingMode.DoNotConvertDataValueToString;
            this.ItmKey.Location = new System.Drawing.Point(119, 63);
            this.ItmKey.Name = "ItmKey";
            this.ItmKey.Size = new System.Drawing.Size(300, 25);
            this.ItmKey.TabIndex = 58;
            this.ItmKey.TextRenderingMode = Infragistics.Win.TextRenderingMode.GDI;
            this.ItmKey.UserInputText = "";
            this.ItmKey.CustomUpdate += new System.ComponentModel.CancelEventHandler(this.ItmKey_CustomUpdate);
            this.ItmKey.ItemNotInList += new Infragistics.Win.UltraWinGrid.ItemNotInListEventHandler(this.Combo_NotInListAdd);
            // 
            // PrmDate
            // 
            appearance148.BackColor = System.Drawing.Color.White;
            appearance148.BorderColor = System.Drawing.Color.LightGray;
            appearance148.FontData.Name = "Calibri";
            appearance148.FontData.SizeInPoints = 11F;
            appearance148.ForeColor = System.Drawing.Color.Black;
            appearance148.TextHAlignAsString = "Right";
            this.PrmDate.Appearance = appearance148;
            this.PrmDate.AutoSize = false;
            this.PrmDate.BackColor = System.Drawing.Color.White;
            appearance149.Image = global::WinUI.Properties.Resources.calendar3;
            appearance149.ImageHAlign = Infragistics.Win.HAlign.Right;
            editorButton1.Appearance = appearance149;
            editorButton1.ButtonStyle = Infragistics.Win.UIElementButtonStyle.FlatBorderless;
            this.PrmDate.ButtonsRight.Add(editorButton1);
            this.PrmDate.calendarContainer = null;
            this.PrmDate.DateValue = null;
            this.PrmDate.DisplayStyle = Infragistics.Win.EmbeddableElementDisplayStyle.Office2007;
            this.PrmDate.Font = new System.Drawing.Font("Tahoma", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.PrmDate.Format = "";
            this.PrmDate.Location = new System.Drawing.Point(119, 13);
            this.PrmDate.MaxLength = 20;
            this.PrmDate.Name = "PrmDate";
            this.PrmDate.Size = new System.Drawing.Size(300, 25);
            this.PrmDate.TabIndex = 61;
            this.PrmDate.TextRenderingMode = Infragistics.Win.TextRenderingMode.GDI;
            this.PrmDate.CustomUpdate += new System.ComponentModel.CancelEventHandler(this.PrmDate_CustomUpdate);
            // 
            // PickSeq
            // 
            appearance7.FontData.ItalicAsString = "True";
            appearance7.FontData.Name = "Calibri";
            appearance7.FontData.SizeInPoints = 10F;
            appearance7.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            this.PickSeq.Appearance = appearance7;
            this.PickSeq.cancelUpdate = false;
            this.PickSeq.Location = new System.Drawing.Point(20, 28);
            this.PickSeq.Name = "PickSeq";
            this.PickSeq.Size = new System.Drawing.Size(232, 20);
            this.PickSeq.TabIndex = 62;
            this.PickSeq.Text = "Append to base on Picking Sequence";
            this.PickSeq.TextRenderingMode = Infragistics.Win.TextRenderingMode.GDI;
            // 
            // panel1
            // 
            this.panel1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(243)))), ((int)(((byte)(234)))), ((int)(((byte)(226)))));
            this.panel1.Controls.Add(this.btnUnSelectAll);
            this.panel1.Controls.Add(this.btnSelectAll);
            this.panel1.Controls.Add(this.ultraLabel1);
            this.panel1.Controls.Add(this.ultraLabel3);
            this.panel1.Controls.Add(this.btnClose);
            this.panel1.Controls.Add(this.PrmDate);
            this.panel1.Controls.Add(this.btnAppend);
            this.panel1.Controls.Add(this.ItmKey);
            this.panel1.Controls.Add(this.ultraLabel2);
            this.panel1.Controls.Add(this.DocKey);
            this.panel1.Location = new System.Drawing.Point(12, 10);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(903, 101);
            this.panel1.TabIndex = 63;
            // 
            // btnUnSelectAll
            // 
            appearance10.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            appearance10.Image = global::WinUI.Properties.Resources.selectnone;
            this.btnUnSelectAll.Appearance = appearance10;
            this.btnUnSelectAll.Font = new System.Drawing.Font("Calibri", 10F, System.Drawing.FontStyle.Italic);
            this.btnUnSelectAll.Location = new System.Drawing.Point(555, 61);
            this.btnUnSelectAll.Name = "btnUnSelectAll";
            this.btnUnSelectAll.Size = new System.Drawing.Size(106, 26);
            this.btnUnSelectAll.TabIndex = 65;
            this.btnUnSelectAll.Text = "&Select None";
            this.btnUnSelectAll.TextRenderingMode = Infragistics.Win.TextRenderingMode.GDI;
            this.btnUnSelectAll.Click += new System.EventHandler(this.btnUnSelectAll_Click);
            // 
            // btnSelectAll
            // 
            appearance9.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            appearance9.Image = global::WinUI.Properties.Resources.selectall;
            this.btnSelectAll.Appearance = appearance9;
            this.btnSelectAll.Font = new System.Drawing.Font("Calibri", 10F, System.Drawing.FontStyle.Italic);
            this.btnSelectAll.Location = new System.Drawing.Point(443, 61);
            this.btnSelectAll.Name = "btnSelectAll";
            this.btnSelectAll.Size = new System.Drawing.Size(106, 26);
            this.btnSelectAll.TabIndex = 64;
            this.btnSelectAll.Text = "Select &All";
            this.btnSelectAll.TextRenderingMode = Infragistics.Win.TextRenderingMode.GDI;
            this.btnSelectAll.Click += new System.EventHandler(this.btnSelectAll_Click);
            // 
            // panel2
            // 
            this.panel2.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(243)))), ((int)(((byte)(234)))), ((int)(((byte)(226)))));
            this.panel2.Controls.Add(this.PickSeq);
            this.panel2.Controls.Add(this.UseSystemPrice);
            this.panel2.Location = new System.Drawing.Point(12, 465);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(903, 56);
            this.panel2.TabIndex = 64;
            // 
            // frmInsertPO
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.White;
            this.ClientSize = new System.Drawing.Size(924, 530);
            this.Controls.Add(this.panel2);
            this.Controls.Add(this.panel1);
            this.Controls.Add(this.tagrdItemPO);
            this.KeyPreview = true;
            this.MaximizeBox = false;
            this.Name = "frmInsertPO";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Tag = "frmInsertPO";
            this.Text = "Insert PO";
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.frmInsertPO_FormClosed);
            this.Load += new System.EventHandler(this.frmInsertPO_Load);
            this.KeyDown += new System.Windows.Forms.KeyEventHandler(this.frmInsertPO_KeyDown);
            ((System.ComponentModel.ISupportInitialize)(this.DocKey)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.UseSystemPrice)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.tagrdItemPO)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.ItmKey)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.PrmDate)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.PickSeq)).EndInit();
            this.panel1.ResumeLayout(false);
            this.panel2.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private Infragistics.Win.Misc.UltraLabel ultraLabel1;
        private TAUtil.TAComboBox DocKey;
        private TAUtil.TACheckBoxEditor UseSystemPrice;
        private TAUtil.TAGridEditor tagrdItemPO;
        private Infragistics.Win.Misc.UltraButton btnClose;
        private Infragistics.Win.Misc.UltraButton btnAppend;
        private Infragistics.Win.Misc.UltraLabel ultraLabel2;
        private Infragistics.Win.Misc.UltraLabel ultraLabel3;
        private TAUtil.TAComboBox ItmKey;
        private TAUtil.TADateEditor PrmDate;
        private TAUtil.TACheckBoxEditor PickSeq;
        private System.Windows.Forms.Panel panel1;
        private Infragistics.Win.Misc.UltraButton btnUnSelectAll;
        private Infragistics.Win.Misc.UltraButton btnSelectAll;
        private System.Windows.Forms.Panel panel2;
    }
}