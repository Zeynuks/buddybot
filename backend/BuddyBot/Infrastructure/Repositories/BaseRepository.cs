using Infrastructure.Foundation.Database;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repositories;
public abstract class BaseRepository<TEntity> where TEntity : class
{
    protected readonly BuddyBotDbContext _dbContext;

    protected BaseRepository( BuddyBotDbContext dbContext )
    {
        _dbContext = dbContext;
    }

    public virtual void Add( TEntity entity )
    {
        _dbContext.Set<TEntity>().Add( entity );
    }

    public virtual async Task<TEntity?> Get( int id )
    {
        return await _dbContext.Set<TEntity>().FindAsync( id );
    }

    public virtual async Task<List<TEntity>> GetAll()
    {
        return await _dbContext.Set<TEntity>().ToListAsync();
    }

    public virtual async Task Delete( int id )
    {
        TEntity? entity = await _dbContext.Set<TEntity>().FindAsync( id );
        if ( entity != null )
        {
            _dbContext.Set<TEntity>().Remove( entity );
        }
    }
}
