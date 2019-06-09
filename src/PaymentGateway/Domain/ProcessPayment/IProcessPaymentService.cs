using System.Threading.Tasks;
using Acquirer.Client.Domain;

namespace PaymentGateway.Domain.ProcessPayment
{
    public interface IProcessPaymentService
    {
        Task<PaymentProcessingResult> Process(CreatePayment createPayment);
    }
}