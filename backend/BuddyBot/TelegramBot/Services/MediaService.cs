namespace TelegramBot.Services;
public class MediaService
{
    private readonly IHttpClientFactory _httpClientFactory;
    private readonly string _baseUrl;

    public MediaService( IHttpClientFactory httpClientFactory, IConfiguration configuration )
    {
        _httpClientFactory = httpClientFactory;
        _baseUrl = configuration.GetValue<string>( "ApiUrl" );
    }

    public async Task<Stream?> DownloadMedia( string mediaId )
    {
        var mediaUrl = $"{_baseUrl}/media/{mediaId}";
        var client = _httpClientFactory.CreateClient();
        var response = await client.GetAsync( mediaUrl );

        if ( response.IsSuccessStatusCode )
        {
            return await response.Content.ReadAsStreamAsync();
        }

        return null;
    }
}
