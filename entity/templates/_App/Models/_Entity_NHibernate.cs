using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using System;
using <%= _.capitalize(baseName) %>.Models.Mappings;

namespace <%= _.capitalize(baseName) %>.Models
{
    public class <%= _.capitalize(name) %> : IMappable
    {
        [JsonProperty(PropertyName = "id")]
        public long? Id { get; set; }
        <% _.each(attrs, function (attr) { %>
        <% if (attr.attrType == 'Enum') { %>[JsonConverter(typeof(StringEnumConverter))]<% } else if (attr.attrType == 'Date') { %>[JsonConverter(typeof(CustomDateTimeConverter))]<% }; %>
        [JsonProperty(PropertyName = "<%= attr.attrName %>")]
        public <% if (attr.attrType == 'Enum') { %><%= _.capitalize(attr.attrName) %><% } %><%= attr.attrImplType %><% if (!attr.required) { %>?<% } %> <%= _.capitalize(attr.attrName) %> { get; set; }<% }); %>
    }
}

