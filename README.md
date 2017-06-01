# AspNetCore.Identity.LiteDB
A LiteDB provider for ASP.NET Core Identity framework.

# Nuget Package
https://www.nuget.org/packages/AspNetCore.Identity.LiteDB

# How to use
You have to add the following lines to your Startup.cs in the ASP.NET Core Project.
```C#
public void ConfigureServices(IServiceCollection services)
{

// Add LiteDB Dependency
services.AddSingleton<LiteDbContext>();

services.AddIdentity<ApplicationUser, IdentityRole>(options =>
                {
                    options.Password.RequireDigit = false;
                    options.Password.RequireUppercase = false;
                    options.Password.RequireLowercase = false;
                    options.Password.RequireNonAlphanumeric = false;
                    options.Password.RequiredLength = 6;
                })
.AddUserStore<LiteDbUserStore<ApplicationUser>>()
.AddRoleStore<LiteDbRoleStore<IdentityRole>>()
.AddDefaultTokenProviders();

}
```

Also make sure you use the right using Statements in Startup.cs, AccountController, ManageController and Values Controller.

```C#
using AspNetCore.Identity.LiteDB;
using AspNetCore.Identity.LiteDB.Models;
```
