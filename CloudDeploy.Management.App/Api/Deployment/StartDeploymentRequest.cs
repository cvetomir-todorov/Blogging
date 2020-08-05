using CloudDeploy.Api;
using System.Collections.Generic;

namespace CloudDeploy.Management.App.Api.Deployment
{
	public sealed class StartDeploymentRequest
	{
		public DeploymentDto Deployment { get; set; }

		public int ProjectID { get; set; }

		public List<int> ServerIDs { get; set; } = new List<int>();
	}
}
