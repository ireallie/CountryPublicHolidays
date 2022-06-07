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
    public interface ICountryRepository
    {
        Task<IEnumerable<CountryEntity>> GetAsync();
        Task<int> InsertAsync(CountryEntity entity);
    }
    public class CountryRepository : ICountryRepository
    {
        private readonly string _connectionString;
        private readonly IRegionRepository _regionRepository;
        private readonly IHolidayTypeRepository _holidayTypeRepository;

        public CountryRepository(
            IConfiguration configuration,
            IRegionRepository regionRepository, 
            IHolidayTypeRepository holidayTypeRepository)
        {
            _connectionString = configuration.GetConnectionString("MainDatabase");
            _regionRepository = regionRepository;
            _holidayTypeRepository = holidayTypeRepository;
        }

        public async Task<IEnumerable<CountryEntity>> GetAsync()
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                await connection.OpenAsync();
                var countryDictionary = new Dictionary<Guid, CountryEntity>();

                var result = (await connection.QueryAsync<CountryEntity, HolidayTypeEntity, CountryEntity>(
                   @"SELECT c.*, h.* FROM Countries c INNER JOIN HolidayTypes h ON c.Id = h.CountryId",
                   (c, h) =>
                   {
                       CountryEntity countryEntity = null;

                       if (!countryDictionary.TryGetValue(c.Id, out countryEntity))
                       {
                           countryEntity = c;
                           countryEntity.HolidayTypes = new List<HolidayTypeEntity>();
                           countryDictionary.Add(countryEntity.Id, countryEntity);
                       }

                       countryEntity.HolidayTypes.Add(h);
                       return countryEntity;
                   }, splitOn: "Id,CountryId"))
                   .Distinct()
                   .ToList();

                result = (await connection.QueryAsync<CountryEntity, RegionEntity, CountryEntity>(
                  @"SELECT c.*, r.* FROM Countries c INNER JOIN Regions r ON c.Id = r.CountryId",
                  (c, r) =>
                  {
                      CountryEntity countryEntity = null;

                      if (!countryDictionary.TryGetValue(c.Id, out countryEntity))
                      {
                          countryEntity = c;
                          countryDictionary.Add(countryEntity.Id, countryEntity);
                      }

                      if (countryEntity.Regions is null)
                      {
                          countryEntity.Regions = new List<RegionEntity>();
                      }

                      countryEntity.Regions.Add(r);
                      return countryEntity;
                  }, splitOn: "Id,CountryId"))
              .Distinct()
              .ToList();

                return countryDictionary.Values;
            }
        }

        public async Task<int> InsertAsync(CountryEntity entity)
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                await connection.OpenAsync();
                using (var transaction = await connection.BeginTransactionAsync())
                {
                    var rowsAffected = await connection.ExecuteAsync(@"
                    INSERT INTO [dbo].[Countries]
                                ([Id]
                                ,[CountryCode]
                                ,[FullName]
                                ,[FromDate]
                                ,[ToDate])
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

                    rowsAffected += await _regionRepository.InsertAsync(connection, transaction, entity.Regions);

                    rowsAffected += await _holidayTypeRepository.InsertAsync(connection, transaction, entity.HolidayTypes);

                    transaction.Commit();

                    return rowsAffected;
                }              
            }
        }
    }
}
