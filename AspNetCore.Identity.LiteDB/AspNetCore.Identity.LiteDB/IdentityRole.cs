using System.Diagnostics.CodeAnalysis;
using LiteDB;
using Microsoft.AspNetCore.Identity;

namespace AspNetCore.Identity.LiteDB
{
   [SuppressMessage("ReSharper", "UnusedMember.Global")]
   public class IdentityRole : IdentityRole<string>
   {
      public IdentityRole() => Id = ObjectId.NewObjectId().ToString();

      public IdentityRole(string roleName) : this() => Name = roleName;

      [BsonId] public new string Id { get; set; }

      public new string Name { get; set; }
   }
}