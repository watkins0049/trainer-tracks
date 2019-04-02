using System;
namespace TrainerTracks.Data.Repository
{
    public interface IRepository<TEntity, in TKey> where TEntity : class
    {
        TEntity GetById(TKey id);
    }
}
