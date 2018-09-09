using Hangfire;
using System;

namespace HangfireServer
{
    class Program
    {
        static void Main(string[] args)
        {
            const string CONNECTION_STRING = "";

            GlobalConfiguration.Configuration.UseSqlServerStorage(CONNECTION_STRING);

            using (var server = new BackgroundJobServer())
            {
                Console.WriteLine("Hangfire Server started.");
                Console.WriteLine($"{nameof(Environment.MachineName)}: {Environment.MachineName}");
                Console.WriteLine("\nPress any key to exit...");
                Console.ReadKey();
            }
        }
    }
}
