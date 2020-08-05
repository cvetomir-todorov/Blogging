namespace CloudDeploy.Processing.Api
{
	public sealed class ProcessDeploymentRequest
	{
		public int ProjectID { get; set; }

		public int DeploymentID { get; set; }
	}
}
