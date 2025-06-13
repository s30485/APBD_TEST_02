# APBD_HW_11 Secure REST API

## Configuration

Before running the application, create an `appsettings.json` file in the project root with the following structure:

```json
{
  "Logging": {
    "LogLevel": {
      "Default": "Debug",
      "System": "Information",
      "Microsoft": "Information"
    }
  },
  "Jwt": {
    "Issuer": "http://localhost:5300/",
    "Audience": "http://localhost:5300/",
    "SecretKey": "your-secret-key",
    "ExpiryMinutes": 60
  },
  "ConnectionStrings": {
    "DefaultConnection": "your-conn-str"
  }
}
```

if you want to initialize the project with one admin user (impossible to create an user with admin perms from non-admin user level) then paste this snippet in line 56 in Program.cs
```json
using (var scope = app.Services.CreateScope())
{
    var db = scope.ServiceProvider.GetRequiredService<MasterContext>();
    var hasher = new PasswordHasher<Account>();

    if (!db.Accounts.Any(a => a.Username == "admin"))
    {
        var account = new Account
        {
            Username = "admin",
            Password = hasher.HashPassword(null!, "YOURPASSWORD"), // at least 12 chars
            RoleId = 1,
            EmployeeId = null
        };
        db.Accounts.Add(account);
        db.SaveChanges();
    }
}
```

# Important: password must comply with criteria
