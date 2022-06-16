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
    public class HolidayRepository : IHolidayRepository
    {
        private readonly string _connectionString;
        private readonly IHolidayFlagRepository _holidayFlagRepository;
        private readonly IHolidayNameRepository _holidayNameRepository;
        private readonly IHolidayNoteRepository _holidayNoteRepository;
        private readonly IHolidayHolidayFlagRepository _holidayHolidayFlagRepository;
        private readonly IHolidayTypeRepository _holidayTypeRepository;
        private readonly ICountryHolidayRepository _countryHolidayRepository;
        private readonly ICountryRepository _countryRepository;


        public HolidayRepository(
            IConfiguration configuration,
            IHolidayFlagRepository holidayFlagRepository,
            IHolidayNameRepository holidayNameRepository,
            IHolidayNoteRepository holidayNoteRepository,
            IHolidayHolidayFlagRepository holidayHolidayFlagRepository,
            IHolidayTypeRepository holidayTypeRepository,
            ICountryHolidayRepository countryHolidayRepository,
            ICountryRepository countryRepository
            )
        {
            _connectionString = configuration.GetConnectionString("MainDatabase");
            _holidayFlagRepository = holidayFlagRepository;
            _holidayNameRepository = holidayNameRepository;
            _holidayNoteRepository = holidayNoteRepository;
            _holidayHolidayFlagRepository = holidayHolidayFlagRepository;
            _holidayTypeRepository = holidayTypeRepository;
            _countryHolidayRepository = countryHolidayRepository;
            _countryRepository = countryRepository;
        }
        public async Task<Dictionary<int, List<HolidayEntity>>> GetAsync()
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                await connection.OpenAsync();

                var holidaysDictionary = new Dictionary<int, List<HolidayEntity>>();

                (await connection.QueryAsync<HolidayEntity, HolidayNameEntity, HolidayNoteEntity, HolidayFlagEntity, HolidayTypeEntity, HolidayEntity >(
                   @"SELECT h.*, hname.*, hnote.*, hflag.*, ht.Type
                    FROM Holidays h 
                    left JOIN HolidayNames hname 
                    ON h.Id = hname.HolidayId 
                    LEFT JOIN HolidayNotes hnote 
                    ON h.Id = hnote.HolidayId
                    left join HolidaysHolidayFlags hhflag
                    on hhflag.HolidayId = h.Id 
                    left join HolidayFlags hflag
                    on hflag.Id = hhflag.HolidayFlagId
                    left join HolidayTypes ht
                    on ht.Id = h.HolidayTypeId",
                   (holiday, name, note, flag, holidayType) =>
                   {
                       int holidayMonth = holiday.Date.Month;

                       if(!holidaysDictionary.Keys.Contains(holidayMonth))
                       {
                           holidaysDictionary.Add(holidayMonth, new List<HolidayEntity>());
                       }

                       HolidayEntity holidayEntity = holiday;

                       if (!holidaysDictionary[holidayMonth].Any(h => h.Id == holiday.Id))
                       {
                           holidayEntity.Name = new List<HolidayNameEntity>();
                           holidayEntity.Note = new List<HolidayNoteEntity>();
                           holidayEntity.Flags = new List<HolidayFlagEntity>();
                           
                           holidaysDictionary[holidayMonth].Add(holiday);
                       }
                       else
                       {
                           holidayEntity = holidaysDictionary[holidayMonth].Where(h => h.Id == holiday.Id).FirstOrDefault();
                       }

                       holidayEntity.Name.Add(name);
                       holidayEntity.HolidayType = holidayType;

                       if (note != null)
                       {
                           holidayEntity.Note.Add(note);
                       }

                       if (flag != null)
                       {
                           holidayEntity.Flags.Add(flag);
                       }
                       return holidayEntity;

                   }, splitOn: "Lang, Lang, Id, Type"))
                   .Distinct()
                   .ToList();

                return holidaysDictionary;
            }
        }

        public async Task<IEnumerable<DateTime>> GetHolidaysDates(string country, int year)
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                var result = await connection.QueryAsync<DateTime>(@"
				             SELECT h.[Date]
				             FROM [Holidays] h
							 join CountriesHolidays ch
							 on ch.HolidayId = h.Id
							 join Countries c
							 on c.Id = ch.CountryId
                             WHERE c.[CountryCode] = @Country AND YEAR(h.[Date]) = @Year",
                new
                {
                    Country = country,
                    Year = year
                });

                return result;
            }
        }

        public async Task<int> InsertAsync(HolidayEntity entity, string country)
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                await connection.OpenAsync();
                using (var transaction = await connection.BeginTransactionAsync())
                {
                    var rowsAffected = 0;

                    var holidayTypeId = await _holidayTypeRepository.GetHolidayTypeIdAsync(connection, transaction, entity.HolidayType.Type);

                    var countryId = await _countryRepository.GetCountryIdAsync(country);

                    var holidayId = await connection.ExecuteScalarAsync<Guid>(@"
                    INSERT INTO [dbo].[Holidays]
                                ([Id]
                                ,[Date]
                                ,[DateTo]
                                ,[ObservedOn]
                                ,[HolidayTypeId])
                            OUTPUT Inserted.Id
                            VALUES
                                (@Id
                                ,@Date
                                ,@DateTo
                                ,@ObservedOn
                                ,@HolidayTypeId)",
                               new
                               {
                                   entity.Id,
                                   entity.Date,
                                   entity.DateTo,
                                   entity.ObservedOn,
                                   HolidayTypeId = holidayTypeId
                               }, transaction);

                    
                    foreach (var flag in entity.Flags)
                    {
                        var holidayFlagId = await _holidayFlagRepository.InsertAsync(connection, transaction, flag);

                        await _holidayHolidayFlagRepository.InsertAsync(connection, transaction, new HolidayHolidayFlagEntity { HolidayId = holidayId, HolidayFlagId = holidayFlagId });
                    }

                    await _countryHolidayRepository.InsertAsync(connection, transaction, new CountryHolidayEntity { HolidayId = holidayId, CountryId = countryId });

                    rowsAffected += await _holidayNameRepository.InsertAsync(connection, transaction, entity.Name);

                    rowsAffected += await _holidayNoteRepository.InsertAsync(connection, transaction, entity.Note);

                    transaction.Commit();

                    return rowsAffected;
                }
            }
        }

        public async Task<bool> IsHoliday(string date, string country)
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                var result = await connection.QueryAsync<HolidayEntity>(@"
				             SELECT h.*
				             FROM [Holidays] h
							 join CountriesHolidays ch
							 on ch.HolidayId = h.Id
							 join Countries c
							 on c.Id = ch.CountryId
                             WHERE h.[Date] = @Date AND c.[CountryCode] = @Country",
                new
                {
                    Date = date,
                    Country = country
                });
                
                return result.Any();
            }
        }
    }
}
