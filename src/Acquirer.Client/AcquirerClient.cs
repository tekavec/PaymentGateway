using System;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Acquirer.Client.Domain;
using Acquirer.Client.Dtos;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace Acquirer.Client
{
    public class AcquirerClient : IAcquirerClient
    {
        private readonly HttpClient httpClient;

        public AcquirerClient(HttpClient httpClient)
        {
            this.httpClient = httpClient;
        }

        public async Task<AcquirerProcessingResult> ProcessPayment(CreatePayment createPayment, Uri acquirerUri)
        {
            var acquirerPayment = new AcquirerPaymentDto
            {
                CardHolder = createPayment.CardHolder,
                CardNumber = createPayment.CardNumber,
                Cvv = createPayment.Cvv,
                ExpiryYear = createPayment.ExpiryYear,
                ExpiryMonth = createPayment.ExpiryMonth,
                Amount = createPayment.Amount,
                Currency = createPayment.Currency
            };
            var json = JsonConvert.SerializeObject(acquirerPayment, DefaultJsonSerializerSetting());
            var paymentContent = new StringContent(json, Encoding.UTF8);
            var response = await httpClient.PostAsync(acquirerUri, paymentContent);

            if (response.IsSuccessStatusCode)
            {
                var responseJson = await response.Content.ReadAsStringAsync();
                var acquirerResponse =
                    JsonConvert.DeserializeObject<AcquirerResponseDto>(responseJson, DefaultJsonSerializerSetting());
                return new AcquirerProcessingResult(acquirerResponse.PaymentId, acquirerResponse.IsPaymentSuccessful);
            }

            return new AcquirerProcessingResult(Guid.Empty, false);
        }

        private static JsonSerializerSettings DefaultJsonSerializerSetting() 
            => new JsonSerializerSettings{ ContractResolver = new DefaultContractResolver()};
    }
}
