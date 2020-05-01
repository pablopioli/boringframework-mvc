using System;
using System.Globalization;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace Boring.Mvc
{
    public class DateJsonConverter : JsonConverter<Date>
    {
        public override bool CanConvert(Type typeToConvert)
        {
            return typeToConvert == typeof(Date);
        }

        public override Date Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        {
            var dateToParse = reader.GetString();

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

        public override void Write(Utf8JsonWriter writer, Date value, JsonSerializerOptions options)
        {
            if (value == Date.MinValue)
            {
                writer.WriteStringValue("");
            }
            else
            {
                writer.WriteStringValue(value.ToIsoString());
            }
        }
    }
}
