
-- --------------------------------------------------
-- Entity Designer DDL Script for SQL Server 2005, 2008, and Azure
-- --------------------------------------------------
-- Date Created: 10/23/2013 14:03:35
-- Generated from EDMX file: D:\Reposgit\DSS\DecisionSupportSystem\DecisionSupportSystem\DSSModelMain.edmx
-- --------------------------------------------------

SET QUOTED_IDENTIFIER OFF;
GO
USE [DssDb];
GO
IF SCHEMA_ID(N'dbo') IS NULL EXECUTE(N'CREATE SCHEMA [dbo]');
GO

-- --------------------------------------------------
-- Dropping existing FOREIGN KEY constraints
-- --------------------------------------------------


-- --------------------------------------------------
-- Dropping existing tables
-- --------------------------------------------------


-- --------------------------------------------------
-- Creating all tables
-- --------------------------------------------------

-- Creating table 'Actions'
CREATE TABLE [dbo].[Actions] (
    [Id] int IDENTITY(1,1) NOT NULL PRIMARY KEY,
    [Name] nvarchar(max)  NULL,
    [Emv] decimal(18,3)  NULL,
    [Eol] decimal(18,3)  NULL,
	[ExtendableActionId] int NULL REFERENCES [dbo].[Actions]([Id]),
	[SavingId] uniqueidentifier NULL,
);
GO

-- Creating table 'Events'
CREATE TABLE [dbo].[Events] (
    [Id] int IDENTITY(1,1) NOT NULL  PRIMARY KEY,
    [Name] nvarchar(max)  NOT NULL,
    [Probability] decimal(18,3) NULL,
	[SavingId] uniqueidentifier NULL,
);
GO

-- Creating table 'Tasks'
CREATE TABLE [dbo].[Tasks] (
    [Id] int IDENTITY(1,1) NOT NULL  PRIMARY KEY,
    [Date] datetime  NOT NULL,
    [Comment] nvarchar(max) NULL,
	[MaxEmv] decimal(18,3) NULL,
	[MinEol] decimal(18,3) NULL,
    [Recommendation] nvarchar(max)  NULL,
	[TaskUniq] nvarchar(100) NOT NULL,
	[SavingId] uniqueidentifier NULL,
	[Deleted] int NOT NULL Default(0),
	[TreeDiagramm] nvarchar(max) NULL,
);

GO
-- Creating table 'Combinations'
CREATE TABLE [dbo].[Combinations] (
    [Id] int IDENTITY(1,1) NOT NULL  PRIMARY KEY,
    [ActionId] int NULL REFERENCES [dbo].[Actions]([Id]),
    [EventId] int NULL REFERENCES [dbo].[Events]([Id]),
    [TaskId] int NULL REFERENCES [dbo].[Tasks]([Id]),
    [Cp] decimal(18,3)  NULL,
    [Wp] decimal(18,3)  NULL,
    [Col] decimal(18,3)  NULL,
    [Wol] decimal(18,3)  NULL,
	[SavingId] uniqueidentifier NULL,
);
GO

-- Creating table 'CombinParamNames'
CREATE TABLE [dbo].[CombinParamNames] (
    [Id] int IDENTITY(1,1) NOT NULL  PRIMARY KEY,
    [Name] nvarchar(100)  NOT NULL
);
GO

-- Creating table 'CombinParams'
CREATE TABLE [dbo].[CombinParams] (
    [Id] int IDENTITY(1,1) NOT NULL  PRIMARY KEY,
    [Value] decimal(18,3)  NOT NULL,
    [CombinationId] int  NULL REFERENCES [dbo].[Combinations]([Id]),
    [NameId] int NULL REFERENCES [dbo].[CombinParamNames]([Id])
);
GO

-- Creating table 'ActionParamNames'
CREATE TABLE [dbo].[ActionParamNames] (
    [Id] int IDENTITY(1,1) NOT NULL PRIMARY KEY,
    [Name] nvarchar(max)  NULL
);
GO

-- Creating table 'ActionParams'
CREATE TABLE [dbo].[ActionParams] (
    [Id] int IDENTITY(1,1) NOT NULL PRIMARY KEY,
    [Value] decimal(18,3)  NOT NULL,
    [ActionId] int  NULL REFERENCES [dbo].[Actions]([Id]),
    [NameId] int NULL REFERENCES [dbo].[ActionParamNames]([Id])
);
GO

-- Creating table 'EventParamNames'
CREATE TABLE [dbo].[EventParamNames] (
    [Id] int IDENTITY(1,1) NOT NULL PRIMARY KEY,
    [Name] nvarchar(100)  NULL
);
GO

-- Creating table 'EventParams'
CREATE TABLE [dbo].[EventParams] (
    [Id] int IDENTITY(1,1) NOT NULL PRIMARY KEY,
    [Value] decimal(18,3)  NOT NULL,
    [EventId] int  NULL REFERENCES [dbo].[Events]([Id]),
    [NameId] int NULL REFERENCES [dbo].[EventParamNames]([Id])
);
GO

-- Creating table 'TaskParamNames'
CREATE TABLE [dbo].[TaskParamNames] (
    [Id] int IDENTITY(1,1) NOT NULL PRIMARY KEY,
    [Name] nvarchar(100)  NULL
);
GO

-- Creating table 'TaskParams'
CREATE TABLE [dbo].[TaskParams] (
    [Id] int IDENTITY(1,1) NOT NULL PRIMARY KEY,
    [Value] decimal(18,3)  NOT NULL,
    [TaskId] int  NULL REFERENCES [dbo].[Tasks]([Id]),
	[NameId] int  NULL REFERENCES [dbo].[TaskParamNames]([Id]),

);
GO
