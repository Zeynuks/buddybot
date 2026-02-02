namespace Domain.Repositories;
public interface IBaseRepository<TEntity> where TEntity : class
{
    Task<TEntity?> Get( int id );
    Task<List<TEntity>> GetAll();
    void Add( TEntity entity );
    Task Delete( int id );
}
