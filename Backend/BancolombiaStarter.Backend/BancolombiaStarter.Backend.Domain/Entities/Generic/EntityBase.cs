namespace BancolombiaStarter.Backend.Domain.Entities.Generic
{
    public class EntityBase<T> : DomainEntity, IEntityBase<T>
    {
        public virtual T Id { get; set; }
        public DateTime CreationOn { get; set; }
        public DateTime UpdateOn { get; set; }
    }
}
