using BancolombiaStarter.Backend.Domain.Entities;
using System.Linq.Expressions;

namespace BancolombiaStarter.Backend.Domain.Services.Interfaces
{
    public interface IFinanceService
    {
        Task<long> InsertAsync(Finance entity);
        Task<List<Finance>> GetAsync(
            Expression<Func<Finance, bool>>? filter = null,
            Func<IQueryable<Finance>, IOrderedQueryable<Finance>>? orderBy = null,
            bool isTracking = false,
            params Expression<Func<Finance, object>>[] includeObjectProperties);
        Task<bool> UpdateAsync(Finance entity);
        Task<bool> DeleteAsync(long id);
    }
}
