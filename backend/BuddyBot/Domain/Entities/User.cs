using Domain.Enums;

namespace Domain.Entities;
public class User
{
    public int Id { get; init; }
    public int? TeamId { get; private set; }
    public string? Login { get; private set; }
    public string? PasswordHash { get; private set; }
    public DateTime? OnboardingAccessTimeUtc { get; private set; }
    public DateTime CreatedAtUtc { get; init; }
    public bool IsDeleted { get; private set; }

    public HRInfo? HRInfo { get; private set; }
    public UserContactInfo? ContactInfo { get; private set; }
    public UserAuthToken? AuthToken { get; init; }
    public OnboardingAccessRequest? OnboardingAccessRequest { get; private set; }
    public Team? Team { get; private set; }

    public List<Role> Roles { get; private set; }
    public List<Feedback> Feedbacks { get; private set; }
    public List<CandidateProcess> CandidateProcesses { get; private set; }
    public List<User> Mentors { get; private set; }
    public List<User> HRs { get; private set; }
    public List<User> MentoredCandidates { get; private set; }
    public List<User> HRCandidates { get; private set; }

    public User()
    {
        Roles = new List<Role>();
        MentoredCandidates = new List<User>();
        HRCandidates = new List<User>();
        Mentors = new List<User>();
        HRs = new List<User>();
        CandidateProcesses = new List<CandidateProcess>();
        Feedbacks = new List<Feedback>();
    }

    public User(DateTime createdAtUtc )
    {
        CreatedAtUtc = createdAtUtc;
        Roles = new List<Role>();
        MentoredCandidates = new List<User>();
        HRCandidates = new List<User>();
        Mentors = new List<User>();
        HRs = new List<User>();
        CandidateProcesses = new List<CandidateProcess>();
        Feedbacks = new List<Feedback>();
    }

    public User( DateTime createdAtUtc, int teamId )
        : this( createdAtUtc )
    {
        TeamId = teamId;
    }

    public User( DateTime createdAtUtc, string login, string passwordHash )
        : this( createdAtUtc )
    {
        Login = login;
        PasswordHash = passwordHash;
    }

    public void SetLogin( string login)
    {
        Login = login;
    }
    
    public void SetPasswordHash( string passwordHash )
    {
        PasswordHash = passwordHash;
    }

    public void SetTeam( Team? team )
    {
        Team = team;
        TeamId = team?.Id;
    }

    public void SetHRInfo( HRInfo hrInfo )
    {
        HRInfo = hrInfo;
    }

    public void SetContactInfo( UserContactInfo contactInfo )
    {
        ContactInfo = contactInfo;
    }

    public void SetOnboardingAccessRequest( OnboardingAccessRequest onboardingAccessRequest )
    {
        OnboardingAccessRequest = onboardingAccessRequest;
    }

    public void SetOnboardingAccessTime( DateTime onboardingAccessTimeUtc )
    {
        OnboardingAccessTimeUtc = onboardingAccessTimeUtc;
    }

    public void ResetOnboardingAccessTime()
    {
        OnboardingAccessTimeUtc = null;
    }

    public void SetHRs( List<User> hrs )
    {
        hrs ??= new List<User>();
        HRs.RemoveAll( existing => !hrs.Any( newItem => newItem.Id == existing.Id ) );

        foreach ( User newUser in hrs )
        {
            if ( !HRs.Any( existing => existing.Id == newUser.Id ) )
            {
                HRs.Add( newUser );
            }
        }
    }

    public void SetMentors( List<User> mentors )
    {
        mentors ??= new List<User>();
        Mentors.RemoveAll( existing => !mentors.Any( newItem => newItem.Id == existing.Id ) );

        foreach ( User newMentor in mentors )
        {
            if ( !Mentors.Any( existing => existing.Id == newMentor.Id ) )
            {
                Mentors.Add( newMentor );
            }
        }
    }
 
    public bool IsHR()
    {
        return Roles.Any( r => r.RoleName == RoleName.HR || r.RoleName == RoleName.SuperHR );
    }

    public bool IsCandidate()
    {
        return Roles.Any( r => r.RoleName == RoleName.Candidate );
    }

    public List<User> GetHRs()
    {
        return HRs ?? new List<User>();
    }

    public List<User> GetMentors()
    {
        return Mentors ?? new List<User>();
    }

    public void SoftDelete()
    {
        IsDeleted = true;
    }
}