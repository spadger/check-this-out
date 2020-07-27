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
    }
}