using Model.Enums;
using System.ComponentModel.DataAnnotations;

namespace Model
{
	public class Patient
	{
		public Name Name { get; set; }
		public GenderEnum? Gender { get; set; }
		[Required]
		public DateTime? BirthDate { get; set; }
		public ActiveEnum? Active { get; set; }
	}
}
