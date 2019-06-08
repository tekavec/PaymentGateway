using System;
using System.Threading.Tasks;
using LaYumba.Functional;
using PaymentGateway.Models;

namespace PaymentGateway.Domain.RetrievePayment
{
    public interface IRetrievePaymentService
    {
        Task<Option<PaymentDetails>> Get(Guid id);
    }
}