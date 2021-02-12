using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using System.Globalization;

namespace ASP_API.AppLibs
{
    public class MinDateTimeConverter : DateTimeConverterBase
    {
        public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
        {

            if (reader.Value == null)
                return (DateTime?)reader.Value;
            else
            {
                //return (DateTime)reader.Value;
                return DateTime.ParseExact(reader.Value + " 00:00:00 AM",
                                        "yyyy-MM-dd h:mm:ss tt",
                                         CultureInfo.InvariantCulture);
            }
        }

        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            DateTime dateTimeValue = (DateTime)value;
            if (dateTimeValue == DateTime.MinValue)
            {
                writer.WriteNull();
                return;
            }

            writer.WriteValue(value);
        }
    }
}