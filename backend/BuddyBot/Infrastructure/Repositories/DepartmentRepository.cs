using Application.Filters;
using Domain.Entities;
using Domain.Filters;
using Domain.Repositories;
using Infrastructure.Foundation.Database;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repositories;
public class DepartmentRepository : BaseRepository<Department>, IDepartmentRepository
{
    public DepartmentRepository( BuddyBotDbContext dbContext ) : base( dbContext )
    {
    }

    public async Task<List<Department>> GetFilteredDepartments( IEnumerable<IFilter<Department>> filters )
    {
        IQueryable<Department> query = _dbContext.Set<Department>()
            .Include( d => d.Teams );

        query = query.OrderByDescending( d => d.Id );
        query = query.ApplyFilters( filters );
        return await query.ToListAsync();
    }
    public async Task<int> CountFilteredDepartments( IEnumerable<IFilter<Department>> filters )
    {
        IQueryable<Department> query = _dbContext.Set<Department>();
        query = query.ApplyFilters( filters );
        return await query.CountAsync();
    }

    public override async Task<Department?> Get( int id )
    {
        return await _dbContext.Set<Department>()
            .Include( d => d.Teams )
            .FirstOrDefaultAsync( d => d.Id == id );
    }

    public async Task<List<Department>> GetDepartmentsLookup()
    {
        return await _dbContext.Set<Department>()
            .OrderByDescending( d => d.Id )
            .ToListAsync();
    }

}
