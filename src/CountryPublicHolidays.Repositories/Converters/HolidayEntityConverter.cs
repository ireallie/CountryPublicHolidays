using CountryPublicHolidays.ServiceLibrary.Entities;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CountryPublicHolidays.ServiceLibrary.EntityConverters
{
    public class HolidayEntityConverter : JsonConverter
    {
        private readonly DateParseHandling _dateParseHandling;

        public HolidayEntityConverter(DateParseHandling dateParseHandling)
        {
            _dateParseHandling = dateParseHandling;
        }
        public override bool CanConvert(Type objectType)
        {
            throw new NotImplementedException();
        }

        public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
        {
            JToken token = JToken.Load(reader);

            var holidayEntity = new HolidayEntity();

            JToken date = token.SelectToken("date");
            holidayEntity.Date = JsonConvert.DeserializeObject<DateTime>(date.ToString(), new DateConverter(typeof(DateTime)));

            JToken dateTo = token["dateTo"];
            if (dateTo != null)
            {
                holidayEntity.DateTo = JsonConvert.DeserializeObject<DateTime>(dateTo.ToString(), new DateConverter(typeof(DateTime)));
            }

            JToken observedOn = token["observedOn"];
            if (observedOn != null)
            {
                holidayEntity.ObservedOn = JsonConvert.DeserializeObject<DateTime>(observedOn.ToString(), new DateConverter(typeof(DateTime)));
            }

            holidayEntity.Name = token.SelectToken("name").ToObject<List<HolidayNameEntity>>();

            JToken note = token["note"];
            if(note != null)
            {
                holidayEntity.Note = note.ToObject<List<HolidayNoteEntity>>();
            }

            JToken flags = token["flags"];
            if(flags != null)
            {
                var flagsList = flags.ToObject<List<string>>();
                var holidayFlagsEntities = new List<HolidayFlagEntity>();

                foreach (var flag in flagsList)
                {
                    holidayFlagsEntities.Add(new HolidayFlagEntity() { Id = Guid.NewGuid(), Name = flag });
                }

                holidayEntity.Flags = holidayFlagsEntities;
            }

            JToken holidayType = token["holidayType"];
            if (holidayType != null)
            {
                //var holidayTypeEntity = new HolidayTypeEntity();
                //holidayEntity.HolidayType = holidayTypeEntity;
                //holidayEntity.HolidayType.Type = (string)holidayType.ToObject(typeof(string));
                holidayEntity.HolidayType = new HolidayTypeEntity() { Type = (string)holidayType.ToObject(typeof(string)) };
                //var holidayTypesList = holidayTypes.ToObject<List<string>>();
                //var holidayTypesEntities = new List<HolidayTypeEntity>();

                //foreach (var holidayType in holidayTypesList)
                //{
                //    holidayTypesEntities.Add(new HolidayTypeEntity() { Id = Guid.NewGuid(), Type = holidayType });
                //}
            }

            return holidayEntity;
        }
        public override bool CanWrite { get { return false; } }

        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            throw new NotImplementedException();
        }
    }
}
