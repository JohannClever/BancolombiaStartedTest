using BancolombiaStarter.Backend.Domain.Entities;
using BancolombiaStarter.Backend.Domain.Ports;
using BancolombiaStarter.Backend.Domain.Services.Generic;
using BancolombiaStarter.Backend.Domain.Services.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using System.Linq.Expressions;

namespace BancolombiaStarter.Backend.Domain.Services
{
    [DomainService]
    public class ProjectsService : IProjectService
    {
        private readonly IGenericRepository<Projects> _repository;
        private readonly IFileBlobStorageManager _storageManager;
        private readonly IConfiguration _configuration;

        public ProjectsService(
            IGenericRepository<Projects> repository,
            IFileBlobStorageManager storageManager,
            IConfiguration configuration)
        {
            _repository = repository;
            _storageManager = storageManager;
            _configuration = configuration;
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

        public async Task<List<Projects>> GetAsync(
            Expression<Func<Projects, bool>>? filter = null,
            Func<IQueryable<Projects>, IOrderedQueryable<Projects>>? orderBy = null,
            bool isTracking = false,
            params Expression<Func<Projects, object>>[] includeObjectProperties)
        {
            var query = await _repository.GetAsync(filter, orderBy, isTracking, includeObjectProperties);
            return query.ToList();
        }

        public async Task<long> InsertAsync(Projects entity, IFormFile picture)
        {

            entity.PictureUrl = await UploadPictureAsync(picture);
            var result = await _repository.AddAsync(entity);
            if (result == null)
                throw new Exception("Error al insertar el proyecto.");

            return result.Id;
        }

        public async Task<bool> UpdateAsync(Projects entity)
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

        private async Task<string> UploadPictureAsync(IFormFile picture)
        {
            string result = string.Empty;
            if (picture == null || picture.Length == 0)
            {
                return result;
            }

            try
            {

                result = await _storageManager.SaveFileAsync(_configuration["Storage:ImageContainer"], picture);

                return result;
            }
            catch (Exception exc)
            {
                var message = exc.InnerException == null ? exc.Message : exc.InnerException.Message;
                throw new Exception(message);
            }
        }
    }
}
