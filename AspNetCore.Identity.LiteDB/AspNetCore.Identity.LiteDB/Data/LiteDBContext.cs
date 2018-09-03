using System;
using System.Linq;
using LiteDB;
using Microsoft.Extensions.Configuration;

namespace AspNetCore.Identity.LiteDB.Data
{
   public class LiteDbContext : ILiteDbContext
   {
      public LiteDbContext(IConfiguration configuration)
      {
         string connectionString;
         try
         {
            connectionString = configuration.GetSection("ConnectionStrings").GetChildren().FirstOrDefault()?.Value;
         }
         catch (NullReferenceException)
         {
            throw new NullReferenceException("No connection string defined in appsettings.json");
         }

         LiteDatabase = new LiteDatabase(connectionString);
      }

      public LiteDatabase LiteDatabase { get; set; }
   }
}