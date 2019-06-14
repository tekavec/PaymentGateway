using System;

namespace PaymentGateway.Models
{
    public sealed class PaymentDetails
    {
        public PaymentDetails(
            Guid key,
            string cardNumber,
            int expiryYear,
            int expiryMonth,
            bool isPaymentSuccessful)
        {
            Key = key;
            CardNumber = cardNumber;
            ExpiryYear = expiryYear;
            ExpiryMonth = expiryMonth;
            IsPaymentSuccessful = isPaymentSuccessful;
        }

        public Guid Key { get; }
        public string CardNumber { get; }
        public int ExpiryYear { get; }
        public int ExpiryMonth { get; }
        public bool IsPaymentSuccessful { get; }
    }
}