using System;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using PaymentGateway.Models;

namespace PaymentGateway.IntegrationTests
{
    public static class TestHelpers
    {
        public static string GetValidMakePaymentV1Json()
            => JsonConvert.SerializeObject(GetValidMakePaymentV1(), DefaultJsonSerializerSetting());

        private static MakePaymentV1 GetValidMakePaymentV1()
            => new MakePaymentV1
            {
                CardHolder = "a card holder",
                CardNumber = "12345678",
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