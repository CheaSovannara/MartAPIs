using Metrix_MartAPIs.DbContexts;
using Metrix_MartAPIs.Model;
using Metrix_MartAPIs.Repositories.GenericRepository;
using Metrix_MartAPIs.Repositories.IRepository;
using Microsoft.EntityFrameworkCore;

namespace Metrix_MartAPIs.Repositories.Repository
{
    public class InvoiceRepository : GenericRepository<Invoice>, IInvoiceRepository
    {
        private readonly MetrixMartDbContext _contex;
        private readonly ILogger<Invoice> _logger;
        public InvoiceRepository(MetrixMartDbContext context, ILogger<Invoice> logger) : base(context)
        {
            _contex = context;
            _logger = logger;
        }

        Task<bool> IInvoiceRepository.DeleteById(string id)
        {
            throw new NotImplementedException();
        }

        Task<IQueryable<Invoice>> IInvoiceRepository.GetAllInvoices()
        {
            throw new NotImplementedException();
        }

        Task<Invoice> IInvoiceRepository.GetInvoiceByIdAsync(string id)
        {
            throw new NotImplementedException();
        }

        //public override async Task<Invoice> AddAsync (Invoice invoice)
        //{

        //    //throw new NotImplementedException();
        //    _logger.LogInformation("Start Service >>> Post Invoice : {DT}", DateTime.Now.ToLongTimeString());
        //    try
        //    {
        //        if(invoice.Quantity <= 0)
        //        {
        //            throw new Exception("Quantity must be a Positive number.");
        //        }
        //        else if(invoice.ProductId == null)
        //        {
        //            throw new Exception("Product Id is Invalid!");
        //        }
        //        else if(invoice.TotalPrice <= 0)
        //        {
        //            throw new Exception("Total Amount is Invalid!");
        //        }
        //        await _contex.Invoices.AddAsync(invoice);
        //        await _contex.SaveChangesAsync();
        //        return invoice;
        //    }
        //    catch(Exception ex)
        //    {
        //        _logger.LogError(ex, "An Error Occurred! {DT}", DateTime.Now.ToLongTimeString());
        //        throw;
        //    }
        //}
        //public async Task<int> GenerateNextInvIdAsync()
        //{
        //    // Get the maximum existing InvId from the database
        //    var maxInvIdString = await _contex.Invoices.MaxAsync(i => i.InvId);
        //    int maxInvId = 0;
        //    if (!string.IsNullOrEmpty(maxInvIdString) && int.TryParse(maxInvIdString, out int parsedId))
        //    {
        //        maxInvId = parsedId;
        //    }

        //    // Increment by 1 to generate the next InvId
        //    return maxInvId + 1;
        //}

        private async Task<string> GenerateNextInvIdAsync()
        {
            var lastInvId = await _contex.Invoices.OrderByDescending(i => i.InvId).Select(i => i.InvId).FirstOrDefaultAsync();

            if (string.IsNullOrEmpty(lastInvId))
            {
                return "1"; // Or your initial InvId value
            }

            if (int.TryParse(lastInvId, out int lastIdNum))
            {
                return (lastIdNum + 1).ToString();
            }
            else
            {
                // Handle cases where lastInvId is not a number
                // Log an error, throw an exception, or use a different ID generation strategy
                _logger.LogError("Last InvId is not a valid number. Cannot generate next InvId.");
                throw new InvalidOperationException("Cannot generate next InvId.");
            }

        }

        public override async Task<Invoice> AddAsync(Invoice invoice)
        {
            _logger.LogInformation("Start Service >>> Post Invoice : {DateTime}", DateTime.Now.ToLongTimeString());

            if (invoice == null)
            {
                _logger.LogError("Invoice can't be null. {DT}", DateTime.Now.ToLongTimeString());
                throw new ArgumentNullException(nameof(invoice), "Invoice cannot be null.");
            }

            try
            {
                // Generate the next InvId
                invoice.InvId = await GenerateNextInvIdAsync();

                ValidateInvoice(invoice);

                await _contex.Invoices.AddAsync(invoice);
                await _contex.SaveChangesAsync();
                return invoice;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An Error Occurred! {DateTime}", DateTime.Now.ToLongTimeString());
                throw;
            }
        }

        private void ValidateInvoice(Invoice invoice)
        {
            if (invoice.Quantity <= 0)
            {
                throw new ArgumentException("Quantity must be a positive number.", nameof(invoice.Quantity));
            }

            if (invoice.ProductId == null)
            {
                throw new ArgumentException("Product Id is invalid.", nameof(invoice.ProductId));
            }

            if (invoice.TotalPrice <= 0)
            {
                throw new ArgumentException("Total Amount is invalid.", nameof(invoice.TotalPrice));
            }
        }

        Task<Invoice> IInvoiceRepository.UpdateInvoice(Invoice invoice)
        {
            throw new NotImplementedException();
        }
    }
}
