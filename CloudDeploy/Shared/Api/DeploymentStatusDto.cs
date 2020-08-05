using System.Text.Json.Serialization;

namespace CloudDeploy.Api
{
	[JsonConverter(typeof(JsonStringEnumConverter))]
	public enum DeploymentStatusDto
	{
		None,
		Pending,
		InProgress,
		Success,
		Failure,
		Stopped
	}
}
