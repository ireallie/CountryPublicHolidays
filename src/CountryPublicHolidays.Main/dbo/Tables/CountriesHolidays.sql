CREATE TABLE [dbo].[CountriesHolidays]
(
	[HolidayId] UNIQUEIDENTIFIER NOT NULL,
	[CountryId] UNIQUEIDENTIFIER NOT NULL,
	PRIMARY KEY ([HolidayId], [CountryId]), 
	CONSTRAINT [FK_CountriesHolidays_To_Holidays_Id] FOREIGN KEY ([HolidayId]) REFERENCES [Holidays]([Id]) ON DELETE CASCADE,
	CONSTRAINT [FK_CountriesHolidays_To_Countries_Id] FOREIGN KEY ([CountryId]) REFERENCES [Countries]([Id]) ON DELETE CASCADE
)
