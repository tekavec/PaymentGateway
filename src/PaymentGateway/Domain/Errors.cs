﻿using LaYumba.Functional;

namespace PaymentGateway.Domain
{
    public static class Errors
    {
        public static UnexpectedError UnexpectedError => new UnexpectedError();
    }

    public sealed class UnexpectedError : Error
    {
        public override string Message { get; }
            = "An unexpected error has occurred";
    }
}