CREATE TABLE [dbo].[Holidays]
(
	[Id]			UNIQUEIDENTIFIER	NOT NULL PRIMARY KEY,
	[Date]			DATETIME			NOT NULL,
	[DateTo]		DATETIME			NULL,
	[ObservedOn]	DATETIME			NULL,
	[HolidayTypeId]	UNIQUEIDENTIFIER	NOT NULL,
	CONSTRAINT [FK_Holidays_To_HolidayTypes_Id] FOREIGN KEY ([HolidayTypeId]) REFERENCES [HolidayTypes]([Id])

)
