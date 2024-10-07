using Dapper;
using Domain.Entities;
using Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Npgsql;

namespace Infrastructure.Services;

public class EmployeeService : IEmployeeService
{
    private readonly DataContext _context;
    private readonly string? _configuration;

    public EmployeeService(DataContext context, IConfiguration configuration)
    {
        _context = context;
        _configuration = configuration.GetConnectionString("Default");
    }

    public async Task<Employee> CreateEmployeeAsync(Employee employee)
    {
        employee.Id = Guid.NewGuid();
        employee.CreatedAt = DateTime.UtcNow;
        employee.UpdatedAt = DateTime.UtcNow;
        _context.Employees.Add(employee);
        await _context.SaveChangesAsync();

        return employee;
    }

    public async Task<Employee?> GetEmployeeByIdAsync(Guid id)
    {
        return await _context.Employees.FindAsync(id);
    }

    public async Task<List<Employee>> GetAllEmployeesAsync()
    {
        return await _context.Employees.ToListAsync();
    }

    public async Task<bool> UpdateEmployeeAsync(Employee updatedEmployee)
    {
        var employee = await _context.Employees.FindAsync(updatedEmployee.Id);

        if (employee == null)
        {
            return false;
        }

        employee.FirstName = updatedEmployee.FirstName;
        employee.LastName = updatedEmployee.LastName;
        employee.Email = updatedEmployee.Email;
        employee.Phone = updatedEmployee.Phone;
        employee.DateOfBirth = updatedEmployee.DateOfBirth;
        employee.HireDate = updatedEmployee.HireDate;
        employee.Position = updatedEmployee.Position;
        employee.Salary = updatedEmployee.Salary;
        employee.DepartmentId = updatedEmployee.DepartmentId;
        employee.ManagerId = updatedEmployee.ManagerId;
        employee.IsActive = updatedEmployee.IsActive;
        employee.Address = updatedEmployee.Address;
        employee.City = updatedEmployee.City;
        employee.Country = updatedEmployee.Country;
        employee.UpdatedAt = DateTime.UtcNow;

        await _context.SaveChangesAsync();
        return true;
    }

    public async Task<bool> DeleteEmployeeAsync(Guid id)
    {
        var employee = await _context.Employees.FindAsync(id);
        if (employee == null)
        {
            return false;
        }

        _context.Employees.Remove(employee);
        await _context.SaveChangesAsync();
        return true;
    }

    public async Task<List<Employee>> GetEmployeesByStatusAsync(bool isActive)
    {
        try
        {
            await using (NpgsqlConnection connection = new NpgsqlConnection(_configuration))
            {
                await connection.OpenAsync();
                var sql = @"SELECT * FROM ""Employees"" WHERE ""IsActive"" = @IsActive";
                var employees = await connection.QueryAsync<Employee>(
                    sql,
                    new { IsActive = isActive }
                );
                return employees.ToList();
            }
        }
        catch (Exception e)
        {
            Console.WriteLine(e.Message);
            throw;
        }
    }

    public async Task<List<Employee>> GetEmployeesByDepartmentAsync(Guid departmentId)
    {
        try
        {
            await using (NpgsqlConnection connection = new NpgsqlConnection(_configuration))
            {
                await connection.OpenAsync();
                var sql = @"SELECT * FROM ""Employees"" WHERE ""DepartmentId"" = @DepartmentId";
                var employees = await connection.QueryAsync<Employee>(
                    sql,
                    new { DepartmentId = departmentId }
                );
                return employees.ToList();
            }
        }
        catch (Exception e)
        {
            Console.WriteLine(e.Message);
            throw;
        }
    }

    public async Task<decimal> GetAverageSalaryAsync()
    {
        try
        {
            await using (NpgsqlConnection connection = new NpgsqlConnection(_configuration))
            {
                await connection.OpenAsync();
                var sql = @"SELECT AVG(""Salary"") FROM ""Employees"" WHERE ""Salary"" > 0";
                var averageSalary = await connection.ExecuteScalarAsync<decimal>(sql);
                return averageSalary;
            }
        }
        catch (Exception e)
        {
            Console.WriteLine(e.Message);
            throw;
        }
    }

    public async Task<List<Employee>> GetEmployeesByBirthDateRangeAsync(
        DateTime startDate,
        DateTime endDate
    )
    {
        try
        {
            await using (NpgsqlConnection connection = new NpgsqlConnection(_configuration))
            {
                await connection.OpenAsync();
                var sql =
                    @"SELECT * FROM ""Employees"" WHERE ""DateOfBirth"" BETWEEN @StartDate AND @EndDate";
                var employees = await connection.QueryAsync<Employee>(
                    sql,
                    new { StartDate = startDate, EndDate = endDate }
                );
                return employees.ToList();
            }
        }
        catch (Exception e)
        {
            Console.WriteLine(e.Message);
            throw;
        }
    }
}
