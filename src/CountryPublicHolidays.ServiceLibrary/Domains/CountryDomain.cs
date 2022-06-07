using CountryPublicHolidays.ServiceLibrary.Entities;
using CountryPublicHolidays.ServiceLibrary.Repositories;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CountryPublicHolidays.ServiceLibrary.Domains
{
    public interface ICountryDomain
    {
        Task SaveCountries(IEnumerable<CountryEntity> entities);
        Task<IEnumerable<CountryEntity>> GetCountries();
    }
    public class CountryDomain : ICountryDomain
    {
        private readonly ICountryRepository _countryRepository;

        public CountryDomain(
            ICountryRepository countryRepository)
        {
            _countryRepository = countryRepository;
        }

        public async Task<IEnumerable<CountryEntity>> GetCountries()
        {
           var result = await _countryRepository.GetAsync();

           return result;
        }

        public async Task SaveCountries(IEnumerable<CountryEntity> entities)
        {
            foreach(var entity in entities)
            {
                await _countryRepository.InsertAsync(entity);
            }   
        }
    }
}
