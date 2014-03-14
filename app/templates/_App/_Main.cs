using System;
using Nancy.Hosting.Self;
using System.Diagnostics;

namespace <%= _.capitalize(baseName) %>
{
    class MainClass
    {
        public static void Main (string[] args)
        {
            var hostConfiguration = new HostConfiguration
            {
                UrlReservations = new UrlReservations() { CreateAutomatically = true }
            };

            var nancyHost = new NancyHost(hostConfiguration,
                new Uri("http://127.0.0.1:8080/"));

            nancyHost.Start();
            
            Console.WriteLine("Nancy now listening - navigating to http://127.0.0.1:8080/. Press enter to stop");
            Process.Start("http://127.0.01:8080/");
            Console.ReadKey();
            
            nancyHost.Stop();
            
            Console.WriteLine("Stopped. Good bye!");
        }
    }
}
