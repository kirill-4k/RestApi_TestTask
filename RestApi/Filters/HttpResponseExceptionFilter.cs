using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using System.Net;
using System.Web.Http;

namespace RestApi.Filters
{
	public class HttpResponseExceptionFilter : IActionFilter, IOrderedFilter
	{
		private readonly ILogger<HttpResponseExceptionFilter> _logger;

		public HttpResponseExceptionFilter(ILogger<HttpResponseExceptionFilter> logger)
        {
            _logger = logger;
        }
        public int Order => int.MaxValue - 10;

		public void OnActionExecuting(ActionExecutingContext context) { }

		public void OnActionExecuted(ActionExecutedContext context)
		{
			if (context.Exception is HttpResponseException httpResponseException)
			{
				_logger.LogError(context.Exception, context.Exception.Message);

				context.Result = new ObjectResult(httpResponseException.Data)
				{
					StatusCode = (int?)httpResponseException.Response.StatusCode
				};

				context.ExceptionHandled = true;
			}
			else if (context.Exception != null)
			{
				_logger.LogError(context.Exception, context.Exception.Message);
				
				context.Result = new ObjectResult(context.Exception.Message)
				{
					StatusCode = (int)HttpStatusCode.InternalServerError
				};

				context.ExceptionHandled = true;
			}
		}
	}
}
