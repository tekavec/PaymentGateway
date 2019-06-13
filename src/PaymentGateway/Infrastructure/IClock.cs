using System;

namespace PaymentGateway.Infrastructure
{
    public interface IClock
    {
        DateTimeOffset UtcNow();
    }
}