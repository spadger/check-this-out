using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;

namespace JonBates.CheckThisOut.Core.BankClient
{
    public class FakeBankClient : IBankClient
    {
        public static class CVVTypes
        {
            public const string Exception = "500";
            public const string ValidationFailure = "400"; // May as well uas HTTP processingStatus codes to make them memorable
        }

        private readonly ILogger<FakeBankClient> _log;

        public FakeBankClient(ILogger<FakeBankClient> log)
        {
            _log = log;
        }

        public Task<Either<PaymentProcessErrorResult, CaptureFundsBankResponse>> CaptureFundsAsync(CaptureFundsRequest request)
        {
            _log.LogInformation(">> CaptureFundsAsync requestId={requestId}", request.RequestId);

            var result = request.CVV switch
            {
                CVVTypes.Exception => BuildException(),
                CVVTypes.ValidationFailure => BuildValidationFailure(),
                _ => BuildSuccess()
            };

            _log.LogInformation("<< CaptureFundsAsync requestId={requestId}", request.RequestId);
            return Task.FromResult(result);
        }

        private Either<PaymentProcessErrorResult, CaptureFundsBankResponse> BuildSuccess()
        {
            _log.LogInformation("Simulating a successful response from the bank");

            var result = new CaptureFundsBankResponse(Guid.NewGuid().ToString("D"));
            return Either<PaymentProcessErrorResult, CaptureFundsBankResponse>.Right(result);
        }

        private Either<PaymentProcessErrorResult, CaptureFundsBankResponse> BuildValidationFailure()
        {
            _log.LogInformation("Simulating a successful validation-failure from the bank");

            var result = new PaymentProcessErrorResult(
                PaymentProcessErrorType.AcquiringBankValidationError,
                null,
                new List<FieldError>
                {
                    new FieldError(nameof(CaptureFundsRequest.ThreeDSToken), "Token not valid"), 
                    new FieldError(nameof(CaptureFundsRequest.PAN), "Checksum Failure")
                }
            );

            return Either<PaymentProcessErrorResult, CaptureFundsBankResponse>.Left(result);
        }

        private Either<PaymentProcessErrorResult, CaptureFundsBankResponse> BuildException()
        {
            _log.LogInformation("Simulating an exception from the bank");

            var result = new PaymentProcessErrorResult(PaymentProcessErrorType.Exception, "some-exception-id");
            return Either<PaymentProcessErrorResult, CaptureFundsBankResponse>.Left(result);
        }
    }
}