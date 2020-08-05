using System;
using CloudDeploy.Api;
using CloudDeploy.Management.Data.Entities;

namespace CloudDeploy.Management.App.Util
{
	public static class Map
	{
		public static CloudProviderEntity CloudProviderDtoToEntity(CloudProviderDto dto)
		{
			switch (dto)
			{
				case CloudProviderDto.None:
					return CloudProviderEntity.None;
				case CloudProviderDto.Aws:
					return CloudProviderEntity.Aws;
				case CloudProviderDto.Azure:
					return CloudProviderEntity.Azure;
				case CloudProviderDto.GoogleCloud:
					return CloudProviderEntity.GoogleCloud;
				default:
					throw new NotSupportedException($"Enum {typeof(CloudProviderDto)} value {dto} is not supported.");
			}
		}

		public static CloudProviderDto CloudProviderEntityToDto(CloudProviderEntity entity)
		{
			switch (entity)
			{
				case CloudProviderEntity.None:
					return CloudProviderDto.None;
				case CloudProviderEntity.Aws:
					return CloudProviderDto.Aws;
				case CloudProviderEntity.Azure:
					return CloudProviderDto.Azure;
				case CloudProviderEntity.GoogleCloud:
					return CloudProviderDto.GoogleCloud;
				default:
					throw new NotSupportedException($"Enum {typeof(CloudProviderEntity)} value {entity} is not supported.");
			}
		}

		public static DeploymentStatusDto DeploymentStatusEntityToDto(DeploymentStatusEntity entity)
		{
			switch (entity)
			{
				case DeploymentStatusEntity.None:
					return DeploymentStatusDto.None;
				case DeploymentStatusEntity.Pending:
					return DeploymentStatusDto.Pending;
				case DeploymentStatusEntity.InProgress:
					return DeploymentStatusDto.InProgress;
				case DeploymentStatusEntity.Success:
					return DeploymentStatusDto.Success;
				case DeploymentStatusEntity.Failure:
					return DeploymentStatusDto.Failure;
				case DeploymentStatusEntity.Stopped:
					return DeploymentStatusDto.Stopped;
				default:
					throw new NotSupportedException($"Enum {typeof(DeploymentStatusEntity)} value {entity} is not supported.");
			}
		}

		public static ProjectDto ProjectEntityToDto(ProjectEntity projectEntity)
		{
			return new ProjectDto
			{
				ID = projectEntity.ID,
				Name = projectEntity.Name,
				PackageSource = projectEntity.PackageSource,
				DeploymentCount = projectEntity.Deployments.Count
			};
		}

		public static ServerDto ServerEntityToDto(ServerEntity serverEntity)
		{
			return new ServerDto
			{
				ID = serverEntity.ID,
				Cloud = CloudProviderEntityToDto(serverEntity.Cloud),
				Address = serverEntity.Address
			};
		}

		public static DeploymentDto DeploymentEntityToDto(DeploymentEntity deploymentEntity)
		{
			return new DeploymentDto
			{
				ID = deploymentEntity.ID,
				PackageVersion = deploymentEntity.PackageVersion,
				Status = DeploymentStatusEntityToDto(deploymentEntity.Status)
			};
		}
	}
}
