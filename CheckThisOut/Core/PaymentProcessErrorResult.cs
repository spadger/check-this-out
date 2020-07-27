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
    }
}