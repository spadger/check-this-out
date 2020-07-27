using System;

namespace JonBates.CheckThisOut.Core.Tests
{
    public static class CaptureFundsMessageBuilder
    {
        public static CaptureFundsRequest BuildRequest(string requestId = "some-request-id", string cvv = "123")
        {
            return new CaptureFundsRequest(
               requestId,
                Guid.NewGuid(),
                "come-customer",
                "some-address",
                "post-code",
                "1010202030304040",
                123.45m,
                "GBP",
                new DateTime(2018, 1, 1),
                new DateTime(2023, 1, 31),
                cvv,
                "some-3ds-token",
                DateTime.Now
            );
        }

        public static Either<PaymentProcessErrorResult, CaptureFundsBankResponse> BuildSuccessfulResponse()
        {
            return Either<PaymentProcessErrorResult, CaptureFundsBankResponse>.Right(
                new CaptureFundsBankResponse("some-id"));
        }


        public static Either<PaymentProcessErrorResult, CaptureFundsBankResponse> BuildUnsuccessfulResponse(
            PaymentProcessErrorType errorType = PaymentProcessErrorType.TransactionAlreadyExists)
        {
            return Either<PaymentProcessErrorResult, CaptureFundsBankResponse>.Left(
                new PaymentProcessErrorResult(errorType));
        }
    }
}