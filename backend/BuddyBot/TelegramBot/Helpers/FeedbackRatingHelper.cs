namespace TelegramBot.Helpers;

public static class FeedbackRatingHelper
{
    public static bool TryParseFeedbackRating( string? callbackData, out int rating )
    {
        rating = default;
        if ( string.IsNullOrWhiteSpace( callbackData ) )
        {
            return false;
        }

        string[] parts = callbackData.Split( ':' );
        if ( parts.Length != 3 )
        {
            return false;
        }

        return int.TryParse( parts[ 2 ], out rating );
    }
}
