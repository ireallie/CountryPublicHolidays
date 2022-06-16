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
    public class HolidayNoteRepository : IHolidayNoteRepository
    {
        public async Task<int> InsertAsync(
            SqlConnection connection, 
            DbTransaction transaction, 
            IEnumerable<HolidayNoteEntity> entities)
        {
            if (entities is null)
            {
                return 0;
            }

            var rowsAffected = 0;

            foreach (var entity in entities)
            {
                rowsAffected += await connection.ExecuteAsync(@"
				                INSERT INTO [dbo].[HolidayNotes]
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
