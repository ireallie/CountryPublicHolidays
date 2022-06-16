using CountryPublicHolidays.ServiceLibrary.Entities;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace CountryPublicHolidays.ServiceLibrary.Repositories
{
    public interface IHolidayRepository
    {
        Task<Dictionary<int, List<HolidayEntity>>> GetAsync();
        Task<int> InsertAsync(HolidayEntity entity, string country);
        Task<bool> IsHoliday(string date, string country);
        Task<IEnumerable<DateTime>> GetHolidaysDates(string country, int year);
    }
}
