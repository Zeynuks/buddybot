using Microsoft.AspNetCore.Http;

namespace Application.Interfaces;
public interface IFileStorageService
{
    Task<string> SaveFile( IFormFile file );
    Task<(Stream Stream, string ContentType, string FileName)?> GetFile( string id );
}
