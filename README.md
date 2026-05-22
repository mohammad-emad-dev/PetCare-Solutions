# PetCare Solutions

PetCare Solutions is a Windows Forms solution for managing veterinary clinic operations and pet owner booking requests.

## Projects

| Project | Purpose | Startup form |
| --- | --- | --- |
| PetCare.ClinicManagement | Clinic/admin application for users, owners, pets, veterinarians, appointments, medical records, and booking approvals. | `LoginForm` |
| PetCare.OwnerBooking | Pet owner application for phone-number login and appointment request submission. | `OwnerLoginForm` |

## Technology

- C#
- .NET Framework 4.7.2
- Windows Forms
- SQL Server
- Visual Studio 2019 or newer

## Repository Structure

```text
PetCareSolutions.sln
src/
  PetCare.ClinicManagement/
    PetCare.ClinicManagement.csproj
  PetCare.OwnerBooking/
    PetCare.OwnerBooking.csproj
database/
  PetCareSolutions.sql
docs/
  MAINTENANCE.md
  screenshots/
```

The root `PetCareSolutions.sln` is the main entry point for development.

## Database Setup

1. Open SQL Server Management Studio.
2. Run `database/PetCareSolutions.sql`.
3. Confirm the `PetCareSolutions` database was created.
4. Update both `App.config` files if your SQL Server instance is not `MOHAMMAD`:

```xml
<connectionStrings>
  <add name="bitcINTERFACE.Properties.Settings.PetCareSolutionsConnectionString"
       connectionString="Data Source=MOHAMMAD;Initial Catalog=PetCareSolutions;Integrated Security=True;Encrypt=True;TrustServerCertificate=True"
       providerName="System.Data.SqlClient" />
</connectionStrings>
```

The owner app uses the connection name:

```text
BookingRequests_System.Properties.Settings.PetCareSolutionsConnectionString
```

## Build and Run

1. Open `PetCareSolutions.sln` in Visual Studio.
2. Build the solution using `Build > Build Solution`.
3. Set the desired startup project:
   - `PetCare.ClinicManagement` for clinic/admin workflows.
   - `PetCare.OwnerBooking` for owner booking workflows.
4. Press `F5`.

Command-line build with Visual Studio MSBuild:

```powershell
& "C:\Program Files\Microsoft Visual Studio\18\Community\MSBuild\Current\Bin\MSBuild.exe" PetCareSolutions.sln /t:Build /p:Configuration=Release
```

## Notes

- Generated folders such as `.vs`, `bin`, and `obj` are intentionally ignored.
- Connection strings are read through `DatabaseConfig` in each application.
- Clinic user passwords are stored in the `passwordHash` column using PBKDF2 hashes. Existing legacy plain-text passwords are upgraded automatically after a successful login.
