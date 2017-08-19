CREATE TABLE [dbo].[GL_HY] --用户信息表
(
	[ID]			NVARCHAR(20) NOT NULL PRIMARY KEY, --会员ID（卡号）
	[NAME]			NVARCHAR(50) NULL, --会员姓名
	[PhoneNumber]	NVARCHAR(50) NULL, --会员电话
	[JIFEN]			NVARCHAR(50) NULL --积分
)
