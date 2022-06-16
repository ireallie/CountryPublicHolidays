using System;
using System.Linq;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace CountryPublicHolidays.ServiceLibrary.Entities
{
    public class DateConverter : JsonConverter
    {
        private readonly Type[] _types;

        public DateConverter()
        {

        }
        public DateConverter(params Type[] types)
        {
            _types = types;
        }
        public override bool CanConvert(Type objectType)
        {
            return _types.Any(t => t == objectType);
        }

        public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
        {
            JToken token = JToken.Load(reader);

            int day = (int)token.SelectToken("day").ToObject(typeof(int));
            int month = (int)token.SelectToken("month").ToObject(typeof(int));
            int year = (int)token.SelectToken("year").ToObject(typeof(int));

            if(year > 9999)
            {
                year = 9999;
            }

            return new DateTime(year, month, day);
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
