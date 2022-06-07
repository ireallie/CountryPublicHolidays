using CountryPublicHolidays.ServiceLibrary.Repositories;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace CountryPublicHolidays.ServiceLibrary.Calculators
{
    public interface IMaxNumberOfFreeDaysCalculator
    {
        Task<int> CalculateNumberOfFreeDaysInARow(string country, int year, HttpClient httpClient);
    }
    public class MaxNumberOfFreeDaysCalculator : IMaxNumberOfFreeDaysCalculator
    {
        private readonly IHolidayRepository _holidayRepository;

        public MaxNumberOfFreeDaysCalculator(
            IHolidayRepository holidayRepository)
        {
            _holidayRepository = holidayRepository;
        }

        public async Task<int> CalculateNumberOfFreeDaysInARow(string country, int year, HttpClient httpClient)
        {
            var isFreeDay = new List<bool>();
            List<DateTime> holidaysDates = (List<DateTime>)await _holidayRepository.GetHolidaysDates(country, year);

            for (var date = new DateTime(year, 1, 1); date <= new DateTime(year, 12, 31); date = date.AddDays(1))
            {
                string requestEndpoint = $"?action=isWorkDay&date={date.ToString(("dd-MM-yyyy"))}&country={country}";
                HttpResponseMessage response = httpClient.GetAsync(requestEndpoint).Result;
                var result = response.Content.ReadAsStringAsync().Result;
                var jo = JObject.Parse(result);
                bool isWorkDay = jo.SelectToken("isWorkDay").Value<bool>();

                if (holidaysDates.Contains(date) || !isWorkDay)
                {
                    isFreeDay.Add(true);
                }
                else
                {
                    isFreeDay.Add(false);
                }
            }

            int maxCount = 0;
            int count = 1;
            int i = 0;

            for (var date = new DateTime(year, 1, 1); date < new DateTime(year, 12, 31); date = date.AddDays(1))
            {
                if (isFreeDay[i] && isFreeDay[i + 1])
                {
                    count++;

                    if (count > maxCount)
                    {
                        maxCount = count;
                    }               
                }
                else
                {
                    count = 1;
                }
                    i++;
            }

            return maxCount;
        }
    }
}
