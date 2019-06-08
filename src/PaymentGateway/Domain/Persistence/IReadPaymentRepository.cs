using System;
using System.Threading.Tasks;
using LaYumba.Functional;
using PaymentGateway.Models;

namespace PaymentGateway.Domain.Persistence
{
    public interface IReadPaymentRepository
    {
        Task<Option<PaymentDetails>> Read(Guid id);
    }

    public interface ISavePaymentRepository
    {
        Task<CreatePaymentDetails> Save();
    }

    public class CreatePaymentDetails
    {
    }
}