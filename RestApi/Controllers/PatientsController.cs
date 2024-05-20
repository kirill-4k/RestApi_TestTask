using Microsoft.AspNetCore.Mvc;
using Model;
using RestApi.Contracts;
using RestApi.Models;
using Swashbuckle.AspNetCore.Annotations;

namespace RestApi.Controllers
{
	[ApiController]
	[Route("api/[controller]")]
	public class PatientsController : ControllerBase
	{
		private readonly ILogger<PatientsController> _logger;
		private readonly IService _service;

		public PatientsController(ILogger<PatientsController> logger, IService service)
		{
			_logger = logger;
			_service = service;
		}
		[HttpGet]
		[SwaggerOperation(
				 Summary = "Get patient by id",
				 Description = "Retrieve patient by GUID")]
		[SwaggerResponse(StatusCodes.Status200OK, "Patient was retrived", Type = typeof(Patient))]
		[SwaggerResponse(StatusCodes.Status404NotFound, "Can not find any patient", Type = typeof(Patient))]
		public async Task<ActionResult> GetPatientAsync(Guid id)
		{
			var patient = await _service.GetPatientAsync(id);
			if (patient != null)
			{
				return Ok(new JsonResult(patient));
			}
			return NotFound();
		}

		/// <summary>
		/// Get patiens with ability to filter 
		/// </summary>
		/// <param name="query">Query for filtering by BirthDate for example [eq2024-01-01] or [gt2020-01-01T00:00:00]</param>
		/// <returns></returns>
		[HttpGet]
		[Route("[action]")]
		[ActionName("Find")]
		[SwaggerOperation(
				 Summary = "Find patiens with ability to filter",
				 Description = "Create a new patient if the same does not exist")]
		[SwaggerResponse(StatusCodes.Status200OK, "Patient was created", Type = typeof(Patient))]
		[SwaggerResponse(StatusCodes.Status400BadRequest, "Invalid patient model")]
		[SwaggerResponse(StatusCodes.Status404NotFound, "Can not find any patient", Type = typeof(Patient))]
		public async Task<ActionResult> FindPatientsAsync(string query)
		{

			var result = await _service.FindAsync(query);

			return CreateActionResult(result.Item1, result.Item2);
		}

		[HttpPut]
		[SwaggerOperation(
				 Summary = "Add a new patient",
				 Description = "Create a new patient if the same does not exist")]
		[SwaggerResponse(StatusCodes.Status201Created, "Patient was created", Type = typeof(Patient))]
		[SwaggerResponse(StatusCodes.Status400BadRequest, "Invalid patient model")]
		[SwaggerResponse(StatusCodes.Status409Conflict, "Can not find insert dublicate for patient", Type = typeof(Patient))]
		public async Task<ActionResult> Put(Patient patient)
		{
			if (!ModelState.IsValid)
			{
				return BadRequest(ModelState);
			}
			var result = await _service.AddPatientAsync(patient);

			return CreateActionResult(result, patient);
		}

		[HttpPatch]
		[SwaggerOperation(
				 Summary = "Make update for patient",
				 Description = "Update patient model except bithdate and family name")]
		[SwaggerResponse(StatusCodes.Status200OK, "Patient was updated", Type = typeof(Patient))]
		[SwaggerResponse(StatusCodes.Status400BadRequest, "Invalid patient model")]
		[SwaggerResponse(StatusCodes.Status404NotFound, "Can not find patient for update")]
		public async Task<ActionResult> Update(Patient patient)
		{
			if (!ModelState.IsValid)
			{
				return BadRequest(ModelState);
			}
			var result = await _service.UpdatePatientAsync(patient);

			return CreateActionResult(result, patient);
		}

		[HttpDelete]
		[SwaggerOperation(
				 Summary = "Delete patient",
				 Description = "Delete patient")]
		[SwaggerResponse(StatusCodes.Status200OK, "Patient was deleted", Type = typeof(Patient))]
		[SwaggerResponse(StatusCodes.Status400BadRequest, "Invalid patient model")]
		[SwaggerResponse(StatusCodes.Status404NotFound, "Can not find patient for delete")]
		public async Task<ActionResult> DeleteAsync(Patient patient)
		{
			if (!ModelState.IsValid)
			{
				return BadRequest(ModelState);
			}
			var result = await _service.DeletePatientAsync(patient);

			return CreateActionResult(result, patient);
		}

		private ObjectResult CreateActionResult<T>(ProcessStatusEnum status, T model)
		{
			var objResult = Problem("Internal problem accured");

			switch (status)
			{
				case ProcessStatusEnum.Success: objResult = Ok(model); break;
				case ProcessStatusEnum.Created: objResult = new ObjectResult(model) { StatusCode = StatusCodes.Status201Created }; break;
				case ProcessStatusEnum.Exists: objResult = new ObjectResult(model) { StatusCode = StatusCodes.Status409Conflict }; break; ; break;
				case ProcessStatusEnum.NotFound: objResult = NotFound(model); break;
				case ProcessStatusEnum.Error: break;
				default: _logger.LogError(new NotImplementedException("Not implemented case for ProcessStatusEnum"), "Have to map ProcessStatusEnum"); break;
			}
			return objResult;
		}		
	}
}