using ServiceStack.Data;
using ServiceStack.OrmLite;
using ServiceStack.OrmLite.Sqlite;
using System;
using System.Configuration;
using Nancy;
using Nancy.Diagnostics;
using Nancy.Bootstrapper;
using Nancy.TinyIoc;
using Nancy.Conventions;

namespace <%= _.capitalize(baseName) %>
{
    public class Bootstrapper : DefaultNancyBootstrapper
    {
        protected override void ApplicationStartup(TinyIoCContainer container, IPipelines pipelines)
        {
            base.ApplicationStartup(container, pipelines);
            
            StaticConfiguration.DisableErrorTraces = false;
        }

       protected override void ConfigureApplicationContainer(TinyIoCContainer container)
        {
            base.ConfigureApplicationContainer(container);

            var dbFactory = new OrmLiteConnectionFactory("/tmp/my.db", SqliteDialect.Provider);

            CreateDatabase(dbFactory);
            container.Register<IDbConnectionFactory>(dbFactory);
        }

        private void CreateDatabase(OrmLiteConnectionFactory dbFactory)
        {
            <% if (entities.length > 0) { %>
            using (var db = dbFactory.OpenDbConnection())
            {
                <% _.each(entities, function (entity) { %>
                db.CreateTable<<%= _.capitalize(entity.name) %>>(false);<% }); %>
            }
            <% }; %>
        }
    }
}
