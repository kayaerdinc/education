using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace EducationPlatform.Api.Extensions;

public static class ClaimsPrincipalExtensions
{
    public static Guid GetUserId(this ClaimsPrincipal principal)
    {
        var idValue = principal.FindFirstValue(JwtRegisteredClaimNames.Sub)
                     ?? principal.FindFirstValue(ClaimTypes.NameIdentifier);

        if (Guid.TryParse(idValue, out var userId))
        {
            return userId;
        }

        throw new InvalidOperationException("Kullanıcı kimliği bulunamadı.");
    }

    public static string GetUserRole(this ClaimsPrincipal principal)
    {
        return principal.FindFirstValue(ClaimTypes.Role)
               ?? throw new InvalidOperationException("Kullanıcı rolü bulunamadı.");
    }
}
