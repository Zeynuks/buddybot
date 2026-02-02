using Domain.Entities;
using Domain.Filters;

namespace Domain.Repositories;
public interface IDepartmentRepository : IBaseRepository<Department>
{
    Task<int> CountFilteredDepartments( IEnumerable<IFilter<Department>> filters );
    Task<List<Department>> GetFilteredDepartments( IEnumerable<IFilter<Department>> filters );
    Task<List<Department>> GetDepartmentsLookup();
}
