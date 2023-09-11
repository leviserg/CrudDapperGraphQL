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
(4,'Kevin','Wayne')

DROP TABLE IF EXISTS Book

CREATE TABLE Book (
	Id INT NOT NULL,
	Title VARCHAR(200),
	ReleaseDate DATETIME,
	CONSTRAINT PK_Book_Id PRIMARY KEY CLUSTERED (Id ASC)
)

INSERT INTO Book (Id, Title, ReleaseDate)
VALUES
(1,'The Case Notes of Sherlock Holmes','2020-05-12 00:00:00.000'),
(2,'No Plan B','2022-05-12 00:00:00.000'),
(3,'One shot','2005-04-05 00:00:00.000'),
(4,'Algorithms','2011-03-01 00:00:00.000')



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
(4,4)
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
	@OrderDirection INT = NULL
AS
BEGIN
	DECLARE @DEFAULTOrderDirection INT = 1
	DECLARE @DEFAULTOrderBY VARCHAR(100) = 'Title'

    SELECT
        B.Id AS Id,
        B.Title AS Title,
        B.ReleaseDate AS ReleaseDate,
		'[' + STRING_AGG('{"Id":'+ CAST(A.Id AS VARCHAR(10)) + ',"Name":"'+ A.[Name]+ '","Surname":"' +A.Surname+ '"}',',') WITHIN GROUP (ORDER BY [Surname]) + ']'
		AS AuthorsJson
    FROM Book B
    JOIN AuthorBook BA ON B.Id = BA.BookId
    JOIN Author A ON BA.AuthorId = A.Id
	GROUP BY B.Id, B.Title, B.ReleaseDate 
    ORDER BY
        CASE WHEN ISNULL(@OrderBy,@DEFAULTOrderBY) = 'Title' AND ISNULL(@OrderDirection,@DEFAULTOrderDirection)=1 THEN B.Title END ASC,
        CASE WHEN ISNULL(@OrderBy,@DEFAULTOrderBY) = 'Title' AND ISNULL(@OrderDirection,@DEFAULTOrderDirection)=2 THEN B.Title END DESC,
		CASE WHEN ISNULL(@OrderBy,@DEFAULTOrderBY) = 'ReleaseDate' AND ISNULL(@OrderDirection,@DEFAULTOrderDirection)=1 THEN B.ReleaseDate END ASC,
		CASE WHEN ISNULL(@OrderBy,@DEFAULTOrderBY) = 'ReleaseDate' AND ISNULL(@OrderDirection,@DEFAULTOrderDirection)=2 THEN B.ReleaseDate END DESC
RETURN 0;
END

EXEC [dbo].[sp_Book_GetAll]

CREATE OR ALTER PROCEDURE [dbo].[sp_Book_Get]
	@Id INT
AS
BEGIN

    SELECT
        B.Id AS Id,
        B.Title AS Title,
        B.ReleaseDate AS ReleaseDate,
		'[' + STRING_AGG('{"Id":'+ CAST(A.Id AS VARCHAR(10)) + ',"Name":"'+ A.[Name]+ '","Surname":"' +A.Surname+ '"}',',') WITHIN GROUP (ORDER BY [Surname]) + ']'
		AS AuthorsJson
    FROM Book B
    JOIN AuthorBook BA ON B.Id = BA.BookId
    JOIN Author A ON BA.AuthorId = A.Id
	WHERE B.Id = @Id
	GROUP BY B.Id, B.Title, B.ReleaseDate 

RETURN 0;
END

EXEC [dbo].[sp_Book_Get] 4

CREATE OR ALTER PROCEDURE [dbo].[sp_Author_GetAll]
	@OrderBy NVARCHAR(100) = NULL,
	@OrderDirection INT = NULL
AS
BEGIN
	DECLARE @DEFAULTOrderDirection INT = 1
	DECLARE @DEFAULTOrderBY VARCHAR(100) = 'Surname'
	
	SELECT Id, [Name], Surname, BooksJson FROM
    (SELECT
        A.Id AS Id,
        A.[Name] AS [Name],
        A.Surname AS Surname,
		'[' + STRING_AGG('{"Id":'+ CAST(B.Id AS VARCHAR(10)) + ',"Title":"'+ B.[Title]+ '","ReleaseDate":"' +FORMAT(B.ReleaseDate, 'yyyy-MM-ddTHH:mm:ss.fff') + '"}',',') WITHIN GROUP (ORDER BY Title) + ']'
		AS BooksJson,
		Count(B.Id) AS BookCount
    FROM Author A
    JOIN AuthorBook BA ON A.Id = BA.AuthorId
    JOIN Book B ON BA.BookId = B.Id
	GROUP BY A.Id, A.[Name], A.Surname) a
    ORDER BY
        CASE WHEN ISNULL(@OrderBy,@DEFAULTOrderBY) = 'Surname' AND ISNULL(@OrderDirection,@DEFAULTOrderDirection)=1 THEN A.Surname END ASC,
        CASE WHEN ISNULL(@OrderBy,@DEFAULTOrderBY) = 'Surname' AND ISNULL(@OrderDirection,@DEFAULTOrderDirection)=2 THEN A.Surname END DESC,
		CASE WHEN ISNULL(@OrderBy,@DEFAULTOrderBY) = 'BookCount' AND ISNULL(@OrderDirection,@DEFAULTOrderDirection)=1 THEN BookCount END ASC,
		CASE WHEN ISNULL(@OrderBy,@DEFAULTOrderBY) = 'BookCount' AND ISNULL(@OrderDirection,@DEFAULTOrderDirection)=2 THEN BookCount END DESC,
		A.Surname ASC
RETURN 0;
END

EXEC [dbo].[sp_Author_GetAll]


CREATE OR ALTER PROCEDURE [dbo].[sp_Author_Get]
	@Id INT
AS
BEGIN

	SELECT
        A.Id AS Id,
        A.[Name] AS [Name],
        A.Surname AS Surname,
		'[' + STRING_AGG('{"Id":'+ CAST(B.Id AS VARCHAR(10)) + ',"Title":"'+ B.[Title]+ '","ReleaseDate":"' +FORMAT(B.ReleaseDate, 'yyyy-MM-ddTHH:mm:ss.fff') + '"}',',') WITHIN GROUP (ORDER BY Title) + ']'
		AS BooksJson
    FROM Author A
    JOIN AuthorBook BA ON A.Id = BA.AuthorId
    JOIN Book B ON BA.BookId = B.Id
	WHERE A.Id = @Id
	GROUP BY A.Id, A.[Name], A.Surname

RETURN 0;
END

EXEC [dbo].[sp_Author_Get] 1

/*
CREATE OR ALTER PROCEDURE [dbo].[pub_BookNotification_CreateOrUpdate]
	@UserId			INT,
	@BookId			UNIQUEIDENTIFIER,
	@NotificationStatusId INT,
	@IsChecked BIT NULL
AS
BEGIN

		DECLARE @Message NVARCHAR(MAX) = (SELECT [MessagePattern] FROM [dbo].[BookNotificationStatus] WHERE Id = @NotificationStatusId AND [IsActive]=1);
		DECLARE @BookName NVARCHAR(500), @SourceUrl NVARCHAR(255), @CreatedDate DATETIME2, @BookState NVARCHAR(MAX);
		DECLARE @DEFAULT_ESTIMATION_TIME_IF_NOT_FOUND INT = 180; -- 3 min

		DECLARE @EstimationTime INT = 0;
		DECLARE @TextToReplaceWithLink  NVARCHAR(20) = '{{BookLinkTitle}}';

		DECLARE @TextToReplaceWithEstimationTime  NVARCHAR(22) = '{{EstimationTimeText}}';
		DECLARE @STATISTICWINDOW DATETIME = DATEADD(WEEK, -2, GETUTCDATE())

		DECLARE @Topics TABLE (
			TopicId INT
		);
	
		INSERT INTO @Topics (TopicId)
		SELECT JSON_VALUE([value],'$.TopicId') AS TopicId
		FROM OPENJSON((SELECT [BookState] FROM [dbo].[Book] WHERE Id = @BookId),'$.Topics')

		SELECT @EstimationTime = ISNULL(MAX(a.estTime),@DEFAULT_ESTIMATION_TIME_IF_NOT_FOUND) FROM (

			SELECT t.TopicId, ISNULL(((AVG(DATEDIFF(SECOND,[DateStart],[DateEnd])) + MIN(DATEDIFF(SECOND,[DateStart],[DateEnd])))/2)+1,1) as estTime -- (+1) - to avoid return 0
			FROM [dbo].[ProgressTimings] pt
			INNER JOIN @Topics t ON t.TopicId = pt.TopicId
			WHERE [DateStart] >= @STATISTICWINDOW AND [DateEnd] IS NOT NULL
			GROUP BY t.TopicId
		
		) a

		SELECT 
			@BookName = CASE WHEN b.BookTypeId = 1 THEN
				ISNULL(b.[Name] + IIF(c.Ticker IS NOT NULL,' (' + c.Ticker + ')',''),'') 
			ELSE
				REPLACE(ISNULL(b.[Name],''),'-',' ')
			END,
			@CreatedDate = COAlESCE(b.[UpdatedDateTime],b.[CreatedDateTime],GETUTCDATE()),
			@SourceUrl = ISNULL(b.[SourceUrl],''),
			@BookState = ISNULL(b.[BookState],'')
		FROM 
			[dbo].[Book] b
		LEFT JOIN [dbo].[Company] c ON b.CompanyId = c.Id
		WHERE b.Id = @BookId

		DECLARE @RemainingTime VARCHAR(20) = IIF(@EstimationTime >= 60, CAST(@EstimationTime/60 AS VARCHAR(4))+ ' min', CAST(@EstimationTime AS VARCHAR(2)) + ' sec')

		SET @Message = REPLACE(REPLACE(
							REPLACE(
								REPLACE(
									REPLACE(
										REPLACE(@Message,'{{text}}',
											IIF(@NotificationStatusId = 3, @BookName + ' Briefing Book failed to process', -- failed
												IIF(@NotificationStatusId = 1,@TextToReplaceWithLink + ' approximately ' + @TextToReplaceWithEstimationTime +' remaining', -- in progress
													@TextToReplaceWithLink))), -- ready
									'{{timestamp}}',CAST(@CreatedDate AS NVARCHAR(30))),
								'{{sourceUrl}}',@SourceUrl),
							'{{error}}','"Book compilation has been failed"'),
						'{{estimationtime}}',CAST(@EstimationTime AS VARCHAR(10))),
						'{{linkTitle}}', @BookName + ' Briefing Book')

		IF NOT EXISTS (SELECT 1 FROM [dbo].[BookNotification] WHERE UserId = @UserId AND BookId = @BookId)
		BEGIN

			BEGIN TRY
			BEGIN TRANSACTION;
				INSERT INTO [dbo].[BookNotification]
					([UserId],[BookId],[NotificationStatusId],[CreatedDateTime],[Message]) 
				VALUES (@UserId, @BookId, @NotificationStatusId, GETUTCDATE(), @Message)

			COMMIT TRANSACTION;
		    END TRY
			BEGIN CATCH
				 ROLLBACK TRANSACTION;
			END CATCH;
		END
		ELSE
		BEGIN
			BEGIN TRY
			BEGIN TRANSACTION;
				UPDATE [dbo].[BookNotification] SET
					[NotificationStatusId] = @NotificationStatusId,
					[UpdatedDateTime] = GETUTCDATE(),
					[Message] = @Message,
					[IsChecked] = ISNULL(@IsChecked,0)
				WHERE UserId = @UserId AND BookId = @BookId

			COMMIT TRANSACTION;
		    END TRY
			BEGIN CATCH
				 ROLLBACK TRANSACTION;
			END CATCH;
		END

		SELECT 
			 bn.[UserId]
			,u.[Email] AS [UserEmail]
			,bn.[BookId]
			,bn.[NotificationStatusId] as [NotificationStatus]
			,bn.[Message] AS [MessageJson]
			,bn.[CreatedDateTime]
			,bn.[UpdatedDateTime]
			,bn.[IsChecked]
			,b.BookTypeId
			,bt.[Name] AS BookType
			,b.[CompanyId]
			,b.[IndividualId]
			,b.IsQuick
		FROM [dbo].[BookNotification] bn
		INNER JOIN [dbo].[User] u ON bn.UserId = u.Id
		INNER JOIN [dbo].[Book] b ON bn.BookId = b.Id
		INNER JOIN [dbo].[BookType] bt ON b.BookTypeId = bt.Id
		WHERE [UserId] = @UserId AND [BookId]=@BookId AND [IsChecked] = 0

END
*/