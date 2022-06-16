using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CountryPublicHolidays.ServiceLibrary.Entities
{
    public class CountryHolidayEntity
    {
        public Guid HolidayId { get; set; }
        public Guid CountryId { get; set; }
    }
}
