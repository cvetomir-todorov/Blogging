namespace CloudDeploy.Api
{
	public sealed class ProjectDto
	{
		public int ID { get; set; }

		public string Name { get; set; }

		public string PackageSource { get; set; }

		public int DeploymentCount { get; set; }
	}
}
