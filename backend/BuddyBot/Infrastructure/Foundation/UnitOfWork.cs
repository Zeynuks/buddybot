using Application;
using Domain.Repositories;
using Infrastructure.Foundation.Database;
using Infrastructure.Repositories;

namespace Infrastructure.Foundation;
public class UnitOfWork : IUnitOfWork
{
    private readonly BuddyBotDbContext _dbContext;

    public UnitOfWork( BuddyBotDbContext dbContext )
    {
        _dbContext = dbContext;
    }

    public async Task CommitAsync()
    {
        _ = await _dbContext.SaveChangesAsync();
    }
}
