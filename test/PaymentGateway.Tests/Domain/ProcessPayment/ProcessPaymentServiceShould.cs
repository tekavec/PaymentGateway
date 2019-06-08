using System;
using System.Threading.Tasks;
using FluentAssertions;
using Moq;
using PaymentGateway.Domain;
using PaymentGateway.Domain.ProcessPayment;
using PaymentGateway.Models;
using Xunit;

namespace PaymentGateway.Tests.Domain.ProcessPayment
{
    public class ProcessPaymentServiceShould
    {
        private readonly Mock<IAcquirerClient> acquirerClient = new Mock<IAcquirerClient>(MockBehavior.Strict);

        [Fact]
        public async Task process_a_payment_and_return_successful_result()
        {
            var paymentId = Guid.NewGuid();
            acquirerClient.Setup(a => a.ProcessPayment(It.IsAny<MakePaymentV1>()))
                .ReturnsAsync(PaymentProcessingResult.CreateSuccessfulResult(paymentId));
            var processPaymentService = new ProcessPaymentService(acquirerClient.Object);

            var result = await processPaymentService.Process(new MakePaymentV1());

            result.Should().BeOfType<PaymentProcessingResult>();
            result.PaymentId.Should().Be(paymentId);
            result.Status.Should().Be(PaymentProcessStatus.Succeeded);
        }

        [Fact]
        public async Task process_a_payment_and_return_failure_result()
        {
            var paymentId = Guid.NewGuid();
            acquirerClient.Setup(a => a.ProcessPayment(It.IsAny<MakePaymentV1>()))
                .ReturnsAsync(PaymentProcessingResult.CreateFailedResult(paymentId));
            var processPaymentService = new ProcessPaymentService(acquirerClient.Object);

            var result = await processPaymentService.Process(new MakePaymentV1());

            result.Should().BeOfType<PaymentProcessingResult>();
            result.PaymentId.Should().Be(paymentId);
            result.Status.Should().Be(PaymentProcessStatus.Failed);
        }
    }
}