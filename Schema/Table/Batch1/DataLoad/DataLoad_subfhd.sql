CREATE TABLE [dbo].[DataLoad_subfhd] --订单表
(
	[LSH]				NVARCHAR(20) NOT NULL PRIMARY KEY, --流水号
	[KDRQ]				NVARCHAR(50) NULL, --开单日期
	[SUBBH]				NVARCHAR(50) NULL, --门店编号
	[HH]				NVARCHAR(50) NULL, --商品编码
	[sl]				NVARCHAR(50) NULL, --销售数量
	[YHKH]				NVARCHAR(50) NULL, --销售会员卡号
	[DH]				NVARCHAR(50) NULL,--单号

	[SYS_SourceFile]	NVARCHAR(50) NULL,
	[SYS_Update]		DATETIME NULL
)
