using Metrix_MartAPIs.Model;
using Metrix_MartAPIs.Repositories.IRepository;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace Metrix_MartAPIs.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EmployeeController : ControllerBase
    {
        private readonly IEmployeeRepository _employeeRepository;
        private readonly ILogger<Employee> _logger;
        public EmployeeController(IEmployeeRepository employeeRepository, ILogger<Employee> logger)
        {
            _employeeRepository = employeeRepository;
            _logger = logger;
        }

        
        [HttpGet("/getAllEmp")]
        public IActionResult GetAllEmployees()
        {
            try
            {
                _logger.LogInformation("Start Service >>> Get All Employees at {DT}", DateTime.Now.ToLongTimeString());
                var employees = _employeeRepository.GetAll();
                if (!employees.Any())
                {
                    _logger.LogError("Failed >>> Get All Employees! at {DT}", DateTime.Now.ToLongTimeString());
                    return NotFound("Employees Not Found!");
                }
                _logger.LogInformation("End of Service >>> Get All Employees! at {DT}", DateTime.Now.ToLongTimeString());
                return Ok(employees);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while trying to retrive the all Employees!", DateTime.Now.ToLongTimeString());
                return StatusCode(500, "Internale Server Error");
            }
        }

        [HttpGet("/getEmployeeby{name}")]
        public IActionResult GetEmployeeByName(string name)
        {
            try
            {
                _logger.LogInformation("Start Service at {DT}", DateTime.Now.ToLongTimeString());
                
                var empName = _employeeRepository.GetEmployeeByName(name);
                if(empName != null)
                {
                    _logger.LogInformation($"Employee's Name is [{name}]", JsonConvert.SerializeObject(name));
                }
                else
                {
                    _logger.LogInformation("Employee Not Found! {DT}", DateTime.Now.ToLongTimeString());
                    return NotFound($"Employee with name '{empName}' not found!");
                }
                return Ok(empName);
            }
            catch (Exception ex)
            {
                _logger.LogInformation(ex, "An Occurred Error while trying to get employee's name! at {DT}", DateTime.Now.ToLongTimeString());
                return StatusCode(500, "Internal Server Error!");
            }
        }

        [HttpPost("/addEmployee")]
        public async Task<IActionResult> AddAsync(Employee employee)
        {
            try
            {
                _logger.LogInformation("Start Service >>>> Add Employee {DT}",DateTime.Now.ToLongTimeString());
                //var emp = await _employeeRepository.AddAsync(employee);
                await _employeeRepository.AddAsync(employee);
                if (employee == null)
                {
                    _logger.LogInformation("Employee is null values can't add. {DT}", DateTime.Now.ToLongTimeString());
                    return BadRequest("Add Employee Failed!");
                }
                _logger.LogInformation($"Add Employee Successful : {employee}", JsonConvert.SerializeObject(employee));
                return Ok(employee);
            }
            catch(Exception ex)
            {
                _logger.LogInformation(ex, "An Error Occurred while Adding the Employee");
                return StatusCode(500, "Internal Server Error!");
            }
        }

        [HttpPost("/updateEmployee{id}")]
        public async Task<IActionResult> UpdateEmployee(Employee employee, string id)
        {
            _logger.LogInformation("Start Service >>> Update Employee! {DT}", DateTime.Now.ToLongTimeString());
            try
            {
                var emp = await _employeeRepository.UpdateEmployee(employee, id);
                return Ok(emp);
            }
            catch
            {
                return StatusCode(500, "Internal Server Error!");
            }
        }

        [HttpDelete("/deleteEmployee{id}")]
        public async Task<IActionResult> DeleteEmployeeById(string id)
        {
            try
            {
                _logger.LogInformation("Start Service >>> Delete Employee Controller : {DT}", DateTime.Now.ToLongTimeString());
                //var employee = await _employeeRepository.GetById(id);
                var isDeleted = await _employeeRepository.DeleteById(id);
                if (isDeleted)
                {
                    _logger.LogInformation($"Deleted Employee : {isDeleted} Successful");
                }
                else
                {
                    _logger.LogError("Delete Employee Failed! >>> Emp_Id Not Found! {DT}", DateTime.Now.ToLongTimeString());
                    return NotFound("Employee Not Found!");
                }
                _logger.LogInformation("End of Delete Service >>> Delete Employee Success! {DT}", DateTime.Now.ToLongTimeString());
                return Ok("Employee is Deleted!");
            }
            catch(Exception ex)
            {
                _logger.LogError(ex,"An Error Occurred while trying to Delete Employee!");
                return StatusCode(500, "Internal Server Error");
            }
        }
    }
}
