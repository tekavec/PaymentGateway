using System;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using PaymentGateway.Models;

namespace PaymentGateway.IntegrationTests
{
    public static class TestHelpers
    {
        public static string GetValidMakePaymentV1Json(string cardNumber = "12345678")
            => JsonConvert.SerializeObject(GetValidMakePaymentV1(cardNumber), DefaultJsonSerializerSetting());

        private static MakePaymentV1 GetValidMakePaymentV1(string cardNumber)
            => new MakePaymentV1
            {
                CardNumber = cardNumber,
                Cvv = "123",
                ExpiryYear = DateTime.Today.Year + 1,
                ExpiryMonth = 12,
                Amount = 99.99m,
                Currency = "GBP"
            };

        private static JsonSerializerSettings DefaultJsonSerializerSetting()
            => new JsonSerializerSettings {ContractResolver = new DefaultContractResolver()};
    }
}