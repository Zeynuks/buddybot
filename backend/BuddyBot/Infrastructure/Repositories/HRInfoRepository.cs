using Domain.Entities;
using Domain.Repositories;
using Infrastructure.Foundation.Database;

namespace Infrastructure.Repositories;
public class HRInfoRepository : BaseRepository<HRInfo>, IHRInfoRepository
{
    public HRInfoRepository( BuddyBotDbContext dbContext ) : base( dbContext )
    {
    }
}
