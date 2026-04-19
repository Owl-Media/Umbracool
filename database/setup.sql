:setvar DB_NAME "UmbracoDb"

IF DB_ID(N'$(DB_NAME)') IS NULL
BEGIN
    DECLARE @sql nvarchar(max) = N'CREATE DATABASE [' + REPLACE(N'$(DB_NAME)', N']', N']]') + N']';
    EXEC (@sql);
END;
GO
