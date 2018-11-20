using System.Threading.Tasks;

namespace AspNetCore.Identity.LiteDB.Demo.Services
{
   public interface IEmailSender
   {
      Task SendEmailAsync(string email, string subject, string message);
   }
}