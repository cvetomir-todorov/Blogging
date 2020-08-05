using System.Linq;
using System.Threading.Tasks;
using CloudDeploy.Management.Data;
using CloudDeploy.Management.Data.Entities;
using CloudDeploy.Processing.Api;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace CloudDeploy.Processing.App.Api
{
	[Route("api/processing")]
	[ApiController]
	public sealed class ProcessingController : ControllerBase
	{
		private readonly ManagementDbContext _managementDbContext;

		public ProcessingController(ManagementDbContext managementDbContext)
		{
			_managementDbContext = managementDbContext;
		}

		[HttpPost]
		public async Task<ActionResult> ProcessDeployment(ProcessDeploymentRequest request)
		{
			// use include with filter when EF supports it
			ProjectEntity projectEntity = await _managementDbContext.Projects
				.Include(project => project.Deployments)
				.Where(project => project.ID == request.ProjectID)
				.FirstOrDefaultAsync();

			if (projectEntity == null)
			{
				return BadRequest();
			}

			DeploymentEntity deploymentEntity = projectEntity.Deployments
				.FirstOrDefault(deployment => deployment.ID == request.DeploymentID);

			if (deploymentEntity == null)
			{
				return Conflict();
			}



			return Ok();
		}

		[HttpDelete("{deploymentID}")]
		public async Task<ActionResult> StopDeployment([FromRoute] int deploymentID)
		{
			DeploymentEntity deploymentEntity = await _managementDbContext.Deployments
				.FirstOrDefaultAsync(deployment => deployment.ID == deploymentID);

			if (deploymentEntity == null)
			{
				return BadRequest();
			}

			return Ok();
		}
	}
}
