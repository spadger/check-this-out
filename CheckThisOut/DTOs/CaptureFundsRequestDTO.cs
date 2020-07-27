using System;
using System.ComponentModel.DataAnnotations;
using JonBates.CheckThisOut.Core;

namespace JonBates.CheckThisOut.DTOs
{
    //This is the only DTo to be bound, so the only class to have public setters
    public class CaptureFundsRequestDTO
    {
        [Required]
        public string RequestId { get; set; }
        [Required]
        public string CustomerName { get; set; }
        [Required]
        public string Address { get; set; }
        [Required]
        public string PostCode { get; set; }
        [Required]
        public string PAN { get; set; }
        [Required]
        public decimal Amount { get; set; }
        [Required, RegularExpression("[A-Z]{3}")]
        public string Currency { get; set; }
        [Required]
        public DateTime? ValidFrom { get; set; }
        [Required]
        public DateTime? ValidTo { get; set; }
        [Required, RegularExpression("\\d{3}")]
        public string CVV { get; set; }
        [Required]
        public string ThreeDSToken { get; set; }
        [Required]
        public DateTime? PostingDate { get; set; }

        public CaptureFundsRequest ToRequest(Guid merchantId) 
        {
            // Could have use AutoMapper here I guess, but this application is trivial and no flattening is required
            return new CaptureFundsRequest(
                RequestId,
                merchantId,
                CustomerName,
                Address,
                PostCode,
                PAN,
                Amount,
                Currency,
                ValidFrom.Value, //These are safe because we have validated they are not null
                ValidTo.Value,
                CVV,
                ThreeDSToken,
                PostingDate.Value
            );
        }

        public static CaptureFundsRequestDTO From(CaptureFundsRequest request)
        {
            return new CaptureFundsRequestDTO
            {
                RequestId = request.RequestId,
                CustomerName = request.CustomerName,
                Address = request.Address,
                PostCode = request.PostCode,
                PAN = request.PAN,
                Amount = request.Amount,
                ValidFrom = request.ValidFrom,
                ValidTo = request.ValidTo,
                CVV = request.CVV,
                ThreeDSToken = request.ThreeDSToken,
                PostingDate = request.PostingDate
            };
        }
    }
}
