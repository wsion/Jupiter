/************************************
Author: Wei Wei
Date: 25th Dec 2016
Description: 
Retrieve data from data load table, insert or update to target table.
*************************************/

CREATE PROCEDURE [dbo].[SP_Import]
	@dataloadTableName VARCHAR(50),
	@targetTableName VARCHAR(50),
	@columns VARCHAR(MAX),
	@source VARCHAR(50),
	@fileName VARCHAR(50)
AS
	DECLARE @sql VARCHAR(MAX);
	DECLARE @colName VARCHAR(20);

	--Update source and target
	SET @sql =  'UPDATE '+ @dataloadTableName + ' ' +
				'SET '+
				'[Source]=''' + @source + ''',' +
				'[Target]=''' + @targetTableName + ''',' +
				'[SourceFile]=''' + @fileName + ''' ' +
				'WHERE [UPDATE] IS NULL';
	PRINT @sql;
	EXEC(@sql);

	--Insert 
	SET @sql = 'INSERT INTO ' + @targetTableName + ' ' +
	'([Uid],' + @columns + ',[Source],[Update]) '+
	'SELECT [Uid],' + @columns + ',[Source],GETDATE() FROM '+ @dataloadTableName +' T1 '+
	'WHERE NOT EXISTS ( SELECT TOP 1 1 FROM ' + @targetTableName + ' T2 WHERE T2.[Uid]=T1.[Uid] AND T2.[Source]=t1.[Source]) AND T1.[Update] IS NULL';
	PRINT @sql;
	EXEC(@sql);

	--Update
	SET @sql='UPDATE T1 SET ';
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
	SET @sql=@sql +'[Update]=GETDATE() '+
			'FROM '+ @targetTableName +' T1 JOIN '+ @dataloadTableName +' T2 ON T2.[Uid]=T1.[Uid] AND T2.[Source]=t1.[Source] AND T2.[Update] IS NULL';
	PRINT @sql;
	EXEC(@sql);

	--Update loading table
	SET @sql =  'UPDATE '+ @dataloadTableName + ' ' +
				'SET '+
						'[Update]=GETDATE() ' +
				'WHERE ' +
						'[UPDATE] IS NULL'
	PRINT @sql;
	EXEC(@sql);

RETURN 0

--EXEC [SP_Import] 'DataLoad1','TBL1','Col1,Col2,Col3','PAYF','Test.txt'