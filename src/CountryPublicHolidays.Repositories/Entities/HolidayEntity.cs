using CountryPublicHolidays.ServiceLibrary.EntityConverters;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CountryPublicHolidays.ServiceLibrary.Entities
{
    [JsonConverter(typeof(HolidayEntityConverter), DateParseHandling.None)]
    public class HolidayEntity
    {
        public Guid Id { get; set; }

        public DateTime Date { get; set; }

        public DateTime? DateTo { get; set; }

        public DateTime? ObservedOn { get; set; }

        //private IList<HolidayFlagEntity> _flags;

        public IList<HolidayFlagEntity> Flags { get; set; } = new List<HolidayFlagEntity>();
        //{
        //    get => _flags;
        //    set
        //    {
        //        if (value != _flags)
        //        {
        //            _flags = value;

        //            foreach (var flag in _flags)
        //            {
        //                flag.HolidayId = Id;
        //            }
        //        }
        //    }
        //}

        private IList<HolidayNameEntity> _name;

        public IList<HolidayNameEntity> Name
        {
            get => _name;
            set
            {
                if (value != _name)
                {
                    _name = value;

                    foreach (var name in _name)
                    {
                        name.HolidayId = Id;
                    }
                }
            }
        }

        private IList<HolidayNoteEntity> _note;

        public IList<HolidayNoteEntity> Note
        {
            get => _note;
            set
            {
                if (value != _note)
                {
                    _note = value;

                    foreach (var note in _note)
                    {
                        note.HolidayId = Id;
                    }
                }
            }
        }

        //public IList<HolidayTypeEntity> HolidayTypes { get; set; } = new List<HolidayTypeEntity>();

        //public Guid CountryId { get; set; }
        public HolidayTypeEntity HolidayType { get; set; }

        public HolidayEntity()
        {
            Id = Guid.NewGuid();
        }
    }
}
