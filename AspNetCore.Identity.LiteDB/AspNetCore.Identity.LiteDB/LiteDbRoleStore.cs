using AspNetCore.Identity.LiteDB.Data;
using LiteDB;
using Microsoft.AspNetCore.Identity;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace AspNetCore.Identity.LiteDB
{
    public class LiteDbRoleStore<TRole> : IRoleStore<TRole>, IQueryableRoleStore<TRole>
        where TRole : IdentityRole, new()
    {
        private readonly LiteCollection<TRole> _roles;

        public LiteDbRoleStore(LiteDbContext dbContext)
        {
            _roles = dbContext.LiteDatabase.GetCollection<TRole>("roles");
        }

        public virtual void Dispose()
        {
            // no need to dispose of anything, mongodb handles connection pooling automatically
        }

        public virtual Task CreateAsync(TRole role)
        {
            return Task.FromResult(_roles.Insert(role));
        }

        public virtual Task UpdateAsync(TRole role)
        {
            return Task.FromResult(_roles.Update(role.Id, role));
        }

        public virtual Task DeleteAsync(TRole role)
        {
            return Task.FromResult(_roles.Delete(role.Id));
        }

        public virtual Task<TRole> FindByIdAsync(string roleId)
        {
            return Task.FromResult(_roles.FindOne(r => r.Id == roleId));
        }

        public virtual Task<TRole> FindByNameAsync(string roleName)
        {
            return Task.FromResult(_roles.FindOne(r => r.Name == roleName));
        }

        public virtual IQueryable<TRole> Roles => _roles.FindAll().AsQueryable();
        public Task<IdentityResult> CreateAsync(TRole role, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }

        public Task<IdentityResult> UpdateAsync(TRole role, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }

        public Task<IdentityResult> DeleteAsync(TRole role, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }

        public Task<string> GetRoleIdAsync(TRole role, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }

        public Task<string> GetRoleNameAsync(TRole role, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }

        public Task SetRoleNameAsync(TRole role, string roleName, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }

        public Task<string> GetNormalizedRoleNameAsync(TRole role, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }

        public Task SetNormalizedRoleNameAsync(TRole role, string normalizedName, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }

        public Task<TRole> FindByIdAsync(string roleId, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }

        public Task<TRole> FindByNameAsync(string normalizedRoleName, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }
    }
}