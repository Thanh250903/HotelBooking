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
		public Repository(ApplicationDBContext dBContext)
		{
			_dbContext = dBContext;
			DbSet = _dbContext.Set<T>();
		}
		public void Add(T entity)
		{
			DbSet.Add(entity);
		}

		public void Delete(T entity)
		{
			DbSet.Remove(entity);
		}

		public T Get(Expression<Func<T, bool>> filter)
		{
			IQueryable<T> query = DbSet;
			query = query.Where(filter);
			return query.FirstOrDefault();
		}

		public IEnumerable<T> GetAll()
		{
			IQueryable<T> query = DbSet;
			return query.ToList();
		}

		public void Save()
		{
			_dbContext.SaveChanges();
		}

	}
}