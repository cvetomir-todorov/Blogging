using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace CloudDeploy.Web
{
	public interface IMvcResultUtility
	{
		ObjectResult InternalServerError(string message);
	}

	public class MvcResultUtility : IMvcResultUtility
	{
		public ObjectResult InternalServerError(string message)
		{
			var problem = new ProblemDetails
			{
				Status = StatusCodes.Status500InternalServerError,
				Title = "Internal server error",
				Detail = message
			};

			ObjectResult result = new ObjectResult(problem);
			result.StatusCode = problem.Status;

			return result;
		}
	}
}
