using Model.Enums;
using System.ComponentModel.DataAnnotations;

namespace Model
{
	public class Name
	{
		public Guid? Id { get; set; }
		public string? Use { get; set; }
		[Required]
		public string Family { get; set; }
		public string[]? Given { get; set; }

	}
}
