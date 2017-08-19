/************************************
Author: Wei Wei
Date: 25th Dec 2016
Description: 
Retrieve data from data load table, insert or update to target table.
*************************************/

CREATE PROCEDURE [dbo].[SP_Import]
	@dataloadTableName VARCHAR(50),
	@targetTableName VARCHAR(50),
	@keyColumns VARCHAR(MAX),
	@columns VARCHAR(MAX),
	@source VARCHAR(50),
	@fileName VARCHAR(50)
AS
	DECLARE @sql VARCHAR(MAX) = '';
	DECLARE @colName VARCHAR(20) = '';
	DECLARE @keyCols VARCHAR(1000) = ''

	-- 1. Update [SYS_SourceFile] of DataLoad table
	SET @sql =  'UPDATE '+ @dataloadTableName + ' ' +
				'SET '+
				'[SYS_SourceFile]=''' + @fileName + ''' ' +
				'WHERE [SYS_Update] IS NULL';
	PRINT @sql;
	EXEC(@sql);



	-- 2. Insert data to target table

	-- 2.1 Build condition statement of key columns
	-- e.g. 
	-- T1.Col1 = T2.Col1 AND T1.Col2=T2.Col2 AND 1=1
	SET @keyCols = '';
	DECLARE cur CURSOR FOR 
			SELECT ITEM_VALUE FROM dbo.F_CM_Split(@keyColumns,',');
	OPEN cur;
	FETCH NEXT FROM cur INTO @colName;
	WHILE (@@FETCH_STATUS=0)
	BEGIN
			SET @keyCols=@keyCols + 'T1.' + @colName + '=T2.' + @colName + ' AND '
			FETCH NEXT FROM cur INTO @colName;
	END
	CLOSE cur;
	DEALLOCATE CUR;
	SET @keyCols = @keyCols + ' 1=1 ';

	--2.2 Build complete INSERT statement
	-- e.g. 
	-- INSERT INTO [Target] (key1,key2,col1,col2)
	-- SELECT key1,key2,col1,col2 FROM [DATALOAD_Target] T1
	-- WHERE NOT EXISTS (SELECT TOP 1 1 FROM [Target] T2
	-- WHERE T1.Col1 = T2.Col1 AND T1.Col2=T2.Col2 AND 1=1)
	-- AND T1.[Update] IS NULL
	SET @sql = 'INSERT INTO ' + @targetTableName + ' ' +
	'(' + @keyColumns + ',' + @columns + ') '+
	'SELECT ' + @keyColumns + ',' + @columns + ' FROM ' + @dataloadTableName +' T1 '+
	'WHERE NOT EXISTS ( SELECT TOP 1 1 FROM ' + @targetTableName + ' T2 ' + 
	'WHERE ' + @keyCols + ') ' +
	'AND T1.[SYS_UPDATE] IS NULL'
	PRINT @sql;
	EXEC(@sql);



	-- 3. Update target table

	-- 3.1 Build column statement
	-- e.g.
	-- col1 = T2.col1, col2 = T2.col2,
	SET @sql='';
	DECLARE cur CURSOR FOR 
		SELECT ITEM_VALUE FROM dbo.F_CM_Split(@columns,',');
	OPEN cur;
	FETCH NEXT FROM cur INTO @colName;
	WHILE (@@FETCH_STATUS=0)
	BEGIN
			SET @sql=@sql + @colName + '=T2.' + @colName + ','
			FETCH NEXT FROM cur INTO @colName;
	END
	CLOSE cur;
	DEALLOCATE CUR;
	-- 3.1.1 Remove the last comma (,)
	SET @sql=SUBSTRING(@sql,0,LEN(@sql))

	-- 3.2 Build complete statement
	-- e.g.
	-- UPDATE T1 SET
	-- col1 = T2.col1, col2 = T2.col
	-- FROM [Target] T1 JOIN [DATALOAD_Target] T2
	-- ON T2.[Update] IS NULL AND
	-- T1.Col1 = T2.Col1 AND T1.Col2=T2.Col2 AND 1=1
		

	SET @sql='UPDATE T1 SET ' + @sql + ' '	+
			'FROM '+ @targetTableName +' T1 JOIN '+ @dataloadTableName +' T2 ' + 
			'ON T2.[SYS_UPDATE] IS NULL AND '+
			@keyCols
	PRINT @sql;
	EXEC(@sql);



	-- 4. Update loading table
	SET @sql =  'UPDATE '+ @dataloadTableName + ' ' +
				'SET '+
						'[SYS_UPDATE]=GETDATE() ' +
				'WHERE ' +
						'[SYS_UPDATE] IS NULL'
	PRINT @sql;
	EXEC(@sql);

RETURN 0

--EXEC [SP_Import] 'DataLoad1','TBL1','Col1,Col2','Col3,Col4','PAYF','Test.txt'