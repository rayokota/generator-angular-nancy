using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using System;

namespace <%= _.capitalize(baseName) %>
{
    public class CustomDateTimeConverter : IsoDateTimeConverter
    {
        public CustomDateTimeConverter() {
            base.DateTimeFormat = "yyyy-MM-dd";
        }
    }
}

