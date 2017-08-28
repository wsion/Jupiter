--TRUNCATE TABLE ColumnMapping;
--INSERT INTO ColumnMapping
--VALUES 
--('TBL1','Col1','Name'),
--('TBL1','Col2','Gender'),
--('TBL1','Col3','Address'),
--('TBL1','Col4','Contact'),
--('TBL1','Col5','Credit')
--GO

TRUNCATE TABLE [Source];
INSERT INTO [Source]
VALUES
('01','联想桥店'),
('02','天顺康大药房')
GO

TRUNCATE TABLE [ImportSetting];

--INSERT INTO [ImportSetting]
--VALUES
--	('ERP',
--	'DataLoad1',
--	'TBL1',
--	'[Col1],[Col2],[Col3],[Col4]',
--	'D:\Github\Jupiter\DataImport\DataFile\',
--	'D:\Github\Jupiter\DataImport\Archive\',
--	'ERP_u_memcard_reg_*.TXT',
--	'用户表'),

INSERT INTO [ImportSetting]
VALUES
	('01',
	'DataLoad_GL_CUSTOM',
	'GL_CUSTOM',
	'[TJBH]',
	'[MC]',
	'D:\Github\Jupiter\DataImport\DataFile\',
	'D:\Github\Jupiter\DataImport\Archive\',
	'01_u_org_busi_ini_data_*.TXT',
	'门店基本信息表'),
	('02',
	'DataLoad_GL_CUSTOM',
	'GL_CUSTOM',
	'[TJBH]',
	'[MC]',
	'D:\Github\Jupiter\DataImport\DataFile\',
	'D:\Github\Jupiter\DataImport\Archive\',
	'02_u_org_busi_ini_data_*.TXT',
	'门店基本信息表')

INSERT INTO [ImportSetting]
VALUES
	('01',
	'DataLoad_YW_KCK',
	'YW_KCK',
	'[HH]',
	'[PM],[SCDW],[GG],[TM],[PZWH],[LSJ],[HYJ]',
	'D:\Github\Jupiter\DataImport\DataFile\',
	'D:\Github\Jupiter\DataImport\Archive\',
	'01_u_ware_*.TXT',
	'产品基本信息表'),
	('02',
	'DataLoad_YW_KCK',
	'YW_KCK',
	'[HH]',
	'[PM],[SCDW],[GG],[TM],[PZWH],[LSJ],[HYJ]',
	'D:\Github\Jupiter\DataImport\DataFile\',
	'D:\Github\Jupiter\DataImport\Archive\',
	'02_u_ware_*.TXT',
	'产品基本信息表')

INSERT INTO [ImportSetting]
VALUES
	('01',
	'DataLoad_subywbalance',
	'subywbalance',
	'[SUBBH],[HH]',
	'[SJSL],[LSJ],[LSJ1],[fperiod]',
	'D:\Github\Jupiter\DataImport\DataFile\',
	'D:\Github\Jupiter\DataImport\Archive\',
	'01_u_store_c_*.TXT',
	'库存表'),
	('02',
	'DataLoad_subywbalance',
	'subywbalance',
	'[SUBBH],[HH]',
	'[SJSL],[LSJ],[LSJ1],[fperiod]',
	'D:\Github\Jupiter\DataImport\DataFile\',
	'D:\Github\Jupiter\DataImport\Archive\',
	'02_u_store_c_*.TXT',
	'库存表')

INSERT INTO [ImportSetting]
VALUES
	('01',
	'DataLoad_GL_HY',
	'GL_HY',
	'[ID]',
	'[NAME],[PhoneNumber],[JIFEN]',
	'D:\Github\Jupiter\DataImport\DataFile\',
	'D:\Github\Jupiter\DataImport\Archive\',
	'01_GL_HY_*.TXT',
	'用户信息表'),
	('01',
	'DataLoad_GL_HY',
	'GL_HY',
	'[ID]',
	'[NAME],[PhoneNumber],[JIFEN]',
	'D:\Github\Jupiter\DataImport\DataFile\',
	'D:\Github\Jupiter\DataImport\Archive\',
	'02_GL_HY_*.TXT',
	'用户信息表')

INSERT INTO [ImportSetting]
VALUES
	('01',
	'DataLoad_subfhd',
	'subfhd',
	'[LSH]',
	'[KDRQ],[SUBBH],[HH],[sl],[YHKH],[DH]',
	'D:\Github\Jupiter\DataImport\DataFile\',
	'D:\Github\Jupiter\DataImport\Archive\',
	'01_subfhd_*.TXT',
	'订单表'),
	('02',
	'DataLoad_subfhd',
	'subfhd',
	'[LSH]',
	'[KDRQ],[SUBBH],[HH],[sl],[YHKH],[DH]',
	'D:\Github\Jupiter\DataImport\DataFile\',
	'D:\Github\Jupiter\DataImport\Archive\',
	'02_subfhd_*.TXT',
	'订单表')
GO


