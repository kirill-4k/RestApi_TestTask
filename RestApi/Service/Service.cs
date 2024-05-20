using Model;
using RestApi.Contracts;
using RestApi.Models;

namespace RestApi.Service
{
	public class Service : IService
	{
		private readonly IDataAccess _dataAccess;
		private readonly IPatientMapping _patientMapping;

		public Service(IDataAccess dataAccess, IPatientMapping patientMapping)
		{
			_dataAccess = dataAccess;
			_patientMapping = patientMapping;
		}

		public async Task<ProcessStatusEnum> AddPatientAsync(Patient patient)
		{
			var current = await _dataAccess.GetPersonByFamilyBithDateAsync(patient.Name.Family, patient.BirthDate);
			if (current != null)
			{
				return ProcessStatusEnum.Exists;
			}
			if (await _dataAccess.CreateAsync(_patientMapping.MapPatientToPerson(patient)))
			{
				return ProcessStatusEnum.Created;
			}
			else return ProcessStatusEnum.Error;
		}

		public async Task<ProcessStatusEnum> DeletePatientAsync(Patient patient)
		{
			var current = await _dataAccess.GetPersonByFamilyBithDateAsync(patient.Name.Family, patient.BirthDate);
			if (current == null)
			{
				return ProcessStatusEnum.NotFound;
			}
			if (await _dataAccess.DeleteAsync(current))
			{
				return ProcessStatusEnum.Success;
			}
			else return ProcessStatusEnum.Error;
		}

		public async Task<(ProcessStatusEnum, IEnumerable<Patient>?)> FindAsync(string query)
		{
			var qb = new QueryParser(query);

			if (!qb.IsValidQuery)
			{
				return (ProcessStatusEnum.BadRequest, null);
			}

			var persons = _dataAccess.FindAsync(qb.GetPredicate());

			if (persons == null || !persons.Any())
				return (ProcessStatusEnum.NotFound, null);

			return (ProcessStatusEnum.Success, _patientMapping.MapPersonsToPatients(persons));
		}


		public async Task<IEnumerable<Patient>> GetAllPatientAsync()
		{
			var persons = await _dataAccess.GetAllAsync();

			return _patientMapping.MapPersonsToPatients(persons);
		}

		public async Task<Patient?> GetPatientAsync(string familyName, DateTime birthDate)
		{
			var person = await _dataAccess.GetPersonByFamilyBithDateAsync(familyName, birthDate);
			if (person != null)
			{
				return _patientMapping.MapPersonToPatient(person);
			}

			else return null;
		}

		public async Task<Patient?> GetPatientAsync(Guid id)
		{
			var person = await _dataAccess.GetPersonByExternalIdAsync(id);
			if (person != null)
			{
				return _patientMapping.MapPersonToPatient(person);
			}

			else return null;
		}

		public async Task<ProcessStatusEnum> UpdatePatientAsync(Patient patient)
		{
			var current = await _dataAccess.GetPersonByFamilyBithDateAsync(patient.Name.Family, patient.BirthDate);
			if (current == null)
			{
				return ProcessStatusEnum.NotFound;
			}
			var updated = _patientMapping.MapPatientToPerson(patient);
			updated.Id = current.Id;

			if (await _dataAccess.UpdateAsync(updated))
			{
				return ProcessStatusEnum.Success;
			}
			else return ProcessStatusEnum.Error;
		}

	}
}