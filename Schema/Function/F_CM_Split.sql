CREATE FUNCTION [dbo].[F_CM_Split] (@text VARCHAR(MAX), @delimiter VARCHAR(1)=' ')
RETURNS @Strings TABLE
(
    ITEM_VALUE    VARCHAR(MAX)
)
AS
BEGIN
    DECLARE  @index INT
    SET @index = -1
    WHILE (LEN(@text) > 0)
        BEGIN
            SET @index = CHARINDEX(@delimiter,@text)
            IF (@index = 0) AND (LEN(@text) > 0)
            BEGIN    
                INSERT INTO @Strings VALUES (@text)
            BREAK
        END
        IF (@index > 1)
            BEGIN
            INSERT INTO @Strings VALUES (LEFT(@text,@index - 1))
            SET @text = RIGHT(@text,(LEN(@text) - @index))
            END
        ELSE
            SET @text = RIGHT(@text,(LEN(@text) - @index))
        END
    RETURN
  END
GO