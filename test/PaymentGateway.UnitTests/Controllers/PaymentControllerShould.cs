using System;
using System.Threading.Tasks;
using Acquirer.Client.Domain;
using FluentAssertions;
using LaYumba.Functional;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Moq;
using PaymentGateway.Controllers;
using PaymentGateway.Domain.ProcessPayment;
using PaymentGateway.Domain.RetrievePayment;
using PaymentGateway.Models;
using Xunit;

namespace PaymentGateway.UnitTests.Controllers
{
    public class PaymentControllerShould
    {
        private readonly TestControllerContext controllerContext = new TestControllerContext();
        private readonly PaymentController paymentController;

        private readonly Mock<IProcessPaymentService> processPaymentService =
            new Mock<IProcessPaymentService>(MockBehavior.Strict);

        private readonly Mock<IRetrievePaymentService> retrievePaymentService =
            new Mock<IRetrievePaymentService>(MockBehavior.Strict);

        private readonly Mock<ILogger<PaymentController>> logger = new Mock<ILogger<PaymentController>>();

        public PaymentControllerShould()
        {
            paymentController = new PaymentController(
                processPaymentService.Object, 
                retrievePaymentService.Object,
                logger.Object)
            {
                ControllerContext = controllerContext
            };
        }

        [Fact]
        public async Task return_result_of_processed_payment_when_payment_successfully_processed()
        {
            var paymentId = Guid.NewGuid();
            processPaymentService.Setup(a => a.Process(It.IsAny<CreatePayment>()))
                .ReturnsAsync(TestHelpers.CreateSuccessfulPaymentProcessingResult(paymentId));

            var response = await paymentController.Post(TestHelpers.GetValidMakePaymentV1()) as CreatedAtActionResult;
            response.StatusCode.Should().Be(StatusCodes.Status201Created);

            var result = response.Value as PaymentProcessingResult;
            result.Key.Should().Be(paymentId);
        }

        [Fact]
        public async Task return_bad_request_for_invalid_input_payment_data()
        {
            controllerContext.ModelState.AddModelError("Request", "Invalid");

            var result = await paymentController.Post(TestHelpers.GetValidMakePaymentV1());

            result.Should().BeOfType<BadRequestObjectResult>();
        }


        [Fact]
        public async Task return_server_error_500_if_exception_was_thrown_during_processing()
        {
            processPaymentService.Setup(a => a.Process(It.IsAny<CreatePayment>())).ThrowsAsync(new Exception());

            var result = await paymentController.Post(TestHelpers.GetValidMakePaymentV1()) as ObjectResult;

            result.StatusCode.Should().Be(StatusCodes.Status500InternalServerError);
        }

        [Fact]
        public async Task return_payment_details_for_a_given_payment_identifier()
        {
            var paymentId = Guid.NewGuid();
            var paymentDetails = TestHelpers.CreatePaymentDetails(paymentId);
            retrievePaymentService.Setup(a => a.Get(paymentId)).ReturnsAsync(paymentDetails);

            var result = await paymentController.Get(paymentId) as OkObjectResult;

            result.Value.Should().BeOfType<PaymentDetails>();
            result.Value.Should().Be(paymentDetails);
        }

        [Fact]
        public async Task return_not_found_404_if_payment_details_are_not_found()
        {
            var paymentId = Guid.NewGuid();
            retrievePaymentService.Setup(a => a.Get(paymentId)).ReturnsAsync(F.None);

            var result = await paymentController.Get(paymentId) as ObjectResult;

            result.StatusCode.Should().Be(StatusCodes.Status404NotFound);
        }

        [Fact]
        public async Task return_server_error_500_if_exception_was_thrown_during_payment_retrieving()
        {
            var paymentId = Guid.NewGuid();
            retrievePaymentService.Setup(a => a.Get(paymentId)).ThrowsAsync(new Exception());

            var result = await paymentController.Get(paymentId) as ObjectResult;

            result.StatusCode.Should().Be(StatusCodes.Status500InternalServerError);
        }
    }
}