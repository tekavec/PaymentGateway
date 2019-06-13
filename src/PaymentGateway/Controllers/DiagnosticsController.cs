using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PaymentGateway.Infrastructure;

namespace PaymentGateway.Controllers
{
    [Route("[controller]/[action]")]
    [AllowAnonymous]
    public class DiagnosticsController : ControllerBase
    {
        private readonly IClock clock;

        public DiagnosticsController(IClock clock)
        {
            this.clock = clock;
        }

        /// <summary>
        /// Returns a reassuring information if the API is up and running
        /// </summary>
        [HttpGet]
        [Produces("text/plain")]
        public Task<IActionResult> Alive()
        {
            return Task.FromResult<IActionResult>(new ServiceIsAlive(clock));
        }

        private sealed class ServiceIsAlive : OkObjectResult
        {
            public ServiceIsAlive(IClock clock) : base($"Alive as of {clock.UtcNow()}")
            {
            }
        }
    }
}