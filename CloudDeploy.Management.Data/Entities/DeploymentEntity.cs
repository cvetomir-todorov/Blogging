using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CloudDeploy.Management.Data.Entities
{
	[Table("Deployment")]
	public sealed class DeploymentEntity : BaseEntity
	{
		public DeploymentEntity()
		{
			Servers = new List<ServerEntity>();
		}

		[Required]
		[StringLength(maximumLength: 32)]
		public string PackageVersion { get; set; }

		[Required]
		public DeploymentStatusEntity Status { get; set; }

		[Required]
		[ForeignKey("ProjectID")]
		public int ProjectID { get; set; }

		public ProjectEntity Project { get; set; }

		[ForeignKey("DeploymentID")]
		public List<ServerEntity> Servers { get; set; }
	}
}
