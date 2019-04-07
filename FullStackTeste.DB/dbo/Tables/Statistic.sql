CREATE TABLE [dbo].[Statistic] (
    [Id]          INT           IDENTITY (1, 1) NOT NULL,
    [IP]          VARCHAR (20)  NOT NULL,
    [Page]        VARCHAR (100) NOT NULL,
    [DateCreated] DATETIME      NOT NULL,
    PRIMARY KEY CLUSTERED ([Id] ASC)
);

