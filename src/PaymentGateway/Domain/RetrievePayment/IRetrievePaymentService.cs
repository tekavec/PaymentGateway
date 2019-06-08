using System;
using System.Threading.Tasks;
using PaymentGateway.Models;

namespace PaymentGateway.Domain.RetrievePayment
{
    public interface IRetrievePaymentService
    {
        Task<PaymentDetails> Get(Guid id);
    }
}