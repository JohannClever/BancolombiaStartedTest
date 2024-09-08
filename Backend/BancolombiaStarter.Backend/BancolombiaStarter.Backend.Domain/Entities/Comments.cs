using BancolombiaStarter.Backend.Domain.Entities.Generic;
using System.ComponentModel.DataAnnotations;

namespace BancolombiaStarter.Backend.Domain.Entities
{
    public class Comments : EntityBase<long>
    {
        public virtual Projects? Project { get; set; }
        public long ProjectId { get; set; }
        [MaxLength(250)]
        public string Observations { get; set; }

        public string IdUser { get; set; }
        public DateTime CreationOn { get; set; }
    }
}
