using LiteDB;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;

namespace AspNetCore.Identity.LiteDB.Models
{
    // Add profile data for application users by adding properties to the ApplicationUser class
    public class ApplicationUser : IdentityUser
    {
        public ApplicationUser()
        {
            Id = ObjectId.NewObjectId().ToString();
            Roles = new List<string>();
            Logins = new List<UserLoginInfo>();
            SerializableLogins = new List<SerializableUserLoginInfo>();
            Claims = new List<IdentityUserClaim>();
            Tokens = new List<UserToken<string>>();
        }

        [BsonId]
        public string Id { get; set; }

        public override string UserName { get; set; }

        public virtual string SecurityStamp { get; set; }

        public EmailInfo Email { get; set; }

        public LockoutInfo Lockout { get; internal set; }

        public PhoneInfo Phone { get; internal set; }

        //public virtual bool EmailConfirmed { get; set; }

        public virtual string PhoneNumber { get; set; }

        public virtual bool PhoneNumberConfirmed { get; set; }

        public virtual bool TwoFactorEnabled { get; set; }

        public virtual DateTime? LockoutEndDateUtc { get; set; }

        public virtual bool LockoutEnabled { get; set; }

        public virtual int AccessFailedCount { get; set; }

        public List<string> Roles { get; set; }

        public List<UserToken<string>> Tokens { get; set; }

        public bool UsesTwoFactorAuthentication { get; internal set; }

        public string AuthenticationKey { get; set; }

        public virtual void AddRole(string role)
        {
            Roles.Add(role);
        }

        public virtual void RemoveRole(string role)
        {
            Roles.Remove(role);
        }

        public virtual string PasswordHash { get; set; }

        public List<SerializableUserLoginInfo> SerializableLogins { get; set; }

        [BsonIgnore]
        public List<UserLoginInfo> Logins
        {
            get
            {
                return SerializableLogins?.Select(x => new UserLoginInfo(x.LoginProvider, x.ProviderKey, "")).ToList() ?? new List<UserLoginInfo>();
            }
            set
            {
                SerializableLogins = value?.Select(x => new SerializableUserLoginInfo(x.LoginProvider, x.ProviderKey)).ToList() ?? new List<SerializableUserLoginInfo>();
            }
        }

        public virtual void AddLogin(UserLoginInfo login)
        {
            SerializableLogins.Add(new SerializableUserLoginInfo(login.LoginProvider, login.ProviderKey));
        }

        public virtual void RemoveLogin(UserLoginInfo login)
        {
            var loginsToRemove = SerializableLogins
                .Where(l => l.LoginProvider == login.LoginProvider)
                .Where(l => l.ProviderKey == login.ProviderKey);

            SerializableLogins = SerializableLogins.Except(loginsToRemove).ToList();
        }

        public virtual bool HasPassword()
        {
            return false;
        }

        public List<IdentityUserClaim> Claims { get; set; }

        public virtual void AddClaim(Claim claim)
        {
            Claims.Add(new IdentityUserClaim(claim));
        }

        public virtual void RemoveClaim(Claim claim)
        {
            var claimsToRemove = Claims
                .Where(c => c.Type == claim.Type)
                .Where(c => c.Value == claim.Value);

            Claims = Claims.Except(claimsToRemove).ToList();
        }

        public void AddToken(UserToken<string> token)
        {
            var existingToken = Tokens.SingleOrDefault(t => t.LoginProvider == token.LoginProvider && t.TokenName == token.TokenName);
            if (existingToken == null)
            {
                Tokens.Add(token);
            }
            else
            {
                existingToken.TokenValue = token.TokenValue;
            }
        }

        public void RemoveToken(string loginProvider, string name)
        {
            Tokens = Tokens
                .Except(Tokens.Where(t => t.LoginProvider == loginProvider && t.TokenName == name))
                .ToList();
        }
    }
}
