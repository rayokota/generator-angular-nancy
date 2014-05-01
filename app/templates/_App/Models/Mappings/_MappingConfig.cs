using FluentNHibernate.Automapping;
using System;

namespace <%= _.capitalize(baseName) %>.Models.Mappings
{
    public class MappingConfig : DefaultAutomappingConfiguration
    {
        public override bool ShouldMap(Type type)
        {
            return typeof(IMappable).IsAssignableFrom(type);
        }
    }
}
