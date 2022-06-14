using Microsoft.EntityFrameworkCore;

namespace ECommerce.Data
{
    public abstract class UnitOfWork : IUnitOfWork
    {
        protected readonly DbContext _dbContext;

        public UnitOfWork(DbContext dbContext) => _dbContext = dbContext;

        #region Asynchronous
        public async virtual ValueTask DisposeAsync() => _dbContext?.DisposeAsync();
        public async virtual Task SaveAsync() => await _dbContext?.SaveChangesAsync();

        #endregion

        #region Non-Asynchronous
        public virtual void Dispose() => _dbContext?.Dispose();
        public virtual void Save() => _dbContext?.SaveChanges();
        #endregion
    }
}
