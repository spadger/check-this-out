using JonBates.CheckThisOut.Core;

namespace JonBates.CheckThisOut.DTOs
{
    public class SubmittedPaymentDTO
    {
        public SubmittedPaymentDTO(SubmittedPayment payment)
        {
            Request = CaptureFundsRequestDTO.From(payment.Request);
            Response = CaptureFundsResponseDTO.From(payment.Response);
        }

        public CaptureFundsRequestDTO Request { get; }
        public CaptureFundsResponseDTO Response { get; }
    }
}