using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using System;
<% if (orm == 'NHibernate') { %>using <%= _.capitalize(baseName) %>.Models.Mappings;
<% } else { %>using ServiceStack.DataAnnotations;<% }; %>

namespace <%= _.capitalize(baseName) %>.Models
{
    public class <%= _.capitalize(name) %><% if (orm == 'NHibernate') { %> : IMappable<% }; %>
    {
        <% if (orm == 'OrmLite') { %>[AutoIncrement]<% }; %>
        [JsonProperty(PropertyName = "id")]
        public virtual long? Id { get; set; }
        <% _.each(attrs, function (attr) { %>
        <% if (attr.attrType == 'Enum') { %>[JsonConverter(typeof(StringEnumConverter))]<% } else if (attr.attrType == 'Date') { %>[JsonConverter(typeof(CustomDateTimeConverter))]<% }; %>
        [JsonProperty(PropertyName = "<%= attr.attrName %>")]
        public virtual <% if (attr.attrType == 'Enum') { %><%= _.capitalize(attr.attrName) %><% } %><%= attr.attrImplType %><% if (!attr.required) { %>?<% } %> <%= _.capitalize(attr.attrName) %> { get; set; }<% }); %>
    }
}

