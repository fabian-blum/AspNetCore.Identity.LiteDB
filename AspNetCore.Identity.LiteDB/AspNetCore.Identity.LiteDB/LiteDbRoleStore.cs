﻿using System;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Runtime.InteropServices;
using System.Threading;
using System.Threading.Tasks;
using AspNetCore.Identity.LiteDB.Data;
using LiteDB;
using Microsoft.AspNetCore.Identity;
using Microsoft.Win32.SafeHandles;

namespace AspNetCore.Identity.LiteDB
{
   [SuppressMessage("ReSharper", "UnusedMember.Global")]
   public class LiteDbRoleStore<TRole> : IQueryableRoleStore<TRole>, IRoleStore<TRole>
      where TRole : IdentityRole, new()
   {
      private readonly ILiteCollection<CancellationToken> _cancellationTokens;
      private readonly ILiteCollection<TRole> _roles;

      public LiteDbRoleStore(ILiteDbContext dbContext)
      {
         _roles = dbContext.LiteDatabase.GetCollection<TRole>("roles");
         _cancellationTokens = dbContext.LiteDatabase.GetCollection<CancellationToken>("cancellationtokens");
      }

      public virtual IQueryable<TRole> Roles => _roles.FindAll().AsQueryable();

      public async Task<IdentityResult> CreateAsync(TRole role, CancellationToken cancellationToken)
      {
         cancellationToken.ThrowIfCancellationRequested();
         ThrowIfDisposed();

         if (role == null) throw new ArgumentNullException(nameof(role));

         await Task.Run(() => { _roles.Insert(role); }, cancellationToken);

         return IdentityResult.Success;
      }

      public async Task<IdentityResult> UpdateAsync(TRole role, CancellationToken cancellationToken)
      {
         cancellationToken.ThrowIfCancellationRequested();
         ThrowIfDisposed();

         if (role == null) throw new ArgumentNullException(nameof(role));

         await Task.Run(() => { _roles.Update(role.Id, role); }, cancellationToken);

         return IdentityResult.Success;
      }

      public async Task<IdentityResult> DeleteAsync(TRole role, CancellationToken cancellationToken)
      {
         cancellationToken.ThrowIfCancellationRequested();
         ThrowIfDisposed();

         if (role == null) throw new ArgumentNullException(nameof(role));

         await Task.Run(() => { _roles.Delete(role.Id); }, cancellationToken);

         return IdentityResult.Success;
      }

      public Task<string> GetRoleIdAsync(TRole role, CancellationToken cancellationToken)
      {
         cancellationToken.ThrowIfCancellationRequested();
         ThrowIfDisposed();

         if (role == null) throw new ArgumentNullException(nameof(role));

         return Task.FromResult(role.Id);
      }

      public Task<string> GetRoleNameAsync(TRole role, CancellationToken cancellationToken)
      {
         cancellationToken.ThrowIfCancellationRequested();
         ThrowIfDisposed();

         if (role == null) throw new ArgumentNullException(nameof(role));

         return Task.FromResult(role.Name);
      }

      public Task SetRoleNameAsync(TRole role, string roleName, CancellationToken cancellationToken)
      {
         cancellationToken.ThrowIfCancellationRequested();
         ThrowIfDisposed();

         if (role == null) throw new ArgumentNullException(nameof(role));
         role.Name = roleName ?? throw new ArgumentNullException(nameof(roleName));

         return Task.CompletedTask;
      }

      public Task<string> GetNormalizedRoleNameAsync(TRole role, CancellationToken cancellationToken)
      {
         cancellationToken.ThrowIfCancellationRequested();
         ThrowIfDisposed();

         if (role == null) throw new ArgumentNullException(nameof(role));

         return Task.FromResult(role.NormalizedName);
      }

      public Task SetNormalizedRoleNameAsync(TRole role, string normalizedName, CancellationToken cancellationToken)
      {
         cancellationToken.ThrowIfCancellationRequested();
         ThrowIfDisposed();

         if (role == null) throw new ArgumentNullException(nameof(role));
         role.NormalizedName = normalizedName ?? throw new ArgumentNullException(nameof(normalizedName));

         return Task.CompletedTask;
      }

      public Task<TRole> FindByIdAsync(string roleId, CancellationToken cancellationToken)
      {
         cancellationToken.ThrowIfCancellationRequested();
         ThrowIfDisposed();

         return Task.FromResult(_roles.FindOne(u => u.Id == roleId));
      }

      public Task<TRole> FindByNameAsync(string normalizedRoleName, CancellationToken cancellationToken)
      {
         cancellationToken.ThrowIfCancellationRequested();
         ThrowIfDisposed();

         var query = _roles.Find(r => r.NormalizedName == normalizedRoleName).FirstOrDefault();

         return Task.FromResult(query);
      }

      public Task SaveChanges(
         CancellationToken cancellationToken = default(CancellationToken)
      )
      {
         _cancellationTokens.Insert(cancellationToken);
         return Task.FromResult(cancellationToken);
      }

      public virtual Task CreateAsync(TRole role) => Task.FromResult(_roles.Insert(role));

      public virtual Task UpdateAsync(TRole role) => Task.FromResult(_roles.Update(role.Id, role));

      public virtual Task DeleteAsync(TRole role) => Task.FromResult(_roles.Delete(role.Id));

      public virtual Task<TRole> FindByIdAsync(string roleId)
      {
         return Task.FromResult(_roles.FindOne(r => r.Id == roleId));
      }

      public virtual Task<TRole> FindByNameAsync(string roleName)
      {
         return Task.FromResult(_roles.FindOne(r => r.Name == roleName));
      }

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
   }
}
