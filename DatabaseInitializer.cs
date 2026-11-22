using Microsoft.Data.Sqlite;

namespace FieldEmployeeManagement.Data;

public static class DatabaseInitializer
{
    public static string DatabasePath { get; } = Path.Combine(
        Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData),
        "FieldEmployeeManagement",
        "field_employees.db");

    public static string ConnectionString => $"Data Source={DatabasePath}";

    public static void Initialize()
    {
        var folder = Path.GetDirectoryName(DatabasePath)!;
        Directory.CreateDirectory(folder);

        using var connection = new SqliteConnection(ConnectionString);
        connection.Open();

        using var command = connection.CreateCommand();
        command.CommandText = """
            CREATE TABLE IF NOT EXISTS Employees (
                Id INTEGER PRIMARY KEY AUTOINCREMENT,
                FullName TEXT NOT NULL,
                JobTitle TEXT NOT NULL,
                Department TEXT NOT NULL,
                Phone TEXT NOT NULL,
                Salary REAL NOT NULL
            );
            """;
        command.ExecuteNonQuery();
    }
}
