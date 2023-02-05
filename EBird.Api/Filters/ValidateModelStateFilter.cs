using System.Linq;
using System.Net;
using AutoWrapper.Extensions;
using EBird.Application.Exceptions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Response;

namespace EBird.Api.Filters
{
    public class ValidateModelStateFilter : ActionFilterAttribute
    {
        public override void OnActionExecuting(ActionExecutingContext context)
        {
            if (!context.ModelState.IsValid)
            {
                var errors = string.Join("; ", context.ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage));
                var response = Response<string>.Builder().SetSuccess(false).SetStatusCode((int)HttpStatusCode.BadRequest).SetMessage("Invalid request").SetData(errors);
                context.Result = new BadRequestObjectResult(response);
            }
        }
    }
}