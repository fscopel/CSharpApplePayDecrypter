using System;
using System.IO;
using Newtonsoft.Json;
using ApplePayDecrypter.Definition;

namespace ExampleDecryption
{
    class Program
    {
        static readonly string paymentToken = @"C:\temp\payment.json";
        static readonly string privateKey = @"C:\temp\privateKey.der";
        static readonly string p12FileLocation = @"C:\temp\certificate.p12";

        static void Main(string[] args)
        {
            var encryptedToken = JsonConvert.DeserializeObject<ApplePaymentToken>(File.ReadAllText(paymentToken));

            var aRequest = new ApplePayRequest()
            {
                ApplePayHeader = new ApplePayHeader()
                {
                    TransactionId = encryptedToken.paymentData.header.transactionId,
                    EphemeralPublicKey = encryptedToken.paymentData.header.ephemeralPublicKey,
                    PublicKeyHash = encryptedToken.paymentData.header.publicKeyHash,
                },
                Data = encryptedToken.paymentData.data,
                PrivateKeyBytes = File.ReadAllBytes(privateKey),
                ApplePayP12Path = p12FileLocation
            };

            var aPay = new ApplePayDecrypter.ApplePayDecrypt(aRequest);
            var result = aPay.Decrypt();
            var decryptedData = JsonConvert.DeserializeObject<AppleDecryptedPayment>(result);

            Console.WriteLine(result);
            Console.ReadLine();
        }
    }
}
