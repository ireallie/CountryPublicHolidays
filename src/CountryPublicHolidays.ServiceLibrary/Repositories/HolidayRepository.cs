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
    public interface IHolidayRepository
    {
        Task<Dictionary<int, List<HolidayEntity>>> GetAsync();
        Task<int> InsertAsync(HolidayEntity entity);
        Task<bool> IsHoliday(string date, string country);
        Task<IEnumerable<DateTime>> GetHolidaysDates(string country, int year);
    }
    public class HolidayRepository : IHolidayRepository
    {
        private readonly string _connectionString;
        private readonly IHolidayFlagRepository _holidayFlagRepository;
        private readonly IHolidayNameRepository _holidayNameRepository;
        private readonly IHolidayNoteRepository _holidayNoteRepository;

        public HolidayRepository(
            IConfiguration configuration,
            IHolidayFlagRepository holidayFlagRepository,
            IHolidayNameRepository holidayNameRepository,
            IHolidayNoteRepository holidayNoteRepository)
        {
            _connectionString = configuration.GetConnectionString("MainDatabase");
            _holidayFlagRepository = holidayFlagRepository;
            _holidayNameRepository = holidayNameRepository;
            _holidayNoteRepository = holidayNoteRepository;
        }
        public async Task<Dictionary<int, List<HolidayEntity>>> GetAsync()
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                await connection.OpenAsync();

                var holidaysDictionary = new Dictionary<int, List<HolidayEntity>>();

                (await connection.QueryAsync<HolidayEntity, HolidayNameEntity, HolidayNoteEntity, HolidayFlagEntity, HolidayEntity>(
                   @"SELECT h.*, hname.*, hnote.*, hflag.* 
                   FROM Holidays h 
                   INNER JOIN HolidayNames hname 
                   ON h.Id = hname.HolidayId 
                   LEFT JOIN HolidayNotes hnote 
                   ON h.Id = hnote.HolidayId
                   LEFT JOIN HolidayFlags hflag
                   ON h.Id = hflag.HolidayId",
                   (holiday, name, note, flag) =>
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

                       if (note != null)
                       {
                           holidayEntity.Note.Add(note);
                       }

                       if (flag != null)
                       {
                           holidayEntity.Flags.Add(flag);
                       }
                       return holidayEntity;

                   }, splitOn: "Lang, Lang, Type"))
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
                             WHERE h.[Country] = @Country AND YEAR(h.[Date]) = @Year",
                new
                {
                    Country = country,
                    Year = year
                });

                return result;
            }
        }

        public async Task<int> InsertAsync(HolidayEntity entity)
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                await connection.OpenAsync();
                using (var transaction = await connection.BeginTransactionAsync())
                {
                    var rowsAffected = await connection.ExecuteAsync(@"
                    INSERT INTO [dbo].[Holidays]
                                ([Id]
                                ,[Date]
                                ,[DateTo]
                                ,[ObservedOn]
                                ,[HolidayType]
                                ,[Country])
                            VALUES
                                (@Id
                                ,@Date
                                ,@DateTo
                                ,@ObservedOn
                                ,@HolidayType
                                ,@Country)",
                               new
                               {
                                   entity.Id,
                                   entity.Date,
                                   entity.DateTo,
                                   entity.ObservedOn,
                                   entity.HolidayType,
                                   entity.Country
                               }, transaction);


                    rowsAffected += await _holidayFlagRepository.InsertAsync(connection, transaction, entity.Flags);

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
				             SELECT *
				             FROM [Holidays]
                             WHERE [Date] = @Date AND [Country] = @Country",
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
