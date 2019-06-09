using System;

namespace PaymentGateway.Domain.Persistence
{
    public class GuidIdentityGenerator: IIdentityGenerator<Guid>
    {
        public Guid NewId => Guid.NewGuid();
    }
}