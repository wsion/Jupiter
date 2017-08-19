CREATE TABLE [dbo].[DataLoad_GL_HY] --用户信息表
(
	[ID]				NVARCHAR(20) NULL, --会员ID（卡号）
	[NAME]				NVARCHAR(50) NULL, --会员姓名
	[PhoneNumber]		NVARCHAR(50) NULL, --会员电话
	[JIFEN]				NVARCHAR(50) NULL, --积分

	[SYS_SourceFile]	NVARCHAR(50) NULL,
	[SYS_Update]		DATETIME NULL
)
