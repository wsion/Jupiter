/************************************
* Author: Wei Wei
* Date: 25th Dec 2016
* Description: 
*************************************/

--IF EXISTS (SELECT Object_ID('SP_GetData'))
--BEGIN
--	DROP PROCEDURE [dbo].[SP_GetData]
--END;
--GO

CREATE PROCEDURE [dbo].[SP_GetData]
	@tableName VARCHAR(50)
AS
	--DECLARE	@tableName VARCHAR(50)='TBL1';
	DECLARE @ColName VARCHAR(50);
	DECLARE @Alias VARCHAR(50);
	DECLARE @sql VARCHAR(500)='SELECT ';
	
	DECLARE cur CURSOR FOR 
	SELECT  [ColumnName],[Alias] FROM ColumnMapping
	WHERE [TableName]=@tableName

	OPEN cur
		FETCH NEXT FROM cur INTO @ColName,@Alias
	WHILE @@FETCH_STATUS=0 
	BEGIN 
		SET @sql = @sql +'['+ @ColName +'] AS [' + @Alias +'], '
		FETCH NEXT FROM cur INTO @ColName,@Alias
	END
	CLOSE cur

	SET @sql =@sql + '[Uid],[Update],T2.[Desc] AS [Source]  FROM ' +@tableName +' T1 JOIN Source T2 ON T1.[Source]=T2.[UniqueKey]'

	EXEC(@sql)

RETURN 0
GO
--EXEC [SP_GetData] 'TBL1'