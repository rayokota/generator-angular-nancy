using System;

namespace <%= _.capitalize(baseName) %>.Models
{
    public enum <%= _.capitalize(attr.attrName) %>Enum
    {
        <% var delim = ''; _.each(attr.enumValues, function (value) { %><%= delim %><%= value %><% delim = ', '; }); %>
    }
}
