using BancolombiaStarter.Backend.Domain.Entities;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BancolombiaStarter.Backend.Infrastructure.Authorization.Entities
{
    public class ApplicationUser : IdentityUser
    {
        public string FullName { get; set; }
        public string PictureUrl { get; set; }
    }
}
