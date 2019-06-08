using System.Threading.Tasks;
using PaymentGateway.Models;

namespace PaymentGateway.Domain.ProcessPayment
{
    public class ProcessPaymentService : IProcessPaymentService
    {
        private readonly IAcquirerClient acquirerClient;

        public ProcessPaymentService(IAcquirerClient acquirerClient)
        {
            this.acquirerClient = acquirerClient;
        }

        public async Task<PaymentProcessingResult> Process(MakePaymentV1 makePayment)
        {
            return await acquirerClient.ProcessPayment(makePayment);
        }
    }
}