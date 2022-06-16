using CountryPublicHolidays.ServiceLibrary.Entities;
using System.Collections.Generic;
using System.Data.Common;
using System.Data.SqlClient;
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
}
