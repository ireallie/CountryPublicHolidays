CREATE TABLE [dbo].[Countries]
(
	[Id]			UNIQUEIDENTIFIER	NOT NULL PRIMARY KEY,
	[CountryCode]	NVARCHAR(3)			NOT NULL,
	[FullName]		NVARCHAR(50)		NOT NULL,
	[FromDate]		DATETIME			NOT NULL,
	[ToDate]		DATETIME			NOT NULL
)

