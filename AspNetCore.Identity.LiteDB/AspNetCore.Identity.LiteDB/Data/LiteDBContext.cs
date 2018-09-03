using System.Linq;
using LiteDB;
using Microsoft.Extensions.Configuration;

namespace AspNetCore.Identity.LiteDB.Data
{
   public class LiteDbContext
   {
      public LiteDbContext(IConfiguration configuration)
      {
         var connectionString = configuration.GetConnectionString("LiteDbIdentity") ?? configuration.GetSection("ConnectionStrings").GetChildren().FirstOrDefault()?.Value;

         LiteDatabase = new LiteDatabase(connectionString);
      }

      public LiteDatabase LiteDatabase { get; set; }
   }
}