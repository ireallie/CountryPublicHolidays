using CountryPublicHolidays.ServiceLibrary.Entities;
using CountryPublicHolidays.ServiceLibrary.Repositories;
using Microsoft.Extensions.Configuration;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Transactions;
using Xunit;

namespace CountryPublicHolidays.ServiceLibrary.Tests.Repositories
{
    public class CountryRepositoryTests : IClassFixture<DatabaseFixture>
    {
        private bool _commitToDatabase = false;
        private readonly DatabaseFixture _databaseFixture;

        public CountryRepositoryTests(DatabaseFixture databaseFixture)
        {
            _databaseFixture = databaseFixture;
        }

        public CountryRepository Setup()
        {
            var configurationSectionMock = new Mock<IConfigurationSection>();
            configurationSectionMock
                .SetupGet(m => m[It.Is<string>(s => s == "MainDatabase")])
                .Returns("Data Source=host.docker.internal,5050; Initial Catalog=CountryPublicHolidays;User Id=sa;Password=P@ssword123;MultipleActiveResultSets=true");

            var configurationMock = new Mock<IConfiguration>();
            configurationMock
                .Setup(a => a.GetSection(It.Is<string>(s => s == "ConnectionStrings")))
                .Returns(configurationSectionMock.Object);

            var countryRepository = new CountryRepository(configurationMock.Object, new RegionRepository(), new HolidayTypeRepository());
            return countryRepository;
        }

        [Fact]
        [Trait("Category", "Database")]
        public async Task InsertAsync_Success()
        {
            var countryRepository = Setup();

            using (var scope = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled))
            {
                var countryId = Guid.NewGuid();
                var rowsAffected = await countryRepository.InsertAsync(new CountryEntity()
                {
                    Id = countryId,
                    CountryCode = "MNE",
                    FullName = "Montenegro",
                    FromDate = DateTime.Now,
                    ToDate = DateTime.Now,
                    Regions = new List<RegionEntity>() 
                    { 
                        new RegionEntity()
                        {
                            Region = "Andrijevica",
                            CountryId = countryId
                        }
                    },
                    HolidayTypes = new List<HolidayTypeEntity>()
                    {
                        new HolidayTypeEntity()
                        {
                            Type = "public_holiday",
                            CountryId = countryId
                        }
                    }
                });;

                if(_commitToDatabase)
                {
                    scope.Complete();
                }

                Assert.Equal(3, rowsAffected);
            }               
        }

        [Fact]
        [Trait("Category", "Database")]
        public async Task GetAsync_Success()
        {
            var countryRepository = Setup();

            var countries = await countryRepository.GetAsync();

            Assert.NotNull(countries);
            Assert.True(countries.Count() >= 1);
        }
    }
}
