
using Metrix_MartAPIs.DbContexts;

namespace Metrix_MartAPIs.Repositories.GenericRepository
{
    public class GenericRepository<TEntity> : IGenericRepository<TEntity> where TEntity : class
    {
        private readonly MetrixMartDbContext _context;
        public GenericRepository(MetrixMartDbContext context)
        {
            _context = context?? throw new ArgumentNullException(nameof(context));
        }

        protected virtual IQueryable<TEntity> GetSet()
        {
            return _context.Set<TEntity>();
        }
        public List<TEntity> GetAll()
        {
            return GetSet().ToList();
        }
        public virtual async Task AddAsync(TEntity tentiy)
        {
            await _context.Set<TEntity>().AddAsync(tentiy);
        }

        public TEntity GetById(string id)
        {
            return _context.Set<TEntity>().Find(id);
        }

        public async Task<bool> DeleteById(string id)
        {
            var tEntity = GetById(id);
            _context.Set<TEntity>().Remove(tEntity);
            _context.SaveChanges();
            return true;
        }

        public async Task<bool> Update(TEntity tentiy)
        {
            _context.Set<TEntity>().Update(tentiy);
            _context.SaveChanges();
            return true;
        }
    }
}
