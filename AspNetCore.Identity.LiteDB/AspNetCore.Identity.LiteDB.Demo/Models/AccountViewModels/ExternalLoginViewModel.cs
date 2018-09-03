using System.ComponentModel.DataAnnotations;

namespace AspNetCore.Identity.LiteDB.Demo.Models.AccountViewModels
{
   public class ExternalLoginViewModel
   {
      [Required] [EmailAddress] public string Email { get; set; }
   }
}