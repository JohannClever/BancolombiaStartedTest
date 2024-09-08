using BancolombiaStarter.Backend.Domain.Entities.Generic;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BancolombiaStarter.Backend.Domain.Entities
{
    public class Projects : EntityBase<long>
    {
        [Required]
        [MaxLength(100)]
        public string Name { get; set; }
        [Required]
        public string Description { get; set; }
        [Required]

        public decimal Goal { get; set; }
        public decimal Pledged { get; set; }
        public int BackersCount { get; set; }

        public string PictureUrl { get; set; }
        [Required]
        public string UserId { get; set; }

        public List<Comments> Comments { get; set; }
        public List<Finance> Finances { get; set; }
    }
}
