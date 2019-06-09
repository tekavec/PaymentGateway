using System.Threading.Tasks;
using PaymentGateway.Domain.ProcessPayment;
using PaymentGateway.Models;

namespace PaymentGateway.Domain.Persistence
{
    public interface ISavePaymentRepository
    {
        Task<SavePaymentResult> Save(NewPayment newPayment);
    }
}