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
    public interface IHolidayFlagRepository
    {
        Task<int> InsertAsync(
            SqlConnection connection,
            DbTransaction transaction,
            IEnumerable<HolidayFlagEntity> entities);
    }
    public class HolidayFlagRepository : IHolidayFlagRepository
    {
        public async Task<int> InsertAsync(
            SqlConnection connection,
            DbTransaction transaction,
            IEnumerable<HolidayFlagEntity> entities)
        {
            if (entities is null)
            {
                return 0;
            }

            var rowsAffected = 0;

            foreach (var entity in entities)
            {
                rowsAffected += await connection.ExecuteAsync(@"
				                INSERT INTO [dbo].[HolidayFlags]
							                ([Type]
							                ,[HolidayId])
						                VALUES
							                (@Type
							                ,@HolidayId)",
                new
                {
                    entity.Type,
                    entity.HolidayId
                }, transaction: transaction);
            }
            return rowsAffected;
        }
    }
}
