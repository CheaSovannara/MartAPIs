namespace Metrix_MartAPIs.Repositories.GenericRepository
{
    public interface IGenericRepository<TEntiy> where TEntiy : class
    {
        public TEntiy GetById(string id);
        public List<TEntiy> GetAll();
        public Task AddAsync(TEntiy tentiy);
        public Task<bool> Update(TEntiy tentiy);
        public Task<bool> DeleteById(string id);
    }
}
