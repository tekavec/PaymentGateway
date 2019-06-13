using System.Threading.Tasks;

namespace PaymentGateway.Infrastructure.Security
{
    public interface ITokenGenerator
    {
        Task<string> GenerateToken();
    }
}