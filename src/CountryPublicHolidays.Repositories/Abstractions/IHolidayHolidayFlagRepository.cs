using CountryPublicHolidays.ServiceLibrary.Entities;
using System.Data.Common;
using System.Data.SqlClient;
using System.Threading.Tasks;

namespace CountryPublicHolidays.ServiceLibrary.Repositories
{
    public interface IHolidayHolidayFlagRepository
    {
        Task InsertAsync(
        SqlConnection connection,
        DbTransaction transaction,
        HolidayHolidayFlagEntity entity);
    }
}
