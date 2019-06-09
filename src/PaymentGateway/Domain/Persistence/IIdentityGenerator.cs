namespace PaymentGateway.Domain.Persistence
{
    public interface IIdentityGenerator<out TKey>
    {
        TKey NewId { get; }
    }
}