using NLog;
using NLog.Config;
using NLog.Targets;
using NLog.Targets.Wrappers;
<% if (orm == 'NHibernate') { %>using FluentNHibernate.Automapping;
using FluentNHibernate.Cfg;
using FluentNHibernate.Cfg.Db;
using NHibernate;
using NHibernate.Cfg;
using NHibernate.Tool.hbm2ddl;<% }; %>
using ServiceStack.Data;
<% if (orm == 'OrmLite') { %>using ServiceStack.OrmLite;
using ServiceStack.OrmLite.Sqlite;<% }; %>
using System;
using System.Configuration;
using Nancy;
using Nancy.Diagnostics;
using Nancy.Bootstrapper;
using Nancy.TinyIoc;
using Nancy.Conventions;
using <%= _.capitalize(baseName) %>.Models;
<% if (orm == 'NHibernate') { %>using <%= _.capitalize(baseName) %>.Models.Mappings;
<% }; %>

namespace <%= _.capitalize(baseName) %>
{
    public class Bootstrapper : DefaultNancyBootstrapper
    {
        private Logger log = LogManager.GetLogger("RequestLogger");

        protected override void ApplicationStartup(TinyIoCContainer container, IPipelines pipelines)
        {
            base.ApplicationStartup(container, pipelines);
            
            StaticConfiguration.DisableErrorTraces = false;

            LogAllRequests(pipelines);
            LogAllResponseCodes(pipelines);
            LogUnhandledExceptions(pipelines);
        }

        private void LogAllRequests(IPipelines pipelines)
        {
          pipelines.BeforeRequest += ctx =>
          {
            log.Info("Handling request {0} \"{1}\"", ctx.Request.Method, ctx.Request.Path);
            return null;
          };
        }

        private void LogAllResponseCodes(IPipelines pipelines)
        {
          pipelines.AfterRequest += ctx =>
            log.Info("Responding {0} to {1} \"{2}\"", ctx.Response.StatusCode, ctx.Request.Method, ctx.Request.Path);
        }

        private void LogUnhandledExceptions(IPipelines pipelines)
        {
          pipelines.OnError.AddItemToStartOfPipeline((ctx, err) =>
          {
            log.ErrorException(string.Format("Request {0} \"{1}\" failed", ctx.Request.Method, ctx.Request.Path), err);
            return null;
          });
        }

        <% if (orm == 'NHibernate') { %> 
        protected override void ConfigureApplicationContainer(TinyIoCContainer container)
        {
            base.ConfigureApplicationContainer(container);

            <% if (entities.length > 0) { %>
            container.Register<ISessionFactory>(CreateSessionFactory());<% }; %>
        }

        protected override void ConfigureRequestContainer(TinyIoCContainer container, NancyContext context)
        {
            base.ConfigureRequestContainer(container, context);

            <% if (entities.length > 0) { %>
            container.Register<ISession>(container.Resolve<ISessionFactory>().OpenSession());<% }; %>
        }

        <% if (entities.length > 0) { %>
        private static ISessionFactory CreateSessionFactory()
        {
            return Fluently
               .Configure()
               .Database(SQLiteConfiguration
                   .Standard
                   <% if (platform == 'Mono') { %>.Driver<MonoSqliteDriver>()<% }; %>
                   .UsingFile("my.db"))
               .Mappings(m => {
                   <% _.each(entities, function(entity) { %>
                   m.AutoMappings.Add(AutoMap.AssemblyOf<<%= _.capitalize(entity.name) %>>(new MappingConfig()).UseOverridesFromAssemblyOf<<%= _.capitalize(entity.name) %>Mapping>());<% }); %>
                   }
               )
               .ExposeConfiguration(BuildSchema)
               .BuildSessionFactory();
        }

        private static void BuildSchema(Configuration config)
        {
            // this NHibernate tool takes a configuration (with mapping info in)
            // and exports a database schema from it
            new SchemaUpdate(config)
                .Execute(false, true);
        }
        <% }; %>
        <% } else { %>
        protected override void ConfigureApplicationContainer(TinyIoCContainer container)
        {
            base.ConfigureApplicationContainer(container);

            var dbFactory = new OrmLiteConnectionFactory("my.db", SqliteDialect.Provider);

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
        <% }; %>
    }

    <% if (orm == 'NHibernate' && platform == 'Mono') { %>
    public class MonoSqliteDriver : NHibernate.Driver.ReflectionBasedDriver
    {
        public MonoSqliteDriver()
            : base(
            "Mono.Data.Sqlite",
            "Mono.Data.Sqlite",
            "Mono.Data.Sqlite.SqliteConnection",
            "Mono.Data.Sqlite.SqliteCommand")
        {
        }

        public override bool UseNamedPrefixInParameter {
            get {
                return true;
            }
        }

        public override bool UseNamedPrefixInSql {
            get {
                return true;
            }
        }

        public override string NamedPrefix {
            get {
                return "@";
            }
        }

        public override bool SupportsMultipleOpenReaders {
            get {
                return false;
            }
        }
    }
    <% }; %>
}
