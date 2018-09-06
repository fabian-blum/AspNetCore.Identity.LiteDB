using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Authorization;

namespace AspNetCore.Identity.LiteDB.Extensions
{
   public static class AuthorizationPolicyBuilderExtensions
   {
      public static AuthorizationPolicyBuilder RequireRole(this AuthorizationPolicyBuilder builder,
         IEnumerable<IdentityRole> roles) => builder.RequireRole(roles.Select(role => role.Name).ToList());


      public static AuthorizationPolicyBuilder RequireRole(this AuthorizationPolicyBuilder builder,
         params IdentityRole[] roles) => builder.RequireRole(roles.Select(role => role.Name).ToList());
   }
}
