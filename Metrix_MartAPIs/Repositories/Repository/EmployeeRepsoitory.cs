using Metrix_MartAPIs.DbContexts;
using Metrix_MartAPIs.Model;
using Metrix_MartAPIs.Repositories.GenericRepository;
using Metrix_MartAPIs.Repositories.IRepository;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Newtonsoft.Json;

namespace Metrix_MartAPIs.Repositories.Repository
{
    public class EmployeeRepsoitory : GenericRepository<Employee>, IEmployeeRepository
    {
        private readonly MetrixMartDbContext _context;
        private readonly ILogger<Employee> _logger;
        public EmployeeRepsoitory(ILogger<Employee> logger, MetrixMartDbContext context) : base(context)
        {
            _context = context;
            _logger = logger;
        }
        public Employee GetEmployeeByName(string name)
        {
            _logger.LogInformation("Start Service at: {DT}", DateTime.Now.ToLongTimeString());
            try
            {
                _logger.LogDebug($"Search Employee by Name : {name}");
                var queryName = _context.Employees.AsQueryable();

                if (name == null)
                {
                    name = name.ToLower();
                    queryName = queryName.Where(e => e.FirstName.ToLower() == name);
                }
                else
                {
                    queryName = queryName.Where(e => e.LastName.ToLower() == name);
                }
                _logger.LogInformation("End of Service! at {DT}", DateTime.Now.ToLongTimeString());
                return queryName.FirstOrDefault();
            }
            catch (Exception ex)
            {
                throw new Exception();
            }
        }
        protected override IQueryable<Employee> GetSet()
        {
            return base.GetSet();
        }
        public override async Task<Employee> AddAsync(Employee employee)
        {
            _logger.LogInformation("Start AddAsync Service >>> at {DT}", DateTime.Now.ToLongTimeString());
            try
            {
                // Get the latest employee ID from the database
                string latestId = await _context.Employees.OrderByDescending(e => e.Emp_Id)
                                                          .Select(e => e.Emp_Id)
                                                          .FirstOrDefaultAsync();
                // Extract the numeric part of the ID
                int latestIdNumber = 0;
                if (!string.IsNullOrEmpty(latestId) && int.TryParse(latestId.Replace("EMP", ""), out latestIdNumber))
                {
                    latestIdNumber++;
                }
                //Generate new Id
                string newId = $"EMP{latestIdNumber:000}";
                employee.Emp_Id = newId;

                await base.AddAsync(employee);
                await _context.SaveChangesAsync();
                var emp = JsonConvert.SerializeObject(employee);
                _logger.LogInformation($"Employee added successfully {emp}");
                return employee;
            }
            catch (DbUpdateException ex)
            {
                _logger.LogInformation("An Error Occurred while trying to Add Employee! {DT}", DateTime.Now.ToLongTimeString());
                throw new InvalidOperationException("Faild to Add Employee!", ex);
            }
        }

        public async Task<bool> DeleteById(string id)
        {
            try
            {
                _logger.LogInformation("Start Service >>> Delete Employee {DT}", DateTime.Now.ToLongTimeString());
                var emp = await _context.Employees.FirstOrDefaultAsync(em => em.Emp_Id == id);
                
                if(emp == null)
                {
                    _logger.LogInformation("Employee was Deleted! {DT}", DateTime.Now.ToLongTimeString());
                }
                _logger.LogInformation($"Delete Employee: {emp}");
                _context.Employees.Remove(emp);
                await _context.SaveChangesAsync();
                _logger.LogInformation("End of Service >>> {DT}", DateTime.Now.ToLongTimeString());
                return true;
            }
            catch(Exception ex)
            {
                return false;
            }
        }

        public List<Employee> GetAll()
        {
            try
            {
                _logger.LogInformation("Start Repository Service >>> Get All Employees at {DT}", DateTime.Now.ToShortTimeString());
                var emp = _context.Employees.ToList();
                if (!emp.Any())
                {
                    _logger.LogInformation("Can't Get All Employees! >>> at {DT}", DateTime.Now.ToLongTimeString());
                    return new List<Employee>();
                }
                var employee = JsonConvert.SerializeObject(emp);
                _logger.LogInformation($"Repository >>>> Get all Employees {employee}");
                return emp;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while retrieving employees! {DT}", DateTime.Now.ToLongTimeString());

                // **Handle the exception and return a message:**
                return new List<Employee>();
            }
        }

        public Task<Employee> GetById(string id)
        {
            throw new NotImplementedException();
        }

        public async Task<Employee> UpdateEmployee(Employee employee, string id)
        {
            _logger.LogInformation("Start Service >>> Update Employee - {DT}", DateTime.Now.ToLongTimeString());
            try
            {
                var emp = JsonConvert.SerializeObject(employee);
                _logger.LogInformation($"Update Employee Success: {emp}");
                _context.Employees.Update(employee);
                _context.SaveChanges();
                _logger.LogInformation("End of Update! {DT}", DateTime.Now.ToLongTimeString());
                return employee;
            }
            catch (DbUpdateException ex)
            {
                _logger.LogError("Db Update Exception Catch {DT}", DateTime.Now.ToLongTimeString());
                return null;
            }
            catch (Exception ex)
            {
                _logger.LogError("An error occurred while updating employee: {Message}", ex.Message);
                throw;
            }
        }

    }
}
