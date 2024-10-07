using Domain.Entities;

namespace Infrastructure.Services
{
    public interface IEmployeeService
    {
        Task<Employee> CreateEmployeeAsync(Employee employee);
        Task<Employee?> GetEmployeeByIdAsync(Guid id);
        Task<List<Employee>> GetAllEmployeesAsync();
        Task<bool> UpdateEmployeeAsync(Employee updatedEmployee);
        Task<bool> DeleteEmployeeAsync(Guid id);
        Task<List<Employee>> GetEmployeesByStatusAsync(bool isActive);
        Task<List<Employee>> GetEmployeesByDepartmentAsync(Guid departmentId);
        Task<decimal> GetAverageSalaryAsync();
        Task<List<Employee>> GetEmployeesByBirthDateRangeAsync(DateTime startDate, DateTime endDate);
    }
}
