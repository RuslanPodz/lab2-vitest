using System.Security.Claims;

namespace MobileYogaApp.Extensions
{
    public static class ClaimsPrincipalExtensions
    {
        public static Guid? GetUserId(this ClaimsPrincipal user)
        {
            var userIdClaim = user.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (string.IsNullOrEmpty(userIdClaim)) return null;

            return Guid.TryParse(userIdClaim, out var userId) ? userId : null;
        }
    }
}