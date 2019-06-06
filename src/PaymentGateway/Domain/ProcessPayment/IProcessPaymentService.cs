﻿using System;
using System.Threading.Tasks;
using PaymentGateway.Models;

namespace PaymentGateway.Domain.ProcessPayment
{
    public interface IProcessPaymentService
    {
        Task<Guid> Process(MakePaymentV1 model);
    }
}