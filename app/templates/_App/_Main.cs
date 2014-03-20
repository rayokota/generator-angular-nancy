using System;
using Nancy.Hosting.Self;
using System.Diagnostics;
using System.Threading;

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
                new Uri("http://localhost:8080/"));

            nancyHost.Start();
            
            Console.WriteLine("Nancy now listening at http://localhost:8080/. Press enter to stop");
            ConsoleKeyInfo key = Console.ReadKey();
            if ((int)key.Key == 0) 
            {
                // Mono returns a ConsoleKeyInfo with a Key value of 0 when stdin is redirected
                // See https://bugzilla.xamarin.com/show_bug.cgi?id=12551
                // For now, just sleep, so that we can run in background with nohup
                Thread.Sleep(Timeout.Infinite);
            }
            
            nancyHost.Stop();
            Console.WriteLine("Stopped. Good bye!");
        }
    }
}
