using Microsoft.Extensions.Configuration;
using BancolombiaStarter.Backend.Domain.Entities;
using BancolombiaStarter.Backend.Domain.Ports;
using BancolombiaStarter.Backend.Domain.Services.Generic;
using BancolombiaStarter.Backend.Domain.Services.Interfaces;
using System.Linq.Expressions;

namespace BancolombiaStarter.Backend.Domain.Services
{
    [DomainService]
    public class CommentsService : ICommentsService
    {
        private readonly IGenericRepository<Comments> _repository;
        private readonly string _storeProcedure;

        public CommentsService(IGenericRepository<Comments> repository, IConfiguration config)
        {
            _repository = repository;
            _storeProcedure = config["StoreProcedures:InserComment"];
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

        public async Task<List<Comments>> GetAsync(
            Expression<Func<Comments, bool>>? filter = null,
            Func<IQueryable<Comments>, IOrderedQueryable<Comments>>? orderBy = null,
            bool isTracking = false,
            params Expression<Func<Comments, object>>[] includeObjectProperties)
        {
            var query = await _repository.GetAsync(filter, orderBy, isTracking, includeObjectProperties);
            return query.ToList();
        }


        public async Task<long> InsertAsync(Comments entity)
        {
            if (string.IsNullOrEmpty(_storeProcedure))
                throw new ArgumentNullException("No se ha asignado el nombre del store procedure para insercción en el app settings");

            var result = await _repository.ExecuteStoreProcedureNonQueryAsync<Comments>(
                _storeProcedure,
                entity,
                new string[] { "Id", "CreationOn" },
                outPutValue: "@Id",
                outPutValueType: System.Data.SqlDbType.BigInt
                );

            if (result == null)
                throw new Exception("Error al insertar el comentario");

            return (long)result;
        }

        public async Task<bool> UpdateAsync(Comments report)
        {
            try
            {
                await _repository.UpdateAsync(report);
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }
    }
}
