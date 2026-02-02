using Domain.Enums;

namespace Domain.Entities;
public class Feedback
{
    public int Id { get; init; }
    public int CandidateId { get; init; }
    public ProcessKind ProcessKind { get; init; }
    public int Rating { get; private set; }
    public string? Comment { get; private set; }
    public DateTime CreatedAtUtc { get; init; }
    public FeedbackState State { get; private set; }
    public bool IsDeleted { get; private set; }

    public User? Candidate { get; init; }

    public Feedback( int candidateId, ProcessKind processKind, int rating )
    {
        CandidateId = candidateId;
        Rating = rating;
        ProcessKind = processKind;
        CreatedAtUtc = DateTime.UtcNow;
        State = FeedbackState.Draft;
    }

    public Feedback( int candidateId, ProcessKind processKind, int rating, string comment )
        : this ( candidateId, processKind, rating)
    {
        Comment = comment;
    }

    public void ConfirmByUser( string? comment )
    {
        State = FeedbackState.ConfirmedByUser;
        if ( !string.IsNullOrWhiteSpace( comment ) )
        {
            Comment = comment;
        }
    }

    public void SetRating( int rating )
    {
        Rating = rating;
    }

    public void SetComment( string? comment )
    {
        Comment = comment;
    }

    public void SoftDelete()
    {
        IsDeleted = true;
    }
}
