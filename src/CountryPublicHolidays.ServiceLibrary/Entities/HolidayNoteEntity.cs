using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CountryPublicHolidays.ServiceLibrary.Entities
{
    public class HolidayNoteEntity
    {
        public string Lang { get; set; }
        public string Text { get; set; }
        public Guid HolidayId { get; set; }
    }
}
