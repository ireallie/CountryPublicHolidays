using System;
using System.Collections.Generic;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace CountryPublicHolidays.ServiceLibrary.Entities
{
    public class HolidayTypeEntityConverter : JsonConverter
    {
        public override bool CanConvert(Type objectType)
        {
            return objectType == typeof(string);
        }

        public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
        {
            JToken token = JToken.Load(reader);

            var typelist = token.ToObject<List<string>>();

            List<HolidayTypeEntity> holidayTypeEntities = new List<HolidayTypeEntity>();

            foreach (var holidaytype in typelist)
            {
                holidayTypeEntities.Add(new HolidayTypeEntity() {Id = Guid.NewGuid(), Type = holidaytype});
            }

            return holidayTypeEntities;
        }

        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            JToken t = JToken.FromObject(value);

            if (t.Type != JTokenType.Object)
            {
                t.WriteTo(writer);
            }
        }
    }
}
