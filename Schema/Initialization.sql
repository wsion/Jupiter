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
('ERP','测试用药房')
GO

TRUNCATE TABLE [ImportSetting];
INSERT INTO [ImportSetting]
VALUES
('ERP','TBL1','[Col1],[Col2],[Col3],[Col4],[Col5],[Col6],[Col7],[Col8],[Col9],[Col10],[Col11],[Col12],[Col13],[Col14],[Col15],[Col16],[Col17],[Col18],[Col19],[Col20],[Col21],[Col22],[Col23],[Col24],[Col25],[Col26],[Col27]','D:\Github\Jupiter\DataImport\DataFile\','D:\Github\Jupiter\DataImport\Archive\','ERP_u_memcard_reg_*.TXT','用户表'),
('ERP','TBL2','[Col1],[Col2],[Col3],[Col4],[Col5],[Col6],[Col7],[Col8],[Col9],[Col10],[Col11],[Col12],[Col13],[Col14],[Col15],[Col16],[Col17],[Col18],[Col19],[Col20],[Col21],[Col22],[Col23],[Col24],[Col25],[Col26],[Col27],[Col28],[Col29],[Col30],[Col31],[Col32],[Col33]','D:\Github\Jupiter\DataImport\DataFile\','D:\Github\Jupiter\DataImport\Archive\','ERP_u_sale_m_*.TXT','订单表'),
('ERP','TBL3','[Col1],[Col2],[Col3],[Col4],[Col5],[Col6],[Col7],[Col8],[Col9],[Col10],[Col11],[Col12],[Col13],[Col14],[Col15],[Col16],[Col17],[Col18],[Col19],[Col20],[Col21],[Col22],[Col23],[Col24],[Col25],[Col26],[Col27],[Col28],[Col29],[Col30],[Col31],[Col32],[Col33],[Col34],[Col35],[Col36]','D:\Github\Jupiter\DataImport\DataFile\','D:\Github\Jupiter\DataImport\Archive\','ERP_u_store_m_*.TXT','库存表')
GO

TRUNCATE TABLE [TBL1]
INSERT INTO [TBL1]
([Uid],[Source],[Update],[Col1],[Col2],[Col3],[Col4],[Col5])
VALUES
(
'1','PAYF',GETDATE(),'张三','男','重庆市沙坪坝区','138965943279','580'
)
GO