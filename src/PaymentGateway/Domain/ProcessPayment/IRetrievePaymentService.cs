using System;
using System.Threading.Tasks;
using PaymentGateway.Models;

namespace PaymentGateway.Domain.ProcessPayment
{
    public interface IRetrievePaymentService
    {
        Task<PaymentDetails> Get(Guid id);
    }
}