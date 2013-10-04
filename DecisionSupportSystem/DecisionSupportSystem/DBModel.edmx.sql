
-- --------------------------------------------------
-- Entity Designer DDL Script for SQL Server 2005, 2008, and Azure
-- --------------------------------------------------
-- Date Created: 10/04/2013 19:58:27
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

IF OBJECT_ID(N'[dbo].[FK_ActionCombination]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[Combinations] DROP CONSTRAINT [FK_ActionCombination];
GO
IF OBJECT_ID(N'[dbo].[FK_EventCombination]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[Combinations] DROP CONSTRAINT [FK_EventCombination];
GO
IF OBJECT_ID(N'[dbo].[FK_TaskCombination]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[Combinations] DROP CONSTRAINT [FK_TaskCombination];
GO
IF OBJECT_ID(N'[dbo].[FK_ConstantTask]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[Constants] DROP CONSTRAINT [FK_ConstantTask];
GO
IF OBJECT_ID(N'[dbo].[FK_ParameterParameterValue]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[ParameterValues] DROP CONSTRAINT [FK_ParameterParameterValue];
GO
IF OBJECT_ID(N'[dbo].[FK_CombinationParameterValue]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[ParameterValues] DROP CONSTRAINT [FK_CombinationParameterValue];
GO
IF OBJECT_ID(N'[dbo].[FK_CombinationConditionalProfit]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[Combinations] DROP CONSTRAINT [FK_CombinationConditionalProfit];
GO
IF OBJECT_ID(N'[dbo].[FK_ActionExpectedMonetaryValue]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[Actions] DROP CONSTRAINT [FK_ActionExpectedMonetaryValue];
GO

-- --------------------------------------------------
-- Dropping existing tables
-- --------------------------------------------------

IF OBJECT_ID(N'[dbo].[Actions]', 'U') IS NOT NULL
    DROP TABLE [dbo].[Actions];
GO
IF OBJECT_ID(N'[dbo].[Combinations]', 'U') IS NOT NULL
    DROP TABLE [dbo].[Combinations];
GO
IF OBJECT_ID(N'[dbo].[Events]', 'U') IS NOT NULL
    DROP TABLE [dbo].[Events];
GO
IF OBJECT_ID(N'[dbo].[Tasks]', 'U') IS NOT NULL
    DROP TABLE [dbo].[Tasks];
GO
IF OBJECT_ID(N'[dbo].[Constants]', 'U') IS NOT NULL
    DROP TABLE [dbo].[Constants];
GO
IF OBJECT_ID(N'[dbo].[Parameters]', 'U') IS NOT NULL
    DROP TABLE [dbo].[Parameters];
GO
IF OBJECT_ID(N'[dbo].[ParameterValues]', 'U') IS NOT NULL
    DROP TABLE [dbo].[ParameterValues];
GO
IF OBJECT_ID(N'[dbo].[ConditionalProfits]', 'U') IS NOT NULL
    DROP TABLE [dbo].[ConditionalProfits];
GO
IF OBJECT_ID(N'[dbo].[ExpectedMonetaryValues]', 'U') IS NOT NULL
    DROP TABLE [dbo].[ExpectedMonetaryValues];
GO

-- --------------------------------------------------
-- Creating all tables
-- --------------------------------------------------

-- Creating table 'Actions'
CREATE TABLE [dbo].[Actions] (
    [Id] int IDENTITY(1,1) NOT NULL,
    [Name] nvarchar(max)  NOT NULL,
    [ExpectedMonetaryValue_Id] int  NOT NULL,
    [ExpectedOpportunityLoss_Id] int  NOT NULL
);
GO

-- Creating table 'Combinations'
CREATE TABLE [dbo].[Combinations] (
    [Id] int IDENTITY(1,1) NOT NULL,
    [ActionId] int  NOT NULL,
    [EventId] int  NOT NULL,
    [TaskId] int  NOT NULL,
    [ConditionalProfit_Id] int  NOT NULL,
    [WeightedProfit_Id] int  NOT NULL,
    [ConditionalOpportunityLoss_Id] int  NOT NULL,
    [WeightedOpportunityLoss_Id] int  NOT NULL
);
GO

-- Creating table 'Events'
CREATE TABLE [dbo].[Events] (
    [Id] int IDENTITY(1,1) NOT NULL,
    [Name] nvarchar(max)  NOT NULL,
    [Probability] decimal(18,8)  NOT NULL
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
    [Value] decimal(18,8)  NOT NULL,
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
    [Value] decimal(18,8)  NOT NULL,
    [ParameterId] int  NOT NULL,
    [CombinationId] int  NOT NULL
);
GO

-- Creating table 'ConditionalProfits'
CREATE TABLE [dbo].[ConditionalProfits] (
    [Id] int IDENTITY(1,1) NOT NULL,
    [Value] float  NOT NULL
);
GO

-- Creating table 'ExpectedMonetaryValues'
CREATE TABLE [dbo].[ExpectedMonetaryValues] (
    [Id] int IDENTITY(1,1) NOT NULL,
    [Value] float  NOT NULL
);
GO

-- Creating table 'ExpectedOpportunityLosses'
CREATE TABLE [dbo].[ExpectedOpportunityLosses] (
    [Id] int IDENTITY(1,1) NOT NULL,
    [Value] float  NOT NULL
);
GO

-- Creating table 'WeightedProfits'
CREATE TABLE [dbo].[WeightedProfits] (
    [Id] int IDENTITY(1,1) NOT NULL,
    [Value] float  NOT NULL
);
GO

-- Creating table 'ConditionalOpportunityLosses'
CREATE TABLE [dbo].[ConditionalOpportunityLosses] (
    [Id] int IDENTITY(1,1) NOT NULL,
    [Value] float  NOT NULL
);
GO

-- Creating table 'WeightedOpportunityLosses'
CREATE TABLE [dbo].[WeightedOpportunityLosses] (
    [Id] int IDENTITY(1,1) NOT NULL,
    [Value] float  NOT NULL
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

-- Creating primary key on [Id] in table 'ConditionalProfits'
ALTER TABLE [dbo].[ConditionalProfits]
ADD CONSTRAINT [PK_ConditionalProfits]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating primary key on [Id] in table 'ExpectedMonetaryValues'
ALTER TABLE [dbo].[ExpectedMonetaryValues]
ADD CONSTRAINT [PK_ExpectedMonetaryValues]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating primary key on [Id] in table 'ExpectedOpportunityLosses'
ALTER TABLE [dbo].[ExpectedOpportunityLosses]
ADD CONSTRAINT [PK_ExpectedOpportunityLosses]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating primary key on [Id] in table 'WeightedProfits'
ALTER TABLE [dbo].[WeightedProfits]
ADD CONSTRAINT [PK_WeightedProfits]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating primary key on [Id] in table 'ConditionalOpportunityLosses'
ALTER TABLE [dbo].[ConditionalOpportunityLosses]
ADD CONSTRAINT [PK_ConditionalOpportunityLosses]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating primary key on [Id] in table 'WeightedOpportunityLosses'
ALTER TABLE [dbo].[WeightedOpportunityLosses]
ADD CONSTRAINT [PK_WeightedOpportunityLosses]
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

-- Creating foreign key on [ConditionalProfit_Id] in table 'Combinations'
ALTER TABLE [dbo].[Combinations]
ADD CONSTRAINT [FK_CombinationConditionalProfit]
    FOREIGN KEY ([ConditionalProfit_Id])
    REFERENCES [dbo].[ConditionalProfits]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;

-- Creating non-clustered index for FOREIGN KEY 'FK_CombinationConditionalProfit'
CREATE INDEX [IX_FK_CombinationConditionalProfit]
ON [dbo].[Combinations]
    ([ConditionalProfit_Id]);
GO

-- Creating foreign key on [ExpectedMonetaryValue_Id] in table 'Actions'
ALTER TABLE [dbo].[Actions]
ADD CONSTRAINT [FK_ActionExpectedMonetaryValue]
    FOREIGN KEY ([ExpectedMonetaryValue_Id])
    REFERENCES [dbo].[ExpectedMonetaryValues]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;

-- Creating non-clustered index for FOREIGN KEY 'FK_ActionExpectedMonetaryValue'
CREATE INDEX [IX_FK_ActionExpectedMonetaryValue]
ON [dbo].[Actions]
    ([ExpectedMonetaryValue_Id]);
GO

-- Creating foreign key on [ExpectedOpportunityLoss_Id] in table 'Actions'
ALTER TABLE [dbo].[Actions]
ADD CONSTRAINT [FK_ActionExpectedOpportunityLoss]
    FOREIGN KEY ([ExpectedOpportunityLoss_Id])
    REFERENCES [dbo].[ExpectedOpportunityLosses]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;

-- Creating non-clustered index for FOREIGN KEY 'FK_ActionExpectedOpportunityLoss'
CREATE INDEX [IX_FK_ActionExpectedOpportunityLoss]
ON [dbo].[Actions]
    ([ExpectedOpportunityLoss_Id]);
GO

-- Creating foreign key on [WeightedProfit_Id] in table 'Combinations'
ALTER TABLE [dbo].[Combinations]
ADD CONSTRAINT [FK_CombinationWeightedProfit]
    FOREIGN KEY ([WeightedProfit_Id])
    REFERENCES [dbo].[WeightedProfits]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;

-- Creating non-clustered index for FOREIGN KEY 'FK_CombinationWeightedProfit'
CREATE INDEX [IX_FK_CombinationWeightedProfit]
ON [dbo].[Combinations]
    ([WeightedProfit_Id]);
GO

-- Creating foreign key on [ConditionalOpportunityLoss_Id] in table 'Combinations'
ALTER TABLE [dbo].[Combinations]
ADD CONSTRAINT [FK_CombinationConditionalOpportunityLoss]
    FOREIGN KEY ([ConditionalOpportunityLoss_Id])
    REFERENCES [dbo].[ConditionalOpportunityLosses]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;

-- Creating non-clustered index for FOREIGN KEY 'FK_CombinationConditionalOpportunityLoss'
CREATE INDEX [IX_FK_CombinationConditionalOpportunityLoss]
ON [dbo].[Combinations]
    ([ConditionalOpportunityLoss_Id]);
GO

-- Creating foreign key on [WeightedOpportunityLoss_Id] in table 'Combinations'
ALTER TABLE [dbo].[Combinations]
ADD CONSTRAINT [FK_CombinationWeightedOpportunityLoss]
    FOREIGN KEY ([WeightedOpportunityLoss_Id])
    REFERENCES [dbo].[WeightedOpportunityLosses]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;

-- Creating non-clustered index for FOREIGN KEY 'FK_CombinationWeightedOpportunityLoss'
CREATE INDEX [IX_FK_CombinationWeightedOpportunityLoss]
ON [dbo].[Combinations]
    ([WeightedOpportunityLoss_Id]);
GO

-- --------------------------------------------------
-- Script has ended
-- --------------------------------------------------