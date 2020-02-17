using System;


namespace ApplePayDecrypter.Definition
{
    public class ApplePayRequest
    {
        public string Data { get; set; }
        public ApplePayHeader ApplePayHeader { get; set; }
        public byte[] PrivateKeyBytes { get; set; }
        public string ApplePayP12Path { get; set; }
    }
}
