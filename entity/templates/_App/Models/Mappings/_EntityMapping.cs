using FluentNHibernate.Automapping;
using FluentNHibernate.Automapping.Alterations;
using System;

namespace <%= _.capitalize(baseName) %>.Models.Mappings
{
    public class <%= _.capitalize(name) %>Mapping : IAutoMappingOverride<<%= _.capitalize(name) %>>
    {
        public void Override(AutoMapping<<%= _.capitalize(name) %>> mapping)
        {
            mapping.Table("<%= _.capitalize(pluralize(name)) %>");

            mapping.Id(x => x.Id, "Id").GeneratedBy.Identity();
        }
    }
}

