using BancolombiaStarter.Backend.Domain.Entities;
using BancolombiaStarter.Backend.Infrastructure.Authorization.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;

namespace BancolombiaStarter.Backend.Infrastructure.DataAccess.SeedData
{
    public static class DbSeeder
    {
        public async static Task SeedData(PersistenceContext dbContext , IServiceProvider serviceProvider)
        {
            var userManager = serviceProvider.GetRequiredService<UserManager<ApplicationUser>>();
            var User1 = await userManager.FindByNameAsync("user1");
            var User2 = await userManager.FindByNameAsync("User2");

            bool isTableExists = dbContext.TableExists(nameof(Projects));
            if (isTableExists && User1 != null && User2 != null)
            {          
                var services = dbContext.Set<Projects>().ToList();
                if(!services.Any())
                {
                    dbContext.Set<Projects>().Add(new Projects()
                    {
                        Name = "Viajar por el mundo",
                        Description = "Quiero viajar por el mundo",
                        UserId = User1.Id,
                        Goal = 1000
                    });
                    dbContext.Set<Projects>().Add(new Projects()
                    {
                        Name = "Mi propia empresa de software",
                        Description = "Creare una empresa de software amigable",
                        UserId = User1.Id,
                        Goal = 3000
                    });
                    dbContext.Set<Projects>().Add(new Projects()
                    {
                        Name = "Financiar mi reposteria",
                        Description = "Quiero montar mi propia reposteria.",
                        UserId = User1.Id,
                        Goal = 5000
                    });
                    dbContext.SaveChanges();
                }
            }
        }
    }
}
