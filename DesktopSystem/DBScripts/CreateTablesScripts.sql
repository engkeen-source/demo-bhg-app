

/****** Object:  Table [dbo].[AR_SO]    Script Date: 04-May-2026 5:33:15 pm ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[AR_SO](
	[DocKey] [int] NOT NULL,
	[DocCodeKey] [int] NOT NULL,
	[DocID] [nvarchar](50) NOT NULL,
	[DocDate] [datetime] NOT NULL,
	[DocType] [int] NOT NULL,
	[DocTypeNm] [nvarchar](50) NOT NULL,
	[DocSign] [smallint] NOT NULL,
	[DocConKey] [int] NOT NULL,
	[DocConNm] [nvarchar](255) NOT NULL,
	[DocConUEN] [nvarchar](50) NULL,
	[DocDeptKey] [int] NOT NULL,
	[DocTranGrpKey] [int] NULL,
	[DocAccKey] [int] NOT NULL,
	[DocGrpKey] [int] NOT NULL,
	[DocPriceType] [int] NULL,
	[DocTermKey] [int] NULL,
	[DocEmKey] [int] NULL,
	[DocBAddrStreet] [nvarchar](255) NULL,
	[DocBAddrPOBox] [nvarchar](50) NULL,
	[DocBAddrCity] [nvarchar](50) NULL,
	[DocBAddrState] [nvarchar](50) NULL,
	[DocBAddrZipCode] [nvarchar](50) NULL,
	[DocBAddrCountry] [nvarchar](50) NULL,
	[DocBAddrRegion] [nvarchar](50) NULL,
	[DocBAddrAttn] [nvarchar](50) NULL,
	[DocBAddrTel1] [nvarchar](50) NULL,
	[DocBAddrTel2] [nvarchar](50) NULL,
	[DocBAddrFax] [nvarchar](50) NULL,
	[DocBAddrEmail] [nvarchar](50) NULL,
	[DocSAddrStreet] [nvarchar](255) NULL,
	[DocSAddrPOBox] [nvarchar](50) NULL,
	[DocSAddrCity] [nvarchar](50) NULL,
	[DocSAddrState] [nvarchar](50) NULL,
	[DocSAddrZipCode] [nvarchar](50) NULL,
	[DocSAddrCountry] [nvarchar](50) NULL,
	[DocSAddrRegion] [nvarchar](50) NULL,
	[DocSAddrAttn] [nvarchar](50) NULL,
	[DocSAddrTel1] [nvarchar](50) NULL,
	[DocSAddrTel2] [nvarchar](50) NULL,
	[DocSAddrFax] [nvarchar](50) NULL,
	[DocSAddrEmail] [nvarchar](50) NULL,
	[DocShipName] [nvarchar](255) NULL,
	[DocShipMark] [nvarchar](50) NULL,
	[DocShipKey] [int] NULL,
	[DocShipDate] [datetime] NULL,
	[DocCustPONum] [nvarchar](50) NULL,
	[DocQONum] [nvarchar](50) NULL,
	[DocSONum] [nvarchar](50) NULL,
	[DocDONum] [nvarchar](50) NULL,
	[DocIVNum] [nvarchar](50) NULL,
	[DocPONum] [nvarchar](50) NULL,
	[DocPDNum] [nvarchar](50) NULL,
	[DocBLNum] [nvarchar](50) NULL,
	[DocRef] [nvarchar](255) NULL,
	[DocDes] [nvarchar](255) NULL,
	[DocRem] [nvarchar](255) NULL,
	[DocRemDelivery] [nvarchar](255) NULL,
	[DocRemPrice] [nvarchar](255) NULL,
	[DocRemValidity] [nvarchar](255) NULL,
	[DocRemPayment] [nvarchar](255) NULL,
	[DocPermitNum] [nvarchar](255) NULL,
	[DocGoodsDestination] [nvarchar](255) NULL,
	[DocCountryOrigin] [nvarchar](255) NULL,
	[DocRemAdditional1] [nvarchar](255) NULL,
	[DocRemAdditional2] [nvarchar](255) NULL,
	[DocRemAdditional3] [nvarchar](255) NULL,
	[DocRemAdditional4] [nvarchar](255) NULL,
	[DocSubTotal] [money] NOT NULL,
	[DocOverallDisAcc] [int] NULL,
	[DocOverallDisRate] [decimal](18, 6) NOT NULL,
	[DocOverallDisAmt] [money] NOT NULL,
	[DocTotalAfterDis] [money] NOT NULL,
	[DocTaxGrpKey] [int] NULL,
	[DocTaxGrpRate] [money] NOT NULL,
	[DocTaxTotal] [money] NOT NULL,
	[DocGrand] [money] NOT NULL,
	[DocCurrKey] [int] NOT NULL,
	[DocCurrRate] [decimal](18, 8) NOT NULL,
	[DocHome] [money] NOT NULL,
	[DocCountryRate] [decimal](18, 8) NOT NULL,
	[DocTaxTotalLocal] [money] NOT NULL,
	[DocCompleted] [bit] NOT NULL,
	[DocStatus] [nvarchar](50) NULL,
	[DocState] [int] NOT NULL,
	[DocPrinted] [bit] NOT NULL,
	[ApproveUserKey] [int] NOT NULL,
	[ApproveDate] [datetime] NULL,
	[DisapproveUserKey] [int] NOT NULL,
	[DisapproveDate] [datetime] NULL,
	[DisapproveCount] [smallint] NOT NULL,
	[DisapproveMsg] [nvarchar](255) NULL,
	[Attachment] [bit] NOT NULL,
	[BranchKey] [int] NOT NULL,
	[CreateDate] [datetime] NULL,
	[CreateUserKey] [int] NULL,
	[LastModifiedDate] [datetime] NULL,
	[LastModifiedUserKey] [int] NULL,
	[PurgeKeep] [int] NOT NULL,
	[PurgeData] [bit] NOT NULL,
	[Custom1] [nvarchar](255) NULL,
	[Custom2] [nvarchar](255) NULL,
	[Custom3] [nvarchar](255) NULL,
	[Custom4] [nvarchar](255) NULL,
	[Custom5] [nvarchar](255) NULL,
	[DocRequiredDate] [datetime] NULL,
	[PrintDept] [nvarchar](1) NULL,
	[IsExportPacking] [bit] NULL,
	[IsCashSale] [bit] NULL,
	[IsSelfCollect] [bit] NULL,
	[RequestingApproval] [bit] NULL,
	[COORejected] [bit] NULL,
	[CrLimitApproval] [bit] NULL,
	[CrLimitRejected] [bit] NULL,
	[CancelSOApproval] [nvarchar](20) NULL,
	[SpecialItemApproval] [nvarchar](20) NULL,
	[SpecialItemRejectReason] [nvarchar](300) NULL,
 CONSTRAINT [PK_AR_SO] PRIMARY KEY CLUSTERED 
(
	[DocKey] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, FILLFACTOR = 90, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO

/****** Object:  Table [dbo].[AR_SODetItm]    Script Date: 04-May-2026 5:33:15 pm ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[AR_SODetItm](
	[DocKey] [int] NOT NULL,
	[DocItmKey] [int] NOT NULL,
	[LineType] [int] NOT NULL,
	[LineLinkKey] [int] NOT NULL,
	[ItmSN] [money] NOT NULL,
	[ItmKey] [int] NOT NULL,
	[ItmKeySelect] [int] NOT NULL,
	[ItmType] [int] NOT NULL,
	[ItmDes] [nvarchar](max) NULL,
	[ItmDeptKey] [int] NOT NULL,
	[ItmTranGrpKey] [int] NULL,
	[ItmAccKey] [int] NULL,
	[ItmLocKey] [int] NULL,
	[ItmStock] [money] NULL,
	[ItmQty] [money] NULL,
	[ItmQtyLink] [money] NULL,
	[ItmQtyAdj] [money] NULL,
	[ItmOrderStatus] [int] NULL,
	[ItmReqDate] [datetime] NULL,
	[ItmPrmDate] [datetime] NULL,
	[ItmUOMKey] [int] NULL,
	[ItmConRate] [decimal](18, 8) NULL,
	[ItmLatestCostF] [decimal](18, 6) NULL,
	[ItmLatestCostH] [decimal](18, 6) NULL,
	[ItmListPrice] [decimal](18, 6) NULL,
	[ItmPriceBefore] [decimal](18, 6) NULL,
	[ItmPriceAfter] [decimal](18, 6) NULL,
	[ItmDisPercent] [money] NULL,
	[ItmDisValue] [decimal](18, 6) NULL,
	[ItmPrice] [decimal](18, 6) NULL,
	[ItmPriceUser] [decimal](18, 6) NULL,
	[ItmControlPrice] [decimal](18, 6) NULL,
	[ItmControlPriceBase] [decimal](18, 6) NULL,
	[ItmAmtShw] [money] NULL,
	[ItmAmtF] [money] NOT NULL,
	[ItmAmtH] [money] NOT NULL,
	[ItmTaxable] [bit] NOT NULL,
	[ItmTaxGrpKey] [int] NULL,
	[ItmTaxGrpRate] [money] NULL,
	[ItmTaxGrpAmtF] [money] NULL,
	[ItmTaxGrpAmtL] [money] NULL,
	[ItmHide] [bit] NOT NULL,
	[ItmColorKey] [int] NULL,
	[ItmScaleSize] [nvarchar](50) NULL,
	[ItmPacking] [nvarchar](255) NULL,
	[ItmRem] [nvarchar](255) NULL,
	[ItmMark] [nvarchar](255) NULL,
	[ItmVendorKey] [int] NULL,
	[ItmVendorNm] [nvarchar](255) NULL,
	[ItmVendorCurrKey] [int] NOT NULL,
	[ItmVendorCurrRate] [float] NOT NULL,
	[ItmVendorPrice] [decimal](18, 6) NOT NULL,
	[ItmVendorPriceRatio] [decimal](18, 6) NOT NULL,
	[ItmVendorPriceLock] [bit] NOT NULL,
	[ItmJobKey] [int] NOT NULL,
	[ItmJobPhaseKey] [int] NOT NULL,
	[ItmJobTaskKey] [int] NOT NULL,
	[ItmJobCostTypeKey] [int] NOT NULL,
	[ItmIGrpDItm] [int] NOT NULL,
	[ItmIGrpQtyLock] [bit] NOT NULL,
	[ItmIGrpToPrint] [bit] NOT NULL,
	[ItmIGrpQtySet] [money] NOT NULL,
	[ItmIGrpAmtSet] [money] NOT NULL,
	[ItmAttachment] [bit] NOT NULL,
	[ItmBatchKey] [int] NULL,
	[ItmBatchQty] [money] NULL,
	[NSLink] [nvarchar](50) NOT NULL,
	[ARQOID] [nvarchar](50) NULL,
	[ARQODK] [int] NOT NULL,
	[ARQODItm] [int] NOT NULL,
	[CreateDate] [datetime] NULL,
	[CreateUserKey] [int] NULL,
	[LastModifiedDate] [datetime] NULL,
	[LastModifiedUserKey] [int] NULL,
	[Custom1] [nvarchar](255) NULL,
	[Custom2] [nvarchar](1000) NULL,
	[Custom3] [nvarchar](255) NULL,
	[ItmDetSN] [money] NOT NULL,
	[Released_trigger] [bit] NOT NULL,
	[ARROID] [nvarchar](50) NULL,
	[ARRODK] [int] NULL,
	[ARRODItm] [int] NULL,
	[GrpStock] [varchar](10) NULL,
	[DSQty] [money] NOT NULL,
	[ObCost] [decimal](18, 6) NULL,
	[ItmRef] [nvarchar](255) NULL,
	[ItmWrtyEndDate] [datetime] NULL,
	[HSCode] [nvarchar](50) NULL,
	[CountryID] [nvarchar](50) NULL,
 CONSTRAINT [aaaaaAR_SOItm_tbl_PK] PRIMARY KEY NONCLUSTERED 
(
	[DocItmKey] ASC,
	[DocKey] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, FILLFACTOR = 90, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO

ALTER TABLE [dbo].[AR_SO] ADD  CONSTRAINT [DF_AR_SO_DocSign]  DEFAULT ((1)) FOR [DocSign]
GO

ALTER TABLE [dbo].[AR_SO] ADD  CONSTRAINT [DF_AR_SO_DocDeptKey]  DEFAULT ((0)) FOR [DocDeptKey]
GO

ALTER TABLE [dbo].[AR_SO] ADD  CONSTRAINT [DF_AR_SO_DocTranGrpKey]  DEFAULT ((0)) FOR [DocTranGrpKey]
GO

ALTER TABLE [dbo].[AR_SO] ADD  CONSTRAINT [DF_AR_SO_DocGrpKey]  DEFAULT ((0)) FOR [DocGrpKey]
GO

ALTER TABLE [dbo].[AR_SO] ADD  CONSTRAINT [DF_AR_SO_DocSubTotal]  DEFAULT ((0)) FOR [DocSubTotal]
GO

ALTER TABLE [dbo].[AR_SO] ADD  CONSTRAINT [DF_AR_SO_DocOverallDisRate]  DEFAULT ((0)) FOR [DocOverallDisRate]
GO

ALTER TABLE [dbo].[AR_SO] ADD  CONSTRAINT [DF_AR_SO_DocOverallDisAmt]  DEFAULT ((0)) FOR [DocOverallDisAmt]
GO

ALTER TABLE [dbo].[AR_SO] ADD  CONSTRAINT [DF_AR_SO_DocTotalAfterDis]  DEFAULT ((0)) FOR [DocTotalAfterDis]
GO

ALTER TABLE [dbo].[AR_SO] ADD  CONSTRAINT [DF_AR_SO_DocTaxGrpRate]  DEFAULT ((0)) FOR [DocTaxGrpRate]
GO

ALTER TABLE [dbo].[AR_SO] ADD  CONSTRAINT [DF_AR_SO_DocTaxTotal]  DEFAULT ((0)) FOR [DocTaxTotal]
GO

ALTER TABLE [dbo].[AR_SO] ADD  CONSTRAINT [DF_AR_SO_DocGrand]  DEFAULT ((0)) FOR [DocGrand]
GO

ALTER TABLE [dbo].[AR_SO] ADD  CONSTRAINT [DF_AR_SO_DocCurrKey]  DEFAULT ((1)) FOR [DocCurrKey]
GO

ALTER TABLE [dbo].[AR_SO] ADD  CONSTRAINT [DF_AR_SO_DocCurrRate]  DEFAULT ((1)) FOR [DocCurrRate]
GO

ALTER TABLE [dbo].[AR_SO] ADD  CONSTRAINT [DF_AR_SO_DocHome]  DEFAULT ((0)) FOR [DocHome]
GO

ALTER TABLE [dbo].[AR_SO] ADD  CONSTRAINT [DF_AR_SO_RecLocalTaxRate]  DEFAULT ((1)) FOR [DocCountryRate]
GO

ALTER TABLE [dbo].[AR_SO] ADD  CONSTRAINT [DF_AR_SO_RecLocalTaxAmt]  DEFAULT ((0)) FOR [DocTaxTotalLocal]
GO

ALTER TABLE [dbo].[AR_SO] ADD  CONSTRAINT [DF_AR_SO_DocCompleted]  DEFAULT ((0)) FOR [DocCompleted]
GO

ALTER TABLE [dbo].[AR_SO] ADD  CONSTRAINT [DF_AR_SO_DocPrinted]  DEFAULT ((0)) FOR [DocPrinted]
GO

ALTER TABLE [dbo].[AR_SO] ADD  CONSTRAINT [DF_AR_SO_ApproveUserKey]  DEFAULT ((0)) FOR [ApproveUserKey]
GO

ALTER TABLE [dbo].[AR_SO] ADD  CONSTRAINT [DF_AR_SO_DisapproveUserKey]  DEFAULT ((0)) FOR [DisapproveUserKey]
GO

ALTER TABLE [dbo].[AR_SO] ADD  CONSTRAINT [DF_AR_SO_DisapproveCount]  DEFAULT ((0)) FOR [DisapproveCount]
GO

ALTER TABLE [dbo].[AR_SO] ADD  CONSTRAINT [DF_AR_SO_Attachment]  DEFAULT ((0)) FOR [Attachment]
GO

ALTER TABLE [dbo].[AR_SO] ADD  CONSTRAINT [DF_AR_SO_BranchKey]  DEFAULT ((0)) FOR [BranchKey]
GO

ALTER TABLE [dbo].[AR_SO] ADD  CONSTRAINT [DF_AR_SO_CreateDate]  DEFAULT (CONVERT([datetime],CONVERT([varchar],getdate(),(1)),(0))) FOR [CreateDate]
GO

ALTER TABLE [dbo].[AR_SO] ADD  CONSTRAINT [DF_AR_SO_PurgeKeep]  DEFAULT ((0)) FOR [PurgeKeep]
GO

ALTER TABLE [dbo].[AR_SO] ADD  CONSTRAINT [DF_AR_SO_PurgeData]  DEFAULT ((0)) FOR [PurgeData]
GO

ALTER TABLE [dbo].[AR_SO] ADD  CONSTRAINT [DF_AR_SO_PrintDept]  DEFAULT (N'S') FOR [PrintDept]
GO

ALTER TABLE [dbo].[AR_SO] ADD  DEFAULT ((0)) FOR [IsExportPacking]
GO

ALTER TABLE [dbo].[AR_SO] ADD  DEFAULT ((0)) FOR [IsCashSale]
GO

ALTER TABLE [dbo].[AR_SO] ADD  DEFAULT ((0)) FOR [IsSelfCollect]
GO

ALTER TABLE [dbo].[AR_SODetItm] ADD  CONSTRAINT [DF_AR_SODetItm_LineLinkKey]  DEFAULT ((0)) FOR [LineLinkKey]
GO

ALTER TABLE [dbo].[AR_SODetItm] ADD  CONSTRAINT [DF__TemporaryUps__SN__49E3F248]  DEFAULT ((1)) FOR [ItmSN]
GO

ALTER TABLE [dbo].[AR_SODetItm] ADD  CONSTRAINT [DF_AR_SOItm_tbl_ItmDeptKey]  DEFAULT ((0)) FOR [ItmDeptKey]
GO

ALTER TABLE [dbo].[AR_SODetItm] ADD  CONSTRAINT [DF_AR_SODetItm_ItmTranGrpKey]  DEFAULT ((0)) FOR [ItmTranGrpKey]
GO

ALTER TABLE [dbo].[AR_SODetItm] ADD  CONSTRAINT [DF_AR_SODetItm_ItmOrderStatus]  DEFAULT ((0)) FOR [ItmOrderStatus]
GO

ALTER TABLE [dbo].[AR_SODetItm] ADD  CONSTRAINT [DF__Temporary__ItmAm__4DB4832C]  DEFAULT ((0)) FOR [ItmAmtF]
GO

ALTER TABLE [dbo].[AR_SODetItm] ADD  CONSTRAINT [DF__Temporary__ItmAm__4F9CCB9E]  DEFAULT ((0)) FOR [ItmAmtH]
GO

ALTER TABLE [dbo].[AR_SODetItm] ADD  CONSTRAINT [DF__Temporary__Taxab__5090EFD7]  DEFAULT ((1)) FOR [ItmTaxable]
GO

ALTER TABLE [dbo].[AR_SODetItm] ADD  CONSTRAINT [DF_AR_SODetItm_ItmTaxGrpRate]  DEFAULT ((0)) FOR [ItmTaxGrpRate]
GO

ALTER TABLE [dbo].[AR_SODetItm] ADD  CONSTRAINT [DF_AR_SODetItm_ItmTaxGrpAmtF]  DEFAULT ((0)) FOR [ItmTaxGrpAmtF]
GO

ALTER TABLE [dbo].[AR_SODetItm] ADD  CONSTRAINT [DF_AR_SODetItm_ItmTaxGrpAmtL]  DEFAULT ((0)) FOR [ItmTaxGrpAmtL]
GO

ALTER TABLE [dbo].[AR_SODetItm] ADD  CONSTRAINT [DF__Temporary__ItmHi__51851410]  DEFAULT ((0)) FOR [ItmHide]
GO

ALTER TABLE [dbo].[AR_SODetItm] ADD  CONSTRAINT [DF_AR_SODetItm_ItmVendorCurrKey]  DEFAULT ((1)) FOR [ItmVendorCurrKey]
GO

ALTER TABLE [dbo].[AR_SODetItm] ADD  CONSTRAINT [DF_AR_SODetItm_ItmVendorCurrRate]  DEFAULT ((1)) FOR [ItmVendorCurrRate]
GO

ALTER TABLE [dbo].[AR_SODetItm] ADD  CONSTRAINT [DF_AR_SODetItm_ItmVendorPrice]  DEFAULT ((0)) FOR [ItmVendorPrice]
GO

ALTER TABLE [dbo].[AR_SODetItm] ADD  CONSTRAINT [DF_AR_SODetItm_ItmVendorPriceRatio]  DEFAULT ((0)) FOR [ItmVendorPriceRatio]
GO

ALTER TABLE [dbo].[AR_SODetItm] ADD  CONSTRAINT [DF_AR_SODetItm_ItmVendorPriceLock]  DEFAULT ((0)) FOR [ItmVendorPriceLock]
GO

ALTER TABLE [dbo].[AR_SODetItm] ADD  CONSTRAINT [DF_AR_SODetItm_ItmJobKey]  DEFAULT ((0)) FOR [ItmJobKey]
GO

ALTER TABLE [dbo].[AR_SODetItm] ADD  CONSTRAINT [DF_AR_SODetItm_ItmJobPhaseKey]  DEFAULT ((0)) FOR [ItmJobPhaseKey]
GO

ALTER TABLE [dbo].[AR_SODetItm] ADD  CONSTRAINT [DF_AR_SODetItm_ItmJobTaskKey]  DEFAULT ((0)) FOR [ItmJobTaskKey]
GO

ALTER TABLE [dbo].[AR_SODetItm] ADD  CONSTRAINT [DF_AR_SODetItm_ItmJobCostTypeKey]  DEFAULT ((0)) FOR [ItmJobCostTypeKey]
GO

ALTER TABLE [dbo].[AR_SODetItm] ADD  CONSTRAINT [DF_AR_SOItm_tbl_ItmIGrpKey]  DEFAULT ((0)) FOR [ItmIGrpDItm]
GO

ALTER TABLE [dbo].[AR_SODetItm] ADD  CONSTRAINT [DF_AR_SOItm_tbl_ItmIGrpQtyLock]  DEFAULT ((0)) FOR [ItmIGrpQtyLock]
GO

ALTER TABLE [dbo].[AR_SODetItm] ADD  CONSTRAINT [DF_AR_SOItm_tbl_ItmIGrpToPrint]  DEFAULT ((0)) FOR [ItmIGrpToPrint]
GO

ALTER TABLE [dbo].[AR_SODetItm] ADD  CONSTRAINT [DF_AR_SOItm_tbl_ItmIGrpQtySet]  DEFAULT ((0)) FOR [ItmIGrpQtySet]
GO

ALTER TABLE [dbo].[AR_SODetItm] ADD  CONSTRAINT [DF_AR_SOItm_tbl_ItmIGrpAmt]  DEFAULT ((0)) FOR [ItmIGrpAmtSet]
GO

ALTER TABLE [dbo].[AR_SODetItm] ADD  CONSTRAINT [DF_AR_SOItm_tbl_ItmAttachment]  DEFAULT ((0)) FOR [ItmAttachment]
GO

ALTER TABLE [dbo].[AR_SODetItm] ADD  CONSTRAINT [DF_AR_SODetItm_ItmBatchKey]  DEFAULT ((0)) FOR [ItmBatchKey]
GO

ALTER TABLE [dbo].[AR_SODetItm] ADD  CONSTRAINT [DF_AR_SODetItm_ItmBatchQty]  DEFAULT ((0)) FOR [ItmBatchQty]
GO

ALTER TABLE [dbo].[AR_SODetItm] ADD  CONSTRAINT [DF_AR_SODetItm_NSLink]  DEFAULT ((0)) FOR [NSLink]
GO

ALTER TABLE [dbo].[AR_SODetItm] ADD  CONSTRAINT [DF_AR_SODetItm_ARQODK]  DEFAULT ((0)) FOR [ARQODK]
GO

ALTER TABLE [dbo].[AR_SODetItm] ADD  CONSTRAINT [DF_AR_SODetItm_ARQODItm]  DEFAULT ((0)) FOR [ARQODItm]
GO

ALTER TABLE [dbo].[AR_SODetItm] ADD  CONSTRAINT [DF_AR_SOItm_tbl_CreateDate_1]  DEFAULT (CONVERT([datetime],CONVERT([varchar],getdate(),(1)),(0))) FOR [CreateDate]
GO

ALTER TABLE [dbo].[AR_SODetItm] ADD  CONSTRAINT [DF_AR_SODetItm_ItmDetSN]  DEFAULT ((0)) FOR [ItmDetSN]
GO

ALTER TABLE [dbo].[AR_SODetItm] ADD  CONSTRAINT [DF__AR_SODetI__Relea__19CACAD2]  DEFAULT ((0)) FOR [Released_trigger]
GO

ALTER TABLE [dbo].[AR_SODetItm] ADD  CONSTRAINT [DF_AR_SODetItm_ARRODK]  DEFAULT ((0)) FOR [ARRODK]
GO

ALTER TABLE [dbo].[AR_SODetItm] ADD  CONSTRAINT [DF_AR_SODetItm_ARRODItm]  DEFAULT ((0)) FOR [ARRODItm]
GO

ALTER TABLE [dbo].[AR_SODetItm] ADD  DEFAULT ((0)) FOR [DSQty]
GO

ALTER TABLE [dbo].[AR_SODetItm] ADD  DEFAULT ((0)) FOR [ObCost]
GO

ALTER TABLE [dbo].[AR_SODetItm]  WITH CHECK ADD  CONSTRAINT [FK_AR_SODetItm_AR_SO] FOREIGN KEY([DocKey])
REFERENCES [dbo].[AR_SO] ([DocKey])
ON UPDATE CASCADE
ON DELETE CASCADE
GO

ALTER TABLE [dbo].[AR_SODetItm] CHECK CONSTRAINT [FK_AR_SODetItm_AR_SO]
GO



