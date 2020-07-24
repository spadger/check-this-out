using System;
using System.Collections.Generic;

namespace JonBates.CheckThisOut.Core.Shared
{
    public class CaptureFundsResponse // I would have loved to have access to F# union-types here to have bound set of response types
    {
        public CaptureFundsResponse(CaptureFundsResponseType responseType, string requestId, string responseId)
        {
            ResponseType = responseType;
            RequestId = requestId;
            ResponseId = responseId;
        }

        public CaptureFundsResponse(CaptureFundsResponseType responseType, string requestId, string responseId, string failureContext, IEnumerable<ValidationFailure> validationFailures)
        {
            ResponseType = responseType;
            RequestId = requestId;
            ResponseId = responseId;
            FailureContext = failureContext;
            ValidationFailures = validationFailures;
        }

        public CaptureFundsResponseType ResponseType{ get; }
        public string RequestId { get; }
        public string ResponseId { get; }
        public string FailureContext { get; }
        public IEnumerable<ValidationFailure> ValidationFailures { get; }

        protected bool Equals(CaptureFundsResponse other)
        {
            return ResponseType == other.ResponseType && RequestId == other.RequestId && ResponseId == other.ResponseId && FailureContext == other.FailureContext && Equals(ValidationFailures, other.ValidationFailures);
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != this.GetType()) return false;
            return Equals((CaptureFundsResponse) obj);
        }

        public override int GetHashCode()
        {
            return HashCode.Combine((int) ResponseType, RequestId, ResponseId, FailureContext, ValidationFailures);
        }
    }
}