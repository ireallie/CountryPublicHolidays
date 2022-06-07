CREATE TABLE [dbo].[HolidayTypes]
(
	[Type] NVARCHAR(25) NOT NULL,
	[CountryId] UNIQUEIDENTIFIER NOT NULL,
	PRIMARY KEY ([Type], [CountryId]), 
	CONSTRAINT [FK_HolidayTypes_To_Countries_Id] FOREIGN KEY ([CountryId]) REFERENCES [Countries]([Id]) ON DELETE CASCADE
)
