using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using Moq;
using PaymentGateway.Controllers;
using PaymentGateway.Domain.ProcessPayment;
using PaymentGateway.Models;
using System;
using System.Threading.Tasks;
using Acquirer.Client.Domain;
using PaymentGateway.Domain.RetrievePayment;
using Xunit;
using static LaYumba.Functional.F;
using static PaymentGateway.Tests.Fakes;

namespace PaymentGateway.Tests.Controllers
{
    public class PaymentControllerShould
    {
        private readonly TestControllerContext controllerContext = new TestControllerContext();
        private readonly PaymentController paymentController;

        private readonly Mock<IProcessPaymentService> processPaymentService =
            new Mock<IProcessPaymentService>(MockBehavior.Strict);

        private readonly Mock<IRetrievePaymentService> retrievePaymentService =
            new Mock<IRetrievePaymentService>(MockBehavior.Strict);

        public PaymentControllerShould()
        {
            paymentController = new PaymentController(
                processPaymentService.Object, 
                retrievePaymentService.Object)
            {
                ControllerContext = controllerContext
            };
        }

        [Fact]
        public async Task return_result_of_processed_payment_when_payment_successfully_processed()
        {
            var transactionId = Guid.NewGuid();
            processPaymentService.Setup(a => a.Process(It.IsAny<CreatePayment>()))
                .ReturnsAsync(CreateSuccessfulPaymentProcessingResult(transactionId));

            var result = await paymentController.Post(GetValidMakePaymentV1()) as CreatedResult;

            result.Value.Should().Be(transactionId);
        }

        [Fact]
        public async Task return_bad_request_for_invalid_input_payment_data()
        {
            controllerContext.ModelState.AddModelError("Request", "Invalid");

            var result = await paymentController.Post(GetValidMakePaymentV1());

            result.Should().BeOfType<BadRequestObjectResult>();
        }

        [Fact]
        public async Task return_server_error_500_if_exception_was_thrown_during_processing()
        {
            processPaymentService.Setup(a => a.Process(It.IsAny<CreatePayment>())).ThrowsAsync(new Exception());

            var result = await paymentController.Post(GetValidMakePaymentV1()) as ObjectResult;

            result.StatusCode.Should().Be(500);
        }

        [Fact]

        public async Task return_payment_details_for_a_given_payment_identifier()
        {
            var paymentId = Guid.NewGuid();
            var paymentDetails = CreatePaymentDetails(paymentId);
            retrievePaymentService.Setup(a => a.Get(paymentId)).ReturnsAsync(paymentDetails);

            var result = await paymentController.Get(paymentId) as OkObjectResult;

            result.Value.Should().BeOfType<PaymentDetails>();
            result.Value.Should().Be(paymentDetails);
        }

        [Fact]
        public async Task return_not_found_404_if_payment_details_are_not_found()
        {
            var paymentId = Guid.NewGuid();
            retrievePaymentService.Setup(a => a.Get(paymentId)).ReturnsAsync(None);

            var result = await paymentController.Get(paymentId) as ObjectResult;

            result.StatusCode.Should().Be(404);
        }

        [Fact]
        public async Task return_server_error_500_if_exception_was_thrown_during_payment_retrieving()
        {
            var paymentId = Guid.NewGuid();
            retrievePaymentService.Setup(a => a.Get(paymentId)).ThrowsAsync(new Exception());

            var result = await paymentController.Get(paymentId) as ObjectResult;

            result.StatusCode.Should().Be(500);
        }
    }
}