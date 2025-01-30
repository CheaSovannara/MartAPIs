using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Metrix_MartAPIs.Data;
using Metrix_MartAPIs.Model;
using Metrix_MartAPIs.Repositories.IRepository;

namespace Metrix_MartAPIs.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class InvoicesController : ControllerBase
    {
        private readonly IInvoiceRepository _invoiceRepository;
        private readonly ILogger<Invoice>_logger;

        public InvoicesController(IInvoiceRepository invoices, ILogger<Invoice> logger)
        {
            _invoiceRepository = invoices;
            _logger = logger;
        }

        // POST: api/Invoices
        [HttpPost]
        public async Task<ActionResult<Invoice>> PostInvoice(Invoice invoice)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            _invoiceRepository.Invoices.Add(invoice);
            await _invoiceRepository.SaveChangesAsync();

            return CreatedAtAction("GetInvoice", new { id = invoice.InvId }, invoice);
        }

        // GET: api/Invoices/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Invoice>> GetInvoice(string id)
        {
            var invoice = await _context.Invoices
                .Include(i => i.Employee)
                .Include(i => i.Product)
                .FirstOrDefaultAsync(i => i.InvId == id);

            if (invoice == null)
            {
                return NotFound();
            }

            return invoice;
        }

        // GET: api/Invoices
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Invoice>>> GetInvoices()
        {
            return await _context.Invoices
                .Include(i => i.Employee)
                .Include(i => i.Product)
                .ToListAsync();
        }

        // PUT: api/Invoices/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutInvoice(string id, Invoice invoice)
        {
            if (id != invoice.InvId)
            {
                return BadRequest();
            }

            _context.Entry(invoice).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!InvoiceExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        // DELETE: api/Invoices/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteInvoice(string id)
        {
            var invoice = await _context.Invoices.FindAsync(id);
            if (invoice == null)
            {
                return NotFound();
            }

            _context.Invoices.Remove(invoice);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool InvoiceExists(string id)
        {
            return _context.Invoices.Any(e => e.InvId == id);
        }
    }
}