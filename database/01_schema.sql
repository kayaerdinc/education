/*
    EducationPlatform Veritabanı Şema Scripti
    - DB First yaklaşımı için oluşturulmuştur.
*/

IF DB_ID(N'EducationPlatform') IS NULL
BEGIN
    CREATE DATABASE EducationPlatform;
END;
GO

USE EducationPlatform;
GO

/* ------------------------------------------------------------
   Tablo mevcutsa sil (FK sırasına dikkat)
------------------------------------------------------------ */
DROP VIEW IF EXISTS dbo.vw_StudentExamSummary;
DROP VIEW IF EXISTS dbo.vw_SurveyDistribution;
DROP TABLE IF EXISTS dbo.Surveys;
DROP TABLE IF EXISTS dbo.ExamAnswers;
DROP TABLE IF EXISTS dbo.ExamAttempts;
DROP TABLE IF EXISTS dbo.StudentVideoProgress;
DROP TABLE IF EXISTS dbo.Choices;
DROP TABLE IF EXISTS dbo.Questions;
DROP TABLE IF EXISTS dbo.Videos;
DROP TABLE IF EXISTS dbo.Users;
GO

/* ------------------------------------------------------------
   USERS
------------------------------------------------------------ */
CREATE TABLE dbo.Users
(
    Id UNIQUEIDENTIFIER NOT NULL CONSTRAINT PK_Users PRIMARY KEY,
    Email NVARCHAR(256) NOT NULL CONSTRAINT UQ_Users_Email UNIQUE,
    PasswordHash VARBINARY(64) NOT NULL,
    PasswordSalt VARBINARY(32) NOT NULL,
    Role NVARCHAR(32) NOT NULL,
    FullName NVARCHAR(150) NOT NULL,
    IsActive BIT NOT NULL CONSTRAINT DF_Users_IsActive DEFAULT (1),
    CreatedAt DATETIME2(3) NOT NULL CONSTRAINT DF_Users_CreatedAt DEFAULT (SYSUTCDATETIME()),
    UpdatedAt DATETIME2(3) NULL
);
GO

ALTER TABLE dbo.Users
ADD CONSTRAINT CK_Users_Role
CHECK (Role IN (N'Student', N'Instructor'));
GO

/* ------------------------------------------------------------
   VIDEOS
------------------------------------------------------------ */
CREATE TABLE dbo.Videos
(
    Id UNIQUEIDENTIFIER NOT NULL CONSTRAINT PK_Videos PRIMARY KEY,
    Title NVARCHAR(200) NOT NULL,
    Description NVARCHAR(1000) NULL,
    Url NVARCHAR(500) NOT NULL,
    OrderIndex INT NOT NULL,
    DurationSeconds INT NULL,
    ThumbnailUrl NVARCHAR(500) NULL,
    CreatedAt DATETIME2(3) NOT NULL CONSTRAINT DF_Videos_CreatedAt DEFAULT (SYSUTCDATETIME())
);
GO

CREATE UNIQUE INDEX UX_Videos_OrderIndex
    ON dbo.Videos (OrderIndex);
GO

/* ------------------------------------------------------------
   STUDENT VIDEO PROGRESS
------------------------------------------------------------ */
CREATE TABLE dbo.StudentVideoProgress
(
    UserId UNIQUEIDENTIFIER NOT NULL,
    VideoId UNIQUEIDENTIFIER NOT NULL,
    CompletedAt DATETIME2(3) NOT NULL CONSTRAINT DF_Progress_CompletedAt DEFAULT (SYSUTCDATETIME()),
    CONSTRAINT PK_StudentVideoProgress PRIMARY KEY (UserId, VideoId),
    CONSTRAINT FK_Progress_Users FOREIGN KEY (UserId) REFERENCES dbo.Users (Id),
    CONSTRAINT FK_Progress_Videos FOREIGN KEY (VideoId) REFERENCES dbo.Videos (Id)
);
GO

/* ------------------------------------------------------------
   QUESTIONS
------------------------------------------------------------ */
CREATE TABLE dbo.Questions
(
    Id UNIQUEIDENTIFIER NOT NULL CONSTRAINT PK_Questions PRIMARY KEY,
    Body NVARCHAR(1000) NOT NULL,
    DisplayOrder INT NOT NULL,
    CreatedAt DATETIME2(3) NOT NULL CONSTRAINT DF_Questions_CreatedAt DEFAULT (SYSUTCDATETIME())
);
GO

CREATE UNIQUE INDEX UX_Questions_DisplayOrder
    ON dbo.Questions (DisplayOrder);
GO

/* ------------------------------------------------------------
   CHOICES
------------------------------------------------------------ */
CREATE TABLE dbo.Choices
(
    Id UNIQUEIDENTIFIER NOT NULL CONSTRAINT PK_Choices PRIMARY KEY,
    QuestionId UNIQUEIDENTIFIER NOT NULL,
    ChoiceLabel CHAR(1) NOT NULL,
    Body NVARCHAR(1000) NOT NULL,
    IsCorrect BIT NOT NULL CONSTRAINT DF_Choices_IsCorrect DEFAULT (0),
    CreatedAt DATETIME2(3) NOT NULL CONSTRAINT DF_Choices_CreatedAt DEFAULT (SYSUTCDATETIME()),
    CONSTRAINT FK_Choices_Questions FOREIGN KEY (QuestionId) REFERENCES dbo.Questions (Id)
);
GO

ALTER TABLE dbo.Choices
ADD CONSTRAINT CK_Choices_Label
CHECK (ChoiceLabel IN ('A', 'B', 'C', 'D'));
GO

CREATE UNIQUE INDEX UX_Choices_Question_Label
    ON dbo.Choices (QuestionId, ChoiceLabel);
GO

/* ------------------------------------------------------------
   EXAM ATTEMPTS
------------------------------------------------------------ */
CREATE TABLE dbo.ExamAttempts
(
    Id UNIQUEIDENTIFIER NOT NULL CONSTRAINT PK_ExamAttempts PRIMARY KEY,
    UserId UNIQUEIDENTIFIER NOT NULL,
    StartedAt DATETIME2(3) NOT NULL CONSTRAINT DF_ExamAttempts_StartedAt DEFAULT (SYSUTCDATETIME()),
    CompletedAt DATETIME2(3) NULL,
    TotalCorrect INT NULL,
    TotalIncorrect INT NULL,
    CONSTRAINT FK_ExamAttempts_Users FOREIGN KEY (UserId) REFERENCES dbo.Users (Id)
);
GO

CREATE INDEX IX_ExamAttempts_UserId
    ON dbo.ExamAttempts (UserId);
GO

/* ------------------------------------------------------------
   EXAM ANSWERS
------------------------------------------------------------ */
CREATE TABLE dbo.ExamAnswers
(
    AttemptId UNIQUEIDENTIFIER NOT NULL,
    QuestionId UNIQUEIDENTIFIER NOT NULL,
    ChoiceId UNIQUEIDENTIFIER NOT NULL,
    AnsweredAt DATETIME2(3) NOT NULL CONSTRAINT DF_ExamAnswers_AnsweredAt DEFAULT (SYSUTCDATETIME()),
    CONSTRAINT PK_ExamAnswers PRIMARY KEY (AttemptId, QuestionId),
    CONSTRAINT FK_ExamAnswers_Attempts FOREIGN KEY (AttemptId) REFERENCES dbo.ExamAttempts (Id) ON DELETE CASCADE,
    CONSTRAINT FK_ExamAnswers_Questions FOREIGN KEY (QuestionId) REFERENCES dbo.Questions (Id),
    CONSTRAINT FK_ExamAnswers_Choices FOREIGN KEY (ChoiceId) REFERENCES dbo.Choices (Id)
);
GO

CREATE INDEX IX_ExamAnswers_Attempt
    ON dbo.ExamAnswers (AttemptId);
GO

/* ------------------------------------------------------------
   SURVEYS
------------------------------------------------------------ */
CREATE TABLE dbo.Surveys
(
    Id UNIQUEIDENTIFIER NOT NULL CONSTRAINT PK_Surveys PRIMARY KEY,
    AttemptId UNIQUEIDENTIFIER NOT NULL,
    Liked BIT NOT NULL,
    ContentClarityRating TINYINT NOT NULL,
    InstructorSupportRating TINYINT NOT NULL,
    AdditionalFeedback NVARCHAR(1000) NULL,
    SubmittedAt DATETIME2(3) NOT NULL CONSTRAINT DF_Surveys_SubmittedAt DEFAULT (SYSUTCDATETIME()),
    CONSTRAINT FK_Surveys_Attempts FOREIGN KEY (AttemptId) REFERENCES dbo.ExamAttempts (Id) ON DELETE CASCADE,
    CONSTRAINT UQ_Surveys_Attempt UNIQUE (AttemptId)
);
GO

ALTER TABLE dbo.Surveys
ADD CONSTRAINT CK_Surveys_RatingRange
CHECK (ContentClarityRating BETWEEN 1 AND 5 AND InstructorSupportRating BETWEEN 1 AND 5);
GO

/* ------------------------------------------------------------
   Raporlama için VIEW'lar
------------------------------------------------------------ */
CREATE VIEW dbo.vw_StudentExamSummary
AS
SELECT
    u.Id AS UserId,
    u.FullName,
    u.Email,
    ea.Id AS AttemptId,
    ea.StartedAt,
    ea.CompletedAt,
    ea.TotalCorrect,
    ea.TotalIncorrect,
    COALESCE(ea.TotalCorrect, 0) + COALESCE(ea.TotalIncorrect, 0) AS TotalAnswered
FROM dbo.Users u
INNER JOIN dbo.ExamAttempts ea ON ea.UserId = u.Id
WHERE u.Role = N'Student';
GO

CREATE VIEW dbo.vw_SurveyDistribution
AS
SELECT
    ea.UserId,
    s.AttemptId,
    s.Liked,
    s.ContentClarityRating,
    s.InstructorSupportRating,
    s.AdditionalFeedback,
    s.SubmittedAt
FROM dbo.Surveys s
INNER JOIN dbo.ExamAttempts ea ON ea.Id = s.AttemptId;
GO

