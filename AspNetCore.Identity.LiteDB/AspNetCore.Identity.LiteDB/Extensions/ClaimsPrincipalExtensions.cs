using System.Security.Claims;

namespace AspNetCore.Identity.LiteDB.Extensions
{
   public static class ClaimsPrincipalExtensions
   {
      public static bool IsInRole(this ClaimsPrincipal claimsPrincipal, IdentityRole role) => claimsPrincipal.IsInRole(role.NormalizedName);
   }
}
