using System.Threading.Tasks;
using PaymentGateway.Domain.ProcessPayment;

namespace PaymentGateway.Domain.Persistence
{
    public interface ISavePaymentRepository
    {
        Task<SavePaymentResult> Save(ProcessedPayment processedPayment);
    }
}