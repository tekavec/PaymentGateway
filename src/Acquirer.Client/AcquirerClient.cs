﻿using System;
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

        public async Task<AcquirerProcessingResult> ProcessPayment(CreatePayment createPayment)
        {
            var acquirerPayment = new AcquirerPaymentDto
            {
                CardNumber = createPayment.CardNumber,
                Cvv = createPayment.Cvv,
                ExpiryYear = createPayment.ExpiryYear,
                ExpiryMonth = createPayment.ExpiryMonth,
                Amount = createPayment.Amount,
                Currency = createPayment.Currency
            };
            var json = JsonConvert.SerializeObject(acquirerPayment, DefaultJsonSerializerSetting());
            var requestMessage = new HttpRequestMessage(HttpMethod.Post, "payment")
            {
                Content = new StringContent(json, Encoding.UTF8)
            };

            var response = await httpClient.SendAsync(requestMessage);
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
