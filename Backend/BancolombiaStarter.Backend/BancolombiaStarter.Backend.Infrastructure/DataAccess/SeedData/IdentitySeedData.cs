using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;
using BancolombiaStarter.Backend.Infrastructure.Authorization.Entities;
using System;
using System.Threading.Tasks;

namespace BancolombiaStarter.Backend.Infrastructure.DataAccess.SeedData
{
    public static class IdentitySeedData
    {
        public static async Task Initialize(IServiceProvider serviceProvider)
        {
            var userManager = serviceProvider.GetRequiredService<UserManager<ApplicationUser>>();
            var roleManager = serviceProvider.GetRequiredService<RoleManager<IdentityRole>>();

            await SeedRoles(roleManager);
            await SeedUsers(userManager);
        }

        private static async Task SeedRoles(RoleManager<IdentityRole> roleManager)
        {
            string[] roleNames = { "Admin", "User" };

            IdentityResult roleResult;

            foreach (var roleName in roleNames)
            {
                var roleExist = await roleManager.RoleExistsAsync(roleName);

                if (!roleExist)
                {
                    roleResult = await roleManager.CreateAsync(new IdentityRole(roleName));
                }
            }
        }

        private static async Task SeedUsers(UserManager<ApplicationUser> userManager)
        {
            if (userManager.FindByNameAsync("admin").Result == null)
            {
                var user = new ApplicationUser
                {
                    UserName = "admin",
                    Email = "admin@example.com",
                    FullName= "Amind Profile"
                    // Agrega más propiedades según tus necesidades
                };

                var result = await userManager.CreateAsync(user, "@Admind1234");

                if (result.Succeeded)
                {
                    // Asignar el rol de administrador al usuario
                    await userManager.AddToRoleAsync(user, "Admin");
                }
            }

            if (userManager.FindByNameAsync("user1").Result == null)
            {
                var user = new ApplicationUser
                {
                    UserName = "user1",
                    Email = "user1@example.com",
                    FullName = "user 1 Profile"
                    // Agrega más propiedades según tus necesidades
                };

                var result = await userManager.CreateAsync(user, "@User11234");

                if (result.Succeeded)
                {
                    // Asignar el rol de administrador al usuario
                    await userManager.AddToRoleAsync(user, "User");
                }
            }

            if (userManager.FindByNameAsync("user2").Result == null)
            {
                var user = new ApplicationUser
                {
                    UserName = "user2",
                    Email = "user2@example.com",
                    FullName = "User 2 Profile"
                    // Agrega más propiedades según tus necesidades
                };

                var result = await userManager.CreateAsync(user, "@User21234");

                if (result.Succeeded)
                {
                    // Asignar el rol de administrador al usuario
                    await userManager.AddToRoleAsync(user, "User");
                }
            }
        }
    }
}
