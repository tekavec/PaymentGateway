using System;

namespace PaymentGateway.Domain.Entities
{
    public class GuidIdentityGenerator: IIdentityGenerator<Guid>
    {
        public Guid NewId => Guid.NewGuid();
    }
}