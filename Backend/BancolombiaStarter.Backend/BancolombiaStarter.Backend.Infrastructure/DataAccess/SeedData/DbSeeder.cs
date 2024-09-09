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
            var admin = await userManager.FindByNameAsync("admin");
            var user1 = await userManager.FindByNameAsync("user1");
            var user2 = await userManager.FindByNameAsync("User2");

            bool isTableExists = dbContext.TableExists(nameof(Projects));
            if (isTableExists && admin != null && user1 != null && user2 != null)
            {          
                var services = dbContext.Set<Projects>().ToList();
                if(!services.Any())
                {
                    dbContext.Set<Projects>().Add(new Projects()
                    {
                        Name = "Mi propia consultora de creación de programas de computo",
                        Description = "Creare super consultora ",
                        UserId = user1.Id,
                        Goal = 30000,
                        FinancedDate = new DateTime(2024, 02, 5),
                        CreationOn = new DateTime(2024, 01, 15),
                        Status = ProjectStatus.Financed,
                        PictureUrl = "https://d30903flf7mc19.cloudfront.net/wp-content/uploads/2023/10/16133037/5-tipos-de-software-empresarial.webp"
                    });
                    dbContext.Set<Projects>().Add(new Projects()
                    {
                        Name = "Viajar por el mundo",
                        Description = "Quiero viajar por el mundo",
                        UserId = user1.Id,
                        Goal = 1000,
                        PictureUrl = "https://i.pinimg.com/736x/57/7e/19/577e195fa536a326d6a400f3c74b3e3a.jpg"
                    });

                    dbContext.Set<Projects>().Add(new Projects()
                    {
                        Name = "Mi propia empresa de software",
                        Description = "Creare una empresa de software amigable",
                        UserId = user2.Id,
                        Goal = 30000,
                        FinancedDate = new DateTime(2024, 02, 5),
                        CreationOn = new DateTime(2024, 01, 15),
                        Status = ProjectStatus.Financed,
                        PictureUrl = "https://encrypted-tbn0.gstatic.com/images?q=tbn:ANd9GcRV26FCtisbPctGcBE009CUKXhcRD4Z5jSEwA&s"
                    });                  
                    
                    dbContext.Set<Projects>().Add(new Projects()
                    {
                        Name = "software en casa",
                        Description = "creación de programas a la medida",
                        UserId = admin.Id,
                        Goal = 30000,
                        CreationOn = new DateTime(2024, 01, 15),
                        PictureUrl = "https://encrypted-tbn0.gstatic.com/images?q=tbn:ANd9GcRV26FCtisbPctGcBE009CUKXhcRD4Z5jSEwA&s"
                    });

                    dbContext.Set<Projects>().Add(new Projects()
                    {
                        Name = "Financiar mi reposteria",
                        Description = "Quiero montar mi propia reposteria.",
                        UserId = user1.Id,
                        Goal = 5000,
                        FinancedDate = new DateTime(2024, 06, 5),
                        CreationOn = new DateTime(2024, 03, 15),
                        Status = ProjectStatus.Financed,
                        PictureUrl = "https://proingra.com/wp-content/uploads/2021/03/Pieza-Art.-25-Mar-Brahman.jpg"
                    });
                    dbContext.Set<Projects>().Add(new Projects()
                    {
                        Name = "Financiar mi panaderia",
                        Description = "Quiero financiar mi propia panaderia.",
                        UserId = user2.Id,
                        Goal = 10000,
                        FinancedDate = new DateTime(2024,01,12),
                        CreationOn = new DateTime(2024, 01, 5),
                        Status = ProjectStatus.Financed,
                        PictureUrl = "https://impulsapopular.com/wp-content/uploads/2019/05/4400-Pasos-para-abrir-una-panader%C3%ADa-de-%C3%A9xito.jpg"
                    });

                    dbContext.Set<Projects>().Add(new Projects()
                    {
                        Name = "Financiar mi churreria",
                        Description = "Quiero tener mi propia churreria.",
                        UserId = user1.Id,
                        Goal = 6000,
                        CreationOn = new DateTime(2024, 01, 5),
                        Status = ProjectStatus.Financed,
                        PictureUrl = "https://encrypted-tbn0.gstatic.com/images?q=tbn:ANd9GcTY05kYxa6Yqe8EarVD2D_jihXLr7VXILEyuwBIp571Di256l1ncjJw8kgS5kZarvDIuFI&usqp=CAU"
                    });

                    dbContext.Set<Projects>().Add(new Projects()
                    {
                        Name = "Abrir mi cafeteria",
                        Description = "Quiero abrir una cafeteria donde venda tanto pan como dulces y postres",
                        UserId = admin.Id,
                        Goal = 100000,
                        CreationOn = new DateTime(2024, 03, 15),
                        PictureUrl = "https://encrypted-tbn0.gstatic.com/images?q=tbn:ANd9GcRheppPd9YuUGR3Ce1mzMv9PIX1Z5ZaYu4-xg&s"
                    });
                    dbContext.SaveChanges();
                }
            }
        }
    }
}
