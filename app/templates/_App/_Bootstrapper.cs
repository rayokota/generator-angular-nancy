using NLog;
using NLog.Config;
using NLog.Targets;
using NLog.Targets.Wrappers;
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
