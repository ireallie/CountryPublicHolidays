using CountryPublicHolidays.ServiceLibrary.Entities;
using Dapper;
using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CountryPublicHolidays.ServiceLibrary.Repositories
{
    public class HolidayFlagRepository : IHolidayFlagRepository
    {
        public async Task<Guid> InsertAsync(
            SqlConnection connection,
            DbTransaction transaction,
            HolidayFlagEntity entity)
        {
            var holidayFlagId = await connection.ExecuteScalarAsync<Guid>(@"
				       IF NOT EXISTS
                          (
                                    SELECT *
                                    FROM[dbo].[HolidayFlags]
                                    WHERE Name = @Name        
                          )

                        BEGIN
                           INSERT INTO[dbo].[HolidayFlags]
                                            ([Id]
                                            ,[Name])
                                        OUTPUT Inserted.Id
                                        VALUES
                                            (@Id
                                            ,@Name)
                        END

                        ELSE
                        BEGIN
                            SELECT Id
                            FROM[dbo].[HolidayFlags]
                            WHERE Name = @Name
                        END",

            new
            {
                entity.Id,
                entity.Name
            }, transaction: transaction);

            return holidayFlagId;
        }
    }
}
