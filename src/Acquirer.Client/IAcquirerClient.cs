using System;
using System.Threading.Tasks;
using Acquirer.Client.Domain;

namespace Acquirer.Client
{
    public interface IAcquirerClient
    {
        Task<AcquirerProcessingResult> ProcessPayment(CreatePayment createPayment, Uri acquirerUri);
    }
}