using Dapper;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CountryPublicHolidays.ServiceLibrary.Tests.Repositories
{
    public class DatabaseFixture : IDisposable
    {
        public SqlConnection Connection { get; private set; }

        public Guid CountryId1 = new Guid("1752073E-909A-4A68-9399-5C97E1129E6E");

        public DatabaseFixture()
        {
            Connection = new SqlConnection("Server=tcp:sqlserverforazure.database.windows.net,1433;Initial Catalog=CountryPublicHolidays;Persist Security Info=False;User ID=ireallie;Password=P@ssword123;MultipleActiveResultSets=False;Encrypt=True;TrustServerCertificate=False;Connection Timeout=30;");
            SeedData();
        }

        public void Dispose()
        {
            DeleteData();
            Connection.Close();
        }

        private void SeedData()
        {
            DeleteData();

            Connection.Execute(@"
BEGIN TRANSACTION
	INSERT INTO [dbo].[Countries]
          ([Id]
          ,[CountryCode]
          ,[FullName]
          ,[FromDate]
          ,[ToDate])
    VALUES
          (@CountryId
          ,'MNE'
          ,'Montenegro Seeded'
          ,GETUTCDATE()
          ,GETUTCDATE())
  INSERT INTO [dbo].[Regions]
              ([Region]
              ,[CountryId])
        VALUES
              ('Andrijevica seeded'
              ,@CountryId)
  INSERT INTO [dbo].[HolidayTypes]
              ([Type]
              ,[CountryId])
        VALUES
              ('public_holiday Seeded'
              ,@CountryId)
COMMIT TRANSACTION
      ", new { CountryId = CountryId1 });
        }

        private void DeleteData()
        {
            Connection.Execute(@"
BEGIN TRANSACTION
  DELETE FROM [dbo].[Regions] WHERE [CountryId] = @CountryId
  DELETE FROM [dbo].[HolidayTypes] WHERE [CountryId] = @CountryId
  DELETE FROM [dbo].[Countries] WHERE [Id] = @CountryId
COMMIT TRANSACTION", new { CountryId = CountryId1 });
        }
    }
}
