use jupiter;

select * from ImportSetting;

select * from ColumnMapping;

select * from [source];

select * from DataLoad;

select * from TBL1;
select * from TBL2;

select * from dbo.F_CM_Split('Col1,Col2,Col3',',')
INSERT INTO TBL1
([Uid], Col1,Col2,[Source],[Update])
SELECT [Uid], Col1,Col2,[Source],GETDATE() FROM DataLoad T1
WHERE NOT EXISTS ( SELECT TOP 1 1 FROM TBL1 T2 WHERE T2.[Uid]=T1.[Uid] AND T2.[Source]=t1.[Source])

SELECT * FROM TBL1

UPDATE T1
	SET Col1=T2.Col1,[Update]=GETDATE()
FROM TBL1 T1
	JOIN DataLoad T2 ON T2.[Uid]=T1.[Uid] AND T2.[Source]=t1.[Source]

	TRUNCATE TABLE TBL1

truncate table [dbo].[DataLoad]
truncate table TBL1
truncate table TBL2

SELECT * FROM TBL1
SELECT * FROM TBL2

exec [dbo].[SP_GetData] 'TBL1'
