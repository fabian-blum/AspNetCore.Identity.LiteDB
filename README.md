# AspNetCore.Identity.LiteDB


## Changes from fabian-blum/AspNetCore.Identity.LiteDB
- Compiled against latest LiteDB to avoid signature mismatch issue (https://github.com/fabian-blum/AspNetCore.Identity.LiteDB/issues/11)
- Added support for ``IQueryableUserStore<TUser>`` (can now use `UserManager<TUser>.Users` to list all users)
- Compiled for .net standard 2.1

# Nuget Package
https://www.nuget.org/packages/AspNetCore.Identity.LiteDB

# How to use
You have to add the following lines to your Startup.cs in the ASP.NET Core Project.

--> See Demo Project

Also make sure you use the right using Statements in Startup.cs, AccountController, ManageController and Values Controller.

```C#
using AspNetCore.Identity.LiteDB;
using AspNetCore.Identity.LiteDB.Models;
```
