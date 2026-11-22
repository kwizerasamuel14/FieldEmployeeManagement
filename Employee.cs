namespace FieldEmployeeManagement.Models;

public class Employee
{
    public int Id { get; set; }
    public string FullName { get; set; } = "";
    public string JobTitle { get; set; } = "";
    public string Department { get; set; } = "";
    public string Phone { get; set; } = "";
    public decimal Salary { get; set; }
}
