using QRCoder;

namespace Payments.Application.Common
{
    public class Barcoder
    {
        public static byte[] GeneratorQR(string qrText)
        {
            using QRCodeGenerator qrGenerator = new QRCodeGenerator();
            using QRCodeData qrCodeData = qrGenerator.CreateQrCode(qrText, QRCodeGenerator.ECCLevel.Q);
            using QRCode qrCode = new QRCode(qrCodeData);
            System.Drawing.Bitmap qrCodeImage = qrCode.GetGraphic(20);
            return Imager.BitmapToBytes(qrCodeImage);
        }
    }
}
