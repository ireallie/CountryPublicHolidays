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
    public class CountryHolidayRepository : ICountryHolidayRepository
    {
        public async Task InsertAsync(
            SqlConnection connection,
            DbTransaction transaction,
            CountryHolidayEntity entity)
        {
            await connection.ExecuteAsync(@"
				INSERT INTO [dbo].[CountriesHolidays]
							([HolidayId]
							,[CountryId])
						VALUES
							(@HolidayId
							,@CountryId)",
            new
            {
                entity.HolidayId,
                entity.CountryId
            }, transaction: transaction);
        }
    }
}
