using System;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc.Testing;
using Xunit;
using static PaymentGateway.IntegrationTests.TestHelpers;

namespace PaymentGateway.IntegrationTests
{
    public class PaymentGatewayApiShould : IClassFixture<WebApplicationFactory<Startup>>
    {
        private readonly WebApplicationFactory<Startup> factory;

        public PaymentGatewayApiShould(WebApplicationFactory<Startup> factory)
        {
            this.factory = factory;
        }

        [Fact]
        public async Task return_NotFound_when_retrieving_non_existing_payment()
        {
            var paymentId = Guid.NewGuid();
            var client = factory.CreateClient();

            var response = await client.GetAsync($"payment/{paymentId}");

            response.StatusCode.Should().Be(404);
        }

        [Fact]
        public async Task process_payment_and_return_with_Created_code_and_route_value()
        {
            var client = factory.CreateClient(new WebApplicationFactoryClientOptions
            {
                AllowAutoRedirect = false
            });
            var defaultPage = await client.GetAsync("/payment");
            var request = new HttpRequestMessage(HttpMethod.Post, defaultPage.RequestMessage.RequestUri)
            {
                Content = new StringContent(
                    GetValidMakePaymentV1Json(), 
                    Encoding.UTF8, 
                    "application/json")
            };
            var response = await client.SendAsync(request);

            response.StatusCode.Should().Be(201);
            response.Headers.Location.AbsolutePath.Should().Contain("/payment/get");
        }
    }
}
