using FieldEmployeeManagement.Data;
using FieldEmployeeManagement.Models;
using Microsoft.Data.Sqlite;

namespace FieldEmployeeManagement.Controllers;

public class EmployeeController
{
    public List<Employee> GetAll(string search = "")
    {
        var employees = new List<Employee>();
        using var connection = new SqliteConnection(DatabaseInitializer.ConnectionString);
        connection.Open();

        using var command = connection.CreateCommand();
        command.CommandText = """
            SELECT Id, FullName, JobTitle, Department, Phone, Salary
            FROM Employees
            WHERE FullName LIKE $search OR JobTitle LIKE $search OR Department LIKE $search
            ORDER BY Id DESC;
            """;
        command.Parameters.AddWithValue("$search", $"%{search}%");

        using var reader = command.ExecuteReader();
        while (reader.Read())
        {
            employees.Add(new Employee
            {
                Id = reader.GetInt32(0),
                FullName = reader.GetString(1),
                JobTitle = reader.GetString(2),
                Department = reader.GetString(3),
                Phone = reader.GetString(4),
                Salary = Convert.ToDecimal(reader.GetDouble(5))
            });
        }
        return employees;
    }

    public void Add(Employee employee)
    {
        Validate(employee);
        using var connection = new SqliteConnection(DatabaseInitializer.ConnectionString);
        connection.Open();
        using var command = connection.CreateCommand();
        command.CommandText = """
            INSERT INTO Employees (FullName, JobTitle, Department, Phone, Salary)
            VALUES ($name, $title, $department, $phone, $salary);
            """;
        AddParameters(command, employee);
        command.ExecuteNonQuery();
    }

    public void Update(Employee employee)
    {
        if (employee.Id <= 0) throw new ArgumentException("Select an employee to update.");
        Validate(employee);
        using var connection = new SqliteConnection(DatabaseInitializer.ConnectionString);
        connection.Open();
        using var command = connection.CreateCommand();
        command.CommandText = """
            UPDATE Employees
            SET FullName=$name, JobTitle=$title, Department=$department,
                Phone=$phone, Salary=$salary
            WHERE Id=$id;
            """;
        AddParameters(command, employee);
        command.Parameters.AddWithValue("$id", employee.Id);
        command.ExecuteNonQuery();
    }

    public void Delete(int id)
    {
        if (id <= 0) throw new ArgumentException("Select an employee to delete.");
        using var connection = new SqliteConnection(DatabaseInitializer.ConnectionString);
        connection.Open();
        using var command = connection.CreateCommand();
        command.CommandText = "DELETE FROM Employees WHERE Id=$id;";
        command.Parameters.AddWithValue("$id", id);
        command.ExecuteNonQuery();
    }

    private static void AddParameters(SqliteCommand command, Employee employee)
    {
        command.Parameters.AddWithValue("$name", employee.FullName.Trim());
        command.Parameters.AddWithValue("$title", employee.JobTitle.Trim());
        command.Parameters.AddWithValue("$department", employee.Department.Trim());
        command.Parameters.AddWithValue("$phone", employee.Phone.Trim());
        command.Parameters.AddWithValue("$salary", employee.Salary);
    }

    private static void Validate(Employee employee)
    {
        if (string.IsNullOrWhiteSpace(employee.FullName) ||
            string.IsNullOrWhiteSpace(employee.JobTitle) ||
            string.IsNullOrWhiteSpace(employee.Department) ||
            string.IsNullOrWhiteSpace(employee.Phone))
            throw new ArgumentException("Please complete all employee fields.");

        if (employee.Salary < 0)
            throw new ArgumentException("Salary cannot be negative.");
    }
}
