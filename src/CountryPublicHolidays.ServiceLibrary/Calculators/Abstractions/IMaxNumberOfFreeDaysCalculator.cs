using System.Net.Http;
using System.Threading.Tasks;

namespace CountryPublicHolidays.ServiceLibrary.Calculators
{
    public interface IMaxNumberOfFreeDaysCalculator
    {
        Task<int> CalculateNumberOfFreeDaysInARow(string country, int year, HttpClient httpClient);
    }
}
