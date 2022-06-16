using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CountryPublicHolidays.ServiceLibrary.Entities
{
    public class RegionEntity
    {
        public string Region { get; set; }
        public Guid CountryId { get; set; }
    }
}
