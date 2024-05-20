using Microsoft.EntityFrameworkCore;
using PatientDB.Models;

namespace PatientDB
{
	public class PatientContext : DbContext
	{
		public PatientContext(DbContextOptions<PatientContext> options) : base(options)
		{ }
		public DbSet<Person> Persons { get; set; }
	}
}
