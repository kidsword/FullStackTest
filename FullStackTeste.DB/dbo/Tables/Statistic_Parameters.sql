CREATE TABLE [dbo].[Statistic_Parameters] (
    [Id]          INT           IDENTITY (1, 1) NOT NULL,
    [StatisticId] INT           NULL,
    [Name]        VARCHAR (255) NOT NULL,
    [Value]       VARCHAR (255) NOT NULL,
    PRIMARY KEY CLUSTERED ([Id] ASC),
    FOREIGN KEY ([StatisticId]) REFERENCES [dbo].[Statistic] ([Id])
);

