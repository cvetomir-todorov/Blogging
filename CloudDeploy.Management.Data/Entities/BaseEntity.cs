using System.ComponentModel.DataAnnotations;

namespace CloudDeploy.Management.Data.Entities
{
	public abstract class BaseEntity
	{
		[Key]
		public int ID { get; set; }
	}
}
