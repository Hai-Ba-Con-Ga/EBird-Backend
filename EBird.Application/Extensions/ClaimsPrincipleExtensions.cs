using System.Security.Claims;

namespace EBird.Application.Extensions
{
    public static class ClaimsPrincipleExtensions
    {
        public static Guid GetUserId(this ClaimsPrincipal user)
        {
            return Guid.Parse(user.FindFirstValue(ClaimTypes.NameIdentifier));
        }
    }
}