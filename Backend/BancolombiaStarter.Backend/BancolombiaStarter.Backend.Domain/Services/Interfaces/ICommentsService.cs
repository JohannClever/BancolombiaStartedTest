using BancolombiaStarter.Backend.Domain.Entities;
using System.Linq.Expressions;

namespace BancolombiaStarter.Backend.Domain.Services.Interfaces
{
    public interface ICommentsService
    {
        Task<long> InsertAsync(Comments entity);
        Task<List<Comments>> GetAsync(
            Expression<Func<Comments, bool>>? filter = null,
            Func<IQueryable<Comments>, IOrderedQueryable<Comments>>? orderBy = null,
            bool isTracking = false,
            params Expression<Func<Comments, object>>[] includeObjectProperties);
        Task<bool> UpdateAsync(Comments entity);
        Task<bool> DeleteAsync(long reportId);
    }
}
