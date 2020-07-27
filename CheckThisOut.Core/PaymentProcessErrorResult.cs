using System;
using System.Collections.Generic;
using System.Linq;

namespace JonBates.CheckThisOut.Core
{
    public class PaymentProcessErrorResult
    {
        public PaymentProcessErrorResult(PaymentProcessErrorType errorType, string exceptionId = null, IEnumerable<FieldError> fieldErrors = null)
        {
            ErrorType = errorType;
            ExceptionId = exceptionId;
            FieldErrors = fieldErrors ?? Enumerable.Empty<FieldError>();
        }

        public PaymentProcessErrorType ErrorType { get; }
        public string ExceptionId { get; } // We wouldn't give actual exceptions, but some way of looking them up
        public IEnumerable<FieldError> FieldErrors { get; }

        protected bool Equals(PaymentProcessErrorResult other)
        {
            return ErrorType == other.ErrorType && ExceptionId == other.ExceptionId && Equals(FieldErrors, other.FieldErrors);
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != this.GetType()) return false;
            return Equals((PaymentProcessErrorResult) obj);
        }

        public override int GetHashCode()
        {
            return HashCode.Combine((int) ErrorType, ExceptionId, FieldErrors);
        }
    }
}