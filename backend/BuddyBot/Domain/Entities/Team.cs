namespace Domain.Entities;
public class Team
{
    public int Id { get; init; }
    public int DepartmentId { get; private set; }
    public string Name { get; private set; }
    public int? LeaderId { get; private set; }
    public bool IsDeleted { get; private set; }

    public Department? Department { get; init; }
    public User? Leader { get; private set; }

    public List<User> Members { get; private set; }

    public Team( int departmentId, string name )
    {
        DepartmentId = departmentId;
        Name = name;
        Members = new List<User>();
    }

    public void AddMember( User user )
    {
        Members.Add( user );
        //user.Teams.Add( this );
    }

    public void AssignLeader( User user )
    {
        LeaderId = user.Id;
        Leader = user;
    }

    public void SetName( string name )
    {
        if ( !string.IsNullOrWhiteSpace( name ) )
        {
            Name = name;
        }
    }

    public void SetDepartment( int departmentId )
    {
        if ( departmentId > 0 )
        {
            DepartmentId = departmentId;
        }
    }
    public void RemoveLeader()
    {
        LeaderId = null;
        Leader = null;
    }

    public void SoftDelete()
    {
        IsDeleted = true;
    }
}
