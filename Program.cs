using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

namespace dotnet_helloworld
{
    public class Program
    {
        public static  IConfigurationRoot GetServerUrlsFromCommandLine(string[] args)
        {
            var config = new ConfigurationBuilder()
                .AddCommandLine(args)
                .Build();
            var serverport = config.GetValue<int?>("port") ?? 5000;
            var serverurls = config.GetValue<string>("server.urls") ?? string.Format("http://*:{0}", serverport);
            var configfile = config.GetValue<string>("config") ?? "config.txt";

            var configDictionary = new Dictionary<string, string>
            {
                {"server.urls", serverurls},
                {"port", serverport.ToString()}
            };

            var builder = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile(configfile);
        
            var builder_config = builder.Build();
            Console.WriteLine($"options1 = {builder_config["options1"]}");
            Console.WriteLine($"options2 = {builder_config["options2"]}");
        
            return new ConfigurationBuilder()
                .AddCommandLine(args)
                .AddInMemoryCollection(configDictionary)
                .Build(); 
        }

        public static void Main(string[] args = null)
        {
            var config = GetServerUrlsFromCommandLine(args);
            BuildWebHost(args, config).Run();
        }

        public static IWebHost BuildWebHost(string[] args, IConfigurationRoot config) =>
            WebHost.CreateDefaultBuilder(args)
                .UseConfiguration(config)
                .UseStartup<Startup>()
                .Build();
    }

}