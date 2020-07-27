using JonBates.CheckThisOut.Core;

namespace JonBates.CheckThisOut.DTOs
{
    public class SubmittedPaymentDetailsResponseDTO
    {
        public SubmittedPaymentDetailsResponseDTO(SubmittedPaymentDetails paymentDetails)
        {
            Request = CaptureFundsRequestDTO.From(paymentDetails.Request);
            ProcessingStatus = paymentDetails.ProcessingStatus;
        }

        public CaptureFundsRequestDTO Request { get; }
        public SubmittedPaymentProcessingStatus ProcessingStatus { get; }
    }
}