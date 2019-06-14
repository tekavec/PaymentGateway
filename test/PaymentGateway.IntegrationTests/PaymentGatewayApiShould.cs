using System;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using FluentAssertions;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.AspNetCore.TestHost;
using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json;
using PaymentGateway.Infrastructure.Security;
using PaymentGateway.Models;
using Xunit;
using static PaymentGateway.IntegrationTests.TestHelpers;

namespace PaymentGateway.IntegrationTests
{
    public class PaymentGatewayApiShould : IClassFixture<WebApplicationFactory<Startup>>
    {
        private readonly WebApplicationFactory<Startup> factory;
        private readonly HttpClient authorizedClient;
        private readonly ITokenGenerator tokenGenerator;

        public PaymentGatewayApiShould(WebApplicationFactory<Startup> factory)
        {
            this.factory = factory;

            var builder = Program.CreateWebHostBuilder(null)
                .ConfigureTestServices(_ => { })
                .UseEnvironment("Testing");
            var server = new TestServer(builder)
            {
                BaseAddress = new Uri("https://localhost/")
            };

            var serviceProvider = server.Host.Services;
            tokenGenerator = serviceProvider.GetService<ITokenGenerator>();
            authorizedClient = server.CreateClient();
        }

        [Fact]
        public async Task return_200OK_for_diagnostic_alive_check()
        {
            var unauthorizedClient = factory.CreateClient();

            var response = await unauthorizedClient.GetAsync("diagnostics/alive");

            response.StatusCode.Should().Be(StatusCodes.Status200OK);
        }

        [Fact]
        public async Task return_401Unauthorized_for_valid_but_unauthorized_request()
        {
            var paymentId = Guid.NewGuid();
            var unauthorizedClient = factory.CreateClient();

            var response = await unauthorizedClient.GetAsync($"payment/{paymentId}");

            response.StatusCode.Should().Be(StatusCodes.Status401Unauthorized);
        }

        [Fact]
        public async Task return_404NotFound_when_retrieving_non_existing_payment()
        {
            await AuthorizeClient();
            var paymentId = Guid.NewGuid();

            var response = await authorizedClient.GetAsync($"payment/{paymentId}");

            response.StatusCode.Should().Be(StatusCodes.Status404NotFound);
        }

        [Fact]
        public async Task process_payment_and_return_with_201Created_code_and_route_value()
        {
            await AuthorizeClient();
            var defaultPage = await authorizedClient.GetAsync("/payment");
            var request = new HttpRequestMessage(HttpMethod.Post, defaultPage.RequestMessage.RequestUri)
            {
                Content = new StringContent(
                    GetValidMakePaymentV1Json(), 
                    Encoding.UTF8, 
                    "application/json")
            };
            var response = await authorizedClient.SendAsync(request);

            response.StatusCode.Should().Be(StatusCodes.Status201Created);
            response.Headers.Location.AbsolutePath.Should().StartWith(defaultPage.RequestMessage.RequestUri.AbsolutePath);
        }

        [Fact]
        public async Task return_200OK_and_return_masked_card_number_when_retrieving_previously_created_payment()
        {
            var cardNumber = "12345678";
            await AuthorizeClient();
            var defaultPage = await authorizedClient.GetAsync("/payment");
            var makePaymentV1 = GetValidMakePaymentV1Json(cardNumber);
            var request = new HttpRequestMessage(HttpMethod.Post, defaultPage.RequestMessage.RequestUri)
            {
                Content = new StringContent(
                    makePaymentV1, 
                    Encoding.UTF8, 
                    "application/json")
            };
            var createPaymentResponse = await authorizedClient.SendAsync(request);

            createPaymentResponse.StatusCode.Should().Be(StatusCodes.Status201Created);

            var result = await authorizedClient.GetAsync(createPaymentResponse.Headers.Location.PathAndQuery);

            result.StatusCode.Should().Be(StatusCodes.Status200OK);

            var contentString = (await result.Content.ReadAsStringAsync());
            var paymentDetail = JsonConvert.DeserializeObject<PaymentDetails>(contentString);
            paymentDetail.CardNumber.Should().Be("******5678");

        }

        private async Task AuthorizeClient()
        {
            var token = await tokenGenerator.GenerateToken();
            authorizedClient.DefaultRequestHeaders.Add("Authorization", $"Bearer {token}");
        }
    }
}
