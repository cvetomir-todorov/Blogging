using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CloudDeploy.Management.Data.Entities
{
	[Table("Project")]
	public sealed class ProjectEntity : BaseEntity
	{
		public ProjectEntity()
		{
			Deployments = new List<DeploymentEntity>();
		}

		[Required]
		[StringLength(maximumLength: 128)]
		public string Name { get; set; }

		[Required]
		public string PackageSource { get; set; }

		public List<DeploymentEntity> Deployments { get; set; }
	}
}
