using LaYumba.Functional;

namespace PaymentGateway.Validators
{
    public delegate Validation<T> Validator<T>(T t);

    public static class ValidationStrategies
    {
        public static Validator<T> HarvestErrors<T>
            (params Validator<T>[] validators)
            => t
                => validators
                    .Traverse(validate => validate(t))
                    .Map(_ => t);
    }
}