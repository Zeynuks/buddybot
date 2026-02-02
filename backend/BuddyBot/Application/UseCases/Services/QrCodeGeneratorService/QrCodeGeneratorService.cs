using Application.Interfaces;
using QRCoder;

namespace Application.UseCases.Services.QrCodeGeneratorService;
public class QrCodeGeneratorService : IQrCodeGeneratorService
{
    public string GenerateQrCodeBase64( string inviteLink )
    {
        var qrGenerator = new QRCodeGenerator();
        var qrCodeData = qrGenerator.CreateQrCode( inviteLink, QRCodeGenerator.ECCLevel.Q );

        var pngByteQrCode = new PngByteQRCode( qrCodeData );
        var qrCodeAsPngByteArr = pngByteQrCode.GetGraphic( 20 );

        return "data:image/png;base64," + Convert.ToBase64String( qrCodeAsPngByteArr );
    }
}