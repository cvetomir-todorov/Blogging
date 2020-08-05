using CloudDeploy.Management.Data.Entities;
using Microsoft.EntityFrameworkCore;

namespace CloudDeploy.Management.Data
{
	public class ManagementDbContext : DbContext
	{
		public ManagementDbContext(DbContextOptions<ManagementDbContext> dbContextOptions) : base(dbContextOptions)
		{}

		public DbSet<ProjectEntity> Projects { get; set; }

		public DbSet<DeploymentEntity> Deployments { get; set; }

		public DbSet<ServerEntity> Servers { get; set; }
	}
}
