using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using PaymentGateway.Models;

namespace PaymentGateway.Controllers
{
    [Route("payment")]
    public class PaymentController : ControllerBase
    {
        [HttpPost]
        public async Task<IActionResult> ProcessPayment([FromBody] MakePaymentV1 model)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            return Ok();
        }
    }
}