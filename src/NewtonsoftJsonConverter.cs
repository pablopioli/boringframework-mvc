using Newtonsoft.Json;
using System;
using System.Globalization;

namespace Boring.Mvc
{
    class NewtonsoftJsonConverter : JsonConverter
    {
        public override bool CanConvert(Type objectType)
        {
            return objectType == typeof(Date);
        }

        public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
        {
            var dateToParse = reader.Value.ToString();

            if (string.IsNullOrEmpty(dateToParse))
            {
                return Date.MinValue;
            }

            var formats = new string[] {
                "yyyy-MM-dd",
                "yyyyMMdd",
                "yyyyMMddTHHmmssZ",
                "yyyyMMddTHHmmZ",
                "yyyyMMddTHHmmss",
                "yyyyMMddTHHmm",
                "yyyyMMddHHmmss",
                "yyyyMMddHHmm",
                "yyyy-MM-ddTHH-mm-ss",
                "yyyy-MM-dd-HH-mm-ss",
                "yyyy-MM-dd-HH-mm"
            };

            DateTime dateAsDateTime;

            foreach (var format in formats)
            {
                if (format.EndsWith("Z") && DateTime.TryParseExact(dateToParse, format, null,
                             DateTimeStyles.AssumeUniversal, out dateAsDateTime))
                {
                    return new Date(dateAsDateTime.Year, dateAsDateTime.Month, dateAsDateTime.Day);
                }

                if (DateTime.TryParseExact(dateToParse, format, null, DateTimeStyles.None, out dateAsDateTime))
                {
                    return new Date(dateAsDateTime.Year, dateAsDateTime.Month, dateAsDateTime.Day);
                }
            }

            throw new JsonException("Invalid date, please use YYYY-MM-DD format");
        }

        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            var date = (Date)value;

            if (date == Date.MinValue)
            {
                serializer.Serialize(writer, "");
            }
            else
            {
                serializer.Serialize(writer, date.ToIsoString());
            }
        }
    }
}
