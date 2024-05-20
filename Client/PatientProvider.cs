using System.Net.Http.Json;
using Model;
using System.Net.Http.Headers;

namespace Client
{
	public class PatientProvider : IDisposable
	{
		HttpClient _client;
		private readonly string _apiUri = "api/patients";
		public PatientProvider()
		{
			HttpClientHandler clientHandler = new HttpClientHandler();
			clientHandler.ServerCertificateCustomValidationCallback = (sender, cert, chain, sslPolicyErrors) => { return true; };
			_client = new HttpClient(clientHandler);

			_client.BaseAddress = new Uri("https://restapi/");

			_client.DefaultRequestHeaders.Accept.Clear();
			_client.DefaultRequestHeaders.Accept.Add(
				new MediaTypeWithQualityHeaderValue("application/json"));

		}
		public async Task<HttpResponseMessage> CreatePatientAsync(Patient patient)
		{
			HttpResponseMessage response = await _client.PutAsJsonAsync(_apiUri, patient);
			return response.EnsureSuccessStatusCode();
		}

		public async Task<IEnumerable<Patient>?> GetAll()
		{
			List<Patient>? patient = null;
			var response = await _client.GetAsync(_apiUri);
			if (response.IsSuccessStatusCode)
			{
				patient = await response.Content.ReadFromJsonAsync<List<Patient>>();
			}
			return patient;
		}

		public void Dispose()
		{
			_client.Dispose();
		}
	}
}
