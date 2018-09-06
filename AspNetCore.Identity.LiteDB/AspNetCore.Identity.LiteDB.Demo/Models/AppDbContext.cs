using AspNetCore.Identity.LiteDB.Data;
using LiteDB;

namespace AspNetCore.Identity.LiteDB.Demo.Models
{
   /// <summary>
   ///   Sample DbContext to configure ILiteDbContext with custom implementation
   /// </summary>
   public class AppDbContext : ILiteDbContext
   {
      public LiteDatabase LiteDatabase { get; } = new LiteDatabase("Filename=Database.db");
   }
}
