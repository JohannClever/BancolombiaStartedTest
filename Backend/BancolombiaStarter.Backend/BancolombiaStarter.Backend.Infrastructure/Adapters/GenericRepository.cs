
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using BancolombiaStarter.Backend.Domain.Commons.Exception;
using BancolombiaStarter.Backend.Domain.Entities.Generic;
using BancolombiaStarter.Backend.Domain.Ports;
using BancolombiaStarter.Backend.Infrastructure.DataAccess;
using BancolombiaStarter.Backend.Infrastructure.Extensions;
using System.Data;
using System.Data.Common;
using System.Linq.Expressions;


namespace BancolombiaStarter.Backend.Infrastructure.Adapters
{
    public class GenericRepository<E> : IGenericRepository<E> where E : DomainEntity
    {
        readonly PersistenceContext _context;
        public GenericRepository(PersistenceContext context)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
        }

        public async Task<E> AddAsync(E entity)
        {
            _ = entity ?? throw new ArgumentNullException(nameof(entity), "Entity can not be null");
            _context.Set<E>().Add(entity);
            await this.CommitAsync();
            return entity;
        }

        public async Task<IEnumerable<E>> AddAsync(IEnumerable<E> entities)
        {
            if (entities is null || !entities.Any()) throw new ArgumentNullException(nameof(entities), "Entities can not be null");
            _context.Set<E>().AddRange(entities);
            await this.CommitAsync();
            return entities;
        }

        public async Task DeleteAsync(E entity)
        {
            if (entity != null)
            {
                _context.Set<E>().Remove(entity);
                await this.CommitAsync().ConfigureAwait(false);
            }
        }

        public async Task DeleteAsync(IEnumerable<E> entities)
        {
            if (entities != null && entities.Any())
            {
                _context.Set<E>().RemoveRange(entities);
                await this.CommitAsync().ConfigureAwait(false);
            }
        }

        public async Task<IEnumerable<E>> GetAsync(Expression<Func<E, bool>>? filter = null, Func<IQueryable<E>, IOrderedQueryable<E>>? orderBy = null, bool isTracking = false, params Expression<Func<E, object>>[] includeObjectProperties)
        {
            IQueryable<E> query = _context.Set<E>();

            if (filter != null)
            {
                query = query.Where(filter);
            }

            if (includeObjectProperties != null)
            {
                foreach (Expression<Func<E, object>> include in includeObjectProperties)
                {
                    query = query.Include(include);
                }
            }

            if (orderBy != null)
            {
                return await orderBy(query).ToListAsync();
            }

            return (!isTracking) ? await query.AsNoTracking().ToListAsync() : await query.ToListAsync();
        }

        public async Task<E> GetByIdAsync(object id)
        {
            return await _context.Set<E>().FindAsync(id);
        }

        public async Task UpdateAsync(E entity)
        {
            if (entity != null)
            {
                _context.Set<E>().Update(entity);
                await this.CommitAsync();
            }
        }

        public async Task<object> ExecuteStoreProcedureNonQueryAsync<T>(
            string storeProcedure, 
            T entity ,
            string[] excludeProperties, 
            string outPutValue = "" ,
            SqlDbType outPutValueType = SqlDbType.Int)
        {
            object result = null;
            try
            {
                var parameters = MapEntityToParameters(entity, excludeProperties);

                SqlParameter outputParameter = null;
                if (!string.IsNullOrEmpty(outPutValue))
                {
                    outputParameter = new SqlParameter(outPutValue, outPutValueType)
                    {
                        Direction = ParameterDirection.Output
                    };
                }

                using var command = CreateCommand(storeProcedure, parameters, outputParameter);
                var resultCommand = await command.ExecuteScalarAsync();

                if (!string.IsNullOrEmpty(outPutValue))
                {
                    result = outputParameter.Value;
                }
                
                return result;
            }
            catch (Exception ex)
            {
                throw new DataAccessException(ex.Message, ex);
            }
        }

        public async Task CommitAsync()
        {
            _context.ChangeTracker.DetectChanges();
            await _context.CommitAsync().ConfigureAwait(false);
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            this._context.Dispose();
        }

        private StoredProcedureParameters MapEntityToParameters<T>(T entity, string[] excludeProperties)
        {
            var parameters = new StoredProcedureParameters();
            var properties = typeof(T).GetProperties();
            foreach (var property in properties)
            {
                var propertyName = property.Name;
                var propertyValue = property.GetValue(entity);
                if (propertyValue != null && !excludeProperties.Contains(property.Name))    
                    parameters.AddParameter(propertyName, propertyValue); 
            }

            return parameters;
        }

        private DbCommand CreateCommand(
            string storeProcedure,
            StoredProcedureParameters parameters,
            SqlParameter outputParameter = null)
        {
            try
            {
                var command = _context.Database.GetDbConnection().CreateCommand();
                command.CommandType = CommandType.StoredProcedure;
                command.CommandText = storeProcedure;

                if (parameters != null)
                {
                    foreach (var param in parameters.Parameters)
                    {
                        command.Parameters.Add(new SqlParameter(param.Key, param.Value));
                    }
                }
                if (outputParameter != null)
                    command.Parameters.Add(outputParameter);


                OpenConnection();

                return command;
            }
            catch (Exception ex)
            {
                throw new DataAccessException(ex.Message, ex);
            }
        }

        private void OpenConnection()
        {
            if (_context.Database.GetDbConnection().State == ConnectionState.Closed)
            {
                _context.Database.GetDbConnection().Open();
            }
        }
    }
}
