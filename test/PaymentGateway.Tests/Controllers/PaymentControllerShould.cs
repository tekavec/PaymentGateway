using System.Threading.Tasks;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using Moq;
using PaymentGateway.Controllers;
using PaymentGateway.Models;
using Xunit;

namespace PaymentGateway.Tests.Controllers
{
    public class PaymentControllerShould
    {
        private readonly TestControllerContext controllerContext = new TestControllerContext();

        private readonly PaymentController paymentController;

        private readonly Mock<IProcessPaymentService> processPaymentService = new Mock<IProcessPaymentService>();

        public PaymentControllerShould()
        {
            paymentController = new PaymentController(processPaymentService.Object)
            {
                ControllerContext = controllerContext
            };
        }

        [Fact]
        public async Task return_ok_result_for_valid_input_payment_data()
        {
            var result = await paymentController.ProcessPayment(new MakePaymentV1());

            result.Should().BeOfType<OkResult>();
        }
        
        [Fact]
        public async Task return_bad_request_for_invalid_input_payment_data()
        {
            controllerContext.ModelState.AddModelError("Request", "Invalid");

            var result = await paymentController.ProcessPayment(new MakePaymentV1());

            result.Should().BeOfType<BadRequestObjectResult>();
        }
    }
}