using System.Diagnostics;
using AspNetCore.Identity.LiteDB.Demo.Models;
using Microsoft.AspNetCore.Mvc;

namespace AspNetCore.Identity.LiteDB.Demo.Controllers
{
   public class HomeController : Controller
   {
      public IActionResult Index() => View();

      public IActionResult About()
      {
         ViewData["Message"] = "Your application description page.";

         return View();
      }

      public IActionResult Contact()
      {
         ViewData["Message"] = "Your contact page.";

         return View();
      }

      public IActionResult Error() => View(new ErrorViewModel
         {RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier});
   }
}