CREATE TABLE [dbo].[subywbalance] --库存表
(
	[SUBBH]	NVARCHAR(20) NOT NULL, --门店编号
	[HH]	NVARCHAR(20) NOT NULL PRIMARY KEY ([SUBBH],[HH]), --商品编码
	[SJSL]	NVARCHAR(50) NULL, --库存数量
	[LSJ]	NVARCHAR(50) NULL, --门店零售价
	[LSJ1]	NVARCHAR(50) NULL, --门店会员价
	[fperiod] NVARCHAR(50) NULL, --当期
)
