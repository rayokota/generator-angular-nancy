using System;
using ServiceStack.DataAnnotations;

namespace <%= _.capitalize(baseName) %>
{
    public class <%= _.capitalize(name) %>
    {
        [AutoIncrement]
        public long Id { get; set; }
        <% _.each(attrs, function (attr) { %>
        public <% if (attr.attrType == 'Enum') { %><%= _.capitalize(attr.attrName) %><% } %><%= attr.attrImplType %><% if (!attr.required) { %>?<% } %> <%= _.capitalize(attr.attrName) %> { get; set; }<% }); %>
    }
}

