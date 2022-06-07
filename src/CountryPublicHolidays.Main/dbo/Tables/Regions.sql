CREATE TABLE [dbo].[Regions]
(
	[Region]	NVARCHAR(50) NOT NULL,
	[CountryId] UNIQUEIDENTIFIER NOT NULL,
	PRIMARY KEY ([Region], [CountryId]), 
	CONSTRAINT [FK_Regions_To_Countries_Id] FOREIGN KEY ([CountryId]) REFERENCES [Countries]([Id]) ON DELETE CASCADE
)
