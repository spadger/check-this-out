using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace JonBates.CheckThisOut.Core.BankClient
{
    public class FakeBankClient : IBankClient
    {
        public static class CVVTypes
        {
            public const string PaymentGatewayInaccessible = "502"; // May as well make these HTTP codes to be easy to remember
            public const string ValidationFailure = "400";
            public const string OtherException = "500";
        }

        public Task<CaptureFundsResponse> CaptureFundsAsync(CaptureFundsRequest request)
        {
            var result = request.CVV switch
            {
                CVVTypes.PaymentGatewayInaccessible => BuildOtherFailure(request, CaptureFundsResponseType.PaymentGatewayInaccessible),
                CVVTypes.ValidationFailure => BuildValidationFailure(request, CaptureFundsResponseType.ValidationFailure),
                CVVTypes.OtherException => BuildOtherFailure(request, CaptureFundsResponseType.OtherFailure),
                _ => BuildSuccess(request, CaptureFundsResponseType.Success)
            };

            return Task.FromResult(result);
        }

        private static CaptureFundsResponse BuildSuccess(CaptureFundsRequest request, CaptureFundsResponseType responseType)
        {
            return new CaptureFundsResponse(
                responseType,
                request.RequestId,
                Guid.NewGuid().ToString("D")
            );
        }

        private static CaptureFundsResponse BuildValidationFailure(CaptureFundsRequest request, CaptureFundsResponseType responseType)
        {
            return new CaptureFundsResponse(
                responseType,
                request.RequestId,
                Guid.NewGuid().ToString("D"),
                "",
                new List<ValidationFailure>
                {
                    new ValidationFailure(nameof(request.ThreeDSToken), "Token not valid"), 
                    new ValidationFailure(nameof(request.PAN), "Invalid BIN")
                }
            );
        }

        private static CaptureFundsResponse BuildOtherFailure(CaptureFundsRequest request, CaptureFundsResponseType responseType)
        {
            return new CaptureFundsResponse(
                responseType,
                request.RequestId,
                Guid.NewGuid().ToString("D"),
                "A simulated exception occurred",
                new List<ValidationFailure>()
            );
        }
    }
}