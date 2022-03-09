using challenge.Data;
using challenge.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System.Linq;

namespace challenge.Repositories
{
    public class ReportingStructureRespository : IReportingStructureRepository
    {
        private readonly EmployeeContext _employeeContext;
        private readonly ILogger<IEmployeeRepository> _logger;

        public ReportingStructureRespository(ILogger<IEmployeeRepository> logger, EmployeeContext employeeContext)
        {
            _employeeContext = employeeContext;
            _logger = logger;
        }

        public ReportingStructure GetById(string id)
        {
            var employees = _employeeContext.Employees.Include(e => e.DirectReports).ToList();
            var employee = employees.SingleOrDefault(e => e.EmployeeId == id);

            return new ReportingStructure() { Employee = employee, NumberOfReports = NumberOfReports(employee).ToString() };
        }

        private int NumberOfReports(Employee employee)
        {
            int count = 0;
            employee.DirectReports?.ForEach(directReport =>
            {
                count++;
                if ((directReport.DirectReports?.Count ?? 0) > 0)
                {
                    count += NumberOfReports(directReport);
                }
            });
            return count;
        }
    }
}