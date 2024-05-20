using AutoFixture;
using Model;

namespace Client
{
	public class PatientGenerator
	{
		public Patient GenerateRandomPatient()
		{
			var fixture = new Fixture();

			return fixture.Create<Patient>();	
		}
	}
}
