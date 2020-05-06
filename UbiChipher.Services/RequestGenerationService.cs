using QRCoder;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace UbiChipher.Services
{
    public class RequestGenerationService
    {
        public async Task<byte[]> CreateQRAsync(string text)
        {
            //if (comboBoxECC.SelectedItem != null)
            {
                //Create generator
                string level = "Q";
                QRCodeGenerator.ECCLevel eccLevel = (QRCodeGenerator.ECCLevel)(level == "L" ? 0 : level == "M" ? 1 : level == "Q" ? 2 : 3);

                //Create raw qr code data
                QRCodeGenerator qrGenerator = new QRCodeGenerator();
                QRCodeData qrCodeData = qrGenerator.CreateQrCode(text, eccLevel);

                //Create byte/raw bitmap qr code
                BitmapByteQRCode qrCodeBmp = new BitmapByteQRCode(qrCodeData);
                byte[] qrCodeImageBmp = qrCodeBmp.GetGraphic(20, new byte[] { 0, 0, 0 }, new byte[] { 255, 255, 255 });

                ////Create byte/raw png qr code
                //PngByteQRCode qrCodePng = new PngByteQRCode(qrCodeData);
                //byte[] qrCodeImagePng = qrCodePng.GetGraphic(20, new byte[] { 144, 201, 111 }, new byte[] { 118, 126, 152 });

                //QRImage.Source = LoadImage(qrCodeImageBmp);

                return qrCodeImageBmp;

            }
        }




    }
}
