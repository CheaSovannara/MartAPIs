using Metrix_MartAPIs.DbContexts;
using Metrix_MartAPIs.Model;
using Metrix_MartAPIs.Repositories.GenericRepository;
using Metrix_MartAPIs.Repositories.IRepository;

namespace Metrix_MartAPIs.Repositories.Repository
{
    public class InvoiceRepository : GenericRepository<Invoice>, IInvoiceRepository
    {
        private readonly MetrixMartDbContext _contex;
        public InvoiceRepository(MetrixMartDbContext context) : base(context)
        {
            _contex = context;
        }

        public void Add(Invoice tentiy)
        {
            throw new NotImplementedException();
        }

        public void Delete(Invoice tentiy)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<Invoice> GetAll()
        {
            throw new NotImplementedException();
        }

        public Invoice GetById(int id)
        {
            throw new NotImplementedException();
        }

        public void Update(Invoice tentiy)
        {
            throw new NotImplementedException();
        }
    }
}
