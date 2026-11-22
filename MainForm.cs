using FieldEmployeeManagement.Controllers;
using FieldEmployeeManagement.Models;

namespace FieldEmployeeManagement.Views;

public class MainForm : Form
{
    private readonly EmployeeController _controller;
    private readonly TextBox _name = new();
    private readonly TextBox _title = new();
    private readonly TextBox _department = new();
    private readonly TextBox _phone = new();
    private readonly NumericUpDown _salary = new();
    private readonly TextBox _search = new();
    private readonly DataGridView _grid = new();
    private int _selectedId;

    public MainForm(EmployeeController controller)
    {
        _controller = controller;
        Text = "Field Employee Management System";
        StartPosition = FormStartPosition.CenterScreen;
        MinimumSize = new Size(900, 600);
        Size = new Size(1100, 720);
        BuildInterface();
        LoadEmployees();
    }

    private void BuildInterface()
    {
        var heading = new Label
        {
            Text = "FIELD EMPLOYEE MANAGEMENT SYSTEM",
            Dock = DockStyle.Top,
            Height = 55,
            TextAlign = ContentAlignment.MiddleCenter,
            Font = new Font("Segoe UI", 16, FontStyle.Bold),
            BackColor = Color.FromArgb(31, 78, 121),
            ForeColor = Color.White
        };
        Controls.Add(heading);

        var content = new TableLayoutPanel
        {
            Dock = DockStyle.Fill,
            Padding = new Padding(18),
            ColumnCount = 1,
            RowCount = 3
        };
        content.RowStyles.Add(new RowStyle(SizeType.Absolute, 175));
        content.RowStyles.Add(new RowStyle(SizeType.Absolute, 55));
        content.RowStyles.Add(new RowStyle(SizeType.Percent, 100));
        Controls.Add(content);
        content.BringToFront();

        var fields = new TableLayoutPanel { Dock = DockStyle.Fill, ColumnCount = 4, RowCount = 2 };
        for (int i = 0; i < 4; i++) fields.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 25));
        fields.RowStyles.Add(new RowStyle(SizeType.Absolute, 72));
        fields.RowStyles.Add(new RowStyle(SizeType.Absolute, 72));
        AddField(fields, "Full name", _name, 0, 0);
        AddField(fields, "Job title", _title, 1, 0);
        AddField(fields, "Department / Site", _department, 2, 0);
        AddField(fields, "Phone", _phone, 3, 0);
        _salary.Maximum = 100000000;
        _salary.DecimalPlaces = 2;
        _salary.ThousandsSeparator = true;
        AddField(fields, "Monthly salary", _salary, 0, 1);
        content.Controls.Add(fields, 0, 0);

        var actions = new FlowLayoutPanel { Dock = DockStyle.Fill, FlowDirection = FlowDirection.LeftToRight };
        actions.Controls.Add(MakeButton("Add Employee", (_, _) => AddEmployee()));
        actions.Controls.Add(MakeButton("Update Selected", (_, _) => UpdateEmployee()));
        actions.Controls.Add(MakeButton("Delete Selected", (_, _) => DeleteEmployee()));
        actions.Controls.Add(MakeButton("Clear", (_, _) => ClearFields()));
        content.Controls.Add(actions, 0, 1);

        var lower = new TableLayoutPanel { Dock = DockStyle.Fill, ColumnCount = 1, RowCount = 2 };
        lower.RowStyles.Add(new RowStyle(SizeType.Absolute, 42));
        lower.RowStyles.Add(new RowStyle(SizeType.Percent, 100));
        var searchPanel = new FlowLayoutPanel { Dock = DockStyle.Fill };
        _search.Width = 300;
        _search.PlaceholderText = "Search by name, title or department...";
        searchPanel.Controls.Add(_search);
        searchPanel.Controls.Add(MakeButton("Search", (_, _) => LoadEmployees(_search.Text)));
        searchPanel.Controls.Add(MakeButton("Show All", (_, _) => { _search.Clear(); LoadEmployees(); }));
        lower.Controls.Add(searchPanel, 0, 0);

        _grid.Dock = DockStyle.Fill;
        _grid.ReadOnly = true;
        _grid.AllowUserToAddRows = false;
        _grid.AllowUserToDeleteRows = false;
        _grid.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
        _grid.MultiSelect = false;
        _grid.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
        _grid.SelectionChanged += (_, _) => CopySelectedEmployee();
        lower.Controls.Add(_grid, 0, 1);
        content.Controls.Add(lower, 0, 2);
    }

    private static void AddField(TableLayoutPanel panel, string label, Control input, int column, int row)
    {
        var container = new Panel { Dock = DockStyle.Fill, Padding = new Padding(5) };
        var caption = new Label { Text = label, Dock = DockStyle.Top, Height = 23, Font = new Font("Segoe UI", 9, FontStyle.Bold) };
        input.Dock = DockStyle.Top;
        container.Controls.Add(input);
        container.Controls.Add(caption);
        panel.Controls.Add(container, column, row);
    }

    private static Button MakeButton(string text, EventHandler click)
    {
        var button = new Button { Text = text, AutoSize = true, Height = 34, Padding = new Padding(10, 3, 10, 3), Margin = new Padding(5) };
        button.Click += click;
        return button;
    }

    private Employee ReadForm() => new()
    {
        Id = _selectedId,
        FullName = _name.Text,
        JobTitle = _title.Text,
        Department = _department.Text,
        Phone = _phone.Text,
        Salary = _salary.Value
    };

    private void AddEmployee()
    {
        try { _controller.Add(ReadForm()); LoadEmployees(); ClearFields(); MessageBox.Show("Employee added successfully."); }
        catch (Exception ex) { MessageBox.Show(ex.Message, "Unable to add employee", MessageBoxButtons.OK, MessageBoxIcon.Warning); }
    }

    private void UpdateEmployee()
    {
        try { _controller.Update(ReadForm()); LoadEmployees(); ClearFields(); MessageBox.Show("Employee updated successfully."); }
        catch (Exception ex) { MessageBox.Show(ex.Message, "Unable to update employee", MessageBoxButtons.OK, MessageBoxIcon.Warning); }
    }

    private void DeleteEmployee()
    {
        if (_selectedId == 0) { MessageBox.Show("Select an employee first."); return; }
        if (MessageBox.Show("Delete the selected employee?", "Confirm deletion", MessageBoxButtons.YesNo, MessageBoxIcon.Question) != DialogResult.Yes) return;
        try { _controller.Delete(_selectedId); LoadEmployees(); ClearFields(); }
        catch (Exception ex) { MessageBox.Show(ex.Message, "Unable to delete employee"); }
    }

    private void LoadEmployees(string search = "")
    {
        _grid.DataSource = _controller.GetAll(search);
        if (_grid.Columns["Id"] is not null) _grid.Columns["Id"]!.HeaderText = "Employee ID";
        if (_grid.Columns["FullName"] is not null) _grid.Columns["FullName"]!.HeaderText = "Full Name";
        if (_grid.Columns["JobTitle"] is not null) _grid.Columns["JobTitle"]!.HeaderText = "Job Title";
        if (_grid.Columns["Department"] is not null) _grid.Columns["Department"]!.HeaderText = "Department / Site";
    }

    private void CopySelectedEmployee()
    {
        if (_grid.CurrentRow?.DataBoundItem is not Employee employee) return;
        _selectedId = employee.Id;
        _name.Text = employee.FullName;
        _title.Text = employee.JobTitle;
        _department.Text = employee.Department;
        _phone.Text = employee.Phone;
        _salary.Value = Math.Min(_salary.Maximum, Math.Max(_salary.Minimum, employee.Salary));
    }

    private void ClearFields()
    {
        _selectedId = 0;
        _name.Clear();
        _title.Clear();
        _department.Clear();
        _phone.Clear();
        _salary.Value = 0;
        _grid.ClearSelection();
    }
}
