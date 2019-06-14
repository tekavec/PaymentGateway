using System;

namespace PaymentGateway.Domain.ProcessPayment
{
    public class ProcessedPayment
    {
        public ProcessedPayment(
            string cardNumber,
            int expiryYear,
            int expiryMonth,
            decimal amount,
            string currency,
            Guid acquirerPaymentId,
            bool isPaymentSuccessful)
        {
            CardNumber = cardNumber;
            ExpiryYear = expiryYear;
            ExpiryMonth = expiryMonth;
            Amount = amount;
            Currency = currency;
            AcquirerPaymentId = acquirerPaymentId;
            IsPaymentSuccessful = isPaymentSuccessful;
        }

        public string CardNumber { get; }
        public int ExpiryYear { get; }
        public int ExpiryMonth { get; }
        public decimal Amount { get; set; }
        public string Currency { get; set; }
        public Guid AcquirerPaymentId { get; }
        public bool IsPaymentSuccessful { get; }
    }
}