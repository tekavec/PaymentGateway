namespace PaymentGateway.Domain.Entities
{
    public abstract class Entity<TKey>
    {
        public abstract TKey Key { get; }

        public override bool Equals(object obj)
        {
            if (obj == null)
                return false;

            if (obj.GetType() != GetType())
                return false;

            var sameKey = Key.Equals(((Entity<TKey>)obj).Key);

            if (sameKey && Key.Equals(default(TKey)))
                return ReferenceEquals(this, obj);

            return sameKey;
        }

        public override int GetHashCode()
        {
            if (Key.Equals(default(TKey)))
                return base.GetHashCode();

            return GetType().GetHashCode() ^ Key.GetHashCode();
        }
    }
}