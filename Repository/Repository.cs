using HotelApp.Data;
using HotelApp.Repository.IRepository;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace HotelApp.Repository
{
	public class Repository<T> : IRepository<T> where T : class
	{
        private readonly ApplicationDBContext _dbContext;
        internal DbSet<T> DbSet { get; set; }
        public Repository(ApplicationDBContext dbContext)
        {
            _dbContext = dbContext;
            DbSet = _dbContext.Set<T>();
        }
        public void Add(T entity)
        {
            if (entity == null)
            {
                throw new ArgumentException(nameof(entity), " Entity cannot be null");
            }
            _dbContext.Add(entity);
        }

        public void Delete(T entity)
        {
            _dbContext.Remove(entity);
        }

        public T Get(Expression<Func<T, bool>> filter, string? includeProperty = null)
        {
            IQueryable<T> query = DbSet;
            query = query.Where(filter);
            if (!String.IsNullOrEmpty(includeProperty))
            {
                query.Include(includeProperty).ToList();
            }
            return query.FirstOrDefault();
        }

        public IEnumerable<T> GetAll(string? includeProperty = null)
        {
            IQueryable<T> query = DbSet;
            if (!String.IsNullOrEmpty(includeProperty))
            {
                query.Include(includeProperty).ToList();
            }
            return query.ToList();
        }

    }
}