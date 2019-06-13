using System;

namespace PaymentGateway.Infrastructure
{
    public class Clock : IClock 
    {
        public DateTimeOffset UtcNow() => DateTimeOffset.Now;
    }
}