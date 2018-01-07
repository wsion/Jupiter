CREATE TABLE [dbo].[ServerTable]
(
	[Id] INT IDENTITY PRIMARY KEY,
	ClientId VARCHAR(50) NOT NULL,
	Token VARCHAR(50) null,
	Msg VARCHAR(50) null
)
