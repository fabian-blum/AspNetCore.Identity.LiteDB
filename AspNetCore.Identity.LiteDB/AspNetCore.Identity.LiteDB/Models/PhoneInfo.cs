using Newtonsoft.Json;
using System;

namespace AspNetCore.Identity.LiteDB.Models
{
    public class PhoneInfo
    {

        public string Number { get; internal set; }
        public DateTime? ConfirmationTime { get; internal set; }
        public bool IsConfirmed => ConfirmationTime != null;

        public static implicit operator PhoneInfo(string input)
            => new PhoneInfo { Number = input };

        [JsonIgnore]
        public bool AllPropertiesAreSetToDefaults =>
            Number == null &&
            ConfirmationTime == null;
    }
}
