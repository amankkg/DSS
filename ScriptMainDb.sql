
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
    [Id] bigint IDENTITY(1,1) NOT NULL PRIMARY KEY,
    [Name] nvarchar(max)  NULL,
    [Emv] float  NULL,
    [Eol] float  NULL,
	[SavingId] uniqueidentifier NULL,
);
GO

-- Creating table 'Events'
CREATE TABLE [dbo].[Events] (
    [Id] bigint IDENTITY(1,1) NOT NULL  PRIMARY KEY,
    [Name] nvarchar(max)  NOT NULL,
    [Probability] float NULL,
	[SavingId] uniqueidentifier NULL,
);
GO

-- Creating table 'Tasks'
CREATE TABLE [dbo].[Tasks] (
    [Id] bigint IDENTITY(1,1) NOT NULL  PRIMARY KEY,
    [Date] datetime  NOT NULL,
    [Comment] nvarchar(max) NULL,
	[MaxEmv] float NULL,
	[MinEol] float NULL,
    [Recommendation] nvarchar(max)  NULL,
	[TaskUniq] nvarchar(100) NOT NULL,
	[SavingId] uniqueidentifier NULL,
	[Deleted] int NOT NULL Default(0),
	[TreeDiagramm] nvarchar(max) NULL,
);

GO
-- Creating table 'Combinations'
CREATE TABLE [dbo].[Combinations] (
    [Id] bigint IDENTITY(1,1) NOT NULL  PRIMARY KEY,
    [ActionId] bigint NULL REFERENCES [dbo].[Actions]([Id]),
    [EventId] bigint NULL REFERENCES [dbo].[Events]([Id]),
    [TaskId] bigint NULL REFERENCES [dbo].[Tasks]([Id]),
    [Cp] float  NULL,
    [Wp] float  NULL,
    [Col] float  NULL,
    [Wol] float  NULL,
	[SavingId] uniqueidentifier NULL,
);
GO

-- Creating table 'CombinParamNames'
CREATE TABLE [dbo].[CombinParamNames] (
    [Id] bigint IDENTITY(1,1) NOT NULL  PRIMARY KEY,
    [Name] nvarchar(100)  NOT NULL
);
GO

-- Creating table 'CombinParams'
CREATE TABLE [dbo].[CombinParams] (
    [Id] bigint IDENTITY(1,1) NOT NULL  PRIMARY KEY,
    [Value] float  NOT NULL,
    [CombinationId] bigint  NULL REFERENCES [dbo].[Combinations]([Id]),
    [NameId] bigint NULL REFERENCES [dbo].[CombinParamNames]([Id])
);
GO

-- Creating table 'ActionParamNames'
CREATE TABLE [dbo].[ActionParamNames] (
    [Id] bigint IDENTITY(1,1) NOT NULL PRIMARY KEY,
    [Name] nvarchar(max)  NULL
);
GO

-- Creating table 'ActionParams'
CREATE TABLE [dbo].[ActionParams] (
    [Id] bigint IDENTITY(1,1) NOT NULL PRIMARY KEY,
    [Value] float  NOT NULL,
    [ActionId] bigint  NULL REFERENCES [dbo].[Actions]([Id]),
    [NameId] bigint NULL REFERENCES [dbo].[ActionParamNames]([Id])
);
GO

-- Creating table 'EventParamNames'
CREATE TABLE [dbo].[EventParamNames] (
    [Id] bigint IDENTITY(1,1) NOT NULL PRIMARY KEY,
    [Name] nvarchar(100)  NULL
);
GO

-- Creating table 'EventParams'
CREATE TABLE [dbo].[EventParams] (
    [Id] bigint IDENTITY(1,1) NOT NULL PRIMARY KEY,
    [Value] float  NOT NULL,
    [EventId] bigint  NULL REFERENCES [dbo].[Events]([Id]),
    [NameId] bigint NULL REFERENCES [dbo].[EventParamNames]([Id])
);
GO

-- Creating table 'TaskParamNames'
CREATE TABLE [dbo].[TaskParamNames] (
    [Id] bigint IDENTITY(1,1) NOT NULL PRIMARY KEY,
    [Name] nvarchar(100)  NULL
);
GO

-- Creating table 'TaskParams'
CREATE TABLE [dbo].[TaskParams] (
    [Id] bigint IDENTITY(1,1) NOT NULL PRIMARY KEY,
    [Value] float  NOT NULL,
    [TaskId] bigint  NULL REFERENCES [dbo].[Tasks]([Id]),
	[NameId] bigint  NULL REFERENCES [dbo].[TaskParamNames]([Id]),

);
GO
