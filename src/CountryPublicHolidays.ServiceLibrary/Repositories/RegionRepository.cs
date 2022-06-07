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
    public interface IRegionRepository
    {
        Task<int> InsertAsync(
            SqlConnection connection, 
            DbTransaction transaction, 
            IEnumerable<RegionEntity> entities);
    }
    public class RegionRepository : IRegionRepository
    {
        public async Task<int> InsertAsync(
            SqlConnection connection, 
            DbTransaction transaction, 
            IEnumerable<RegionEntity> entities)
        {
            if (entities is null)
            {
                return 0;
            }

            var rowsAffected = 0;

            foreach (var entity in entities)
            {
                rowsAffected += await connection.ExecuteAsync(@"
				                INSERT INTO [dbo].[Regions]
							                ([Region]
							                ,[CountryId])
						                VALUES
							                (@Region
							                ,@CountryId)",
                new
                {
                    entity.Region,
                    entity.CountryId
                }, transaction: transaction);
            }
            return rowsAffected; 
        }
    }
}
