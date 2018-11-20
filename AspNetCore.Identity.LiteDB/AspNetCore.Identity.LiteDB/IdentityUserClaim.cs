using System.Diagnostics.CodeAnalysis;
using System.Security.Claims;

namespace AspNetCore.Identity.LiteDB
{
   [SuppressMessage("ReSharper", "UnusedMember.Global")]
   public class IdentityUserClaim
   {
      public IdentityUserClaim()
      {
      }

      public IdentityUserClaim(Claim claim)
      {
         Type = claim.Type;
         Value = claim.Value;
      }

      public string Type { get; set; }
      public string Value { get; set; }

      public Claim ToSecurityClaim() => new Claim(Type, Value);
   }
}