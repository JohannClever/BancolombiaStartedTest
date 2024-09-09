using BancolombiaStarter.Backend.Domain.Entities.Generic;
using System.ComponentModel.DataAnnotations;

namespace BancolombiaStarter.Backend.Domain.Entities
{
    public enum ProjectStatus
    {
        Created= 0,
        OnProccess = 1,
        Financed = 2
    }
    public class Projects : EntityBase<long>
    {
        public Projects()
        {
            Status = ProjectStatus.Created;
        }

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

        public ProjectStatus Status { get; set; }
        public DateTime? FinancedDate { get; set; }

        public List<Comments> Comments { get; set; }
        public List<Finance> Finances { get; set; }
    }
}
