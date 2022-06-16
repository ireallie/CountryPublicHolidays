using CountryPublicHolidays.ServiceLibrary.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace CountryPublicHolidays.ServiceLibrary.Domains
{
    public interface ICountryDomain
    {
        Task SaveCountries(IEnumerable<CountryEntity> entities);
        Task<IEnumerable<CountryEntity>> GetCountries();
    }
}
