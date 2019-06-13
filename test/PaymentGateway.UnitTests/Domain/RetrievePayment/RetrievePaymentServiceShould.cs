using System;
using System.Threading.Tasks;
using FluentAssertions;
using LaYumba.Functional;
using Microsoft.Extensions.Logging;
using Moq;
using PaymentGateway.Domain.Persistence;
using PaymentGateway.Domain.RetrievePayment;
using PaymentGateway.Models;
using Xunit;

namespace PaymentGateway.UnitTests.Domain.RetrievePayment
{
    public class RetrievePaymentServiceShould
    {
        private readonly Mock<IReadPaymentRepository> paymentRepository = new Mock<IReadPaymentRepository>(MockBehavior.Strict);
        private readonly Mock<ILogger<RetrievePaymentService>> logger = new Mock<ILogger<RetrievePaymentService>>();
        private readonly RetrievePaymentService retrievePaymentService;

        public RetrievePaymentServiceShould()
        {
            retrievePaymentService = new RetrievePaymentService(paymentRepository.Object, logger.Object);
        }

        [Fact]
        public async Task retrieve_payment_details_if_payment_exist()
        {
            var paymentId = Guid.NewGuid();
            var paymentDetails = F.Some(TestHelpers.CreatePaymentDetails(paymentId));
            paymentRepository.Setup(a => a.Read(paymentId)).ReturnsAsync(paymentDetails);

            var result = await retrievePaymentService.Get(paymentId);

            result.Should().Be(paymentDetails);
        }

        [Fact]
        public async Task not_retrieve_payment_details_if_payment_does_not_exist()
        {
            var nonExistingPaymentId = Guid.NewGuid();
            paymentRepository.Setup(a => a.Read(nonExistingPaymentId)).ReturnsAsync(It.IsAny<Option<PaymentDetails>>());

            var result = await retrievePaymentService.Get(nonExistingPaymentId);

            result.Should().Be((Option<PaymentDetails>) F.None);
        }
    }
}