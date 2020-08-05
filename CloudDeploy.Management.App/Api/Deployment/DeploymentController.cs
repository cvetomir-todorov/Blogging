using System;
using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;
using CloudDeploy.Api;
using CloudDeploy.Management.App.Util;
using CloudDeploy.Management.Data;
using CloudDeploy.Management.Data.Entities;
using CloudDeploy.Processing.Api;
using CloudDeploy.Web;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace CloudDeploy.Management.App.Api.Deployment
{
	[Route("api/deployment")]
	[ApiController]
	public sealed class DeploymentController : ControllerBase
	{
		private readonly ILogger _logger;
		private readonly ManagementDbContext _managementDbContext;
		private readonly HttpClient _httpClient;
		private readonly IMvcResultUtility _mvcResult;

		public DeploymentController(
			ILogger<DeploymentController> logger,
			ManagementDbContext managementDbContext,
			HttpClient httpClient,
			IMvcResultUtility mvcResult)
		{
			_logger = logger;
			_managementDbContext = managementDbContext;
			_httpClient = httpClient;
			_mvcResult = mvcResult;
		}

		[HttpPost]
		public async Task<ActionResult> StartDeployment([FromBody] StartDeploymentRequest request)
		{
			DeploymentDto deploymentDto = await PersistDeployment(request);
			bool success = await SendProcessRequest(request.ProjectID, deploymentDto);
			if (success)
			{
				return Ok(deploymentDto);
			}
			else
			{
				return _mvcResult.InternalServerError("Unable to process deployment.");
			}
		}

		private async Task<DeploymentDto> PersistDeployment(StartDeploymentRequest request)
		{
			DeploymentEntity deploymentEntity = new DeploymentEntity
			{
				PackageVersion = request.Deployment.PackageVersion,
				Status = DeploymentStatusEntity.Pending,
				ProjectID = request.ProjectID,
			};

			foreach (int serverID in request.ServerIDs)
			{
				ServerEntity serverEntity = new ServerEntity
				{
					ID = serverID
				};
				deploymentEntity.Servers.Add(serverEntity);
			}

			_managementDbContext.Deployments.Add(deploymentEntity);
			int recordsAffected = await _managementDbContext.SaveChangesAsync();
			if (recordsAffected == 1)
			{
				DeploymentDto deploymentDto = Map.DeploymentEntityToDto(deploymentEntity);
				return deploymentDto;
			}
			else
			{
				throw new InvalidOperationException("Failed to persist new deployment.");
			}
		}

		private async Task<bool> SendProcessRequest(int projectID, DeploymentDto deploymentDto)
		{
			ProcessDeploymentRequest processRequest = new ProcessDeploymentRequest
			{
				DeploymentID = deploymentDto.ID,
				ProjectID = projectID
			};
			string processJson = JsonSerializer.Serialize(processRequest);
			HttpContent processContent = new StringContent(processJson);

			try
			{
				HttpResponseMessage processResponse = await _httpClient.PostAsync("api/processing", processContent);
				if (!processResponse.IsSuccessStatusCode)
				{
					string content = await processResponse.Content.ReadAsStringAsync();
					_logger.LogError("Unexpected HTTP status code {0} {1} while processing deployment {2}. Content is {3}.",
						processResponse.StatusCode, processResponse.ReasonPhrase, deploymentDto, content);

					return false;
				}

				return true;
			}
			catch (HttpRequestException httpRequestException)
			{
				_logger.LogError(httpRequestException, "Unexpected error while processing deployment {0}.", deploymentDto);
				return false;
			}
		}

		[HttpDelete("{id}")]
		public async Task<ActionResult> StopDeployment([FromRoute] int id)
		{
			bool success = await SendStopRequest(id);
			if (success)
			{
				return Ok();
			}
			else
			{
				return _mvcResult.InternalServerError("Unable to stop deployment.");
			}
		}

		private async Task<bool> SendStopRequest(int id)
		{
			try
			{
				HttpResponseMessage stopResponse = await _httpClient.DeleteAsync($"api/processing/{id}");
				if (!stopResponse.IsSuccessStatusCode)
				{
					string content = await stopResponse.Content.ReadAsStringAsync();
					_logger.LogError("Unexpected HTTP status code {0} {1} while stopping deployment with ID {2}. Content is {3}.",
						stopResponse.StatusCode, stopResponse.ReasonPhrase, id, content);

					return false;
				}

				return true;
			}
			catch (HttpRequestException httpRequestException)
			{
				_logger.LogError(httpRequestException, "Unexpected error while stopping deployment with ID {0}.", id);
				return false;
			}
		}

		[HttpGet("{id}")]
		public async Task<ActionResult> GetDeployment([FromRoute] int id)
		{
			DeploymentEntity deploymentEntity = await _managementDbContext.Deployments
				.FirstOrDefaultAsync(deployment => deployment.ID == id);

			if (deploymentEntity == null)
			{
				return NotFound();
			}

			DeploymentDto deploymentDto = Map.DeploymentEntityToDto(deploymentEntity);
			return Ok(deploymentDto);
		}
	}
}
