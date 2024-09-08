using BancolombiaStarter.Backend.Domain.Entities;
using Microsoft.AspNetCore.Http;
using System.Linq.Expressions;

namespace BancolombiaStarter.Backend.Domain.Services.Interfaces
{
    public interface IProjectService
    {
        Task<long> InsertAsync(Projects entity, IFormFile? picture);
        Task<List<Projects>> GetAsync(
            Expression<Func<Projects, bool>>? filter = null,
            Func<IQueryable<Projects>, IOrderedQueryable<Projects>>? orderBy = null,
            bool isTracking = false,
            params Expression<Func<Projects, object>>[] includeObjectProperties);
        Task<bool> UpdateAsync(Projects entity);
        Task<bool> DeleteAsync(long id);
    }
}
