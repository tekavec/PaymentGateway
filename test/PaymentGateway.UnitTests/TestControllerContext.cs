using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace PaymentGateway.Tests
{
    public class TestControllerContext : ControllerContext
    {
        public TestControllerContext()
        {
            HttpContext = new DefaultHttpContext();
        }
    }
}