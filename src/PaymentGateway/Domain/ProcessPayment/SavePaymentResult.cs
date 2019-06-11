using System;

namespace PaymentGateway.Domain.ProcessPayment
{
    public class SavePaymentResult
    {
        public SavePaymentResult(Guid key)
        {
            Key = key;
        }

        public Guid Key { get; }
    }
}