using CountryPublicHolidays.ServiceLibrary.Entities;
using Dapper;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CountryPublicHolidays.ServiceLibrary.Repositories
{
    public class HolidayTypeRepository : IHolidayTypeRepository
    {
        private readonly string _connectionString;
        public HolidayTypeRepository(
            IConfiguration configuration)
        {
            _connectionString = configuration.GetConnectionString("MainDatabase");
        }
        public async Task<Guid> InsertAsync(
            SqlConnection connection,
            DbTransaction transaction,
            HolidayTypeEntity entity)
        {
           var holidayTypeId = await connection.ExecuteScalarAsync<Guid>(@"
                IF NOT EXISTS
                        (
                            SELECT *
                            FROM [dbo].[HolidayTypes]
                            WHERE Type = @Type
                        )

                        BEGIN
                           INSERT INTO [dbo].[HolidayTypes]
							                ([Id]
							                ,[Type])
                                        OUTPUT Inserted.Id
						                VALUES
							                (@Id
							                ,@Type)
                        END

                        ELSE
                        BEGIN                    
                            SELECT Id
                            FROM [dbo].[HolidayTypes]
                            WHERE Type = @Type
                        END",
                new
                {
                    entity.Id,
                    entity.Type
                }, transaction: transaction);
  
            return holidayTypeId;
        }

        public async Task<Guid> GetHolidayTypeIdAsync(
            SqlConnection connection,
            DbTransaction transaction,
            string holidayType)
        {
                var result = await connection.QueryFirstAsync<Guid>(@"
				SELECT Id
				FROM [HolidayTypes]
                WHERE[Type] = @Type",
                new
                {
                    Type = holidayType
                }, transaction: transaction);
                return result;
        }
    }
}
