using CountryPublicHolidays.ServiceLibrary.Entities;
using System;
using System.Data.Common;
using System.Data.SqlClient;
using System.Threading.Tasks;

namespace CountryPublicHolidays.ServiceLibrary.Repositories
{
    public interface IHolidayFlagRepository
    {
        Task<Guid> InsertAsync(
        SqlConnection connection,
        DbTransaction transaction,
        HolidayFlagEntity entity);
    }
}
