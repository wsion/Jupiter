/************************************
* Author: Wei Wei
* Date: 25th Dec 2016
* Description: 
*************************************/

CREATE PROCEDURE [dbo].[SP_Import]
	@tableName VARCHAR(50),
	@source VARCHAR(50)
AS
	DECLARE @sql VARCHAR(1000);

	--Update source and target
	UPDATE 
			DataLoad 
	SET
			[Source]=@source,
			[Target]=@tableName
	WHERE
			[UPDATE] IS NULL;

	--Clean target table
	SET @sql='DELETE FROM '+ @tableName +' WHERE [SOURCE]='''+ @source +'''';
	EXEC(@sql);	
	PRINT @sql;

	--Insert records
	SET @sql= 
	'INSERT INTO ['+ @tableName +']
			(
			[Source],
			[Update],
			[Col1],[Col2],[Col3],[Col4],[Col5],[Col6],[Col7],[Col8],[Col9],[Col10],[Col11],[Col12],[Col13],[Col14],[Col15],[Col16],[Col17],[Col18],[Col19],[Col20]
			)
		SELECT 
			[Source],
			GETDATE(),
			[Col1],[Col2],[Col3],[Col4],[Col5],[Col6],[Col7],[Col8],[Col9],[Col10],[Col11],[Col12],[Col13],[Col14],[Col15],[Col16],[Col17],[Col18],[Col19],[Col20]
		FROM 
			DataLoad
		WHERE 
			[UPDATE] is null'

	EXEC(@sql);

	--Update loading table
	UPDATE DataLoad
	SET 
			[Update]=GETDATE()
	WHERE 
			[UPDATE] IS NULL;

RETURN 0

--EXEC [SP_Import] 'TBL1','PAYF'
