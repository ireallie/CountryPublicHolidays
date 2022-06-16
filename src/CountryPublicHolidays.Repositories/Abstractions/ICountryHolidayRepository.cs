using CountryPublicHolidays.ServiceLibrary.Entities;
using System.Data.Common;
using System.Data.SqlClient;
using System.Threading.Tasks;

namespace CountryPublicHolidays.ServiceLibrary.Repositories
{
    public interface ICountryHolidayRepository
    {
        Task InsertAsync(
        SqlConnection connection,
        DbTransaction transaction,
        CountryHolidayEntity entity);
    }
}
