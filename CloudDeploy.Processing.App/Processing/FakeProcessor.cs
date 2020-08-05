using System;
using System.Threading.Tasks;
using CloudDeploy.Management.Data;
using CloudDeploy.Management.Data.Entities;
using Microsoft.Extensions.Logging;

namespace CloudDeploy.Processing.App.Processing
{
	public interface IProcessor
	{
		Task ProcessDeployment(DeploymentEntity deploymentEntity);

		Task StopDeployment(DeploymentEntity deploymentEntity);
	}

	public sealed class FakeProcessor : IProcessor
	{
		private readonly ILogger _logger;
		private readonly ManagementDbContext _managementDbContext;

		public FakeProcessor(ILogger<FakeProcessor> logger, ManagementDbContext managementDbContext)
		{
			_logger = logger;
			_managementDbContext = managementDbContext;
		}

		public async Task ProcessDeployment(DeploymentEntity deploymentEntity)
		{
			try
			{
				_logger.LogInformation("Starting deployment {0} ...", deploymentEntity);
				await UpdateStatus(deploymentEntity, DeploymentStatusEntity.InProgress);

				foreach (ServerEntity serverEntity in deploymentEntity.Servers)
				{
					_logger.LogInformation("Deployment {0} -> server {1}.", deploymentEntity, serverEntity);
				}

				_logger.LogInformation("Finished deployment {0}.", deploymentEntity);
				await UpdateStatus(deploymentEntity, DeploymentStatusEntity.Success);
			}
			catch (Exception exception)
			{
				_logger.LogError(exception, "Failed deployment {0}.", deploymentEntity);
				await UpdateStatus(deploymentEntity, DeploymentStatusEntity.Failure);
			}
		}

		public async Task StopDeployment(DeploymentEntity deploymentEntity)
		{
			try
			{
				_logger.LogInformation("Stopping deployment {0} ...", deploymentEntity);
				await UpdateStatus(deploymentEntity, DeploymentStatusEntity.Stopped);
				_logger.LogInformation("Stopped deployment {0}.", deploymentEntity);
			}
			catch (Exception exception)
			{
				_logger.LogError(exception, "Failed to stop deployment {0}.", deploymentEntity);
			}
		}

		private async Task UpdateStatus(DeploymentEntity deploymentEntity, DeploymentStatusEntity newStatus)
		{
			deploymentEntity.Status = newStatus;
			int recordsAffected = await _managementDbContext.SaveChangesAsync();
			if (recordsAffected == 0)
			{
				throw new InvalidOperationException($"Failed to update status of deployment {deploymentEntity.ID} to {newStatus}.");
			}
			else if (recordsAffected > 1)
			{
				throw new InvalidOperationException($"Updated {recordsAffected} deployments instead of just 1.");
			}
		}
	}
}
