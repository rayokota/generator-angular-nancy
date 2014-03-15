using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using System;

namespace <%= _.capitalize(baseName) %>.Models
{
    public class CustomDateTimeConverter : IsoDateTimeConverter
    {
        public CustomDateTimeConverter() {
            base.DateTimeFormat = "yyyy-MM-dd";
        }
    }
}

