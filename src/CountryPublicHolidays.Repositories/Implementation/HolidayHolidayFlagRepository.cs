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
    public class HolidayHolidayFlagRepository : IHolidayHolidayFlagRepository
    {
        public async Task InsertAsync(
            SqlConnection connection,
            DbTransaction transaction,
            HolidayHolidayFlagEntity entity)
        {
            await connection.ExecuteAsync(@"
				INSERT INTO [dbo].[HolidaysHolidayFlags]
							([HolidayId]
							,[HolidayFlagId])
						VALUES
							(@HolidayId
							,@HolidayFlagId)",
            new
            {
                entity.HolidayId,
                entity.HolidayFlagId
            }, transaction: transaction);
        }
    }
}
