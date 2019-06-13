using System;
using System.Threading.Tasks;
using FluentAssertions;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Moq;
using PaymentGateway.Controllers;
using PaymentGateway.Domain.ProcessPayment;
using PaymentGateway.Infrastructure;
using Xunit;

namespace PaymentGateway.UnitTests.Controllers
{
    public class DiagnosticsControllerShould
    {
        private readonly Mock<IClock> clock = new Mock<IClock>(MockBehavior.Strict);

        [Fact]
        public async Task return_API_is_alive_message()
        {
            clock.Setup(a => a.UtcNow()).Returns(DateTimeOffset.UtcNow);

            var response = await new DiagnosticsController(clock.Object).Alive();

            var objectResult = response as ObjectResult;
            objectResult.StatusCode.Should().Be(StatusCodes.Status200OK);
        }
    }
}