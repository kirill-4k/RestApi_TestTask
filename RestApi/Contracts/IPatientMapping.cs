using Model;
using PatientDB.Models;

namespace RestApi.Contracts
{
	public interface IPatientMapping
    {
		Person MapPatientToPerson(Patient patient);
		Patient MapPersonToPatient(Person person);
		IEnumerable<Patient> MapPersonsToPatients(IEnumerable<Person> persons);
	}
}