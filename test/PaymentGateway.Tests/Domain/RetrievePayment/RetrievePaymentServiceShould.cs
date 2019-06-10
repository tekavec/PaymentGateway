using System;
using System.Threading.Tasks;
using FluentAssertions;
using LaYumba.Functional;
using Moq;
using PaymentGateway.Domain.Persistence;
using PaymentGateway.Domain.RetrievePayment;
using PaymentGateway.Models;
using Xunit;
using static PaymentGateway.Tests.Fakes;
using static LaYumba.Functional.F;

namespace PaymentGateway.Tests.Domain.RetrievePayment
{
    public class RetrievePaymentServiceShould
    {
        private readonly Mock<IReadPaymentRepository> paymentRepository = new Mock<IReadPaymentRepository>(MockBehavior.Strict);

        [Fact]
        public async Task retrieve_payment_details_if_payment_exist()
        {
            var paymentId = Guid.NewGuid();
            var retrievePaymentService = new RetrievePaymentService(paymentRepository.Object);
            var paymentDetails = Some(CreatePaymentDetails(paymentId));
            paymentRepository.Setup(a => a.Read(paymentId)).ReturnsAsync(paymentDetails);

            var result = await retrievePaymentService.Get(paymentId);

            result.Should().Be(paymentDetails);
        }

        [Fact]
        public async Task not_retrieve_payment_details_if_payment_does_not_exist()
        {
            var id = Guid.NewGuid();
            var retrievePaymentService = new RetrievePaymentService(paymentRepository.Object);
            paymentRepository.Setup(a => a.Read(id)).ReturnsAsync(It.IsAny<Option<PaymentDetails>>());

            var result = await retrievePaymentService.Get(id);

            result.Should().Be((Option<PaymentDetails>) None);
        }
    }
}