-- CREATE DATABASE BookLibrary

USE BookLibrary

DROP TABLE IF EXISTS AuthorBook
DROP TABLE IF EXISTS Author

CREATE TABLE Author (
	Id INT NOT NULL,
	[Name] VARCHAR(200),
	[Surname] VARCHAR(200),
	CONSTRAINT PK_Author_Id PRIMARY KEY CLUSTERED (Id ASC)
)

INSERT INTO Author (Id, [Name], [SurName])
VALUES
(1,'Arthur','Doile'),
(2,'Lee','Child'),
(3,'Robert','Sedgewick'),
(4,'Kevin','Wayne'),
(5,'Mark','Twain');

DROP TABLE IF EXISTS Book

CREATE TABLE Book (
	Id INT NOT NULL,
	Title VARCHAR(200),
	ReleaseDate DATETIME2,
	CONSTRAINT PK_Book_Id PRIMARY KEY CLUSTERED (Id ASC)
)

INSERT INTO Book (Id, Title, ReleaseDate)
VALUES
(1,'The Case Notes of Sherlock Holmes','2020-05-12 00:00:00.000'),
(2,'No Plan B','2022-05-12 00:00:00.000'),
(3,'One shot','2005-04-05 00:00:00.000'),
(4,'Algorithms','2011-03-01 00:00:00.000'),
(5,'The Adventures of Tom Sawyer','1876-09-10 00:00:00.000'),
(6,'Adventures of Huckleberry Finn','1885-07-05 00:00:00.000');

CREATE TABLE AuthorBook (
	AuthorId INT NOT NULL,
	BookId INT NOT NULL,
	CONSTRAINT PK_AuthorBook PRIMARY KEY CLUSTERED (AuthorId, BookId),
    CONSTRAINT FK_AuthorBook_Author FOREIGN KEY (AuthorId) REFERENCES Author(Id) ON DELETE CASCADE,
	CONSTRAINT FK_AuthorBook_Book FOREIGN KEY (BookId) REFERENCES Book(Id) ON DELETE CASCADE
)

INSERT INTO AuthorBook (AuthorId, BookId)
VALUES
(1,1),
(2,2),
(2,3),
(3,4),
(4,4),
(5,5),
(5,6);
GO
/*
SELECT 
	b.Title as Book, 
	STRING_AGG(a.[Name] + ' ' + a.SurName,', ') as Authors, 
	b.ReleaseDate  
FROM AuthorBook ab
INNER JOIN Author a ON ab.AuthorId = a.Id
INNER JOIN Book b ON ab.BookId = b.Id
GROUP BY b.Title, b.ReleaseDate
ORDER BY b.Title, Authors
*/

CREATE OR ALTER PROCEDURE [dbo].[sp_Book_GetAll]
	@OrderBy NVARCHAR(100) = NULL,
	@OrderDirection INT = NULL,
	@Limit INT = 20,
	@Offset INT = 0,
	@SearchText NVARCHAR(MAX) = NULL
AS
BEGIN
	DECLARE @DEFAULTOrderDirection INT = 1
	DECLARE @DEFAULTOrderBY NVARCHAR(100) = 'Title'
	DECLARE @IsSearchParam INT = IIF(LEN(ISNULL(@SearchText,''))> 0, 1, 0)
	DECLARE @CleanSearchText NVARCHAR(MAX) = UPPER(@SearchText) -- TBD - Function with remove spec chars

	;WITH TotalCountCTE AS (
		SELECT COUNT(*) AS TotalCount
		FROM Book B
		WHERE @IsSearchParam = 0 OR UPPER(B.Title) LIKE '%' + @CleanSearchText + '%' 
	)

    SELECT
        B.Id AS Id,
        B.Title AS Title,
        B.ReleaseDate AS ReleaseDate,
		ISNULL('[' + STRING_AGG('{"Id":'+ CAST(A.Id AS VARCHAR(10)) + ',"Name":"'+ A.[Name]+ '","Surname":"' +A.Surname+ '"}',',') WITHIN GROUP (ORDER BY [Surname]) + ']','[]')
		AS AuthorsJson,
		tc.TotalCount
    FROM Book B
    LEFT JOIN AuthorBook BA ON B.Id = BA.BookId
    LEFT JOIN Author A ON BA.AuthorId = A.Id
	CROSS JOIN TotalCountCTE tc
	WHERE @IsSearchParam = 0 OR UPPER(B.Title) LIKE '%' + @CleanSearchText + '%' 
	GROUP BY B.Id, B.Title, B.ReleaseDate, tc.TotalCount
    ORDER BY
        CASE WHEN ISNULL(@OrderBy,@DEFAULTOrderBY) = 'Title' AND ISNULL(@OrderDirection,@DEFAULTOrderDirection)=1 THEN B.Title END ASC,
        CASE WHEN ISNULL(@OrderBy,@DEFAULTOrderBY) = 'Title' AND ISNULL(@OrderDirection,@DEFAULTOrderDirection)=2 THEN B.Title END DESC,
		CASE WHEN ISNULL(@OrderBy,@DEFAULTOrderBY) = 'ReleaseDate' AND ISNULL(@OrderDirection,@DEFAULTOrderDirection)=1 THEN B.ReleaseDate END ASC,
		CASE WHEN ISNULL(@OrderBy,@DEFAULTOrderBY) = 'ReleaseDate' AND ISNULL(@OrderDirection,@DEFAULTOrderDirection)=2 THEN B.ReleaseDate END DESC
	OFFSET @Offset ROWS
	FETCH NEXT @Limit ROWS ONLY

RETURN 0;
END
GO

-- EXEC [dbo].[sp_Book_GetAll] @SearchText = 'th'

CREATE OR ALTER PROCEDURE [dbo].[sp_Book_Get]
	@Id INT
AS
BEGIN

    SELECT
        B.Id AS Id,
        B.Title AS Title,
        B.ReleaseDate AS ReleaseDate,
		ISNULL('[' + STRING_AGG('{"Id":'+ CAST(A.Id AS VARCHAR(10)) + ',"Name":"'+ A.[Name]+ '","Surname":"' +A.Surname+ '"}',',') WITHIN GROUP (ORDER BY [Surname]) + ']','[]')
		AS AuthorsJson
    FROM Book B
    LEFT JOIN AuthorBook BA ON B.Id = BA.BookId
    LEFT JOIN Author A ON BA.AuthorId = A.Id
	WHERE B.Id = @Id
	GROUP BY B.Id, B.Title, B.ReleaseDate 

RETURN 0;
END
GO

-- EXEC [dbo].[sp_Book_Get] 4

CREATE OR ALTER PROCEDURE [dbo].[sp_Author_GetAll]
	@OrderBy NVARCHAR(100) = NULL,
	@OrderDirection INT = NULL,
	@Limit INT = 20,
	@Offset INT = 0,
	@SearchText VARCHAR(MAX) = NULL
AS
BEGIN
	DECLARE @DEFAULTOrderDirection INT = 1
	DECLARE @DEFAULTOrderBY VARCHAR(100) = 'Name'
	DECLARE @IsSearchParam INT = IIF(LEN(ISNULL(@SearchText,''))> 0, 1, 0)
	DECLARE @CleanSearchText NVARCHAR(MAX) = UPPER(@SearchText) -- TBD - Function with remove spec chars

	;WITH TotalCountCTE AS (
		SELECT COUNT(*) AS TotalCount
		FROM Author A
		WHERE @IsSearchParam = 0 OR UPPER(A.[Name]+' '+A.Surname) LIKE '%' + @CleanSearchText + '%'
	)
	
	SELECT Id, [Name], Surname, BooksJson, tc.TotalCount FROM
    (SELECT
        A.Id AS Id,
        A.[Name] AS [Name],
        A.Surname AS Surname,
		ISNULL('[' + STRING_AGG('{"Id":'+ CAST(B.Id AS VARCHAR(10)) + ',"Title":"'+ B.[Title]+ '","ReleaseDate":"' +FORMAT(B.ReleaseDate, 'yyyy-MM-ddTHH:mm:ss.fff') + '"}',',') WITHIN GROUP (ORDER BY Title) + ']','[]')
		AS BooksJson,
		Count(B.Id) AS BookCount
    FROM Author A
    LEFT JOIN AuthorBook BA ON A.Id = BA.AuthorId
    LEFT JOIN Book B ON BA.BookId = B.Id
	WHERE @IsSearchParam = 0 OR UPPER(A.[Name]+' '+A.Surname) LIKE '%' + @CleanSearchText + '%'
	GROUP BY A.Id, A.[Name], A.Surname) a
	CROSS JOIN TotalCountCTE tc
    ORDER BY
        CASE WHEN ISNULL(@OrderBy,@DEFAULTOrderBY) = 'Name' AND ISNULL(@OrderDirection,@DEFAULTOrderDirection)=1 THEN A.[Name]+A.Surname END ASC,
        CASE WHEN ISNULL(@OrderBy,@DEFAULTOrderBY) = 'Name' AND ISNULL(@OrderDirection,@DEFAULTOrderDirection)=2 THEN A.[Name]+A.Surname END DESC,
		CASE WHEN ISNULL(@OrderBy,@DEFAULTOrderBY) = 'BookCount' AND ISNULL(@OrderDirection,@DEFAULTOrderDirection)=1 THEN BookCount END ASC,
		CASE WHEN ISNULL(@OrderBy,@DEFAULTOrderBY) = 'BookCount' AND ISNULL(@OrderDirection,@DEFAULTOrderDirection)=2 THEN BookCount END DESC,
		A.[Name]+A.Surname ASC
	OFFSET @Offset ROWS
	FETCH NEXT @Limit ROWS ONLY
RETURN 0;
END
GO

-- EXEC [dbo].[sp_Author_GetAll] @SearchText = 'o', @Limit = 4, @Offset = 0

CREATE OR ALTER PROCEDURE [dbo].[sp_Author_Get]
	@Id INT
AS
BEGIN

	SELECT
        A.Id AS Id,
        A.[Name] AS [Name],
        A.Surname AS Surname,
		ISNULL('[' + STRING_AGG('{"Id":'+ CAST(B.Id AS VARCHAR(10)) + ',"Title":"'+ B.[Title]+ '","ReleaseDate":"' +FORMAT(B.ReleaseDate, 'yyyy-MM-ddTHH:mm:ss.fff') + '"}',',') WITHIN GROUP (ORDER BY Title) + ']','[]')	AS BooksJson
    FROM Author A
    LEFT JOIN AuthorBook BA ON A.Id = BA.AuthorId
    LEFT JOIN Book B ON BA.BookId = B.Id
	WHERE A.Id = @Id
	GROUP BY A.Id, A.[Name], A.Surname

RETURN 0;
END
GO

--EXEC [dbo].[sp_Author_Get] 5

CREATE OR ALTER PROCEDURE [dbo].[sp_Author_CreateOrUpdate]
	@Id INT = NULL,
	@Name VARCHAR(200),
	@Surname VARCHAR(200)
AS
BEGIN
	DECLARE	@ERR_MSG AS NVARCHAR(4000) ,@ERR_STATE AS SMALLINT 
	IF NOT EXISTS (SELECT 1 FROM [dbo].[Author] WHERE Id = ISNULL(@Id,0))
	BEGIN
		SET @Id = (SELECT MAX(Id) FROM [dbo].[Author]) + 1
		BEGIN TRY
		BEGIN TRANSACTION;
			INSERT INTO [dbo].[Author] ([Id],[Name],[Surname]) VALUES (@Id, @Name, @Surname)
		COMMIT TRANSACTION;
		END TRY
		BEGIN CATCH
			ROLLBACK TRANSACTION
			SELECT @ERR_MSG = 'Procedure Name: ' + OBJECT_NAME(@@PROCID) +'. '+ ERROR_MESSAGE(), @ERR_STATE = ERROR_STATE();
			THROW 50001, @ERR_MSG, @ERR_STATE;
		END CATCH;
	END
	ELSE
	BEGIN
		BEGIN TRY
		BEGIN TRANSACTION;
			UPDATE [dbo].[Author] SET
				[Name] = @Name,
				[Surname] = @Surname
			WHERE Id = @Id
		COMMIT TRANSACTION;
		END TRY
		BEGIN CATCH
			ROLLBACK TRANSACTION
			SELECT @ERR_MSG = 'Procedure Name: ' + OBJECT_NAME(@@PROCID) +'. '+ ERROR_MESSAGE(), @ERR_STATE = ERROR_STATE();
			THROW 50001, @ERR_MSG, @ERR_STATE;
		END CATCH;
	END

	SELECT
		A.Id AS Id,
		A.[Name] AS [Name],
		A.Surname AS Surname,
		ISNULL('[' + STRING_AGG('{"Id":'+ CAST(B.Id AS VARCHAR(10)) + ',"Title":"'+ B.[Title]+ '","ReleaseDate":"' +FORMAT(B.ReleaseDate, 'yyyy-MM-ddTHH:mm:ss.fff') + '"}',',') WITHIN GROUP (ORDER BY Title) + ']','[]')	AS BooksJson
	FROM Author A
	LEFT JOIN AuthorBook BA ON A.Id = BA.AuthorId
	LEFT JOIN Book B ON BA.BookId = B.Id
	WHERE A.Id = @Id
	GROUP BY A.Id, A.[Name], A.Surname

	RETURN 0;
END
GO

--EXEC [dbo].[sp_Author_CreateOrUpdate] @Id = 2, @Name = 'Lee', @Surname = 'Child'
--EXEC [dbo].[sp_Author_CreateOrUpdate] @Name = 'Walter', @Surname = 'Scott'

DROP PROCEDURE IF EXISTS [dbo].[sp_Book_CreateOrUpdate];

DROP TYPE IF EXISTS [dbo].[ListInt];

CREATE TYPE [dbo].[ListInt] AS TABLE
(
	Value INT NOT NULL PRIMARY KEY
);
GO

CREATE OR ALTER PROCEDURE [dbo].[sp_Book_CreateOrUpdate]
	@Id INT = NULL,
	@Title VARCHAR(200),
	@ReleaseDate DATETIME,
	@AuthorIds [dbo].[ListInt] readonly
AS
BEGIN
	DECLARE	@ERR_MSG AS NVARCHAR(4000) ,@ERR_STATE AS SMALLINT 
	IF NOT EXISTS (SELECT 1 FROM [dbo].[Book] WHERE Id = ISNULL(@Id,0))
	BEGIN
		SET @Id = (SELECT MAX(Id) FROM [dbo].[Book]) + 1
		BEGIN TRY
		BEGIN TRANSACTION;
			INSERT INTO [dbo].[Book] ([Id],[Title],[ReleaseDate]) VALUES (@Id, @Title, @ReleaseDate);
			INSERT INTO [dbo].[AuthorBook] (AuthorId, BookId) SELECT Value, @Id FROM @AuthorIds;
		COMMIT TRANSACTION;
		END TRY
		BEGIN CATCH
			ROLLBACK TRANSACTION
			SELECT @ERR_MSG = 'Procedure Name: ' + OBJECT_NAME(@@PROCID) +'. '+ ERROR_MESSAGE(), @ERR_STATE = ERROR_STATE();
			THROW 50001, @ERR_MSG, @ERR_STATE;
		END CATCH;
	END
	ELSE
	BEGIN
		BEGIN TRY
		BEGIN TRANSACTION;
			UPDATE [dbo].[Book] SET
				[Title] = @Title,
				[ReleaseDate] = @ReleaseDate
			WHERE Id = @Id;
			DELETE FROM [dbo].[AuthorBook] WHERE BookId = @Id;
			INSERT INTO [dbo].[AuthorBook] (AuthorId, BookId) SELECT Value, @Id FROM @AuthorIds;
		COMMIT TRANSACTION;
		END TRY
		BEGIN CATCH
			ROLLBACK TRANSACTION
			SELECT @ERR_MSG = 'Procedure Name: ' + OBJECT_NAME(@@PROCID) +'. '+ ERROR_MESSAGE(), @ERR_STATE = ERROR_STATE();
			THROW 50001, @ERR_MSG, @ERR_STATE;
		END CATCH;
	END

    SELECT
        B.Id AS Id,
        B.Title AS Title,
        B.ReleaseDate AS ReleaseDate,
		ISNULL('[' + STRING_AGG('{"Id":'+ CAST(A.Id AS VARCHAR(10)) + ',"Name":"'+ A.[Name]+ '","Surname":"' +A.Surname+ '"}',',') WITHIN GROUP (ORDER BY [Surname]) + ']','[]') AS AuthorsJson
    FROM Book B
    LEFT JOIN AuthorBook BA ON B.Id = BA.BookId
    LEFT JOIN Author A ON BA.AuthorId = A.Id
	WHERE B.Id = @Id
	GROUP BY B.Id, B.Title, B.ReleaseDate 

	RETURN 0;
END
GO

--DECLARE @Authors dbo.ListInt;
--INSERT INTO @Authors (Value) VALUES (5),(2); 
--INSERT INTO @Authors (Value) VALUES (5); 
--EXEC [dbo].[sp_Book_CreateOrUpdate] @Id = 5, @Title = 'Ivanhoe', @ReleaseDate = '1819.12.31 00:00:00.000', @AuthorIds = @Authors

CREATE OR ALTER PROCEDURE [dbo].[sp_Book_Delete]
	@Id INT = NULL
AS
BEGIN
	DECLARE	@ERR_MSG AS NVARCHAR(4000) ,@ERR_STATE AS SMALLINT 

		BEGIN TRY
		BEGIN TRANSACTION;
			DELETE FROM [dbo].[Book] WHERE 1=1 AND Id = ISNULL(@Id,0);
		COMMIT TRANSACTION;
		END TRY
		BEGIN CATCH
			ROLLBACK TRANSACTION
			SELECT @ERR_MSG = 'Procedure Name: ' + OBJECT_NAME(@@PROCID) +'. '+ ERROR_MESSAGE(), @ERR_STATE = ERROR_STATE();
			THROW 50001, @ERR_MSG, @ERR_STATE;
		END CATCH;

	RETURN 0;
END
GO

CREATE OR ALTER PROCEDURE [dbo].[sp_Author_Delete]
	@Id INT = NULL
AS
BEGIN
	DECLARE	@ERR_MSG AS NVARCHAR(4000) ,@ERR_STATE AS SMALLINT 

		BEGIN TRY
		BEGIN TRANSACTION;
			DELETE FROM [dbo].[Author] WHERE 1=1 AND Id = ISNULL(@Id,0);
		COMMIT TRANSACTION;
		END TRY
		BEGIN CATCH
			ROLLBACK TRANSACTION
			SELECT @ERR_MSG = 'Procedure Name: ' + OBJECT_NAME(@@PROCID) +'. '+ ERROR_MESSAGE(), @ERR_STATE = ERROR_STATE();
			THROW 50001, @ERR_MSG, @ERR_STATE;
		END CATCH;

	RETURN 0;
END
GO