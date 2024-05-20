using Model;
using RestApi.Models;

namespace RestApi.Contracts
{
	public interface IService
	{
		public Task<Patient?> GetPatientAsync(string familyName, DateTime birthDate);
		public Task<IEnumerable<Patient>> GetAllPatientAsync();
		public Task<ProcessStatusEnum> AddPatientAsync(Patient patient);
		public Task<ProcessStatusEnum> UpdatePatientAsync(Patient patient);
		public Task<ProcessStatusEnum> DeletePatientAsync(Patient patient);
		public Task<(ProcessStatusEnum, IEnumerable<Patient>?)> FindAsync(string query);
		Task<Patient> GetPatientAsync(Guid id);
	}
}
