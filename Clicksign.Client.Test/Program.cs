using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;

namespace Clicksign.Client.Test
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            var clicksign = new ClicksignClient("https://sandbox.clicksign.com", "272037a5-8f03-4908-a60c-6af9e83afc08");

            var bytes = File.ReadAllBytes("C:\\test.pdf");

            var document = await clicksign.UploadDocument("/files/test1.pdf", bytes);

            var signer = await clicksign.CreateSigner("Danilo Delgado", "303.748.600-76", new DateTime(1999, 01, 01), "zzzzzzzzzzz@gmail.com", "71999999999", NotificationType.Email);

            var documentSigner = await clicksign.CreateDocumentSigner(document.Document.Key, signer.Key, SignerSign.Sign);

            var documentInfo = await clicksign.GetDocument(document.Document.Key);

            await clicksign.NotifyEmail(documentInfo.Document.Signers[0].RequestSignatureKey, "test test");
        }
    }
}
