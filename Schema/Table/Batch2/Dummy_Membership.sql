CREATE TABLE [dbo].[Dummy_Membership]
(
	[memcardno] VARCHAR(100) NOT NULL PRIMARY KEY,
	[idcard] VARCHAR(50) NULL,
	[sex] BIT NULL,
	[handset] VARCHAR(50) NULL,
	[createuser] VARCHAR(10) NULL,
	[createtime] DATETIME NULL

)