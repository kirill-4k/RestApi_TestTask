using Model;
using Model.Enums;
using Newtonsoft.Json.Linq;
using PatientDB.Models;
using RestApi.Contracts;

namespace RestApi.Service
{
	public class DomainDtoMapping : IPatientMapping
	{
		public Person MapPatientToPerson(Patient patient)
		{
			if (patient.BirthDate == null)
			{
				throw new NullReferenceException(nameof(patient));
			}

			return new Person()
			{
				Active = patient.Active != null ? (patient.Active == ActiveEnum.True) : null,
				ExternalId = patient.Name.Id,
				BirthDate = patient.BirthDate.Value,
				Id = Guid.NewGuid(),
				FamilyName = patient.Name.Family,
				FirstName = patient.Name.Given?.Length > 0 ? patient.Name.Given[0] : null,
				MiddleName = patient.Name.Given?.Length > 1 ? patient.Name.Given[1] : null,
				Gender = (int?)patient.Gender
			};
		}
		public Patient MapPersonToPatient(Person person)
		{
			return new Patient
			{
				Active = person.Active != null ? (person.Active.Value? ActiveEnum.True: ActiveEnum.False) : null,
				BirthDate = person.BirthDate,
				Gender = (GenderEnum?)person.Gender,
				Name = new Name
				{
					Family = person.FamilyName,
					Id = person.ExternalId,
					Use = person.Use,
					Given = new string[2] { person.FirstName, person.MiddleName }
				}
			};
		}

		public IEnumerable<Patient> MapPersonsToPatients(IEnumerable<Person> persons)
		{
			var patients = new List<Patient>();
			foreach (var person in persons)
			{
				patients.Add(MapPersonToPatient(person));
			}
			return patients;
		}
	}
}
