using System;
using System.Threading.Tasks;
using FluentAssertions;
using Moq;
using PaymentGateway.Domain;
using PaymentGateway.Domain.Persistence;
using PaymentGateway.Domain.ProcessPayment;
using PaymentGateway.Models;
using Xunit;
using static PaymentGateway.Tests.Fakes;

namespace PaymentGateway.Tests.Domain.ProcessPayment
{
    public class ProcessPaymentServiceShould
    {
        private readonly Mock<IAcquirerClient> acquirerClient = new Mock<IAcquirerClient>(MockBehavior.Strict);
        private readonly Mock<ISavePaymentRepository> savePaymentRepository = new Mock<ISavePaymentRepository>(MockBehavior.Strict);
        private readonly ProcessPaymentService processPaymentService;

        public ProcessPaymentServiceShould()
        {
            processPaymentService = new ProcessPaymentService(
                acquirerClient.Object, 
                savePaymentRepository.Object);
        }

        [Fact]
        public async Task process_a_payment_and_return_successful_result()
        {
            var acquirerPaymentId = Guid.NewGuid().ToString();
            var savePaymentResult = new SavePaymentResult(Guid.NewGuid());
            acquirerClient.Setup(a => a.ProcessPayment(It.IsAny<MakePaymentV1>()))
                .ReturnsAsync(CreateSuccessfulAcquirerProcessingResult(acquirerPaymentId));
            savePaymentRepository.Setup(a => a.Save(It.IsAny<NewPayment>()))
                .ReturnsAsync(savePaymentResult);

            var result = await processPaymentService.Process(new MakePaymentV1());

            result.Should().BeOfType<PaymentProcessingResult>();
            result.Key.Should().Be(savePaymentResult.Key);
            result.Status.Should().Be(PaymentProcessStatus.Succeeded);
        }

        [Fact]
        public async Task process_a_payment_and_return_failure_result()
        {
            var acquirerPaymentId = Guid.NewGuid().ToString();
            var savePaymentResult = new SavePaymentResult(Guid.NewGuid());
            acquirerClient.Setup(a => a.ProcessPayment(It.IsAny<MakePaymentV1>()))
                .ReturnsAsync(CreateUnsuccessfulAcquirerProcessingResult(acquirerPaymentId));
            savePaymentRepository.Setup(a => a.Save(It.IsAny<NewPayment>()))
                .ReturnsAsync(savePaymentResult);

            var result = await processPaymentService.Process(new MakePaymentV1());

            result.Should().BeOfType<PaymentProcessingResult>();
            result.Key.Should().Be(savePaymentResult.Key);
            result.Status.Should().Be(PaymentProcessStatus.Failed);
        }
    }
}