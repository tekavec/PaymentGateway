using System;
using System.Threading.Tasks;
using Acquirer.Client;
using Acquirer.Client.Domain;
using FluentAssertions;
using Moq;
using PaymentGateway.Domain.Persistence;
using PaymentGateway.Domain.ProcessPayment;
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
            var acquirerPaymentId = Guid.NewGuid();
            var savePaymentResult = new SavePaymentResult(Guid.NewGuid());
            acquirerClient.Setup(a => a.ProcessPayment(It.IsAny<CreatePayment>()))
                .ReturnsAsync(CreateSuccessfulAcquirerProcessingResult(acquirerPaymentId));
            savePaymentRepository.Setup(a => a.Save(It.IsAny<ProcessedPayment>()))
                .ReturnsAsync(savePaymentResult);

            var result = await processPaymentService.Process(GetCreatePayment());

            result.Should().BeOfType<PaymentProcessingResult>();
            result.Key.Should().Be(savePaymentResult.Key);
            result.Status.Should().Be(true);
        }

        [Fact]
        public async Task process_a_payment_and_return_failure_result()
        {
            var acquirerPaymentId = Guid.NewGuid();
            var savePaymentResult = new SavePaymentResult(Guid.NewGuid());
            acquirerClient.Setup(a => a.ProcessPayment(It.IsAny<CreatePayment>()))
                .ReturnsAsync(CreateUnsuccessfulAcquirerProcessingResult(acquirerPaymentId));
            savePaymentRepository.Setup(a => a.Save(It.IsAny<ProcessedPayment>()))
                .ReturnsAsync(savePaymentResult);

            var result = await processPaymentService.Process(GetCreatePayment());

            result.Should().BeOfType<PaymentProcessingResult>();
            result.Key.Should().Be(savePaymentResult.Key);
            result.Status.Should().Be(false);
        }
    }
}