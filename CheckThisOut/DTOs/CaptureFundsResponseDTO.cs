using System.Collections.Generic;
using JonBates.CheckThisOut.Core;

namespace JonBates.CheckThisOut.DTOs
{
    public class CaptureFundsResponseDTO
    {
        public CaptureFundsResponseDTO(CaptureFundsResponseType responseType, string requestId, string responseId, string failureContext, IEnumerable<ValidationFailure> validationFailures)
        {
            ResponseType = responseType;
            RequestId = requestId;
            ResponseId = responseId;
            FailureContext = failureContext;
            ValidationFailures = validationFailures;
        }

        public CaptureFundsResponseType ResponseType { get; }
        public string RequestId { get; }
        public string ResponseId { get; }
        public string FailureContext { get; }
        public IEnumerable<ValidationFailure> ValidationFailures { get; }

        public static CaptureFundsResponseDTO From(CaptureFundsResponse response)
        {
            if (response == null)
            {
                return null;
            }

            return new CaptureFundsResponseDTO(
                response.ResponseType,
                response.RequestId,
                response.ResponseId,
                response.FailureContext,
                response.ValidationFailures
            );

        }
    }
}