using Metrix_MartAPIs.Model;
using Metrix_MartAPIs.Repositories.GenericRepository;

namespace Metrix_MartAPIs.Repositories.IRepository
{
    public interface IInvoiceRepository : IGenericRepository<Invoice>
    {
        public Task<Invoice> GetInvoiceByIdAsync(string id);
        Task<IQueryable<Invoice>> GetAllInvoices();
        public Task<Invoice> UpdateInvoice (Invoice invoice);
        public Task<bool> DeleteById(string id);
    }
}
