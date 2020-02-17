using System;


namespace ApplePayDecrypter.Definition
{
    public class ApplePayHeader
    {
        public string EphemeralPublicKey { get; set; }
        public string PublicKeyHash { get; set; }
        public string TransactionId { get; set; }
    }
}
