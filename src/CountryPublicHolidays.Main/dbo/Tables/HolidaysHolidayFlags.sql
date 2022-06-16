CREATE TABLE [dbo].[HolidaysHolidayFlags]
(
	[HolidayId] UNIQUEIDENTIFIER NOT NULL,
	[HolidayFlagId] UNIQUEIDENTIFIER NOT NULL,
	PRIMARY KEY ([HolidayId], [HolidayFlagId]), 
	CONSTRAINT [FK_HolidaysHolidayFlags_To_Holidays_Id] FOREIGN KEY ([HolidayId]) REFERENCES [Holidays]([Id]) ON DELETE CASCADE,
	CONSTRAINT [FK_HolidaysHolidayFlags_To_HolidayFlags_Id] FOREIGN KEY ([HolidayFlagId]) REFERENCES [HolidayFlags]([Id]) ON DELETE CASCADE
)
