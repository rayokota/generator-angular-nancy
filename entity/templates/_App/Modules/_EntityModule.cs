using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using Nancy;
using Nancy.ModelBinding;
using ServiceStack.Data;
<% if (orm == 'NHibernate') { %>using NHibernate;
<% } else { %>using ServiceStack.OrmLite;<% }; %>
using System.Data;
using <%= _.capitalize(baseName) %>.Models;

namespace <%= _.capitalize(baseName) %>.Modules
{
    public class <%= _.capitalize(name) %>Module : Nancy.NancyModule
    {
        <% if (orm == 'NHibernate') { %>
        private readonly ISession _session;

        public <%= _.capitalize(name) %>Module(ISession session)
            : this()
        {
            _session = session;
        }

        public <%= _.capitalize(name) %>Module()
            : base("/<%= baseName %>/<%= pluralize(name) %>")
        {
            Get["/"] = parameters =>
            {
                IList<<%= _.capitalize(name) %>> rows = null;
                using (var tx = _session.BeginTransaction())
                {
                    rows = _session.CreateCriteria(typeof(<%= _.capitalize(name) %>)).List<<%= _.capitalize(name) %>>();
                    tx.Commit();
                }
                return Response.AsJson(rows);
            };

            Get["/{id}"] = parameters =>
            {
                <%= _.capitalize(name) %> row = null;
                long rowId = parameters.id;
                using (var tx = _session.BeginTransaction())
                {
                    row = (<%= _.capitalize(name) %>)_session.Get(typeof(<%= _.capitalize(name) %>), rowId);
                    if (row == null)
                    {
                        return HttpStatusCode.NotFound;
                    }
                    tx.Commit();
                }
                return Response.AsJson(row);
            };

            Post["/"] = parameters =>
            {
                <%= _.capitalize(name) %> row = this.Bind<<%= _.capitalize(name) %>>();
                using (var tx = _session.BeginTransaction())
                {
                    _session.SaveOrUpdate(row);
                    tx.Commit();
                }
                return Response.AsJson(row, HttpStatusCode.Created);
            };

            Put["/{id}"] = parameters =>
            {
                <%= _.capitalize(name) %> row = this.Bind<<%= _.capitalize(name) %>>();
                row.Id = parameters.id;
                using (var tx = _session.BeginTransaction())
                {
                    <%= _.capitalize(name) %> oldRow = (<%= _.capitalize(name) %>)_session.Get(typeof(<%= _.capitalize(name) %>), row.Id);
                    if (oldRow == null)
                    {
                        return HttpStatusCode.NotFound;
                    }
                    _session.Merge(row);
                    tx.Commit();
                }
                return Response.AsJson(row);
            };

            Delete["/{id}"] = parameters =>
            {
                long rowId = parameters.id;
                using (var tx = _session.BeginTransaction())
                {
                    <%= _.capitalize(name) %> oldRow = (<%= _.capitalize(name) %>)_session.Get(typeof(<%= _.capitalize(name) %>), rowId);
                    if (oldRow == null)
                    {
                        return HttpStatusCode.NotFound;
                    }
                    _session.Delete(oldRow);
                    tx.Commit();
                }
                return HttpStatusCode.NoContent;
            };
        }
        <% } else { %>
        private readonly IDbConnectionFactory _dbFactory;

        public <%= _.capitalize(name) %>Module(IDbConnectionFactory dbFactory)
            : this()
        {
            _dbFactory = dbFactory;
        }

        public <%= _.capitalize(name) %>Module()
            : base("/<%= baseName %>/<%= pluralize(name) %>")
        {
            Get["/"] = parameters =>
            {
                List<<%= _.capitalize(name) %>> rows = null;
                using (IDbConnection db = _dbFactory.OpenDbConnection()) 
                {
                    rows = db.Select<<%= _.capitalize(name) %>>();
                }
                return Response.AsJson(rows);
            };

            Get["/{id}"] = parameters =>
            {
                <%= _.capitalize(name) %> row = null;
                long rowId = parameters.id;
                using (IDbConnection db = _dbFactory.OpenDbConnection()) 
                {
                    row = db.Single<<%= _.capitalize(name) %>>(r => r.Id == rowId);
                    if (row == null) 
                    {
                        return HttpStatusCode.NotFound;
                    }
                }
                return Response.AsJson(row);
            };

            Post["/"] = parameters =>
            {
                <%= _.capitalize(name) %> row = this.Bind<<%= _.capitalize(name) %>>();
                using (IDbConnection db = _dbFactory.OpenDbConnection())
                {
                    db.Insert(row);
                    row.Id = db.LastInsertId();
                }
                return Response.AsJson(row, HttpStatusCode.Created);
            };

            Put["/{id}"] = parameters =>
            {
                <%= _.capitalize(name) %> row = this.Bind<<%= _.capitalize(name) %>>();
                row.Id = parameters.id;
                using (IDbConnection db = _dbFactory.OpenDbConnection())
                {
                    <%= _.capitalize(name) %> oldRow = db.Single<<%= _.capitalize(name) %>>(r => r.Id == row.Id);
                    if (oldRow == null) 
                    {
                        return HttpStatusCode.NotFound;
                    }
                    db.Update(row);
                }
                return Response.AsJson(row);
            };

            Delete["/{id}"] = parameters =>
            {
                long rowId = parameters.id;
                using (IDbConnection db = _dbFactory.OpenDbConnection())
                {
                    db.Delete<<%= _.capitalize(name) %>>(r => r.Id == rowId);
                }
                return HttpStatusCode.NoContent;
            };
        }
    <% }; %>
    }
}

