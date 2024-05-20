using Microsoft.AspNetCore.Mvc;
using Model;
using RestApi.Contracts;
using RestApi.Models;
using Swashbuckle.AspNetCore.Annotations;

namespace RestApi.Controllers
{
	[ApiController]
	[Route("[controller]/[Action]")]
	public class ApiController : ControllerBase
	{
		private readonly ILogger<PatientsController> _logger;
		private readonly IService _service;

		public ApiController(ILogger<PatientsController> logger, IService service)
		{
			_logger = logger;
			_service = service;
		}
		
		[HttpGet]
		[ActionName("IsAlive")]
		public ActionResult IsAlive()
		{
			return Ok("Online");
		}
		[HttpGet]
		[SwaggerOperation(
				 Summary = "Get all patients",
				 Description = "Retrieve all patients, can take a long time")]
		[SwaggerResponse(StatusCodes.Status200OK, "Patients were retrived", Type = typeof(Patient))]
		[SwaggerResponse(StatusCodes.Status404NotFound, "Can not find any patient", Type = typeof(Patient))]
		public async Task<ActionResult> GetAllAsync()
		{
			var patients = await _service.GetAllPatientAsync();
			if (patients.Any())
			{
				return Ok(new JsonResult(patients));
			}
			return NotFound();
		}
	}
}