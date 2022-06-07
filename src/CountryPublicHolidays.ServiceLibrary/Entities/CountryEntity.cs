using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace CountryPublicHolidays.ServiceLibrary.Entities
{
    public class CountryEntity
    {
        public Guid Id { get; set; }
        public string CountryCode { get; set; }
        public string FullName { get; set; }

        [JsonConverter(typeof(DateConverter))]
        public DateTime FromDate { get; set; }

        [JsonConverter(typeof(DateConverter))]
        public DateTime ToDate { get; set; }

        private IList<RegionEntity> _regions;

        [JsonConverter(typeof(RegionEntityConverter))]
        public IList<RegionEntity> Regions
        {
            get => _regions;
            set
            {
                if (value != _regions)
                {
                    _regions = value;

                    foreach (var regionEntity in _regions)
                    {
                        regionEntity.CountryId = Id;
                    }
                }
            }
        }

        private IList<HolidayTypeEntity> _holidayTypes;

        [JsonConverter(typeof(HolidayTypeEntityConverter))]

        public IList<HolidayTypeEntity> HolidayTypes 
        {   
            get => _holidayTypes;
            set
            {
                if (value != _holidayTypes)
                {
                    _holidayTypes = value;

                    foreach (var holidayTypeEntity in _holidayTypes)
                    {
                        holidayTypeEntity.CountryId = Id;
                    }
                }
            }
        }

        public CountryEntity()
        {
            Id = Guid.NewGuid();
        }
    }
}
