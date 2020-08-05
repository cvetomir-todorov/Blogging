namespace CloudDeploy.Api
{
	public sealed class ServerDto
	{
		public int ID { get; set; }

		public CloudProviderDto Cloud { get; set; }

		public string Address { get; set; }
	}
}
