using System;
using System.Diagnostics.CodeAnalysis;
using LiteDB;

namespace AspNetCore.Identity.LiteDB.Models
{
   [SuppressMessage("ReSharper", "UnusedMember.Global")]
   public class PhoneInfo
   {
      public string Number { get; internal set; }
      public DateTime? ConfirmationTime { get; internal set; }
      public bool IsConfirmed => ConfirmationTime != null;

      [BsonIgnore]
      public bool AllPropertiesAreSetToDefaults =>
         Number == null &&
         ConfirmationTime == null;

      public static implicit operator PhoneInfo(string input)
         => new PhoneInfo {Number = input};
   }
}
