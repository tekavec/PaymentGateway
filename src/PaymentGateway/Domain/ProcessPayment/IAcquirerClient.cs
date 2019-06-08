using System.Threading.Tasks;
using PaymentGateway.Models;

namespace PaymentGateway.Domain.ProcessPayment
{
    public interface IAcquirerClient
    {
        Task<PaymentProcessingResult> ProcessPayment(MakePaymentV1 makePayment);
    }
}