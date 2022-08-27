/****** CREATE DATABASE ******/
IF NOT EXISTS(SELECT * FROM sys.databases WHERE name = 'Library')
            BEGIN
                CREATE DATABASE [Library]
            END
GO
/****** CREATE TABLES ******/		
USE [Library]
            IF NOT EXISTS (SELECT * FROM sysobjects WHERE name='BookCategory' and xtype='U')
            BEGIN
                CREATE TABLE [dbo].[BookCategory]
                (
                    [Id] UNIQUEIDENTIFIER DEFAULT (newsequentialid()) NOT NULL PRIMARY KEY,
                    [Name] NVARCHAR (150) NOT NULL,
                );
            END

            IF NOT EXISTS (SELECT * FROM sysobjects WHERE name='Course' and xtype='U')
            BEGIN        
                CREATE TABLE [dbo].[Course]
                (
                    [Id] UNIQUEIDENTIFIER DEFAULT (newsequentialid()) NOT NULL PRIMARY KEY,
                    [Name] NVARCHAR (150) NOT NULL,
                );
            END;

            IF NOT EXISTS (SELECT * FROM sysobjects WHERE name='Student' and xtype='U')
            BEGIN
                CREATE TABLE [dbo].[Student]
                (	
                    [Id] UNIQUEIDENTIFIER DEFAULT (newsequentialid()) NOT NULL PRIMARY KEY,
                    [Name] NVARCHAR (150) NOT NULL,
                    [Email] NVARCHAR (150) NOT NULL,

                    [CourseId] UNIQUEIDENTIFIER NOT NULL,
                    CONSTRAINT [FK_Student_Couse] FOREIGN KEY ([CourseId]) REFERENCES [Course]([Id]),
                );
            END;

            IF NOT EXISTS (SELECT * FROM sysobjects WHERE name='CourseBooksCategories' and xtype='U')
            BEGIN
                CREATE TABLE [dbo].[CourseBooksCategories]
                (
                    [CourseId] UNIQUEIDENTIFIER NOT NULL,
                    [CategoryId] UNIQUEIDENTIFIER NOT NULL,

                    CONSTRAINT [FK_CourseBooksCategories_Couse] FOREIGN KEY ([CourseId]) REFERENCES [Course]([Id]),
                    CONSTRAINT [FK_CourseBooksCategories_Category] FOREIGN KEY ([CategoryId]) REFERENCES [BookCategory]([Id]),
                    CONSTRAINT [PK_CourseBooksCategories] PRIMARY KEY ([CourseId], [CategoryId])
                );
            END;
            
            IF NOT EXISTS (SELECT * FROM sysobjects WHERE name='Book' and xtype='U')
            BEGIN
                CREATE TABLE [dbo].[Book]
                (
                    [Id] UNIQUEIDENTIFIER DEFAULT (newsequentialid()) NOT NULL PRIMARY KEY,
                    [Title] NVARCHAR (150) NOT NULL,
                    [Author] NVARCHAR (100) NOT NULL,
                    [Pages] INT NULL,
                    [Publisher] NVARCHAR (300) NOT NULL,
                    [BookCategoryId] UNIQUEIDENTIFIER NOT NULL,
                    [LentToStudentId] UNIQUEIDENTIFIER NULL

                    CONSTRAINT [FK_Book_Category] FOREIGN KEY ([BookCategoryId]) REFERENCES [BookCategory]([Id]),
                    CONSTRAINT [FK_Book_Student] FOREIGN KEY ([LentToStudentId]) REFERENCES [Student]([Id]),
                );
            END;
/****** INSERT DATA ******/
GO
USE [Library]
			IF NOT EXISTS (SELECT * FROM Book)
						BEGIN
							INSERT [dbo].[Book] ([Id], [Title], [Author], [Pages], [Publisher], [BookCategoryId], [LentToStudentId]) VALUES (N'4c008a16-6725-42c6-83fc-289a1f230b38', N'O edifício até sua cobertura', N'Hélio Alves de Azeredo', 193, N'Blucher', N'20efaba1-64bd-4b7f-82f4-c1d05550e305', NULL)
							INSERT [dbo].[Book] ([Id], [Title], [Author], [Pages], [Publisher], [BookCategoryId], [LentToStudentId]) VALUES (N'd77c87cd-97a9-41ed-b944-69ef10d8eb94', N'Concreto armado: eu te amo', N'Manoel Henrique Campos', 652, N'Blucher', N'20efaba1-64bd-4b7f-82f4-c1d05550e305', NULL)
							INSERT [dbo].[Book] ([Id], [Title], [Author], [Pages], [Publisher], [BookCategoryId], [LentToStudentId]) VALUES (N'bc118136-1a03-408c-a048-897439398bb2', N'Pragmatic Programmer', N'Andrew Hunt', 352, N'Addison-Wesley Professional', N'6031d727-7fb5-47fa-a6d5-6e5cfeceff21', NULL)
							INSERT [dbo].[Book] ([Id], [Title], [Author], [Pages], [Publisher], [BookCategoryId], [LentToStudentId]) VALUES (N'344f6f55-57c8-415d-896f-c0d2293742b7', N'Clean Code', N'Uncle Bob', 429, N'Atlas', N'6031d727-7fb5-47fa-a6d5-6e5cfeceff21', NULL)
							INSERT [dbo].[BookCategory] ([Id], [Name]) VALUES (N'6031d727-7fb5-47fa-a6d5-6e5cfeceff21', N'TI')
							INSERT [dbo].[BookCategory] ([Id], [Name]) VALUES (N'20efaba1-64bd-4b7f-82f4-c1d05550e305', N'Engenharia Civil')
						END;
			IF NOT EXISTS (SELECT * FROM Course)
						BEGIN
							INSERT [dbo].[Course] ([Id], [Name]) VALUES (N'a2c6f987-d83f-4fb3-9982-68553965b421', N'Sistemas de Infomração')
							INSERT [dbo].[Course] ([Id], [Name]) VALUES (N'7ecb8a32-4452-43a0-b78a-ca1552303304', N'Engenharia Civil')
							INSERT [dbo].[CourseBooksCategories] ([CourseId], [CategoryId]) VALUES (N'a2c6f987-d83f-4fb3-9982-68553965b421', N'6031d727-7fb5-47fa-a6d5-6e5cfeceff21')
							INSERT [dbo].[CourseBooksCategories] ([CourseId], [CategoryId]) VALUES (N'7ecb8a32-4452-43a0-b78a-ca1552303304', N'20efaba1-64bd-4b7f-82f4-c1d05550e305')
						END;
			IF NOT EXISTS (SELECT * FROM Student)
						BEGIN
							INSERT [dbo].[Student] ([Id], [Name], [Email], [CourseId]) VALUES (N'1673a9fd-191a-479c-a41f-3dc5611aa98e', N'Student One', N'student_one@domain.com', N'a2c6f987-d83f-4fb3-9982-68553965b421')
							INSERT [dbo].[Student] ([Id], [Name], [Email], [CourseId]) VALUES (N'0c4833c4-5138-48d7-80a7-60abe82a5c6c', N'Student Two', N'student_two@domain.com', N'7ecb8a32-4452-43a0-b78a-ca1552303304')
							INSERT [dbo].[Student] ([Id], [Name], [Email], [CourseId]) VALUES (N'6a2ecda5-e160-4a42-898b-8f4a73989688', N'Student 3', N'student_three@domain.com', N'7ecb8a32-4452-43a0-b78a-ca1552303304')
							INSERT [dbo].[Student] ([Id], [Name], [Email], [CourseId]) VALUES (N'5f35054b-7a3a-4dce-8355-cf81b8b223d1', N'Student 4', N'student_four@gmail.com', N'a2c6f987-d83f-4fb3-9982-68553965b421')
						END;