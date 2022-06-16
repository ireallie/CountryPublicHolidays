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
    public class CountryHolidayTypeRepository : ICountryHolidayTypeRepository
    {
        public async Task InsertAsync(
            SqlConnection connection,
            DbTransaction transaction,
            CountryHolidayTypeEntity entity)
        {
                await connection.ExecuteAsync(@"
				INSERT INTO [dbo].[CountriesHolidayTypes]
							([CountryId]
							,[HolidayTypeId])
						VALUES
							(@CountryId
							,@HolidayTypeId)",
                new
                {
                    entity.CountryId,
                    entity.HolidayTypeId
                }, transaction: transaction);
        }
    }
}
