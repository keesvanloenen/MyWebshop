IF OBJECT_ID(N'[__EFMigrationsHistory]') IS NULL
BEGIN
    CREATE TABLE [__EFMigrationsHistory] (
        [MigrationId] nvarchar(150) NOT NULL,
        [ProductVersion] nvarchar(32) NOT NULL,
        CONSTRAINT [PK___EFMigrationsHistory] PRIMARY KEY ([MigrationId])
    );
END;
GO

BEGIN TRANSACTION;
CREATE TABLE [People] (
    [Id] int NOT NULL IDENTITY,
    [FirstName] nvarchar(50) NOT NULL,
    [LastName] nvarchar(50) NOT NULL,
    [Discriminator] nvarchar(34) NOT NULL,
    [HireDate] date NULL,
    [EnrollmentDate] date NULL,
    CONSTRAINT [PK_People] PRIMARY KEY ([Id])
);

CREATE TABLE [Departments] (
    [Id] int NOT NULL IDENTITY,
    [Name] nvarchar(50) NOT NULL,
    [Budget] Money NOT NULL,
    [StartDate] date NOT NULL,
    [AdministratorId] int NULL,
    CONSTRAINT [PK_Departments] PRIMARY KEY ([Id]),
    CONSTRAINT [FK_Departments_People_AdministratorId] FOREIGN KEY ([AdministratorId]) REFERENCES [People] ([Id]) ON DELETE SET NULL
);

CREATE TABLE [OfficeAssignments] (
    [InstructorId] int NOT NULL,
    [Location] nvarchar(50) NOT NULL,
    [Timestamp] rowversion NOT NULL,
    CONSTRAINT [PK_OfficeAssignments] PRIMARY KEY ([InstructorId]),
    CONSTRAINT [FK_OfficeAssignments_People_InstructorId] FOREIGN KEY ([InstructorId]) REFERENCES [People] ([Id]) ON DELETE CASCADE
);

CREATE TABLE [Courses] (
    [Id] int NOT NULL,
    [Title] nvarchar(100) NOT NULL,
    [Credits] int NOT NULL,
    [DepartmentId] int NOT NULL,
    CONSTRAINT [PK_Courses] PRIMARY KEY ([Id]),
    CONSTRAINT [FK_Courses_Departments_DepartmentId] FOREIGN KEY ([DepartmentId]) REFERENCES [Departments] ([Id]) ON DELETE NO ACTION
);

CREATE TABLE [CoursesInstructors] (
    [CourseId] int NOT NULL,
    [InstructorId] int NOT NULL,
    CONSTRAINT [PK_CoursesInstructors] PRIMARY KEY ([CourseId], [InstructorId]),
    CONSTRAINT [FK_CoursesInstructors_Courses_CourseId] FOREIGN KEY ([CourseId]) REFERENCES [Courses] ([Id]) ON DELETE CASCADE,
    CONSTRAINT [FK_CoursesInstructors_People_InstructorId] FOREIGN KEY ([InstructorId]) REFERENCES [People] ([Id]) ON DELETE CASCADE
);

CREATE TABLE [CourseStudent] (
    [CoursesId] int NOT NULL,
    [StudentsId] int NOT NULL,
    CONSTRAINT [PK_CourseStudent] PRIMARY KEY ([CoursesId], [StudentsId]),
    CONSTRAINT [FK_CourseStudent_Courses_CoursesId] FOREIGN KEY ([CoursesId]) REFERENCES [Courses] ([Id]) ON DELETE CASCADE,
    CONSTRAINT [FK_CourseStudent_People_StudentsId] FOREIGN KEY ([StudentsId]) REFERENCES [People] ([Id]) ON DELETE CASCADE
);

CREATE TABLE [Enrollments] (
    [CourseId] int NOT NULL,
    [StudentId] int NOT NULL,
    [Grade] decimal(3,2) NULL,
    CONSTRAINT [PK_Enrollments] PRIMARY KEY ([StudentId], [CourseId]),
    CONSTRAINT [FK_Enrollments_Courses_CourseId] FOREIGN KEY ([CourseId]) REFERENCES [Courses] ([Id]) ON DELETE CASCADE,
    CONSTRAINT [FK_Enrollments_People_StudentId] FOREIGN KEY ([StudentId]) REFERENCES [People] ([Id]) ON DELETE CASCADE
);

CREATE TABLE [OnlineCourses] (
    [Id] int NOT NULL,
    [Url] nvarchar(max) NOT NULL,
    CONSTRAINT [PK_OnlineCourses] PRIMARY KEY ([Id]),
    CONSTRAINT [FK_OnlineCourses_Courses_Id] FOREIGN KEY ([Id]) REFERENCES [Courses] ([Id]) ON DELETE CASCADE
);

CREATE TABLE [OnSiteCourses] (
    [Id] int NOT NULL,
    [Location] nvarchar(max) NOT NULL,
    [Time] time NOT NULL,
    CONSTRAINT [PK_OnSiteCourses] PRIMARY KEY ([Id]),
    CONSTRAINT [FK_OnSiteCourses_Courses_Id] FOREIGN KEY ([Id]) REFERENCES [Courses] ([Id]) ON DELETE CASCADE
);

CREATE INDEX [IX_Courses_DepartmentId] ON [Courses] ([DepartmentId]);

CREATE INDEX [IX_CoursesInstructors_InstructorId] ON [CoursesInstructors] ([InstructorId]);

CREATE INDEX [IX_CourseStudent_StudentsId] ON [CourseStudent] ([StudentsId]);

CREATE INDEX [IX_Departments_AdministratorId] ON [Departments] ([AdministratorId]);

CREATE INDEX [IX_Enrollments_CourseId] ON [Enrollments] ([CourseId]);

INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
VALUES (N'20251218110642_InitialCreate', N'10.0.1');

COMMIT;
GO

BEGIN TRANSACTION;

               CREATE PROCEDURE GetDepartmentsWithMostEnrollments
               @NumberOfEnrollments INT
               AS
               BEGIN
                 SELECT d.Id, d.Name, d.StartDate, d.Budget, d.AdministratorId, COUNT(e.CourseId) AS EnrollmentsCount
                 FROM Departments d
                 INNER JOIN Courses c ON d.Id = c.DepartmentId
                 INNER JOIN Enrollments e ON c.Id = e.CourseId
                 GROUP BY d.Id, d.Name, d.StartDate, d.Budget, d.AdministratorId
                 HAVING COUNT(e.CourseId) > @NumberOfEnrollments
                 ORDER BY EnrollmentsCount DESC
               END
            


               CREATE FUNCTION dbo.GetEnrollmentsCountByDepartmentId(@DepartmentId INT)
               RETURNS INT
               AS
               BEGIN
                 DECLARE @EnrollmentsCount INT

                 SELECT @EnrollmentsCount = COUNT(e.CourseId)
                 FROM Departments d
                 INNER JOIN Courses c ON d.Id = c.DepartmentId
                 INNER JOIN Enrollments e ON c.Id = e.CourseId
                 WHERE d.Id = @DepartmentId
                 GROUP BY d.Id

                 RETURN @EnrollmentsCount
               END
            


               CREATE FUNCTION dbo.GetDepartmentsWithMoreThanGivenNumberOfEnrollments(@NumberOfEnrollments INT)
               RETURNS TABLE
               AS
               RETURN
               (
               SELECT d.Id, d.Name, d.Budget, d.StartDate, d.AdministratorId
               FROM Departments d
               INNER JOIN Courses c ON d.Id = c.DepartmentId
               INNER JOIN Enrollments e ON c.Id = e.CourseId
               GROUP BY d.Id, d.Name, d.Budget, d.StartDate, d.AdministratorId
               HAVING COUNT(e.CourseId) > @NumberOfEnrollments
               )
            

INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
VALUES (N'20251218122146_SqlCustomization', N'10.0.1');

COMMIT;
GO

BEGIN TRANSACTION;
IF EXISTS (SELECT * FROM [sys].[identity_columns] WHERE [name] IN (N'Id', N'Discriminator', N'FirstName', N'HireDate', N'LastName') AND [object_id] = OBJECT_ID(N'[People]'))
    SET IDENTITY_INSERT [People] ON;
INSERT INTO [People] ([Id], [Discriminator], [FirstName], [HireDate], [LastName])
VALUES (1, N'Instructor', N'Fadi', '2002-08-06', N'Fakhouri'),
(2, N'Instructor', N'Stacy', '1999-06-01', N'Serrano'),
(3, N'Instructor', N'Jasmine', '1997-10-12', N'Stewart'),
(4, N'Instructor', N'Kristen', '2001-07-23', N'Xu'),
(5, N'Instructor', N'Roger', '2000-12-07', N'Van Houten');
IF EXISTS (SELECT * FROM [sys].[identity_columns] WHERE [name] IN (N'Id', N'Discriminator', N'FirstName', N'HireDate', N'LastName') AND [object_id] = OBJECT_ID(N'[People]'))
    SET IDENTITY_INSERT [People] OFF;

IF EXISTS (SELECT * FROM [sys].[identity_columns] WHERE [name] IN (N'Id', N'Discriminator', N'EnrollmentDate', N'FirstName', N'LastName') AND [object_id] = OBJECT_ID(N'[People]'))
    SET IDENTITY_INSERT [People] ON;
INSERT INTO [People] ([Id], [Discriminator], [EnrollmentDate], [FirstName], [LastName])
VALUES (6, N'Student', '2005-09-01', N'Gytis', N'Barzdukas'),
(7, N'Student', '2001-09-01', N'Peggy', N'Justice'),
(8, N'Student', '2002-09-01', N'Yan', N'Li'),
(9, N'Student', '2003-09-01', N'Laura', N'Norman'),
(10, N'Student', '2005-09-01', N'Nino', N'Olivotto'),
(11, N'Student', '2005-09-01', N'Wayne', N'Tang'),
(12, N'Student', '2002-09-01', N'Meredith', N'Alonso'),
(13, N'Student', '2004-09-01', N'Sophia', N'Lopez'),
(14, N'Student', '2000-09-01', N'Meredith', N'Browning'),
(15, N'Student', '2003-09-01', N'Arturo', N'Anand'),
(16, N'Student', '2000-09-01', N'Alexandra', N'Walker'),
(17, N'Student', '2004-09-01', N'Carson', N'Powell'),
(18, N'Student', '2001-09-01', N'Damien', N'Jai'),
(19, N'Student', '2005-09-01', N'Robyn', N'Carlson'),
(20, N'Student', '2001-09-01', N'Carson', N'Bryant'),
(21, N'Student', '2004-09-01', N'Robyn', N'Suarez'),
(22, N'Student', '2004-09-01', N'Roger', N'Holt'),
(23, N'Student', '2005-09-01', N'Carson', N'Alexander'),
(24, N'Student', '2001-09-01', N'Isaiah', N'Morgan'),
(25, N'Student', '2005-09-01', N'Randall', N'Martin'),
(26, N'Student', '2002-09-01', N'Cody', N'Rogers'),
(27, N'Student', '2001-09-01', N'Anthony', N'White'),
(28, N'Student', '2004-09-01', N'Rachel', N'Griffin'),
(29, N'Student', '2003-09-01', N'Alicia', N'Shan'),
(30, N'Student', '2003-01-30', N'Erica', N'Gao');
IF EXISTS (SELECT * FROM [sys].[identity_columns] WHERE [name] IN (N'Id', N'Discriminator', N'EnrollmentDate', N'FirstName', N'LastName') AND [object_id] = OBJECT_ID(N'[People]'))
    SET IDENTITY_INSERT [People] OFF;

IF EXISTS (SELECT * FROM [sys].[identity_columns] WHERE [name] IN (N'Id', N'Discriminator', N'FirstName', N'HireDate', N'LastName') AND [object_id] = OBJECT_ID(N'[People]'))
    SET IDENTITY_INSERT [People] ON;
INSERT INTO [People] ([Id], [Discriminator], [FirstName], [HireDate], [LastName])
VALUES (31, N'DepartmentAdministrator', N'Kim', '1995-03-11', N'Abercrombie'),
(32, N'DepartmentAdministrator', N'Roger', '1998-07-01', N'Harui'),
(33, N'DepartmentAdministrator', N'Roger', '2004-02-12', N'Zheng'),
(34, N'DepartmentAdministrator', N'Candace', '2001-01-15', N'Kapoor');
IF EXISTS (SELECT * FROM [sys].[identity_columns] WHERE [name] IN (N'Id', N'Discriminator', N'FirstName', N'HireDate', N'LastName') AND [object_id] = OBJECT_ID(N'[People]'))
    SET IDENTITY_INSERT [People] OFF;

IF EXISTS (SELECT * FROM [sys].[identity_columns] WHERE [name] IN (N'Id', N'AdministratorId', N'Budget', N'Name', N'StartDate') AND [object_id] = OBJECT_ID(N'[Departments]'))
    SET IDENTITY_INSERT [Departments] ON;
INSERT INTO [Departments] ([Id], [AdministratorId], [Budget], [Name], [StartDate])
VALUES (1, 31, 350000.0, N'Engineering', '2007-01-09'),
(2, 34, 120000.0, N'English', '2007-01-09'),
(3, 33, 200000.0, N'Economics', '2007-01-09'),
(4, 32, 250000.0, N'Mathematics', '2007-01-09');
IF EXISTS (SELECT * FROM [sys].[identity_columns] WHERE [name] IN (N'Id', N'AdministratorId', N'Budget', N'Name', N'StartDate') AND [object_id] = OBJECT_ID(N'[Departments]'))
    SET IDENTITY_INSERT [Departments] OFF;

IF EXISTS (SELECT * FROM [sys].[identity_columns] WHERE [name] IN (N'InstructorId', N'Location') AND [object_id] = OBJECT_ID(N'[OfficeAssignments]'))
    SET IDENTITY_INSERT [OfficeAssignments] ON;
INSERT INTO [OfficeAssignments] ([InstructorId], [Location])
VALUES (1, N'29 Adams'),
(2, N'271 Williams'),
(3, N'131 Smith'),
(4, N'203 Williams'),
(5, N'213 Smith'),
(31, N'17 Smith'),
(32, N'37 Williams'),
(33, N'143 Smith'),
(34, N'57 Adams');
IF EXISTS (SELECT * FROM [sys].[identity_columns] WHERE [name] IN (N'InstructorId', N'Location') AND [object_id] = OBJECT_ID(N'[OfficeAssignments]'))
    SET IDENTITY_INSERT [OfficeAssignments] OFF;

IF EXISTS (SELECT * FROM [sys].[identity_columns] WHERE [name] IN (N'Id', N'Credits', N'DepartmentId', N'Title') AND [object_id] = OBJECT_ID(N'[Courses]'))
    SET IDENTITY_INSERT [Courses] ON;
INSERT INTO [Courses] ([Id], [Credits], [DepartmentId], [Title])
VALUES (1045, 4, 4, N'Calculus'),
(1050, 4, 1, N'Chemistry'),
(1061, 4, 1, N'Physics'),
(2021, 3, 2, N'Composition'),
(2030, 2, 2, N'Poetry'),
(2042, 4, 2, N'Literature'),
(3141, 4, 4, N'Trigonometry'),
(4022, 3, 3, N'Microeconomics'),
(4041, 3, 3, N'Macroeconomics'),
(4061, 2, 3, N'Quantitative');
IF EXISTS (SELECT * FROM [sys].[identity_columns] WHERE [name] IN (N'Id', N'Credits', N'DepartmentId', N'Title') AND [object_id] = OBJECT_ID(N'[Courses]'))
    SET IDENTITY_INSERT [Courses] OFF;

IF EXISTS (SELECT * FROM [sys].[identity_columns] WHERE [name] IN (N'CourseId', N'StudentId', N'Grade') AND [object_id] = OBJECT_ID(N'[Enrollments]'))
    SET IDENTITY_INSERT [Enrollments] ON;
INSERT INTO [Enrollments] ([CourseId], [StudentId], [Grade])
VALUES (2021, 6, 4.0),
(2030, 6, 3.5),
(2021, 7, 3.0),
(2030, 7, 4.0),
(2021, 8, 2.5),
(2042, 8, 3.5),
(2021, 9, 3.5),
(2042, 9, 4.0),
(2021, 10, 3.0),
(2042, 10, 3.0),
(4041, 11, 3.5),
(4041, 12, NULL),
(4041, 13, 2.5),
(4041, 14, NULL),
(4061, 14, NULL),
(4022, 15, 4.0),
(4061, 15, 4.0),
(4022, 16, 3.0),
(4041, 16, 3.0),
(4022, 17, 2.5),
(4022, 18, 2.0),
(4022, 19, NULL),
(4022, 20, 3.5),
(4061, 21, 4.0),
(4061, 22, 2.0),
(4022, 23, 3.0),
(4041, 23, 3.5),
(4061, 23, 2.5),
(1045, 24, 1.5),
(4022, 24, 3.0),
(1061, 25, 4.0),
(1050, 26, 3.5),
(1061, 26, 3.0),
(1045, 27, 2.5),
(1050, 27, 3.5),
(1061, 28, 4.0),
(1050, 29, 3.5),
(1061, 29, 4.0);
IF EXISTS (SELECT * FROM [sys].[identity_columns] WHERE [name] IN (N'CourseId', N'StudentId', N'Grade') AND [object_id] = OBJECT_ID(N'[Enrollments]'))
    SET IDENTITY_INSERT [Enrollments] OFF;

IF EXISTS (SELECT * FROM [sys].[identity_columns] WHERE [name] IN (N'Id', N'Location', N'Time') AND [object_id] = OBJECT_ID(N'[OnSiteCourses]'))
    SET IDENTITY_INSERT [OnSiteCourses] ON;
INSERT INTO [OnSiteCourses] ([Id], [Location], [Time])
VALUES (1045, N'121 Smith', '15:30:00'),
(1050, N'123 Smith', '11:30:00'),
(1061, N'234 Smith', '13:15:00'),
(2042, N'225 Adams', '11:00:00'),
(4022, N'23 Williams', '09:00:00'),
(4061, N'22 Williams', '11:15:00');
IF EXISTS (SELECT * FROM [sys].[identity_columns] WHERE [name] IN (N'Id', N'Location', N'Time') AND [object_id] = OBJECT_ID(N'[OnSiteCourses]'))
    SET IDENTITY_INSERT [OnSiteCourses] OFF;

IF EXISTS (SELECT * FROM [sys].[identity_columns] WHERE [name] IN (N'Id', N'Url') AND [object_id] = OBJECT_ID(N'[OnlineCourses]'))
    SET IDENTITY_INSERT [OnlineCourses] ON;
INSERT INTO [OnlineCourses] ([Id], [Url])
VALUES (2021, N'http://www.fineartschool.net/Composition'),
(2030, N'http://www.fineartschool.net/Poetry'),
(3141, N'http://www.fineartschool.net/Trigonometry'),
(4041, N'http://www.fineartschool.net/Macroeconomics');
IF EXISTS (SELECT * FROM [sys].[identity_columns] WHERE [name] IN (N'Id', N'Url') AND [object_id] = OBJECT_ID(N'[OnlineCourses]'))
    SET IDENTITY_INSERT [OnlineCourses] OFF;

IF EXISTS (SELECT * FROM [sys].[identity_columns] WHERE [name] IN (N'CourseId', N'InstructorId') AND [object_id] = OBJECT_ID(N'[CoursesInstructors]'))
    SET IDENTITY_INSERT [CoursesInstructors] ON;
INSERT INTO [CoursesInstructors] ([CourseId], [InstructorId])
VALUES (2030, 1),
(2021, 2),
(1061, 3),
(4041, 4),
(4061, 5),
(1050, 31),
(1045, 32),
(4022, 33),
(2042, 34);
IF EXISTS (SELECT * FROM [sys].[identity_columns] WHERE [name] IN (N'CourseId', N'InstructorId') AND [object_id] = OBJECT_ID(N'[CoursesInstructors]'))
    SET IDENTITY_INSERT [CoursesInstructors] OFF;

INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
VALUES (N'20251218141329_SeedData', N'10.0.1');

COMMIT;
GO

