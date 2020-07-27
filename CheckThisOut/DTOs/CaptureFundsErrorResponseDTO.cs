using System;
using System.Collections.Generic;
using System.Linq;
using JonBates.CheckThisOut.Core;

namespace JonBates.CheckThisOut.DTOs
{
    public class CaptureFundsErrorResponseDTO
    {
        public CaptureFundsErrorResponseDTO(PaymentProcessErrorResult error)
        {
            ExceptionId = error.ExceptionId;
            FieldErrors = error.FieldErrors ?? Enumerable.Empty<FieldError>();
        }

        public string ExceptionId { get; } // We wouldn't give actual exceptions, but some way of looking them up
        public IEnumerable<FieldError> FieldErrors { get; }

        protected bool Equals(CaptureFundsErrorResponseDTO other)
        {
            return ExceptionId == other.ExceptionId && Equals(FieldErrors, other.FieldErrors);
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != this.GetType()) return false;
            return Equals((CaptureFundsErrorResponseDTO) obj);
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(ExceptionId, FieldErrors);
        }
    }
}