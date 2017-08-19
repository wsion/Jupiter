CREATE TABLE [dbo].[DataLoad_YW_KCK]
(
	[HH]				NVARCHAR(20) NULL, --商品编码
	[PM]				NVARCHAR(50) NULL, --商品名称
	[SCDW]				NVARCHAR(50) NULL, --生产单位
	[GG]				NVARCHAR(50) NULL, --规格
	[TM]				NVARCHAR(50) NULL, --条形码
	[PZWH]				NVARCHAR(50) NULL, --批准文号
	[LSJ]				NVARCHAR(50) NULL, --零售价
	[HYJ]				NVARCHAR(50) NULL, --会员价

	[SYS_SourceFile]	NVARCHAR(50) NULL,
	[SYS_Update]		DATETIME NULL
)
