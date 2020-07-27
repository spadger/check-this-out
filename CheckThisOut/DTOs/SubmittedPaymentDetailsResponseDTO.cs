using System;
using JonBates.CheckThisOut.Core;

namespace JonBates.CheckThisOut.DTOs
{
    public class SubmittedPaymentDetailsResponseDTO
    {
        public SubmittedPaymentDetailsResponseDTO(SubmittedPaymentDetails paymentDetails)
        {
            var request = paymentDetails.Request;
            RequestId = request.RequestId;
            CustomerName = request.CustomerName;
            Address = request.Address;
            PostCode = request.PostCode;
            MaskedPAN = request.PAN.Substring(12, 4).PadLeft(16, '*');
            Amount = request.Amount;
            Currency = request.Currency;
            ValidFrom = request.ValidFrom;
            ValidTo = request.ValidTo;
            PostingDate = request.PostingDate;

            ProcessingStatus = paymentDetails.ProcessingStatus;
        }

        public string RequestId { get; }
        public string CustomerName { get; }
        public string Address { get; }
        public string PostCode { get; }
        public string MaskedPAN { get; }
        public decimal Amount { get; }
        public string Currency { get; }
        public DateTime ValidFrom { get; }
        public DateTime ValidTo { get; }
        public DateTime PostingDate { get; }
        public SubmittedPaymentProcessingStatus ProcessingStatus { get; }
    }
}