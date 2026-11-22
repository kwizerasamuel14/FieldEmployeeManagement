# Field Employee Management System

A C# Windows Forms desktop application demonstrating MVC architecture and database-backed CRUD operations.

## Features
- Add employee records
- View employee records in a table
- Update selected employee records
- Delete employee records
- Search by employee name, job title, or department/site
- SQLite database created automatically on first launch

## Technologies
- C#
- .NET 8 Windows Forms
- SQLite (`Microsoft.Data.Sqlite`)
- MVC architecture

## Requirements
- Windows 10 or Windows 11
- Visual Studio 2022
- Workload: **.NET desktop development**
- .NET 8 SDK

## Run the project
1. Open `FieldEmployeeManagement.csproj` in Visual Studio.
2. Allow NuGet packages to restore.
3. Set `FieldEmployeeManagement` as the startup project if prompted.
4. Press **F5** or click **Start**.

The application creates its SQLite database automatically at:
`%LOCALAPPDATA%/FieldEmployeeManagement/field_employees.db`

## MVC structure
- `Models/Employee.cs` — employee data model.
- `Views/MainForm.cs` — Windows Forms user interface.
- `Controllers/EmployeeController.cs` — application logic and CRUD operations.
- `Data/DatabaseInitializer.cs` — database creation and connection configuration.

## Database
The application uses SQLite, so a separate database server is not required. The database table is created automatically when the program starts. The SQL definition is also included in `database/schema.sql`.

## Notes
This is a source-code submission project. Build and run it on Windows with the .NET desktop development workload installed. Do not commit `bin/`, `obj/`, `.vs/`, or your local database file.
