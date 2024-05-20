using Microsoft.EntityFrameworkCore;
using PatientDB;
using PatientDB.Models;
using RestApi.Contracts;

namespace RestApi.DataAccess
{
	public class DataAccess : IDataAccess
	{
		private readonly PatientContext _patientContext;

		public DataAccess(PatientContext patientContext)
		{
			_patientContext = patientContext;

		}

		public async Task<bool> CreateAsync(Person model)
		{
			_patientContext.Persons.Add(model);
			return await _patientContext.SaveChangesAsync() == 1;
		}


		public async Task<bool> DeleteAsync(Person model)
		{
			_patientContext.Entry(model).State = EntityState.Deleted;

			return await _patientContext.SaveChangesAsync() == 1;
		}

		public IEnumerable<Person> FindAsync(Func<Person, bool> predicate)
		{
			return _patientContext.Persons.AsNoTracking().Where(predicate).ToList();
		}

		public async Task<List<Person>> GetAllAsync()
		{
			return await _patientContext.Persons.AsNoTracking().ToListAsync();
		}

		public async Task<Person?> GetPersonByFamilyBithDateAsync(string family, DateTime? birthDate)
		{
			return await _patientContext.Persons.AsNoTracking().Where(x => x.FamilyName == family && x.BirthDate == birthDate).FirstOrDefaultAsync();
		}

		public async Task<Person?> GetPersonByExternalIdAsync(Guid id)
		{
			return await _patientContext.Persons.AsNoTracking().Where(x => x.ExternalId == id).FirstOrDefaultAsync();
		}

		public async Task<bool> UpdateAsync(Person model)
		{
			_patientContext.Entry<Person>(model).State = EntityState.Modified;
			return await _patientContext.SaveChangesAsync() > 0;
		}
	}
}
