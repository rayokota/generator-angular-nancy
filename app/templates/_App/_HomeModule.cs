using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using Nancy;

namespace Myapp
{
    public class HomeModule : Nancy.NancyModule
    {
        public HomeModule()
        {
            Get["/"] = parameters =>
            {
                return Response.AsRedirect("Content/index.html");
            };

        }
    }
}

