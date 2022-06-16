using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json.Linq;

namespace CountryPublicHolidays.ServiceLibrary.Entities
{
    public class HolidayTypeEntity
    {
        public Guid Id { get; set; }
        public string Type { get; set; }
        public IList<CountryEntity> Countries { get; set; } = new List<CountryEntity>();
        public IList<HolidayEntity> Holidays { get; set; } = new List<HolidayEntity>();
    }
}
