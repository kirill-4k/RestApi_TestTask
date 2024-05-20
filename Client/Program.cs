using Client;
using Model;

var p = new PatientGenerator();

using var provider = new PatientProvider();
try
{
	//	docker run --network compose-network client
	var patients = new List<Patient>();

	for (int i = 0; i < 100; i++)
	{
		patients.Add(p.GenerateRandomPatient());
	}

	var paralelOption = new ParallelOptions() { MaxDegreeOfParallelism = 5 };
	await Parallel.ForEachAsync(patients, paralelOption, async (patient, ct)  =>
	{
		//Console.WriteLine($"Start paralel {patient.Name.Family}");
		var answer = await provider.CreatePatientAsync(patient);
		//Console.WriteLine("  - "+answer.StatusCode.ToString() + " - " + patient.Name.Family);
	});
}
catch (Exception ex)
{
	Console.WriteLine(ex.ToString());
}
Console.WriteLine($"Completed");
Console.ReadLine();