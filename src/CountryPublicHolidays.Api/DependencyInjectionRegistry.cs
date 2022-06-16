using CountryPublicHolidays.ServiceLibrary.Calculators;
using CountryPublicHolidays.ServiceLibrary.Domains;
using CountryPublicHolidays.ServiceLibrary.Repositories;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;

namespace CountryPublicHolidays.Api
{
    public static class DependencyInjectionRegistry
    {
        public static IServiceCollection AddMyServices(this IServiceCollection services)
        {

            services.AddControllers();

            services.AddSwaggerDocument(config =>
            {
                config.PostProcess = document =>
                {
                    document.Info.Version = "v1";
                    document.Info.Title = "CountryPublicHolidays API";
                    document.Info.Description = "A small WEB API application that returns country public holidays.";
                };
            });


            Uri jsonApiEndpoint = new Uri("https://kayaposoft.com/enrico/json/v2.0");
            HttpClient httpClient = new HttpClient()
            {
                BaseAddress = jsonApiEndpoint
            };

            httpClient.DefaultRequestHeaders.Accept.Add(
            new MediaTypeWithQualityHeaderValue("application/json"));

            ServicePointManager.FindServicePoint(jsonApiEndpoint).ConnectionLeaseTimeout = 60000;


            services.AddSingleton<ICountryRepository, CountryRepository>();
            services.AddSingleton<IRegionRepository, RegionRepository>();
            services.AddSingleton<IHolidayTypeRepository, HolidayTypeRepository>();
            services.AddSingleton<ICountryDomain, CountryDomain>();
            services.AddSingleton<HttpClient>(httpClient);
            services.AddSingleton<IHolidayRepository, HolidayRepository>();
            services.AddSingleton<IHolidayFlagRepository, HolidayFlagRepository>();
            services.AddSingleton<IHolidayNameRepository, HolidayNameRepository>();
            services.AddSingleton<IHolidayNoteRepository, HolidayNoteRepository>();
            services.AddSingleton<IHolidayDomain, HolidayDomain>();
            services.AddSingleton<IMaxNumberOfFreeDaysCalculator, MaxNumberOfFreeDaysCalculator>();
            services.AddSingleton<ICountryHolidayTypeRepository, CountryHolidayTypeRepository>();
            services.AddSingleton<IHolidayHolidayFlagRepository, HolidayHolidayFlagRepository>();
            services.AddSingleton<ICountryHolidayRepository, CountryHolidayRepository>();

            return services;
        }
    }
}
