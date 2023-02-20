using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace EBird.Api.Controllers.Exentions
{
    public static class ControllerExtensionMethods
    {
        public static string GetUserId(this Microsoft.AspNetCore.Mvc.ControllerBase controller)
        {
            return controller.User.FindFirstValue(ClaimTypes.NameIdentifier);
        }

        public static Guid ToGuid(this Guid? source)
        {
          return  source ?? Guid.Empty;
        }
    }
}