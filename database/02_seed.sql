/*
    EducationPlatform Veritabanı Seed Scripti
    - Şema oluşturulduktan sonra çalıştırılmalıdır.
*/

USE EducationPlatform;
GO

/* ------------------------------------------------------------
   Mevcut veriyi temizle (FK sırasına göre)
------------------------------------------------------------ */
DELETE FROM dbo.Surveys;
DELETE FROM dbo.ExamAnswers;
DELETE FROM dbo.ExamAttempts;
DELETE FROM dbo.StudentVideoProgress;
DELETE FROM dbo.Choices;
DELETE FROM dbo.Questions;
DELETE FROM dbo.Videos;
DELETE FROM dbo.Users;
GO

/* ------------------------------------------------------------
   Kullanıcılar
------------------------------------------------------------ */
DECLARE @InstructorId UNIQUEIDENTIFIER = 'f9b1d2dc-5c2c-4b18-b3da-20f4f4ede801';
DECLARE @Student1Id  UNIQUEIDENTIFIER = '1f6c0b69-a7d9-47ce-87a4-41eb3e9f82a0';
DECLARE @Student2Id  UNIQUEIDENTIFIER = 'b6d99b7d-e651-4f73-9c72-7520f2d1af57';

DECLARE @InstructorSalt VARBINARY(32) = 0x7A9C9DDDF3AC44F5B5F2C6A1D3E4B5A6;
DECLARE @Student1Salt  VARBINARY(32) = 0x52E16F7AB49A4D84899CFA0C6E3DBBA1;
DECLARE @Student2Salt  VARBINARY(32) = 0xA9F63C2E4D5A4F5798BCAA1D4E6F8B21;

INSERT INTO dbo.Users (Id, Email, PasswordHash, PasswordSalt, Role, FullName, IsActive, CreatedAt)
VALUES
(@InstructorId, N'instructor@medtrain.local',
 HASHBYTES('SHA2_256', CONVERT(VARBINARY(128), N'Instructor123!') + @InstructorSalt),
 @InstructorSalt, N'Instructor', N'Dr. Ayşe Demir', 1, SYSUTCDATETIME()),
(@Student1Id, N'student1@medtrain.local',
 HASHBYTES('SHA2_256', CONVERT(VARBINARY(128), N'Student123!') + @Student1Salt),
 @Student1Salt, N'Student', N'Mehmet Karaca', 1, SYSUTCDATETIME()),
(@Student2Id, N'student2@medtrain.local',
 HASHBYTES('SHA2_256', CONVERT(VARBINARY(128), N'Student123!') + @Student2Salt),
 @Student2Salt, N'Student', N'Elif Yılmaz', 1, SYSUTCDATETIME());
GO

/* ------------------------------------------------------------
   Videolar
------------------------------------------------------------ */
DECLARE @Video1Id UNIQUEIDENTIFIER = '31d048ef-5724-4b11-8d56-9490c6bf8b6d';
DECLARE @Video2Id UNIQUEIDENTIFIER = '2c65a2c8-0f63-41dc-88fd-003a18165d75';
DECLARE @Video3Id UNIQUEIDENTIFIER = '82ea04c1-4a1a-466d-9234-37b58c4f7515';

INSERT INTO dbo.Videos (Id, Title, Description, Url, OrderIndex, DurationSeconds, ThumbnailUrl, CreatedAt)
VALUES
(@Video1Id, N'Anatomiye Giriş', N'İnsan vücudu temel anatomisi ve terminoloji.', N'https://videos.medtrain.local/anatomi-giris.mp4', 1, 900, N'https://images.medtrain.local/anatomi.jpg', SYSUTCDATETIME()),
(@Video2Id, N'Fizyoloji Temelleri', N'Sinir sistemi ve dolaşım sistemi fizyolojisi.', N'https://videos.medtrain.local/fizyoloji-temelleri.mp4', 2, 1100, N'https://images.medtrain.local/fizyoloji.jpg', SYSUTCDATETIME()),
(@Video3Id, N'Klinik Vaka Çalışması', N'Gerçek vaka üzerinden tanı ve tedavi yaklaşımı.', N'https://videos.medtrain.local/klinik-vaka.mp4', 3, 980, N'https://images.medtrain.local/vaka.jpg', SYSUTCDATETIME());
GO

/* ------------------------------------------------------------
   Sorular ve Şıklar
------------------------------------------------------------ */
DECLARE @Q1 UNIQUEIDENTIFIER = '5eb6c510-7a0f-49f2-8c14-7a18c9428f9f';
DECLARE @Q2 UNIQUEIDENTIFIER = 'c0a06a40-9f57-4bb2-8cfe-b4b5d0ef9e20';
DECLARE @Q3 UNIQUEIDENTIFIER = '53b1c3aa-cc35-426c-a61a-58a25231f6eb';
DECLARE @Q4 UNIQUEIDENTIFIER = 'a99b7720-6ff2-4ab1-82a4-34b4951271ce';
DECLARE @Q5 UNIQUEIDENTIFIER = 'bf18c9b2-5d8d-4c15-9352-23736c40fb88';

INSERT INTO dbo.Questions (Id, Body, DisplayOrder, CreatedAt)
VALUES
(@Q1, N'Kalpte kaç odacık bulunur?', 1, SYSUTCDATETIME()),
(@Q2, N'Beyin-omurilik sıvısı hangi yapı tarafından üretilir?', 2, SYSUTCDATETIME()),
(@Q3, N'Solunum sisteminde gaz değişiminin gerçekleştiği yapı aşağıdakilerden hangisidir?', 3, SYSUTCDATETIME()),
(@Q4, N'İnsan vücudundaki en büyük atardamar hangisidir?', 4, SYSUTCDATETIME()),
(@Q5, N'İskelet kaslarının kasılmasında temel enerji molekülü nedir?', 5, SYSUTCDATETIME());
GO

DECLARE @Q1A UNIQUEIDENTIFIER = '4f0f4bb7-93e9-4cbe-bff8-2ef884cd3d3a';
DECLARE @Q1B UNIQUEIDENTIFIER = '6f6f1645-01ad-47c4-8fcc-8dfd06361741';
DECLARE @Q1C UNIQUEIDENTIFIER = '7f509103-1a86-4d3b-bfa9-8e70f21595ac';
DECLARE @Q1D UNIQUEIDENTIFIER = '80b29500-9a6a-43dd-9d23-5f0f2a9e2c96';

DECLARE @Q2A UNIQUEIDENTIFIER = 'bb32cf4c-5a5e-4e43-93e2-3304c3ebf1af';
DECLARE @Q2B UNIQUEIDENTIFIER = '0c0a64b3-2de3-44c6-990d-48be1a50e98e';
DECLARE @Q2C UNIQUEIDENTIFIER = '3de03b87-9c13-4ef2-bd67-4c74bb4fb2c2';
DECLARE @Q2D UNIQUEIDENTIFIER = 'fdc6ff32-1383-49f0-8d2f-4e6480b12152';

DECLARE @Q3A UNIQUEIDENTIFIER = 'c99fdc97-5bee-4070-8d39-9c43ebba9950';
DECLARE @Q3B UNIQUEIDENTIFIER = '6a2a0ec6-2dfd-4a19-8f2f-2b3a19bbf501';
DECLARE @Q3C UNIQUEIDENTIFIER = 'e7f86aa3-91d0-4e5d-b0ec-71d1ed04bca4';
DECLARE @Q3D UNIQUEIDENTIFIER = 'f8a4aa8c-4d42-46e9-9d2b-0f3f161dc1d1';

DECLARE @Q4A UNIQUEIDENTIFIER = '74f69bfc-16d5-43b4-975f-21869d5d34c3';
DECLARE @Q4B UNIQUEIDENTIFIER = '128c9f1c-25f8-4a6d-b1e1-f1d9c406c7a2';
DECLARE @Q4C UNIQUEIDENTIFIER = 'be2d6bbf-85df-4ec9-9bc3-655a26fbfa40';
DECLARE @Q4D UNIQUEIDENTIFIER = 'cbc34aa7-9153-4d66-a014-ecc1d3b1f25f';

DECLARE @Q5A UNIQUEIDENTIFIER = '2c7d8eea-4833-49a7-8084-70c2ae72bdaf';
DECLARE @Q5B UNIQUEIDENTIFIER = 'ae2ffce1-3762-4275-a8c8-31d9a079f608';
DECLARE @Q5C UNIQUEIDENTIFIER = '0f2c6354-f46e-40c5-9d61-2fc4f7390f3d';
DECLARE @Q5D UNIQUEIDENTIFIER = '4d049d31-6c70-4ad5-93fd-3e922ed8a278';

INSERT INTO dbo.Choices (Id, QuestionId, ChoiceLabel, Body, IsCorrect, CreatedAt)
VALUES
(@Q1A, @Q1, 'A', N'İki', 0, SYSUTCDATETIME()),
(@Q1B, @Q1, 'B', N'Üç', 0, SYSUTCDATETIME()),
(@Q1C, @Q1, 'C', N'Dört', 1, SYSUTCDATETIME()),
(@Q1D, @Q1, 'D', N'Beş', 0, SYSUTCDATETIME()),

(@Q2A, @Q2, 'A', N'Pons', 0, SYSUTCDATETIME()),
(@Q2B, @Q2, 'B', N'Serebellum', 0, SYSUTCDATETIME()),
(@Q2C, @Q2, 'C', N'Koroid pleksus', 1, SYSUTCDATETIME()),
(@Q2D, @Q2, 'D', N'Hipotalamus', 0, SYSUTCDATETIME()),

(@Q3A, @Q3, 'A', N'Alveoller', 1, SYSUTCDATETIME()),
(@Q3B, @Q3, 'B', N'Bronşlar', 0, SYSUTCDATETIME()),
(@Q3C, @Q3, 'C', N'Larenks', 0, SYSUTCDATETIME()),
(@Q3D, @Q3, 'D', N'Trakea', 0, SYSUTCDATETIME()),

(@Q4A, @Q4, 'A', N'Pulmoner arter', 0, SYSUTCDATETIME()),
(@Q4B, @Q4, 'B', N'Aorta', 1, SYSUTCDATETIME()),
(@Q4C, @Q4, 'C', N'Karotid arter', 0, SYSUTCDATETIME()),
(@Q4D, @Q4, 'D', N'Femoral arter', 0, SYSUTCDATETIME()),

(@Q5A, @Q5, 'A', N'ADP', 0, SYSUTCDATETIME()),
(@Q5B, @Q5, 'B', N'ATP', 1, SYSUTCDATETIME()),
(@Q5C, @Q5, 'C', N'Laktik Asit', 0, SYSUTCDATETIME()),
(@Q5D, @Q5, 'D', N'Glikojen', 0, SYSUTCDATETIME());
GO

/* ------------------------------------------------------------
   Öğrenci Video İlerlemesi
------------------------------------------------------------ */
INSERT INTO dbo.StudentVideoProgress (UserId, VideoId, CompletedAt)
VALUES
(@Student1Id, @Video1Id, DATEADD(HOUR, -8, SYSUTCDATETIME())),
(@Student1Id, @Video2Id, DATEADD(HOUR, -7, SYSUTCDATETIME()));
GO

/* ------------------------------------------------------------
   Örnek Sınav Denemesi ve Cevapları
------------------------------------------------------------ */
DECLARE @Attempt1 UNIQUEIDENTIFIER = '7500b5ec-8fa8-4dee-8c71-4de94876c9a7';

INSERT INTO dbo.ExamAttempts (Id, UserId, StartedAt, CompletedAt, TotalCorrect, TotalIncorrect)
VALUES
(@Attempt1, @Student1Id, DATEADD(HOUR, -6, SYSUTCDATETIME()), DATEADD(HOUR, -6, SYSUTCDATETIME()), 4, 1);
GO

INSERT INTO dbo.ExamAnswers (AttemptId, QuestionId, ChoiceId, AnsweredAt)
VALUES
(@Attempt1, @Q1, @Q1C, DATEADD(HOUR, -6, SYSUTCDATETIME())),
(@Attempt1, @Q2, @Q2C, DATEADD(HOUR, -6, SYSUTCDATETIME())),
(@Attempt1, @Q3, @Q3A, DATEADD(HOUR, -6, SYSUTCDATETIME())),
(@Attempt1, @Q4, @Q4B, DATEADD(HOUR, -6, SYSUTCDATETIME())),
(@Attempt1, @Q5, @Q5A, DATEADD(HOUR, -6, SYSUTCDATETIME())); -- bilinçli yanlış cevap
GO

/* ------------------------------------------------------------
   Değerlendirme Anketi
------------------------------------------------------------ */
INSERT INTO dbo.Surveys (Id, AttemptId, Liked, ContentClarityRating, InstructorSupportRating, AdditionalFeedback, SubmittedAt)
VALUES
('4f25aeb9-6ba0-4f2a-9e3a-1a3795b94b88', @Attempt1, 1, 5, 4, N'İçerik oldukça faydalıydı, vaka sayısı artırılabilir.', DATEADD(HOUR, -5, SYSUTCDATETIME()));
GO

