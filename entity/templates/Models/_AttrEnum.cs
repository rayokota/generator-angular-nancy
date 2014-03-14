using System;

namespace <%= _.capitaiize(baseName) >%
{
    public enum <%= _.capitalize(attr.attrName) %>Enum
    {
        <% var delim = ''; _.each(attr.enumValues, function (value) { %><%= delim %><%= _.capitalize(value) %><% delim = ', '; }); %>
    }
}
