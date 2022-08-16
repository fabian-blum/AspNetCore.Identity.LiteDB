﻿using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Runtime.InteropServices;
using System.Security.Claims;
using System.Threading;
using System.Threading.Tasks;
using AspNetCore.Identity.LiteDB.Data;
using AspNetCore.Identity.LiteDB.Models;
using LiteDB;
using Microsoft.AspNetCore.Identity;
using Microsoft.Win32.SafeHandles;

namespace AspNetCore.Identity.LiteDB
{
   [SuppressMessage("ReSharper", "UnusedMember.Global")]
   [SuppressMessage("ReSharper", "RedundantExtendsListEntry")]
   public class LiteDbUserStore<TUser> : IUserStore<TUser>,
      IUserRoleStore<TUser>,
      IUserLoginStore<TUser>,
      IUserPasswordStore<TUser>,
      IUserClaimStore<TUser>,
      IUserSecurityStampStore<TUser>,
      IUserTwoFactorStore<TUser>,
      IUserAuthenticationTokenStore<TUser>,
      IUserTwoFactorRecoveryCodeStore<TUser>,
      IUserEmailStore<TUser>,
      IUserLockoutStore<TUser>,
      IUserPhoneNumberStore<TUser>,
      IQueryableUserStore<TUser>,
      IUserAuthenticatorKeyStore<TUser> where TUser : ApplicationUser, new()
   {
      private const string AuthenticatorStoreLoginProvider = "[AspNetAuthenticatorStore]";
      private const string AuthenticatorKeyTokenName = "AuthenticatorKey";
      private const string RecoveryCodeTokenName = "RecoveryCodes";

      private readonly ILiteCollection<CancellationToken> _cancellationTokens;
      private readonly ILiteCollection<TUser> _users;

      public LiteDbUserStore(ILiteDbContext dbContext)
      {
         _users = dbContext.LiteDatabase.GetCollection<TUser>("users");
         _cancellationTokens = dbContext.LiteDatabase.GetCollection<CancellationToken>("cancellationtokens");
      }

      public IQueryable<TUser> Users => _users.FindAll().AsQueryable();

      public Task SaveChanges(
         CancellationToken cancellationToken = default(CancellationToken)
      )
      {
         _cancellationTokens.Insert(cancellationToken);
         return Task.FromResult(cancellationToken);
      }

      #region IUserStore

      public Task<string> GetUserIdAsync(TUser user, CancellationToken cancellationToken = default(CancellationToken))
      {
         cancellationToken.ThrowIfCancellationRequested();
         ThrowIfDisposed();
         if (user == null) throw new ArgumentNullException(nameof(user));

         return Task.FromResult(user.Id);
      }

      public Task<string> GetUserNameAsync(TUser user, CancellationToken cancellationToken = default(CancellationToken))
      {
         cancellationToken.ThrowIfCancellationRequested();
         ThrowIfDisposed();

         if (user == null) throw new ArgumentNullException(nameof(user));

         return Task.FromResult(user.UserName);
      }

      public Task SetUserNameAsync(TUser user, string userName,
         CancellationToken cancellationToken = default(CancellationToken))
      {
         cancellationToken.ThrowIfCancellationRequested();
         ThrowIfDisposed();

         if (user == null) throw new ArgumentNullException(nameof(user));
         user.UserName = userName ?? throw new ArgumentNullException(nameof(userName));

         return Task.CompletedTask;
      }

      public Task<string> GetNormalizedUserNameAsync(TUser user,
         CancellationToken cancellationToken = default(CancellationToken))
      {
         cancellationToken.ThrowIfCancellationRequested();
         ThrowIfDisposed();

         if (user == null) throw new ArgumentNullException(nameof(user));

         return Task.FromResult(user.NormalizedUserName);
      }

      public Task SetNormalizedUserNameAsync(TUser user, string normalizedName,
         CancellationToken cancellationToken = default(CancellationToken))
      {
         cancellationToken.ThrowIfCancellationRequested();
         ThrowIfDisposed();

         if (user == null) throw new ArgumentNullException(nameof(user));
         user.NormalizedUserName = normalizedName ?? throw new ArgumentNullException(nameof(normalizedName));

         return Task.CompletedTask;
      }

      public async Task<IdentityResult> CreateAsync(
         TUser user,
         CancellationToken cancellationToken = default(CancellationToken)
      )
      {
         cancellationToken.ThrowIfCancellationRequested();
         ThrowIfDisposed();

         if (user == null) throw new ArgumentNullException(nameof(user));

         await Task.Run(() => { _users.Insert(user); }, cancellationToken);

         return IdentityResult.Success;
      }

      public async Task<IdentityResult> UpdateAsync(TUser user,
         CancellationToken cancellationToken = default(CancellationToken))
      {
         cancellationToken.ThrowIfCancellationRequested();
         ThrowIfDisposed();

         if (user == null) throw new ArgumentNullException(nameof(user));

         await Task.Run(() => { _users.Update(user.Id, user); }, cancellationToken);


         return IdentityResult.Success;
      }

      public async Task<IdentityResult> DeleteAsync(
         TUser user,
         CancellationToken cancellationToken = default(CancellationToken)
      )
      {
         cancellationToken.ThrowIfCancellationRequested();
         ThrowIfDisposed();

         if (user == null) throw new ArgumentNullException(nameof(user));

         await Task.Run(() => { _users.Delete(user.Id); }, cancellationToken);

         return IdentityResult.Success;
      }

      public Task<TUser> FindByIdAsync(string userId, CancellationToken cancellationToken = default(CancellationToken))
      {
         cancellationToken.ThrowIfCancellationRequested();
         ThrowIfDisposed();

         cancellationToken.ThrowIfCancellationRequested();
         return Task.FromResult(_users.FindOne(u => u.Id == userId));
      }

      public Task<TUser> FindByNameAsync(string normalizedUserName,
         CancellationToken cancellationToken = default(CancellationToken))
      {
         cancellationToken.ThrowIfCancellationRequested();
         ThrowIfDisposed();

         var query = _users.Find(u => u.NormalizedUserName == normalizedUserName).FirstOrDefault();

         return Task.FromResult(query);
      }

      #endregion

      #region IUserLoginStore

      public Task AddLoginAsync(TUser user, UserLoginInfo login,
         CancellationToken cancellationToken = default(CancellationToken))
      {
         cancellationToken.ThrowIfCancellationRequested();
         ThrowIfDisposed();

         if (user == null) throw new ArgumentNullException(nameof(user));

         if (login == null) throw new ArgumentNullException(nameof(login));

         user.AddLogin(login);

         return Task.CompletedTask;
      }

      public Task RemoveLoginAsync(TUser user, string loginProvider, string providerKey,
         CancellationToken cancellationToken = default(CancellationToken))
      {
         cancellationToken.ThrowIfCancellationRequested();
         ThrowIfDisposed();

         if (user == null) throw new ArgumentNullException(nameof(user));

         if (loginProvider == null) throw new ArgumentNullException(nameof(loginProvider));

         if (providerKey == null) throw new ArgumentNullException(nameof(providerKey));

         user.RemoveLogin(new UserLoginInfo(loginProvider, providerKey, loginProvider));

         return Task.CompletedTask;
      }

      public Task<IList<UserLoginInfo>> GetLoginsAsync(TUser user,
         CancellationToken cancellationToken = default(CancellationToken))
      {
         cancellationToken.ThrowIfCancellationRequested();
         ThrowIfDisposed();

         if (user == null) throw new ArgumentNullException(nameof(user));

         return Task.FromResult<IList<UserLoginInfo>>(user.Logins.ToList());
      }

      public Task<TUser> FindByLoginAsync(string loginProvider, string providerKey,
         CancellationToken cancellationToken = default(CancellationToken))
      {
         cancellationToken.ThrowIfCancellationRequested();
         ThrowIfDisposed();

         if (loginProvider == null) throw new ArgumentNullException(nameof(loginProvider));

         if (providerKey == null) throw new ArgumentNullException(nameof(providerKey));

         var query = _users.Find(l =>
            l.Logins.Any(s => (s.LoginProvider == loginProvider) & (s.ProviderKey == providerKey)));

         return Task.FromResult(query.FirstOrDefault());
      }

      #endregion

      #region IUserPasswordStore

      public Task SetPasswordHashAsync(TUser user, string passwordHash,
         CancellationToken cancellationToken = default(CancellationToken))
      {
         cancellationToken.ThrowIfCancellationRequested();
         ThrowIfDisposed();

         if (user == null) throw new ArgumentNullException(nameof(user));

         user.PasswordHash = passwordHash;

         return Task.CompletedTask;
      }

      public Task<string> GetPasswordHashAsync(TUser user,
         CancellationToken cancellationToken = default(CancellationToken))
      {
         cancellationToken.ThrowIfCancellationRequested();
         ThrowIfDisposed();

         if (user == null) throw new ArgumentNullException(nameof(user));

         return Task.FromResult(user.PasswordHash);
      }

      public Task<bool> HasPasswordAsync(TUser user, CancellationToken cancellationToken = default(CancellationToken))
      {
         cancellationToken.ThrowIfCancellationRequested();
         ThrowIfDisposed();

         if (user == null) throw new ArgumentNullException(nameof(user));

         return Task.FromResult(user.PasswordHash != null);
      }

      #endregion

      #region IUserClaimStore

      public Task<IList<Claim>> GetClaimsAsync(TUser user,
         CancellationToken cancellationToken = default(CancellationToken))
      {
         cancellationToken.ThrowIfCancellationRequested();
         ThrowIfDisposed();

         if (user == null) throw new ArgumentNullException(nameof(user));

         return Task.FromResult<IList<Claim>>(user.Claims.Select(c => new Claim(c.Type, c.Value)).ToList());
      }

      public Task AddClaimsAsync(TUser user, IEnumerable<Claim> claims,
         CancellationToken cancellationToken = default(CancellationToken))
      {
         cancellationToken.ThrowIfCancellationRequested();
         ThrowIfDisposed();

         if (user == null) throw new ArgumentNullException(nameof(user));

         if (claims == null) throw new ArgumentNullException(nameof(claims));

         foreach (var claim in claims) user.AddClaim(claim);

         return Task.CompletedTask;
      }

      public Task ReplaceClaimAsync(TUser user, Claim claim, Claim newClaim,
         CancellationToken cancellationToken = default(CancellationToken))
      {
         cancellationToken.ThrowIfCancellationRequested();
         ThrowIfDisposed();

         if (user == null) throw new ArgumentNullException(nameof(user));

         if (claim == null) throw new ArgumentNullException(nameof(claim));

         if (newClaim == null) throw new ArgumentNullException(nameof(newClaim));

         user.RemoveClaim(claim);
         user.AddClaim(newClaim);

         return Task.CompletedTask;
      }

      public Task RemoveClaimsAsync(TUser user, IEnumerable<Claim> claims,
         CancellationToken cancellationToken = default(CancellationToken))
      {
         cancellationToken.ThrowIfCancellationRequested();
         ThrowIfDisposed();

         if (user == null) throw new ArgumentNullException(nameof(user));

         if (claims == null) throw new ArgumentNullException(nameof(claims));

         foreach (var claim in claims) user.RemoveClaim(claim);

         return Task.CompletedTask;
      }

      public Task<IList<TUser>> GetUsersForClaimAsync(Claim claim,
         CancellationToken cancellationToken = default(CancellationToken))
      {
         cancellationToken.ThrowIfCancellationRequested();
         ThrowIfDisposed();

         if (claim == null) throw new ArgumentNullException(nameof(claim));

         var query = _users.Find(l => l.Claims.Any(c => c.Type == claim.Type && c.Value == claim.Value));


         return Task.FromResult(query.ToList() as IList<TUser>);
      }

      #endregion

      #region IUserSecurityStampStore

      public Task SetSecurityStampAsync(TUser user, string stamp,
         CancellationToken cancellationToken = default(CancellationToken))
      {
         cancellationToken.ThrowIfCancellationRequested();
         ThrowIfDisposed();

         if (user == null) throw new ArgumentNullException(nameof(user));
         user.SecurityStamp = stamp ?? throw new ArgumentNullException(nameof(stamp));
         return Task.CompletedTask;
      }

      public Task<string> GetSecurityStampAsync(TUser user,
         CancellationToken cancellationToken = default(CancellationToken))
      {
         cancellationToken.ThrowIfCancellationRequested();
         ThrowIfDisposed();

         if (user == null) throw new ArgumentNullException(nameof(user));

         return Task.FromResult(user.SecurityStamp);
      }

      #endregion

      #region TokenTwoFactor

      public Task SetTwoFactorEnabledAsync(TUser user, bool enabled,
         CancellationToken cancellationToken = default(CancellationToken))
      {
         cancellationToken.ThrowIfCancellationRequested();
         ThrowIfDisposed();

         if (user == null) throw new ArgumentNullException(nameof(user));

         user.UsesTwoFactorAuthentication = enabled;
         user.TwoFactorEnabled = enabled;
         return Task.CompletedTask;
      }

      public Task<bool> GetTwoFactorEnabledAsync(TUser user,
         CancellationToken cancellationToken = default(CancellationToken))
      {
         cancellationToken.ThrowIfCancellationRequested();
         ThrowIfDisposed();

         if (user == null) throw new ArgumentNullException(nameof(user));

         return Task.FromResult(user.UsesTwoFactorAuthentication);
      }

      public Task SetAuthenticatorKeyAsync(TUser user, string key, CancellationToken cancellationToken) =>
         SetTokenAsync(user, AuthenticatorStoreLoginProvider, AuthenticatorKeyTokenName, key, cancellationToken);

      public Task<string> GetAuthenticatorKeyAsync(TUser user, CancellationToken cancellationToken) =>
         GetTokenAsync(user, AuthenticatorStoreLoginProvider, AuthenticatorKeyTokenName, cancellationToken);

      public Task SetTokenAsync(TUser user, string loginProvider, string name, string value,
         CancellationToken cancellationToken)
      {
         return Task.Run(() =>
         {
            var authToken = user.Tokens.SingleOrDefault(t => t.LoginProvider == loginProvider && t.TokenName == name);
            if (authToken == null)
            {
               SetTwoFactorEnabledAsync(user, true, cancellationToken);
               user.AddToken(new UserToken<string>
               {
                  TokenValue = value,
                  TokenName = name,
                  LoginProvider = loginProvider,
                  UserId = user.Id
               });
            }

            else
            {
               authToken.TokenValue = value;
            }
         }, cancellationToken);
      }

      public Task RemoveTokenAsync(TUser user, string loginProvider, string name, CancellationToken cancellationToken)
      {
         return Task.Run(() => user.RemoveToken(loginProvider, name), cancellationToken);
      }

      public Task<string> GetTokenAsync(TUser user, string loginProvider, string name,
         CancellationToken cancellationToken)
      {
         var tokenEntity =
            user.Tokens.SingleOrDefault(
               l =>
                  l.TokenName == name && l.LoginProvider == loginProvider &&
                  l.UserId == user.Id);
         return Task.FromResult(tokenEntity?.TokenValue);
      }

      public Task ReplaceCodesAsync(TUser user, IEnumerable<string> recoveryCodes, CancellationToken cancellationToken)
      {
         var mergedCodes = string.Join(";", recoveryCodes);
         return SetTokenAsync(user, AuthenticatorStoreLoginProvider, RecoveryCodeTokenName, mergedCodes,
            cancellationToken);
      }

      public async Task<bool> RedeemCodeAsync(TUser user, string code, CancellationToken cancellationToken)
      {
         var mergedCodes =
            await GetTokenAsync(user, AuthenticatorStoreLoginProvider, RecoveryCodeTokenName, cancellationToken) ?? "";
         var splitCodes = mergedCodes.Split(';');
         if (!splitCodes.Contains(code)) return false;
         var updatedCodes = new List<string>(splitCodes.Where(s => s != code));
         await ReplaceCodesAsync(user, updatedCodes, cancellationToken);
         return true;
      }

      public async Task<int> CountCodesAsync(TUser user, CancellationToken cancellationToken)
      {
         var mergedCodes =
            await GetTokenAsync(user, AuthenticatorStoreLoginProvider, RecoveryCodeTokenName, cancellationToken) ?? "";
         return mergedCodes.Length > 0 ? mergedCodes.Split(';').Length : 0;
      }

      #endregion

      #region IUserEmailStore

      public Task SetEmailAsync(TUser user, string email,
         CancellationToken cancellationToken = default(CancellationToken))
      {
         cancellationToken.ThrowIfCancellationRequested();
         ThrowIfDisposed();

         if (user == null) throw new ArgumentNullException(nameof(user));
         user.Email = email ?? throw new ArgumentNullException(nameof(email));

         return Task.CompletedTask;
      }

      public Task<string> GetEmailAsync(TUser user, CancellationToken cancellationToken = default(CancellationToken))
      {
         cancellationToken.ThrowIfCancellationRequested();
         ThrowIfDisposed();

         if (user == null) throw new ArgumentNullException(nameof(user));

         if (user.Email == null)
            throw new InvalidOperationException(
               "Cannot get the confirmation status of the e-mail since the user doesn't have an e-mail.");

         return Task.FromResult(user.Email?.Address);
      }

      public Task<bool> GetEmailConfirmedAsync(TUser user,
         CancellationToken cancellationToken = default(CancellationToken))
      {
         cancellationToken.ThrowIfCancellationRequested();
         ThrowIfDisposed();

         if (user == null) throw new ArgumentNullException(nameof(user));

         if (user.Email == null)
            throw new InvalidOperationException(
               "Cannot get the confirmation status of the e-mail since the user doesn't have an e-mail.");

         return Task.FromResult(user.Email.IsConfirmed);
      }

      public Task SetEmailConfirmedAsync(TUser user, bool confirmed,
         CancellationToken cancellationToken = default(CancellationToken))
      {
         cancellationToken.ThrowIfCancellationRequested();
         ThrowIfDisposed();

         if (user == null) throw new ArgumentNullException(nameof(user));

         if (user.Email == null)
            throw new InvalidOperationException(
               "Cannot set the confirmation status of the e-mail since the user doesn't have an e-mail.");

         user.Email.ConfirmationTime = confirmed
            ? (DateTime?)DateTime.UtcNow
            : null;

         return Task.CompletedTask;
      }

      public Task<TUser> FindByEmailAsync(string normalizedEmail,
         CancellationToken cancellationToken = default(CancellationToken))
      {
         cancellationToken.ThrowIfCancellationRequested();
         ThrowIfDisposed();

         return Task.FromResult(_users.FindOne(u => u.Email.NormalizedAddress == normalizedEmail));
      }

      public Task<string> GetNormalizedEmailAsync(TUser user,
         CancellationToken cancellationToken = default(CancellationToken))
      {
         cancellationToken.ThrowIfCancellationRequested();
         ThrowIfDisposed();

         if (user == null) throw new ArgumentNullException(nameof(user));

         return Task.FromResult(user.Email?.NormalizedAddress);
      }

      public Task SetNormalizedEmailAsync(TUser user, string normalizedEmail,
         CancellationToken cancellationToken = default(CancellationToken))
      {
         cancellationToken.ThrowIfCancellationRequested();
         ThrowIfDisposed();

         if (user == null) throw new ArgumentNullException(nameof(user));

         if (user.Email != null && normalizedEmail != null) user.Email.NormalizedAddress = normalizedEmail;

         return Task.CompletedTask;
      }

      #endregion

      #region IUserLockoutStore

      public Task<DateTimeOffset?> GetLockoutEndDateAsync(TUser user,
         CancellationToken cancellationToken = default(CancellationToken))
      {
         cancellationToken.ThrowIfCancellationRequested();
         ThrowIfDisposed();

         if (user == null) throw new ArgumentNullException(nameof(user));

         return Task.FromResult(user.Lockout?.EndDate);
      }

      public Task SetLockoutEndDateAsync(TUser user, DateTimeOffset? lockoutEnd,
         CancellationToken cancellationToken = default(CancellationToken))
      {
         cancellationToken.ThrowIfCancellationRequested();
         ThrowIfDisposed();

         if (user == null) throw new ArgumentNullException(nameof(user));

         if (user.Lockout == null) user.Lockout = new LockoutInfo();

         user.Lockout.EndDate = lockoutEnd;
         return Task.CompletedTask;
      }

      public Task<int> IncrementAccessFailedCountAsync(TUser user,
         CancellationToken cancellationToken = default(CancellationToken))
      {
         cancellationToken.ThrowIfCancellationRequested();
         ThrowIfDisposed();

         if (user == null) throw new ArgumentNullException(nameof(user));

         if (user.Lockout == null) user.Lockout = new LockoutInfo();

         var newAccessFailedCount = ++user.Lockout.AccessFailedCount;
         return Task.FromResult(newAccessFailedCount);
      }

      public Task ResetAccessFailedCountAsync(TUser user,
         CancellationToken cancellationToken = default(CancellationToken))
      {
         cancellationToken.ThrowIfCancellationRequested();
         ThrowIfDisposed();

         if (user == null) throw new ArgumentNullException(nameof(user));

         if (user.Lockout != null) user.Lockout.AccessFailedCount = 0;

         return Task.CompletedTask;
      }

      public Task<int> GetAccessFailedCountAsync(TUser user,
         CancellationToken cancellationToken = default(CancellationToken))
      {
         cancellationToken.ThrowIfCancellationRequested();
         ThrowIfDisposed();

         if (user == null) throw new ArgumentNullException(nameof(user));

         return Task.FromResult(user.Lockout?.AccessFailedCount ?? 0);
      }

      public Task<bool> GetLockoutEnabledAsync(TUser user,
         CancellationToken cancellationToken = default(CancellationToken))
      {
         cancellationToken.ThrowIfCancellationRequested();
         ThrowIfDisposed();

         if (user == null) throw new ArgumentNullException(nameof(user));

         return Task.FromResult(user.Lockout != null && user.Lockout.Enabled);
      }

      public Task SetLockoutEnabledAsync(TUser user, bool enabled,
         CancellationToken cancellationToken = default(CancellationToken))
      {
         cancellationToken.ThrowIfCancellationRequested();
         ThrowIfDisposed();

         if (user == null) throw new ArgumentNullException(nameof(user));

         if (user.Lockout == null) user.Lockout = new LockoutInfo();

         user.Lockout.Enabled = enabled;

         return Task.CompletedTask;
      }

      #endregion

      #region IUserPhoneNumberStore

      public Task SetPhoneNumberAsync(TUser user, string phoneNumber,
         CancellationToken cancellationToken = default(CancellationToken))
      {
         cancellationToken.ThrowIfCancellationRequested();
         ThrowIfDisposed();

         if (user == null) throw new ArgumentNullException(nameof(user));

         user.Phone = phoneNumber;
         return Task.CompletedTask;
      }

      public Task<string> GetPhoneNumberAsync(TUser user,
         CancellationToken cancellationToken = default(CancellationToken))
      {
         cancellationToken.ThrowIfCancellationRequested();
         ThrowIfDisposed();

         if (user == null) throw new ArgumentNullException(nameof(user));

         return Task.FromResult(user.Phone?.Number);
      }

      public Task<bool> GetPhoneNumberConfirmedAsync(TUser user,
         CancellationToken cancellationToken = default(CancellationToken))
      {
         cancellationToken.ThrowIfCancellationRequested();
         ThrowIfDisposed();

         if (user == null) throw new ArgumentNullException(nameof(user));

         if (user.Phone == null)
            throw new InvalidOperationException(
               "Cannot get the confirmation status of the phone number since the user doesn't have a phone number.");

         return Task.FromResult(user.Phone.IsConfirmed);
      }

      public Task SetPhoneNumberConfirmedAsync(TUser user, bool confirmed,
         CancellationToken cancellationToken = default(CancellationToken))
      {
         cancellationToken.ThrowIfCancellationRequested();
         ThrowIfDisposed();

         if (user == null) throw new ArgumentNullException(nameof(user));

         if (user.Phone == null)
            throw new InvalidOperationException(
               "Cannot set the confirmation status of the phone number since the user doesn't have a phone number.");

         user.Phone.ConfirmationTime = confirmed
            ? (DateTime?)DateTime.UtcNow
            : null;

         return Task.CompletedTask;
      }

      #endregion

      #region IDisposable

      private void ThrowIfDisposed()
      {
         if (_disposed) throw new ObjectDisposedException(GetType().Name);
      }

      private bool _disposed;

      public void Dispose()
      {
         Dispose(true);
         GC.SuppressFinalize(this);
      }

      protected virtual void Dispose(bool disposing)
      {
         if (_disposed)
            return;

         _disposed = true;
      }

      #endregion

      #region IUserRoleStore

      public Task AddToRoleAsync(TUser user, string roleName, CancellationToken cancellationToken)
      {
         cancellationToken.ThrowIfCancellationRequested();
         ThrowIfDisposed();
         if (user == null) throw new ArgumentNullException(nameof(user));
         if (roleName == null) throw new ArgumentNullException(nameof(roleName));

         user.Roles.Add(roleName);
         return Task.CompletedTask;
      }

      public Task RemoveFromRoleAsync(TUser user, string roleName, CancellationToken cancellationToken)
      {
         cancellationToken.ThrowIfCancellationRequested();
         ThrowIfDisposed();
         if (user == null) throw new ArgumentNullException(nameof(user));
         if (roleName == null) throw new ArgumentNullException(nameof(roleName));

         user.Roles.Remove(roleName);
         return Task.CompletedTask;
      }

      public Task<IList<string>> GetRolesAsync(TUser user, CancellationToken cancellationToken)
      {
         cancellationToken.ThrowIfCancellationRequested();
         ThrowIfDisposed();
         if (user == null) throw new ArgumentNullException(nameof(user));
         var result = user.Roles as IList<string>;
         return Task.FromResult(result);
      }

      public Task<bool> IsInRoleAsync(TUser user, string roleName, CancellationToken cancellationToken)
      {
         cancellationToken.ThrowIfCancellationRequested();
         ThrowIfDisposed();
         if (user == null) throw new ArgumentNullException(nameof(user));
         if (roleName == null) throw new ArgumentNullException(nameof(roleName));
         return Task.FromResult(user.Roles.Contains(roleName));
      }

      public Task<IList<TUser>> GetUsersInRoleAsync(string roleName, CancellationToken cancellationToken)
      {
         cancellationToken.ThrowIfCancellationRequested();
         ThrowIfDisposed();
         if (roleName == null) throw new ArgumentNullException(nameof(roleName));
         return Task.FromResult((IList<TUser>)_users.Find(u => u.Roles.Contains(roleName)).ToList());
      }
      #endregion
   }
}
