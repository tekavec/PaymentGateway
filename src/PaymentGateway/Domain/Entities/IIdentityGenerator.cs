namespace PaymentGateway.Domain.Entities
{
    public interface IIdentityGenerator<out TKey>
    {
        TKey NewId { get; }
    }
}