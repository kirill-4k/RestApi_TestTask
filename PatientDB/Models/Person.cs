using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PatientDB.Models
{
	public class Person
	{
		[Key]
		public Guid Id { get; set; }
		[Required]
		public string FamilyName { get; set; }
		[Required]
		public DateTime BirthDate { get; set; }
		public Guid? ExternalId { get; set; }
		public string? FirstName { get; set; }
		public string? MiddleName { get; set; }
		public string? Use { get; set; }
		public int? Gender { get; set; }
		public bool? Active { get; set; }
	}
}
