using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PaymentGateway.Infrastructure.Security;

namespace PaymentGateway.Controllers
{
    [Authorize]
    [Route("[controller]/[action]")]
    public class AccountController : ControllerBase
    {
        private readonly ITokenGenerator tokenGenerator;

        public AccountController(ITokenGenerator tokenGenerator)
        {
            this.tokenGenerator = tokenGenerator;
        }

        [AllowAnonymous]
        [HttpGet]
        public async Task<IActionResult> GenerateToken()
        {
            var token = await tokenGenerator.GenerateToken();
            return Ok(new { token });
        }
    }
}