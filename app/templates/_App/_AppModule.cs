using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using Nancy;
using Nancy.ModelBinding;
using ServiceStack.Data;
using ServiceStack.OrmLite;
using System.Data;

namespace <%= _.capitalize(baseName) %>
{
    public class <%= _.capitalize(baseName) %>Module : Nancy.NancyModule
    {
        private readonly IDbConnectionFactory _dbFactory;

        public <%= _.capitalize(baseName) %>Module(IDbConnectionFactory dbFactory)
            : this()
        {
            _dbFactory = dbFactory;
        }

        public <%= _.capitalize(baseName) %>Module()
            : base("/<%= baseName %>")
        {
            <% _.each(entities, function (entity) { %>
            Get["/<%= pluralize(entity.name) %>"] = parameters =>
            {
                List<<%= _.capitalize(entity.name) %>> rows = null;
                using (IDbConnection db = _dbFactory.OpenDbConnection()) 
                {
                    rows = db.Select<<%= _.capitalize(entity.name) %>>();
                    return Response.AsJson(rows);
                }
            };

            Get["/<%= pluralize(entity.name) %>/{id}"] = parameters =>
            {
                <%= _.capitalize(entity.name) %> row = null;
                long rowId = parameters.id;
                using (IDbConnection db = _dbFactory.OpenDbConnection()) 
                {
                    row = db.Single<<%= _.capitalize(entity.name) %>>(r => r.Id == rowId);
                    if (row == null) 
                    {
                        return HttpStatusCode.NotFound;
                    }
                }
                return Response.AsJson(row);
            };

            Post["/<%= pluralize(entity.name) %>"] = parameters =>
            {
                <%= _.capitalize(entity.name) %> row = this.Bind<<%= _.capitalize(entity.name) %>>();
                using (IDbConnection db = _dbFactory.OpenDbConnection())
                {
                    db.Insert(row);
                    row.Id = db.LastInsertId();
                }
                return Response.AsJson(row, HttpStatusCode.Created);
            };

            Put["/<%= pluralize(entity.name) %>/{id}"] = parameters =>
            {
                <%= _.capitalize(entity.name) %> row = this.Bind<<%= _.capitalize(entity.name) %>>();
                row.Id = parameters.id;
                using (IDbConnection db = _dbFactory.OpenDbConnection())
                {
                    <%= _.capitalize(entity.name) %> oldRow = db.Single<<%= _.capitalize(entity.name) %>>(r => r.Id == row.Id);
                    if (oldRow == null) 
                    {
                        return HttpStatusCode.NotFound;
                    }
                    db.Update(row);
                }
                return Response.AsJson(row);
            };

            Delete["/<%= pluralize(entity.name) %>/{id}"] = parameters =>
            {
                long rowId = parameters.id;
                using (IDbConnection db = _dbFactory.OpenDbConnection())
                {
                    db.Delete<<%= _.capitalize(entity.name) %>>(r => r.Id == rowId);
                }
                return HttpStatusCode.NoContent;
            };

            <% }); %>
        }
    }
}

