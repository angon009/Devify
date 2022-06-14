using System.Linq.Expressions;

namespace ECommerce.Data
{
    public interface IRepository<TEntity, TKey>
        where TEntity : class, IEntity<TKey>
    {
        #region Asynchronous Methods
        Task AddAsync(TEntity entity);
        Task RemoveAsync(TKey id);
        Task RemoveAsync(TEntity entityToDelete);
        Task RemoveAsync(Expression<Func<TEntity, bool>> filter);
        Task EditAsync(TEntity entityToUpdate);
        Task AddOrModify(TEntity entity, Expression<Func<TEntity, bool>> filter);
        Task<int> GetCountAsync(Expression<Func<TEntity, bool>> filter = null);
        Task<IList<TEntity>> GetAsync(Expression<Func<TEntity, bool>> filter, string includeProperties = "");
        Task<IList<TEntity>> GetAllAsync();
        Task<TEntity> GetByIdAsync(TKey id);
        Task<(IList<TEntity> data, int total, int totalDisplay)> GetAsync(
            Expression<Func<TEntity, bool>> filter = null,
            Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>> orderBy = null,
            string includeProperties = "", int pageIndex = 1, int pageSize = 10, bool isTrackingOff = false);

        Task<(IList<TEntity> data, int total, int totalDisplay)> GetDynamicAsync(
            Expression<Func<TEntity, bool>> filter = null,
            string orderBy = null,
            string includeProperties = "", int pageIndex = 1, int pageSize = 10, bool isTrackingOff = false);

        Task<IList<TEntity>> GetAsync(Expression<Func<TEntity, bool>> filter = null,
            Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>> orderBy = null,
            string includeProperties = "", bool isTrackingOff = false);

        Task<IList<TEntity>> GetDynamicAsync(Expression<Func<TEntity, bool>> filter = null,
            string orderBy = null,
            string includeProperties = "", bool isTrackingOff = false);

        #endregion

        #region Non-Asynchronous Methods
        void Add(TEntity entity);
        void Remove(TKey id);
        void Remove(TEntity entityToDelete);
        void Remove(Expression<Func<TEntity, bool>> filter);
        void Edit(TEntity entityToUpdate);
        int GetCount(Expression<Func<TEntity, bool>> filter = null);
        IList<TEntity> Get(Expression<Func<TEntity, bool>> filter, string includeProperties = "");
        IList<TEntity> GetAll();
        TEntity GetById(TKey id);
        (IList<TEntity> data, int total, int totalDisplay) Get(
            Expression<Func<TEntity, bool>> filter = null,
            Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>> orderBy = null,
            string includeProperties = "", int pageIndex = 1, int pageSize = 10, bool isTrackingOff = false);

        (IList<TEntity> data, int total, int totalDisplay) GetDynamic(
            Expression<Func<TEntity, bool>> filter = null,
            string orderBy = null,
            string includeProperties = "", int pageIndex = 1, int pageSize = 10, bool isTrackingOff = false);

        IList<TEntity> Get(Expression<Func<TEntity, bool>> filter = null,
            Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>> orderBy = null,
            string includeProperties = "", bool isTrackingOff = false);

        IList<TEntity> GetDynamic(Expression<Func<TEntity, bool>> filter = null,
            string orderBy = null,
            string includeProperties = "", bool isTrackingOff = false);

        #endregion

        #region Custom Method
        Task<TEntity> GetByIncludeAsync(Expression<Func<TEntity, bool>> filter, string includeProperties = "");
        #endregion
    }
}
