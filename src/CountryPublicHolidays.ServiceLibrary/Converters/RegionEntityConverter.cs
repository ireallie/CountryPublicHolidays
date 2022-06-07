using System;
using System.Collections.Generic;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace CountryPublicHolidays.ServiceLibrary.Entities
{
    public class RegionEntityConverter : JsonConverter
    {
        public override bool CanConvert(Type objectType)
        {
            return objectType == typeof(string);
        }

        public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
        {
            JToken token = JToken.Load(reader);

            var regionList = token.ToObject<List<string>>();

            List<RegionEntity> holidayTypeEntities = new List<RegionEntity>();

            foreach (var region in regionList)
            {
                holidayTypeEntities.Add(new RegionEntity() { Region = region });
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
