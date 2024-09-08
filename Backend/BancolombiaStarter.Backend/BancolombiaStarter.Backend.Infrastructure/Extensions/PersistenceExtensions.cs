using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using BancolombiaStarter.Backend.Domain.Ports;
using BancolombiaStarter.Backend.Infrastructure.Adapters;
using System.Data;

namespace BancolombiaStarter.Backend.Infrastructure.Extensions
{
    public static class PersistenceExtensions
    {
        public static IServiceCollection AddPersistence(this IServiceCollection svc, IConfiguration config)
        {
            svc.AddTransient(typeof(IGenericRepository<>), typeof(GenericRepository<>));
            svc.AddTransient<IDbConnection>((sp) => new SqlConnection(config.GetConnectionString("database")));
            svc.AddTransient<IFileBlobStorageManager, FileBlobStorageManager>();
            return svc;
        }
    }
}
