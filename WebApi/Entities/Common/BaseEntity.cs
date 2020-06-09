
namespace Entities.Common
{
    public interface IEntity
    {
        //Each class that implement IEntity will have a table in Database!!!
    }

    public abstract class BaseEntity<TKey> : IEntity
    {
        public TKey Id { get; set; }
    }

    public abstract class BaseEntity : BaseEntity<int>
    {
    }
}
