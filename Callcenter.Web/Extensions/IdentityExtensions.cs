using System.Security.Claims;

namespace Callcenter.Web.Extensions;

public static class IdentityExtensions
{
    public static bool IsAdmin(this ClaimsPrincipal user)
    {
        var userGroup = user.FindFirst("Group")?.Value;
            
        return !string.IsNullOrWhiteSpace(userGroup) && (
            userGroup.Contains("Администр", StringComparison.OrdinalIgnoreCase) ||
            userGroup.Contains("Инжене", StringComparison.OrdinalIgnoreCase) ||
            userGroup.Contains("Руковод", StringComparison.OrdinalIgnoreCase));
    }
    
    public static int? GetOrgId(this ClaimsPrincipal user)
    {
        var orgId = user.FindFirst("OrganisationId")?.Value;
            
        return int.TryParse(orgId, out var id) ? id : null;
    }
}