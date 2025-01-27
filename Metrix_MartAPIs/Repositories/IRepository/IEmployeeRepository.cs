using Metrix_MartAPIs.Model;
using Metrix_MartAPIs.Repositories.GenericRepository;

namespace Metrix_MartAPIs.Repositories.IRepository
{
    public interface IEmployeeRepository : IGenericRepository<Employee>
    {
        public Employee GetEmployeeByName(string name);
        Task<bool> DeleteById (string id);
        public Task<Employee> UpdateEmployee (Employee employee, string id);
        public Task<Employee> GetById(string id);
    }
}
