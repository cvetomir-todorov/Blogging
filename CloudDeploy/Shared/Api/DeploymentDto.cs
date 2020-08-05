namespace CloudDeploy.Api
{
	public sealed class DeploymentDto
	{
		public int ID { get; set; }

		public string PackageVersion { get; set; }

		public DeploymentStatusDto Status { get; set; }
	}
}
