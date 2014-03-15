using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using Nancy;

namespace <%= _.capitalize(baseName) %>.Modules
{
    public class IndexModule : Nancy.NancyModule
    {
        public IndexModule()
        {
            Get["/"] = parameters =>
            {
                return Response.AsRedirect("Content/index.html");
            };

        }
    }
}

