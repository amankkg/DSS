
-- --------------------------------------------------
-- Entity Designer DDL Script for SQL Server 2005, 2008, and Azure
-- --------------------------------------------------
-- Date Created: 10/02/2013 21:52:53
-- Generated from EDMX file: C:\Users\Ownmenot\Documents\GitHub\DSS\DecisionSupportSystem\DecisionSupportSystem\DBModel.edmx
-- --------------------------------------------------

SET QUOTED_IDENTIFIER OFF;
GO
USE [DSSDB];
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
    [Id] int IDENTITY(1,1) NOT NULL,
    [Name] nvarchar(max)  NOT NULL
);
GO

-- Creating table 'Combinations'
CREATE TABLE [dbo].[Combinations] (
    [Id] int IDENTITY(1,1) NOT NULL,
    [ActionId] int  NOT NULL,
    [EventId] int  NOT NULL,
    [TaskId] int  NOT NULL
);
GO

-- Creating table 'Events'
CREATE TABLE [dbo].[Events] (
    [Id] int IDENTITY(1,1) NOT NULL,
    [Name] nvarchar(max)  NOT NULL,
    [Probability] decimal(18,0)  NOT NULL
);
GO

-- Creating table 'Tasks'
CREATE TABLE [dbo].[Tasks] (
    [Id] int IDENTITY(1,1) NOT NULL,
    [Date] datetime  NOT NULL,
    [Comment] nvarchar(max)  NOT NULL
);
GO

-- Creating table 'Constants'
CREATE TABLE [dbo].[Constants] (
    [Id] int IDENTITY(1,1) NOT NULL,
    [Name] nvarchar(max)  NOT NULL,
    [Value] decimal(18,0)  NOT NULL,
    [TaskId] int  NOT NULL
);
GO

-- Creating table 'Parameters'
CREATE TABLE [dbo].[Parameters] (
    [Id] int IDENTITY(1,1) NOT NULL,
    [Name] nvarchar(max)  NOT NULL
);
GO

-- Creating table 'ParameterValues'
CREATE TABLE [dbo].[ParameterValues] (
    [Id] int IDENTITY(1,1) NOT NULL,
    [Value] decimal(18,0)  NOT NULL,
    [ParameterId] int  NOT NULL,
    [CombinationId] int  NOT NULL
);
GO

-- --------------------------------------------------
-- Creating all PRIMARY KEY constraints
-- --------------------------------------------------

-- Creating primary key on [Id] in table 'Actions'
ALTER TABLE [dbo].[Actions]
ADD CONSTRAINT [PK_Actions]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating primary key on [Id] in table 'Combinations'
ALTER TABLE [dbo].[Combinations]
ADD CONSTRAINT [PK_Combinations]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating primary key on [Id] in table 'Events'
ALTER TABLE [dbo].[Events]
ADD CONSTRAINT [PK_Events]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating primary key on [Id] in table 'Tasks'
ALTER TABLE [dbo].[Tasks]
ADD CONSTRAINT [PK_Tasks]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating primary key on [Id] in table 'Constants'
ALTER TABLE [dbo].[Constants]
ADD CONSTRAINT [PK_Constants]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating primary key on [Id] in table 'Parameters'
ALTER TABLE [dbo].[Parameters]
ADD CONSTRAINT [PK_Parameters]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating primary key on [Id] in table 'ParameterValues'
ALTER TABLE [dbo].[ParameterValues]
ADD CONSTRAINT [PK_ParameterValues]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- --------------------------------------------------
-- Creating all FOREIGN KEY constraints
-- --------------------------------------------------

-- Creating foreign key on [ActionId] in table 'Combinations'
ALTER TABLE [dbo].[Combinations]
ADD CONSTRAINT [FK_ActionCombination]
    FOREIGN KEY ([ActionId])
    REFERENCES [dbo].[Actions]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;

-- Creating non-clustered index for FOREIGN KEY 'FK_ActionCombination'
CREATE INDEX [IX_FK_ActionCombination]
ON [dbo].[Combinations]
    ([ActionId]);
GO

-- Creating foreign key on [EventId] in table 'Combinations'
ALTER TABLE [dbo].[Combinations]
ADD CONSTRAINT [FK_EventCombination]
    FOREIGN KEY ([EventId])
    REFERENCES [dbo].[Events]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;

-- Creating non-clustered index for FOREIGN KEY 'FK_EventCombination'
CREATE INDEX [IX_FK_EventCombination]
ON [dbo].[Combinations]
    ([EventId]);
GO

-- Creating foreign key on [TaskId] in table 'Combinations'
ALTER TABLE [dbo].[Combinations]
ADD CONSTRAINT [FK_TaskCombination]
    FOREIGN KEY ([TaskId])
    REFERENCES [dbo].[Tasks]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;

-- Creating non-clustered index for FOREIGN KEY 'FK_TaskCombination'
CREATE INDEX [IX_FK_TaskCombination]
ON [dbo].[Combinations]
    ([TaskId]);
GO

-- Creating foreign key on [TaskId] in table 'Constants'
ALTER TABLE [dbo].[Constants]
ADD CONSTRAINT [FK_ConstantTask]
    FOREIGN KEY ([TaskId])
    REFERENCES [dbo].[Tasks]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;

-- Creating non-clustered index for FOREIGN KEY 'FK_ConstantTask'
CREATE INDEX [IX_FK_ConstantTask]
ON [dbo].[Constants]
    ([TaskId]);
GO

-- Creating foreign key on [ParameterId] in table 'ParameterValues'
ALTER TABLE [dbo].[ParameterValues]
ADD CONSTRAINT [FK_ParameterParameterValue]
    FOREIGN KEY ([ParameterId])
    REFERENCES [dbo].[Parameters]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;

-- Creating non-clustered index for FOREIGN KEY 'FK_ParameterParameterValue'
CREATE INDEX [IX_FK_ParameterParameterValue]
ON [dbo].[ParameterValues]
    ([ParameterId]);
GO

-- Creating foreign key on [CombinationId] in table 'ParameterValues'
ALTER TABLE [dbo].[ParameterValues]
ADD CONSTRAINT [FK_CombinationParameterValue]
    FOREIGN KEY ([CombinationId])
    REFERENCES [dbo].[Combinations]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;

-- Creating non-clustered index for FOREIGN KEY 'FK_CombinationParameterValue'
CREATE INDEX [IX_FK_CombinationParameterValue]
ON [dbo].[ParameterValues]
    ([CombinationId]);
GO

-- --------------------------------------------------
-- Script has ended
-- --------------------------------------------------