# PetCare Solutions

PetCare Solutions is a Windows Forms solution for managing a veterinary clinic and letting pet owners submit booking requests.

## Projects

| Project | Purpose | Startup form |
| --- | --- | --- |
| PetCare Booker | Clinic/admin application for users, owners, pets, veterinarians, appointments, medical records, and booking approvals. | `login` |
| BookingRequests System | Pet owner application for phone-number login and appointment request submission. | `LOGIN_ONWERS` |

## Technology

- C#
- .NET Framework 4.7.2
- Windows Forms
- SQL Server
- Visual Studio 2019 or newer

## Folder Structure

```text
PetCareSolutions.sln
SQL PetCare Solutions.sql
PetCare Booker/
  bitcINTERFACE/
    bitcINTERFACE.sln
    bitcINTERFACE/
BookingRequests System/
  BookingRequests System/
    BookingRequests System.sln
    BookingRequests System/
screenshots/
```

The root `PetCareSolutions.sln` opens both applications together. The older per-project solution files are kept for compatibility.

## Database Setup

1. Open SQL Server Management Studio.
2. Run `SQL PetCare Solutions.sql`.
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
2. Restore/build the solution using `Build > Build Solution`.
3. Set the desired startup project:
   - `PetCare Booker` for clinic/admin workflows.
   - `BookingRequests System` for owner booking workflows.
4. Press `F5`.

Command-line build with Visual Studio MSBuild:

```powershell
& "C:\Program Files\Microsoft Visual Studio\18\Community\MSBuild\Current\Bin\MSBuild.exe" PetCareSolutions.sln /t:Build /p:Configuration=Release
```

## Notes

- Generated folders such as `.vs`, `bin`, and `obj` are intentionally ignored.
- Connection strings are read through `DatabaseConfig` in each application.
- Password fields are still stored in the legacy `passwordHash` column format. Real password hashing should be a future security upgrade.
