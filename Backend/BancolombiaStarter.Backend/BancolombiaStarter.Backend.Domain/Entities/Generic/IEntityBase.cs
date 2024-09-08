namespace BancolombiaStarter.Backend.Domain.Entities.Generic
{
    public interface IEntityBase<T>
    {
        T Id { get; set; }
    }
}
