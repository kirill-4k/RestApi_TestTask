using Swashbuckle.AspNetCore.Annotations;

namespace RestApi.Models
{
	public enum ProcessStatusEnum
	{
		Success,
		Exists,
		NotFound,
		Error,
		Created,
		BadRequest
	}
}
