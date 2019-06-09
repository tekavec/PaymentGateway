using System;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using Acquirer.Client.Domain;
using FluentAssertions;
using Xunit;

namespace Acquirer.Client.UnitTests
{
    public class AcquirerClientShould
    {
        private readonly HttpMessageHandlerStub httpMessageHandler;
        private readonly AcquirerClient acquirerClient;

        public AcquirerClientShould()
        {
            httpMessageHandler = new HttpMessageHandlerStub
            {
                ResponseMessage = new HttpResponseMessage()
            };

            acquirerClient = new AcquirerClient(new HttpClient(httpMessageHandler));
        }

        [Fact]
        public async Task return_successful_processing_result_if_payment_was_successfully_processed()
        {
            var createPayment = GetCreatePayment();
            httpMessageHandler.ResponseMessage.Content = new StringContent(@"
                {
                    'PaymentId':'A058E3B1-0163-4A5A-9A28-D53DC5B9FF15',
                    'IsPaymentSuccessful': true
                }
            ");

            var result = await acquirerClient.ProcessPayment(
                createPayment, 
                new Uri("http://acquirer/createpayment"));

            result.AcquirerPaymentId.Should().Be(new Guid("A058E3B1-0163-4A5A-9A28-D53DC5B9FF15"));
            result.IsPaymentSuccessful.Should().Be(true);
        }

        [Fact]
        public async Task return_unsuccessful_processing_result_if_payment_was_not_successfully_processed()
        {
            var createPayment = GetCreatePayment();
            httpMessageHandler.ResponseMessage.Content = new StringContent(@"
                {
                    'PaymentId':'B112F772-BF9A-457C-AB0F-DD6C3C32F436',
                    'IsPaymentSuccessful': false
                }
            ");

            var result = await acquirerClient.ProcessPayment(
                createPayment, 
                new Uri("http://acquirer/createpayment"));

            result.AcquirerPaymentId.Should().Be(new Guid("B112F772-BF9A-457C-AB0F-DD6C3C32F436"));
            result.IsPaymentSuccessful.Should().Be(false);
        }

        [Theory]
        [InlineData(HttpStatusCode.BadRequest)]
        [InlineData(HttpStatusCode.InternalServerError)]
        [InlineData(HttpStatusCode.Forbidden)]
        public async Task return_processing_result_without_payment_id_if_acquirer_responded_with_unsuccessful_status_code(HttpStatusCode statusCode)
        {
            var createPayment = GetCreatePayment();
            httpMessageHandler.ResponseMessage = new HttpResponseMessage(statusCode);

            var result = await acquirerClient.ProcessPayment(
                createPayment, 
                new Uri("http://acquirer/createpayment"));

            result.AcquirerPaymentId.Should().Be(Guid.Empty);
            result.IsPaymentSuccessful.Should().Be(false);
        }

        private static CreatePayment GetCreatePayment()
            => new CreatePayment(
                cardHolder: "a card holder",
                cardNumber: "a card number",
                cvv: "123",
                expiryYear: 2029,
                expiryMonth: 12,
                amount: 12345.67m,
                currency: "GBP");
    }
}
