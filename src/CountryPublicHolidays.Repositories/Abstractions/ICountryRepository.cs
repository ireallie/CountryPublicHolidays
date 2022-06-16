using CountryPublicHolidays.ServiceLibrary.Entities;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace CountryPublicHolidays.ServiceLibrary.Repositories
{
    public interface ICountryRepository
    {
        Task<IEnumerable<CountryEntity>> GetAsync();
        Task<Guid> InsertAsync(CountryEntity entity);
        Task<Guid> GetCountryIdAsync(string country);
    }
}
