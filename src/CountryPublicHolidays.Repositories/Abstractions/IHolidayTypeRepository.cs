using CountryPublicHolidays.ServiceLibrary.Entities;
using System;
using System.Data.Common;
using System.Data.SqlClient;
using System.Threading.Tasks;

namespace CountryPublicHolidays.ServiceLibrary.Repositories
{
    public interface IHolidayTypeRepository
    {
            Task<Guid> InsertAsync(
            SqlConnection connection,
            DbTransaction transaction,
            HolidayTypeEntity entity);

            Task<Guid> GetHolidayTypeIdAsync(
            SqlConnection connection,
            DbTransaction transaction,
            string holidayType);
    }
}
