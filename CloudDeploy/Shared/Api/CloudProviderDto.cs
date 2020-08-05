using System.Text.Json.Serialization;

namespace CloudDeploy.Api
{
	[JsonConverter(typeof(JsonStringEnumConverter))]
	public enum CloudProviderDto
	{
		None,
		Aws,
		Azure,
		GoogleCloud
	}
}
