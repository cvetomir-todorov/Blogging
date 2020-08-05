using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Hosting;

namespace CloudDeploy.Management.App
{
	public class Program
	{
		public static void Main(string[] args)
		{
			IHostBuilder hostBuilder = CreateHostBuilder(args);
			IHost host = hostBuilder.Build();
			host.Run();
		}

		public static IHostBuilder CreateHostBuilder(string[] args)
		{
			return Host
				.CreateDefaultBuilder(args)
				.ConfigureWebHostDefaults(webBuilder =>
				{
					webBuilder.UseStartup<Startup>();
				});
		}
	}
}
