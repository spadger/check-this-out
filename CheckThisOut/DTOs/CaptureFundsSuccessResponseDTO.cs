using JonBates.CheckThisOut.Core;

namespace JonBates.CheckThisOut.DTOs
{
    public class CaptureFundsSuccessResponseDTO
    {
        public CaptureFundsSuccessResponseDTO(CaptureFundsBankResponse response)
        {
            ResponseId = response.ResponseId;
        }

        public string ResponseId { get; }
    }
}