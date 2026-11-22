using FieldEmployeeManagement.Data;
using FieldEmployeeManagement.Controllers;
using FieldEmployeeManagement.Views;

namespace FieldEmployeeManagement;

internal static class Program
{
    [STAThread]
    private static void Main()
    {
        ApplicationConfiguration.Initialize();
        DatabaseInitializer.Initialize();
        var controller = new EmployeeController();
        Application.Run(new MainForm(controller));
    }
}
