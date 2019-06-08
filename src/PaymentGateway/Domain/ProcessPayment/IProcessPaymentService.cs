﻿using System.Threading.Tasks;
using PaymentGateway.Models;

namespace PaymentGateway.Domain.ProcessPayment
{
    public interface IProcessPaymentService
    {
        Task<PaymentProcessingResult> Process(MakePaymentV1 model);
    }
}