using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;
using BancolombiaStarter.Backend.Infrastructure.Authorization.Entities;
using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.SignalR;
using static System.Net.Mime.MediaTypeNames;
using System.Buffers.Text;

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
                    FullName= "Amind Profile",
                    PictureUrl = "https://img.freepik.com/foto-gratis/chico-guapo-seguro-posando-contra-pared-blanca_176420-32936.jpg"
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
                    FullName = "user 1 Profile",
                    PictureUrl = "https://plus.unsplash.com/premium_photo-1664536392896-cd1743f9c02c?fm=jpg&q=60&w=3000&ixlib=rb-4.0.3&ixid=M3wxMjA3fDB8MHxzZWFyY2h8MXx8cGVyc29uYXxlbnwwfHwwfHx8MA%3D%3D"
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
                    FullName = "User 2 Profile",
                    PictureUrl = "https://imagenes.eltiempo.com/files/image_1200_600/files/fp/uploads/2024/03/19/65f9d492598ea.r_d.1079-279-5658.jpeg"
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
