using CountryPublicHolidays.ServiceLibrary.Calculators;
using CountryPublicHolidays.ServiceLibrary.Domains;
using CountryPublicHolidays.ServiceLibrary.Entities;
using CountryPublicHolidays.ServiceLibrary.Repositories;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;

namespace CountryPublicHolidays.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PublicHolidaysController : ControllerBase
    {
        private readonly ICountryDomain _countryDomain;
        private readonly HttpClient _httpClient;
        private readonly IHolidayDomain _holidayDomain;
        private readonly IMaxNumberOfFreeDaysCalculator _maxNumberOfFreeDaysCalculator;

        public PublicHolidaysController(
            ICountryDomain countryDomain,
            HttpClient httpClient,
            IHolidayDomain holidayDomain,
            IMaxNumberOfFreeDaysCalculator maxNumberOfFreeDaysCalculator)
        {
            _countryDomain = countryDomain;
            _httpClient = httpClient;
            _holidayDomain = holidayDomain;
            _maxNumberOfFreeDaysCalculator = maxNumberOfFreeDaysCalculator;
        }

        /// <summary>
        /// Get the maximum number of free (free day + holiday) days in a row, which will be by a given country and year.
        /// </summary>
        /// <param name="country"></param>
        /// <param name="year"></param>
        /// <returns></returns>

        [HttpGet("getMaxNumberOfFreeDays")] // api/publicholidays/getMaxNumberOfFreeDays?country=montenegro&year=2022
        public async Task<JsonResult> GetMaxNumberOfFreeDaysAsync(
            [FromQuery]string country,
            [FromQuery]int year)
        {
            int result = await _maxNumberOfFreeDaysCalculator.CalculateNumberOfFreeDaysInARow(country, year, _httpClient);

            return new JsonResult(result);
        }

        /// <summary>
        /// Get a specific day status (workday, free day, holiday) for a given date and country.
        /// </summary>
        /// <param name="date"></param>
        /// <param name="country"></param>
        /// <returns></returns>

        [HttpGet("getDayStatus")] // api/publicholidays/getDayStatus?date=05-07-2022&country=svk
        public async Task<JsonResult> GetDayStatusAsync(
            [FromQuery]string date,
            [FromQuery]string country) 
        {
            var dayStatus = new DayStatusViewModel();

            bool isHoliday = await _holidayDomain.IsHoliday(date, country);

            if (isHoliday)
            {
                dayStatus.DayStatus = "holiday";
                return new JsonResult(dayStatus);
            }

            string requestEndpoint = $"?action=isWorkDay&date={date}&country={country}";
            HttpResponseMessage response = _httpClient.GetAsync(requestEndpoint).Result;

            if (response.IsSuccessStatusCode)
            {
                var result = response.Content.ReadAsStringAsync().Result;
                var jo = JObject.Parse(result);
                bool isWorkDay = jo.SelectToken("isWorkDay").Value<bool>();

                if (isWorkDay)
                {
                    dayStatus.DayStatus = "workday";
                    return new JsonResult(dayStatus);
                }
                else
                {
                    dayStatus.DayStatus = "free day";
                    return new JsonResult(dayStatus);
                }
            }
            else
            {
                return new JsonResult(("{0} ({1})", (int)response.StatusCode, response.ReasonPhrase));
            }
        }

        /// <summary>
        /// Returns the grouped by a month holidays list for a given country and year.
        /// </summary>
        /// <param name="country"></param>
        /// <param name="year"></param>
        /// <returns></returns>

        [HttpGet("getGroupedHolidaysByMonth")] // api/publicholidays/getGroupedHolidaysByMonth?country=montenegro&year=2022
        public async Task<JsonResult> GetGroupedHolidaysByMonthAsync(
            [FromQuery]string country,
            [FromQuery]int year)
        {
            var holidays = await _holidayDomain.GetHolidays();

            if (holidays.Count() > 0)
            {
                return new JsonResult(holidays);
            }

            string requestEndpoint = $"?action=getHolidaysForYear&year={year}&country={country}";

            HttpResponseMessage response = _httpClient.GetAsync(requestEndpoint).Result;
            
            IEnumerable<HolidayEntity> holidaysList = null;

            if (response.IsSuccessStatusCode)
            {
                holidaysList = response.Content.ReadAsAsync<IEnumerable<HolidayEntity>>().Result;
                await _holidayDomain.SaveHolidays(holidaysList, country);
            }
            else
            {
                return new JsonResult(("{0} ({1})", (int)response.StatusCode, response.ReasonPhrase));
            }

            return new JsonResult(holidaysList);
        }

        /// <summary>
        /// Returns the list of supported countries.
        /// </summary>
        /// <returns></returns>

        [HttpGet("getSupportedCountriesList")] // api/publicholidays/getSupportedCountriesList
        public async Task<JsonResult> GetSupportedCountriesListAsync()
        {
            var countries = await _countryDomain.GetCountries();

            if (countries.Count() > 0)
            {
                return new JsonResult(countries);
            }

            string requestEndpoint = "?action=getSupportedCountries";

            HttpResponseMessage response = _httpClient.GetAsync(requestEndpoint).Result;  

            IEnumerable<CountryEntity> countriesList = null;

            if (response.IsSuccessStatusCode)
            {
                countriesList = response.Content.ReadAsAsync<IEnumerable<CountryEntity>>().Result;
                await _countryDomain.SaveCountries(countriesList);
            }
            else
            {
                return new JsonResult(("{0} ({1})", (int)response.StatusCode, response.ReasonPhrase));
            }

            return new JsonResult(countriesList);
        }
    }
}
