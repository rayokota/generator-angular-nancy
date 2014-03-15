using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using System;

namespace Myapp
{
    public class CustomDateTimeConverter : IsoDateTimeConverter
    {
        public CustomDateTimeConverter() {
            base.DateTimeFormat = "yyyy-MM-dd";
        }
    }
}

