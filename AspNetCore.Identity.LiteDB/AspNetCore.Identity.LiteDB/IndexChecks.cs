using LiteDB;
using Microsoft.AspNetCore.Identity;

namespace AspNetCore.Identity.LiteDB
{
    public class IndexChecks
    {
        public static void EnsureUniqueIndexOnUserName<TUser>(LiteCollection<TUser> users)
            where TUser : IdentityUser, new()
        {
            users.EnsureIndex(u => u.UserName, true);
        }

        public static void EnsureUniqueIndexOnRoleName<TRole>(LiteCollection<TRole> roles)
            where TRole : IdentityRole, new()
        {
            roles.EnsureIndex(r => r.Name, true);
        }

        public static void EnsureUniqueIndexOnEmail<TUser>(LiteCollection<TUser> users)
            where TUser : IdentityUser, new()
        {
            users.EnsureIndex(u => u.Email, true);
        }
    }
}