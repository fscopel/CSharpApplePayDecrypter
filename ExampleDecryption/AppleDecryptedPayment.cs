using Newtonsoft.Json;

namespace ExampleDecryption
{
    public class AppleDecryptedPayment
    {
        [JsonProperty("applicationPrimaryAccountNumber")]
        public string ApplicationPrimaryAccountNumber { get; set; }

        [JsonProperty("applicationExpirationDate")]
        public string ApplicationExpirationDate { get; set; }

        [JsonProperty("currencyCode")]
        public string CurrencyCode { get; set; }

        [JsonProperty("transactionAmount")]
        public long TransactionAmount { get; set; }

        [JsonProperty("deviceManufacturerIdentifier")]
        public string DeviceManufacturerIdentifier { get; set; }

        [JsonProperty("paymentDataType")]
        public string PaymentDataType { get; set; }

        [JsonProperty("paymentData")]
        public ApplePaymentData PaymentData { get; set; }
    }

    public class ApplePaymentData
    {
        [JsonProperty("onlinePaymentCryptogram")]
        public string OnlinePaymentCryptogram { get; set; }

        [JsonProperty("eciIndicator")]
        public string EciIndicator { get; set; }
    }
}
