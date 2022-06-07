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
        public string Type { get; set; }
        public Guid CountryId { get; set; }

    }
}
