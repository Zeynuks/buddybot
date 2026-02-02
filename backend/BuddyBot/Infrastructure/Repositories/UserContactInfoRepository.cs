using Domain.Entities;
using Domain.Repositories;
using Infrastructure.Foundation.Database;

namespace Infrastructure.Repositories;
public class UserContactInfoRepository : BaseRepository<UserContactInfo>, IUserContactInfoRepository 
{
    public UserContactInfoRepository( BuddyBotDbContext dbContext ) : base( dbContext )
    {
    }
}
