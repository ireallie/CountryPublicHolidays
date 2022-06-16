using CountryPublicHolidays.ServiceLibrary.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace CountryPublicHolidays.ServiceLibrary.Domains
{
    public interface IHolidayDomain
    {
        Task SaveHolidays(IEnumerable<HolidayEntity> entities, string country);
        Task<Dictionary<int, List<HolidayEntity>>> GetHolidays();
        Task<bool> IsHoliday(string date, string country);
    }
}
