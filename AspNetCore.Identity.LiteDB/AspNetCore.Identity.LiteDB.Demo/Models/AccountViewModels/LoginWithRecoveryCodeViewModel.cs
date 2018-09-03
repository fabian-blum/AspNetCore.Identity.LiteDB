using System.ComponentModel.DataAnnotations;

namespace AspNetCore.Identity.LiteDB.Demo.Models.AccountViewModels
{
   public class LoginWithRecoveryCodeViewModel
   {
      [Required]
      [DataType(DataType.Text)]
      [Display(Name = "Recovery Code")]
      public string RecoveryCode { get; set; }
   }
}