using System.ComponentModel.DataAnnotations;

namespace AspNetCore.Identity.LiteDB.Demo.Models.AccountViewModels
{
   public class ForgotPasswordViewModel
   {
      [Required] [EmailAddress] public string Email { get; set; }
   }
}