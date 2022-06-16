CREATE TABLE [dbo].[CountriesHolidayTypes]
(
	[CountryId] UNIQUEIDENTIFIER NOT NULL,
	[HolidayTypeId] UNIQUEIDENTIFIER NOT NULL,
	PRIMARY KEY ([CountryId], [HolidayTypeId]), 
	CONSTRAINT [FK_CountriesHolidayTypes_To_Countries_Id] FOREIGN KEY ([CountryId]) REFERENCES [Countries]([Id]) ON DELETE CASCADE,
	CONSTRAINT [FK_CountriesHolidayTypes_To_HolidayTypes_Id] FOREIGN KEY ([HolidayTypeId]) REFERENCES [HolidayTypes]([Id]) ON DELETE CASCADE
)
