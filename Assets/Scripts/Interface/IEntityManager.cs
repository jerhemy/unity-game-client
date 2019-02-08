using Client.Entity;

namespace Interface
{
    public interface IEntityManager
    {
        bool AddEntity(Entity entity);
        bool RemoveEntity(long id);
        
        bool AddClient(long id, Entity client);
        bool RemoveClient(long id);

    }
}