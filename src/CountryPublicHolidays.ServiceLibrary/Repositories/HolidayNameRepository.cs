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
    public interface IHolidayNameRepository
    {
        Task<int> InsertAsync(
            SqlConnection connection,
            DbTransaction transaction,
            IEnumerable<HolidayNameEntity> entities);
    }
    public class HolidayNameRepository : IHolidayNameRepository
    {
        public async Task<int> InsertAsync(
            SqlConnection connection,
            DbTransaction transaction,
            IEnumerable<HolidayNameEntity> entities)
        {
            var rowsAffected = 0;

            foreach (var entity in entities)
            {
                rowsAffected += await connection.ExecuteAsync(@"
				INSERT INTO [dbo].[HolidayNames]
							([Lang]
							,[Text]
                            ,[HolidayId])
						VALUES
							(@Lang
							,@Text
                            ,@HolidayId)",
                new
                {
                    entity.Lang,
                    entity.Text,
                    entity.HolidayId
                }, transaction: transaction);
            }
            return rowsAffected;
        }
    }
}
