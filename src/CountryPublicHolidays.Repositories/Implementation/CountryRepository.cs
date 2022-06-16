using CountryPublicHolidays.ServiceLibrary.Entities;
using Dapper;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CountryPublicHolidays.ServiceLibrary.Repositories
{
    public class CountryRepository : ICountryRepository
    {
        private readonly string _connectionString;
        private readonly IRegionRepository _regionRepository;
        private readonly IHolidayTypeRepository _holidayTypeRepository;
        private readonly ICountryHolidayTypeRepository _countryHolidayTypeRepository;

        public CountryRepository(
            IConfiguration configuration,
            IRegionRepository regionRepository, 
            IHolidayTypeRepository holidayTypeRepository,
            ICountryHolidayTypeRepository countryHolidayTypeRepository)
        {
            _connectionString = configuration.GetConnectionString("MainDatabase");
            _regionRepository = regionRepository;
            _holidayTypeRepository = holidayTypeRepository;
            _countryHolidayTypeRepository = countryHolidayTypeRepository;
        }

        public async Task<IEnumerable<CountryEntity>> GetAsync()
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                await connection.OpenAsync();

                string sql = @"select c.*, ht.Type, r.Region
                            from Countries c 
                            join CountriesHolidayTypes cht on cht.CountryId = c.Id 
                            join HolidayTypes ht on ht.Id = cht.HolidayTypeId
                            left join Regions r on r.CountryId = c.Id";

                var countryDictionary = new Dictionary<Guid, CountryEntity>();

                var list = connection.Query<CountryEntity, HolidayTypeEntity, RegionEntity, CountryEntity >(
                    sql,
                    (country, holidayType, region) =>
                    {
                        CountryEntity countryEntry;

                        if (!countryDictionary.TryGetValue(country.Id, out countryEntry))
                        {
                            countryEntry = country;
                            country.HolidayTypes = new List<HolidayTypeEntity>();
                            country.Regions = new List<RegionEntity>();
                            countryDictionary.Add(countryEntry.Id, countryEntry);
                        }

                        if(!countryDictionary[country.Id].HolidayTypes.Any(ht => ht.Type == holidayType.Type))
                        {
                            countryEntry.HolidayTypes.Add(holidayType);
                        }

                        countryEntry.Regions.Add(region);
                        return countryEntry;
                    },
                    splitOn: "Type, Region")
                .Distinct()
                .ToList();

                return list;
            }
        }

        public async Task<Guid> GetCountryIdAsync(string country)
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                var result = await connection.QueryFirstAsync<Guid>(@"
				SELECT Id
				FROM [Countries]
                WHERE[CountryCode] = @Country",
                new
                {
                    Country = country
                });
                return result;
            }
        }

        public async Task<Guid> InsertAsync(CountryEntity entity)
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                await connection.OpenAsync();
                using (var transaction = await connection.BeginTransactionAsync())
                {
                    var countryId = await connection.ExecuteScalarAsync<Guid>(@"
                        INSERT INTO [dbo].[Countries]
                                    ([Id]
                                    ,[CountryCode]
                                    ,[FullName]
                                    ,[FromDate]
                                    ,[ToDate])
                                OUTPUT Inserted.Id
                                VALUES
                                    (@Id
                                    ,@CountryCode
                                    ,@FullName
                                    ,@FromDate
                                    ,@ToDate)",
                               new
                               {
                                   entity.Id,
                                   entity.CountryCode,
                                   entity.FullName,
                                   entity.FromDate,
                                   entity.ToDate
                               }, transaction);

                    await _regionRepository.InsertAsync(connection, transaction, entity.Regions);

                    foreach (var holidayType in entity.HolidayTypes)
                    {
                        var holidayTypeId = await _holidayTypeRepository.InsertAsync(connection, transaction, holidayType);

                        await _countryHolidayTypeRepository.InsertAsync(connection, transaction, new CountryHolidayTypeEntity { CountryId = countryId, HolidayTypeId = holidayTypeId });
                    }

                    transaction.Commit();

                    return countryId;
                }              
            }
        }
    }
}
