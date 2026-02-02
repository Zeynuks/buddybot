using Application.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace Admin.Api.Controllers;

[ApiController]
[Route( "api/media" )]
public class MediaController : ControllerBase
{
    private static readonly string[] AllowedExtensions = [ ".jpg", ".jpeg", ".png", ".mp4", ".webm" ];
    private const long MaxFileSizeBytes = 50 * 1024 * 1024;

    [HttpPost( "upload" )]
    public async Task<ActionResult<string>> Upload( 
        IFormFile file,
        [FromServices] IFileStorageService storage )
    {
        if ( file is null || file.Length == 0 )
        {
            return BadRequest( "Файл не предоставлен." );
        }

        if ( file.Length > MaxFileSizeBytes )
        {
            return BadRequest( "Превышен максимальный размер файла (50 МБ)." );
        }

        var extension = Path.GetExtension( file.FileName ).ToLowerInvariant();
        if ( !AllowedExtensions.Contains( extension ) )
        {
            return BadRequest( "Недопустимый формат файла. Разрешены: JPG, PNG, MP4, WEBM." );
        }

        var id = await storage.SaveFile( file );
        return Ok( id );
    }

    [HttpGet( "{id}" )]
    public async Task<IActionResult> Download(
        string id,
        [FromServices] IFileStorageService storage )
    {
        var result = await storage.GetFile( id );
        if ( result == null )
        {
            return NotFound();
        }

        var (stream, contentType, fileName) = result.Value;

        return File( stream, contentType, fileName );
    }
}
