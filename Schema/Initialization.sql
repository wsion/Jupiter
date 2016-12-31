TRUNCATE TABLE ColumnMapping;
INSERT INTO ColumnMapping
VALUES 
('TBL1','Col1','Name'),
('TBL1','Col2','Gender'),
('TBL1','Col3','Address'),
('TBL1','Col4','Contact'),
('TBL1','Col5','Credit')
GO

TRUNCATE TABLE [Source];
INSERT INTO [Source]
VALUES
('PAYF','平安药房'),
('ZSMDYF','真善美大药房')
GO

TRUNCATE TABLE [ImportSetting];
INSERT INTO [ImportSetting]
VALUES
('PAYF','TBL1','D:\Github\Jupiter\DataImport\DataFile\','PAYF_USERS_*.TXT','用户表'),
('PAYF','TBL2','D:\Github\Jupiter\DataImport\DataFile\','PAYF_PRODUCTION_*.TXT','产品表')
GO

TRUNCATE TABLE [TBL1]
INSERT INTO [TBL1]
([Source],[Update],[Col1],[Col2],[Col3],[Col4],[Col5])
VALUES
(
'PAYF',GETDATE(),'张三','男','重庆市沙坪坝区','138965943279','580'
)
GO