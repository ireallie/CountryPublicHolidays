CREATE TABLE [dbo].[HolidayFlags]
(
	[Type] NVARCHAR(25) NOT NULL,
	[HolidayId] UNIQUEIDENTIFIER NOT NULL,
	CONSTRAINT [FK_HolidayFlags_To_Holidays_Id] FOREIGN KEY ([HolidayId]) REFERENCES [Holidays]([Id]) ON DELETE CASCADE
)
