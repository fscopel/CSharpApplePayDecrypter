using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExampleDecryption
{
    public class ApplePaymentToken
    {
        public PaymentData paymentData { get; set; }
        public PaymentMethod paymentMethod { get; set; }
        public string transactionIdentifier { get; set; }
    }

    public class Header
    {

        public string ephemeralPublicKey { get; set; }
        public string publicKeyHash { get; set; }
        public string transactionId { get; set; }
    }

    public class PaymentData
    {
        public string version { get; set; }
        public string data { get; set; }
        public string signature { get; set; }
        public Header header { get; set; }
    }

    public class PaymentMethod
    {
        public string displayName { get; set; }
        public string network { get; set; }
        public string type { get; set; }
    }

   
}
