using Microsoft.Extensions.Configuration;
using BancolombiaStarter.Backend.Domain.Entities;
using BancolombiaStarter.Backend.Domain.Ports;
using BancolombiaStarter.Backend.Domain.Services.Generic;
using BancolombiaStarter.Backend.Domain.Services.Interfaces;
using System.Linq.Expressions;

namespace BancolombiaStarter.Backend.Domain.Services
{
    [DomainService]
    public class FinanceService : IFinanceService
    {
        private readonly IGenericRepository<Finance> _repository;

        public FinanceService(IGenericRepository<Finance> repository)
        {
            _repository = repository;
        }

        public async Task<bool> DeleteAsync(long id)
        {
            var entity = (await _repository.GetAsync(filter: x => x.Id == id)).FirstOrDefault();

            if (entity == null)
                return false;

            try
            {
                await _repository.DeleteAsync(entity);
                return true;

            }
            catch (Exception exc)
            {
                return false;
            }
        }

        public async Task<List<Finance>> GetAsync(
            Expression<Func<Finance, bool>>? filter = null,
            Func<IQueryable<Finance>, IOrderedQueryable<Finance>>? orderBy = null,
            bool isTracking = false,
            params Expression<Func<Finance, object>>[] includeObjectProperties)
        {
            var query = await _repository.GetAsync(filter, orderBy, isTracking, includeObjectProperties);
            return query.ToList();
        }

        public async Task<long> InsertAsync(Finance entity)
        {
            var result = await _repository.AddAsync(entity);

            if (result == null)
                throw new Exception("Error al insertar el estado");

            return result.Id;
        }

        public async Task<bool> UpdateAsync(Finance entity)
        {
            try
            {
                await _repository.UpdateAsync(entity);
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }
    }
}
