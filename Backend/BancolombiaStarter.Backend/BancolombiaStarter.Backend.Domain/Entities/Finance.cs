using BancolombiaStarter.Backend.Domain.Entities.Generic;
using System.ComponentModel.DataAnnotations;

namespace BancolombiaStarter.Backend.Domain.Entities
{
    public class Finance : EntityBase<long>
    {
        public string UserId { get; set; }

        [Required]
        [MaxLength(100)]
        public string Description { get; set; }
        public long Value { get; set; }
        public virtual Projects? Project { get; set; }
        public long ProjectId { get; set; }
    }
}
