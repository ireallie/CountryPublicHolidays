using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CountryPublicHolidays.ServiceLibrary.Entities
{
    public class HolidayFlagEntity
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public IList<HolidayEntity> Holidays { get; set; } = new List<HolidayEntity>();
    }
}
