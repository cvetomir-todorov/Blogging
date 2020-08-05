using CloudDeploy.Api;

namespace CloudDeploy.Management.App.Api.Server
{
	public sealed class UpdateServerRequest
	{
		public CloudProviderDto Cloud { get; set; }

		public string Address { get; set; }
	}
}
