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
    public interface IHolidayTypeRepository
    {
        Task<int> InsertAsync(
            SqlConnection connection,
            DbTransaction transaction,
            IEnumerable<HolidayTypeEntity> entities);
    }
    public class HolidayTypeRepository : IHolidayTypeRepository
    {
        public async Task<int> InsertAsync(
            SqlConnection connection, 
            DbTransaction transaction, 
            IEnumerable<HolidayTypeEntity> entities)
        {
            var rowsAffected = 0;

            foreach (var entity in entities)
            {
                rowsAffected += await connection.ExecuteAsync(@"
				INSERT INTO [dbo].[HolidayTypes]
							([Type]
							,[CountryId])
						VALUES
							(@Type
							,@CountryId)",
                new
                {   entity.Type,
                    entity.CountryId
                }, transaction: transaction);
            }
            return rowsAffected;
        }
    }
}
