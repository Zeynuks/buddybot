namespace TelegramBot.Helpers;
public enum TokenParseResult
{
    NoToken,
    InvalidTokenFormat,
    TokenParsed
}
public static class InviteTokenHelper
{
    public static TokenParseResult TryParseTokenFromStartCommand( string? messageText, out Guid token )
    {
        token = default;
        if ( string.IsNullOrWhiteSpace( messageText ) )
        {
            return TokenParseResult.NoToken;
        }

        string[] parts = messageText.Split( ' ', 2, StringSplitOptions.RemoveEmptyEntries );
        if ( parts.Length < 2 )
        {
            return TokenParseResult.NoToken;
        }

        if ( Guid.TryParse( parts[ 1 ], out token ) )
        {
            return TokenParseResult.TokenParsed;
        }
        else
        {
            return TokenParseResult.InvalidTokenFormat;
        }
    }
}
