using CountryPublicHolidays.ServiceLibrary.Entities;
using CountryPublicHolidays.ServiceLibrary.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CountryPublicHolidays.ServiceLibrary.Domains
{
    public class HolidayDomain : IHolidayDomain
    {
        private readonly IHolidayRepository _holidayRepository;

        public HolidayDomain(
            IHolidayRepository holidayRepository)
        {
            _holidayRepository = holidayRepository;
        }
        public async Task<Dictionary<int, List<HolidayEntity>>> GetHolidays()
        {
            var result = await _holidayRepository.GetAsync();

            return result;
        }

        public async Task<bool> IsHoliday(string date, string country)
        {
            return await _holidayRepository.IsHoliday(date, country);
        }

        public async Task SaveHolidays(IEnumerable<HolidayEntity> entities, string country)
        {
            foreach (var entity in entities)
            {
                await _holidayRepository.InsertAsync(entity, country);
            }
        }
    }
}
