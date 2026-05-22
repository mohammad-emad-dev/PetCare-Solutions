# BookingRequests System

This is the pet owner Windows Forms application. It lets an owner sign in with a registered phone number, choose one of their pets, and submit a booking request.

Open the root solution instead of this project directly when possible:

```text
..\..\PetCareSolutions.sln
```

The database connection is read from `BookingRequests System\App.config` through `DatabaseConfig`.

Build from the repository root:

```powershell
& "C:\Program Files\Microsoft Visual Studio\18\Community\MSBuild\Current\Bin\MSBuild.exe" PetCareSolutions.sln /t:Build /p:Configuration=Release
```
