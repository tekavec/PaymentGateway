﻿using System;

namespace PaymentGateway.Domain.Persistence
{
    public class PaymentEntity : Entity<Guid>
    {
        public PaymentEntity(
            Guid key,
            string cardHolder,
            string cardNumber,
            int expiryYear,
            int expiryMonth,
            decimal amount,
            string currency,
            Guid acquirerPaymentId,
            bool processedStatus)
        {
            Key = key;
            CardHolder = cardHolder;
            CardNumber = cardNumber;
            ExpiryYear = expiryYear;
            ExpiryMonth = expiryMonth;
            Amount = amount;
            Currency = currency;
            AcquirerPaymentId = acquirerPaymentId;
            ProcessedStatus = processedStatus;
        }

        public override Guid Key { get; }
        public string CardHolder { get; }
        public string CardNumber { get; }
        public int ExpiryYear { get; }
        public int ExpiryMonth { get; }
        public decimal Amount { get; set; }
        public string Currency { get; set; }
        public Guid AcquirerPaymentId { get; }
        public bool ProcessedStatus { get; }
    }
}