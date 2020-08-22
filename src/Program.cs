using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Chat
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var host = CreateWebHostBuilder(args)
				.UseKestrel()
				.UseUrls("http://0.0.0.0:" + Environment.GetEnvironmentVariable("PORT"))
				.Build();

		    using(var scope = host.Services.CreateScope())
		    {
		    }
		    
		    host.Run();
        }

        public static IWebHostBuilder CreateWebHostBuilder(string[] args) =>
            WebHost.CreateDefaultBuilder(args)
	            .ConfigureAppConfiguration(config =>
	            {
		            config.AddEnvironmentVariables();
	            })
                .UseStartup<Startup>();
    }
}
