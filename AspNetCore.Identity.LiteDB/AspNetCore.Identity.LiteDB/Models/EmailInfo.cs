using System;
using System.Diagnostics.CodeAnalysis;
using Newtonsoft.Json;

namespace AspNetCore.Identity.LiteDB.Models
{
   [SuppressMessage("ReSharper", "UnusedMember.Global")]
   public class EmailInfo
   {
      public string Address { get; internal set; }

      public string NormalizedAddress { get; internal set; }

      public DateTime? ConfirmationTime { get; internal set; }
      public bool IsConfirmed => ConfirmationTime != null;

      [JsonIgnore]
      public bool AllPropertiesAreSetToDefaults =>
         Address == null &&
         NormalizedAddress == null &&
         ConfirmationTime == null;

      public static implicit operator EmailInfo(string input)
         => new EmailInfo {Address = input, NormalizedAddress = input, ConfirmationTime = DateTime.Now};
   }
}