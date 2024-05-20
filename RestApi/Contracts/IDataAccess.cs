using PatientDB.Models;

namespace RestApi.Contracts
{
	public interface IDataAccess
	{
		public Task<List<Person>> GetAllAsync();
		public Task<Person?> GetPersonByFamilyBithDateAsync(string family, DateTime? birthDate);
		public Task<bool> CreateAsync(Person model);
		public Task<bool> UpdateAsync(Person model);
		public Task<bool> DeleteAsync(Person model);
		IEnumerable<Person> FindAsync(Func<Person, bool> func);
		Task<Person?> GetPersonByExternalIdAsync(Guid id);
	}
}
