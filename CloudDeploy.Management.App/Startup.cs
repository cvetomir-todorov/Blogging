using CloudDeploy.Management.Data;
using CloudDeploy.Web;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.OpenApi.Models;

namespace CloudDeploy.Management.App
{
	public class Startup
	{
		public Startup(IConfiguration configuration)
		{
			Configuration = configuration;
		}

		public IConfiguration Configuration { get; }

		public void ConfigureServices(IServiceCollection services)
		{
			services.AddDbContext<ManagementDbContext>(contextOptions =>
			{
				contextOptions.UseSqlServer(Configuration.GetConnectionString("ManagementDB"),
					sqlOptions => sqlOptions.MigrationsAssembly(typeof(ManagementDbContext).Assembly.FullName));
			});
			services.AddControllers();
			services.AddHttpClient();
			services.AddSwaggerGen(swagger =>
			{
				swagger.SwaggerDoc("v1", new OpenApiInfo
				{
					Version = "v1",
					Title = "CloudDeploy Management API"
				});
			});
			services.AddSingleton<IMvcResultUtility, MvcResultUtility>();
		}

		public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
		{
			using (IServiceScope scope = app.ApplicationServices.CreateScope())
			{
				ManagementDbContext managementDbContext = scope.ServiceProvider.GetRequiredService<ManagementDbContext>();
				managementDbContext.Database.Migrate();
			}

			if (env.IsDevelopment())
			{
				app.UseDeveloperExceptionPage();
			}

			app.UseHttpsRedirection();
			app.UseSwagger();
			app.UseSwaggerUI(swagger =>
			{
				swagger.SwaggerEndpoint("/swagger/v1/swagger.json", "Cloud Deploy Management API");
			});
			app.UseRouting();
			app.UseAuthorization();
			app.UseEndpoints(endpoints =>
			{
				endpoints.MapControllers();
			});
		}
	}
}
