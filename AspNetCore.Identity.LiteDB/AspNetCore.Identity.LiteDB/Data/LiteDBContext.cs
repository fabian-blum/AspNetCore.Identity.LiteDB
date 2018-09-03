using LiteDB;
using Microsoft.Extensions.Configuration;

namespace AspNetCore.Identity.LiteDB.Data
{
   public class LiteDbContext
   {
      public LiteDbContext(IConfiguration configuration) =>
         LiteDatabase = new LiteDatabase(configuration.GetConnectionString("Default"));

      public LiteDatabase LiteDatabase { get; set; }
   }
}