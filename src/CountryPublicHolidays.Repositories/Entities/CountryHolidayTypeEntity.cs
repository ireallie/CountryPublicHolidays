using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CountryPublicHolidays.ServiceLibrary.Entities
{
    public class CountryHolidayTypeEntity
    {
        public Guid CountryId { get; set; }
        public Guid HolidayTypeId { get; set; }
    }
}
