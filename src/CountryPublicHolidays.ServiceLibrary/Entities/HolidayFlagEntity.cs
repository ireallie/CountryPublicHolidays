using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CountryPublicHolidays.ServiceLibrary.Entities
{
    public class HolidayFlagEntity
    {
        public string Type { get; set; }
        public Guid HolidayId { get; set; }
    }
}
