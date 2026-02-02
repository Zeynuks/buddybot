using Application.Interfaces;
using Application.Options;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Options;

namespace Infrastructure.FileUtils;
public class LocalFileStorage(
    IWebHostEnvironment env,
    IOptions<FileStorageOptions> options )
    : IFileStorageService
{
    public async Task<string> SaveFile( IFormFile file )
    {
        var root = Path.Combine( env.ContentRootPath, options.Value.RootPath! );
        Directory.CreateDirectory( root );

        var ext = Path.GetExtension( file.FileName );
        var fileName = $"{Guid.NewGuid()}{ext}";
        var fullPath = Path.Combine( root, fileName );

        await using var fs = File.Create( fullPath );
        await file.CopyToAsync( fs );

        return fileName;
    }

    public Task<(Stream Stream, string ContentType, string FileName)?> GetFile( string id )
    {
        var root = Path.Combine( env.ContentRootPath, options.Value.RootPath! );
        var fullPath = Path.Combine( root, id );

        if ( !File.Exists( fullPath ) )
            return Task.FromResult<(Stream, string, string)?>( null );

        var stream = File.OpenRead( fullPath );
        var ext = Path.GetExtension( fullPath ).ToLowerInvariant();
        var contentType = ext switch
        {
            ".png" => "image/png",
            ".jpg" => "image/jpeg",
            ".jpeg" => "image/jpeg",
            ".gif" => "image/gif",
            ".mp4" => "video/mp4",
            _ => "application/octet-stream"
        };

        return Task.FromResult<(Stream, string, string)?>( (stream, contentType, id) );
    }
}
