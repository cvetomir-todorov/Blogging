using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CloudDeploy.Management.Data.Entities
{
	[Table("Server")]
	public sealed class ServerEntity : BaseEntity
	{
		[Required]
		public CloudProviderEntity Cloud { get; set; }

		[Required]
		[StringLength(maximumLength: 1024)]
		public string Address { get; set; }
	}
}
