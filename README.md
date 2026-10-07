# EcommerceStore (ASP.NET Core MVC)

Simple e-commerce demo: product list with search and category filter, product details,
session-based shopping cart, and checkout that saves orders to a database.

**Stack:** ASP.NET Core MVC (.NET 10), C#, Entity Framework Core, SQLite or SQL Server, Bootstrap.

## Run locally
1. Install the .NET 10 SDK.
2. In this folder run: `dotnet run`
3. Open the `http://localhost:xxxx` link shown in the terminal.

The database is created automatically with 6 sample products.
By default it uses SQLite (file `ecommerce.db`, nothing to install).

## Use SQL Server instead
In `appsettings.json` set `"DatabaseProvider": "SqlServer"` and edit the `SqlServer` connection string
(LocalDB is included with Visual Studio). Delete `ecommerce.db` if it exists.

## Not included yet
User authentication and the admin dashboard (product management).
