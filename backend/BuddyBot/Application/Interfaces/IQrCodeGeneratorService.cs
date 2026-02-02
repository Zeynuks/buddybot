namespace Application.Interfaces;
public interface IQrCodeGeneratorService
{
    string GenerateQrCodeBase64( string inviteLink );
}