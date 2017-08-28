using LiteDB;
using Microsoft.AspNetCore.Identity;

namespace AspNetCore.Identity.LiteDB
{

    public class IdentityRole : IdentityRole<string>
    {
        public IdentityRole()
        {
            Id = ObjectId.NewObjectId().ToString();
        }

        public IdentityRole(string roleName) : this()
        {
            Name = roleName;
        }

        [BsonId]
        public string Id { get; set; }

        public string Name { get; set; }
    }

}