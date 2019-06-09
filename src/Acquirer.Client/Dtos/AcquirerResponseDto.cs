using System;

namespace Acquirer.Client.Dtos
{
    public sealed class AcquirerResponseDto
    {
        public Guid PaymentId { get; set; }
        public bool IsPaymentSuccessful { get; set; }
    }
}